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
        public static LeftArray<KeyValue<FieldIndex, MemberAttribute>> GetFields(FieldIndex[] fields, SerializeAttribute typeAttribute)
        {
            LeftArray<KeyValue<FieldIndex, MemberAttribute>> values = new LeftArray<KeyValue<FieldIndex, MemberAttribute>>(fields.Length);
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !field.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                {
                    MemberAttribute attribute = field.GetAttribute<MemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                    if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup))
                    {
                        values.Add(new KeyValue<FieldIndex, MemberAttribute>(field, attribute));
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
        public static LeftArray<PropertyMethod> GetProperties(PropertyIndex[] properties, SerializeAttribute typeAttribute)
        {
            LeftArray<PropertyMethod> values = new LeftArray<PropertyMethod>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanRead)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        MemberAttribute attribute = property.GetAttribute<MemberAttribute>(typeAttribute.IsBaseTypeAttribute);
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
            MethodInfo method;
            if (typeMethods.TryGetValue(type, out method)) return method;
            if (type.IsValueType)
            {
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    //if (genericType == typeof(Nullable<>)) method = nullableSerializeMethod.MakeGenericMethod(type.GetGenericArguments());
                    if (genericType == typeof(Nullable<>)) method = StructGenericType.Get(type.GetGenericArguments()[0]).XmlSerializeNullableMethod;
                }
                //if (method == null) method = structSerializeMethod.MakeGenericMethod(type);
                if (method == null) method = GenericType.Get(type).XmlSerializeStructMethod;
            }
            //else method = classSerializeMethod.MakeGenericMethod(type);
            else method = method = GenericType.Get(type).XmlSerializeClassMethod;
            typeMethods.Set(type, method);
            return method;
        }
        ///// <summary>
        ///// 枚举转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> enumMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumToStringMethod = typeof(Serializer).GetMethod("enumToString", BindingFlags.Instance | BindingFlags.NonPublic);
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
            if (type.IsArray) return GetArray(type.GetElementType());
            //if (type.IsEnum) return GetEnum(type);
            if (type.IsEnum) return GenericType.Get(type).XmlSerializeEnumToStringMethod;
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            return GetIEnumerable(type) ?? GetType(type);
        }
        /// <summary>
        /// 是否输出字符串函数信息
        /// </summary>
        private static readonly MethodInfo isOutputSubStringMethod = ((Func<SubString, bool>)GenericType.XmlSerializer.isOutputSubString).Method;// typeof(Serializer).GetMethod("isOutputSubString", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 是否输出字符串函数信息
        /// </summary>
        private static readonly MethodInfo isOutputStringMethod = ((Func<string, bool>)GenericType.XmlSerializer.isOutputString).Method;// typeof(Serializer).GetMethod("isOutputString", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 是否输出对象函数信息
        /// </summary>
        private static readonly MethodInfo isOutputMethod = ((Func<object, bool>)GenericType.XmlSerializer.isOutput).Method;// typeof(Serializer).GetMethod("isOutput", BindingFlags.Instance | BindingFlags.NonPublic);
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
        ///// <summary>
        ///// 是否输出可空对象函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> isOutputNullableMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 是否输出可空对象函数信息
        ///// </summary>
        //private static readonly MethodInfo isOutputNullableMethod = typeof(Serializer).GetMethod("isOutputNullable", BindingFlags.Instance | BindingFlags.NonPublic);
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
                //StructGenericType
                //MethodInfo method;
                //if (isOutputNullableMethods.TryGetValue(type, out method)) return method;
                //isOutputNullableMethods.Set(type, method = isOutputNullableMethod.MakeGenericMethod(type.GetGenericArguments()));
                //return method;
            }
            return null;
        }
        /// <summary>
        /// 基类转换函数信息
        /// </summary>
        internal static readonly MethodInfo BaseSerializeMethod = typeof(Serializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> arrayMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo structArrayMethod = typeof(Serializer).GetMethod("structArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo arrayMethod = typeof(Serializer).GetMethod("array", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 获取数组转换委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>数组转换委托调用函数信息</returns>
        public static MethodInfo GetArray(Type type)
        {
            if (type.IsValueType) return GenericType.Get(type).XmlSerializeStructArrayMethod;
            return GenericType.Get(type).XmlSerializeArrayMethod;
            //MethodInfo method;
            //if (arrayMethods.TryGetValue(type, out method)) return method;
            //arrayMethods.Set(type, method = (type.IsValueType ? structArrayMethod : arrayMethod).MakeGenericMethod(type));
            //return method;
        }
        ///// <summary>
        ///// 可空类型转换调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> nullableMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 可空类型转换函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableSerializeMethod = typeof(Serializer).GetMethod("nullableSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取可空类型转换委托调用函数信息
        ///// </summary>
        ///// <param name="type">数组类型</param>
        ///// <returns>可空类型转换委托调用函数信息</returns>
        //public static MethodInfo GetNullable(Type type)
        //{
        //    MethodInfo method;
        //    if (nullableMethods.TryGetValue(type, out method)) return method;
        //    nullableMethods.Set(type, method = nullableSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
        //    return method;
        //}
        /// <summary>
        /// 枚举集合转换调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> enumerableMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo structStructEnumerableMethod = typeof(Serializer).GetMethod("structStructEnumerable", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo structClassEnumerableMethod = typeof(Serializer).GetMethod("structClassEnumerable", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo classStructEnumerableMethod = typeof(Serializer).GetMethod("classStructEnumerable", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典转换函数信息
        ///// </summary>
        //private static readonly MethodInfo classClassEnumerableMethod = typeof(Serializer).GetMethod("classClassEnumerable", BindingFlags.Instance | BindingFlags.NonPublic);
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
                            //method = (type.IsValueType ? (argumentType.IsValueType ? structStructEnumerableMethod : structClassEnumerableMethod) : (argumentType.IsValueType ? classStructEnumerableMethod : classClassEnumerableMethod)).MakeGenericMethod(type, argumentType);
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, argumentType);
                            method = type.IsValueType ? (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeStructClassEnumerableMethod) : (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassClassEnumerableMethod);
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = (type.IsValueType ? (argumentType.IsValueType ? structStructEnumerableMethod : structClassEnumerableMethod) : (argumentType.IsValueType ? classStructEnumerableMethod : classClassEnumerableMethod)).MakeGenericMethod(type, argumentType);
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, argumentType);
                            method = type.IsValueType ? (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeStructClassEnumerableMethod) : (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassClassEnumerableMethod);
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = (type.IsValueType ? (argumentType.IsValueType ? structStructEnumerableMethod : structClassEnumerableMethod) : (argumentType.IsValueType ? classStructEnumerableMethod : classClassEnumerableMethod)).MakeGenericMethod(type, argumentType);
                            EnumerableGenericType2 EnumerableGenericType2 = EnumerableGenericType2.Get(type, argumentType);
                            method = type.IsValueType ? (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeStructStructEnumerableMethod : EnumerableGenericType2.XmlSerializeStructClassEnumerableMethod) : (argumentType.IsValueType ? EnumerableGenericType2.XmlSerializeClassStructEnumerableMethod : EnumerableGenericType2.XmlSerializeClassClassEnumerableMethod);
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = (type.IsValueType ? (argumentType.IsValueType ? structStructEnumerableMethod : structClassEnumerableMethod) : (argumentType.IsValueType ? classStructEnumerableMethod : classClassEnumerableMethod)).MakeGenericMethod(type, argumentType);
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
                            //method = (type.IsValueType ? structStructEnumerableMethod : classStructEnumerableMethod).MakeGenericMethod(type, typeof(KeyValuePair<,>).MakeGenericType(interfaceType.GetGenericArguments()));
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
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Serializer) && methodInfo.GetAttribute<CustomAttribute>() != null)
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

