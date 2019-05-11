using System;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Metadata;
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
        internal static readonly MethodInfo BaseSerializeMethod = typeof(Serializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>字段成员集合</returns>
        public static LeftArray<FieldIndex> GetFields(FieldIndex[] fields, SerializeAttribute typeAttribute)
        {
            LeftArray<FieldIndex> values = new LeftArray<FieldIndex>(fields.Length);
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !field.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                {
                    SerializeMemberAttribute attribute = field.GetAttribute<SerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
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
        public static LeftArray<KeyValue<PropertyIndex, MethodInfo>> GetProperties(PropertyIndex[] properties, SerializeAttribute typeAttribute)
        {
            LeftArray<KeyValue<PropertyIndex, MethodInfo>> values = new LeftArray<KeyValue<PropertyIndex, MethodInfo>>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanRead)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        SerializeMemberAttribute attribute = property.GetAttribute<SerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
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
            MethodInfo methodInfo = Serializer.GetSerializeMethod(type);
            if (methodInfo != null) return methodInfo;
            //if (type.IsArray) return GetArray(type.GetElementType());
            if (type.IsArray) return GenericType.Get(type.GetElementType()).JsonSerializeArrayMethod;
            //if (type.IsEnum) return GetEnum(type);
            if (type.IsEnum) return GenericType.Get(type).JsonSerializeEnumToStringMethod;
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>)) return GetDictionary(type);
                //if (genericType == typeof(Nullable<>)) return GetNullable(type);
                if (genericType == typeof(Nullable<>)) return StructGenericType.Get(type.GetGenericArguments()[0]).JsonSerializeNullableMethod;
                //if (genericType == typeof(KeyValuePair<,>)) return GetKeyValuePair(type);
                if (genericType == typeof(KeyValuePair<,>)) return GenericType2.Get(type.GetGenericArguments()).JsonSerializeKeyValuePairMethod;
            }
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            return GetIEnumerable(type) ?? GetType(type);
        }
#if !NOJIT
        /// <summary>
        /// 名称数据信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<string, Pointer> namePools = new AutoCSer.Threading.LockDictionary<string, Pointer>();
        /// <summary>
        /// 获取名称数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public unsafe static char* GetNamePool(string name)
        {
            Pointer pointer;
            if (namePools.TryGetValue(name, out pointer)) return pointer.Char;
            char* value = NamePool.Get(name, 2, 2);
            *(int*)value = ',' + ('"' << 16);
            *(int*)(value + (2 + name.Length)) = '"' + (':' << 16);
            namePools.Set(name, new Pointer { Data = value });
            return value;
        }
