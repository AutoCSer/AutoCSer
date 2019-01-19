using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 服务端标识
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ClientId
    {
        /// <summary>
        /// 时钟周期标识
        /// </summary>
        internal long Tick;
        /// <summary>
        /// 索引位置
        /// </summary>
        internal int Index;
        /// <summary>
        /// 索引编号
        /// </summary>
        internal int Identity;
    }
}
