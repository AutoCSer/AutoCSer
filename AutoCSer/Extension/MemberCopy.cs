using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 成员复制扩展操作
    /// </summary>
    public static partial class MemberCopy
    {
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="baseType">复制成员对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType copy<valueType, baseType>(this baseType value, AutoCSer.Metadata.MemberMap<baseType> memberMap = null)
            where valueType : class, baseType
        {
            if (value == null) return null;
            valueType newValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<baseType>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="baseType">复制成员对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType copy<valueType, baseType>(this valueType value, AutoCSer.Metadata.MemberMap<baseType> memberMap)
            where valueType : class, baseType
        {
            if (value == null) return null;
            valueType newValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<baseType>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType copy<valueType>(this valueType value, AutoCSer.Metadata.MemberMap<valueType> memberMap = null)
            where valueType : class
        {
            if (value == null) return null;
            valueType newValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<valueType>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="readValue">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyForm<valueType>(this valueType value, valueType readValue, AutoCSer.Metadata.MemberMap<valueType> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<valueType>.Copy(ref value, readValue, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="readValue">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyForm<valueType>(this valueType value, ref valueType readValue, AutoCSer.Metadata.MemberMap<valueType> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<valueType>.Copy(ref value, readValue, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="writeValue">目标对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyTo<valueType>(this valueType value, valueType writeValue, AutoCSer.Metadata.MemberMap<valueType> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<valueType>.Copy(ref writeValue, value, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="writeValue">目标对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void copyTo<valueType>(this valueType value, ref valueType writeValue, AutoCSer.Metadata.MemberMap<valueType> memberMap = null)
        {
            AutoCSer.MemberCopy.Copyer<valueType>.Copy(ref writeValue, value, memberMap);
        }
        /// <summary>
        /// 对象浅复制
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType memberwiseClone<valueType>(this valueType value)
        {
            return AutoCSer.MemberCopy.Copyer<valueType>.MemberwiseClone(value);
        }
    }
}