#if !NOJIT
        /// <summary>
        /// 名称数据信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<string, Pointer> nameStartPools = new AutoCSer.Threading.LockDictionary<string, Pointer>();
        /// <summary>
        /// 获取名称数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public unsafe static char* GetNameStartPool(string name)
        {
            Pointer pointer;
            if (nameStartPools.TryGetValue(name, out pointer)) return pointer.Char;
            char* value = Emit.NamePool.Get(name, 1, 1);
            *value = '<';
            *(value + (1 + name.Length)) = '>';
            nameStartPools.Set(name, new Pointer { Data = value });
            return value;
        }
        /// <summary>
        /// 名称数据信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<string, Pointer> nameEndPools = new AutoCSer.Threading.LockDictionary<string, Pointer>();
        /// <summary>
        /// 获取名称数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public unsafe static char* GetNameEndPool(string name)
        {
            Pointer pointer;
            if (nameEndPools.TryGetValue(name, out pointer)) return pointer.Char;
            char* value = Emit.NamePool.Get(name, 2, 1);
            *(int*)value = '<' + ('/' << 16);
            *(value + (2 + name.Length)) = '>';
            nameEndPools.Set(name, new Pointer { Data = value });
            return value;
        }
#endif

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
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
            Pub.ClearCaches += clearCache;
        }
    }
}
