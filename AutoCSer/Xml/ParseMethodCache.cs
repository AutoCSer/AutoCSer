using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Threading;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Xml
{
    /// <summary>
    /// 类型解析器静态信息
    /// </summary>
    internal static class ParseMethodCache
    {
        /// <summary>
        /// 获取属性成员集合
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>属性成员集合</returns>
        public static LeftArray<PropertyMethod> GetProperties(PropertyIndex[] properties, SerializeAttribute typeAttribute)
        {
            LeftArray<PropertyMethod> values = new LeftArray<PropertyMethod>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanWrite)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        MemberAttribute attribute = property.GetAttribute<MemberAttribute>(typeAttribute.IsBaseTypeAttribute);
                        if (typeAttribute.IsAttribute ? (attribute != null && attribute.IsSetup) : (attribute == null || attribute.IsSetup))
                        {
                            MethodInfo method = property.Member.GetSetMethod(true);
                            if (method != null && method.GetParameters().Length == 1)
                            {
                                values.Add(new PropertyMethod { Property = property, Method = method, Attribute = attribute });
                            }
                        }
                    }
                }
            }
            return values;
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
            DynamicMethod dynamicMethod = new DynamicMethod("XmlParser" + field.Name, null, new Type[] { typeof(Parser), type.MakeByRefType() }, type, true);
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
            DynamicMethod dynamicMethod = new DynamicMethod("XmlParser" + property.Name, null, new Type[] { typeof(Parser), type.MakeByRefType() }, type, true);
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
                if (enumType == typeof(uint)) method = GenericType.Get(type).XmlParseEnumUIntMethod;// enumUIntMethod.MakeGenericMethod(type);
                else if (enumType == typeof(byte)) method = GenericType.Get(type).XmlParseEnumByteMethod;// enumByteMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ulong)) method = GenericType.Get(type).XmlParseEnumULongMethod;// enumULongMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ushort)) method = GenericType.Get(type).XmlParseEnumUShortMethod;// enumUShortMethod.MakeGenericMethod(type);
                else if (enumType == typeof(long)) method = GenericType.Get(type).XmlParseEnumLongMethod;// enumLongMethod.MakeGenericMethod(type);
                else if (enumType == typeof(short)) method = GenericType.Get(type).XmlParseEnumShortMethod;// enumShortMethod.MakeGenericMethod(type);
                else if (enumType == typeof(sbyte)) method = GenericType.Get(type).XmlParseEnumSByteMethod;// enumSByteMethod.MakeGenericMethod(type);
                else method = GenericType.Get(type).XmlParseEnumIntMethod;// enumIntMethod.MakeGenericMethod(type);
            }
            else
            {
                if (enumType == typeof(uint)) method = GenericType.Get(type).XmlParseEnumUIntFlagsMethod;// enumUIntFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(byte)) method = GenericType.Get(type).XmlParseEnumByteFlagsMethod;// enumByteFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ulong)) method = GenericType.Get(type).XmlParseEnumULongFlagsMethod;// enumULongFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(ushort)) method = GenericType.Get(type).XmlParseEnumUShortFlagsMethod;// enumUShortFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(long)) method = GenericType.Get(type).XmlParseEnumLongFlagsMethod;// enumLongFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(short)) method = GenericType.Get(type).XmlParseEnumShortFlagsMethod;// enumShortFlagsMethod.MakeGenericMethod(type);
                else if (enumType == typeof(sbyte)) method = GenericType.Get(type).XmlParseEnumSByteFlagsMethod;// enumSByteFlagsMethod.MakeGenericMethod(type);
                else method = GenericType.Get(type).XmlParseEnumIntFlagsMethod;// enumIntFlagsMethod.MakeGenericMethod(type);
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
        /// <summary>
        /// 获取值类型解析调用函数信息
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>值类型解析调用函数信息</returns>
        public static MethodInfo GetValueType(Type type)
        {
            Type nullType = type.nullableType();
            if (nullType == null) return StructGenericType.Get(type).XmlParseStructMethod;
            return StructGenericType.Get(nullType).XmlParseNullableMethod;
            //MethodInfo method;
            //if (valueTypeMethods.TryGetValue(type, out method)) return method;
            //Type nullType = type.nullableType();
            //if (nullType == null) method = structParseMethod.MakeGenericMethod(type);
            //else method = nullableParseMethod.MakeGenericMethod(nullType);
            //valueTypeMethods.Set(type, method);
            //return method;
        }
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
        //    //if (type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, nullValue<Type>.Array, null) == null) method = noConstructorMethod.MakeGenericMethod(type);
        //    //else
        //    method = typeParseMethod.MakeGenericMethod(type);
        //    typeMethods.Set(type, method);
        //    return method;
        //}
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
            if (type.IsArray) return GenericType.Get(type.GetElementType()).XmlParseArrayMethod;
            if (type.IsEnum) return GetEnum(type);
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    Type[] parameterTypes = type.GetGenericArguments();
                    //return parameterTypes[0].IsEnum ? GetNullableEnumParse(type, parameterTypes) : GetNullableParse(type, parameterTypes);
                    return parameterTypes[0].IsEnum ? StructGenericType.Get(parameterTypes[0]).XmlParseNullableEnumMethod : StructGenericType.Get(parameterTypes[0]).XmlParseNullableMethod;
                }
                //if (genericType == typeof(KeyValuePair<,>)) return GetKeyValuePair(type);
                if (genericType == typeof(KeyValuePair<,>)) return GenericType2.Get(type.GetGenericArguments()).XmlParseKeyValuePairMethod;
            }
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            //if (type.IsAbstract || type.IsInterface) return typeParser.GetNoConstructorParser(type);
            if ((methodInfo = GetIEnumerableConstructor(type)) != null) return methodInfo;
            if (type.IsValueType) return GetValueType(type);
            //return GetType(type);
            return GenericType.Get(type).XmlParseTypeMethod;
        }
        /// <summary>
        /// 基类转换函数信息
        /// </summary>
        internal static readonly MethodInfo BaseParseMethod = typeof(Parser).GetMethod("baseParse", BindingFlags.Instance | BindingFlags.NonPublic);
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
        ///// 可空枚举类型解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> nullableEnumParseMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 可空枚举类型解析函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableEnumParseMethod = typeof(Parser).GetMethod("nullableEnumParse", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取可空枚举类型解析委托调用函数信息
        ///// </summary>
        ///// <param name="type">数组类型</param>
        ///// <param name="parameterTypes">泛型参数类型集合</param>
        ///// <returns>可空枚举类型解析委托调用函数信息</returns>
        //public static MethodInfo GetNullableEnumParse(Type type, Type[] parameterTypes)
        //{
        //    MethodInfo method;
        //    if (nullableEnumParseMethods.TryGetValue(type, out method)) return method;
        //    nullableEnumParseMethods.Set(type, method = nullableEnumParseMethod.MakeGenericMethod(parameterTypes));
        //    return method;
        //}
        ///// <summary>
        ///// 可空类型解析调用函数信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> nullableParseMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 可空类型解析函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableParseMethod = typeof(Parser).GetMethod("nullableParse", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取可空类型解析委托调用函数信息
        ///// </summary>
        ///// <param name="type">数组类型</param>
        ///// <param name="parameterTypes">泛型参数类型集合</param>
        ///// <returns>可空类型解析委托调用函数信息</returns>
        //public static MethodInfo GetNullableParse(Type type, Type[] parameterTypes)
        //{
        //    MethodInfo method;
        //    if (nullableParseMethods.TryGetValue(type, out method)) return method;
        //    nullableParseMethods.Set(type, method = nullableParseMethod.MakeGenericMethod(parameterTypes));
        //    return method;
        //}
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
                            method = GenericType2.Get(type, argumentType).XmlParseListConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = collectionConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).XmlParseCollectionConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = enumerableConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).XmlParseEnumerableConstructorMethod;
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = arrayConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).XmlParseArrayConstructorMethod;
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
                            method = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).XmlParseDictionaryConstructorMethod;
                            break;
                        }
                    }
                }
            }
            enumerableConstructorMethods.Set(type, method);
            return method;
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
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Parser) && methodInfo.GetAttribute<CustomAttribute>() != null)
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
        /// 泛型定义类型成员名称查找数据
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, Pointer> genericDefinitionMemberSearchers = new AutoCSer.Threading.LockDictionary<Type, Pointer>();
        /// <summary>
        /// 泛型定义类型成员名称查找数据创建锁
        /// </summary>
        private static readonly object genericDefinitionMemberSearcherCreateLock = new object();
        /// <summary>
        /// 获取泛型定义成员名称查找数据
        /// </summary>
        /// <param name="type">泛型定义类型</param>
        /// <param name="names">成员名称集合</param>
        /// <returns>泛型定义成员名称查找数据</returns>
        internal static Pointer GetGenericDefinitionMemberSearcher(Type type, string[] names)
        {
            Pointer data;
            if (genericDefinitionMemberSearchers.TryGetValue(type = type.GetGenericTypeDefinition(), out data)) return data;
            Monitor.Enter(genericDefinitionMemberSearcherCreateLock);
            if (genericDefinitionMemberSearchers.TryGetValue(type, out data))
            {
                Monitor.Exit(genericDefinitionMemberSearcherCreateLock);
                return data;
            }
            try
            {
                genericDefinitionMemberSearchers.Set(type, data = AutoCSer.StateSearcher.CharBuilder.Create(names, true).Pointer);
            }
            finally { Monitor.Exit(genericDefinitionMemberSearcherCreateLock); }
            return data;
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
            //nullableEnumParseMethods.Clear();
            //nullableParseMethods.Clear();
            //keyValuePairMethods.Clear();
            enumerableConstructorMethods.Clear();
            customMethods.Clear();
        }
        static ParseMethodCache()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
