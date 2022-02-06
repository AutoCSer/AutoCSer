using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extensions;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析函数信息
    /// </summary>
    internal static class DeSerializeMethodCache
    {
        /// <summary>
        /// 基类转换函数信息
        /// </summary>
        internal static readonly MethodInfo BaseDeSerializeMethod = typeof(JsonDeSerializer).GetMethod("baseDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <param name="defaultMember">默认解析字段</param>
        /// <returns>字段成员集合</returns>
        public static LeftArray<FieldIndex> GetFields(FieldIndex[] fields, JsonDeSerializeAttribute typeAttribute, ref FieldIndex defaultMember)
        {
            LeftArray<FieldIndex> values = new LeftArray<FieldIndex>(fields.Length);
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !field.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                {
                    JsonDeSerializeMemberAttribute attribute = field.GetAttribute<JsonDeSerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                    if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup))
                    {
                        if (attribute != null && attribute.IsDefault) defaultMember = field;
                        values.Add(field);
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
        public static LeftArray<KeyValue<PropertyIndex, MethodInfo>> GetProperties(PropertyIndex[] properties, JsonDeSerializeAttribute typeAttribute)
        {
            LeftArray<KeyValue<PropertyIndex, MethodInfo>> values = new LeftArray<KeyValue<PropertyIndex, MethodInfo>>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanWrite)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        JsonDeSerializeMemberAttribute attribute = property.GetAttribute<JsonDeSerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                        if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup))
                        {
                            MethodInfo method = property.Member.GetSetMethod(true);
                            if (method != null && method.GetParameters().Length == 1)
                            {
                                values.Add(new KeyValue<PropertyIndex, MethodInfo>(property, method));
                            }
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
        /// <param name="isCustom"></param>
        /// <returns>成员转换函数信息</returns>
        internal static MethodInfo GetMemberMethodInfo(Type type, ref bool isCustom)
        {
            MethodInfo methodInfo = JsonDeSerializer.GetDeSerializeMethod(type);
            if (methodInfo != null) return methodInfo;
            if (type.IsArray) return GenericType.Get(type.GetElementType()).JsonDeSerializeArrayMethod.Method;
            if (type.IsEnum)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false)) return (EnumGenericType.Get(type).JsonDeSerializeEnumFlagsDelegate).Method;
                return (EnumGenericType.Get(type).JsonDeSerializeEnumDelegate).Method;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>)) return GenericType2.Get(type.GetGenericArguments()).JsonDeSerializeDictionaryMethod.Method;
                if (genericType == typeof(Nullable<>)) return GetNullable(type).Method;
                if (genericType == typeof(KeyValuePair<,>)) return GenericType2.Get(type.GetGenericArguments()).JsonDeSerializeKeyValuePairMethod.Method;
            }
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            if ((methodInfo = GetIEnumerableConstructor(type)) != null) return methodInfo;
            if (type.IsValueType) return StructGenericType.Get(type).JsonDeSerializeStructMethod;
            return GenericType.Get(type).JsonDeSerializeTypeMethod;
        }
#if !NOJIT
        /// <summary>
        /// 创建解析委托函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns>解析委托函数</returns>
        public static DynamicMethod CreateDynamicMethod(Type type, FieldInfo field)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("JsonDeSerializer" + field.Name, null, new Type[] { typeof(JsonDeSerializer), type.MakeByRefType() }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            bool isCustom = false;
            MethodInfo method = GetMemberMethodInfo(field.FieldType, ref isCustom);
            if (isCustom)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Ldflda, field);
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Ldflda, field);
            }
            generator.call(method);
            generator.Emit(OpCodes.Ret);
            return dynamicMethod;
        }
        /// <summary>
        /// 创建解析委托函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <param name="propertyMethod"></param>
        /// <returns>解析委托函数</returns>
        public static DynamicMethod CreateDynamicMethod(Type type, PropertyInfo property, MethodInfo propertyMethod)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("JsonDeSerializer" + property.Name, null, new Type[] { typeof(JsonDeSerializer), type.MakeByRefType() }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            Type memberType = property.PropertyType;
            LocalBuilder loadMember = generator.DeclareLocal(memberType);
            generator.initobjShort(memberType, loadMember);
            bool isCustom = false;
            MethodInfo method = GetMemberMethodInfo(memberType, ref isCustom);
            if (isCustom)
            {
                generator.Emit(OpCodes.Ldloca_S, loadMember);
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldloca_S, loadMember);
            }
            generator.call(method);

            generator.Emit(OpCodes.Ldarg_1);
            if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldloc_0);
            generator.call(propertyMethod);
            generator.Emit(OpCodes.Ret);
            return dynamicMethod;
        }
#endif
        /// <summary>
        /// 获取可空类型解析调用函数信息
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>可空类型解析调用函数信息</returns>
        public static Delegate GetNullable(Type type)
        {
            Type parameterType = type.GetGenericArguments()[0];
            if (parameterType.IsEnum) return StructGenericType.Get(parameterType).JsonDeSerializeNullableEnumMethod;
            return StructGenericType.Get(parameterType).JsonDeSerializeNullableMethod;
        }
        /// <summary>
        /// 自定义解析调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> customMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 自定义解析委托调用函数信息
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>自定义解析委托调用函数信息</returns>
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
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(JsonDeSerializer) && methodInfo.GetAttribute<JsonDeSerializeCustomAttribute>() != null)
                        {
                            method = methodInfo.Method;
                            break;
                        }
                    }
                }
            }
            else
            {
                Type refType = type.MakeByRefType();
                foreach (AutoCSer.Metadata.AttributeMethod methodInfo in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                {
                    if (methodInfo.Method.ReturnType == typeof(void))
                    {
                        ParameterInfo[] parameters = methodInfo.Method.GetParameters();
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(JsonDeSerializer) && parameters[1].ParameterType == refType)
                        {
                            if (methodInfo.GetAttribute<JsonDeSerializeCustomAttribute>() != null)
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
        /// 获取枚举构造调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> enumerableConstructorMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 获取枚举构造调用函数信息
        /// </summary>
        /// <param name="type">集合类型</param>
        /// <returns>枚举构造调用函数信息</returns>
        public static MethodInfo GetIEnumerableConstructor(Type type)
        {
            MethodInfo method;
            if (enumerableConstructorMethods.TryGetValue(type, out method)) return method;
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
                            method = GenericType2.Get(type, argumentType).JsonDeSerializeListConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            method = GenericType2.Get(type, argumentType).JsonDeSerializeCollectionConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            method = GenericType2.Get(type, argumentType).JsonDeSerializeEnumerableConstructorMethod;
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            method = GenericType2.Get(type, argumentType).JsonDeSerializeArrayConstructorMethod;
                            break;
                        }
                    }
                    else if (genericType == typeof(IDictionary<,>))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { interfaceType }, null);
                        if (constructorInfo != null)
                        {
                            Type[] parameters = interfaceType.GetGenericArguments();
                            method = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).JsonDeSerializeDictionaryConstructorMethod;
                            break;
                        }
                    }
                }
            }
            enumerableConstructorMethods.Set(type, method);
            return method;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            customMethods.Clear();
            enumerableConstructorMethods.Clear();
        }
        static DeSerializeMethodCache()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(DeSerializeMethodCache), 60 * 60);
        }
    }
}
