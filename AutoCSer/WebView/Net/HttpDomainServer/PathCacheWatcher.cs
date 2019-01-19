using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 指定路径下的文件缓存监视
    /// </summary>
    internal sealed class PathCacheWatcher
    {
        /// <summary>
        /// 文件监视器缓冲区
        /// </summary>
        private byte[] fileWatcherBuffer;
        /// <summary>
        /// 文件监视访问锁
        /// </summary>
        private readonly object fileWatcherLock = new object();
        /// <summary>
        /// 文件监视器
        /// </summary>
        private FileSystemWatcher fileWatcher;
        /// <summary>
        /// 监视路径
        /// </summary>
        private string path;
        /// <summary>
        /// 路径标识
        /// </summary>
        private int identity;
        /// <summary>
        /// 引用次数
        /// </summary>
        private int count = 1;
        /// <summary>
        /// 指定路径下的文件缓存监视
        /// </summary>
        /// <param name="path"></param>
        private PathCacheWatcher(string path)
        {
            identity = ++pathIdentity;
            fileWatcherBuffer = new byte[AutoCSer.IO.File.MaxFullNameLength];
            fileWatcher = new FileSystemWatcher(this.path = path);
            fileWatcher.IncludeSubdirectories = true;
            fileWatcher.EnableRaisingEvents = true;
            fileWatcher.Changed += fileChanged;
            fileWatcher.Deleted += fileChanged;
        }
        /// <summary>
        /// 文件更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private unsafe void fileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                string fullPath = e.FullPath;
                if (fullPath.Length - path.Length <= fileWatcherBuffer.Length)
                {
                    char directorySeparatorChar = Path.DirectorySeparatorChar;
                    Monitor.Enter(fileWatcherLock);
                    try
                    {
                        fixed (byte* bufferFixed = fileWatcherBuffer)
                        fixed (char* pathFixed = fullPath)
                        {
                            byte* write = bufferFixed;
                            char* start = pathFixed + path.Length, end = pathFixed + fullPath.Length;
                            while (start != end)
                            {
                                char value = *start++;
                                if ((uint)(value - 'A') < 26) *write++ = (byte)(value | 0x20);
                                else *write++ = value == directorySeparatorChar ? (byte)'/' : (byte)value;
                            }
                            FileCacheKey key = new FileCacheKey(identity, fileWatcherBuffer, 0, (int)(write - bufferFixed));
                            FileCacheQueue.Remove(ref key);
                        }
                    }
                    finally { Monitor.Exit(fileWatcherLock); }
                }
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 释放文件监视器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void free()
        {
#if !XAMARIN
            using (fileWatcher)
#endif
            {
                fileWatcher.EnableRaisingEvents = false;
                fileWatcher.Changed -= fileChanged;
                fileWatcher.Deleted -= fileChanged;
            }
        }

        /// <summary>
        /// 路径标识
        /// </summary>
        private static int pathIdentity;
        /// <summary>
        /// 缓存集合
        /// </summary>
        private static readonly Dictionary<string, PathCacheWatcher> caches = DictionaryCreator.CreateOnly<string, PathCacheWatcher>();
        /// <summary>
        /// 缓存集合访问锁
        /// </summary>
        private static readonly object cacheLock = new object();
        /// <summary>
        /// 添加监视路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns>路径标识</returns>
        internal static int Add(string path)
        {
            PathCacheWatcher cache;
            Monitor.Enter(cacheLock);
            if (caches.TryGetValue(path, out cache))
            {
                ++cache.count;
                Monitor.Exit(cacheLock);
            }
            else
            {
                try
                {
                    caches.Add(path, cache = new PathCacheWatcher(path));
                }
                finally { Monitor.Exit(cacheLock); }
            }
            return cache.identity;
        }
        /// <summary>
        /// 释放监视路径
        /// </summary>
        /// <param name="path"></param>
        internal static void Free(string path)
        {
            PathCacheWatcher cache;
            Monitor.Enter(cacheLock);
            if (caches.TryGetValue(path, out cache) && --cache.count == 0)
            {
                caches.Remove(path);
                Monitor.Exit(cacheLock);
                cache.free();
                return;
            }
            Monitor.Exit(cacheLock);
        }
    }
}
