using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务基类
    /// </summary>
    public abstract class Server : AutoCSer.Net.TcpInternalServer.TimeVerifyServer, IDisposable
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        internal CacheManager Cache;
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Cache != null)
            {
                Cache.Dispose();
                Cache = null;
            }
        }

        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">数据结构定义节点查询参数</param>
        /// <returns>返回参数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal ReturnParameter Query(OperationParameter.QueryNode parameter)
        {
            return Cache.Query(ref parameter.QueryData);
            //return Cache.Query(parameter.Buffer);
        }
        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">数据结构定义节点查询参数</param>
        /// <returns>返回参数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        internal ReturnParameter QueryStream(OperationParameter.QueryNode parameter)
        {
            ReturnParameter returnValue = Cache.Query(ref parameter.QueryData);
            //ReturnParameter returnValue = Cache.Query(parameter.Buffer);
            returnValue.IsDeSerializeStream = true;
            return returnValue;
        }
    }
}
