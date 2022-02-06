using System;
using System.Reflection;
using AutoCSer.Emit;
using System.Collections.Generic;
using AutoCSer.Metadata;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机对象生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Creator<T> 
    {
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        internal delegate void creator(ref T value, Config config);
        /// <summary>
        /// 基本类型随机数创建函数
        /// </summary>
        private static readonly Func<T> defaultCreator;
        /// <summary>
        /// 随机对象创建函数
        /// </summary>
        private static readonly Func<Config, T> configNullCreator;
        /// <summary>
        /// 随机对象创建函数
        /// </summary>
        private static readonly Func<Config, T> configCreator;
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
        public static T Create(Config config = null)
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
            if (DefaultConstructor<T>.Type == DefaultConstructorType.None) return default(T);
            T value = DefaultConstructor<T>.Constructor();
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
        internal static T CreateNull(Config config)
        {
            if (defaultCreator != null) return defaultCreator();
            if (configNullCreator != null) return configNullCreator(config);
            if (DefaultConstructor<T>.Type == DefaultConstructorType.None) return default(T);
            if (isValueType)
            {
                T value = DefaultConstructor<T>.Constructor();
                MemberCreator(ref value, config);
                return value;
            }
            else
            {
                object historyValue = config.TryGetValue(typeof(T));
                if (historyValue != null) return (T)historyValue;
                if (AutoCSer.Random.Default.NextBit() == 0) return default(T);
                T value = config.SaveHistory(DefaultConstructor<T>.Constructor());
                MemberCreator(ref value, config);
                return value;
            }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static T CreateNotNull(Config config)
        {
            if (defaultCreator != null) return defaultCreator();
            if (configNullCreator != null) return configNullCreator(config);
            if (DefaultConstructor<T>.Type == AutoCSer.Metadata.DefaultConstructorType.None) return default(T);
            if (isValueType)
            {
                T value = DefaultConstructor<T>.Constructor();
                MemberCreator(ref value, config);
                return value;
            }
            else
            {
                object historyValue = config.TryGetValue(typeof(T));
                if (historyValue != null) return (T)historyValue;
                T value = config.SaveHistory(DefaultConstructor<T>.Constructor());
                MemberCreator(ref value, config);
                return value;
            }
        }

        static Creator()
        {
            Type type = typeof(T);
            MethodInfo method = MethodCache.GetMethod(type);
            if (method != null)
            {
                defaultCreator = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), method);
                return;
            }
            if ((method = MethodCache.GetConfigMethod(type)) != null)
            {
                configCreator = (Func<Config, T>)Delegate.CreateDelegate(typeof(Func<Config, T>), method);
                configNullCreator = (Func<Config, T>)Delegate.CreateDelegate(typeof(Func<Config, T>), MethodCache.GetConfigNullMethod(type));
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
                    configNullCreator = (Func<Config, T>)GenericType.CreateArrayNullDelegate;
                    configCreator = (Func<Config, T>)GenericType.CreateArrayDelegate;
                }
                return;
            }
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(uint)) defaultCreator = Creator.EnumUInt<T>;
                else if (enumType == typeof(byte)) defaultCreator = Creator.EnumByte<T>;
                else if (enumType == typeof(ulong)) defaultCreator = Creator.EnumULong<T>;
                else if (enumType == typeof(ushort)) defaultCreator = Creator.EnumUShort<T>;
                else if (enumType == typeof(long)) defaultCreator = Creator.EnumLong<T>;
                else if (enumType == typeof(short)) defaultCreator = Creator.EnumShort<T>;
                else if (enumType == typeof(sbyte)) defaultCreator = Creator.EnumSByte<T>;
                else defaultCreator = Creator.EnumInt<T>;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    configNullCreator = configCreator = (Func<Config, T>)Delegate.CreateDelegate(typeof(Func<Config, T>), MethodCache.CreateNullableMethod.MakeGenericMethod(type.GetGenericArguments()));
                    return;
                }
                if (genericType == typeof(LeftArray<>))
                {
                    //configNullCreator = configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateLeftArrayMethod.MakeGenericMethod(type.GetGenericArguments()));
                    configNullCreator = configCreator = (Func<Config, T>)AutoCSer.RandomObject.Metadata.GenericType.Get(type.GetGenericArguments()[0]).CreateLeftArrayDelegate;
                    return;
                }
                if (genericType == typeof(ListArray<>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListArrayNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListArrayMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType GenericType = AutoCSer.RandomObject.Metadata.GenericType.Get(type.GetGenericArguments()[0]);
                    configNullCreator = (Func<Config, T>)GenericType.CreateListArrayNullDelegate;
                    configCreator = (Func<Config, T>)GenericType.CreateListArrayDelegate;
                    return;
                }
                if (genericType == typeof(List<>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateListMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType GenericType = AutoCSer.RandomObject.Metadata.GenericType.Get(type.GetGenericArguments()[0]);
                    configNullCreator = (Func<Config, T>)GenericType.CreateListNullDelegate;
                    configCreator = (Func<Config, T>)GenericType.CreateListDelegate;
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
                    configNullCreator = (Func<Config, T>)GenericType2.CreateEnumerableConstructorNullDelegate;
                    configCreator = (Func<Config, T>)GenericType2.CreateEnumerableConstructorDelegate;
                    return;
                }
                if (genericType == typeof(Dictionary<,>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateDictionaryNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateDictionaryMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType2 GenericType2 = AutoCSer.RandomObject.Metadata.GenericType2.Get(type.GetGenericArguments());
                    configNullCreator = (Func<Config, T>)GenericType2.CreateDictionaryNullDelegate;
                    configCreator = (Func<Config, T>)GenericType2.CreateDictionaryDelegate;
                    return;
                }
                if (genericType == typeof(SortedDictionary<,>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedDictionaryNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedDictionaryMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType2 GenericType2 = AutoCSer.RandomObject.Metadata.GenericType2.Get(type.GetGenericArguments());
                    configNullCreator = (Func<Config, T>)GenericType2.CreateSortedDictionaryNullDelegate;
                    configCreator = (Func<Config, T>)GenericType2.CreateSortedDictionaryDelegate;
                    return;
                }
                if (genericType == typeof(SortedList<,>))
                {
                    //Type[] parameterTypes = type.GetGenericArguments();
                    //configNullCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedListNullMethod.MakeGenericMethod(parameterTypes));
                    //configCreator = (Func<Config, valueType>)Delegate.CreateDelegate(typeof(Func<Config, valueType>), MethodCache.CreateSortedListMethod.MakeGenericMethod(parameterTypes));
                    AutoCSer.RandomObject.Metadata.GenericType2 GenericType2 = AutoCSer.RandomObject.Metadata.GenericType2.Get(type.GetGenericArguments());
                    configNullCreator = (Func<Config, T>)GenericType2.CreateSortedListNullDelegate;
                    configCreator = (Func<Config, T>)GenericType2.CreateSortedListDelegate;
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
