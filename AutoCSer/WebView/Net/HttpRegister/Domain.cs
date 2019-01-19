using System;
using AutoCSer.Extension;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// 域名信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class Domain
    {
        /// <summary>
        /// 域名
        /// </summary>
        internal byte[] DomainData;
        /// <summary>
        /// 域名
        /// </summary>
        public string DomainName
        {
            set { DomainData = value.getBytes(); }
        }
        /// <summary>
        /// TCP服务端口信息
        /// </summary>
        public HostPort Host;
        /// <summary>
        /// 安全连接服务端口信息
        /// </summary>
        public HostPort SslHost;
        /// <summary>
        /// 域名是否全名,否则表示泛域名后缀
        /// </summary>
        public bool IsFullName = true;
        /// <summary>
        /// 是否仅用于内网 IP 映射
        /// </summary>
        public bool IsOnlyHost;
    }
}
