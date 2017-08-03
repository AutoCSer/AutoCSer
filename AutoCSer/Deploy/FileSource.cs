using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 文件数据源
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FileSource
    {
        /// <summary>
        /// 文件数据
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// 文件数据源索引
        /// </summary>
        public int Index;
    }
}
