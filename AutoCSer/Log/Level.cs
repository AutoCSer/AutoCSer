using System;

namespace AutoCSer
{
    /// <summary>
    /// 日志级别
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// AutoCSer 框架底层调试信息
        /// </summary>
        AutoCSer = 1,
        /// <summary>
        /// 一般信息
        /// </summary>
        Info = 2,
        /// <summary>
        /// 调试信息
        /// </summary>
        Debug = 4,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 8,
        /// <summary>
        /// 异常
        /// </summary>
        Exception = 0x10,
        /// <summary>
        /// 一般错误
        /// </summary>
        Error = 0x20,
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal = 0x40,
        ///// <summary>
        ///// 等待缓存写入文件
        ///// </summary>
        //Flush = 0x80,
        /// <summary>
        /// 所有日志
        /// </summary>
        All = 0xff
    }
}
