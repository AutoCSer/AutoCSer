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
        private LeftArray<IClient> clients = new LeftArray<IClient>(0);
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
        internal ClientServerSet(ServerLog log)
        {
            serverSet = new ServerSet(log);
        }
        ///// <summary>
        ///// 清除数据
        ///// </summary>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal bool Clear()
        //{
        //    if (clients.Length == 0) return true;
        //    if (serverSet != null) serverSet.ClientClear();
        //    return false;
        //}
        /// <summary>
        /// 添加 TCP 客户端
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(IClient client)
        {
            clients.Add(client);
            client.OnServerChange(serverSet != null && serverSet.Server != null ? serverSet : null);
        }
        /// <summary>
        /// 移除 TCP 客户端
        /// </summary>
        /// <param name="client"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Remove(IClient client)
        {
            clients.Remove(client);
        }
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="log"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(ServerLog log)
        {
            bool isMainChanged;
            if (serverSet == null)
            {
                serverSet = new ServerSet(log);
                isMainChanged = true;
            }
            else serverSet.Add(log, out isMainChanged);
            if (isMainChanged) onServerChange(serverSet);
        }
        /// <summary>
        /// 移除服务
        /// </summary>
        /// <param name="log"></param>
        internal void Remove(ServerLog log)
        {
            if (serverSet != null && serverSet.Remove(log)) onServerChange(serverSet);
        }
        /// <summary>
        /// TCP 服务更新
        /// </summary>
        /// <param name="serverSet"></param>
        private void onServerChange(ServerSet serverSet)
        {
            if (serverSet.Server == null) serverSet = null;
            foreach (IClient client in clients) client.OnServerChange(serverSet);
        }
    }
}
