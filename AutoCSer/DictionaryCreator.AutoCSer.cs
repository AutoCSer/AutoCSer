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
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<long, valueType> CreateLong<valueType>()
        {
#if __IOS__
            return new Dictionary<long, valueType>(EqualityComparer.Long);
#else
            return new Dictionary<long, valueType>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<short, valueType> CreateShort<valueType>()
        {
#if __IOS__
            return new Dictionary<short, valueType>(EqualityComparer.Short);
#else
            return new Dictionary<short, valueType>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<char, valueType> CreateChar<valueType>()
        {
#if __IOS__
            return new Dictionary<char, valueType>(EqualityComparer.Char);
#else
            return new Dictionary<char, valueType>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<HashString, valueType> CreateHashString<valueType>(int capacity)
        {
#if __IOS__
            return new Dictionary<HashString, valueType>(capacity, EqualityComparer.HashString);
#else
            return new Dictionary<HashString, valueType>(capacity);
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<HashBytes, valueType> CreateHashBytes<valueType>()
        {
#if __IOS__
            return new Dictionary<HashBytes, valueType>(EqualityComparer.HashBytes);
#else
            return new Dictionary<HashBytes, valueType>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<HashBytes, valueType> CreateHashBytes<valueType>(int capacity)
        {
#if __IOS__
            return new Dictionary<HashBytes, valueType>(capacity, EqualityComparer.HashBytes);
#else
            return new Dictionary<HashBytes, valueType>(capacity);
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<SubString, valueType> CreateSubString<valueType>()
        {
#if __IOS__
            return new Dictionary<SubString, valueType>(EqualityComparer.SubString);
#else
            return new Dictionary<SubString, valueType>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Dictionary<AutoCSer.Net.HostPort, valueType> CreateHostPort<valueType>()
        {
#if __IOS__
            return new Dictionary<AutoCSer.Net.HostPort, valueType>(EqualityComparer.HostPort);
#else
            return new Dictionary<AutoCSer.Net.HostPort, valueType>();
#endif
        }
    }
}
