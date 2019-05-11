using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析函数信息
    /// </summary>
    internal static class ParseMethodCache
    {
        /// <summary>
        /// 基类转换函数信息
        /// </summary>
        internal static readonly MethodInfo BaseParseMethod = typeof(Parser).GetMethod("baseParse", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <param name="defaultMember">默认解析字段</param>
        /// <returns>字段成员集合</returns>
        public static LeftArray<FieldIndex> GetFields(FieldIndex[] fields, ParseAttribute typeAttribute, ref FieldIndex defaultMember)
        {
            LeftArray<FieldIndex> values = new LeftArray<FieldIndex>(fields.Length);
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !field.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                {
                    ParseMemberAttribute attribute = field.GetAttribute<ParseMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
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
        public static LeftArray<KeyValue<PropertyIndex, MethodInfo>> GetProperties(PropertyIndex[] properties, ParseAttribute typeAttribute)
        {
            LeftArray<KeyValue<PropertyIndex, MethodInfo>> values = new LeftArray<KeyValue<PropertyIndex, MethodInfo>>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanWrite)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        ParseMemberAttribute attribute = property.GetAttribute<ParseMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
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
            MethodInfo methodInfo = Parser.GetParseMethod(type);
            if (methodInfo != null) return methodInfo;
            //if (type.IsArray) return GetArray(type.GetElementType());
            if (type.IsArray) return GenericType.Get(type.GetElementType()).JsonParseArrayMethod;
            if (type.IsEnum) return GetEnum(type);
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                //if (genericType == typeof(Dictionary<,>)) return GetDictionary(type);
                if (genericType == typeof(Dictionary<,>)) return GenericType2.Get(type.GetGenericArguments()).JsonParseDictionaryMethod;
                if (genericType == typeof(Nullable<>)) return GetNullable(type);
                //if (genericType == typeof(KeyValuePair<,>)) return GetKeyValuePair(type);
                if (genericType == typeof(KeyValuePair<,>)) return GenericType2.Get(type.GetGenericArguments()).JsonParseKeyValuePairMethod;
            }
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            //if (type.IsAbstract || type.IsInterface) return typeParser.GetNoConstructorParser(type);
            if ((methodInfo = GetIEnumerableConstructor(type)) != null) return methodInfo;
            //if (type.IsValueType) return GetValueType(type);
            //return GetType(type);
            if (type.IsValueType) return StructGenericType.Get(type).JsonParseStructMethod;
            return GenericType.Get(type).JsonParseTypeMethod;
        }
#if !NOJIT
        /// <summary>
        /// 是否匹配默认顺序名称 函数信息
        /// </summary>
        internal static readonly MethodInfo ParserIsNameMethod = typeof(Parser).GetMethod("IsName", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(byte*), typeof(int).MakeByRefType() }, null);
        /// <summary>
        /// 解析状态 字段信息
        /// </summary>
        internal static readonly FieldInfo ParseStateField = typeof(Parser).GetField("ParseState", BindingFlags.Instance | BindingFlags.NonPublic);
        
        /// <summary>
        /// 创建解析委托函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns>解析委托函数</returns>
        public static DynamicMethod CreateDynamicMethod(Type type, FieldInfo field)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("JsonParser" + field.Name, null, new Type[] { typeof(Parser), type.MakeByRefType() }, type, true);
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
            DynamicMethod dynamicMethod = new DynamicMethod("JsonParser" + property.Name, null, new Type[] { typeof(Parser), type.MakeByRefType() }, type, true);
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
        /// 枚举解析调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> enumMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumByteMethod = typeof(Parser).GetMethod("enumByte", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumSByteMethod = typeof(Parser).GetMethod("enumSByte", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumShortMethod = typeof(Parser).GetMethod("enumShort", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUShortMethod = typeof(Parser).GetMethod("enumUShort", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumIntMethod = typeof(Parser).GetMethod("enumInt", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUIntMethod = typeof(Parser).GetMethod("enumUInt", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumLongMethod = typeof(Parser).GetMethod("enumLong", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumULongMethod = typeof(Parser).GetMethod("enumULong", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumByteFlagsMethod = typeof(Parser).GetMethod("enumByteFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumSByteFlagsMethod = typeof(Parser).GetMethod("enumSByteFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumShortFlagsMethod = typeof(Parser).GetMethod("enumShortFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUShortFlagsMethod = typeof(Parser).GetMethod("enumUShortFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumIntFlagsMethod = typeof(Parser).GetMethod("enumIntFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUIntFlagsMethod = typeof(Parser).GetMethod("enumUIntFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumLongFlagsMethod = typeof(Parser).GetMethod("enumLongFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举值解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumULongFlagsMethod = typeof(Parser).GetMethod("enumULongFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 获取枚举解析调用函数信息
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>枚举解析调用函数信息</returns>
        public static MethodInfo GetEnum(Type type)
        {
            MethodInfo method;
            if (enumMethods.TryGetValue(type, out method)) return method;
            Type enumType = System.Enum.GetUnderlyingType(type);
            if (AutoCSer.Metadata.TypeAttribute.GetAttribute<FlagsAttribute>(type) == null)
            {
                if (enumType == typeof(uint)) method = GenericType.Get(type).JsonParseEnumUIntMethod;// enumUIntMethod.MakeGenericMethod(type);
                else if (enumType == typeof(byte)) method = GenericType.Get(type).JsonParseEnumByteMethod;// enumByteMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ulong)) method = GenericType.Get(type).JsonParseEnumULongMethod;// enumULongMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ushort)) method = GenericType.Get(type).JsonParseEnumUShortMethod;// enumUShortMethod.MakeGenericMethod(type);
                else if (enumType == typeof(long)) method = GenericType.Get(type).JsonParseEnumLongMethod;// enumLongMethod.MakeGenericMethod(type);
                else if (enumType == typeof(short)) method = GenericType.Get(type).JsonParseEnumShortMethod;// enumShortMethod.MakeGenericMethod(type);
                else if (enumType == typeof(sbyte)) method = GenericType.Get(type).JsonParseEnumSByteMethod;// enumSByteMethod.MakeGenericMethod(type);
                else method = GenericType.Get(type).JsonParseEnumIntMethod;// enumIntMethod.MakeGenericMethod(type);
            }
            else
            {
                if (enumType == typeof(uint)) method = GenericType.Get(type).JsonParseEnumUIntFlagsMethod;// enumUIntFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(byte)) method = GenericType.Get(type).JsonParseEnumByteFlagsMethod;// enumByteFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ulong)) method = GenericType.Get(type).JsonParseEnumULongFlagsMethod;// enumULongFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ushort)) method = GenericType.Get(type).JsonParseEnumUShortFlagsMethod;// enumUShortFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(long)) method = GenericType.Get(type).JsonParseEnumLongFlagsMethod;// enumLongFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(short)) method = GenericType.Get(type).JsonParseEnumShortFlagsMethod;// enumShortFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(sbyte)) method = GenericType.Get(type).JsonParseEnumSByteFlagsMethod;// enumSByteFlagsMethod.MakeGenericMethod(type);
                else method = GenericType.Get(type).JsonParseEnumIntFlagsMethod;// enumIntFlagsMethod.MakeGenericMethod(type);
            }
            enumMethods.Set(type, method);
            return method;
        }
        ///// <summary>
        ///// 值类型解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> valueTypeMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 集合构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo structParseMethod = typeof(Parser).GetMethod("structParse", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取值类型解析调用函数信息
        ///// </summary>
        ///// <param name="type">数据类型</param>
        ///// <returns>值类型解析调用函数信息</returns>
        //public static MethodInfo GetValueType(Type type)
        //{
        //    MethodInfo method;
        //    if (valueTypeMethods.TryGetValue(type, out method)) return method;
        //    valueTypeMethods.Set(type, method = structParseMethod.MakeGenericMethod(type));
        //    return method;
        //}
        ///// <summary>
        ///// 引用类型解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> typeMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 引用类型对象解析函数信息
        ///// </summary>
        //private static readonly MethodInfo typeParseMethod = typeof(Parser).GetMethod("typeParse", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取引用类型解析调用函数信息
        ///// </summary>
        ///// <param name="type">数据类型</param>
        ///// <returns>引用类型解析调用函数信息</returns>
        //public static MethodInfo GetType(Type type)
        //{
        //    MethodInfo method;
        //    if (typeMethods.TryGetValue(type, out method)) return method;
        //    //if (type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, nullValue<Type>.Array, null) == null) method = checkNoConstructorMethod.MakeGenericMethod(type);
        //    //else
        //    method = typeParseMethod.MakeGenericMethod(type);
        //    typeMethods.Set(type, method);
        //    return method;
        //}

        ///// <summary>
        ///// 数组解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> arrayMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 数组解析函数信息
        ///// </summary>
        //private static readonly MethodInfo arrayMethod = typeof(Parser).GetMethod("array", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取数组解析委托调用函数信息
        ///// </summary>
        ///// <param name="type">数组类型</param>
        ///// <returns>数组解析委托调用函数信息</returns>
        //public static MethodInfo GetArray(Type type)
        //{
        //    MethodInfo method;
        //    if (arrayMethods.TryGetValue(type, out method)) return method;
        //    arrayMethods.Set(type, method = arrayMethod.MakeGenericMethod(type));
        //    return method;
        //}
        ///// <summary>
        ///// 字典解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> dictionaryMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 字典解析函数信息
        ///// </summary>
        //private static readonly MethodInfo dictionaryMethod = typeof(Parser).GetMethod("dictionary", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取字典解析调用函数信息
        ///// </summary>
        ///// <param name="type">数据类型</param>
        ///// <returns>字典解析调用函数信息</returns>
        //public static MethodInfo GetDictionary(Type type)
        //{
        //    MethodInfo method;
        //    if (dictionaryMethods.TryGetValue(type, out method)) return method;
        //    method = dictionaryMethod.MakeGenericMethod(type.GetGenericArguments());
        //    dictionaryMethods.Set(type, method);
        //    return method;
        //}
        ///// <summary>
        ///// 可空类型解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> nullableMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 集合构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableEnumParseMethod = typeof(Parser).GetMethod("nullableEnumParse", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableParseMethod = typeof(Parser).GetMethod("nullableParse", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 获取可空类型解析调用函数信息
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>可空类型解析调用函数信息</returns>
        public static MethodInfo GetNullable(Type type)
        {
            Type parameterType = type.GetGenericArguments()[0];
            if (parameterType.IsEnum) return StructGenericType.Get(parameterType).JsonParseNullableEnumMethod;
            return StructGenericType.Get(parameterType).JsonParseNullableMethod;

            //MethodInfo method;
            //if (nullableMethods.TryGetValue(type, out method)) return method;
            //Type[] parameterTypes = type.GetGenericArguments();
            //method = (parameterTypes[0].IsEnum ? nullableEnumParseMethod : nullableParseMethod).MakeGenericMethod(parameterTypes);
            //nullableMethods.Set(type, method);
            //return method;
        }
        ///// <summary>
        ///// 键值对解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> keyValuePairMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 值类型对象解析函数信息
        ///// </summary>
        //private static readonly MethodInfo keyValuePairParseMethod = typeof(Parser).GetMethod("keyValuePairParse", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取键值对解析调用函数信息
        ///// </summary>
        ///// <param name="type">数据类型</param>
        ///// <returns>键值对解析调用函数信息</returns>
        //public static MethodInfo GetKeyValuePair(Type type)
        //{
        //    MethodInfo method;
        //    if (keyValuePairMethods.TryGetValue(type, out method)) return method;
        //    keyValuePairMethods.Set(type, method = keyValuePairParseMethod.MakeGenericMethod(type.GetGenericArguments()));
        //    return method;
        //}
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
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Parser) && methodInfo.GetAttribute<ParseCustomAttribute>() != null)
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
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Parser) && parameters[1].ParameterType == refType)
                        {
                            if (methodInfo.GetAttribute<ParseCustomAttribute>() != null)
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
        ///// <summary>
        ///// 集合构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo listConstructorMethod = typeof(Parser).GetMethod("listConstructor", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo collectionConstructorMethod = typeof(Parser).GetMethod("collectionConstructor", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo enumerableConstructorMethod = typeof(Parser).GetMethod("enumerableConstructor", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo arrayConstructorMethod = typeof(Parser).GetMethod("arrayConstructor", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合构造解析函数信息
        ///// </summary>
        //private static readonly MethodInfo dictionaryConstructorMethod = typeof(Parser).GetMethod("dictionaryConstructor", BindingFlags.Instance | BindingFlags.NonPublic);
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
                            //method = listConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).JsonParseListConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = collectionConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).JsonParseCollectionConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = enumerableConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).JsonParseEnumerableConstructorMethod;
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = arrayConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).JsonParseArrayConstructorMethod;
                            break;
                        }
                    }
                    else if (genericType == typeof(IDictionary<,>))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { interfaceType }, null);
                        if (constructorInfo != null)
                        {
                            Type[] parameters = interfaceType.GetGenericArguments();
                            //method = dictionaryConstructorMethod.MakeGenericMethod(type, parameters[0], parameters[1]);
                            method = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).JsonParseDictionaryConstructorMethod;
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
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            enumMethods.Clear();
            //valueTypeMethods.Clear();
            //typeMethods.Clear();
            //arrayMethods.Clear();
            //dictionaryMethods.Clear();
            //nullableMethods.Clear();
            //keyValuePairMethods.Clear();
            customMethods.Clear();
            enumerableConstructorMethods.Clear();
        }
        static ParseMethodCache()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
