using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache.Value
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
        /// 数据节点
        /// </summary>
        [FieldOffset(0)]
        public Node Node;
    }
}
