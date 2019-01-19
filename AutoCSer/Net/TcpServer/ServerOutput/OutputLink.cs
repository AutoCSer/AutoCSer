using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ServerOutput
{
    /// <summary>
    /// TCP 服务端套接字输出信息
    /// </summary>
    /// <typeparam name="outputLinkType">TCP 服务端套接字输出信息类型</typeparam>
    internal abstract class OutputLink<outputLinkType> : AutoCSer.Threading.Link<outputLinkType>
        where outputLinkType : OutputLink<outputLinkType>
    {
        /// <summary>
        /// 输出流起始位置
        /// </summary>
        internal const int StreamStartIndex = sizeof(uint) + sizeof(int);
        /// <summary>
        /// 设置保留命令参数
        /// </summary>
        protected const CommandFlags setCommandFlags = CommandFlags.JsonSerialize;

        /// <summary>
        /// 创建输出信息
        /// </summary>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="buildInfo">输出创建参数</param>
        internal abstract outputLinkType Build(ServerSocketSenderBase sender, ref SenderBuildInfo buildInfo);
        /// <summary>
        /// 释放 TCP 服务端套接字输出信息
        /// </summary>
        /// <returns></returns>
        protected abstract outputLinkType free();
        /// <summary>
        /// 取消输出
        /// </summary>
        /// <param name="head"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CancelLink(outputLinkType head)
        {
            while (head != null) head = head.free();
        }
    }
    /// <summary>
    /// TCP 服务端套接字输出信息
    /// </summary>
    internal abstract class OutputLink : OutputLink<OutputLink>
    {
        /// <summary>
        /// 会话标识
        /// </summary>
        internal uint CommandIndex;
    }
}
