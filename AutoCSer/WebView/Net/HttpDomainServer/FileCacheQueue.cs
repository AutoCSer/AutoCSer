using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 文件缓存队列
    /// </summary>
    internal sealed class FileCacheQueue
    {
        /// <summary>
        /// 当前可缓存字节数
        /// </summary>
        private long freeCacheSize;
        /// <summary>
        /// 文件缓存队列
        /// </summary>
        private FifoPriorityQueue<FileCacheKey, FileCache> files = new FifoPriorityQueue<FileCacheKey, FileCache>();
        /// <summary>
        /// 文件缓存队列访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock fileLock;
        /// <summary>
        /// 删除文件缓存
        /// </summary>
        /// <param name="path"></param>
        private void remove(ref FileCacheKey path)
        {
            FileCache file;
            fileLock.Enter();
            if (files.Remove(ref path, out file)) freeCacheSize += file.Size;
            fileLock.Exit();
        }
        /// <summary>
        /// 获取文件缓存
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private FileCache get(ref FileCacheKey path)
        {
            fileLock.Enter();
            FileCache file = files.Get(ref path, null);
            fileLock.Exit();
            return file;
        }
        /// <summary>
        /// 获取文件缓存，失败时创建缓存对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileCache"></param>
        /// <param name="isCopyPath">是否复制请求路径</param>
        /// <returns>是否新的文件缓存数据</returns>
        private byte get(ref FileCacheKey path, out FileCache fileCache, bool isCopyPath)
        {
            byte isNewFileCache = 0;
            fileLock.Enter();
            if ((fileCache = files.Get(ref path, null)) == null)
            {
                fileLock.SleepFlag = 1;
                byte isLock = 0;
                try
                {
                    fileCache = new FileCache();
                    isLock = 1;
                    if (isCopyPath) path.CopyPath();
                    files.UnsafeAdd(ref path, fileCache);
                    return isNewFileCache = 1;
                }
                finally
                {
                    fileLock.ExitSleepFlag();
                    if (isNewFileCache == 0 && isLock != 0) fileCache.PulseAll();
                }
            }
            else fileLock.Exit();
            return 0;
        }
        /// <summary>
        /// 删除新建的缓存
        /// </summary>
        /// <param name="path"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void removeOnly(ref FileCacheKey path)
        {
            FileCache fileCache;
            fileLock.Enter();
            files.Remove(ref path, out fileCache);
            fileLock.Exit();
        }
        /// <summary>
        /// 设置文件缓存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileCache"></param>
        /// <param name="fileSize"></param>
        private void set(ref FileCacheKey path, FileCache fileCache, int fileSize)
        {
            fileLock.EnterSleepFlag();
            try
            {
                fileCache.Size = fileSize;
                FileCache oldFileCache = files.Set(ref path, fileCache);
                freeCacheSize -= fileSize;
                if (oldFileCache == null)
                {
                    while (freeCacheSize < 0 && files.Count > 1) freeCacheSize += files.UnsafePopValue().Size;
                }
                else if (oldFileCache != fileCache) freeCacheSize += oldFileCache.Size;
            }
            finally { fileLock.ExitSleepFlag(); }
        }
        /// <summary>
        /// 清除文件缓存
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            fileLock.Enter();
            try
            {
                if (files.Count != 0)
                {
                    fileLock.SleepFlag = 1;
                    files = new FifoPriorityQueue<FileCacheKey, FileCache>();
                }
                freeCacheSize = maxCacheSize;
            }
            finally { fileLock.ExitSleepFlag(); }
        }

        /// <summary>
        /// 文件缓存最大字节数
        /// </summary>
        private static readonly long maxCacheSize = Http.SocketBase.Config.FileCacheTotalSize >> 8;
        /// <summary>
        /// 最大缓存文件节数
        /// </summary>
        internal static readonly int MaxFileSize = Http.SocketBase.Config.FileCacheSize;
        /// <summary>
        /// 文件缓存是否预留 HTTP 头部空间
        /// </summary>
        internal static readonly bool IsFileCacheHeader = Http.SocketBase.Config.IsFileCacheHeader;
        /// <summary>
        /// 文件缓存队列
        /// </summary>
        private static readonly FileCacheQueue[] queues;
        /// <summary>
        /// 删除文件缓存
        /// </summary>
        /// <param name="path"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Remove(ref FileCacheKey path)
        {
            queues[path.HashCode & 0xff].remove(ref path);
        }
        /// <summary>
        /// 获取文件缓存
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static FileCache Get(ref FileCacheKey path)
        {
            return queues[path.HashCode & 0xff].get(ref path);
        }
        /// <summary>
        /// 获取文件缓存，失败时创建缓存对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileCache"></param>
        /// <param name="isCopyPath">是否复制请求路径</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte Get(ref FileCacheKey path, out FileCache fileCache, bool isCopyPath)
        {
            return queues[path.HashCode & 0xff].get(ref path, out fileCache, isCopyPath);
        }
        /// <summary>
        /// 删除新建的缓存
        /// </summary>
        /// <param name="path"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void RemoveOnly(ref FileCacheKey path)
        {
            queues[path.HashCode & 0xff].removeOnly(ref path);
        }
        /// <summary>
        /// 设置文件缓存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileCache"></param>
        /// <param name="fileSize"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Set(ref FileCacheKey path, FileCache fileCache, int fileSize)
        {
            queues[path.HashCode & 0xff].set(ref path, fileCache, fileSize);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache()
        {
            foreach (FileCacheQueue queue in queues) queue.Clear();
        }
        static FileCacheQueue()
        {
            queues = new FileCacheQueue[256];
            for (int index = 256; index != 0; queues[--index] = new FileCacheQueue()) ;
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(FileCacheQueue), 0);
        }
    }
}
