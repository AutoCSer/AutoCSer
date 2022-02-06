using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace AutoCSer.Log
{
    /// <summary>
    /// 文件日志
    /// </summary>
    public partial class File : ILog, IDisposable
    {
        /// <summary>
        /// 日志文件名
        /// </summary>
        protected readonly string fileName;
        /// <summary>
        /// 文件字符编码
        /// </summary>
        protected readonly Encoding encoding;
        /// <summary>
        /// 写日志文件等待
        /// </summary>
        private readonly TimeSpan writeTaskDelay;
        /// <summary>
        /// 日志文件流
        /// </summary>
        private FileStream fileStream;
        /// <summary>
        /// 日志文件流文件名称
        /// </summary>
        private string fileStreamName;
        /// <summary>
        /// 日志文件流
        /// </summary>
        private StreamWriter streamWriter;
        /// <summary>
        /// 写盘异常
        /// </summary>
        private Exception flushException = LogData.EmptyException;
        /// <summary>
        /// 已经写盘的日志同步标志
        /// </summary>
        private long flushWaitId;
        /// <summary>
        /// 日志同步标志
        /// </summary>
        private long waitId;
        /// <summary>
        /// 日志队列访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock logLock;
        /// <summary>
        /// 允许日志级别
        /// </summary>
        public readonly LogLevel Level;
        /// <summary>
        /// 允许日志级别
        /// </summary>
        public LogLevel LogLevel { get { return Level; } }
        /// <summary>
        /// 是否已经启动写日志任务
        /// </summary>
        private bool isLogTask;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 新的日志信息队列
        /// </summary>
        private LogData.Queue logQueue = new LogData.Queue(new LogData());
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <param name="level">允许日志级别</param>
        /// <param name="fileName">日志文件</param>
        /// <param name="writeMilliseconds">写日志等待毫秒数</param>
        /// <param name="encoding">默认为 UTF-8</param>
        public File(LogLevel level = LogLevel.All, string fileName = "AutoCSer.log", int writeMilliseconds = 1000, Encoding encoding = null)
        {
            Level = level;
            this.fileName = fileName;
            writeTaskDelay = new TimeSpan(0, 0, 0, 0, writeMilliseconds);
            this.encoding = encoding ?? Encoding.UTF8;
            fileStreamName = string.Empty;
            if (!open())
            {
                isDisposed = true;
                dispose();
            }
        }
        /// <summary>
        /// 打开日志文件
        /// </summary>
        /// <returns></returns>
        private bool open()
        {
            string fileName = this.fileName;
            do
            {
                try
                {
                    fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4 << 10, FileOptions.None);
                    fileStreamName = fileName;
                    fileStream.Seek(0, SeekOrigin.End);
                    streamWriter = new StreamWriter(fileStream, encoding, 4 << 10);
                    return true;
                }
                catch (Exception exception)
                {
                    AutoCSer.Common.Config.OnLogFileException(this, exception);
                }
                if (!System.IO.File.Exists(fileName)) return false;
                fileName = this.fileName + "." + AutoCSer.Threading.SecondTimer.Now.ToString("yyyyMMdd-HHmmss") + "_" + ((uint)Random.Default.Next()).toHex() + ".log";
            }
            while (true);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            logLock.Enter();
            if (isLogTask) logLock.Exit();
            else
            {
                isLogTask = true;
                logLock.Exit();
                try
                {
                    dispose();
                }
                catch (Exception exception)
                {
                    flushException = exception;
                }
                finally
                {
                    logLock.Enter();
                    LogData log = logQueue.GetClear();
                    isLogTask = false;
                    if (log == null) logLock.Exit();
                    else
                    {
                        logLock.Exit();
                        do
                        {
                            if (log.Level == LogData.FlushLevel)
                            {
                                if (object.ReferenceEquals(flushException, LogData.EmptyException)) flushException = new ObjectDisposedException(fileName);
                                flushWaitId = long.MaxValue;
                                break;
                            }
                            log = log.LinkNext;
                        }
                        while (log != null);
                    }
                }
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void dispose()
        {
            if (streamWriter != null)
            {
                streamWriter.Dispose();
                streamWriter = null;
            }
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        protected virtual void append(LogData log)
        {
            if (!isDisposed)
            {
                logLock.Enter();
                bool isLogTask = this.isLogTask;
                logQueue.Push(log);
                this.isLogTask = true;
                logLock.Exit();
#if DOTNET2
                if (!isLogTask) AutoCSer.Threading.ThreadPool.TinyBackground.Start(write);
#else
                if (!isLogTask) AutoCSer.Threading.CatchTask.Add(write());
#endif
            }
        }
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        public virtual void Exception(Exception exception, string message = null, LogLevel level = LogLevel.Exception)
        {
            if ((Level & level) != 0) append(new LogData(exception, message, level));
        }
        /// <summary>
        /// 移动日志文件
        /// </summary>
        /// <returns>新的日志文件名称</returns>
        internal string UnsafeMoveBak()
        {
            return fileStream != null ? moveBak() : null;
        }
        /// <summary>
        /// 移动日志文件
        /// </summary>
        /// <returns>新的日志文件名称</returns>
        private string moveBak()
        {
            if (fileStream != null) fileStream.Dispose();
            try
            {
                return AutoCSer.IO.File.MoveBak(fileStreamName);
            }
            finally { open(); }
        }
    }
}
