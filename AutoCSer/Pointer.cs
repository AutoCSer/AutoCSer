using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 指针(因为指针无法静态初始化)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe partial struct Pointer : IEquatable<Pointer>
    {
        /// <summary>
        /// 带长度的指针
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public partial struct Size
        {
            /// <summary>
            /// 指针
            /// </summary>
            internal void* Data;
            /// <summary>
            /// 指针
            /// </summary>
            public Pointer Pointer
            {
                get { return new Pointer { Data = Data }; }
            }
            /// <summary>
            /// 字节长度
            /// </summary>
            internal int ByteSize;
            /// <summary>
            /// 字节长度
            /// </summary>
            public int DataSize
            {
                get { return ByteSize; }
            }
            /// <summary>
            /// 字节指针
            /// </summary>
            public byte* Byte
            {
                get { return (byte*)Data; }
            }
            /// <summary>
            /// 字符指针
            /// </summary>
            public char* Char
            {
                get { return (char*)Data; }
            }
            /// <summary>
            /// 整形指针
            /// </summary>
            public int* Int
            {
                get { return (int*)Data; }
            }
            /// <summary>
            /// 整形指针
            /// </summary>
            public uint* UInt
            {
                get { return (uint*)Data; }
            }
            /// <summary>
            /// 整形指针
            /// </summary>
            public ulong* ULong
            {
                get { return (ulong*)Data; }
            }
            /// <summary>
            /// 清空数据
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void SetNull()
            {
                Data = null;
                ByteSize = 0;
            }
            /// <summary>
            /// 设置指针数据
            /// </summary>
            /// <param name="data"></param>
            /// <param name="size"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(void* data, int size)
            {
                Data = data;
                ByteSize = size;
            }
        }
        /// <summary>
        /// 指针
        /// </summary>
        internal void* Data;
        /// <summary>
        /// 字节指针
        /// </summary>
        public byte* Byte
        {
            get { return (byte*)Data; }
        }
        /// <summary>
        /// 字节指针
        /// </summary>
        public sbyte* SByte
        {
            get { return (sbyte*)Data; }
        }
        /// <summary>
        /// 字符指针
        /// </summary>
        public char* Char
        {
            get { return (char*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public short* Short
        {
            get { return (short*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public ushort* UShort
        {
            get { return (ushort*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public int* Int
        {
            get { return (int*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public uint* UInt
        {
            get { return (uint*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public long* Long
        {
            get { return (long*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public ulong* ULong
        {
            get { return (ulong*)Data; }
        }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public override int GetHashCode()
        {
            return (int)((uint)((ulong)Data >> 3) ^ (uint)((ulong)Data >> 35));
        }
        /// <summary>
        /// 指针比较
        /// </summary>
        /// <param name="obj">待比较指针</param>
        /// <returns>指针是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((Pointer)obj);
        }
        /// <summary>
        /// 指针比较
        /// </summary>
        /// <param name="other">待比较指针</param>
        /// <returns>指针是否相等</returns>
        public bool Equals(Pointer other)
        {
            return Data == other.Data;
        }
    }
}
