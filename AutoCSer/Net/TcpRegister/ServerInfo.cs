using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 服务注册信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class ServerInfo
    {
        /// <summary>
        /// 注册时钟周期标识
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal long RegisterTick;
        /// <summary>
        /// 索引位置
        /// </summary>
        internal int ClientIndex;
        /// <summary>
        /// 索引编号
        /// </summary>
        internal int ClientIdentity;
        /// <summary>
        /// TCP服务名称标识
        /// </summary>
        internal string Name;
        /// <summary>
        /// 主机名称或者IP地址
        /// </summary>
        internal string Host;
        /// <summary>
        /// 端口号
        /// </summary>
        internal int Port;
        /// <summary>
        /// TCP 服务主机与端口信息
        /// </summary>
        internal HostPort HostPort
        {
            get { return new HostPort { Host = Host, Port = Port }; }
        }
        /// <summary>
        /// 是否只允许一个 TCP 服务实例
        /// </summary>
        internal bool IsSingle;
        /// <summary>
        /// 是否重连检测 TCP 服务注册信息
        /// </summary>
        internal bool IsCheckRegister;
        /// <summary>
        /// 主机名称转换成IP地址
        /// </summary>
        /// <returns>是否转换成功</returns>
        internal bool HostToIpAddress()
        {
            IPAddress ipAddress = HostPort.HostToIPAddress(Host);
            if (ipAddress == null) return false;
            Host = ipAddress.ToString();
            return true;
        }
        /// <summary>
        /// 判断是否匹配服务信息
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool ClientEquals(ServerInfo server)
        {
            return ((ClientIndex ^ server.ClientIndex) | (ClientIdentity ^ server.ClientIdentity) | (Port ^ server.Port)) == 0 && Host == server.Host;
        }
        /// <summary>
        /// 判断服务主机与端口信息是否匹配
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool HostPortEquals(ServerInfo server)
        {
            return Port == server.Port && Host == server.Host;
        }
    }
}
