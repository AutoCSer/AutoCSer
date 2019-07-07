using System;
using System.IO;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 下载文件
    /// </summary>
    internal sealed class ServerDownloadFile : ServerFile
    {
        /// <summary>
        /// 文件读取回调处理
        /// </summary>
        private AsyncCallback onReadHandle;
        /// <summary>
        /// 创建文件回调处理
        /// </summary>
        private readonly Func<ReturnValue<DownloadFileIdentity>, bool> onCreated;
        /// <summary>
        /// 文件数据缓冲区
        /// </summary>
        private byte[] buffer;
        /// <summary>
        /// 当前读取位置
        /// </summary>
        private long index;
        /// <summary>
        /// 文件读取字节数
        /// </summary>
        private int readSize;
        /// <summary>
        /// 文件下载回调处理
        /// </summary>
        private Func<ReturnValue<DownloadData>, bool> onDownload;
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="server">文件同步服务端</param>
        /// <param name="path">文件路径</param>
        /// <param name="listFileItem">列表文件数据</param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="onCreated">创建文件回调处理</param>
        internal ServerDownloadFile(Server server, string path, ref ListFileItem listFileItem, FileInfo fileInfo, Func<ReturnValue<DownloadFileIdentity>, bool> onCreated)
            : base(server, path, ref listFileItem, fileInfo)
        {
            this.onCreated = onCreated;
        }
        /// <summary>
        /// 删除同步文件
        /// </summary>
        /// <param name="fileIdentity">服务端返回下载文件信息</param>
        private void removeCreate(DownloadFileIdentity fileIdentity)
        {
            server.Remove(Identity);
            if (fileStream != null) fileStream.Dispose();
            onCreated(fileIdentity);
        }
        /// <summary>
        /// 开始创建文件
        /// </summary>
        internal void Start()
        {
            DownloadFileIdentity fileIdentity = null;
            bool isAsynchronous = false;
            try
            {
                int bufferSize = (int)Math.Min(server.DownloadBufferSize, fileInfo.Length);
                fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
                if (fileStream.Length == fileInfo.Length)
                {
                    if (fileStream.Length != 0 && !listFileItem.Check(fileInfo))
                    {
                        buffer = new byte[bufferSize];
                        if (fileStream.Length <= buffer.Length)
                        {
                            fileStream.BeginRead(buffer, 0, bufferSize, onReadAll, this);
                        }
                        else
                        {
                            if (listFileItem.LastWriteTime == fileInfo.LastWriteTimeUtc && listFileItem.Length < fileStream.Length)
                            {
                                fileStream.Seek(index = listFileItem.Length, SeekOrigin.Begin);
                            }
                            fileStream.BeginRead(buffer, 0, readSize = (int)Math.Min(fileStream.Length - index, bufferSize), onReadFirst, this);
                        }
                        isAsynchronous = true;
                    }
                    else fileIdentity = new DownloadFileIdentity(fileInfo);
                }
            }
            finally
            {
                if (!isAsynchronous) removeCreate(fileIdentity);
            }
        }
        /// <summary>
        /// 小文件读取回调处理
        /// </summary>
        /// <param name="result"></param>
        private void onReadAll(IAsyncResult result)
        {
            DownloadFileIdentity fileIdentity = null;
            try
            {
                fileIdentity = fileStream.EndRead(result) == fileStream.Length ? new DownloadFileIdentity(fileInfo, buffer) : new DownloadFileIdentity(SynchronousState.ReadError);
            }
            catch { }
            finally { removeCreate(fileIdentity); }
        }
        /// <summary>
        /// 第一次读取文件操作
        /// </summary>
        /// <param name="result"></param>
        private void onReadFirst(IAsyncResult result)
        {
            DownloadFileIdentity fileIdentity = null;
            try
            {
                if (fileStream.EndRead(result) == readSize)
                {
                    if ((index += readSize) == fileStream.Length) fileIdentity = new DownloadFileIdentity(fileInfo, buffer, index - readSize, index);
                    else fileIdentity = new DownloadFileIdentity(Identity, fileInfo, buffer, index - readSize, index);
                }
                else fileIdentity = new DownloadFileIdentity(SynchronousState.ReadError);
            }
            catch { }
            finally
            {
                if (fileIdentity != null && fileIdentity.Tick != 0)
                {
                    checkTimeoutSeconds = AutoCSer.Date.NowTime.CurrentSeconds;
                    onCreated(fileIdentity);
                }
                else removeCreate(fileIdentity);
            }
        }
        /// <summary>
        /// 删除同步文件
        /// </summary>
        /// <param name="data">下载数据</param>
        private void remove(DownloadData data)
        {
            server.Remove(Identity);
            if (fileStream != null) fileStream.Dispose();
            onDownload(data);
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="onDownload">下载回调处理</param>
        internal void Download(ref Func<ReturnValue<DownloadData>, bool> onDownload)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                this.onDownload = onDownload;
                checkTimeoutSeconds = AutoCSer.Date.NowTime.CurrentSeconds;
                onDownload = null;

                if (onReadHandle == null) onReadHandle = onRead;
                fileStream.BeginRead(buffer, 0, readSize = (int)Math.Min(fileStream.Length - index, buffer.Length), onReadHandle, this);
                state = SynchronousState.Asynchronous;
            }
            finally
            {
                if (state != SynchronousState.Asynchronous) remove(new DownloadData(state));
            }
        }
        /// <summary>
        /// 读取文件操作
        /// </summary>
        /// <param name="result"></param>
        private void onRead(IAsyncResult result)
        {
            DownloadData downloadData = new DownloadData(SynchronousState.Unknown);
            long nextSize = 0;
            try
            {
                if (fileStream.EndRead(result) == readSize)
                {
                    nextSize = fileStream.Length - (index += readSize);
                    downloadData.Set(buffer, readSize);
                }
                else downloadData.State = SynchronousState.ReadError;
            }
            catch { }
            finally
            {
                if (downloadData.State == SynchronousState.Success && nextSize != 0)
                {
                    checkTimeoutSeconds = AutoCSer.Date.NowTime.CurrentSeconds;
                    onDownload(downloadData);
                }
                else remove(downloadData);
            }
        }
    }
}
