using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务客户端
    /// </summary>
    public sealed partial class MasterClient : Client, IDisposable
    {
        /// <summary>
        /// TCP 客户端
        /// </summary>
        private MasterServer.TcpInternalClient client;
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="client">缓存服务客户端</param>
        public MasterClient(MasterServer.TcpInternalClient client = null)
        {
            this.client = client ?? new MasterServer.TcpInternalClient();
        }
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="serverAttribute">缓存服务端配置</param>
        public MasterClient(AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute)
        {
            client = new MasterServer.TcpInternalClient(serverAttribute);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        /// <summary>
        /// 获取或者创建数据结构信息
        /// </summary>
        /// <param name="dataStructure"></param>
        /// <returns></returns>
        internal override AutoCSer.Net.TcpServer.ReturnValue<IndexIdentity> GetOrCreate(ClientDataStructure dataStructure)
        {
            return client.GetOrCreate(new OperationParameter.ClientDataStructure { DataStructure = dataStructure });
        }
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        public override ReturnValue<bool> RemoveDataStructure(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) return new ReturnValue<bool> { Type = ReturnType.NullArgument };
            IndexIdentity identity = default(IndexIdentity);
            identity.Set(client.Remove(new OperationParameter.RemoveDataStructure { CacheName = cacheName }));
            if (identity.ReturnType == ReturnType.Success) removeDataStructure(cacheName, ref identity);
            return new ReturnValue<bool> { TcpReturnType = identity.TcpReturnType, Type = identity.ReturnType, Value = identity.ReturnType == ReturnType.Success };
        }
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="identity"></param>
        private void removeDataStructure(HashString cacheName, ref IndexIdentity identity)
        {
            ClientDataStructure dataStructure;
            Monitor.Enter(cacheNameLock);
            try
            {
                if (CacheNames.TryGetValue(cacheName, out dataStructure) && dataStructure.Identity.Equals(ref identity) == 0) CacheNames.Remove(cacheName);
            }
            finally { Monitor.Exit(cacheNameLock); }
        }
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="onRemove"></param>
        public override void RemoveDataStructure(string cacheName, Action<ReturnValue<bool>> onRemove)
        {
            if (string.IsNullOrEmpty(cacheName))
            {
                onRemove(new ReturnValue<bool> { Type = ReturnType.NullArgument });
                return;
            }
            try
            {
                client.Remove(new OperationParameter.RemoveDataStructure { CacheName = cacheName }, returnValue =>
                {
                    IndexIdentity identity = returnValue.Value;
                    identity.TcpReturnType = returnValue.Type;
                    if (identity.ReturnType == ReturnType.Success) removeDataStructure(cacheName, ref identity);
                    if (onRemove != null) onRemove(new ReturnValue<bool> { TcpReturnType = identity.TcpReturnType, Type = identity.ReturnType, Value = identity.ReturnType == ReturnType.Success });
                });
                onRemove = null;
            }
            finally
            {
                if (onRemove != null) onRemove(new ReturnValue<bool> { Type = ReturnType.TcpError });
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        internal override AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Query(DataStructure.Abstract.Node node)
        {
            return client.Query(new OperationParameter.QueryNode { Node = node });
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void Query(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            client.Query(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void QueryStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            client.QueryStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Operation(DataStructure.Abstract.Node node)
        {
            return client.Operation(new OperationParameter.OperationNode { Node = node });
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        internal override void OperationOnly(DataStructure.Abstract.Node node)
        {
            client.OperationOnly(new OperationParameter.OperationNode { Node = node });
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void Operation(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            client.Operation(new OperationParameter.OperationNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void OperationStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            client.OperationStream(new OperationParameter.OperationNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.ReturnValue WriteFile()
        {
            return client.WriteFile();
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="onWrite"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteFile(Action<AutoCSer.Net.TcpServer.ReturnValue> onWrite)
        {
            client.WriteFile(onWrite);
        }
    }
}
