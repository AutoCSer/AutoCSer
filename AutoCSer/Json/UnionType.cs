using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Json
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
        /// JSON 解析配置参数
        /// </summary>
        [FieldOffset(0)]
        public ParseConfig ParseConfig;
        /// <summary>
        /// JSON 解析类型配置
        /// </summary>
        [FieldOffset(0)]
        public ParseAttribute ParseAttribute;
        /// <summary>
        /// JSON 序列化配置参数
        /// </summary>
        [FieldOffset(0)]
        public SerializeConfig SerializeConfig;
        /// <summary>
        /// JSON 序列化类型配置
        /// </summary>
        [FieldOffset(0)]
        public SerializeAttribute SerializeAttribute;
    }
}
