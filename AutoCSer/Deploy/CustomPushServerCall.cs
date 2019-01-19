using AutoCSer.Net.TcpServer;
using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 服务端推送调用
    /// </summary>
    internal sealed class CustomPushServerCall : ServerCallBase
    {
        /// <summary>
        /// 部署服务
        /// </summary>
        private readonly Server server;
        /// <summary>
        /// 自定义数据
        /// </summary>
        private readonly byte[] customData;
        /// <summary>
        /// 服务端推送委托编号
        /// </summary>
        private int onPushIdentity;
        /// <summary>
        /// 自定义轮询调用
        /// </summary>
        /// <param name="server">部署服务</param>
        /// <param name="customData">自定义数据</param>
        /// <param name="onPushIdentity">服务端推送委托编号</param>
        internal CustomPushServerCall(Server server, byte[] customData, int onPushIdentity)
        {
            this.server = server;
            this.customData = customData;
            this.onPushIdentity = onPushIdentity;
        }
        /// <summary>
        /// 调用处理
        /// </summary>
        public override void Call()
        {
            try
            {
                server.CallCustomPush(customData, onPushIdentity);
            }
            catch (Exception error)
            {
                server.TcpServer.AddLog(error);
            }
        }
    }
}
