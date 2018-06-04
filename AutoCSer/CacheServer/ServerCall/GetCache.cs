using System;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.ServerCall
{
    /// <summary>
    /// 获取缓存数据
    /// </summary>
    internal sealed class GetCache : AutoCSer.Net.TcpServer.ServerCallBase
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        private readonly AutoCSer.CacheServer.CacheManager cache;
        /// <summary>
        /// 获取缓存数据回调委托
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter>, bool> onCache;
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="onCache">获取缓存数据回调委托</param>
        internal GetCache(AutoCSer.CacheServer.CacheManager cache, Func<AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter>, bool> onCache)
        {
            this.cache = cache;
            this.onCache = onCache;
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        public override void Call()
        {
            try
            {
                new CacheGetter(cache, onCache);
            }
            catch (Exception error)
            {
                cache.TcpServer.AddLog(error);
            }
        }
    }
}
