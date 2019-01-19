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
            *(char*)data = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte3 >> 4));
            *(char*)(data + 1) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte3 & 15));
            *(char*)(data + 2) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte2 >> 4));
            *(char*)(data + 3) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte2 & 15));
            *(char*)(data + 4) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte1 >> 4));
            *(char*)(data + 5) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte1 & 15));
            *(char*)(data + 6) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte0 >> 4));
            *(char*)(data + 7) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte0 & 15));
            *(char*)(data + 8) = '-';
            *(char*)(data + 9) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte5 >> 4));
            *(char*)(data + 10) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte5 & 15));
            *(char*)(data + 11) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte4 >> 4));
            *(char*)(data + 12) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte4 & 15));
            *(char*)(data + 13) = '-';
            *(char*)(data + 14) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte7 >> 4));
            *(char*)(data + 15) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte7 & 15));
            *(char*)(data + 16) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte6 >> 4));
            *(char*)(data + 17) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte6 & 15));
            *(char*)(data + 18) = '-';
            *(char*)(data + 19) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte8 >> 4));
            *(char*)(data + 20) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte8 & 15));
            *(char*)(data + 21) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte9 >> 4));
            *(char*)(data + 22) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte9 & 15));
            *(char*)(data + 23) = '-';
            *(char*)(data + 24) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte10 >> 4));
            *(char*)(data + 25) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte10 & 15));
            *(char*)(data + 26) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte11 >> 4));
            *(char*)(data + 27) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte11 & 15));
            *(char*)(data + 28) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte12 >> 4));
            *(char*)(data + 29) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte12 & 15));
            *(char*)(data + 30) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte13 >> 4));
            *(char*)(data + 31) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte13 & 15));
            *(char*)(data + 32) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte14 >> 4));
            *(char*)(data + 33) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte14 & 15));
            *(char*)(data + 34) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte15 >> 4));
            *(char*)(data + 35) = (char)AutoCSer.Extension.Number.ToHex((uint)(Byte15 & 15));
        }
    }
}
