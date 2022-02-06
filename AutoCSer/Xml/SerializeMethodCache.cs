using System;
using AutoCSer.Metadata;
using System.Reflection;
using System.Collections.Generic;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML 序列化函数信息
    /// </summary>
    internal static class SerializeMethodCache 
    {
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>字段成员集合</returns>
        public static LeftArray<KeyValue<FieldIndex, XmlSerializeMemberAttribute>> GetFields(FieldIndex[] fields, XmlSerializeAttribute typeAttribute)
        {
            LeftArray<KeyValue<FieldIndex, XmlSerializeMemberAttribute>> values = new LeftArray<KeyValue<FieldIndex, XmlSerializeMemberAttribute>>(fields.Length);
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !field.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                {
                    XmlSerializeMemberAttribute attribute = field.GetAttribute<XmlSerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                    if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup))
                    {
                        values.Add(new KeyValue<FieldIndex, XmlSerializeMemberAttribute>(field, attribute));
                    }
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
        public static LeftArray<PropertyMethod> GetProperties(PropertyIndex[] properties, XmlSerializeAttribute typeAttribute)
        {
            LeftArray<PropertyMethod> values = new LeftArray<PropertyMethod>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanRead)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        XmlSerializeMemberAttribute attribute = property.GetAttribute<XmlSerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                        if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup))
                        {
                            MethodInfo method = property.Member.GetGetMethod(true);
                            if (method != null && method.GetParameters().Length == 0) values.Add(new PropertyMethod { Property = property, Method = method, Attribute = attribute });
                        }
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 未知类型转换调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> typeMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 未知类型枚举转换委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型转换委托调用函数信息</returns>
        public static MethodInfo GetType(Type type)
        {
            MethodInfo method;
            if (typeMethods.TryGetValue(type, out method)) return method;
            if (type.IsValueType)
            {
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType == typeof(Nullable<>)) method = StructGenericType.Get(type.GetGenericArguments()[0]).XmlSerializeNullableMethod.Method;
                }
                if (method == null) method = GenericType.Get(type).XmlSerializeStructMethod;
            }
            else method = method = GenericType.Get(type).XmlSerializeClassMethod;
            typeMethods.Set(type, method);
            return method;
        }
        /// <summary>
        /// 获取成员转换函数信息
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="isCustom">成员类型</param>
        /// <returns>成员转换函数信息</returns>
        internal static MethodInfo GetMemberMethodInfo(Type type, ref bool isCustom)
        {
            MethodInfo methodInfo = XmlSerializer.GetSerializeMethod(type);
            if (methodInfo != null) return methodInfo;
            if (type.IsArray) return GetArray(type.GetElementType()).Method;
            //if (type.IsEnum) return GetEnum(type);
            if (type.IsEnum) return GenericType.Get(type).XmlSerializeEnumToStringMethod;
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            Delegate enumerableDelegate = GetIEnumerable(type);
            return enumerableDelegate != null ? GetIEnumerable(type).Method : GetType(type);
        }
        /// <summary>
        /// 是否输出字符串函数信息
        /// </summary>
        private static readonly MethodInfo isOutputSubStringMethod = ((Func<XmlSerializer, SubString, bool>)XmlSerializer.IsOutputSubString).Method;
        /// <summary>
        /// 是否输出字符串函数信息
        /// </summary>
        private static readonly MethodInfo isOutputStringMethod = ((Func<XmlSerializer, string, bool>)XmlSerializer.IsOutputString).Method;
        /// <summary>
        /// 是否输出对象函数信息
        /// </summary>
        private static readonly MethodInfo isOutputMethod = ((Func<XmlSerializer, object, bool>)XmlSerializer.IsOutput).Method;
        /// <summary>
        /// 获取是否输出对象函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static MethodInfo GetIsOutputMethod(Type type)
        {
            if (type.IsValueType) return type == typeof(SubString) ? isOutputSubStringMethod : GetIsOutputNullable(type);
            return type == typeof(string) ? isOutputStringMethod : isOutputMethod;
        }
        /// <summary>
        /// 获取是否输出可空对象函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>数组转换委托调用函数信息</returns>
        public static MethodInfo GetIsOutputNullable(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return StructGenericType.Get(type.GetGenericArguments()[0]).XmlSerializeIsOutputNullableMethod;
            }
            return null;
        }
        /// <summary>
        /// 基类转换函数信息
        /// </summary>
        internal static readonly MethodInfo BaseSerializeMethod = typeof(XmlSerializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取数组转换委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>数组转换委托调用函数信息</returns>
        public static Delegate GetArray(Type type)
        {
            if (type.IsValueType) return GenericType.Get(type).XmlSerializeStructArrayMethod;
            return GenericType.Get(type).XmlSerializeArrayMethod;
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
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, argumentType);
                            method = type.IsValueType ? (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeStructClassEnumerableMethod) : (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassClassEnumerableMethod);
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, argumentType);
                            method = type.IsValueType ? (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeStructClassEnumerableMethod) : (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassClassEnumerableMethod);
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, argumentType);
                            method = type.IsValueType ? (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeStructClassEnumerableMethod) : (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassClassEnumerableMethod);
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, argumentType);
                            method = type.IsValueType ? (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeStructClassEnumerableMethod) : (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassClassEnumerableMethod);
                            break;
                        }
                    }
                    else if (genericType == typeof(IDictionary<,>))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { interfaceType }, null);
                        if (constructorInfo != null)
                        {
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, typeof(KeyValuePair<,>).MakeGenericType(interfaceType.GetGenericArguments()));
                            method = type.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod;
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
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(XmlSerializer) && methodInfo.GetAttribute<CustomAttribute>() != null)
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
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(XmlSerializer) && parameters[1].ParameterType == type)
                        {
                            if (methodInfo.GetAttribute<CustomAttribute>() != null)
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
            typeMethods.Clear();
            //enumMethods.Clear();
            //isOutputNullableMethods.Clear();
            //arrayMethods.Clear();
            //nullableMethods.Clear();
            enumerableMethods.Clear();
            customMethods.Clear();
        }
        static SerializeMethodCache()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(SerializeMethodCache), 60 * 60);
        }
    }
}
