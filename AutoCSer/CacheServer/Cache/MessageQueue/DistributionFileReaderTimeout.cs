using System;
using System.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息分发 读文件 超时定时任务
    /// </summary>
    internal sealed class DistributionFileReaderTimeout : AutoCSer.Threading.SecondTimerNode
    {
        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        protected internal override void OnTimer()
        {
            if (isTimer == 0 && readerCount != 0)
            {
                isTimer = 1;
                queueOnTimer.AddQueueTaskLinkThread();
            }
        }

        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        private static readonly QueueTaskThread.DistributionOnTimer queueOnTimer = new QueueTaskThread.DistributionOnTimer();
        /// <summary>
        /// 消息分发集合
        /// </summary>
        private static DistributionFileReader[] readers = EmptyArray<DistributionFileReader>.Array;
        /// <summary>
        /// 消息分发数量
        /// </summary>
        private static int readerCount;
        /// <summary>
        /// 是否正在检测消息超时
        /// </summary>
        private static volatile int isTimer;
        /// <summary>
        /// 添加消息分发
        /// </summary>
        /// <param name="reader"></param>
        internal static void AddReader(DistributionFileReader reader)
        {
            if (readerCount == readers.Length) readers = readers.copyNew(Math.Max(readerCount << 1, sizeof(int)));
            reader.ReaderIndex = readerCount;
            readers[readerCount++] = reader;
        }
        /// <summary>
        /// 删除消息分发
        /// </summary>
        /// <param name="reader"></param>
        internal static void RemoveReader(DistributionFileReader reader)
        {
            int readerIndex = reader.ReaderIndex;
            if (readerIndex >= 0 && Object.ReferenceEquals(readers[readerIndex], reader))
            {
                if (--readerCount != readerIndex) (readers[readerIndex] = readers[readerCount]).ReaderIndex = readerIndex;
                readers[readerCount] = null;
            }
        }
        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void QueueOnTimer()
        {
            try
            {
                if (readerCount != 0)
                {
                    int count = readerCount;
                    foreach (DistributionFileReader reader in readers)
                    {
                        reader.OnTimer();
                        if (--count == 0) return;
                    }
                }
            }
            finally { isTimer = 0; }
        }

        static DistributionFileReaderTimeout()
        {
            AutoCSer.Threading.SecondTimer.InternalTaskArray.NodeLink.PushNotNull(new DistributionFileReaderTimeout());
        }
    }
}
