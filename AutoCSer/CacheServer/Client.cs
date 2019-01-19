using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client : IDisposable
    {
        /// <summary>
        /// 缓存主服务客户端
        /// </summary>
        internal readonly MasterServer.TcpInternalClient MasterClient;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.CheckSocketVersion checkSocketVersion;
        /// <summary>
        /// 缓存主服务客户端
        /// </summary>
        private readonly SlaveServer.TcpInternalClient slaveClient;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.CheckSocketVersion slaveCheckSocketVersion;
        /// <summary>
        /// 数据结构定义信息集合
        /// </summary>
        internal readonly Dictionary<HashString, ClientDataStructure> CacheNames = AutoCSer.DictionaryCreator.CreateHashString<ClientDataStructure>();
        /// <summary>
        /// 数据结构定义信息集合访问锁
        /// </summary>
        private readonly object cacheNameLock = new object();
        /// <summary>
        /// 客户端套接字编号
        /// </summary>
        internal ulong SocketIdentity;
        /// <summary>
        /// 缓存主服务客户端套接字是否可用
        /// </summary>
        private volatile bool isMasterSocket;
        /// <summary>
        /// 是否使用主服务客户端
        /// </summary>
        private bool isMasterClient
        {
            get { return isMasterSocket || slaveClient == null; }
        }
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="masterClient">缓存主服务客户端</param>
        public Client(MasterServer.TcpInternalClient masterClient = null)
        {
            MasterClient = masterClient ?? new MasterServer.TcpInternalClient();
            checkSocketVersion = MasterClient._TcpClient_.CreateCheckSocketVersion(onClientSocket);
            isMasterSocket = true;
        }
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="masterClient">缓存主服务客户端</param>
        /// <param name="slaveClient">缓存从服务客户端</param>
        public Client(MasterServer.TcpInternalClient masterClient, SlaveServer.TcpInternalClient slaveClient)
        {
            MasterClient = masterClient ?? new MasterServer.TcpInternalClient();
            this.slaveClient = slaveClient ?? new SlaveServer.TcpInternalClient();
            checkSocketVersion = MasterClient._TcpClient_.CreateCheckSocketVersion(onClientSocket);
            slaveCheckSocketVersion = this.slaveClient._TcpClient_.CreateCheckSocketVersion(onSlaveClientSocket);
        }
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="slaveClient">缓存从服务客户端</param>
        public Client(SlaveServer.TcpInternalClient slaveClient)
        {
            this.slaveClient = slaveClient ?? new SlaveServer.TcpInternalClient();
            slaveCheckSocketVersion = this.slaveClient._TcpClient_.CreateCheckSocketVersion(onSlaveClientSocket);
            isMasterSocket = false;
        }
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="masterAttribute">缓存主服务端配置</param>
        /// <param name="slaveAttribute">缓存从服务端配置</param>
        public Client(AutoCSer.Net.TcpInternalServer.ServerAttribute masterAttribute, AutoCSer.Net.TcpInternalServer.ServerAttribute slaveAttribute)
            : this(new MasterServer.TcpInternalClient(masterAttribute), new SlaveServer.TcpInternalClient(slaveAttribute)) { }
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="masterAttribute">缓存主服务端配置</param>
        public Client(AutoCSer.Net.TcpInternalServer.ServerAttribute masterAttribute) : this(new MasterServer.TcpInternalClient(masterAttribute)) { }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (MasterClient != null) MasterClient.Dispose();
            if (slaveClient != null) slaveClient.Dispose();
            isMasterSocket = false;
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            if (slaveClient != null) isMasterSocket = parameter.Type == AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SetSocket;
            ++SocketIdentity;
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onSlaveClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            if (!isMasterSocket) ++SocketIdentity;
        }

        /// <summary>
        /// 获取或者创建数据结构信息
        /// </summary>
        /// <param name="dataStructure"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.ReturnValue<IndexIdentity> GetOrCreate(ClientDataStructure dataStructure)
        {
            if (isMasterClient) return MasterClient.GetOrCreate(new OperationParameter.ClientDataStructure { DataStructure = dataStructure });
            return slaveClient.Get(new OperationParameter.ClientDataStructure { DataStructure = dataStructure });
        }
        /// <summary>
        /// 获取或者创建数据结构信息
        /// </summary>
        /// <typeparam name="valueType">数据结构定义节点类型</typeparam>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns>数据结构定义节点</returns>
        public ReturnValue<valueType> GetOrCreateDataStructure<valueType>(string cacheName)
            where valueType : DataStructure.Abstract.Node
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = typeof(valueType).FullName;
            HashString hashName = cacheName;
            ClientDataStructure dataStructure;
            Monitor.Enter(cacheNameLock);
            try
            {
                if (CacheNames.TryGetValue(hashName, out dataStructure))
                {
                    if (dataStructure.NodeType == typeof(valueType)) return (valueType)dataStructure.Node;
                    return new ReturnValue<valueType> { Type = ReturnType.DataStructureNameExists };
                }
                Func<DataStructure.Abstract.Node, valueType> constructor = (Func<DataStructure.Abstract.Node, valueType>)DataStructure.Abstract.Node.GetConstructor(typeof(valueType));
                if (constructor != null)
                {
                    valueType node = constructor(null);
                    dataStructure = new ClientDataStructure(this, cacheName, typeof(valueType), node);
                    IndexIdentity identity = dataStructure.Identity;
                    if (identity.ReturnType == ReturnType.Success)
                    {
                        CacheNames.Add(hashName, dataStructure);
                        return node;
                    }
                    return new ReturnValue<valueType> { Type = identity.ReturnType, TcpReturnType = identity.TcpReturnType };
                }
            }
            finally { Monitor.Exit(cacheNameLock); }
            return new ReturnValue<valueType> { Type = ReturnType.DataStructureNotFoundConstructor };
        }

        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        public ReturnValue<bool> RemoveDataStructure(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) return new ReturnValue<bool> { Type = ReturnType.NullArgument };
            IndexIdentity identity = default(IndexIdentity);
            identity.Set(MasterClient.Remove(new OperationParameter.RemoveDataStructure { CacheName = cacheName }));
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
        public void RemoveDataStructure(string cacheName, Action<ReturnValue<bool>> onRemove)
        {
            if (string.IsNullOrEmpty(cacheName))
            {
                onRemove(new ReturnValue<bool> { Type = ReturnType.NullArgument });
                return;
            }
            try
            {
                MasterClient.Remove(new OperationParameter.RemoveDataStructure { CacheName = cacheName }, returnValue =>
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
        /// 数据立即写入文件
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.ReturnValue WriteFile()
        {
            return MasterClient.WriteFile();
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="onWrite"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteFile(Action<AutoCSer.Net.TcpServer.ReturnValue> onWrite)
        {
            MasterClient.WriteFile(onWrite);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Operation(DataStructure.Abstract.Node node)
        {
            return MasterClient.Operation(new OperationParameter.OperationNode { Node = node });
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationOnly(DataStructure.Abstract.Node node)
        {
            MasterClient.OperationOnly(new OperationParameter.OperationNode { Node = node });
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Operation(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            MasterClient.Operation(new OperationParameter.OperationNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            MasterClient.OperationStream(new OperationParameter.OperationNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationReturnParameter(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn != null) Operation(node, onReturn);
            else OperationOnly(node);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Operation(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn != null) Operation(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
            else OperationOnly(node);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationStream(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            OperationStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Operation<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet != null) Operation(node, value => onGet(new ReturnValue<valueType>(ref value)));
            else OperationOnly(node);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationStream<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            OperationStream(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }

        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> OperationAsynchronous(DataStructure.Abstract.Node node)
        {
            return MasterClient.OperationAsynchronous(new OperationParameter.OperationNode { Node = node });
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronousOnly(DataStructure.Abstract.Node node)
        {
            MasterClient.OperationAsynchronousOnly(new OperationParameter.OperationNode { Node = node });
        }
        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronous(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            MasterClient.OperationAsynchronous(new OperationParameter.OperationNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronousStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            MasterClient.OperationAsynchronousStream(new OperationParameter.OperationNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronousReturnParameter(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn != null) OperationAsynchronous(node, onReturn);
            else OperationAsynchronousOnly(node);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronous(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn != null) OperationAsynchronous(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
            else OperationAsynchronousOnly(node);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronousStream(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            OperationAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Query(DataStructure.Abstract.Node node)
        {
            if(isMasterClient) return MasterClient.Query(new OperationParameter.QueryNode { Node = node });
            return slaveClient.Query(new OperationParameter.QueryNode { Node = node });
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) MasterClient.Query(new OperationParameter.QueryNode { Node = node }, onReturn);
            else slaveClient.Query(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) MasterClient.QueryStream(new OperationParameter.QueryNode { Node = node }, onReturn);
            else slaveClient.QueryStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryOnly(DataStructure.Abstract.Node node)
        {
            if (isMasterClient) MasterClient.QueryOnly(new OperationParameter.QueryNode { Node = node });
            else slaveClient.QueryOnly(new OperationParameter.QueryNode { Node = node });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryOnly(DataStructure.Abstract.Node node)
        {
            MasterClient.QueryOnly(new OperationParameter.QueryNode { Node = node });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query(DataStructure.Abstract.Node node, Action<ReturnValue<int>> onGet)
        {
            Query(node, value => onGet(value.Value.Parameter.GetInt(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream(DataStructure.Abstract.Node node, Action<ReturnValue<int>> onGet)
        {
            QueryStream(node, value => onGet(value.Value.Parameter.GetInt(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onGet)
        {
            Query(node, value => onGet(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onGet)
        {
            QueryStream(node, value => onGet(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            Query(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            QueryStream(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> QueryAsynchronous(DataStructure.Abstract.Node node)
        {
            if (isMasterClient) return MasterClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node });
            return slaveClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node });
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronous(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) MasterClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node }, onReturn);
            else slaveClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronousStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) MasterClient.QueryAsynchronousStream(new OperationParameter.QueryNode { Node = node }, onReturn);
            else slaveClient.QueryAsynchronousStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronous(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            QueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronousStream(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            QueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronous(DataStructure.Abstract.Node node, Action<ReturnValue<ulong>> onReturn)
        {
            QueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetULong(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronousStream(DataStructure.Abstract.Node node, Action<ReturnValue<ulong>> onReturn)
        {
            QueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetULong(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronous<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType[]>> onGet)
        {
            QueryAsynchronous(node, value => onGet(ReturnArray<valueType>.Get(value)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronousStream<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType[]>> onGet)
        {
            QueryAsynchronousStream(node, value => onGet(ReturnArray<valueType>.Get(value)));
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> MasterQueryAsynchronous(DataStructure.Abstract.Node node)
        {
            return MasterClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node });
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronous(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            MasterClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronousStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            MasterClient.QueryAsynchronousStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronous(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            MasterQueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronousStream(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            MasterQueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronous(DataStructure.Abstract.Node node, Action<ReturnValue<ulong>> onReturn)
        {
            MasterQueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetULong(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronousStream(DataStructure.Abstract.Node node, Action<ReturnValue<ulong>> onReturn)
        {
            MasterQueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetULong(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronous(DataStructure.Abstract.Node node, Action<ReturnValue<uint>> onReturn)
        {
            MasterQueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetUInt(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronousStream(DataStructure.Abstract.Node node, Action<ReturnValue<uint>> onReturn)
        {
            MasterQueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetUInt(value.Type)));
        }

        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallback(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) return MasterClient.QueryKeepCallback(new OperationParameter.QueryNode { Node = node }, onReturn);
            return slaveClient.QueryKeepCallback(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallbackStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) return MasterClient.QueryKeepCallbackStream(new OperationParameter.QueryNode { Node = node }, onReturn);
            return slaveClient.QueryKeepCallbackStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallback<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            return QueryKeepCallback(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallbackStream<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            return QueryKeepCallbackStream(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }

        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallback(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            return MasterClient.QueryKeepCallback(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallbackStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            return MasterClient.QueryKeepCallbackStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallback<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            return MasterQueryKeepCallback(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallbackStream<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<valueType>> onGet)
        {
            return MasterQueryKeepCallbackStream(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }

        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallback(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>> onReturn)
        {
            return MasterClient.QueryKeepCallback(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallbackStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>> onReturn)
        {
            return MasterClient.QueryKeepCallbackStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallback<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<KeyValue<ulong, valueType>>> onGet)
        {
            return MasterQueryKeepCallback(node, value => onGet(ReturnValue<valueType>.Get(ref value)));
        }
        /// <summary>
        /// 异步查询数据（保持回调）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback MasterQueryKeepCallbackStream<valueType>(DataStructure.Abstract.Node node, Action<ReturnValue<KeyValue<ulong, valueType>>> onGet)
        {
            return MasterQueryKeepCallbackStream(node, value => onGet(ReturnValue<valueType>.Get(ref value)));
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Operation(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = MasterClient.Operation(new OperationParameter.ShortPathOperationNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                {
                    return MasterClient.Operation(new OperationParameter.ShortPathOperationNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void OperationNotNull(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                MasterClient.Operation(new OperationParameter.ShortPathOperationNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        MasterClient.Operation(new OperationParameter.ShortPathOperationNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
            }
            else onReturn(new ReturnParameter(returnType));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void OperationStream(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                MasterClient.OperationStream(new OperationParameter.ShortPathOperationNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        MasterClient.OperationStream(new OperationParameter.ShortPathOperationNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
            }
            else onReturn(new ReturnParameter(returnType));
        }
        
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationNotNull(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            OperationNotNull(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationStream(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            OperationStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationNotNull<valueType>(ShortPath.Parameter.Node node, Action<ReturnValue<valueType>> onGet)
        {
            OperationNotNull(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationStream<valueType>(ShortPath.Parameter.Node node, Action<ReturnValue<valueType>> onGet)
        {
            OperationStream(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }

        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> OperationAsynchronous(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = MasterClient.OperationAsynchronous(new OperationParameter.ShortPathOperationNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                {
                    return MasterClient.OperationAsynchronous(new OperationParameter.ShortPathOperationNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void OperationAsynchronousNotNull(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                MasterClient.OperationAsynchronous(new OperationParameter.ShortPathOperationNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        MasterClient.OperationAsynchronous(new OperationParameter.ShortPathOperationNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
            }
            else onReturn(new ReturnParameter(returnType));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void OperationAsynchronousStream(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                MasterClient.OperationAsynchronousStream(new OperationParameter.ShortPathOperationNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        MasterClient.OperationAsynchronousStream(new OperationParameter.ShortPathOperationNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
            }
            else onReturn(new ReturnParameter(returnType));
        }
        
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronousNotNull(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            OperationAsynchronousNotNull(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationAsynchronousStream(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            OperationAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Query(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType;
            if (isMasterClient)
            {
                if ((returnType = shortPath.Check(MasterClient)) == ReturnType.Success)
                {
                    AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = MasterClient.Query(new OperationParameter.ShortPathQueryNode { Node = node });
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        return MasterClient.Query(new OperationParameter.ShortPathQueryNode { Node = node });
                    }
                    return value;
                }
            }
            else if ((returnType = shortPath.Check(slaveClient)) == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = slaveClient.Query(new OperationParameter.ShortPathQueryNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                {
                    return slaveClient.Query(new OperationParameter.ShortPathQueryNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void Query(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType;
            if (isMasterClient)
            {
                if ((returnType = shortPath.Check(MasterClient)) == ReturnType.Success)
                {
                    MasterClient.Query(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                    {
                        if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                        {
                            MasterClient.Query(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                        }
                        else onReturn(value);
                    });
                    return;
                }
            }
            else if ((returnType = shortPath.Check(slaveClient)) == ReturnType.Success)
            {
                slaveClient.Query(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                    {
                        slaveClient.Query(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
                return;
            }
            onReturn(new ReturnParameter(returnType));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void QueryStream(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType;
            if (isMasterClient)
            {
                if ((returnType = shortPath.Check(MasterClient)) == ReturnType.Success)
                {
                    MasterClient.QueryStream(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                    {
                        if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                        {
                            MasterClient.QueryStream(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                        }
                        else onReturn(value);
                    });
                    return;
                }
            }
            else if ((returnType = shortPath.Check(slaveClient)) == ReturnType.Success)
            {
                slaveClient.QueryStream(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                    {
                        slaveClient.QueryStream(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
                return;
            }
            onReturn(new ReturnParameter(returnType));
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query(ShortPath.Parameter.Node node, Action<ReturnValue<int>> onGet)
        {
            Query(node, value => onGet(value.Value.Parameter.GetInt(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream(ShortPath.Parameter.Node node, Action<ReturnValue<int>> onGet)
        {
            QueryStream(node, value => onGet(value.Value.Parameter.GetInt(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onGet)
        {
            Query(node, value => onGet(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onGet)
        {
            QueryStream(node, value => onGet(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query<valueType>(ShortPath.Parameter.Node node, Action<ReturnValue<valueType>> onGet)
        {
            Query(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream<valueType>(ShortPath.Parameter.Node node, Action<ReturnValue<valueType>> onGet)
        {
            QueryStream(node, value => onGet(new ReturnValue<valueType>(ref value)));
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> QueryAsynchronous(ShortPath.Parameter.Node node)
        {
            if (isMasterClient) return MasterQueryAsynchronous(node);
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(slaveClient);
            if (returnType == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = slaveClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                {
                    return slaveClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void QueryAsynchronous(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) MasterQueryAsynchronous(node, onReturn);
            else
            {
                ShortPath.Node shortPath = node.ShortPath;
                ReturnType returnType = shortPath.Check(slaveClient);
                if (returnType == ReturnType.Success)
                {
                    slaveClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                    {
                        if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                        {
                            slaveClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                        }
                        else onReturn(value);
                    });
                }
                else onReturn(new ReturnParameter(returnType));
            }
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void QueryAsynchronousStream(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (isMasterClient) MasterQueryAsynchronousStream(node, onReturn);
            else
            {
                ShortPath.Node shortPath = node.ShortPath;
                ReturnType returnType = shortPath.Check(slaveClient);
                if (returnType == ReturnType.Success)
                {
                    slaveClient.QueryAsynchronousStream(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                    {
                        if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                        {
                            slaveClient.QueryAsynchronousStream(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                        }
                        else onReturn(value);
                    });
                }
                else onReturn(new ReturnParameter(returnType));
            }
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronous(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            QueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronousStream(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            QueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronous(ShortPath.Parameter.Node node, Action<ReturnValue<ulong>> onReturn)
        {
            QueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetULong(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronousStream(ShortPath.Parameter.Node node, Action<ReturnValue<ulong>> onReturn)
        {
            QueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetULong(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronous<valueType>(ShortPath.Parameter.Node node, Action<ReturnValue<valueType[]>> onGet)
        {
            QueryAsynchronous(node, value => onGet(ReturnArray<valueType>.Get(value)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryAsynchronousStream<valueType>(ShortPath.Parameter.Node node, Action<ReturnValue<valueType[]>> onGet)
        {
            QueryAsynchronousStream(node, value => onGet(ReturnArray<valueType>.Get(value)));
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        internal AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> MasterQueryAsynchronous(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = MasterClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                {
                    return MasterClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void MasterQueryAsynchronous(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                MasterClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        MasterClient.QueryAsynchronous(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
            }
            else onReturn(new ReturnParameter(returnType));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal void MasterQueryAsynchronousStream(ShortPath.Parameter.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                MasterClient.QueryAsynchronousStream(new OperationParameter.ShortPathQueryNode { Node = node }, value =>
                {
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        MasterClient.QueryAsynchronousStream(new OperationParameter.ShortPathQueryNode { Node = node }, onReturn);
                    }
                    else onReturn(value);
                });
            }
            else onReturn(new ReturnParameter(returnType));
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronous(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            MasterQueryAsynchronous(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MasterQueryAsynchronousStream(ShortPath.Parameter.Node node, Action<ReturnValue<bool>> onReturn)
        {
            MasterQueryAsynchronousStream(node, value => onReturn(value.Value.Parameter.GetBool(value.Type)));
        }
    }
}
