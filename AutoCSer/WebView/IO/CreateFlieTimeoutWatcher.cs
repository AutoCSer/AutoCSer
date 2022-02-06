using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using AutoCSer.Extensions;
using AutoCSer.Log;
using System.Runtime.CompilerServices;

namespace AutoCSer.IO
{
    /// <summary>
    /// 新建文件监视
    /// </summary>
    internal sealed class CreateFlieTimeoutWatcher : AutoCSer.Threading.SecondTimerTaskNode, IDisposable
    {
        /// <summary>
        /// 超时检测计数
        /// </summary>
        private long onTimeSeconds = long.MaxValue;
        /// <summary>
        /// 超时检测秒数
        /// </summary>
        private readonly int timeoutSeconds;
        /// <summary>
        /// 超时处理
        /// </summary>
        private readonly object onTimeout;
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly ILog log;
        /// <summary>
        /// 新建文件处理
        /// </summary>
        private FileSystemEventHandler onCreatedHandle;
        /// <summary>
        /// 文件监视器集合
        /// </summary>
        private Dictionary<HashString, CreateFlieTimeoutCounter> watchers;
        /// <summary>
        /// 文件监视器集合访问锁
        /// </summary>
        private readonly object watcherLock = new object();
        /// <summary>
        /// 超时检测文件集合访问锁
        /// </summary>
        private readonly object fileLock = new object();
        /// <summary>
        /// 超时检测文件集合
        /// </summary>
        private LeftArray<KeyValue<FileInfo, long>> files = new LeftArray<KeyValue<FileInfo, long>>(0);
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary>
        private int isTimer;
        /// <summary>
        /// 是否释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 超时处理类型
        /// </summary>
        private CreateFlieTimeoutType onTimeoutType;
        /// <summary>
        /// 新建文件监视
        /// </summary>
        /// <param name="timeoutSeconds">超时检测秒数</param>
        private CreateFlieTimeoutWatcher(int timeoutSeconds) : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, timeoutSeconds, Threading.SecondTimerThreadMode.Synchronous, Threading.SecondTimerKeepMode.After, timeoutSeconds)
        {
            this.timeoutSeconds = timeoutSeconds;
        }
        /// <summary>
        /// 新建文件监视
        /// </summary>
        /// <param name="seconds">超时检测秒数</param>
        /// <param name="onTimeout">超时处理</param>
        /// <param name="onTimeoutType">超时处理类型</param>
        /// <param name="log">日志处理</param>
        internal CreateFlieTimeoutWatcher(int seconds, object onTimeout, CreateFlieTimeoutType onTimeoutType, ILog log = null) : this(Math.Max(seconds, 2))
        {
            this.onTimeout = onTimeout;
            this.log = log ?? AutoCSer.LogHelper.Default;
            switch (this.onTimeoutType = onTimeoutType)
            {
                case CreateFlieTimeoutType.HttpServerRegister: onCreatedHandle = onCreatedHttpServerRegister; break;
                default: onCreatedHandle = onCreated; break;
            }
            watchers = DictionaryCreator.CreateHashString<CreateFlieTimeoutCounter>();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                KeepMode = AutoCSer.Threading.SecondTimerKeepMode.Canceled;
                Monitor.Enter(watcherLock);
                try
                {
                    if (watchers.Count != 0)
                    {
                        foreach (CreateFlieTimeoutCounter counter in watchers.Values) dispose(counter.Watcher);
                        watchers.Clear();
                    }
                }
                finally { Monitor.Exit(watcherLock); }
            }
        }
        /// <summary>
        /// 关闭文件监视器
        /// </summary>
        /// <param name="watcher">文件监视器</param>
        private void dispose(FileSystemWatcher watcher)
        {
#if !XAMARIN
            using (watcher)
#endif
            {
                watcher.EnableRaisingEvents = false;
                watcher.Created -= onCreatedHandle;
            }
        }
        /// <summary>
        /// 添加监视路径
        /// </summary>
        /// <param name="path">监视路径</param>
        internal void Add(string path)
        {
            if (isDisposed == 0)
            {
                path = AutoCSer.IO.File.FileNameToLower(path);
                CreateFlieTimeoutCounter counter;
                HashString pathKey = path;
                Monitor.Enter(watcherLock);
                try
                {
                    if (watchers.TryGetValue(pathKey, out counter))
                    {
                        ++counter.Count;
                        watchers[pathKey] = counter;
                    }
                    else
                    {
                        counter.Create(path, onCreatedHandle);
                        watchers.Add(pathKey, counter);
                    }
                }
                finally { Monitor.Exit(watcherLock); }
            }
        }
        /// <summary>
        /// 删除监视路径
        /// </summary>
        /// <param name="path">监视路径</param>
        internal void Remove(string path)
        {
            path = AutoCSer.IO.File.FileNameToLower(path);
            CreateFlieTimeoutCounter counter;
            HashString pathKey = path;
            Monitor.Enter(watcherLock);
            try
            {
                if (watchers.TryGetValue(pathKey, out counter))
                {
                    if (--counter.Count == 0) watchers.Remove(pathKey);
                    else watchers[pathKey] = counter;
                }
            }
            finally { Monitor.Exit(watcherLock); }
            if (counter.Count == 0 && counter.Watcher != null) dispose(counter.Watcher);
        }
        /// <summary>
        /// 新建文件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onCreatedHttpServerRegister(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (AutoCSer.Diagnostics.ProcessCopyServer.FileWatcherFilter(e)) onCreated(e);
            }
            catch (Exception error)
            {
                log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
        }
        /// <summary>
        /// 新建文件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                onCreated(e);
            }
            catch (Exception error)
            {
                log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
        }
        /// <summary>
        /// 新建文件处理
        /// </summary>
        /// <param name="e"></param>
        private void onCreated(FileSystemEventArgs e)
        {
            if (isDisposed == 0)
            {
                FileInfo file = new FileInfo(e.FullPath);
                if (file.Exists)
                {
                    Monitor.Enter(fileLock);
                    long seconds = AutoCSer.Threading.SecondTimer.CurrentSeconds + timeoutSeconds;
                    try
                    {
                        files.PrepLength(1);
                        files.Array[files.Length++].Set(file, seconds);
                        if (onTimeSeconds != long.MaxValue)
                        {
                            onTimeSeconds = seconds;
                            AppendTaskArray();
                        }
                    }
                    finally { Monitor.Exit(fileLock); }
                }
            }
        }
        /// <summary>
        /// 定时器触发
        /// </summary>
        protected internal override void OnTimer()
        {
            if (AutoCSer.Threading.SecondTimer.CurrentSeconds >= onTimeSeconds && isTimer == 0)
            {
                isTimer = 1;
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(onTimer);
            }
        }
        /// <summary>
        /// 定时器触发
        /// </summary>
        private void onTimer()
        {
            try
            {
                if (isDisposed == 0)
                {
                    long maxSeconds = long.MinValue;
                    int index = 0;
                    Monitor.Enter(fileLock);
                    int count = files.Length;
                    KeyValue<FileInfo, long>[] fileArray = files.Array;
                    try
                    {
                        while (index != count)
                        {
                            KeyValue<FileInfo, long> fileTime = fileArray[index];
                            if (fileTime.Value <= AutoCSer.Threading.SecondTimer.CurrentSeconds)
                            {
                                FileInfo file = fileTime.Key;
                                long length = file.Length;
                                file.Refresh();
                                if (file.Exists)
                                {
                                    if (length == file.Length)
                                    {
                                        try
                                        {
                                            using (FileStream fileStream = file.Open(FileMode.Open, FileAccess.Write, FileShare.None)) fileArray[index] = fileArray[--count];
                                        }
                                        catch { ++index; }
                                    }
                                    else ++index;
                                }
                                else fileArray[index] = fileArray[--count];
                            }
                            else
                            {
                                ++index;
                                if (fileTime.Value > maxSeconds) maxSeconds = fileTime.Value;
                            }
                        }
                        files.Length = count;
                        if (maxSeconds == long.MinValue)
                        {
                            if (count == 0) onTimeSeconds = long.MaxValue;
                        }
                        else onTimeSeconds = maxSeconds;
                    }
                    catch (Exception error)
                    {
                        log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    finally { Monitor.Exit(fileLock); }
                    if ((count | isDisposed) == 0)
                    {
                        try
                        {
                            switch (onTimeoutType)
                            {
                                case CreateFlieTimeoutType.HttpServerRegister: ((AutoCSer.Net.HttpRegister.Server)onTimeout).OnFileWatcherTimeout(); break;
                            }
                        }
                        catch (Exception error)
                        {
                            log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                        }
                    }
                }
            }
            finally { isTimer = 0; }
        }
    }
}
