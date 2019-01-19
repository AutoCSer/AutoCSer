using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 客户端 TCP 服务信息集合
    /// </summary>
    internal sealed class ClientServerSet
    {
        /// <summary>
        /// TCP 服务信息集合
        /// </summary>
        private ServerSet serverSet;
        /// <summary>
        /// 客户端集合
        /// </summary>
        private LeftArray<IClient> clients;
        /// <summary>
        /// 客户端 TCP 服务信息集合
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        internal ClientServerSet(IClient client)
        {
            clients.Add(client);
        }
        /// <summary>
        /// 客户端 TCP 服务信息集合
        /// </summary>
        /// <param name="log"></param>
        internal ClientServerSet(Log log)
        {
            serverSet = new ServerSet(log);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Clear()
        {
            if (clients.Length == 0) return true;
            if (serverSet != null) serverSet.ClientClear();
            return false;
        }
        /// <summary>
        /// 添加 TCP 客户端
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        /// <param name="isLoaded"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(IClient client, bool isLoaded)
        {
            clients.Add(client);
            if (isLoaded) client.OnServerChange(serverSet != null && serverSet.Server != null ? serverSet : null);
        }
        /// <summary>
        /// 移除 TCP 客户端
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Remove(IClient client)
        {
            clients.Remove(client);
            return clients.Count == 0 && (serverSet == null || serverSet.Server == null);
        }
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="log"></param>
        /// <param name="isLoaded"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(Log log, bool isLoaded)
        {
            if (serverSet == null) serverSet = new ServerSet(log);
            else if (serverSet.Server == null) serverSet.Server = log;
            else serverSet.Add(log);
            if (isLoaded) onServerChange(serverSet);
        }
        /// <summary>
        /// 移除服务
        /// </summary>
        /// <param name="log"></param>
        /// <param name="isLoaded"></param>
        /// <returns></returns>
        internal bool Remove(Log log, bool isLoaded)
        {
            if (serverSet != null)
            {
                Log oldLog = serverSet.Server;
                if (oldLog != null)
                {
                    if (serverSet.Remove(log.Server) == oldLog && serverSet.Server != null && isLoaded)
                    {
                        onServerChange(serverSet);
                        return false;
                    }
                    if (serverSet.Server != null) return false;
                }
            }
            return clients.Length == 0;
        }
        /// <summary>
        /// TCP 服务更新
        /// </summary>
        /// <param name="serverSet"></param>
        private void onServerChange(ServerSet serverSet)
        {
            foreach (IClient client in clients) client.OnServerChange(serverSet);
        }
        /// <summary>
        /// 服务加载完毕
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnLoaded()
        {
            onServerChange(serverSet != null && serverSet.Server != null ? serverSet : null);
        }
    }
}
