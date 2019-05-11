using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机对象生成函数信息
    /// </summary>
    internal static partial class MethodCache
    {
        /// <summary>
        /// 最大随机数组尺寸
        /// </summary>
        private const uint maxSize = (1 << 4) - 1;
        /// <summary>
        /// 基本类型随机数创建函数信息集合
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> createMethods;
        /// <summary>
        /// 获取基本类型随机数创建函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns>基本类型随机数创建函数信息</returns>
        internal static MethodInfo GetMethod(Type type)
        {
            MethodInfo method;
            if (createMethods.TryGetValue(type, out method))
            {
                createMethods.Remove(type);
                return method;
            }
            return null;
        }
        /// <summary>
        /// 基本类型随机数创建函数信息集合
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> createConfigMethods;
        /// <summary>
        /// 获取基本类型随机数创建函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns>基本类型随机数创建函数信息</returns>
        internal static MethodInfo GetConfigMethod(Type type)
        {
            MethodInfo method;
            if (createConfigMethods.TryGetValue(type, out method))
            {
                createConfigMethods.Remove(type);
                return method;
            }
            return null;
        }
        /// <summary>
        /// 基本类型随机数创建函数信息集合
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> createConfigNullMethods;
        /// <summary>
        /// 获取基本类型随机数创建函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns>基本类型随机数创建函数信息</returns>
        internal static MethodInfo GetConfigNullMethod(Type type)
        {
            MethodInfo method;
            if (createConfigNullMethods.TryGetValue(type, out method))
            {
                createConfigNullMethods.Remove(type);
                return method;
            }
            return null;
        }

        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static valueType create<valueType>(Config config)
        {
            return Creator<valueType>.CreateNull(config);
        }
        ///// <summary>
        ///// 创建随机对象函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateMethod = typeof(MethodCache).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机成员对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <param name="config"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void createMember<valueType>(ref valueType value, Config config)
        {
            Creator<valueType>.MemberCreator(ref value, config);
        }
        ///// <summary>
        ///// 创建随机对象函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateMemberMethod = typeof(MethodCache).GetMethod("createMember", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static valueType[] createArray<valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(valueType[]));
            if (historyValue != null) return (valueType[])historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            if (length > 0)
            {
                valueType[] value = config.SaveHistory(new valueType[--length]);
                while (length != 0) value[--length] = Creator<valueType>.CreateNull(config);
                return value;
            }
            return null;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateArrayMethod = typeof(MethodCache).GetMethod("createArray", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static valueType[] createArrayNull<valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(valueType[]));
            if (historyValue != null) return (valueType[])historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            valueType[] value = config.SaveHistory(new valueType[length]);
            while (length != 0) value[--length] = Creator<valueType>.CreateNull(config);
            return value;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateArrayNullMethod = typeof(MethodCache).GetMethod("createArrayNull", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Nullable<valueType> createNullable<valueType>(Config config) where valueType : struct
        {
            if (createBool()) return Creator<valueType>.CreateNotNull(config);
            return new Nullable<valueType>();
        }
        /// <summary>
        /// 创建可空随机对象函数信息
        /// </summary>
        internal static readonly MethodInfo CreateNullableMethod = typeof(MethodCache).GetMethod("createNullable", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static LeftArray<valueType> createLeftArray<valueType>(Config config)
        {
            return new LeftArray<valueType>(createArray<valueType>(config));
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateLeftArrayMethod = typeof(MethodCache).GetMethod("createLeftArray", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static ListArray<valueType> createListArray<valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(ListArray<valueType>));
            if (historyValue != null) return (ListArray<valueType>)historyValue;
            ListArray<valueType> value = config.SaveHistory(new ListArray<valueType>());
            value.Add(createArray<valueType>(config));
            return value;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateListArrayMethod = typeof(MethodCache).GetMethod("createListArray", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static ListArray<valueType> createListArrayNull<valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(ListArray<valueType>));
            if (historyValue != null) return (ListArray<valueType>)historyValue;
            valueType[] array = createArrayNull<valueType>(config);
            return array == null ? null : config.SaveHistory(new ListArray<valueType>(array, true));
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateListArrayNullMethod = typeof(MethodCache).GetMethod("createListArrayNull", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static System.Collections.Generic.List<valueType> createList<valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(System.Collections.Generic.List<valueType>));
            if (historyValue != null) return (System.Collections.Generic.List<valueType>)historyValue;
            System.Collections.Generic.List<valueType> value = config.SaveHistory(new System.Collections.Generic.List<valueType>());
            value.AddRange(createArray<valueType>(config));
            return value;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateListMethod = typeof(MethodCache).GetMethod("createList", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static System.Collections.Generic.List<valueType> createListNull<valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(System.Collections.Generic.List<valueType>));
            if (historyValue != null) return (System.Collections.Generic.List<valueType>)historyValue;
            valueType[] array = createArrayNull<valueType>(config);
            return array == null ? null : config.SaveHistory(new System.Collections.Generic.List<valueType>(array));
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateListNullMethod = typeof(MethodCache).GetMethod("createListNull", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="argumentType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static valueType createEnumerableConstructor<valueType, argumentType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(valueType));
            if (historyValue != null) return (valueType)historyValue;
            return config.SaveHistory(Emit.EnumerableConstructor<valueType, argumentType>.Constructor(createArray<argumentType>(config)));
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateEnumerableConstructorMethod = typeof(MethodCache).GetMethod("createEnumerableConstructor", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="argumentType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static valueType createEnumerableConstructorNull<valueType, argumentType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(valueType));
            if (historyValue != null) return (valueType)historyValue;
            valueType[] array = createArrayNull<valueType>(config);
            return array == null ? default(valueType) : config.SaveHistory(Emit.EnumerableConstructor<valueType, argumentType>.Constructor(createArray<argumentType>(config)));
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateEnumerableConstructorNullMethod = typeof(MethodCache).GetMethod("createEnumerableConstructorNull", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static Dictionary<keyType, valueType> createDictionary<keyType, valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(Dictionary<keyType, valueType>));
            if (historyValue != null) return (Dictionary<keyType, valueType>)historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            Dictionary<keyType, valueType> values = config.SaveHistory(DictionaryCreator.CreateAny<keyType, valueType>((int)length));
            while (length-- != 0)
            {
                keyType key = Creator<keyType>.CreateNotNull(config);
                valueType value;
                if (!values.TryGetValue(key, out value)) values.Add(key, Creator<valueType>.CreateNull(config));
            }
            return values;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateDictionaryMethod = typeof(MethodCache).GetMethod("createDictionary", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static Dictionary<keyType, valueType> createDictionaryNull<keyType, valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(Dictionary<keyType, valueType>));
            if (historyValue != null) return (Dictionary<keyType, valueType>)historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            if (length > 0)
            {
                Dictionary<keyType, valueType> values = config.SaveHistory(DictionaryCreator.CreateAny<keyType, valueType>((int)length));
                while (--length != 0)
                {
                    keyType key = Creator<keyType>.CreateNotNull(config);
                    valueType value;
                    if (!values.TryGetValue(key, out value)) values.Add(key, Creator<valueType>.CreateNull(config));
                }
                return values;
            }
            return null;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateDictionaryNullMethod = typeof(MethodCache).GetMethod("createDictionaryNull", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static SortedDictionary<keyType, valueType> createSortedDictionary<keyType, valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(Dictionary<keyType, valueType>));
            if (historyValue != null) return (SortedDictionary<keyType, valueType>)historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            SortedDictionary<keyType, valueType> values = config.SaveHistory(new SortedDictionary<keyType, valueType>());
            while (length-- != 0)
            {
                keyType key = Creator<keyType>.CreateNotNull(config);
                valueType value;
                if (!values.TryGetValue(key, out value)) values.Add(key, Creator<valueType>.CreateNull(config));
            }
            return values;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateSortedDictionaryMethod = typeof(MethodCache).GetMethod("createSortedDictionary", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static SortedDictionary<keyType, valueType> createSortedDictionaryNull<keyType, valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(Dictionary<keyType, valueType>));
            if (historyValue != null) return (SortedDictionary<keyType, valueType>)historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            if (length > 0)
            {
                SortedDictionary<keyType, valueType> values = config.SaveHistory(new SortedDictionary<keyType, valueType>());
                while (--length != 0)
                {
                    keyType key = Creator<keyType>.CreateNotNull(config);
                    valueType value;
                    if (!values.TryGetValue(key, out value)) values.Add(key, Creator<valueType>.CreateNull(config));
                }
                return values;
            }
            return null;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateSortedDictionaryNullMethod = typeof(MethodCache).GetMethod("createSortedDictionaryNull", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static SortedList<keyType, valueType> createSortedList<keyType, valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(Dictionary<keyType, valueType>));
            if (historyValue != null) return (SortedList<keyType, valueType>)historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            SortedList<keyType, valueType> values = config.SaveHistory(new SortedList<keyType, valueType>((int)length));
            while (length-- != 0)
            {
                keyType key = Creator<keyType>.CreateNotNull(config);
                valueType value;
                if (!values.TryGetValue(key, out value)) values.Add(key, Creator<valueType>.CreateNull(config));
            }
            return values;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateSortedListMethod = typeof(MethodCache).GetMethod("createSortedList", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static SortedList<keyType, valueType> createSortedListNull<keyType, valueType>(Config config)
        {
            object historyValue = config.TryGetValue(typeof(Dictionary<keyType, valueType>));
            if (historyValue != null) return (SortedList<keyType, valueType>)historyValue;
            uint length = (uint)AutoCSer.Random.Default.NextByte() & maxSize;
            if (length > 0)
            {
                SortedList<keyType, valueType> values = config.SaveHistory(new SortedList<keyType, valueType>((int)length));
                while (--length != 0)
                {
                    keyType key = Creator<keyType>.CreateNotNull(config);
                    valueType value;
                    if (!values.TryGetValue(key, out value)) values.Add(key, Creator<valueType>.CreateNull(config));
                }
                return values;
            }
            return null;
        }
        ///// <summary>
        ///// 创建随机数组函数信息
        ///// </summary>
        //internal static readonly MethodInfo CreateSortedListNullMethod = typeof(MethodCache).GetMethod("createSortedListNull", BindingFlags.Static | BindingFlags.NonPublic);

        static MethodCache()
        {
            createMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            createConfigMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            createConfigNullMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(MethodCache).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(CreateMethod), false))
                {
                    createMethods.Add(method.ReturnType, method);
                }
                else
                {
                    if (method.IsDefined(typeof(CreateConfigMethod), false))
                    {
                        createConfigMethods.Add(method.ReturnType, method);
                    }
                    if (method.IsDefined(typeof(CreateConfigNullMethod), false))
                    {
                        createConfigNullMethods.Add(method.ReturnType, method);
                    }
                }
            }
        }
    }
}
