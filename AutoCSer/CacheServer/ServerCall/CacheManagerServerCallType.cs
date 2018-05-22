using System;

namespace AutoCSer.CacheServer.ServerCall
{
    /// <summary>
    /// 缓存管理 TCP 服务器端同步调用类型
    /// </summary>
    internal enum CacheManagerServerCallType : byte
    {
        ///// <summary>
        ///// 缓存数据加载完成
        ///// </summary>
        //Loaded,
        /// <summary>
        /// 允许写操作
        /// </summary>
        SetCanWrite,
        /// <summary>
        /// 禁用写操作
        /// </summary>
        CancelCanWrite,
        /// <summary>
        /// 释放缓存管理
        /// </summary>
        Dispose,
        ///// <summary>
        ///// 超时处理
        ///// </summary>
        //Timeout,
        /// <summary>
        /// 重建文件流失败
        /// </summary>
        CreateNewFileStreamError,
        /// <summary>
        /// 重建文件流检测缓存队列
        /// </summary>
        CreateNewFileStreamCheckQueue,
    }
}
