using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存从服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = SlaveServer.ServerName, Host = "127.0.0.1", Port = (int)AutoCSer.Net.ServerPort.SlaveCacheServer, IsInternalClient = true)]
    public partial class SlaveServer : Server
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "SlaveCache";
        /// <summary>
        /// 缓存从服务默认配置
        /// </summary>
        private static readonly SlaveServerConfig defaultConfig = ConfigLoader.GetUnion(typeof(SlaveServerConfig)).SlaveServerConfig ?? new SlaveServerConfig();
        /// <summary>
        /// 缓存从服务配置
        /// </summary>
        internal readonly SlaveServerConfig Config;
        /// <summary>
        /// 缓存主服务客户端
        /// </summary>
        private MasterServer.TcpInternalClient masterClient;
        /// <summary>
        /// 缓存从服务客户端
        /// </summary>
        private SlaveServer.TcpInternalClient slaveClient;
        /// <summary>
        /// 缓存主服务客户端 TCP 套接字更新访问锁
        /// </summary>
        private readonly object newSocketLock = new object();
        /// <summary>
        /// 当前缓存主服务客户端 TCP 套接字
        /// </summary>
        private AutoCSer.Net.TcpServer.ClientSocketBase socket;
        /// <summary>
        /// 加载缓存保持回调
        /// </summary>
        private AutoCSer.Net.TcpServer.KeepCallback loadCacheKeepCallback;
        /// <summary>
        /// 当前加载缓存
        /// </summary>
        private CacheManager loadCache;
        /// <summary>
        /// 缓存主服务
        /// </summary>
        public SlaveServer() : this(null) { }
        /// <summary>
        /// 缓存主服务
        /// </summary>
        /// <param name="config">缓存主服务配置</param>
        public SlaveServer(SlaveServerConfig config) : base()
        {
            Config = config ?? defaultConfig;
            if (Config.IsMasterCacheServer) masterClient = new MasterServer.TcpInternalClient(Config.CacheServerAttribute);
            else slaveClient = new SlaveServer.TcpInternalClient(Config.CacheServerAttribute);
        }
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="tcpServer">TCP服务端</param>
        public override void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            base.SetTcpServer(tcpServer);
            if (Config.IsMasterCacheServer) masterClient._TcpClient_.OnSetSocket(onClientSocket);
            else slaveClient._TcpClient_.OnSetSocket(onClientSocket);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="socket"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketBase socket)
        {
            Monitor.Enter(newSocketLock);
            try
            {
                if (socket.IsSocketVersion(ref this.socket))
                {
                    if (loadCacheKeepCallback != null)
                    {
                        loadCacheKeepCallback.Dispose();
                        loadCacheKeepCallback = null;
                    }
                    CacheManager cache = new CacheManager(this, server);
                    if (masterClient != null)
                    {
                        if ((loadCacheKeepCallback = masterClient.GetCache(cache.Load)) != null) loadCache = cache;
                    }
                    else
                    {
                        if ((loadCacheKeepCallback = slaveClient.GetCache(cache.Load)) != null) loadCache = cache;
                    }
                }
            }
            finally { Monitor.Exit(newSocketLock); }
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="onCache">缓存数据回调委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        internal void GetCache(Func<AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter>, bool> onCache)
        {
            CacheManager cache = Cache;
            if (cache != null) AutoCSer.Net.TcpServer.ServerCallQueue.Default.Add(new ServerCall.GetCache(cache, onCache));
            else onCache(default(CacheReturnParameter));
        }

        /// <summary>
        /// 获取数据结构索引标识
        /// </summary>
        /// <param name="parameter">数据结构操作参数</param>
        /// <returns>数据结构索引标识</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false)]
        internal IndexIdentity Get(OperationParameter.ClientDataStructure parameter)
        {
            CacheManager cache = Cache;
            if (cache != null) return cache.GetDataStructure(parameter.Buffer);
            return new IndexIdentity { ReturnType = ReturnType.NotFoundSlaveCache };
        }
    }
}
