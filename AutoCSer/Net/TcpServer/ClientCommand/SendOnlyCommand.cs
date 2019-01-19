using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
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
        internal void Set(ClientSocket socket, CommandInfo command)
        {
            Socket = socket;
            CommandInfo = command;
            //CommandIndex = (uint)(CommandFlags.NullIndex | CommandFlags.NullData);
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.ByteSize) >= sizeof(int) + sizeof(uint))
            {
                byte* write = stream.CurrentData;
                CommandBase nextBuild = LinkNext;
                *(int*)write = CommandInfo.Command;
                //*(uint*)(write + sizeof(int)) = (CommandIndex & Server.CommandIndexAnd) | (uint)(CommandInfo.CommandFlags | CommandFlags.NullData);
                *(uint*)(write + sizeof(int)) = (uint)(CommandInfo.CommandFlags | CommandFlags.NullData);
                ++buildInfo.Count;
                stream.ByteSize += sizeof(int) + sizeof(uint);

                Socket = null;
                LinkNext = null;
                AutoCSer.Threading.RingPool<SendOnlyCommand>.Default.PushNotNull(this);
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
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
            //CommandIndex = (uint)CommandFlags.NullIndex;
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if (buildInfo.Count == 0 || (buildInfo.SendBufferSize - stream.ByteSize) >= CommandInfo.MaxDataSize)
            {
                int streamLength = stream.ByteSize;
                stream.PrepLength(sizeof(uint) + sizeof(int) * 3);
                CommandBase nextBuild = LinkNext;
                stream.ByteSize += sizeof(uint) + sizeof(int) * 2;
                if ((CommandInfo.CommandFlags & CommandFlags.JsonSerialize) == 0) Socket.Serialize(CommandInfo, ref InputParameter);
                else Socket.JsonSerialize(ref InputParameter);
                int dataLength = stream.ByteSize - streamLength - (sizeof(int) * 2 + sizeof(uint));
                if (dataLength <= Socket.MaxInputSize)
                {
                    byte* write = stream.Data.Byte + streamLength;
                    buildInfo.IsVerifyMethod |= CommandInfo.IsVerifyMethod;
                    ++buildInfo.Count;
                    *(int*)write = CommandInfo.Command;
                    //*(uint*)(write + sizeof(int)) = (CommandIndex & Server.CommandIndexAnd) | (uint)CommandInfo.CommandFlags;
                    *(uint*)(write + sizeof(int)) = (uint)CommandInfo.CommandFlags;
                    *(int*)(write + (sizeof(uint) + sizeof(int))) = dataLength;
                    CommandInfo.CheckMaxDataSize(Math.Max(dataLength + (sizeof(int) * 2 + sizeof(uint)), stream.LastPrepSize - streamLength));
                }
                else stream.ByteSize = streamLength;

                InputParameter = default(inputParameterType);
                Socket = null;
                LinkNext = null;
                AutoCSer.Threading.RingPool<SendOnlyCommand<inputParameterType>>.Default.PushNotNull(this);
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
    }
}
