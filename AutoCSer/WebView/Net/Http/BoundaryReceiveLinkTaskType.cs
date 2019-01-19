using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 接受数据处理任务类型
    /// </summary>
    internal enum BoundaryReceiveLinkTaskType : byte
    {
        /// <summary>
        /// 写文件
        /// </summary>
        WriteFile,
        /// <summary>
        /// 获取文件表单值
        /// </summary>
        GetFile,
        /// <summary>
        /// 获取文件表单值并结束
        /// </summary>
        GetFileFinally,
    }
}
