using System;
using System.Net;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP 服务主机与端口信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HostPort : IEquatable<HostPort>
    {
        /// <summary>
        /// 主机名称或者 IP 地址
        /// </summary>
        public string Host;
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port;
        /// <summary>
        /// 设置 TCP 服务主机与端口信息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(string host, int port)
        {
            Host = host;
            Port = port;
        }
        /// <summary>
        /// 主机名称转换为 IP 地址
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool HostToIPAddress()
        {
            IPAddress ipAddress = HostToIPAddress(Host);
            if (ipAddress == null) return false;
            Host = ipAddress.ToString();
            return true;
        }
        /// <summary>
        /// 判断是否TCP服务端口信息
        /// </summary>
        /// <param name="other">TCP服务端口信息</param>
        /// <returns>是否同一TCP服务端口信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(ref HostPort other)
        {
            return Host == other.Host && Port == other.Port;
        }

        /// <summary>
        /// 判断是否TCP服务端口信息
        /// </summary>
        /// <param name="other">TCP服务端口信息</param>
        /// <returns>是否同一TCP服务端口信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(HostPort other)
        {
            return Host == other.Host && Port == other.Port;
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return Host == null ? Port : (Host.GetHashCode() ^ Port);
        }
        /// <summary>
        /// 判断是否TCP服务端口信息
        /// </summary>
        /// <param name="other">TCP服务端口信息</param>
        /// <returns>是否同一TCP服务端口信息</returns>
        public override bool Equals(object other)
        {
            return Equals((HostPort)other);
        }

        /// <summary>
        /// 主机名称转换为 IP 地址
        /// </summary>
        /// <param name="host"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        internal static IPAddress HostToIPAddress(string host, ILog log = null)
        {
            if (!string.IsNullOrEmpty(host))
            {
                IPAddress value;
                if (IPAddress.TryParse(host, out value)) return value;
                try
                {
                    return Dns.GetHostEntry(host).AddressList[0];
                }
                catch (Exception error)
                {
                    (log ?? AutoCSer.Log.Pub.Log).Add(AutoCSer.Log.LogType.Error, error, host);
                }
            }
            return IPAddress.Any;
        }
    }
}
