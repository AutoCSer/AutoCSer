using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 文件同步服务端
    /// </summary>
    public sealed class Server : IDisposable
    {
        /// <summary>
        /// 文件同步根目录
        /// </summary>
        internal readonly string Path;
        /// <summary>
        /// 最大文件下载字节数
        /// </summary>
        internal readonly int DownloadBufferSize;
        /// <summary>
        /// 未完成同步文件集合
        /// </summary>
        private readonly Dictionary<long, ServerFile> files = DictionaryCreator.CreateLong<ServerFile>();
        /// <summary>
        /// 未完成同步文件集合 访问锁
        /// </summary>
        private readonly object fileLock = new object();
        /// <summary>
        /// 超时检测
        /// </summary>
        private readonly Action onTimerHandle;
        /// <summary>
        /// 超时秒数
        /// </summary>
        private readonly int timeoutSeconds;
        /// <summary>
        /// 超时检测间隔秒数
        /// </summary>
        private readonly int checkTimeoutSeconds;
        /// <summary>
        /// 超时检测文件编号集合
        /// </summary>
        private readonly List<long> timeoutIdentitys = new List<long>();
        /// <summary>
        /// 当前超时检测秒数
        /// </summary>
        private int timerSeconds;
        /// <summary>
        /// 删除同步文件
        /// </summary>
        /// <param name="identity"></param>
        internal void Remove(long identity)
        {
            Monitor.Enter(fileLock);
            files.Remove(identity);
            Monitor.Exit(fileLock);
        }
        /// <summary>
        /// 文件同步服务端
        /// </summary>
        /// <param name="path">文件同步根目录</param>
        /// <param name="downloadBufferSize">最大文件下载字节数，默认为 4KB</param>
        /// <param name="timeoutSeconds">超时秒数，默认为 60</param>
        /// <param name="checkTimeoutSeconds">超时检测间隔秒数，默认为 30</param>
        public Server(string path, int downloadBufferSize = 4 << 10, int timeoutSeconds = 60, int checkTimeoutSeconds = 30)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists) directory.Create();
            Path = directory.FullName;
            DownloadBufferSize = Math.Max(downloadBufferSize, 1 << 10);
            this.timeoutSeconds = Math.Max(timeoutSeconds, 1);
            timerSeconds = this.checkTimeoutSeconds = Math.Max(checkTimeoutSeconds, 1);
            AutoCSer.Date.OnTime += (onTimerHandle = onTimer);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            AutoCSer.Date.OnTime -= onTimerHandle;
        }
        /// <summary>
        /// 超时检测
        /// </summary>
        private void onTimer()
        {
            if (Interlocked.Decrement(ref timerSeconds) == 0)
            {
                try
                {
                    if (files.Count != 0)
                    {
                        timeoutIdentitys.Clear();
                        Monitor.Enter(fileLock);
                        try
                        {
                            foreach (ServerFile serverFile in files.Values)
                            {
                                if (serverFile.CheckTimeout(timeoutSeconds)) timeoutIdentitys.Add(serverFile.Identity);
                            }
                            foreach (long identity in timeoutIdentitys) files.Remove(identity);
                        }
                        finally { Monitor.Exit(fileLock); }
                    }
                }
                finally
                {
                    
                    Interlocked.Exchange(ref timerSeconds, checkTimeoutSeconds);
                }
            }
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件列表</returns>
        public ListFileItem[] GetList(string path)
        {
            DirectoryInfo Directory = new DirectoryInfo(System.IO.Path.Combine(Path, path));
            if (Directory.Exists && Directory.FullName.StartsWith(Path, StringComparison.OrdinalIgnoreCase)) return getList(Directory);
            return null;
        }
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static ListFileItem[] getList(DirectoryInfo directory)
        {
            DirectoryInfo[] DirectoryArray = directory.GetDirectories();
            FileInfo[] FileArray = directory.GetFiles();
            ListFileItem[] ListFileArray = new ListFileItem[DirectoryArray.Length + FileArray.Length];
            int Index = 0;
            foreach (DirectoryInfo NextDirectory in DirectoryArray) ListFileArray[Index++].Set(NextDirectory.Name, NextDirectory.LastWriteTimeUtc, long.MinValue);
            foreach (FileInfo NextFile in FileArray) ListFileArray[Index++].Set(NextFile.Name, NextFile.LastWriteTimeUtc, NextFile.Length);
            return ListFileArray;
        }
        /// <summary>
        /// 获取列表文件
        /// </summary>
        /// <param name="path">文件名称</param>
        /// <returns>列表文件</returns>
        public ListFileItem Get(string path)
        {
            FileInfo FileInfo = new FileInfo(System.IO.Path.Combine(Path, path));
            if (FileInfo.Exists && FileInfo.FullName.StartsWith(Path, StringComparison.OrdinalIgnoreCase)) return new ListFileItem(FileInfo);
            return default(ListFileItem);
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录名称</param>
        /// <returns>列表文件</returns>
        public ListFileItem Create(string path)
        {
            DirectoryInfo Directory = new DirectoryInfo(System.IO.Path.Combine(Path, path));
            if (Directory.FullName.StartsWith(Path, StringComparison.OrdinalIgnoreCase))
            {
                if (!Directory.Exists) Directory.Create();
                return new ListFileItem(Directory);
            }
            return default(ListFileItem);
        }
        /// <summary>
        /// 删除文件列表
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="listFileArray">文件列表</param>
        /// <returns>删除后的文件列表</returns>
        public ListFileItem[] Delete(string path, ListFileItem[] listFileArray)
        {
            DirectoryInfo Directory = new DirectoryInfo(System.IO.Path.Combine(Path, path));
            if (Directory.Exists && Directory.FullName.StartsWith(Path, StringComparison.OrdinalIgnoreCase))
            {
                string DirectoryFullName = Directory.FullName;
                foreach (ListFileItem ListFile in listFileArray)
                {
                    if (ListFile.IsFile)
                    {
                        FileInfo DeleteFile = new FileInfo(System.IO.Path.Combine(DirectoryFullName, ListFile.Name));
                        if (DeleteFile.Exists && DeleteFile.FullName.StartsWith(Path, StringComparison.OrdinalIgnoreCase)) DeleteFile.Delete();
                    }
                    else
                    {
                        DirectoryInfo DeleteDirectory = new DirectoryInfo(System.IO.Path.Combine(DirectoryFullName, ListFile.Name));
                        if (DeleteDirectory.Exists && DeleteDirectory.FullName.StartsWith(Path, StringComparison.OrdinalIgnoreCase)) DeleteDirectory.Delete(true);
                    }
                }
                return getList(Directory);
            }
            return null;
        }

        /// <summary>
        /// 上传小文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="lastWriteTime">最后修改时间</param>
        /// <param name="data">文件数据</param>
        /// <returns>文件同步状态</returns>
        public SynchronousState UploadAll(string path, DateTime lastWriteTime, ref SubArray<byte> data)
        {
            FileInfo FileInfo = new FileInfo(System.IO.Path.Combine(Path, path));
            if (FileInfo.FullName.StartsWith(Path, StringComparison.OrdinalIgnoreCase))
            {
                File.WriteAllBytes(FileInfo.FullName, data.ToArray());
                FileInfo.LastWriteTimeUtc = lastWriteTime;
                return SynchronousState.Success;
            }
            return SynchronousState.Unknown;
        }
        /// <summary>
        /// 创建文件上传
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="listFileItem">列表文件数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="data">文件数据</param>
        /// <param name="onCreated">上传回调处理</param>
        public void CreateUpload(string path, ref ListFileItem listFileItem, long index, ref SubArray<byte> data, Func<ReturnValue<UploadFileIdentity>, bool> onCreated)
        {
            ServerUploadFile uploadFile = null;
            try
            {
                uploadFile = new ServerUploadFile(this, path, ref listFileItem, index, ref data, onCreated);
                Monitor.Enter(fileLock);
                try
                {
                    files.Add(uploadFile.Identity, uploadFile);
                }
                finally { Monitor.Exit(fileLock); }
                onCreated = null;
            }
            finally
            {
                if (onCreated == null) uploadFile.Start();
                else onCreated(new UploadFileIdentity(SynchronousState.ServerException));
            }
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="fileIdentity">服务端返回文件信息</param>
        /// <param name="data">文件数据</param>
        /// <param name="onUploaded">上传回调处理</param>
        public void Upload(ref UploadFileIdentity fileIdentity, ref SubArray<byte> data, Func<ReturnValue<SynchronousState>, bool> onUploaded)
        {
            SynchronousState state = SynchronousState.IdentityError;
            try
            {
                if (fileIdentity.Tick == AutoCSer.Pub.StartTime.Ticks)
                {
                    ServerFile serverFile;
                    Monitor.Enter(fileLock);
                    if (files.TryGetValue(fileIdentity.Identity, out serverFile))
                    {
                        Monitor.Exit(fileLock);
                        state = SynchronousState.ServerException;
                        new UnionType { Value = serverFile }.ServerUploadFile.Upload(ref data, ref onUploaded);
                    }
                    else
                    {
                        Monitor.Exit(fileLock);
                    }
                }
            }
            finally
            {
                if (onUploaded != null) onUploaded(state);
            }
        }

        /// <summary>
        /// 创建文件下载
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="listFileItem">列表文件数据</param>
        /// <param name="onCreated">下载回调处理</param>
        public void CreateDownload(string path, ref ListFileItem listFileItem, Func<ReturnValue<DownloadFileIdentity>, bool> onCreated)
        {
            ServerDownloadFile downloadFile = null;
            SynchronousState state = SynchronousState.ServerException;
            try
            {
                FileInfo fileInfo = new FileInfo(System.IO.Path.Combine(System.IO.Path.Combine(Path, path), listFileItem.Name));
                if (fileInfo.Exists)
                {
                    downloadFile = new ServerDownloadFile(this, path, ref listFileItem, fileInfo, onCreated);
                    Monitor.Enter(fileLock);
                    try
                    {
                        files.Add(downloadFile.Identity, downloadFile);
                    }
                    finally { Monitor.Exit(fileLock); }
                    onCreated = null;
                }
                else state = SynchronousState.NotExists;
            }
            finally
            {
                if (onCreated == null) downloadFile.Start();
                else onCreated(new DownloadFileIdentity(state));
            }
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="tick">时钟周期标识</param>
        /// <param name="identity">文件编号</param>
        /// <param name="onDownload">下载回调处理</param>
        public void Download(long tick, long identity, Func<ReturnValue<DownloadData>, bool> onDownload)
        {
            SynchronousState state = SynchronousState.IdentityError;
            try
            {
                if (tick == AutoCSer.Pub.StartTime.Ticks)
                {
                    ServerFile serverFile;
                    Monitor.Enter(fileLock);
                    if (files.TryGetValue(identity, out serverFile))
                    {
                        Monitor.Exit(fileLock);
                        state = SynchronousState.ServerException;
                        new UnionType { Value = serverFile }.ServerDownloadFile.Download(ref onDownload);
                    }
                    else
                    {
                        Monitor.Exit(fileLock);
                    }
                }
            }
            finally
            {
                if (onDownload != null) onDownload(new DownloadData(state));
            }
        }
    }
}
