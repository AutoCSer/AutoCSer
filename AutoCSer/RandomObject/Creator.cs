using System;
using System.Reflection;
using AutoCSer.Emit;
using System.Collections.Generic;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机对象生成
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public static class Creator<valueType> 
    {
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        internal delegate void creator(ref valueType value, Config config);
        /// <summary>
        /// 基本类型随机数创建函数
        /// </summary>
        private static readonly Func<valueType> defaultCreator;
        /// <summary>
        /// 随机对象创建函数
        /// </summary>
        private static readonly Func<Config, valueType> configNullCreator;
        /// <summary>
        /// 随机对象创建函数
        /// </summary>
        private static readonly Func<Config, valueType> configCreator;
        /// <summary>
        /// 随机对象创建函数
        /// </summary>
        internal static readonly creator MemberCreator;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;

        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static valueType Create(Config config = null)
        {
            if (defaultCreator != null) return defaultCreator();
            if (configCreator != null)
            {
                if (config == null) config = Config.Default;
                if (config.History == null) return configCreator(config);
                try
                {
                    return configCreator(config);
                }
                finally { config.History.Clear(); }
            }
            Func<valueType> creator = Constructor<valueType>.New;
            if (creator == null) return default(valueType);
            valueType value = creator();
            if (config == null) config = Config.Default;
            if (config.History == null) MemberCreator(ref value, config);
            else
            {
                try
                {
                    MemberCreator(ref value, config);
                }
                finally { config.History.Clear(); }
            }
            return value;
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static valueType CreateNull(Config config)
        {
            if (defaultCreator != null) return defaultCreator();
            if (configNullCreator != null) return configNullCreator(config);
            Func<valueType> creator = Constructor<valueType>.New;
            if (creator == null) return default(valueType);
            if (isValueType)
            {
                valueType value = creator();
                MemberCreator(ref value, config);
                return value;
            }
            else
            {
                object historyValue = config.TryGetValue(typeof(valueType));
                if (historyValue != null) return (valueType)historyValue;
                if (AutoCSer.Random.Default.NextBit() == 0) return default(valueType);
                valueType value = config.SaveHistory(creator());
                MemberCreator(ref value, config);
                return value;
            }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static valueType CreateNotNull(Config config)
        {
            if (defaultCreator != null) return defaultCreator();
            if (configNullCreator != null) return configNullCreator(config);
            Func<valueType> creator = Constructor<valueType>.New;
            if (creator == null) return default(valueType);
            if (isValueType)
            {
                valueType value = creator();
                MemberCreator(ref value, config);
                return value;
            }
            else
            {
                object historyValue = config.TryGetValue(typeof(valueType));
                if (historyValue != null) return (valueType)historyValue;
                valueType value = config.SaveHistory(creator());
                MemberCreator(ref value, config);
                return value;
            }
        }

        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumByte()
        {
            return Emit.EnumCast<valueType, byte>.FromInt(MethodCache.CreateByte());
        }
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumSByte()
        {
            return Emit.EnumCast<valueType, sbyte>.FromInt(MethodCache.CreateSByte());
        }
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumShort()
        {
            return Emit.EnumCast<valueType, short>.FromInt(MethodCache.CreateShort());
        }
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumUShort()
        {
            return Emit.EnumCast<valueType, ushort>.FromInt(MethodCache.CreateUShort());
        }
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumInt()
        {
            return Emit.EnumCast<valueType, int>.FromInt(MethodCache.CreateInt());
        }
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumUInt()
        {
            return Emit.EnumCast<valueType, uint>.FromInt(MethodCache.CreateUInt());
        }
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumLong()
        {
            return Emit.EnumCast<valueType, long>.FromInt(MethodCache.CreateLong());
        }
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <returns></returns>
        private static valueType enumULong()
        {
            return Emit.EnumCast<valueType, ulong>.FromInt(MethodCache.CreateULong());
        }

        static Creator()
        {
            Type type = typeof(valueType);
            MethodInfo method = MethodCache.GetMethod(type);
            if (method != null)
            {
                defaultCreator = (Func<valueType>)Delegate.CreateDelegate(typeof(Func<valueType>), method);
                return;
            }
            if ((method = MethodCache.GetConfigMethod(type)) != null)
            {
                configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), method);
                configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.GetConfigNullMethod(type));
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    //Type elementType = type.GetElementType();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateArrayNullMethod.MakeGenericMethod(elementType));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateArrayMethod.MakeGenericMethod(elementType));
                    AutoCSer.RandomObject.Metadata.GenericType GenericType = AutoCSer.RandomObject.Metadata.GenericType.Get(type.GetElementType());
                    configNullCreator = (Func<Config, valueType>)GenericType.CreateArrayNullDelegate;
                    configCreator = (Func<Config, valueType>)GenericType.CreateArrayDelegate;
                }
                return;
            }
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(uint)) defaultCreator = enumUInt;
                else if (enumType == typeof(byte)) defaultCreator = enumByte;
                else if (enumType == typeof(ulong)) defaultCreator = enumULong;
                else if (enumType == typeof(ushort)) defaultCreator = enumUShort;
                else if (enumType == typeof(long)) defaultCreator = enumLong;
                else if (enumType == typeof(short)) defaultCreator = enumShort;
                else if (enumType == typeof(sbyte)) defaultCreator = enumSByte;
                else defaultCreator = enumInt;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    configNullCreator = configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateNullableMethod.MakeGenericMethod(type.GetGenericArguments()));
                    return;
                }
                if (genericType == typeof(LeftArray<>))
                {
                    //configNullCreator = configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateLeftArrayMethod.MakeGenericMethod(type.GetGenericArguments()));
                    configNullCreator = configCreator = (Func<Config, valueType>)AutoCSer.RandomObject.Metadata.GenericType.Get(type.GetGenericArguments()[0]).CreateLeftArrayDelegate;
                    return;
                }
                if (genericType == typeof(ListArray<>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListArrayNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListArrayMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType GenericType = AutoCSer.RandomObject.Metadata.GenericType.Get(type.GetGenericArguments()[0]);
                    configNullCreator = (Func<Config, valueType>)GenericType.CreateListArrayNullDelegate;
                    configCreator = (Func<Config, valueType>)GenericType.CreateListArrayDelegate;
                    return;
                }
                if (genericType == typeof(List<>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType GenericType = AutoCSer.RandomObject.Metadata.GenericType.Get(type.GetGenericArguments()[0]);
                    configNullCreator = (Func<Config, valueType>)GenericType.CreateListNullDelegate;
                    configCreator = (Func<Config, valueType>)GenericType.CreateListDelegate;
                    return;
                }
#if DOTNET2
                if (genericType == typeof(Queue<>) || genericType == typeof(Stack<>) || genericType == typeof(LinkedList<>))
#else
                if (genericType == typeof(HashSet<>) || genericType == typeof(Queue<>) || genericType == typeof(Stack<>) || genericType == typeof(SortedSet<>) || genericType == typeof(LinkedList<>))
#endif
                {
                    //Type[] parameterTypes = new Type[] { type, type.GetGenericArguments()[0] };
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateEnumerableConstructorNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateEnumerableConstructorMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType2 GenericType2 = AutoCSer.RandomObject.Metadata.GenericType2.Get(new Type[] { type, type.GetGenericArguments()[0] });
                    configNullCreator = (Func<Config, valueType>)GenericType2.CreateEnumerableConstructorNullDelegate;
                    configCreator = (Func<Config, valueType>)GenericType2.CreateEnumerableConstructorDelegate;
                    return;
                }
                if (genericType == typeof(Dictionary<,>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateDictionaryNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateDictionaryMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType2 GenericType2 = AutoCSer.RandomObject.Metadata.GenericType2.Get(type.GetGenericArguments());
                    configNullCreator = (Func<Config, valueType>)GenericType2.CreateDictionaryNullDelegate;
                    configCreator = (Func<Config, valueType>)GenericType2.CreateDictionaryDelegate;
                    return;
                }
                if (genericType == typeof(SortedDictionary<,>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedDictionaryNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedDictionaryMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType2 GenericType2 = AutoCSer.RandomObject.Metadata.GenericType2.Get(type.GetGenericArguments());
                    configNullCreator = (Func<Config, valueType>)GenericType2.CreateSortedDictionaryNullDelegate;
                    configCreator = (Func<Config, valueType>)GenericType2.CreateSortedDictionaryDelegate;
                    return;
                }
                if (genericType == typeof(SortedList<,>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedListNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedListMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType2 GenericType2 = AutoCSer.RandomObject.Metadata.GenericType2.Get(type.GetGenericArguments());
                    configNullCreator = (Func<Config, valueType>)GenericType2.CreateSortedListNullDelegate;
                    configCreator = (Func<Config, valueType>)GenericType2.CreateSortedListDelegate;
                    return;
                }
            }
            if (type.IsPointer || type.IsInterface || typeof(Delegate).IsAssignableFrom(type)) return;
            isValueType = type.IsValueType;
#if NOJIT
            MemberCreator = new memberRandom().Random();
#else
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(type);
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))// | BindingFlags.DeclaredOnly
            {
                dynamicMethod.Push(field);
            }
            dynamicMethod.Base();
            try
            {
                MemberCreator = (creator)dynamicMethod.Create<creator>();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
#endif
        }
    }
}
