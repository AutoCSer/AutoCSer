using System;
using AutoCSer.Deploy;
using AutoCSer.Extensions;

namespace AutoCSer.Web.DeployServer
{
    /// <summary>
    /// 自定义任务调用
    /// </summary>
    internal sealed class ServerCustomTask
    {
        /// <summary>
        /// 示例发布以后的后续处理
        /// </summary>
        /// <param name="server"></param>
        /// <param name="sender"></param>
        /// <param name="customData"></param>
        /// <returns></returns>
        public DeployResultData OnExample(AutoCSer.Deploy.Server server, AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, byte[] customData)
        {
            string path = AutoCSer.Web.Config.Deploy.ServerPath + @"www.AutoCSer.com\Download\";
            System.IO.File.Copy(path + "AutoCSer.zip", path + "AutoCSer.Example.zip", true);
            return DeployState.Success;
        }
    }
}
