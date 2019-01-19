using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 取消保持回调客户端命令
    /// </summary>
    internal sealed class CancelKeepCommand : Command
    {
        /// <summary>
        /// 命令信息
        /// </summary>
        private static readonly CommandInfo commandInfo = new CommandInfo { IsSendOnly = 1, MaxDataSize = sizeof(int) * 3 };
        /// <summary>
        /// 取消保持回调会话标识
        /// </summary>
        private int cancelCommandIndex;
        /// <summary>
        /// 取消保持回调客户端命令
        /// </summary>
        internal CancelKeepCommand()
        {
            CommandInfo = commandInfo;
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.ByteSize) >= sizeof(int) * 3)
            {
                CommandBase nextBuild = LinkNext;
                byte* write = stream.CurrentData;
                *(int*)write = Server.CancelKeepCommandIndex;
                *(int*)(write + sizeof(int)) = cancelCommandIndex;
                *(int*)(write + (sizeof(uint) + sizeof(int))) = 0;
                stream.ByteSize += sizeof(int) * 3;
                ++buildInfo.Count;

                Socket = null;
                LinkNext = null;
                AutoCSer.Threading.RingPool<CancelKeepCommand>.Default.PushNotNull(this);
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 取消保持回调客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="cancelCommandIndex">取消保持回调会话标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(ClientSocket socket, int cancelCommandIndex)
        {
            Socket = socket;
            this.cancelCommandIndex = cancelCommandIndex;
        }
    }
}
