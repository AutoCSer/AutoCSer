using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ShortPath
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 短路径节点
        /// </summary>
        [FieldOffset(0)]
        public CacheServer.ShortPath.Node Value;
    }
}
