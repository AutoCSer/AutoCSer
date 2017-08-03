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
