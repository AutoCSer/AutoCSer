using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署客户端任务扩展信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ClientTaskInfo
    {
        /// <summary>
        /// 文件数据源索引集合
        /// </summary>
        internal LeftArray<KeyValue<string, int>> FileIndexs;
        /// <summary>
        /// Web部署目录信息
        /// </summary>
        internal Directory Directory;
        /// <summary>
        /// 服务器端任务索引
        /// </summary>
        internal int TaskIndex;
    }
}
