using System;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpStreamServer.ServerOutput
{
    /// <summary>
    /// TCP 服务端套接字输出信息
    /// </summary>
    internal sealed class ReturnTypeOutput : OutputLink
    {
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal TcpServer.ReturnType ReturnType;
        /// <summary>
        /// 创建输出信息
        /// </summary>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="buildInfo">输出创建参数</param>
        internal unsafe override OutputLink Build(ServerSocketSenderBase sender, ref TcpServer.SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = sender.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.ByteSize) >= sizeof(int))
            {
                OutputLink nextBuild = LinkNext;
                *(int*)stream.CurrentData = (byte)ReturnType;
                LinkNext = null;
                stream.ByteSize += sizeof(int);
                AutoCSer.Threading.RingPool<ReturnTypeOutput>.Default.PushNotNull(this);
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 释放 TCP 服务端套接字输出信息
        /// </summary>
        /// <returns></returns>
        protected override OutputLink free()
        {
            OutputLink next = LinkNext;
            LinkNext = null;
            AutoCSer.Threading.RingPool<ReturnTypeOutput>.Default.PushNotNull(this);
            return next;
        }
    }
}
