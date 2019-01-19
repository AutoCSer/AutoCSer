using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 自定义数据命令
    /// </summary>
    internal sealed class CustomDataCommand : Command
    {
        /// <summary>
        /// 命令信息
        /// </summary>
        private static readonly CommandInfo commandInfo = new CommandInfo { IsSendOnly = 1, MaxDataSize = sizeof(int) * 3 };

        /// <summary>
        /// 自定义数据
        /// </summary>
        private SubArray<byte> data;
        /// <summary>
        /// 服务端自定义数据回调命令
        /// </summary>
        /// <param name="socket">TCP 客户端套接字</param>
        internal CustomDataCommand(ClientSocket socket)
        {
            Socket = socket;
            CommandInfo = commandInfo;
        }
        /// <summary>
        /// 服务端自定义数据回调命令
        /// </summary>
        /// <param name="socket">TCP 客户端套接字</param>
        /// <param name="commandInfo">命令信息</param>
        internal CustomDataCommand(ClientSocket socket, CommandInfo commandInfo)
        {
            Socket = socket;
            CommandInfo = commandInfo;
        }
        /// <summary>
        /// 自定义数据命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="data">自定义数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, byte[] data)
        {
            Socket = socket;
            this.data.Set(data, 0, data.Length);
        }
        /// <summary>
        /// 自定义数据命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="data">自定义数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, ref SubArray<byte> data)
        {
            Socket = socket;
            this.data = data;
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            int dataSize = (data.Length + 3) & (int.MaxValue), prepLength = dataSize + (sizeof(uint) + sizeof(int) * 2);
            if (buildInfo.Count == 0 || (buildInfo.SendBufferSize - stream.ByteSize) >= prepLength
                || (stream.FreeSize >= prepLength && prepLength > buildInfo.SendBufferSize - TcpServer.ClientCommand.Command.StreamStartIndex))
            {
                CommandBase nextBuild = LinkNext;
                stream.PrepLength(prepLength);
                byte* write = stream.CurrentData;
                *(int*)write = Server.CustomDataCommandIndex;
                *(uint*)(write + sizeof(int)) = (uint)data.Length;
                *(int*)(write + (sizeof(uint) + sizeof(int))) = dataSize;
                if (data.Length != 0)
                {
                    fixed (byte* dataFixed = data.Array) Memory.CopyNotNull(dataFixed + data.Start, write + (sizeof(uint) + sizeof(int) * 2), data.Length);
                }
                stream.ByteSize += dataSize + (sizeof(uint) + sizeof(int) * 2);
                ++buildInfo.Count;

                data.Array = null;
                Socket = null;
                LinkNext = null;
                AutoCSer.Threading.RingPool<CustomDataCommand>.Default.PushNotNull(this);
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
    }
}
