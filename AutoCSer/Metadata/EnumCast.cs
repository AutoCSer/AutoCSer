using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 枚举值转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="UT"></typeparam>
    internal static class EnumCast<T, UT>
        where UT : struct, IConvertible
    {
#if NOJIT
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static UT ToInt(T value)
        {
            return (UT)(object)value;
        }
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T FromInt(UT value)
        {
            return (T)(object)value;
        }
#else
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        public static readonly Func<T, UT> ToInt;
        /// <summary>
        /// 数字转枚举委托
        /// </summary>
        public static readonly Func<UT, T> FromInt;

        static EnumCast()
        {
            EnumGenericType EnumGenericType = EnumGenericType.Get(typeof(T));
            ToInt = (Func<T, UT>)EnumGenericType.ToIntDelegate;
            FromInt = (Func<UT, T>)EnumGenericType.FromIntDelegate;
        }
#endif
    }
}
