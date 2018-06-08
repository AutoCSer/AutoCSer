using System;

namespace AutoCSer.CacheServer.Cache.Lock.QueueTaskThread
{
    /// <summary>
    /// 释放锁任务
    /// </summary>
    internal sealed class Exit : Node
    {
        /// <summary>
        /// 随机序号
        /// </summary>
        internal readonly ulong RandomNo;
        /// <summary>
        /// 返回调用委托
        /// </summary>
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> OnReturn;
        /// <summary>
        /// 添加获取数据任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parser"></param>
        internal Exit(Lock.Node node, ref OperationParameter.NodeParser parser) : base(node)
        {
            OnReturn = parser.OnReturn;
            RandomNo = parser.ValueData.Int64.ULong;
            parser.OnReturn = null;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            Lock.Exit(this);
            return LinkNext;
        }
    }
}
