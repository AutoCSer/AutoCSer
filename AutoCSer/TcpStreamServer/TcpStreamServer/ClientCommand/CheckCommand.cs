using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpStreamServer.ClientCommand
{
    /// <summary>
    /// 心跳检测命令
    /// </summary>
    internal sealed class CheckCommand : Command
    {
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override TcpServer.ClientCommand.CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if (stream.ByteSize == 0 && LinkNext == null)
            {
                *(int*)stream.CurrentData = Server.CheckCommandIndex | (int)(uint)TcpServer.CommandFlags.NullData;
                stream.ByteSize += sizeof(int);
            }
            Interlocked.Exchange(ref FreeLock, 1);
            IsBuildError = true;
            Socket = null;
            return LinkNext;
        }
        /// <summary>
        /// 释放 TCP 客户端命令
        /// </summary>
        protected override void free()
        {
            LinkNext = null;
            AutoCSer.Threading.RingPool<CheckCommand>.Default.PushNotNull(this);
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
