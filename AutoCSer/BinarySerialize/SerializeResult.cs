using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 序列化结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SerializeResult
    {
        /// <summary>
        /// 序列化数据
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// 警告提示状态
        /// </summary>
        public SerializeWarning Warning;
        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator byte[](SerializeResult value) { return value.Data; }
    }
}
