using System;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.ServerCall
{
    /// <summary>
    /// 获取缓存操作队列
    /// </summary>
    internal sealed class CacheGetterGetQueue : AutoCSer.Net.TcpServer.ServerCallBase
    {
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private readonly CacheGetter getter;
        /// <summary>
        /// 获取缓存操作队列
        /// </summary>
        /// <param name="getter">获取缓存操作队列</param>
        internal CacheGetterGetQueue(CacheGetter getter)
        {
            this.getter = getter;
        }
        /// <summary>
        /// 获取缓存操作队列
        /// </summary>
        public override void Call()
        {
            try
            {
                getter.GetQueue();
            }
            catch (Exception error)
            {
                getter.Cache.TcpServer.AddLog(error);
            }
        }
    }
}
