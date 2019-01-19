using System;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 默认 TCP 注册服务包装
    /// </summary>
    public sealed class DefaultServer : IDisposable
    {
        /// <summary>
        /// TCP 内部注册读取服务对象
        /// </summary>
        public ReaderServer Server { get; private set; }
        /// <summary>
        /// TCP 内部注册写服务
        /// </summary>
        private Server.TcpInternalServer server;
        /// <summary>
        /// TCP 内部注册读取服务
        /// </summary>
        private ReaderServer.TcpInternalServer readerServer;
        /// <summary>
        /// 默认 TCP 注册服务包装
        /// </summary>
        /// <param name="serverTarget">TCP 内部注册读取服务对象</param>
        /// <param name="server">TCP 内部注册写服务</param>
        /// <param name="readerServer">TCP 内部注册读取服务</param>
        private DefaultServer(ReaderServer serverTarget, Server.TcpInternalServer server, ReaderServer.TcpInternalServer readerServer)
        {
            Server = serverTarget;
            this.server = server;
            this.readerServer = readerServer;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (readerServer != null)
            {
                readerServer.Dispose();
                readerServer = null;
            }
            if (server != null)
            {
                server.Dispose();
                server = null;
            }
        }
        /// <summary>
        /// 创建默认 TCP 注册服务包装
        /// </summary>
        /// <returns></returns>
        public static DefaultServer Create()
        {
            Server.TcpInternalServer server = null;
            ReaderServer.TcpInternalServer readerServer = null;
            bool isListen = false;
            try
            {
                ReaderServer serverTarget = ReaderServer.Create();
                server = new Server.TcpInternalServer(null, null, serverTarget.Server);
                if (server.IsListen)
                {
                    readerServer = new ReaderServer.TcpInternalServer(null, null, serverTarget);
                    if (readerServer.IsListen)
                    {
                        DefaultServer defaultServer = new DefaultServer(serverTarget, server, readerServer);
                        isListen = true;
                        return defaultServer;
                    }
                }
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                if (!isListen)
                {
                    if (server != null) server.Dispose();
                    if (readerServer != null) readerServer.Dispose();
                }
            }
            return null;
        }
    }
}
