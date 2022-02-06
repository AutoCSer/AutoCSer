using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字数据发送完成后的任务线程集合
    /// </summary>
    internal sealed class OnSendThreadArray : AutoCSer.Threading.TaskSwitchThreadArray<OnSendThread, Socket>
    {
        /// <summary>
        /// HTTP 套接字数据发送完成后的任务线程集合
        /// </summary>
        /// <param name="config"></param>
        private OnSendThreadArray(AutoCSer.Threading.TaskSwitchThreadConfig config = null) : base(config ?? DefaultConfig) { }
        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <returns></returns>
        public override OnSendThread CreateThread() { return new OnSendThread(this); }

        /// <summary>
        /// HTTP 套接字数据发送完成后的任务线程集合
        /// </summary>
        internal static readonly OnSendThreadArray Default = (OnSendThreadArray)AutoCSer.Configuration.Common.Get(typeof(OnSendThreadArray)) ?? new OnSendThreadArray((AutoCSer.Threading.TaskSwitchThreadConfig)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Threading.TaskSwitchThreadConfig), "HttpOnSendThread"));
    }
}
