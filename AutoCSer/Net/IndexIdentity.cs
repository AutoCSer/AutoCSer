using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 索引标识
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct IndexIdentity
    {
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
