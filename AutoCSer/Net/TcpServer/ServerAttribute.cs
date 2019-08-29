using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public abstract class ServerAttribute : ServerBaseAttribute
    {
        /// <summary>
        /// 远程表达式服务端任务类型，默认为 Timeout
        /// </summary>
        public ServerTaskType RemoteExpressionServerTask = ServerTaskType.Timeout;
        /// <summary>
        /// 客户端最大自定义数据包字节大小，0 表示不限
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract int GetMaxCustomDataSize { get; }
        /// <summary>
        /// 客户端重建连接休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract int GetClientTryCreateSleep { get; }
        /// <summary>
        /// 客户端第一次重建连接休眠毫秒数（默认为客户端重建连接休眠毫秒数）
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual int GetClientFirstTryCreateSleep { get { return GetClientTryCreateSleep; } }
        /// <summary>
        /// 批量处理休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract int GetClientOutputSleep { get; }
        /// <summary>
        /// 批量处理休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract int GetServerOutputSleep { get; }
        /// <summary>
        /// 服务端创建输出是否开启线程
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract bool GetIsServerBuildOutputThread { get; }
        /// <summary>
        /// 命令池初始化二进制大小 2^n
        /// </summary>
        internal abstract byte GetCommandPoolBitSize { get; }
        /// <summary>
        /// 默认为 false 表示更注重客户端异步吞吐性能，否则更注重客户端 await 吞吐性能
        /// </summary>
        public bool IsClientAwaiter;
    }
}
