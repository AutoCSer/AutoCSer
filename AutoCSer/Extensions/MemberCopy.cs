using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 成员复制扩展操作
    /// </summary>
    public static partial class MemberCopy
    {
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="VT">对象类型</typeparam>
        /// <typeparam name="BT">复制成员对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static VT copy<VT, BT>(this BT value, AutoCSer.Metadata.MemberMap<BT> memberMap = null)
            where VT : class, BT
        {
            if (value == null) return null;
            VT newValue = AutoCSer.Metadata.DefaultConstructor<VT>.Constructor();
            AutoCSer.MemberCopy.Copyer<BT>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="VT">对象类型</typeparam>
        /// <typeparam name="BT">复制成员对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static VT copy<VT, BT>(this VT value, AutoCSer.Metadata.MemberMap<BT> memberMap)
            where VT : class, BT
        {
            if (value == null) return null;
            VT newValue = AutoCSer.Metadata.DefaultConstructor<VT>.Constructor();
            AutoCSer.MemberCopy.Copyer<BT>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T copy<T>(this T value, AutoCSer.Metadata.MemberMap<T> memberMap = null)
            where T : class
        {
            if (value == null) return null;
            T newValue = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            AutoCSer.MemberCopy.Copyer<T>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="readValue">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyForm<T>(this T value, T readValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<T>.Copy(ref value, readValue, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="readValue">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyForm<T>(this T value, ref T readValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<T>.Copy(ref value, readValue, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="writeValue">目标对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyTo<T>(this T value, T writeValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<T>.Copy(ref writeValue, value, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="writeValue">目标对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyTo<T>(this T value, ref T writeValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<T>.Copy(ref writeValue, value, memberMap);
        }
        /// <summary>
        /// 对象浅复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T memberwiseClone<T>(this T value)
        {
            return AutoCSer.MemberCopy.Copyer<T>.MemberwiseClone(value);
        }
    }
}
