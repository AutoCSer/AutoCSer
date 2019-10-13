using System;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 设置当前读取数据标识
    /// </summary>
    internal sealed class SetDistributionIdentity : Node
    {
        /// <summary>
        /// 消息分发 读文件
        /// </summary>
        private readonly DistributionFileReader reader;
        /// <summary>
        /// 确认已完成消息标识
        /// </summary>
        private readonly ulong identity;
        /// <summary>
        /// 消息超时追加到文件完毕
        /// </summary>
        internal AutoCSer.Net.TcpServer.ServerCallback<ReturnParameter> ServerCallback
        {
            get { return new OnAppendFileCallback(this); }
        }
        /// <summary>
        /// 设置当前读取数据标识
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="identity">确认已完成消息标识</param>
        internal SetDistributionIdentity(DistributionFileReader reader, ulong identity) : base(reader.Node)
        {
            this.reader = reader;
            this.identity = identity;
        }
        /// <summary>
        /// 消息超时追加到文件完毕
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool OnAppendFile(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            if (value.Type == Net.TcpServer.ReturnType.Success) reader.SetIdentity(identity);
            return true;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            reader.SetIdentity(identity);
            return LinkNext;
        }

        /// <summary>
        /// 消息超时追加到文件完毕
        /// </summary>
        internal sealed class OnAppendFileCallback : AutoCSer.Net.TcpServer.ServerCallback<ReturnParameter>
        {
            /// <summary>
            /// 设置当前读取数据标识
            /// </summary>
            private readonly SetDistributionIdentity node;
            /// <summary>
            /// 消息超时追加到文件完毕
            /// </summary>
            /// <param name="node">设置当前读取数据标识</param>
            internal OnAppendFileCallback(SetDistributionIdentity node)
            {
                this.node = node;
            }
            /// <summary>
            /// 消息超时追加到文件完毕
            /// </summary>
            /// <param name="returnValue"></param>
            /// <returns></returns>
            public override bool Callback(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> returnValue)
            {
                return node.OnAppendFile(returnValue);
            }
        }

    }
}
