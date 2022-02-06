using System;
using System.IO;
using System.Runtime.CompilerServices;

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
        public virtual void Warn(string message, LogLevel level = LogLevel.Warn)
        {
            if ((Level & level) != 0) append(new LogData(message, level, null, null, 0));
        }
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        public virtual void Info(string message, LogLevel level = LogLevel.Info)
        {
            if ((Level & level) != 0) append(new LogData(message, level, null, null, 0));
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        public virtual void Debug(string message, LogLevel level = LogLevel.Debug)
        {
            if ((Level & level) != 0) append(new LogData(message, level, null, null, 0));
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        public virtual void Error(string message, LogLevel level = LogLevel.Error)
        {
            if ((Level & level) != 0) append(new LogData(message, level, null, null, 0));
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        public virtual void Fatal(string message, LogLevel level = LogLevel.Fatal)
        {
            if ((Level & level) != 0) append(new LogData(message, level, null, null, 0));
        }
        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <returns></returns>
        protected virtual void write()
        {
            do
            {
                if (logQueue.IsEmpty) System.Threading.Thread.Sleep(writeTaskDelay);
                logLock.Enter();
                LogData log = logQueue.GetClear();
                logLock.Exit();
                if (log != null) write(log);
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
                        streamWriter.Flush();
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
                        write(log);
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
        private void write(LogData log)
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
                                streamWriter.Flush();
                                flushWaitId = log.WaitId;
                            }
                            else log.write(streamWriter);
                            log = log.LinkNext;
                        }
                        while (log != null);
                    }
                    catch (Exception exception)
                    {
                        if (log != null && log.Level == LogData.FlushLevel)
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
        private bool flush()
        {
            logLock.SleepFlag = 0;
            LogData log = logQueue.GetClear();
            isLogTask = true;
            logLock.Exit();
            try
            {
                write(log);
                StreamWriter streamWriter = this.streamWriter;
                if (streamWriter != null) streamWriter.Flush();
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
                        AutoCSer.Threading.ThreadPool.TinyBackground.Start(write);
                    }
                }
                else AutoCSer.Threading.ThreadPool.TinyBackground.Start(write);
            }
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <param name="waitMilliseconds">轮询等待毫秒数</param>
        /// <returns>写盘是否成功</returns>
        public virtual bool Flush(int waitMilliseconds)
        {
            if (!isDisposed)
            {
                LogData flushLog = new LogData();
                logLock.Enter();
                if (!isLogTask)
                {
                    logLock.SleepFlag = 1;
                    return flush();
                }
                flushLog.WaitId = ++waitId;
                logQueue.Push(flushLog);
                logLock.Exit();
                do
                {
                    System.Threading.Thread.Sleep(waitMilliseconds);
                    if (flushWaitId >= flushLog.WaitId)
                    {
                        if (object.ReferenceEquals(flushException, LogData.EmptyException)) return true;
                        throw flushException;
                    }
                    logLock.Enter();
                    if (!isLogTask)
                    {
                        logLock.SleepFlag = 1;
                        return flush();
                    }
                    logLock.Exit();
                }
                while (true);
            }
            return false;
        }
    }
}
