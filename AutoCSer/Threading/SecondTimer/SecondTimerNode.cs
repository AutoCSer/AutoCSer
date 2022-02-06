using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时任务节点
    /// </summary>
    public abstract class SecondTimerNode : DoubleLink<SecondTimerNode>
    {
        /// <summary>
        /// 触发定时操作
        /// </summary>
        protected internal abstract void OnTimer();

        /// <summary>s
        /// 执行定时任务
        /// </summary>
        /// <param name="node"></param>
        internal static void LinkOnTimer(SecondTimerNode node)
        {
            do
            {
                try
                {
                    do
                    {
                        node.OnTimer();
                    }
                    while ((node = node.DoubleLinkPrevious) != null);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.Exception(exception, null, LogLevel.AutoCSer | LogLevel.Exception);
                }
                if (node == null) break;
                node = node.DoubleLinkPrevious;
            }
            while (node != null);
        }
    }
}
