using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存数据获取阶段
    /// </summary>
    internal enum CacheGetStep : byte
    {
        /// <summary>
        /// 缓存快照处理
        /// </summary>
        Snapshot,
        /// <summary>
        /// 操作数据队列
        /// </summary>
        TcpQueue,
        /// <summary>
        /// 加载完成
        /// </summary>
        Loaded,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
    }
}
