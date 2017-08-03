using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块客户端池
    /// </summary>
    internal static class ClientPool
    {
        /// <summary>
        /// 磁盘块客户端集合
        /// </summary>
        private static readonly Server.TcpInternalClient[] clients;
        /// <summary>
        /// 获取磁盘块客户端
        /// </summary>
        /// <param name="index">磁盘块索引位置</param>
        /// <returns>磁盘块客户端</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Server.TcpInternalClient Get(ulong index)
        {
            return Get((int)(index >> Server.IndexShift));
        }
        /// <summary>
        /// 获取磁盘块客户端
        /// </summary>
        /// <param name="index">磁盘块客户端编号</param>
        /// <returns>磁盘块客户端</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Server.TcpInternalClient Get(int index)
        {
            return index < clients.Length ? clients[index] : null;
        }

        static ClientPool()
        {
            ClientConfig config = ConfigLoader.GetUnion(typeof(ClientConfig)).ClientConfig ?? new ClientConfig();
            (clients = new Server.TcpInternalClient[Math.Max(config.Count, 1)])[0] = new Server.TcpInternalClient();
            Net.TcpInternalServer.ServerAttribute attribute = clients[0]._TcpClient_.Attribute;
            for (var index = clients.Length; index != 1; )
            {
                Net.TcpInternalServer.ServerAttribute copyAttribute = AutoCSer.MemberCopy.Copyer<Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(attribute);
                copyAttribute.Name = attribute.Name + (--index).toString();
                copyAttribute.Host = null;
                copyAttribute.Port = 0;
                clients[index] = new Server.TcpInternalClient(copyAttribute);
            }
        }
    }
}
