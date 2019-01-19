using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 服务信息集合缓存信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ServerSetCache
    {
        /// <summary>
        /// TCP服务名称标识
        /// </summary>
        internal string Name;
        /// <summary>
        /// TCP 服务注册信息
        /// </summary>
        internal ServerInfo Server;
        /// <summary>
        /// TCP 服务注册信息集合
        /// </summary>
        internal ServerInfo[] Servers;
        /// <summary>
        /// 设置 TCP 服务信息集合缓存信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="serverSet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(string name, ServerSet serverSet)
        {
            Name = name;
            Server = serverSet.Server.Server;
            Servers = serverSet.Servers.Length == 0 ? null : serverSet.Servers.GetArray(value => value.Server);
        }
    }
}
