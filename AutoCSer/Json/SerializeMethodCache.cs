using System;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Metadata;
using AutoCSer.Extensions;
#if !NOJIT
using AutoCSer.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 序列化函数信息
    /// </summary>
    internal static class SerializeMethodCache
    {
        /// <summary>
        /// 基类转换函数信息
        /// </summary>
        internal static readonly MethodInfo BaseSerializeMethod = typeof(JsonSerializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>字段成员集合</returns>
        public static LeftArray<FieldIndex> GetFields(FieldIndex[] fields, JsonSerializeAttribute typeAttribute)
        {
            LeftArray<FieldIndex> values = new LeftArray<FieldIndex>(fields.Length);
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !field.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                {
                    JsonSerializeMemberAttribute attribute = field.GetAttribute<JsonSerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                    if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup)) values.Add(field);
                }
            }
            return values;
        }
        /// <summary>
        /// 获取属性成员集合
        /// </summary>
        /// <param name="properties">属性成员集合</param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>属性成员集合</returns>
        public static LeftArray<KeyValue<PropertyIndex, MethodInfo>> GetProperties(PropertyIndex[] properties, JsonSerializeAttribute typeAttribute)
        {
            LeftArray<KeyValue<PropertyIndex, MethodInfo>> values = new LeftArray<KeyValue<PropertyIndex, MethodInfo>>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanRead)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        JsonSerializeMemberAttribute attribute = property.GetAttribute<JsonSerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                        if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup))
                        {
                            MethodInfo method = property.Member.GetGetMethod(true);
                            if (method != null && method.GetParameters().Length == 0) values.Add(new KeyValue<PropertyIndex, MethodInfo>(property, method));
                        }
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 获取成员转换函数信息
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="isCustom">成员类型</param>
        /// <returns>成员转换函数信息</returns>
        internal static MethodInfo GetMemberMethodInfo(Type type, ref bool isCustom)
        {
            MethodInfo methodInfo = JsonSerializer.GetSerializeMethod(type);
            if (methodInfo != null) return methodInfo;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType();
                    if (elementType.IsValueType && (!elementType.IsGenericType || elementType.GetGenericTypeDefinition() != typeof(Nullable<>)))
                    {
                        return StructGenericType.Get(elementType).JsonSerializeStructArrayMethod.Method;
                    }
                    return GenericType.Get(elementType).JsonSerializeArrayMethod.Method;
                }
                return GenericType.Get(type).JsonSerializeNotSupportDelegate.Method;
            }
            if (type.IsEnum) return GenericType.Get(type).JsonSerializeEnumToStringMethod;
            if (type.isSerializeNotSupport()) return GenericType.Get(type).JsonSerializeNotSupportDelegate.Method;
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>)) return GetDictionary(type).Method;
                if (genericType == typeof(Nullable<>)) return StructGenericType.Get(type.GetGenericArguments()[0]).JsonSerializeNullableMethod.Method;
                if (genericType == typeof(KeyValuePair<,>)) return GenericType2.Get(type.GetGenericArguments()).JsonSerializeKeyValuePairMethod.Method;
            }
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            Delegate enumerableDelegate = GetIEnumerable(type);
            return enumerableDelegate != null ? enumerableDelegate.Method : GetType(type);
        }

        /// <summary>
        /// 未知类型枚举转换委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型转换委托调用函数信息</returns>
        public static MethodInfo GetType(Type type)
        {
            if (type.IsValueType) return GenericType.Get(type).JsonSerializeStructSerializeMethod;
            return GenericType.Get(type).JsonSerializeClassSerializeMethod;
        }

        /// <summary>
        /// 获取字典转换委托调用函数信息
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>字典转换委托调用函数信息</returns>
        public static Delegate GetDictionary(Type type)
        {
            Type[] types = type.GetGenericArguments();
            if (types[0] == typeof(string)) return GenericType.Get(types[1]).JsonSerializeStringDictionaryMethod;
            return GenericType2.Get(types).JsonSerializeDictionaryMethod;
        }
        /// <summary>
        /// 枚举集合转换调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, Delegate> enumerableMethods = new AutoCSer.Threading.LockDictionary<Type, Delegate>();
        /// <summary>
        /// 获取枚举集合转换委托调用函数信息
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>枚举集合转换委托调用函数信息</returns>
        public static Delegate GetIEnumerable(Type type)
        {
            Delegate method;
            if (enumerableMethods.TryGetValue(type, out method)) return method;
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType)
                {
                    Type genericType = interfaceType.GetGenericTypeDefinition();
                    if (genericType == typeof(IEnumerable<>))
                    {
                        Type[] parameters = interfaceType.GetGenericArguments();
                        Type argumentType = parameters[0];
                        parameters[0] = typeof(IList<>).MakeGenericType(argumentType);
                        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeEnumerableMethod;
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeEnumerableMethod;
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeEnumerableMethod;
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeEnumerableMethod;
                            break;
                        }
                    }
                    else if (genericType == typeof(IDictionary<,>))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { interfaceType }, null);
                        if (constructorInfo != null)
                        {
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, GenericType2.Get(interfaceType.GetGenericArguments()).KeyValuePairType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, GenericType2.Get(interfaceType.GetGenericArguments()).KeyValuePairType).JsonSerializeEnumerableMethod;
                            break;
                        }
                    }
                }
            }
            enumerableMethods.Set(type, method);
            return method;
        }
        /// <summary>
        /// 自定义转换调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> customMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 自定义枚举转换委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>自定义转换委托调用函数信息</returns>
        public static MethodInfo GetCustom(Type type)
        {
            MethodInfo method;
            if (customMethods.TryGetValue(type, out method)) return method;
            if (type.IsValueType)
            {
                foreach (AutoCSer.Metadata.AttributeMethod methodInfo in AutoCSer.Metadata.AttributeMethod.Get(type))
                {
                    if (methodInfo.Method.ReturnType == typeof(void))
                    {
                        ParameterInfo[] parameters = methodInfo.Method.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(JsonSerializer) && methodInfo.GetAttribute<JsonSerializeCustomAttribute>() != null)
                        {
                            method = methodInfo.Method;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (AutoCSer.Metadata.AttributeMethod methodInfo in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                {
                    if (methodInfo.Method.ReturnType == typeof(void))
                    {
                        ParameterInfo[] parameters = methodInfo.Method.GetParameters();
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(JsonSerializer) && parameters[1].ParameterType == type)
                        {
                            if (methodInfo.GetAttribute<JsonSerializeCustomAttribute>() != null)
                            {
                                method = methodInfo.Method;
                                break;
                            }
                        }
                    }
                }
            }
            customMethods.Set(type, method);
            return method;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            //enumMethods.Clear();
            //typeMethods.Clear();
            //objectMethods.Clear();
            //arrayMethods.Clear();
            //dictionaryMethods.Clear();
            //nullableMethods.Clear();
            //keyValuePairMethods.Clear();
            enumerableMethods.Clear();
            customMethods.Clear();
        }
        static SerializeMethodCache()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(SerializeMethodCache), 60 * 60);
        }
    }
}
