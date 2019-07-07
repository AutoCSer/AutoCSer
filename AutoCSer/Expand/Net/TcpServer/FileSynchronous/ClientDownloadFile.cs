using System;
using System.IO;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 下载文件
    /// </summary>
    internal sealed class ClientDownloadFile : ClientFile
    {
        /// <summary>
        /// 文件写入回调处理
        /// </summary>
        private AsyncCallback onWriteHandle;
        /// <summary>
        /// 文件下载回调处理
        /// </summary>
        private Action<ReturnValue<DownloadData>> onDownloadHandle;
        /// <summary>
        /// 服务端返回下载文件信息
        /// </summary>
        private DownloadFileIdentity fileIdentity;
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="client">文件同步客户端</param>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名称</param>
        /// <param name="fileNameKey">文件名称关键字</param>
        /// <param name="onCompleted">文件同步完成处理委托</param>
        internal ClientDownloadFile(Client client, string path, string name, ref FileNameKey fileNameKey, Action<ClientFile, SynchronousState> onCompleted)
            : base(client, path, name, ref fileNameKey, onCompleted, false)
        {
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns>文件同步状态</returns>
        internal SynchronousState Download()
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (fileInfo.Exists)
                {
                    client.IClient.CreateDownload(path, new ListFileItem(name, fileInfo.LastWriteTimeUtc, fileStream.Length), onCreated);
                }
                else
                {
                    client.IClient.CreateDownload(path, new ListFileItem(name, default(DateTime), 0), onCreated);
                }
                state = SynchronousState.Asynchronous;
            }
            finally
            {
                if (state != SynchronousState.Asynchronous) client.Remove(ref FileNameKey);
            }
            return state;
        }
        /// <summary>
        /// 文件下载创建回调处理
        /// </summary>
        /// <param name="fileIdentity"></param>
        private void onCreated(ReturnValue<DownloadFileIdentity> fileIdentity)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (fileIdentity.Type == ReturnType.Success)
                {
                    if (fileIdentity.Value != null)
                    {
                        this.fileIdentity = fileIdentity.Value;
                        if (this.fileIdentity.Tick == 0)
                        {
                            state = this.fileIdentity.GetErrorState();
                            if (state == SynchronousState.Success)
                            {
                                state = SynchronousState.Unknown;
                                byte[] data = this.fileIdentity.Data;
                                if (data == null)
                                {
                                    if (this.fileIdentity.ListFileItem.Length == 0)
                                    {
                                        fileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None, 1);
                                        fileStream.Dispose();
                                        fileStream = null;
                                        fileInfo.LastWriteTimeUtc = this.fileIdentity.ListFileItem.LastWriteTime;
                                        state = SynchronousState.Success;
                                    }
                                    else
                                    {
                                        if (fileInfo.Exists && this.fileIdentity.ListFileItem.Check(fileInfo)) state = SynchronousState.Success;
                                    }
                                }
                                else state = writeFrist();
                            }
                        }
                        else state = writeFrist();
                    }
                }
                else state = SynchronousState.TcpError;
            }
            catch { }
            finally { Remove(state); }
        }
        /// <summary>
        /// 首次写入文件数据
        /// </summary>
        /// <returns></returns>
        private SynchronousState writeFrist()
        {
            if (fileIdentity.Index == 0)
            {
                byte[] data = fileIdentity.Data;
                fileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None, data.Length, FileOptions.Asynchronous);
                fileStream.BeginWrite(data, 0, data.Length, onWriteHandle = onWrite, this);
                return SynchronousState.Asynchronous;
            }
            if (fileInfo.Exists)
            {
                byte[] data = fileIdentity.Data;
                fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, data.Length, FileOptions.Asynchronous);
                if (fileIdentity.ListFileItem.LastWriteTime == fileInfo.LastWriteTimeUtc && fileIdentity.Index == fileStream.Length)
                {
                    fileStream.Seek(0, SeekOrigin.End);
                    fileStream.BeginWrite(data, 0, data.Length, onWriteHandle = onWrite, this);
                    return SynchronousState.Asynchronous;
                }
            }
            return SynchronousState.Unknown;
        }
        /// <summary>
        /// 文件首次写入回调处理
        /// </summary>
        /// <param name="result"></param>
        private void onWrite(IAsyncResult result)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                fileStream.EndWrite(result);
                fileInfo.LastWriteTimeUtc = fileIdentity.ListFileItem.LastWriteTime;
                if (fileStream.Length == fileIdentity.ListFileItem.Length) state = SynchronousState.Success;
                else
                {
                    if (onDownloadHandle == null) onDownloadHandle = onDownload;
                    client.IClient.Download(fileIdentity.Tick, fileIdentity.Identity, onDownload);
                    state = SynchronousState.Asynchronous;
                }
            }
            catch { }
            finally { Remove(state); }
        }
        /// <summary>
        /// 文件下载回调处理
        /// </summary>
        /// <param name="downloadData"></param>
        private void onDownload(ReturnValue<DownloadData> downloadData)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (downloadData.Type == ReturnType.Success)
                {
                    if (downloadData.Value.State == SynchronousState.Success)
                    {
                        SubArray<byte> data = downloadData.Value.Data;
                        fileStream.BeginWrite(data.Array, data.Start, data.Length, onWriteHandle, this);
                        state = SynchronousState.Asynchronous;
                    }
                    else state = downloadData.Value.State;
                }
                else state = SynchronousState.TcpError;
            }
            catch { }
            finally { Remove(state); }
        }
    }
}
