using System;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 反序列化结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeSerializeResult
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        public MemberMap MemberMap;
        /// <summary>
        /// 数据字节长度
        /// </summary>
        public int DataLength;
        /// <summary>
        /// 反序列化状态
        /// </summary>
        public DeSerializeState State;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(DeSerializeResult value) { return value.State == DeSerializeState.Success; }
    }
}
