using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 添加获取数据任务
    /// </summary>
    internal sealed class GetMessage : Node
    {
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private readonly FileReader reader;
        /// <summary>
        /// 读取消息起始标识
        /// </summary>
        internal readonly ulong Identity;
        /// <summary>
        /// 返回调用委托
        /// </summary>
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> OnReturn;
        /// <summary>
        /// 是否反序列化网络流，否则需要 Copy 数据
        /// </summary>
        internal readonly bool IsReturnStream;
        /// <summary>
        /// 添加获取数据任务
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="parser"></param>
        internal GetMessage(FileReader reader, ref OperationParameter.NodeParser parser) : base(reader.Node)
        {
            OnReturn = parser.OnReturn;
            IsReturnStream = parser.ReturnParameter.IsReturnDeSerializeStream;
            Identity = parser.ValueData.Int64.ULong;
            this.reader = reader;
            parser.OnReturn = null;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            reader.Get(this);
            return LinkNext;
        }
    }
}
