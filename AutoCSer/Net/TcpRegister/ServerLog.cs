using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 服务注册信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, IsAnonymousFields = true)]
    public sealed class ServerLog
    {
        /// <summary>
        /// 随机标识
        /// </summary>
        public ulong Random { get; internal set; }
        /// <summary>
        /// TCP服务名称标识
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// 主机名称或者IP地址
        /// </summary>
        public string Host { get; internal set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; internal set; }
        /// <summary>
        /// TCP 服务主机与端口信息
        /// </summary>
        public HostPort HostPort
        {
            get { return new HostPort { Host = Host, Port = Port }; }
        }
        /// <summary>
        /// 是否只允许一个 TCP 服务实例
        /// </summary>
        public bool IsSingle { get; internal set; }
        /// <summary>
        /// 是否主服务
        /// </summary>
        public bool IsMain { get; internal set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; internal set; }
        /// <summary>
        /// 主机名称转换成IP地址
        /// </summary>
        /// <returns>是否转换成功</returns>
        public bool HostToIpAddress()
        {
            IPAddress ipAddress = HostPort.HostToIPAddress(Host);
            if (ipAddress == null) return false;
            Host = ipAddress.ToString();
            return true;
        }
        /// <summary>
        /// 判断服务主机与端口信息是否匹配
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool HostPortEquals(ServerLog server)
        {
            return Port == server.Port && Host == server.Host;
        }
    }
}
