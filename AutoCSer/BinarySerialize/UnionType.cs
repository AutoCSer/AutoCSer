using System;
using System.Runtime.InteropServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 二进制反序列化配置参数
        /// </summary>
        [FieldOffset(0)]
        public DeSerializeConfig DeSerializeConfig;
        /// <summary>
        /// 二进制序列化配置参数
        /// </summary>
        [FieldOffset(0)]
        public SerializeConfig SerializeConfig;
        /// <summary>
        /// 二进制数据序列化类型配置
        /// </summary>
        [FieldOffset(0)]
        public SerializeAttribute SerializeAttribute;
    }
}
