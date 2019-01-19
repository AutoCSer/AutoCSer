using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端命令信息
    /// </summary>
    public sealed class CommandInfo : TcpServer.CommandInfoBase
    {
        /// <summary>
        /// 是否保持异步回调
        /// </summary>
        public byte IsKeepCallback;
        /// <summary>
        /// 客户端是否仅仅发送数据(不需要应答)
        /// </summary>
        public byte IsSendOnly;
        /// <summary>
        /// 任务类型
        /// </summary>
        public ClientTaskType TaskType;

        /// <summary>
        /// 最大命令数据字节长度
        /// </summary>
        internal int MaxDataSize;
        /// <summary>
        /// 更新最大命令数据字节长度
        /// </summary>
        /// <param name="size">命令数据字节长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CheckMaxDataSize(int size)
        {
            if (size > MaxDataSize) MaxDataSize = size;
        }
    }
}
