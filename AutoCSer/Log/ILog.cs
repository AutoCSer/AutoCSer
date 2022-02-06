using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public partial interface ILog
    {
        /// <summary>
        /// 允许日志级别
        /// </summary>
        LogLevel LogLevel { get; }
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        void Exception(Exception exception, string message = null, LogLevel level = LogLevel.Exception);
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <param name="waitMilliseconds">轮询等待毫秒数</param>
        /// <returns>写盘是否成功</returns>
        bool Flush(int waitMilliseconds);
    }
}
