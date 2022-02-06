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
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> CreateOnly<KT, VT>()
             where KT : class
        {
            return new Dictionary<KT, VT>();
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> CreateAny<KT, VT>()
        {
#if __IOS__
            return new Dictionary<KT, VT>(AutoCSer.IOS.EqualityComparer<KT>.Default);
#else
            return new Dictionary<KT, VT>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> CreateAny<KT, VT>(int capacity)
        {
#if __IOS__
            return new Dictionary<KT, VT>(capacity, AutoCSer.IOS.EqualityComparer<KT>.Default);
#else
            return new Dictionary<KT, VT>(capacity);
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<int, T> CreateInt<T>()
        {
#if __IOS__
            return new Dictionary<int, T>(AutoCSer.IOS.IntComparer.Default);
#else
            return new Dictionary<int, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<HashString, T> CreateHashString<T>()
        {
#if __IOS__
            return new Dictionary<HashString, T>(AutoCSer.IOS.HashStringComparer.Default);
#else
            return new Dictionary<HashString, T>();
#endif
        }
    }
    /// <summary>
    /// 创建字典
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    public static class DictionaryCreator<KT> where KT : IEquatable<KT>
    {
#if __IOS__
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(KT).IsValueType;
#endif
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> Create<VT>()
        {
#if __IOS__
            if (isValueType) return new Dictionary<KT, VT>(AutoCSer.IOS.EquatableComparer<KT>.Default);
#endif
            return new Dictionary<KT, VT>();
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> Create<VT>(int capacity)
        {
#if __IOS__
            if (isValueType) return new Dictionary<KT, VT>(capacity, AutoCSer.IOS.EquatableComparer<KT>.Default);
#endif
            return new Dictionary<KT, VT>(capacity);
        }
    }
}
