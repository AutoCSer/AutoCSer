using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer.ServerOutput
{
    /// <summary>
    /// TCP 服务端套接字输出信息
    /// </summary>
    /// <typeparam name="outputParameterType">输出数据类型</typeparam>
    internal unsafe sealed class Output<outputParameterType> : OutputLink
        where outputParameterType : struct
    {
        /// <summary>
        /// 服务端输出信息
        /// </summary>
        private TcpServer.OutputInfo outputInfo;
        /// <summary>
        /// 输出参数
        /// </summary>
        private outputParameterType outputParameter;
        /// <summary>
        /// 创建输出信息
        /// </summary>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="buildInfo">输出创建参数</param>
        internal override OutputLink Build(TcpServer.ServerSocketSenderBase sender, ref TcpServer.SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = sender.OutputSerializer.Stream;
            if (stream.ByteSize == 0 || (buildInfo.SendBufferSize - stream.ByteSize) >= outputInfo.MaxDataSize)
            {
                ulong markData = sender.ServerSocket.MarkData;
                int streamLength = stream.ByteSize;
                OutputLink nextBuild = LinkNext;
                stream.PrepLength(sizeof(int) * 2);
                stream.ByteSize += sizeof(int);
                if (outputInfo.IsJsonSerialize == 0) sender.Serialize(outputInfo, ref outputParameter);
                else sender.JsonSerialize(ref outputParameter);
                int dataLength = stream.ByteSize - streamLength - sizeof(int);
                byte* dataStart = stream.Data.Byte + streamLength;
                *(int*)dataStart = dataLength + OutputDataSize;
                if (markData != 0) TcpServer.CommandBase.Mark(dataStart + sizeof(int), markData, dataLength);
                outputInfo.CheckMaxDataSize(Math.Max(dataLength + sizeof(int), stream.LastPrepSize - streamLength));
                LinkNext = null;
                outputParameter = default(outputParameterType);
                AutoCSer.Threading.RingPool<Output<outputParameterType>>.Default.PushNotNull(this);
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 设置输出参数
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
        {
            this.outputInfo = outputInfo;
            this.outputParameter = outputParameter;
        }
        /// <summary>
        /// 释放 TCP 服务端套接字输出信息
        /// </summary>
        /// <returns></returns>
        protected override OutputLink free()
        {
            OutputLink next = LinkNext;
            outputParameter = default(outputParameterType);
            LinkNext = null;
            AutoCSer.Threading.RingPool<Output<outputParameterType>>.Default.PushNotNull(this);
            return next;
        }
    }
}
