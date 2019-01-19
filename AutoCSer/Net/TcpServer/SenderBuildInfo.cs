using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 输出创建参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = sizeof(int) * 4)]
    internal struct SenderBuildInfo
    {
        /// <summary>
        /// 发送数据缓冲区字节大小
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal int SendBufferSize;
        /// <summary>
        /// 当前创建命令数量
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(4)]
        internal int Count;
        /// <summary>
        /// 是否需要发送数据
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(8)]
        internal int IsSend;
        /// <summary>
        /// 数据是否需要发送数据
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(8)]
        internal byte isFullSend;
        /// <summary>
        /// 是否存在验证函数
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(9)]
        internal bool IsVerifyMethod;
        /// <summary>
        /// 是否需要关闭
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(10)]
        internal bool IsClose;
        /// <summary>
        /// 是否错误
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(12)]
        internal bool IsError;
        /// <summary>
        /// 是否创建了新的缓冲区
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(13)]
        internal byte IsNewBuffer;
        /// <summary>
        /// 发送数据量过低次数
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(14)]
        internal byte SendSizeLessCount;
        /// <summary>
        /// 客户端是否更注重 await
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(15)]
        internal bool IsClientAwaiter;
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            Count = IsSend = 0;
        }
    }
}
