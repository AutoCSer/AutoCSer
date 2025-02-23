﻿using System;
using AutoCSer.Extensions;

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
        public override void RunTask()
        {
            DistributionFileReaderTimeout.QueueOnTimer();
        }
    }
}
