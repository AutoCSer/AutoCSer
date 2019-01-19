using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 指针(因为指针无法静态初始化)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 带长度的指针
        /// </summary>
        public partial struct Size
        {
            /// <summary>
            /// 字节长度
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public int GetSize()
            {
                return ByteSize;
            }
        }
    }
}
