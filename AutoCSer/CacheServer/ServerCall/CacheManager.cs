using System;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.ServerCall
{
    /// <summary>
    /// 缓存管理 TCP 服务器端同步调用
    /// </summary>
    internal sealed class CacheManager : AutoCSer.Net.TcpServer.ServerCallBase
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        private readonly AutoCSer.CacheServer.CacheManager cache;
        /// <summary>
        /// 调用类型
        /// </summary>
        private readonly CacheManagerServerCallType type;
        /// <summary>
        /// 缓存管理 TCP 服务器端同步调用
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="type">调用类型</param>
        internal CacheManager(AutoCSer.CacheServer.CacheManager cache, CacheManagerServerCallType type)
        {
            this.cache = cache;
            this.type = type;
        }
        /// <summary>
        /// 缓存管理
        /// </summary>
        public override void Call()
        {
            try
            {
                switch (type)
                {
                    //case CacheManagerServerCallType.Loaded: Cache.Loaded(); return;
                    case CacheManagerServerCallType.Dispose: cache.DisposeTcpQueue(); return;
                    case CacheManagerServerCallType.SetCanWrite: cache.CanWrite = true; return;
                    case CacheManagerServerCallType.CancelCanWrite: cache.CanWrite = false; return;
                    //case CacheManagerServerCallType.Timeout:
                    //    for (CacheTimeout timeout = CacheTimeout.Timeouts.End; timeout != null; timeout = timeout.DoubleLinkPrevious) timeout.OnTimer();
                    //    return;
                    case CacheManagerServerCallType.CreateNewFileStreamError: cache.CreateNewFileStreamError(); return;
                    case CacheManagerServerCallType.CreateNewFileStreamCheckQueue: cache.NewFile.CheckQueue(); return;
                }

            }
            catch (Exception error)
            {
                cache.TcpServer.AddLog(error);
            }
        }
    }
}
