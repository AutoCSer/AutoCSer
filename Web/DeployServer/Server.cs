using System;
using System.Threading;

namespace AutoCSer.Web.DeployServer
{
    /// <summary>
    /// 部署服务
    /// </summary>
    public sealed class Server : AutoCSer.Deploy.Server
    {
        /// <summary>
        /// 切换服务锁
        /// </summary>
        private readonly ManualResetEvent switchEvent;
        /// <summary>
        /// 切换服务锁
        /// </summary>
        private readonly ManualResetEvent exitEvent;
        /// <summary>
        /// 自定义任务集合
        /// </summary>
        private readonly AutoCSer.Deploy.ServerCustomTask customTask;
        /// <summary>
        /// 部署服务
        /// </summary>
        /// <param name="switchEvent"></param>
        /// <param name="exitEvent"></param>
        internal Server(ManualResetEvent switchEvent, ManualResetEvent exitEvent)
        {
            this.switchEvent = switchEvent;
            this.exitEvent = exitEvent;
            customTask = new AutoCSer.Deploy.ServerCustomTask<ServerCustomTask>(new ServerCustomTask());
        }
        /// <summary>
        /// 切换服务前的调用
        /// </summary>
        protected override void beforeSwitch()
        {
            switchEvent.Set();
            exitEvent.WaitOne();
        }
        /// <summary>
        /// 自定义任务处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public override Deploy.DeployState CallCustomTask(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, Deploy.Task task)
        {
            return customTask.Call(this, sender, task);
        }
    }
}
