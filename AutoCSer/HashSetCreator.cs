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
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>HASH表</returns>
        public static HashSet<T> CreateOnly<T>() where T : class
        {
            return new HashSet<T>();
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>HASH表</returns>
        public static HashSet<T> CreateAny<T>()
        {
#if __IOS__
            return new HashSet<T>(AutoCSer.IOS.EqualityComparer<T>.Default);
#else
            return new HashSet<T>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        public static HashSet<int> CreateInt()
        {
#if __IOS__
            return new HashSet<int>(AutoCSer.IOS.IntComparer.Default);
#else
            return new HashSet<int>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        public static HashSet<SubString> CreateSubString()
        {
#if __IOS__
            return new HashSet<SubString>(AutoCSer.IOS.SubStringComparer.Default);
#else
            return new HashSet<SubString>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        public static HashSet<HashString> CreateHashString()
        {
#if __IOS__
            return new HashSet<HashString>(AutoCSer.IOS.HashStringComparer.Default);
#else
            return new HashSet<HashString>();
#endif
        }
    }
    /// <summary>
    /// 创建 HashSet表
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public static class HashSetCreator<T> where T : IEquatable<T>
    {
#if __IOS__
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(T).IsValueType;
#endif
        /// <summary>
        /// 创建 HashSet 表
        /// </summary>
        /// <returns>HashSet 表</returns>
        public static HashSet<T> Create()
        {
#if __IOS__
            if (isValueType) return new HashSet<T>(AutoCSer.IOS.EquatableComparer<T>.Default);
#endif
            return new HashSet<T>();
        }
    }
}
