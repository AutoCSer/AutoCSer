using System;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.ServerCall
{
    /// <summary>
    /// 异步操作
    /// </summary>
    internal sealed class OperationAsynchronous : AutoCSer.Net.TcpServer.ServerCallBase
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        private readonly AutoCSer.CacheServer.CacheManager cache;
        /// <summary>
        /// 操作数据缓冲区
        /// </summary>
        private readonly Buffer buffer;
        /// <summary>
        /// 操作完成回调
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation;
        /// <summary>
        /// 异步操作
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="buffer">操作数据缓冲区</param>
        /// <param name="onOperation">操作完成回调</param>
        internal OperationAsynchronous(AutoCSer.CacheServer.CacheManager cache, Buffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation)
        {
            this.cache = cache;
            this.buffer = buffer;
            this.onOperation = onOperation;
        }
        /// <summary>
        /// 获取缓存操作队列
        /// </summary>
        public override void Call()
        {
            try
            {
                cache.Operation(buffer, onOperation);
            }
            catch (Exception error)
            {
                cache.TcpServer.AddLog(error);
            }
        }
    }
}
