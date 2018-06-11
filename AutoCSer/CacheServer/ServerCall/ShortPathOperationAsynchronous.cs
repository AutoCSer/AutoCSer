using System;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.ServerCall
{
    /// <summary>
    /// 异步操作
    /// </summary>
    internal sealed class ShortPathOperationAsynchronous : AutoCSer.Net.TcpServer.ServerCallBase
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        private readonly AutoCSer.CacheServer.CacheManager cache;
        /// <summary>
        /// 短路径操作参数
        /// </summary>
        private OperationParameter.ShortPathOperationNode parameter;
        /// <summary>
        /// 操作完成回调
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation;
        /// <summary>
        /// 异步操作
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="parameter">短路径操作参数</param>
        /// <param name="onOperation">操作完成回调</param>
        internal ShortPathOperationAsynchronous(AutoCSer.CacheServer.CacheManager cache, ref OperationParameter.ShortPathOperationNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation)
        {
            this.cache = cache;
            this.parameter = parameter;
            this.onOperation = onOperation;
        }
        /// <summary>
        /// 获取缓存操作队列
        /// </summary>
        public override void Call()
        {
            try
            {
                cache.Operation(ref parameter, onOperation);
            }
            catch (Exception error)
            {
                cache.TcpServer.AddLog(error);
            }
        }
    }
}
