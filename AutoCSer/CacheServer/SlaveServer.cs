using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存从服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = SlaveServer.ServerName, Host = "127.0.0.1", Port = (int)AutoCSer.Net.ServerPort.SlaveCacheServer, IsInternalClient = true, IsAutoClient = false, CheckSeconds = 1)]
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
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private AutoCSer.Net.TcpServer.CheckSocketVersion checkSocketVersion;
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
            checkSocketVersion = Config.IsMasterCacheServer ? masterClient._TcpClient_.CreateCheckSocketVersion(onClientSocket) : slaveClient._TcpClient_.CreateCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            if (parameter.Type == AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SetSocket)
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

        /// <summary>
        /// 创建缓存服务静态路由集群节点
        /// </summary>
        /// <param name="index">节点编号</param>
        /// <param name="attribute">TCP 调用服务器端配置信息</param>
        /// <param name="log">日志接口</param>
        public TcpInternalServer CreateStaticRoute(int index, AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
        {
            return CreateStaticRoute(index, attribute, this, log);
        }
        /// <summary>
        /// 创建缓存服务静态路由集群节点
        /// </summary>
        /// <param name="index">节点编号</param>
        /// <param name="attribute">TCP 调用服务器端配置信息</param>
        /// <param name="value">TCP 服务目标对象</param>
        /// <param name="log">日志接口</param>
        public static TcpInternalServer CreateStaticRoute(int index, AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, SlaveServer value = null, AutoCSer.Log.ILog log = null)
        {
            return new TcpInternalServer(CreateStaticRouteAttribute(index, attribute ?? AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig(ServerName, typeof(SlaveServer))), null, value, null, log);
        }
    }
}
