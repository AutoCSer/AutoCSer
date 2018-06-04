using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存主服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = MasterServer.ServerName, Host = "127.0.0.1", Port = (int)AutoCSer.Net.ServerPort.MasterCacheServer, IsInternalClient = true)]
    public unsafe partial class MasterServer : Server
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "MasterCache";
        /// <summary>
        /// 缓存主服务默认配置
        /// </summary>
        private static readonly MasterServerConfig defaultConfig = ConfigLoader.GetUnion(typeof(MasterServerConfig)).MasterServerConfig ?? new MasterServerConfig();

        /// <summary>
        /// 缓存主服务配置
        /// </summary>
        internal readonly MasterServerConfig Config;
        /// <summary>
        /// 缓存主服务
        /// </summary>
        public MasterServer() : this(null) { }
        /// <summary>
        /// 缓存主服务
        /// </summary>
        /// <param name="config">缓存主服务配置</param>
        public MasterServer(MasterServerConfig config)
        {
            Config = config ?? defaultConfig;
            if (Config.MinCompressSize <= 0) Config.MinCompressSize = int.MaxValue;
        }
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="tcpServer">TCP服务端</param>
        public override void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            base.SetTcpServer(tcpServer);
            Cache = new CacheManager(Config, tcpServer);
        }
        /// <summary>
        /// 设置是否允许写操作
        /// </summary>
        public bool CanWrite
        {
            set
            {
                server.CallQueue.Add(new ServerCall.CacheManager(Cache, value ? ServerCall.CacheManagerServerCallType.SetCanWrite : ServerCall.CacheManagerServerCallType.CancelCanWrite));
            }
        }
        /// <summary>
        /// 设置是否允许写操作
        /// </summary>
        /// <param name="canWrite">是否允许写操作</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal void SetCanWrite(bool canWrite)
        {
            Cache.CanWrite = canWrite;
        }
        /// <summary>
        /// 获取物理文件版本
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false)]
        internal ulong GetFileVersion()
        {
            FileStreamWriter file = Cache.File;
            return file != null ? file.Version : 0;
        }
        /// <summary>
        /// 读取物理文件
        /// </summary>
        /// <param name="version"></param>
        /// <param name="index"></param>
        /// <param name="onRead"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        internal void ReadFile(ulong version, long index, Func<AutoCSer.Net.TcpServer.ReturnValue<ReadFileParameter>, bool> onRead)
        {
            FileStreamWriter file = Cache.File;
            if (file == null || !file.Read(version, index, onRead)) onRead(default(ReadFileParameter));
        }
        /// <summary>
        /// 重建文件流
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal ReturnType NewFileStream()
        {
            return Cache.NewFileStream();
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="onCache">缓存数据回调委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        internal void GetCache(Func<AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter>, bool> onCache)
        {
            server.CallQueue.Add(new ServerCall.GetCache(Cache, onCache));
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteFile()
        {
            Cache.WriteFile();
        }

        /// <summary>
        /// 添加数据结构定义
        /// </summary>
        /// <param name="parameter">数据结构操作参数</param>
        /// <returns>数据结构索引标识</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false)]
        internal IndexIdentity GetOrCreate(OperationParameter.ClientDataStructure parameter)
        {
            return Cache.GetOrCreateDataStructure(parameter.Buffer);
        }
        /// <summary>
        /// 删除数据结构定义
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal IndexIdentity Remove(OperationParameter.RemoveDataStructure parameter)
        {
            return Cache.RemoveDataStructure(parameter.Buffer);
        }

        /// <summary>
        /// 操作数据并返回参数
        /// </summary>
        /// <param name="parameter">数据结构定义节点操作参数</param>
        /// <returns>返回参数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal ReturnParameter Operation(OperationParameter.OperationNode parameter)
        {
            return Cache.Operation(parameter.Buffer);
        }
        /// <summary>
        /// 操作数据并返回参数
        /// </summary>
        /// <param name="parameter">数据结构定义节点操作参数</param>
        /// <returns>返回参数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        internal ReturnParameter OperationStream(OperationParameter.OperationNode parameter)
        {
            ReturnParameter returnValue = Cache.Operation(parameter.Buffer);
            returnValue.IsDeSerializeStream = true;
            return returnValue;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parameter">数据结构定义节点操作参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientSendOnly = true)]
        internal void OperationOnly(OperationParameter.OperationNode parameter)
        {
            Cache.Operation(parameter.Buffer);
        }

        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="parameter">数据结构定义节点操作参数</param>
        /// <param name="onOperation"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAsynchronous = true)]
        internal void OperationAsynchronous(OperationParameter.OperationNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation)
        {
            TcpServer.CallQueue.Add(new ServerCall.OperationAsynchronous(Cache, parameter.Buffer, onOperation));
        }
        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="parameter">数据结构定义节点操作参数</param>
        /// <param name="onOperation"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        internal void OperationAsynchronousStream(OperationParameter.OperationNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation)
        {
            TcpServer.CallQueue.Add(new ServerCall.OperationAsynchronous(Cache, parameter.Buffer, onOperation));
        }
        /// <summary>
        /// 模拟异步操作数据
        /// </summary>
        /// <param name="parameter">数据结构定义节点操作参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientSendOnly = true)]
        internal void OperationAsynchronousOnly(OperationParameter.OperationNode parameter)
        {
            Cache.Operation(parameter.Buffer, nullCallbackHandle);
        }

        /// <summary>
        /// 异步操作空回调
        /// </summary>
        private static readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> nullCallbackHandle = nullCallback;
        /// <summary>
        /// 异步操作空回调
        /// </summary>
        /// <param name="returnParameter"></param>
        /// <returns></returns>
        private static bool nullCallback(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> returnParameter)
        {
            return true;
        }
    }
}
