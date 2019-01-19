using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册读取服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = ReaderServer.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.TcpRegisterReader)]
    public partial class ReaderServer : TcpInternalServer.TimeVerifyServer
    {
        /// <summary>
        /// 读取服务名称后缀
        /// </summary>
        public const string ServerNameSuffix = "Reader";
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = Server.ServerName + ServerNameSuffix;
        /// <summary>
        /// TCP注册服务
        /// </summary>
        public Server Server { get; private set; }
        /// <summary>
        /// TCP 服务端注册
        /// </summary>
        /// <returns>TCP 服务端标识</returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private ClientId register()
        {
            return Server.Register();
        }
        /// <summary>
        /// TCP 服务端轮询
        /// </summary>
        /// <param name="clientId">TCP 服务端标识</param>
        /// <param name="onLog">TCP 服务注册通知委托</param>
        [TcpServer.KeepCallbackMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.TcpQueue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getLog(ClientId clientId, Func<TcpServer.ReturnValue<Log>, bool> onLog)
        {
            Server.GetLog(ref clientId, onLog);
        }

        /// <summary>
        /// 创建 TCP 注册服务目标对象
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReaderServer Create()
        {
            return new ReaderServer { Server = new Server() };
        }
    }
}
