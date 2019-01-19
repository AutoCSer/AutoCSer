using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 服务信息集合
    /// </summary>
    public sealed class ServerSet
    {
        /// <summary>
        /// TCP 服务注册信息
        /// </summary>
        internal Log Server;
        /// <summary>
        /// TCP 服务注册信息集合
        /// </summary>
        internal LeftArray<Log> Servers;
        /// <summary>
        /// TCP 服务信息集合
        /// </summary>
        /// <param name="log"></param>
        internal ServerSet(Log log)
        {
            Server = log;
        }
        /// <summary>
        /// TCP 服务信息集合
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="registerServer"></param>
        internal ServerSet(ServerSetCache cache, Server registerServer)
        {
            Server = new Log { Type = LogType.RegisterServer, Server = cache.Server };
            registerServer.SetIpPort(cache.Server);
            if (cache.Servers != null)
            {
                Servers.PrepLength(cache.Servers.Length);
                foreach (ServerInfo server in cache.Servers)
                {
                    Servers.UnsafeAdd(new Log { Type = LogType.RegisterServer, Server = server });
                    registerServer.SetIpPort(server);
                }
            }
        }
        /// <summary>
        /// 添加 TCP 服务注册信息
        /// </summary>
        /// <param name="log"></param>
        internal void Add(Log log)
        {
            if (log.Server.IsSingle)
            {
                Server = log;
                Servers.ClearOnlyLength();
            }
            else
            {
                if (!Server.Server.IsSingle) Servers.Add(Server);
                Server = log;
                Servers.Remove(value => !value.Server.IsSingle);
            }
        }
        /// <summary>
        /// 失败重连检测 TCP 服务注册信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns>是否添加成功</returns>
        internal bool Check(Log log)
        {
            if (Server != null)
            {
                if (Server.Server.HostPortEquals(log.Server))
                {
                    Server = log;
                    return false;
                }
                int count = Servers.Length;
                if (count != 0)
                {
                    foreach (Log server in Servers.Array)
                    {
                        if (server.Server.HostPortEquals(log.Server))
                        {
                            Servers.Array[Servers.Length - count] = log;
                            return false;
                        }
                        if (--count == 0) break;
                    }
                }
            }
            Add(log);
            return true;
        }
        /// <summary>
        /// TCP 内部注册服务日志初始化回调
        /// </summary>
        /// <param name="onLog"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnLog(Func<TcpServer.ReturnValue<Log>, bool> onLog)
        {
            foreach (Log log in Servers) onLog(log);
            onLog(Server);
        }
        /// <summary>
        /// 移除 TCP 服务注册信息
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        internal Log Remove(ServerInfo server)
        {
            Log log = null;
            if (this.Server.Server.ClientEquals(server))
            {
                log = this.Server;
                if (Servers.Length == 0) this.Server = null;
                else this.Server = Servers.UnsafePop();
            }
            else
            {
                int index = Servers.IndexOf(value => value.Server.ClientEquals(server));
                if (index != -1)
                {
                    log = Servers.Array[index];
                    Servers.RemoveAt(index);
                }
            }
            return log;
        }

        /// <summary>
        /// 客户端清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ClientClear()
        {
            Server = null;
            Servers.ClearOnlyLength();
        }
    }
}
