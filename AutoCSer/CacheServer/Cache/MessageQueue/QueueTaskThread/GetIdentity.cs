using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 获取当前读取数据标识
    /// </summary>
    internal sealed class GetIdentity : Node
    {
        /// <summary>
        /// 返回调用委托
        /// </summary>
        private Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onReturn;
        /// <summary>
        /// 队列数据 读取配置
        /// </summary>
        internal ReaderConfig Config;
        /// <summary>
        /// 队列数据 读文件索引
        /// </summary>
        internal int ReaderIndex;
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="messageQueue">消息队列节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="parser"></param>
        internal GetIdentity(MessageQueue.Node messageQueue, ReaderConfig config, ref OperationParameter.NodeParser parser) : base(messageQueue)
        {
            Config = config;
            onReturn = parser.OnReturn;
            parser.OnReturn = null;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="messageQueue">消息队列节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="parser"></param>
        /// <param name="readerIndex">队列数据 读文件索引</param>
        internal GetIdentity(MessageQueue.Node messageQueue, ReaderConfig config, ref OperationParameter.NodeParser parser, int readerIndex) : this(messageQueue, config, ref parser)
        {
            ReaderIndex = readerIndex;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            try
            {
                MessageQueue.GetReadIdentity(this);
            }
            finally
            {
                if (onReturn != null) onReturn(new ReturnParameter(ReturnType.MessageQueueCreateReaderError));
            }
            return LinkNext;
        }
        /// <summary>
        /// 调用返回委托
        /// </summary>
        /// <param name="identity"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnReturn(ulong identity)
        {
            ReturnParameter returnParameter = default(ReturnParameter);
            returnParameter.Parameter.ReturnParameterSet(identity);
            onReturn(returnParameter);
            onReturn = null;
        }
        /// <summary>
        /// 调用返回委托
        /// </summary>
        /// <param name="returnType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnReturn(ReturnType returnType)
        {
            onReturn(new ReturnParameter(returnType));
            onReturn = null;
        }
    }
}
