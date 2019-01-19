using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 接受数据处理类型
    /// </summary>
    internal enum BoundaryReceiveType : byte
    {
        /// <summary>
        /// 接收第一个分割符
        /// </summary>
        OnFirstBoundary,
        /// <summary>
        /// 接收换行数据
        /// </summary>
        OnEnter,
        /// <summary>
        /// 接收表单值
        /// </summary>
        OnValue
    }
}
