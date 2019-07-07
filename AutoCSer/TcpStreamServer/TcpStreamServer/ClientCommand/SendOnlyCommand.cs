using System;
using AutoCSer.Net.TcpServer;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpStreamServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    internal sealed class SendOnlyCommand : Command
    {
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, TcpServer.CommandInfo command)
        {
            Socket = socket;
            CommandInfo = command;
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override TcpServer.ClientCommand.CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.ByteSize) >= sizeof(int))
            {
                *(int*)stream.CurrentData = CommandInfo.Command | (int)(uint)(CommandInfo.CommandFlags | CommandFlags.NullData);
                stream.ByteSize += sizeof(int);
                Socket = null;
                Interlocked.Exchange(ref FreeLock, 1);
                IsBuildError = true;
                return LinkNext;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 释放 TCP 客户端命令
        /// </summary>
        protected override void free()
        {
            LinkNext = null;
            AutoCSer.Threading.RingPool<SendOnlyCommand>.Default.PushNotNull(this);
        }
    }
    /// <summary>
    /// 客户端命令
    /// </summary>
    /// <typeparam name="inputParameterType">输入参数类型</typeparam>
    internal sealed class SendOnlyCommand<inputParameterType> : Command
        where inputParameterType : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        internal inputParameterType InputParameter;
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, ref inputParameterType inputParameter)
        {
            Socket = socket;
            CommandInfo = command;
            InputParameter = inputParameter;
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override TcpServer.ClientCommand.CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            int streamLength = stream.ByteSize;
            if (streamLength == 0 || (buildInfo.SendBufferSize - stream.ByteSize) >= CommandInfo.MaxDataSize)
            {
                stream.PrepLength(sizeof(int) * 3);
                stream.ByteSize += sizeof(int) * 2;
                if ((CommandInfo.CommandFlags & CommandFlags.JsonSerialize) == 0) Socket.Serialize(CommandInfo, ref InputParameter);
                else Socket.JsonSerialize(ref InputParameter);
                int dataLength = stream.ByteSize - streamLength - sizeof(int) * 2;
                if (dataLength <= Socket.MaxInputSize)
                {
                    ulong markData = Socket.Sender.SendMarkData;
                    byte* write = stream.Data.Byte + streamLength;
                    buildInfo.IsVerifyMethod |= CommandInfo.IsVerifyMethod;
                    *(int*)write = CommandInfo.Command | (int)(uint)CommandInfo.CommandFlags;
                    *(int*)(write + sizeof(int)) = dataLength;
                    if (markData != 0) TcpServer.CommandBase.Mark(write + sizeof(int) * 2, markData, dataLength);
                    CommandInfo.CheckMaxDataSize(Math.Max(dataLength + sizeof(int) * 2, stream.LastPrepSize - streamLength));
                }
                else stream.ByteSize = streamLength;

                InputParameter = default(inputParameterType);
                Socket = null;
                Interlocked.Exchange(ref FreeLock, 1);
                IsBuildError = true;
                return LinkNext;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 释放 TCP 客户端命令
        /// </summary>
        protected override void free()
        {
            LinkNext = null;
            AutoCSer.Threading.RingPool<SendOnlyCommand<inputParameterType>>.Default.PushNotNull(this);
        }
    }
}
