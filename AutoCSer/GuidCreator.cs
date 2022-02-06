using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Guid 联合体
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal partial struct GuidCreator
    {
        /// <summary>
        /// Guid
        /// </summary>
        [FieldOffset(0)]
        public System.Guid Value;

        [FieldOffset(0)]
        internal byte Byte0;
        [FieldOffset(1)]
        internal byte Byte1;
        [FieldOffset(2)]
        internal byte Byte2;
        [FieldOffset(3)]
        internal byte Byte3;
        [FieldOffset(4)]
        internal byte Byte4;
        [FieldOffset(5)]
        internal byte Byte5;
        [FieldOffset(4)]
        internal ushort Byte45;
        [FieldOffset(6)]
        internal byte Byte6;
        [FieldOffset(7)]
        internal byte Byte7;
        [FieldOffset(6)]
        internal ushort Byte67;
        [FieldOffset(8)]
        internal byte Byte8;
        [FieldOffset(9)]
        internal byte Byte9;
        [FieldOffset(10)]
        internal byte Byte10;
        [FieldOffset(11)]
        internal byte Byte11;
        [FieldOffset(12)]
        internal byte Byte12;
        [FieldOffset(13)]
        internal byte Byte13;
        [FieldOffset(14)]
        internal byte Byte14;
        [FieldOffset(15)]
        internal byte Byte15;

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="data"></param>
        internal unsafe void ToString(char* data)
        {
            *data = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte3 >> 4));
            *(data + 1) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte3 & 15));
            *(data + 2) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte2 >> 4));
            *(data + 3) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte2 & 15));
            *(data + 4) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte1 >> 4));
            *(data + 5) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte1 & 15));
            *(data + 6) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte0 >> 4));
            *(data + 7) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte0 & 15));
            *(data + 8) = '-';
            *(data + 9) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte5 >> 4));
            *(data + 10) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte5 & 15));
            *(data + 11) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte4 >> 4));
            *(data + 12) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte4 & 15));
            *(data + 13) = '-';
            *(data + 14) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte7 >> 4));
            *(data + 15) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte7 & 15));
            *(data + 16) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte6 >> 4));
            *(data + 17) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte6 & 15));
            *(data + 18) = '-';
            *(data + 19) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte8 >> 4));
            *(data + 20) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte8 & 15));
            *(data + 21) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte9 >> 4));
            *(data + 22) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte9 & 15));
            *(data + 23) = '-';
            *(data + 24) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte10 >> 4));
            *(data + 25) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte10 & 15));
            *(data + 26) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte11 >> 4));
            *(data + 27) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte11 & 15));
            *(data + 28) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte12 >> 4));
            *(data + 29) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte12 & 15));
            *(data + 30) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte13 >> 4));
            *(data + 31) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte13 & 15));
            *(data + 32) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte14 >> 4));
            *(data + 33) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte14 & 15));
            *(data + 34) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte15 >> 4));
            *(data + 35) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)(Byte15 & 15));
        }
    }
}
