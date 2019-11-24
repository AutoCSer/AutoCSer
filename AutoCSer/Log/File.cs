using System;
using AutoCSer.IO;
using System.IO;
using System.Text;
using System.Diagnostics;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Log
{
    /// <summary>
    /// 文件日志处理(默认日志)
    /// </summary>
    public sealed partial class File : AutoCSer.Threading.DoubleLink<File>, ILog, IDisposable
    {
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public const string DefaultFilePrefix = "log_";
        /// <summary>
        /// 文件日志链表
        /// </summary>
        internal static YieldLink Files;

        /// <summary>
        /// 日志同步访问锁
        /// </summary>
        private readonly object waitLock = new object();
        /// <summary>
        /// 日志缓存队列
        /// </summary>
        private readonly FifoPriorityQueue<HashString, bool> cache = new FifoPriorityQueue<HashString, bool>();
        /// <summary>
        /// 最大缓存数量
        /// </summary>
        private readonly int maxCacheCount;
        /// <summary>
        /// 日志文件名
        /// </summary>
        private string fileName;
        /// <summary>
        /// 日志文件流
        /// </summary>
        private FileStreamWriter fileStream;
        /// <summary>
        /// 新的日志信息队列
        /// </summary>
        private DebugInfo.Queue newDebugs = new DebugInfo.Queue(new DebugInfo());
        /// <summary>
        /// 当前正在处理的日志信息队列
        /// </summary>
        private DebugInfo.Queue debugs = new DebugInfo.Queue(new DebugInfo());
        /// <summary>
        /// 日志信息访问锁
        /// </summary>
        private int debugLock;
        /// <summary>
        /// 日志同步标志
        /// </summary>
        private int waitId;
        /// <summary>
        /// 已释放日志同步标志
        /// </summary>
        private int freeWaitId;
        /// <summary>
        /// 最后一次输出缓存
        /// </summary>
        private HashString lastCache;
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary>
        private int isTimer;
        /// <summary>
        /// 是否添加到文件日志链表
        /// </summary>
        private int isPushFiles;
        /// <summary>
        /// 最大字节长度(小于等于0表示不限)
        /// </summary>
        public int MaxSize = Pub.Config.MaxFileSize;
        /// <summary>
        /// 日志处理类型
        /// </summary>
        public LogType Type { get; set; }
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 是否文件流模式
        /// </summary>
        private bool isFieStream;
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <param name="fileName">日志文件</param>
        /// <param name="maxCacheCount">最大缓存数量</param>
        /// <param name="type">最大缓存数量</param>
        public File(string fileName, int maxCacheCount, LogType type = LogType.All ^ LogType.AutoCSer ^ LogType.Debug ^ LogType.Info ^ LogType.Warn) 
        {
            Type = type;
            this.maxCacheCount = maxCacheCount <= 0 ? 1 : maxCacheCount;
            if ((this.fileName = fileName) != null)
            {
                tryOpen();
                if (isFieStream)
                {
                    Files.PushNotNull(this);
                    isPushFiles = 1;
                    AutoCSer.DomainUnload.Unloader.AddLast(this, DomainUnload.Type.LogFileDispose);
                }
            }
        }
        /// <summary>
        /// 打开日志文件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void tryOpen()
        {
            string fileName = this.fileName;
            if (!open(false))
            {
                do
                {
                    this.fileName = fileName + "." + Date.NowTime.Now.ToString("yyyyMMdd-HHmmss") + "_" + ((uint)Random.Default.Next()).toHex() + ".txt";
                }
                while (!open(true) && System.IO.File.Exists(this.fileName));
            }
        }
        /// <summary>
        /// 打开日志文件
        /// </summary>
        /// <param name="isTrace">是否输出调试信息</param>
        /// <returns>日志文件是否打开成功</returns>
        private bool open(bool isTrace)
        {
            try
            {
                fileStream = new FileStreamWriter(fileName, FileMode.OpenOrCreate, FileShare.Read, FileOptions.None, SubBuffer.Size.Kilobyte4, null, AutoCSer.Config.Pub.Default.EncodingCache);
                return isFieStream = true;
            }
            catch (Exception error)
            {
                if (isTrace) AutoCSer.Log.Trace.Console(error.ToString());
            }
            return isFieStream = false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            if (Interlocked.CompareExchange(ref isPushFiles, 0, 1) == 1)
            {
                Files.PopNotNull(this);
                Flush();
                AutoCSer.DomainUnload.Unloader.RemoveLast(this, DomainUnload.Type.LogFileDispose, false);
                if (Pub.Log != this || AutoCSer.DomainUnload.Unloader.State != DomainUnload.State.Run)
                {
#if XAMARIN
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
#else
                    if (fileStream != null)
                    {
                        if (fileStream.NewLength == 0)
                        {
                            fileStream.Dispose();
                            fileStream = null;
                        }
                        else
                        {
                            string fileName = fileStream.FileName;
                            fileStream.Dispose();
                            fileStream = null;
                            AutoCSer.IO.File.MoveBak(fileName);
                        }
                    }
#endif
                }
            }
        }
        /// <summary>
        /// 等待写入文件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Flush()
        {
            if (!isDisposed && isFieStream)
            {
                DebugInfo value = new DebugInfo { Type = LogType.Flush };
                add(value, true);
                wait(value);
            }
        }
#if !XAMARIN
        /// <summary>
        /// 移动日志文件
        /// </summary>
        /// <returns>新的日志文件名称</returns>
        internal string UnsafeMoveBak()
        {
            return isFieStream && fileStream != null ? moveBak() : null;
        }
        /// <summary>
        /// 移动日志文件
        /// </summary>
        /// <returns>新的日志文件名称</returns>
        private string moveBak()
        {
            string fileName = fileStream.FileName;
            fileStream.Dispose();
            fileStream = null;
            try
            {
                return AutoCSer.IO.File.MoveBak(fileName);
            }
            finally { tryOpen(); }
        }
#endif
        /// <summary>
        /// 日志输出同步等待
        /// </summary>
        /// <param name="value"></param>
        private void wait(DebugInfo value)
        {
            if (freeWaitId < value.WaitId)
            {
                do
                {
                    Monitor.Enter(waitLock);
                    if (freeWaitId >= value.WaitId)
                    {
                        Monitor.Exit(waitLock);
                        return;
                    }
                    Monitor.Wait(waitLock);
                    Monitor.Exit(waitLock);
                }
                while (true);
            }
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isWait"></param>
        private void add(DebugInfo value, bool isWait)
        {
            if (!isDisposed)
            {
                //bool isEmpty;
                if (isWait)
                {
                    value.Type |= LogType.Flush;
                    while (System.Threading.Interlocked.CompareExchange(ref debugLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.FileLogPushDebug);
                    //isEmpty = newDebugs.IsEmpty;
                    value.WaitId = ++waitId;
                    newDebugs.Push(value);
                    System.Threading.Interlocked.Exchange(ref debugLock, 0);
                }
                else
                {
                    while (System.Threading.Interlocked.CompareExchange(ref debugLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.FileLogPushDebug);
                    //isEmpty = newDebugs.IsEmpty;
                    newDebugs.Push(value);
                    System.Threading.Interlocked.Exchange(ref debugLock, 0);
                }
            }
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(LogType type, Exception error, string message, CacheType cache)
        {
            add(type, error, message, cache, false);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        /// <param name="isWait">是否同步</param>
        /// <returns>日志信息</returns>
        private DebugInfo add(LogType type, Exception error, string message, CacheType cache, bool isWait)
        {
            if ((Type & type) != 0)
            {
                if (error != null)
                {
                    //if (!error.Message.StartsWith(ExceptionPrefix, StringComparison.Ordinal))
                    {
                        DebugInfo value = new DebugInfo
                        {
                            Type = type,
                            Exception = error,
                            Message = message,
                            CacheType = cache
                        };
                        add(value, isWait);
                        return value;
                    }
                }
                if (message != null) return add(type, message, null, null, cache, isWait);
            }
            return null;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        /// <param name="isWait">是否同步</param>
        /// <returns>日志信息</returns>
        private DebugInfo add(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache, bool isWait)
        {
            if ((Type & type) != 0)
            {
                DebugInfo value = new DebugInfo
                {
                    Type = type,
                    Message = message,
                    StackFrame = stackFrame,
                    StackTrace = stackTrace,
                    CacheType = cache
                };
                add(value, isWait);
                return value;
            }
            return null;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache)
        {
            add(type, message, stackFrame, stackTrace, cache, false);
        }
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Wait(LogType type, Exception error, string message, CacheType cache)
        {
            DebugInfo value = add(type, error, message, cache, true);
            if (value != null) wait(value);
        }
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Wait(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache)
        {
            DebugInfo value = add(type, message, stackFrame, stackTrace, cache, true);
            if (value != null) wait(value);
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Throw(LogType type, Exception error, string message, CacheType cache)
        {
            DebugInfo value = add(type, error, message, cache, false);
            throw error ?? new Exception(value == null ? type.ToString() : value.ToString());
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Throw(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache)
        {
            DebugInfo value = add(type, message, stackFrame, stackTrace, cache, false);
            throw new Exception(value == null ? type.ToString() : value.ToString());
        }
        /// <summary>
        /// 同步添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WaitThrow(LogType type, Exception error, string message, CacheType cache)
        {
            DebugInfo value = add(type, error, message, cache, true);
            if (value != null)
            {
                wait(value);
                throw error ?? new Exception(value.toString);
            }
            throw new Exception(type.ToString());
        }
        /// <summary>
        /// 同步添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WaitThrow(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache)
        {
            DebugInfo value = add(type, message, stackFrame, stackTrace, cache, true);
            if (value != null)
            {
                wait(value);
                throw new Exception(value.toString);
            }
            throw new Exception(type.ToString());
        }

        /// <summary>
        /// 定时器触发日志写入
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimer()
        {
            if (!newDebugs.IsEmpty && System.Threading.Interlocked.CompareExchange(ref isTimer, 1, 0) == 0)
            {
                output();
                System.Threading.Interlocked.Exchange(ref isTimer, 0);
            }
        }
        /// <summary>
        /// 日志写入
        /// </summary>
        private void output()
        {
            DebugInfo value = null;
            FileStreamWriter fileStream;
            do
            {
                while (System.Threading.Interlocked.CompareExchange(ref debugLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.FileLogExchangeDebug);
                if (newDebugs.IsEmpty)
                {
                    System.Threading.Interlocked.Exchange(ref debugLock, 0);
                    return;
                }
                debugs.Exchange(ref newDebugs);
                System.Threading.Interlocked.Exchange(ref debugLock, 0);
                do
                {
                    try
                    {
                        while ((value = debugs.PopOnly()) != null)
                        {
                            switch (value.CacheType)
                            {
                                case CacheType.Queue:
                                    HashString cacheKey = value.ToString();
                                    if (cache.Get(ref cacheKey, false)) fileStream = null;
                                    else
                                    {
                                        cache.UnsafeAdd(ref cacheKey, true);
                                        if (cache.Count > maxCacheCount) cache.UnsafePopNode();
                                        fileStream = this.fileStream;
                                    }
                                    break;
                                case CacheType.Last:
                                    HashString key = value.ToString();
                                    if (key.Equals(ref lastCache)) fileStream = null;
                                    else
                                    {
                                        lastCache = key;
                                        fileStream = this.fileStream;
                                    }
                                    break;
                                default: fileStream = this.fileStream; break;
                            }
                            if (fileStream != null)
                            {
                                if (value.Type == LogType.Flush) fileStream.Flush();
                                else
                                {
                                    string log = @"
" + Date.NowTime.Now.toString() + " : " + value.ToString() + @"
";
#if XAMARIN
                                    Trace.Console(log);
                                    if (fileStream.WriteNotEmpty(log) != -1 && (value.Type & LogType.Flush) != 0) fileStream.Flush();
#else
                                    if (fileStream.WriteNotEmpty(log) >= MaxSize && MaxSize > 0) moveBak();
                                    else if ((value.Type & LogType.Flush) != 0) fileStream.Flush();
#endif
                                }
                            }
                            pulseWait(value);
                        }
                        goto END;
                    }
                    catch
                    {
                        pulseWait(value);
                        CatchCount.Add(CatchCount.Type.Log_Output);
                    }
                }
                while (true);
            END:
                debugs.SetHeaderNextNull();
            }
            while (true);
        }
        /// <summary>
        /// 唤醒等待
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void pulseWait(DebugInfo value)
        {
            if (value.WaitId != 0)
            {
                freeWaitId = value.WaitId;
                Monitor.Enter(waitLock);
                Monitor.PulseAll(waitLock);
                Monitor.Exit(waitLock);
            }
        }

        static File()
        {
            ++Date.NowTime.Count;
        }
    }
}
