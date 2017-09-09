using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 基本类型随机数创建函数
    /// </summary>
    internal sealed class CreateMethod : Attribute { }

    /// <summary>
    /// 随机对象生成函数信息
    /// </summary>
    internal static partial class MethodCache
    {
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static bool createBool()
        {
            return AutoCSer.Random.Default.NextBit() != 0;
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static byte CreateByte()
        {
            return AutoCSer.Random.Default.NextByte();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static sbyte CreateSByte()
        {
            return (sbyte)AutoCSer.Random.Default.NextByte();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static short CreateShort()
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return short.MinValue;
                case 1: return short.MaxValue;
            }
            return (short)AutoCSer.Random.Default.NextUShort();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static ushort CreateUShort()
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return ushort.MinValue;
                case 1: return ushort.MaxValue;
            }
            return AutoCSer.Random.Default.NextUShort();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static int CreateInt()
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return int.MinValue;
                case 1: return int.MaxValue;
            }
            return AutoCSer.Random.Default.Next();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static uint CreateUInt()
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return uint.MinValue;
                case 1: return uint.MaxValue;
            }
            return (uint)AutoCSer.Random.Default.Next();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static long CreateLong()
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return long.MinValue;
                case 1: return long.MaxValue;
            }
            return (long)AutoCSer.Random.Default.NextULong();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static ulong CreateULong()
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return ulong.MinValue;
                case 1: return ulong.MaxValue;
            }
            return AutoCSer.Random.Default.NextULong();
        }
        /// <summary>
        /// 随机数除数
        /// </summary>
        private static readonly decimal decimalDiv = 100;
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static decimal createDecimal()
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return decimal.MinValue;
                case 1: return decimal.MaxValue;
            }
            return (decimal)(long)AutoCSer.Random.Default.NextULong() / decimalDiv;
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        [CreateMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static System.Guid createGuid()
        {
            return System.Guid.NewGuid();
        }
    }
}
