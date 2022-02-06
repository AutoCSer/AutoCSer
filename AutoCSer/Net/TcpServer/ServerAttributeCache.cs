using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务配置缓存
    /// </summary>
    internal struct ServerAttributeCache
    {
        /// <summary>
        /// 附加验证字符串信息哈希值
        /// </summary>
        internal ulong VerifyHashCode;
        /// <summary>
        /// 服务名称
        /// </summary>
        internal string ServerName;
        /// <summary>
        /// 客户端接收命令超时
        /// </summary>
        internal int ReceiveVerifyCommandSeconds;
        /// <summary>
        /// 最大输入数据字节数
        /// </summary>
        internal int MaxInputSize;
        /// <summary>
        /// 服务端批量处理等待类型
        /// </summary>
        internal OutputWaitType OutputWaitType;
        /// <summary>
        /// 服务端创建输出是否开启线程
        /// </summary>
        internal bool IsBuildOutputThread;
        /// <summary>
        /// 远程表达式服务端任务类型
        /// </summary>
        internal ServerTaskType RemoteExpressionTask;
        /// <summary>
        /// 远程表达式服务器端独占 TCP 同步调用队列编号
        /// </summary>
        internal byte RemoteExpressionCallQueueIndex;
        /// <summary>
        /// 设置配置数据
        /// </summary>
        /// <param name="attribute"></param>
        internal void Set(ServerBaseAttribute attribute)
        {
            ServerName = attribute.ServerName;
            VerifyHashCode = attribute.VerifyHashCode;
            ReceiveVerifyCommandSeconds = attribute.GetReceiveVerifyCommandSeconds;
            if ((MaxInputSize = attribute.GetMaxInputSize) <= 0) MaxInputSize = int.MaxValue;
            OutputWaitType = attribute.GetServerOutputWaitType;
            IsBuildOutputThread = attribute.GetIsServerBuildOutputThread;
            RemoteExpressionTask = attribute.GetRemoteExpressionServerTask;
            RemoteExpressionCallQueueIndex = attribute.GetRemoteExpressionCallQueueIndex;
        }
    }
}
