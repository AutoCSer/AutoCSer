﻿using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 远程表达式客户端检测服务端映射标识
    /// </summary>
    internal sealed class RemoteExpressionServerNodeIdChecker : RemoteExpression.ServerNodeIdChecker
    {
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        internal ClientSocketSender Sender;
        /// <summary>
        /// 获取服务端映射标识集合
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal override ReturnValue<int[]> Get(AutoCSer.Reflection.RemoteType[] types)
        {
            return Sender.GetRemoteExpressionNodeId(types);
        }
    }
}
