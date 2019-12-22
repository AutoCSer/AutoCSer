using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using AutoCSer.Extension;

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
        internal ServerLog Server;
        /// <summary>
        /// TCP 服务注册信息集合
        /// </summary>
        internal LeftArray<ServerLog> Servers;
        /// <summary>
        /// 服务信息数量
        /// </summary>
        public int Count
        {
            get { return (Server != null ? 1 : 0) + Servers.Length; }
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="index">位置索引</param>
        /// <returns></returns>
        public ServerLog this[int index]
        {
            get
            {
                if (index == 0) return Server;
                return Servers[ - 1];
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServerLog> GetServers()
        {
            if (Server != null) yield return Server;
            foreach (ServerLog server in Servers) yield return server;
        }
        /// <summary>
        /// TCP 服务信息集合
        /// </summary>
        /// <param name="server"></param>
        internal ServerSet(ServerLog server)
        {
            Server = server;
        }
        /// <summary>
        /// 添加 TCP 服务注册信息
        /// </summary>
        /// <param name="server"></param>
        /// <param name="isMainChanged">主服务是否被修改</param>
        /// <returns>日志是否需要推送到客户端</returns>
        internal bool Add(ServerLog server, out bool isMainChanged)
        {
            if (Server == null)
            {
                Server = server;
                return isMainChanged = true;
            }
            server.Name = Server.Name;
            if (server.IsSingle ^ Server.IsSingle)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Warn, "TCP 服务 " + server.Name + " 单实例定义冲突 " + server.IsSingle.ToString());
            }
            if (server.HostPortEquals(Server))
            {
                if (server.Random == Server.Random) return isMainChanged = false;
                if (server.IsSingle) Servers.Length = 0;
                Server = server;
                isMainChanged = false;
                return true;
            }
            if (server.IsSingle || Server.IsSingle)
            {
                Servers.Length = 0;
                Server = server;
                return isMainChanged = true;
            }
            int index = indexOf(server);
            if (!Server.IsMain || server.IsMain)
            {
                Server.IsMain = false;
                if (index < 0) Servers.Add(Server);
                else Servers.Array[index] = Server;
                Server = server;
                return isMainChanged = true;
            }
            isMainChanged = false;
            if (index < 0)
            {
                Servers.Add(server);
                return true;
            }
            if (server.Random == Servers.Array[index].Random) return false;
            Servers.Array[index] = server;
            return true;
        }
        /// <summary>
        /// 查找匹配服务位置
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        private int indexOf(ServerLog server)
        {
            if (Servers.Length == 0) return -1;
            int count = Servers.Length;
            ServerLog[] serverArray = Servers.Array;
            foreach (ServerLog nextServer in serverArray)
            {
                if (server.HostPortEquals(nextServer)) return Servers.Length - count;
                if (--count == 0) return -1;
            }
            return -1;
        }
        /// <summary>
        /// TCP 内部注册服务日志初始化回调
        /// </summary>
        /// <param name="onLog"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool OnLog(AutoCSer.Net.TcpServer.ServerCallback<ServerLog> onLog)
        {
            foreach (ServerLog log in Servers)
            {
                if (!onLog.Callback(log)) return false;
            }
            return onLog.Callback(Server);
        }
        /// <summary>
        /// 移除 TCP 服务注册信息
        /// </summary>
        /// <param name="server"></param>
        /// <returns>主服务是否被修改</returns>
        internal bool Remove(ServerLog server)
        {
            if (Server != null)
            {
                if (server.HostPortEquals(Server))
                {
                    if (server.Random == Server.Random)
                    {
                        Server = Servers.Length == 0 ? null : Servers.Array[--Servers.Length];
                        return true;
                    }
                }
                else
                {
                    int index = indexOf(server);
                    if (index >= 0 && server.Random == Servers.Array[index].Random) Servers.Array[index] = Servers.Array[--Servers.Length];
                }
            }
            return false;
        }
    }
}
