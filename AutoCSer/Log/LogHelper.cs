using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 公共日志配置
    /// </summary>
    public static partial class LogHelper
    {
        /// <summary>
        /// 公共配置日志，默认为文件日志
        /// </summary>
        public static readonly ILog Default = (ILog)Configuration.Cache.Get(typeof(ILog), string.Empty) ?? new AutoCSer.Log.File();
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Exception(Exception exception, string message = null, LogLevel level = LogLevel.Exception)
        {
            Default.Exception(exception, message, level);
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <param name="waitMilliseconds">轮询等待毫秒数</param>
        /// <returns>写盘是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Flush(int waitMilliseconds = 1)
        {
            Default.Flush(waitMilliseconds);
        }
    }
}
