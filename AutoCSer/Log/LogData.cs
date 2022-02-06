using System;
using System.IO;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志数据
    /// </summary>
    public sealed class LogData : AutoCSer.Threading.Link<LogData>
    {
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message;
        /// <summary>
        /// 错误异常
        /// </summary>
        public Exception Exception;
        /// <summary>
        /// 日志同步标志
        /// </summary>
        public long WaitId;
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level;
        /// <summary>
        /// 调用源代码行号
        /// </summary>
        public int CallerLineNumber;
        /// <summary>
        /// 调用成员名称
        /// </summary>
        public string CallerMemberName;
        /// <summary>
        /// 调用源代码文件路径
        /// </summary>
        public string CallerFilePath;
        /// <summary>
        /// 等待日志数据
        /// </summary>
        /// <param name="level">日志级别</param>
        internal LogData(LogLevel level = FlushLevel)
        {
            Level = level;
            Exception = EmptyException;
            Message = CallerMemberName = CallerFilePath = string.Empty;
        }
        /// <summary>
        /// 文件日志数据
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public LogData(string message, LogLevel level, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            Message = message;
            Level = level;
            Exception = EmptyException;
            CallerMemberName = callerMemberName ?? string.Empty;
            CallerFilePath = callerFilePath ?? string.Empty;
            CallerLineNumber = callerLineNumber;
        }
        /// <summary>
        /// 文件日志数据
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        public LogData(Exception exception, string message = null, LogLevel level = LogLevel.Exception)
        {
            Exception = exception;
            Message = message ?? string.Empty;
            Level = level;
            CallerMemberName = CallerFilePath = string.Empty;
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="streamWriter"></param>
        internal void write(StreamWriter streamWriter)
        {
            if (streamWriter != null)
            {
                if (CallerMemberName.Length != 0)
                {
                    streamWriter.Write(@"
调用成员信息 : ");
                    streamWriter.Write(CallerMemberName);
                    if (CallerFilePath.Length != 0)
                    {
                        streamWriter.Write(" in ");
                        streamWriter.Write(CallerFilePath);
                        if (CallerLineNumber != 0)
                        {
                            streamWriter.Write(" line ");
                            streamWriter.Write(CallerLineNumber);
                        }
                    }

                }
                if (Message.Length != 0)
                {
                    streamWriter.Write(@"
");
                    streamWriter.Write(Message);
                }
                if (!object.ReferenceEquals(Exception, EmptyException))
                {
                    streamWriter.Write(@"
");
                    streamWriter.Write(Exception.ToString());
                }
            }
        }

        /// <summary>
        /// 等待写入完成日志级别
        /// </summary>
        internal const LogLevel FlushLevel = (LogLevel)0x80;
        /// <summary>
        /// 默认空异常
        /// </summary>
        internal static readonly Exception EmptyException = new Exception();
    }
}
