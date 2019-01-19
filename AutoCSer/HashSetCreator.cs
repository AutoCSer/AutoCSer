using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 创建 HashSet
    /// </summary>
    public static class HashSetCreator
    {
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>HASH表</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static HashSet<valueType> CreateOnly<valueType>() where valueType : class
        {
            return new HashSet<valueType>();
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>HASH表</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static HashSet<valueType> CreateAny<valueType>()
        {
#if __IOS__
            return new HashSet<valueType>(EqualityComparer.comparer<valueType>.Default);
#else
            return new HashSet<valueType>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static HashSet<int> CreateInt()
        {
#if __IOS__
            return new HashSet<int>(EqualityComparer.Int);
#else
            return new HashSet<int>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static HashSet<SubString> CreateSubString()
        {
#if __IOS__
            return new HashSet<SubString>(EqualityComparer.SubString);
#else
            return new HashSet<SubString>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static HashSet<HashString> CreateHashString()
        {
#if __IOS__
            return new HashSet<HashString>(EqualityComparer.HashString);
#else
            return new HashSet<HashString>();
#endif
        }
    }
    /// <summary>
    /// 创建 HashSet
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public static class HashSetCreator<valueType>
    {
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(valueType).IsValueType;
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static HashSet<valueType> Create()
        {
#if __IOS__
            if (isValueType) return new HashSet<valueType>(EqualityComparer.Comparer<valueType>.Default);
#endif
            return new HashSet<valueType>();
        }
    }
}
