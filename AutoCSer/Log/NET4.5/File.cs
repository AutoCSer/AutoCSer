using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Log
{
    /// <summary>
    /// 文件日志
    /// </summary>
    public partial class File
    {
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="message">警告日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public virtual void Warn(string message, LogLevel level = LogLevel.Warn, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if ((Level & level) != 0) append(new LogData(message, level, callerMemberName, callerFilePath, callerLineNumber));
        }
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public virtual void Info(string message, LogLevel level = LogLevel.Info, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if ((Level & level) != 0) append(new LogData(message, level, callerMemberName, callerFilePath, callerLineNumber));
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public virtual void Debug(string message, LogLevel level = LogLevel.Debug, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if ((Level & level) != 0) append(new LogData(message, level, callerMemberName, callerFilePath, callerLineNumber));
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public virtual void Error(string message, LogLevel level = LogLevel.Error, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if ((Level & level) != 0) append(new LogData(message, level, callerMemberName, callerFilePath, callerLineNumber));
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public virtual void Fatal(string message, LogLevel level = LogLevel.Fatal, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if ((Level & level) != 0) append(new LogData(message, level, callerMemberName, callerFilePath, callerLineNumber));
        }

        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <returns></returns>
        protected virtual async Task write()
        {
            do
            {
                if (logQueue.IsEmpty) await Task.Delay(writeTaskDelay);
                logLock.Enter();
                LogData log = logQueue.GetClear();
                logLock.Exit();
                if (log != null) await write(log);
                else
                {
                    bool isReturn = false;
                    try
                    {
                        if (isDisposed)
                        {
                            dispose();
                            isReturn = true;
                            return;
                        }
                        StreamWriter streamWriter = this.streamWriter;
                        if (streamWriter == null)
                        {
                            isReturn = true;
                            return;
                        }
                        await streamWriter.FlushAsync();
                    }
                    catch (Exception exception)
                    {
                        flushException = exception;
                    }
                    finally
                    {
                        if (isReturn)
                        {
                            logLock.Enter();
                            isLogTask = false;
                            logLock.Exit();
                        }
                    }
                    logLock.Enter();
                    log = logQueue.GetClear();
                    if (log != null)
                    {
                        logLock.Exit();
                        await write(log);
                    }
                    else
                    {
                        isLogTask = false;
                        logLock.Exit();
                        return;
                    }
                }
            }
            while (true);
        }
        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private async Task write(LogData log)
        {
            StreamWriter streamWriter = this.streamWriter;
            if (streamWriter != null && log != null)
            {
                do
                {
                    try
                    {
                        do
                        {
                            if (log.Level == LogData.FlushLevel)
                            {
                                await streamWriter.FlushAsync();
                                flushWaitId = log.WaitId;
                            }
                            else log.write(streamWriter);
                            log = log.LinkNext;
                        }
                        while (log != null);
                    }
                    catch (Exception exception)
                    {
                        if (log?.Level == LogData.FlushLevel)
                        {
                            flushException = exception;
                            flushWaitId = log.WaitId;
                        }
                    }
                    if (log != null) log = log.LinkNext;
                }
                while (log != null);
            }
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <returns>写盘是否成功</returns>
        private async Task<bool> flush()
        {
            logLock.SleepFlag = 0;
            LogData log = logQueue.GetClear();
            isLogTask = true;
            logLock.Exit();
            try
            {
                await write(log);
                StreamWriter streamWriter = this.streamWriter;
                if (streamWriter != null) await streamWriter.FlushAsync();
                if (object.ReferenceEquals(flushException, LogData.EmptyException)) return true;
                throw flushException;
            }
            finally
            {
                if (logQueue.IsEmpty)
                {
                    logLock.Enter();
                    if (logQueue.IsEmpty)
                    {
                        isLogTask = false;
                        logLock.Exit();
                    }
                    else
                    {
                        logLock.Exit();
                        AutoCSer.Threading.CatchTask.Add(write());
                    }
                }
                else AutoCSer.Threading.CatchTask.Add(write());
            }
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <param name="waitMilliseconds">轮询等待毫秒数</param>
        /// <returns>写盘是否成功</returns>
        public virtual async Task<bool> FlushAsync(int waitMilliseconds)
        {
            if (!isDisposed)
            {
                LogData flushLog = new LogData();
                logLock.Enter();
                if (!isLogTask)
                {
                    logLock.SleepFlag = 1;
                    return await flush();
                }
                flushLog.WaitId = ++waitId;
                logQueue.Push(flushLog);
                logLock.Exit();
                do
                {
                    await Task.Delay(waitMilliseconds);
                    if (flushWaitId >= flushLog.WaitId)
                    {
                        if (object.ReferenceEquals(flushException, LogData.EmptyException)) return true;
                        throw flushException;
                    }
                    logLock.Enter();
                    if (!isLogTask)
                    {
                        logLock.SleepFlag = 1;
                        return await flush();
                    }
                    logLock.Exit();
                }
                while (true);
            }
            return false;
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <param name="waitMilliseconds">轮询等待毫秒数</param>
        /// <returns>写盘是否成功</returns>
        public virtual bool Flush(int waitMilliseconds)
        {
            return FlushAsync(waitMilliseconds).Result;
        }
    }
}
