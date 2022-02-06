using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Threading;
using AutoCSer.Extensions;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Xml
{
    /// <summary>
    /// 类型解析器静态信息
    /// </summary>
    internal static class DeSerializeMethodCache
    {
        /// <summary>
        /// 获取属性成员集合
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>属性成员集合</returns>
        public static LeftArray<PropertyMethod> GetProperties(PropertyIndex[] properties, XmlSerializeAttribute typeAttribute)
        {
            LeftArray<PropertyMethod> values = new LeftArray<PropertyMethod>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanWrite)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !property.Member.IsDefined(typeof(IgnoreMemberAttribute), typeAttribute.IsBaseTypeAttribute))
                    {
                        XmlSerializeMemberAttribute attribute = property.GetAttribute<XmlSerializeMemberAttribute>(typeAttribute.IsBaseTypeAttribute);
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
            DynamicMethod dynamicMethod = new DynamicMethod("XmlDeSerializer" + field.Name, null, new Type[] { typeof(XmlDeSerializer), type.MakeByRefType() }, type, true);
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
            DynamicMethod dynamicMethod = new DynamicMethod("XmlDeSerializer" + property.Name, null, new Type[] { typeof(XmlDeSerializer), type.MakeByRefType() }, type, true);
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
        /// 获取值类型解析调用函数信息
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>值类型解析调用函数信息</returns>
        public static MethodInfo GetValueType(Type type)
        {
            Type nullType = type.nullableType();
            if (nullType == null) return StructGenericType.Get(type).XmlDeSerializeStructMethod;
            return StructGenericType.Get(nullType).XmlDeSerializeNullableMethod.Method;
        }
        /// <summary>
        /// 获取成员转换函数信息
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="isCustom"></param>
        /// <returns>成员转换函数信息</returns>
        internal static MethodInfo GetMemberMethodInfo(Type type, ref bool isCustom)
        {
            MethodInfo methodInfo = XmlDeSerializer.GetDeSerializeMethod(type);
            if (methodInfo != null) return methodInfo;
            if (type.IsArray) return GenericType.Get(type.GetElementType()).XmlDeSerializeArrayMethod.Method;
            if (type.IsEnum)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false)) return (EnumGenericType.Get(type).XmlDeSerializeEnumFlagsDelegate).Method;
                return (EnumGenericType.Get(type).XmlDeSerializeEnumDelegate).Method;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    Type[] parameterTypes = type.GetGenericArguments();
                    return parameterTypes[0].IsEnum ? StructGenericType.Get(parameterTypes[0]).XmlDeSerializeNullableEnumMethod.Method : StructGenericType.Get(parameterTypes[0]).XmlDeSerializeNullableMethod.Method;
                }
                if (genericType == typeof(KeyValuePair<,>)) return GenericType2.Get(type.GetGenericArguments()).XmlDeSerializeKeyValuePairMethod.Method;
            }
            if ((methodInfo = GetCustom(type)) != null)
            {
                isCustom = type.IsValueType;
                return methodInfo;
            }
            if ((methodInfo = GetIEnumerableConstructor(type)) != null) return methodInfo;
            if (type.IsValueType) return GetValueType(type);
            //return GetType(type);
            return GenericType.Get(type).XmlDeSerializeTypeMethod;
        }
        /// <summary>
        /// 基类转换函数信息
        /// </summary>
        internal static readonly MethodInfo BaseDeSerializeMethod = typeof(XmlDeSerializer).GetMethod("baseDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
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
                            //method = listConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).XmlDeSerializeListConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = collectionConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).XmlDeSerializeCollectionConstructorMethod;
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = enumerableConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).XmlDeSerializeEnumerableConstructorMethod;
                            break;
                        }
                        parameters[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //method = arrayConstructorMethod.MakeGenericMethod(type, argumentType);
                            method = GenericType2.Get(type, argumentType).XmlDeSerializeArrayConstructorMethod;
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
                            method = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).XmlDeSerializeDictionaryConstructorMethod;
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
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(XmlDeSerializer) && methodInfo.GetAttribute<CustomAttribute>() != null)
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
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(XmlDeSerializer) && parameters[1].ParameterType == refType)
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
        private static readonly AutoCSer.Threading.LockDictionary<Type, AutoCSer.Memory.Pointer> genericDefinitionMemberSearchers = new AutoCSer.Threading.LockDictionary<Type, AutoCSer.Memory.Pointer>();
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
        internal static AutoCSer.Memory.Pointer GetGenericDefinitionMemberSearcher(Type type, string[] names)
        {
            AutoCSer.Memory.Pointer data;
            if (genericDefinitionMemberSearchers.TryGetValue(type = type.GetGenericTypeDefinition(), out data)) return data;
            Monitor.Enter(genericDefinitionMemberSearcherCreateLock);
            if (genericDefinitionMemberSearchers.TryGetValue(type, out data))
            {
                Monitor.Exit(genericDefinitionMemberSearcherCreateLock);
                return data;
            }
            try
            {
                genericDefinitionMemberSearchers.Set(type, data = AutoCSer.StateSearcher.CharBuilder.Create(names, true));
            }
            finally { Monitor.Exit(genericDefinitionMemberSearcherCreateLock); }
            return data;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            enumerableConstructorMethods.Clear();
            customMethods.Clear();
        }
        static DeSerializeMethodCache()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(DeSerializeMethodCache), 60 * 60);
        }
    }
}
