using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 添加获取数据任务
    /// </summary>
    internal sealed class GetDistributionMessage : Node
    {
        /// <summary>
        /// 消息分发 读文件
        /// </summary>
        private readonly DistributionFileReader reader;
        /// <summary>
        /// 单次可发送数量
        /// </summary>
        private readonly int sendCount;
        /// <summary>
        /// 返回调用委托
        /// </summary>
        internal AutoCSer.Net.TcpServer.ServerCallback<IdentityReturnParameter> OnReturn;
        /// <summary>
        /// 是否反序列化网络流，否则需要 Copy 数据
        /// </summary>
        private readonly bool isReturnStream;
        /// <summary>
        /// 添加获取数据任务
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="onReturn"></param>
        /// <param name="parser"></param>
        internal GetDistributionMessage(DistributionFileReader reader, AutoCSer.Net.TcpServer.ServerCallback<IdentityReturnParameter> onReturn, ref OperationParameter.NodeParser parser) : base(reader.Node)
        {
            OnReturn = onReturn;
            sendCount = Math.Max(parser.ValueData.Int64.Int, 1);
            isReturnStream = parser.ReturnParameter.IsReturnDeSerializeStream;
            this.reader = reader;
            parser.ReturnParameter.Value = null;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        public override void RunTask()
        {
            reader.Get(this);
        }
        /// <summary>
        /// 设置消息回调信息
        /// </summary>
        /// <param name="getter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ref DistributionMessageGetter getter)
        {
            getter.Set(OnReturn, sendCount, isReturnStream);
        }
    }
}
