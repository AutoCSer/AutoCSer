using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时任务继续模式
    /// </summary>
    public enum SecondTimerKeepMode : byte
    {
        /// <summary>
        /// 仅执行当前一次
        /// </summary>
        Once,
        /// <summary>
        /// 执行之后添加新的定时任务
        /// </summary>
        After,
        /// <summary>
        /// 执行之前添加新的定时任务
        /// </summary>
        Before,
        /// <summary>
        /// 已经取消执行
        /// </summary>
        Canceled,
    }
}
