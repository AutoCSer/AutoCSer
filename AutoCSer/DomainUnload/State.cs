using System;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 卸载状态
    /// </summary>
    internal enum State
    {
        /// <summary>
        /// 正常运行状态
        /// </summary>
        Run,
        /// <summary>
        /// 卸载中，等待事务结束
        /// </summary>
        WaitTransaction,
        /// <summary>
        /// 卸载事件处理
        /// </summary>
        Event,
        /// <summary>
        /// 已经卸载
        /// </summary>
        Unloaded
    }
}