#endif

        ///// <summary>
        ///// 枚举转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> enumMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumToStringMethod = typeof(Serializer).GetMethod("EnumToString", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取枚举转换委托调用函数信息
        ///// </summary>
        ///// <param name="type">数组类型</param>
        ///// <returns>枚举转换委托调用函数信息</returns>
        //public static MethodInfo GetEnum(Type type)
        //{
        //    MethodInfo method;
        //    if (enumMethods.TryGetValue(type, out method)) return method;
        //    enumMethods.Set(type, method = enumToStringMethod.MakeGenericMethod(type));
        //    return method;
        //}
        ///// <summary>
        ///// 未知类型转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> typeMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo classSerializeMethod = typeof(Serializer).GetMethod("classSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo structSerializeMethod = typeof(Serializer).GetMethod("structSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 未知类型枚举转换委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型转换委托调用函数信息</returns>
        public static MethodInfo GetType(Type type)
        {
            if (type.IsValueType) return GenericType.Get(type).JsonSerializeStructSerializeMethod;
            return GenericType.Get(type).JsonSerializeClassSerializeMethod;
            //MethodInfo method;
            //if (typeMethods.TryGetValue(type, out method)) return method;
            //typeMethods.Set(type, method = type.IsValueType ? structSerializeMethod.MakeGenericMethod(type) : classSerializeMethod.MakeGenericMethod(type));
            //return method;
        }

        ///// <summary>
        ///// object转换调用委托信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, Action<Serializer, object>> objectMethods = new AutoCSer.Threading.LockDictionary<Type, Action<Serializer, object>>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo serializeObjectMethod = typeof(Serializer).GetMethod("serializeObject", BindingFlags.Static | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取object转换调用委托信息
        ///// </summary>
        ///// <param name="type">真实类型</param>
        ///// <returns>object转换调用委托信息</returns>
        //public static Action<Serializer, object> GetObject(Type type)
        //{
        //    Action<Serializer, object> method;
        //    if (objectMethods.TryGetValue(type, out method)) return method;
        //    method = (Action<Serializer, object>)Delegate.CreateDelegate(typeof(Action<Serializer, object>), serializeObjectMethod.MakeGenericMethod(type));
        //    objectMethods.Set(type, method);
        //    return method;
        //}
        ///// <summary>
        ///// 数组转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> arrayMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo arrayMethod = typeof(Serializer).GetMethod("array", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取数组转换委托调用函数信息
        ///// </summary>
        ///// <param name="type">数组类型</param>
        ///// <returns>数组转换委托调用函数信息</returns>
        //public static MethodInfo GetArray(Type type)
        //{
        //    MethodInfo method;
        //    if (arrayMethods.TryGetValue(type, out method)) return method;
        //    arrayMethods.Set(type, method = arrayMethod.MakeGenericMethod(type));
        //    return method;
        //}
        ///// <summary>
        ///// 字典转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> dictionaryMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo dictionaryMethod = typeof(Serializer).GetMethod("dictionary", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字符串字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo stringDictionaryMethod = typeof(Serializer).GetMethod("stringDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 获取字典转换委托调用函数信息
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>字典转换委托调用函数信息</returns>
        public static MethodInfo GetDictionary(Type type)
        {
            Type[] types = type.GetGenericArguments();
            if (types[0] == typeof(string)) return GenericType.Get(types[1]).JsonSerializeStringDictionaryMethod;
            return GenericType2.Get(types).JsonSerializeDictionaryMethod;

            //MethodInfo method;
            //if (dictionaryMethods.TryGetValue(type, out method)) return method;
            //Type[] types = type.GetGenericArguments();
            //if (types[0] == typeof(string)) method = stringDictionaryMethod.MakeGenericMethod(types[1]);
            //else method = dictionaryMethod.MakeGenericMethod(types);
            //dictionaryMethods.Set(type, method);
            //return method;
        }
        ///// <summary>
        ///// 可空类型转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> nullableMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 值类型对象转换函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableSerializeMethod = typeof(Serializer).GetMethod("nullableSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取可空类型转换委托调用函数信息
        ///// </summary>
        ///// <param name="type">枚举类型</param>
        ///// <returns>可空类型转换委托调用函数信息</returns>
        //public static MethodInfo GetNullable(Type type)
        //{
        //    MethodInfo method;
        //    if (nullableMethods.TryGetValue(type, out method)) return method;
        //    nullableMethods.Set(type, method = nullableSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
        //    return method;
        //}
        ///// <summary>
        ///// 键值对转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> keyValuePairMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo keyValuePairSerializeMethod = typeof(Serializer).GetMethod("keyValuePairSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取键值对转换委托调用函数信息
        ///// </summary>
        ///// <param name="type">枚举类型</param>
        ///// <returns>键值对转换委托调用函数信息</returns>
        //public static MethodInfo GetKeyValuePair(Type type)
        //{
        //    MethodInfo method;
        //    if (keyValuePairMethods.TryGetValue(type, out method)) return method;
        //    keyValuePairMethods.Set(type, method = keyValuePairSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
        //    return method;
        //}
        /// <summary>
        /// 枚举集合转换调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> enumerableMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo structEnumerableMethod = typeof(Serializer).GetMethod("structEnumerable", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumerableMethod = typeof(Serializer).GetMethod("enumerable", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 获取枚举集合转换委托调用函数信息
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>枚举集合转换委托调用函数信息</returns>
        public static MethodInfo GetIEnumerable(Type type)
        {
            MethodInfo method;
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
                            //method = (type.IsValueType ? structEnumerableMethod : enumerableMethod).MakeGenericMethod(type, argumentType);
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeEnumerableMethod;
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = (type.IsValueType ? structEnumerableMethod : enumerableMethod).MakeGenericMethod(type, argumentType);
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeEnumerableMethod;
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = (type.IsValueType ? structEnumerableMethod : enumerableMethod).MakeGenericMethod(type, argumentType);
                            if (type.IsValueType) method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeStructEnumerableMethod;
                            else method = EnumerableGenericType2.Get(type, argumentType).JsonSerializeEnumerableMethod;
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = (type.IsValueType ? structEnumerableMethod : enumerableMethod).MakeGenericMethod(type, argumentType);
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
                            //method = (type.IsValueType ? structEnumerableMethod : enumerableMethod).MakeGenericMethod(type, typeof(KeyValuePair<,>).MakeGenericType(interfaceType.GetGenericArguments()));
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
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Serializer) && methodInfo.GetAttribute<SerializeCustomAttribute>() != null)
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
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Serializer) && parameters[1].ParameterType == type)
                        {
                            if (methodInfo.GetAttribute<SerializeCustomAttribute>() != null)
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
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
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
            Pub.ClearCaches += clearCache;
        }
    }
}
