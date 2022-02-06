using System;
using System.Threading;
using AutoCSer.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AutoCSer.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用队列处理
    /// </summary>
    public class ServerCallQueue : AutoCSer.Threading.TaskQueueThread<ServerCallBase>
    {
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="isStart">是否启动线程</param>
        internal ServerCallQueue(bool isBackground, bool isStart) : base(isBackground, isStart) { }
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        public ServerCallQueue(bool isBackground = true) : this(isBackground, true) { }
        /// <summary>
        /// 低优先级任务节点
        /// </summary>
        internal sealed class ServerCallLink : ServerCallBase
        {
            /// <summary>
            /// 低优先级任务队列链表
            /// </summary>
            private readonly LowPriorityLink link;
            /// <summary>
            /// 低优先级任务节点
            /// </summary>
            /// <param name="link">低优先级任务队列链表</param>
            internal ServerCallLink(LowPriorityLink link)
            {
                this.link = link;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            public override void RunTask()
            {
                link.RunTask();
            }
        }
        /// <summary>
        /// 添加低优先级任务队列链表
        /// </summary>
        /// <param name="link"></param>
        public override void Add(LowPriorityLink link)
        {
            Add(new ServerCallLink(link));
        }

        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        internal static readonly ServerCallQueue Default;
        /// <summary>
        /// TCP 服务器端同步调用低优先级队列处理
        /// </summary>
        internal static readonly LowPriorityLink DefaultLink;
        static ServerCallQueue()
        {
            Default = new ServerCallQueue();
            DefaultLink = Default.CreateLink();
        }
    }
}
