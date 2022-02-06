using System;
using System.Collections.Generic;
using AutoCSer.Extensions;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientObject
    {
        /// <summary>
        /// 部署任务状态更新回调
        /// </summary>
        internal AutoCSer.Net.TcpServer.ServerCallback<Log> OnLog;
        /// <summary>
        /// 自定义推送
        /// </summary>
        public AutoCSer.Net.TcpServer.ServerCallback<byte[]> OnCustomPush { get; internal set; }
        /// <summary>
        /// 发布信息集合
        /// </summary>
        internal Dictionary<int, DeployInfo> Deploys = new Dictionary<int, DeployInfo>();
        /// <summary>
        /// 获取发布信息
        /// </summary>
        /// <param name="deployIndex"></param>
        /// <returns></returns>
        internal DeployInfo GetDeploy(int deployIndex)
        {
            DeployInfo deployInfo;
            if (Deploys.TryGetValue(deployIndex, out deployInfo))
            {
                deployInfo.DeploySeconds = AutoCSer.Threading.SecondTimer.CurrentSeconds;
                if (Deploys.Count > 1) checkTimeout();
                return deployInfo;
            }
            if (Deploys.Count != 0) checkTimeout();
            return null;
        }
        /// <summary>
        /// 发布信息超时检测
        /// </summary>
        private void checkTimeout()
        {
            long timeoutSeconds = AutoCSer.Threading.SecondTimer.CurrentSeconds - DeployInfo.TimeoutSeconds;
            foreach (DeployInfo deployInfo in Deploys.Values.getFind(p => p.DeploySeconds <= timeoutSeconds)) Deploys.Remove(deployInfo.Index);
        }
    }
}
