using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 参数数据 联合体
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(ulong))]
    internal struct Bit64
    {
        /// <summary>
        /// ulong
        /// </summary>
        [FieldOffset(0)]
        internal ulong ULong;
        /// <summary>
        /// long
        /// </summary>
        [FieldOffset(0)]
        internal long Long;
        /// <summary>
        /// uint
        /// </summary>
        [FieldOffset(0)]
        internal uint UInt;
        /// <summary>
        /// int
        /// </summary>
        [FieldOffset(0)]
        internal int Int;
        /// <summary>
        /// ushort
        /// </summary>
        [FieldOffset(0)]
        internal ushort UShort;
        /// <summary>
        /// short
        /// </summary>
        [FieldOffset(0)]
        internal short Short;
        /// <summary>
        /// byte
        /// </summary>
        [FieldOffset(0)]
        internal byte Byte;
        /// <summary>
        /// sbyte
        /// </summary>
        [FieldOffset(0)]
        internal sbyte SByte;
        /// <summary>
        /// char
        /// </summary>
        [FieldOffset(0)]
        internal char Char;
        /// <summary>
        /// bool
        /// </summary>
        [FieldOffset(0)]
        internal bool Bool;
        /// <summary>
        /// float
        /// </summary>
        [FieldOffset(0)]
        internal float Float;
        /// <summary>
        /// double
        /// </summary>
        [FieldOffset(0)]
        internal double Double;
        /// <summary>
        /// DateTime
        /// </summary>
        [FieldOffset(0)]
        internal DateTime DateTime;

        /// <summary>
        /// 起始位置
        /// </summary>
        [FieldOffset(0)]
        internal int Index;
        /// <summary>
        /// 数据长度
        /// </summary>
        [FieldOffset(sizeof(int))]
        internal int Length;
        ///// <summary>
        ///// 哈希值
        ///// </summary>
        //internal int HashCode
        //{
        //    get { return Index ^ Length; }
        //}
        /// <summary>
        /// 设置数据起始位置与长度
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="length">数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int index, int length)
        {
            Index = index;
            Length = length;
        }
        ///// <summary>
        ///// 设置数据起始位置
        ///// </summary>
        ///// <param name="index">起始位置</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void SetIndex(int index)
        //{
        //    Index = index;
        //    Length = 0;
        //}
        ///// <summary>
        ///// 设置数据长度
        ///// </summary>
        ///// <param name="length">数据长度</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void SetLength(int length)
        //{
        //    Index = 0;
        //    Length = length;
        //}
    }
}
