using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 获取可发送数量
    /// </summary>
    internal sealed class GetDistributionSendCount : Node
    {
        /// <summary>
        /// 返回调用委托
        /// </summary>
        private Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onReturn;
        /// <summary>
        /// 消息分发 读取配置
        /// </summary>
        internal DistributionConfig Config;
        /// <summary>
        /// 获取可发送数量
        /// </summary>
        /// <param name="distributor">消息队列节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="parser"></param>
        internal GetDistributionSendCount(Distributor distributor, DistributionConfig config, ref OperationParameter.NodeParser parser) : base(distributor)
        {
            Config = config;
            onReturn = parser.OnReturn;
            parser.OnReturn = null;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            try
            {
                new UnionType { Value = MessageQueue }.Distributor.GetSendCount(this);
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
        /// <param name="sendCount"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnReturn(uint sendCount)
        {
            ReturnParameter returnParameter = default(ReturnParameter);
            returnParameter.Parameter.ReturnParameterSet(sendCount);
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
