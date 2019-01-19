using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务集群客户端静态路由
    /// </summary>
    public sealed partial class StaticRoute : IDisposable
    {
        /// <summary>
        /// 客户端集合
        /// </summary>
        private readonly Client[] clients;
        /// <summary>
        /// 客户端集合访问锁
        /// </summary>
        private readonly object clientLock = new object();
        /// <summary>
        /// 缓存主服务端配置
        /// </summary>
        private readonly AutoCSer.Net.TcpInternalServer.ServerAttribute masterAttribute;
        /// <summary>
        /// 缓存从服务端配置
        /// </summary>
        private readonly AutoCSer.Net.TcpInternalServer.ServerAttribute slaveAttribute;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// 缓存服务集群客户端静态路由
        /// </summary>
        /// <param name="masterAttribute">缓存主服务端配置</param>
        /// <param name="slaveAttribute">缓存从服务端配置</param>
        /// <param name="count">集群节点数量</param>
        private StaticRoute(AutoCSer.Net.TcpInternalServer.ServerAttribute masterAttribute, AutoCSer.Net.TcpInternalServer.ServerAttribute slaveAttribute, int count)
        {
            clients = new Client[Math.Max(count, 1)];
            this.masterAttribute = masterAttribute;
            this.slaveAttribute = slaveAttribute;
        }
        /// <summary>
        /// 缓存服务集群客户端静态路由
        /// </summary>
        /// <param name="count">集群节点数量</param>
        /// <param name="masterAttribute">缓存主服务端配置</param>
        /// <param name="slaveAttribute">缓存从服务端配置</param>
        public StaticRoute(int count, AutoCSer.Net.TcpInternalServer.ServerAttribute masterAttribute, AutoCSer.Net.TcpInternalServer.ServerAttribute slaveAttribute)
            : this(masterAttribute ?? AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig(MasterServer.ServerName, typeof(AutoCSer.CacheServer.MasterServer))
                  , slaveAttribute ?? AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig(SlaveServer.ServerName, typeof(AutoCSer.CacheServer.SlaveServer))
                  , count)
        {
        }
        /// <summary>
        /// 缓存服务集群客户端静态路由
        /// </summary>
        /// <param name="count">集群节点数量</param>
        /// <param name="masterAttribute">缓存主服务端配置</param>
        public StaticRoute(int count, AutoCSer.Net.TcpInternalServer.ServerAttribute masterAttribute)
            : this(masterAttribute ?? AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig(MasterServer.ServerName, typeof(AutoCSer.CacheServer.MasterServer)), null, count)
        {
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                foreach (Client client in clients)
                {
                    if (client != null) client.Dispose();
                }
                Array.Clear(clients, 0, clients.Length);
            }
        }
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Client getClient(string cacheName)
        {
            int index = cacheName.GetHashCode() % clients.Length;
            Client client = clients[index];
            return client ?? createClient(index);
        }
        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Client createClient(int index)
        {
            if (isDisposed == 0)
            {
                Monitor.Enter(clientLock);
                Client client = clients[index];
                if (client != null)
                {
                    Monitor.Exit(clientLock);
                    return client;
                }
                try
                {
                    client = slaveAttribute != null ? new Client(Server.CreateStaticRouteAttribute(index, masterAttribute), Server.CreateStaticRouteAttribute(index, slaveAttribute)) : new Client(Server.CreateStaticRouteAttribute(index, masterAttribute));
                    clients[index] = client;
                }
                finally { Monitor.Exit(clientLock); }
                return client;
            }
            return null;
        }
        /// <summary>
        /// 获取或者创建数据结构信息
        /// </summary>
        /// <typeparam name="valueType">数据结构定义节点类型</typeparam>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns>数据结构定义节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> GetOrCreateDataStructure<valueType>(string cacheName)
            where valueType : DataStructure.Abstract.Node
        {
            return getClient(cacheName).GetOrCreateDataStructure<valueType>(cacheName);
        }
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> RemoveDataStructure(string cacheName)
        {
            return getClient(cacheName).RemoveDataStructure(cacheName);
        }
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="onRemove"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void RemoveDataStructure(string cacheName, Action<ReturnValue<bool>> onRemove)
        {
            getClient(cacheName).RemoveDataStructure(cacheName, onRemove);
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <returns>错误节点数量</returns>
        public int WriteFile()
        {
            int errorCount = 0;
            for (int index = clients.Length; index != 0;)
            {
                --index;
                if ((clients[index] ?? createClient(index)).MasterClient.WriteFile().Type != AutoCSer.Net.TcpServer.ReturnType.Success) ++errorCount;
            }
            return errorCount;
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.ReturnValue WriteFile(string cacheName)
        {
            return getClient(cacheName).MasterClient.WriteFile();
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="onWrite"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteFile(string cacheName, Action<AutoCSer.Net.TcpServer.ReturnValue> onWrite)
        {
            getClient(cacheName).MasterClient.WriteFile(onWrite);
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="index">节点编号</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.ReturnValue WriteFile(int index)
        {
            return (clients[index] ?? createClient(index)).MasterClient.WriteFile();
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="index">节点编号</param>
        /// <param name="onWrite"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteFile(int index, Action<AutoCSer.Net.TcpServer.ReturnValue> onWrite)
        {
            (clients[index] ?? createClient(index)).MasterClient.WriteFile(onWrite);
        }
    }
}
