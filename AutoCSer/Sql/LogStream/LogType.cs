using System;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 日志类型
    /// </summary>
    internal enum LogType : byte
    {
        /// <summary>
        /// 客户端数量超限
        /// </summary>
        CountError,
        /// <summary>
        /// 数据加载完毕
        /// </summary>
        Loaded,
        /// <summary>
        /// 添加数据
        /// </summary>
        Insert,
        /// <summary>
        /// 更新数据
        /// </summary>
        Update,
        /// <summary>
        /// 删除数据
        /// </summary>
        Delete
    }
}
