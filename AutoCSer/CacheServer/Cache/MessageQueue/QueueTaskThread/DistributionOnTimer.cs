using System;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 定时器触发消息处理超时检测
    /// </summary>
    internal sealed class DistributionOnTimer : Node
    {
        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        internal DistributionOnTimer() : base(null) { }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            Node next = LinkNext;
            try
            {
                DistributionFileReader.QueueOnTimer();
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
            LinkNext = null;
            return next;
        }
    }
}
