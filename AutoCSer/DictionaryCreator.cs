using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="keyType">关键字类型</typeparam>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<keyType, valueType> CreateOnly<keyType, valueType>()
             where keyType : class
        {
            return new Dictionary<keyType, valueType>();
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="keyType">关键字类型</typeparam>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<keyType, valueType> CreateAny<keyType, valueType>()
        {
#if __IOS__
            return new Dictionary<keyType, valueType>(EqualityComparer.comparer<keyType>.Default);
#else
            return new Dictionary<keyType, valueType>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="keyType">关键字类型</typeparam>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<keyType, valueType> CreateAny<keyType, valueType>(int capacity)
        {
#if __IOS__
            return new Dictionary<keyType, valueType>(capacity, EqualityComparer.comparer<keyType>.Default);
#else
            return new Dictionary<keyType, valueType>(capacity);
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<int, valueType> CreateInt<valueType>()
        {
#if __IOS__
            return new Dictionary<int, valueType>(EqualityComparer.Int);
#else
            return new Dictionary<int, valueType>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<HashString, valueType> CreateHashString<valueType>()
        {
#if __IOS__
            return new Dictionary<HashString, valueType>(EqualityComparer.HashString);
#else
            return new Dictionary<HashString, valueType>();
#endif
        }
    }
    /// <summary>
    /// 创建字典
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public static class DictionaryCreator<keyType> where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(keyType).IsValueType;
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<keyType, valueType> Create<valueType>()
        {
#if __IOS__
            if (isValueType) return new Dictionary<keyType, valueType>(EqualityComparer.comparer<keyType>.Default);
#endif
            return new Dictionary<keyType, valueType>();
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<keyType, valueType> Create<valueType>(int capacity)
        {
#if __IOS__
            if (isValueType) return new Dictionary<keyType, valueType>(capacity, EqualityComparer.comparer<keyType>.Default);
#endif
            return new Dictionary<keyType, valueType>(capacity);
        }
    }
}
