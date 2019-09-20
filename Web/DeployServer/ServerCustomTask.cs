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
        /// <param name="sender"></param>
        /// <param name="customData"></param>
        /// <returns></returns>
        public DeployState OnDeployServerUpdated(Server server, AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, byte[] customData)
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(server.OnDeployServerUpdated);
            return DeployState.Success;
        }
        /// <summary>
        /// 示例发布以后的后续处理
        /// </summary>
        /// <param name="server"></param>
        /// <param name="sender"></param>
        /// <param name="customData"></param>
        /// <returns></returns>
        public DeployState OnExample(Server server, AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, byte[] customData)
        {
            string path = AutoCSer.Web.Config.Deploy.ServerPath + @"www.AutoCSer.com\Download\";
            System.IO.File.Copy(path + "AutoCSer.zip", path + "AutoCSer.Example.zip", true);
            return DeployState.Success;
        }
        /// <summary>
        /// 游戏服务更新以后的后续处理
        /// </summary>
        /// <param name="server"></param>
        /// <param name="sender"></param>
        /// <param name="customData"></param>
        /// <returns></returns>
        public DeployState OnGameServerUpdated(Server server, AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, byte[] customData)
        {
            AutoCSer.Deploy.Server.UpdateSwitchFile(@"C:\AutoCSer\GameServer\", "AutoCSer.GameServer.exe")
                .StartProcessDirectory();
            return DeployState.Success;
        }
    }
}
