using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 空闲字节数
        /// </summary>
        public int FreeSize
        {
            get { return Data.ByteSize - ByteSize; }
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size"></param>
        public void MoveSize(int size)
        {
            int byteSize = ByteSize + size;
            if (byteSize > Data.ByteSize) throw new IndexOutOfRangeException("ByteSize[" + ByteSize.toString() + "] + size[" + size.toString() + "] > Data.ByteSize[" + Data.ByteSize.toString() + "]");
            if (byteSize < 0) throw new IndexOutOfRangeException("ByteSize[" + ByteSize.toString() + "] + size[" + size.toString() + "] < 0");
            ByteSize = byteSize;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe void Write(SubString value)
        {
            if (value.Length != 0) WriteNotEmpty(ref value);
        }
        /// <summary>
        /// 写字符串集合
        /// </summary>
        /// <param name="values">字符串集合</param>
        public unsafe void Write(params string[] values)
        {
            if (values != null)
            {
                int length = 0;
                foreach (string value in values)
                {
                    if (value != null) length += value.Length;
                }
                prepSize(length <<= 1);
                byte* write = Data.Byte + ByteSize;
                foreach (string value in values)
                {
                    if (value != null)
                    {
                        StringExtension.CopyNotNull(value, write);
                        write += value.Length << 1;
                    }
                }
                ByteSize += length;
            }
        }
    }
}
