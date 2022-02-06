using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 公共日志配置
    /// </summary>
    public static partial class LogHelper
    {
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="message">警告日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Warn(string message, LogLevel level = LogLevel.Warn, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            Default.Warn(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Info(string message, LogLevel level = LogLevel.Info, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            Default.Info(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Debug(string message, LogLevel level = LogLevel.Debug, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            Default.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Error(string message, LogLevel level = LogLevel.Error, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            Default.Error(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Fatal(string message, LogLevel level = LogLevel.Fatal, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            Default.Fatal(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <param name="waitMilliseconds">轮询等待毫秒数</param>
        /// <returns>写盘是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static async Task FlushAsync(int waitMilliseconds = 1)
        {
            await Default.FlushAsync(waitMilliseconds);
        }
    }
}
