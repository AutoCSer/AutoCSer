using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Emit
{
    /// <summary>
    /// 可空类型构造函数
    /// </summary>
    internal static class NullableConstructor
    {
        /// <summary>
        /// 可空类型构造函数
        /// </summary>
        internal static readonly Dictionary<Type, MethodInfo> Constructors;
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool? create(bool value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static byte? create(byte value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static char? create(char value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static DateTime? create(DateTime value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static decimal? create(decimal value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static double? create(double value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static float? create(float value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Guid? create(Guid value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static short? create(short value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static int? create(int value)
        {
            return value;
        }
        /// <summary>
        /// 创建可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static long? create(long value)
        {
            return value;
        }

        static NullableConstructor()
        {
            Constructors = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            Constructors.Add(typeof(bool), ((Func<bool, bool?>)create).Method);
            Constructors.Add(typeof(byte), ((Func<byte, byte?>)create).Method);
            Constructors.Add(typeof(char), ((Func<char, char?>)create).Method);
            Constructors.Add(typeof(DateTime), ((Func<DateTime, DateTime?>)create).Method);
            Constructors.Add(typeof(decimal), ((Func<decimal, decimal?>)create).Method);
            Constructors.Add(typeof(double), ((Func<double, double?>)create).Method);
            Constructors.Add(typeof(float), ((Func<float, float?>)create).Method);
            Constructors.Add(typeof(Guid), ((Func<Guid, Guid?>)create).Method);
            Constructors.Add(typeof(short), ((Func<short, short?>)create).Method);
            Constructors.Add(typeof(int), ((Func<int, int?>)create).Method);
            Constructors.Add(typeof(long), ((Func<long, long?>)create).Method);
        }
    }
}
