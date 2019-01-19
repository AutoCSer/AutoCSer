using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 心跳检测命令
    /// </summary>
    internal sealed class CheckCommand : Command
    {
        /// <summary>
        /// 命令信息
        /// </summary>
        private static readonly CommandInfo commandInfo = new CommandInfo { IsSendOnly = 1, MaxDataSize = sizeof(int) * 2 };
        /// <summary>
        /// 心跳检测命令
        /// </summary>
        private CheckCommand()
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
            CommandBase nextBuild = LinkNext;
            if (buildInfo.Count == 0 && nextBuild == null)
            {
                UnmanagedStream stream = Socket.OutputSerializer.Stream;
                buildInfo.Count = 1;
                byte* write = stream.CurrentData;
                Socket = null;
                *(int*)write = Server.CheckCommandIndex;
                //*(uint*)(write + sizeof(int)) |= (uint)(CommandFlags.NullData | CommandFlags.NullIndex);
                *(uint*)(write + sizeof(int)) |= (uint)CommandFlags.NullData;
                stream.ByteSize += sizeof(int) + sizeof(uint);
                AutoCSer.Threading.RingPool<CheckCommand>.Default.PushNotNull(this);
            }
            else LinkNext = null;
            return nextBuild;
        }
        /// <summary>
        /// 获取心跳检测命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <returns>心跳检测命令</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static CheckCommand Get(ClientSocket socket)
        {
            CheckCommand command = AutoCSer.Threading.RingPool<CheckCommand>.Default.Pop();
            if (command == null)
            {
                try
                {
                    command = new CheckCommand();
                }
                catch (Exception error)
                {
                    socket.Log.Add(AutoCSer.Log.LogType.Debug, error);
                    return null;
                }
            }
            command.Socket = socket;
            return command;
        }
    }
}
