using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 主机端口信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PortCache
    {
        /// <summary>
        /// IP 地址
        /// </summary>
        public string Host;
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port;
        /// <summary>
        /// 设置 主机端口信息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}
