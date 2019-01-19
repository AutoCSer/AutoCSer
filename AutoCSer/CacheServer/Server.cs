using System;
using AutoCSer.Extension;
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
            return new ReturnParameter(Cache.Query(ref parameter.QueryData));
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
            ValueData.Data returnValue = Cache.Query(ref parameter.QueryData);
            returnValue.IsReturnDeSerializeStream = true;
            return new ReturnParameter(ref returnValue);
        }
        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">数据结构定义节点查询参数</param>
        /// <returns>返回参数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientSendOnly = true)]
        internal void QueryOnly(OperationParameter.QueryNode parameter)
        {
            Cache.Query(ref parameter.QueryData);
        }

        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">数据结构定义节点查询参数</param>
        /// <param name="onQuery"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal void QueryAsynchronous(OperationParameter.QueryNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery)
        {
            Cache.Query(ref parameter.QueryData, onQuery, false);
        }
        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">数据结构定义节点查询参数</param>
        /// <param name="onQuery"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        internal void QueryAsynchronousStream(OperationParameter.QueryNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery)
        {
            Cache.Query(ref parameter.QueryData, onQuery, true);
        }

        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">数据结构定义节点查询参数</param>
        /// <param name="onQuery"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        internal void QueryKeepCallback(OperationParameter.QueryNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery)
        {
            Cache.Query(ref parameter.QueryData, onQuery, false);
        }
        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">数据结构定义节点查询参数</param>
        /// <param name="onQuery"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        internal void QueryKeepCallbackStream(OperationParameter.QueryNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery)
        {
            Cache.Query(ref parameter.QueryData, onQuery, true);
        }

        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">短路径查询参数</param>
        /// <returns>返回参数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal ReturnParameter Query(OperationParameter.ShortPathQueryNode parameter)
        {
            return new ReturnParameter(Cache.ShortPathQuery(ref parameter.QueryData));
        }
        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">短路径查询参数</param>
        /// <returns>返回参数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        internal ReturnParameter QueryStream(OperationParameter.ShortPathQueryNode parameter)
        {
            ValueData.Data returnValue = Cache.ShortPathQuery(ref parameter.QueryData);
            returnValue.IsReturnDeSerializeStream = true;
            return new ReturnParameter(ref returnValue);
        }

        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">短路径查询参数</param>
        /// <param name="onQuery"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal void QueryAsynchronous(OperationParameter.ShortPathQueryNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery)
        {
            Cache.ShortPathQuery(ref parameter.QueryData, onQuery, false);
        }
        /// <summary>
        /// 表达式节点查询
        /// </summary>
        /// <param name="parameter">短路径查询参数</param>
        /// <param name="onQuery"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        internal void QueryAsynchronousStream(OperationParameter.ShortPathQueryNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery)
        {
            Cache.ShortPathQuery(ref parameter.QueryData, onQuery, true);
        }

        /// <summary>
        /// 创建缓存静态路由集群节点 TCP 内部服务配置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute CreateStaticRouteAttribute(int index, AutoCSer.Net.TcpInternalServer.ServerAttribute attribute)
        {
            (attribute = AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(attribute)).Name += "_" + index.toString();
            return attribute;
        }
    }
}
