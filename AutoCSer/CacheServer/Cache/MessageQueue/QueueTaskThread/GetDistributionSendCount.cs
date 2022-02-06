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
        private AutoCSer.Net.TcpServer.ServerCallback<ReturnParameter> onReturn;
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
        public override void RunTask()
        {
            try
            {
                new UnionType.Distributor { Object = MessageQueue }.Value.GetSendCount(this);
            }
            finally
            {
                if (onReturn != null) onReturn.Callback(new ReturnParameter(ReturnType.MessageQueueCreateReaderError));
            }
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
            onReturn.Callback(returnParameter);
            onReturn = null;
        }
        /// <summary>
        /// 调用返回委托
        /// </summary>
        /// <param name="returnType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnReturn(ReturnType returnType)
        {
            onReturn.Callback(new ReturnParameter(returnType));
            onReturn = null;
        }
    }
}
