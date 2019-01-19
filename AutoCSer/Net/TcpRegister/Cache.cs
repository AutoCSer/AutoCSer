using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 缓存数据
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Cache
    {
        /// <summary>
        /// TCP 服务信息集合缓存信息集合
        /// </summary>
        public ServerSetCache[] ServerSets;
        /// <summary>
        /// 主机端口信息集合
        /// </summary>
        public PortCache[] IpPorts;
    }
}
