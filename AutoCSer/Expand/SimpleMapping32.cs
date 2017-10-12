using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 简单对称映射
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SimpleMapping32
    {
        /// <summary>
        /// 第一次运算乘数,必须为奇数
        /// </summary>
        private readonly uint multiplier1;
        /// <summary>
        /// 第一次异或操作数
        /// </summary>
        private readonly uint xor1;
        /// <summary>
        /// 第二次运算乘数,必须为奇数
        /// </summary>
        private readonly uint multiplier2;
        /// <summary>
        /// 第二次异或操作数
        /// </summary>
        private readonly uint xor2;
        /// <summary>
        /// 简单对称映射
        /// </summary>
        /// <param name="multiplier1">第一次运算乘数,必须为奇数</param>
        /// <param name="xor1">第一次异或操作数</param>
        /// <param name="multiplier2">第二次运算乘数,必须为奇数</param>
        /// <param name="xor2">第二次异或操作数</param>
        public SimpleMapping32(uint multiplier1, uint xor1, uint multiplier2, uint xor2)
        {
            this.multiplier1 = multiplier1 | 1;
            this.xor1 = xor1;
            this.multiplier2 = multiplier2 | 1;
            this.xor2 = xor2;
        }
        /// <summary>
        /// 映射编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public uint Encode(uint value)
        {
            return (((value * multiplier1) ^ xor1) * multiplier2) ^ xor2;
        }
        /// <summary>
        /// 映射解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public uint Decode(uint value)
        {
            return decode(decode(value ^ xor2, multiplier2) ^ xor1, multiplier1);
        }
        /// <summary>
        /// 乘法解码
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        private static uint decode(uint mod, uint multiplier)
        {
            uint value = 0, bit = 1;
            do
            {
                if ((mod & bit) != 0)
                {
                    value |= bit;
                    mod -= multiplier;
                }
                bit <<= 1;
                multiplier <<= 1;
            }
            while (bit != 0x80000000U);
            return mod == 0 ? value : (value | 0x80000000U);
        }
    }
}
