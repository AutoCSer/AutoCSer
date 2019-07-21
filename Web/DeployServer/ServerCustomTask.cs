using System;
using AutoCSer.Deploy;
using AutoCSer.Extension;

namespace AutoCSer.Web.DeployServer
{
    /// <summary>
    /// 自定义任务调用
    /// </summary>
    internal sealed class ServerCustomTask : IServerCustomTask
    {
        /// <summary>
        /// 发布服务更新以后的后续处理
        /// </summary>
        /// <param name="server"></param>
        /// <param name="customData"></param>
        /// <returns></returns>
        public DeployState OnDeployServerUpdated(Server server, byte[] customData)
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(server.OnDeployServerUpdated);
            return DeployState.Success;
        }
        /// <summary>
        /// 游戏服务更新以后的后续处理
        /// </summary>
        /// <param name="server"></param>
        /// <param name="customData"></param>
        public void OnGameServerUpdated(Server server, byte[] customData)
        {
            AutoCSer.Deploy.Server.UpdateSwitchFile(@"C:\AutoCSer\GameServer\", "AutoCSer.GameServer.exe")
                .StartProcessDirectory();
        }
    }
}
