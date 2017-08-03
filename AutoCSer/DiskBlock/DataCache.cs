using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块缓存
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DataCache
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        internal HashBytes Data;
        /// <summary>
        /// 磁盘块索引位置
        /// </summary>
        internal long Index;
        /// <summary>
        /// 数据缓存次数
        /// </summary>
        internal int Count;
    }
}
