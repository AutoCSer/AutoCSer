using System;
using System.IO;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 上传文件
    /// </summary>
    internal sealed class ServerUploadFile : ServerFile
    {
        /// <summary>
        /// 文件写入回调处理
        /// </summary>
        private AsyncCallback onWriteHandle;
        /// <summary>
        /// 创建文件回调处理
        /// </summary>
        private readonly Func<ReturnValue<UploadFileIdentity>, bool> onCreated;
        /// <summary>
        /// 当前写入位置
        /// </summary>
        private long index;
        /// <summary>
        /// 写入数据
        /// </summary>
        private SubArray<byte> data;
        /// <summary>
        /// 文件上传回调
        /// </summary>
        private Func<ReturnValue<SynchronousState>, bool> onUploaded;
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="server">文件同步服务端</param>
        /// <param name="path">文件路径</param>
        /// <param name="listFileItem">列表文件数据</param>
        /// <param name="index">当前写入位置</param>
        /// <param name="data">写入数据</param>
        /// <param name="onCreated">创建文件回调处理</param>
        internal ServerUploadFile(Server server, string path, ref ListFileItem listFileItem, long index, ref SubArray<byte> data, Func<ReturnValue<UploadFileIdentity>, bool> onCreated)
            : base(server, path, ref listFileItem, null)
        {
            this.onCreated = onCreated;
            this.index = index;
            this.data = data;
        }
        /// <summary>
        /// 删除同步文件
        /// </summary>
        /// <param name="state">文件同步状态</param>
        private void removeCreate(SynchronousState state)
        {
            server.Remove(Identity);
            if (fileStream != null) fileStream.Dispose();
            onCreated(new UploadFileIdentity(state));
        }
        /// <summary>
        /// 开始创建文件
        /// </summary>
        internal void Start()
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (index == 0)
                {
                    fileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None, data.Length, FileOptions.Asynchronous);
                    fileStream.BeginWrite(data.Array, data.Start, data.Length, onWriteFirst, this);
                    state = SynchronousState.Asynchronous;
                }
                else if (fileInfo.Exists && index == fileInfo.Length)
                {
                    fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, data.Length, FileOptions.Asynchronous);
                    if (index == fileStream.Length && fileInfo.LastWriteTimeUtc == listFileItem.LastWriteTime)
                    {
                        fileStream.Seek(0, SeekOrigin.End);
                        fileStream.BeginWrite(data.Array, data.Start, data.Length, onWriteFirst, this);
                        state = SynchronousState.Asynchronous;
                    }
                }
            }
            finally
            {
                if (state != SynchronousState.Asynchronous) removeCreate(state);
            }
        }
        /// <summary>
        /// 第一次写入回调处理
        /// </summary>
        /// <param name="result"></param>
        private void onWriteFirst(IAsyncResult result)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                fileStream.EndWrite(result);
                if ((index += data.Length) == listFileItem.Length)
                {
                    fileStream.Dispose();
                    fileStream = null;
                }
                if (fileInfo.LastWriteTimeUtc != listFileItem.LastWriteTime) fileInfo.LastWriteTimeUtc = listFileItem.LastWriteTime;
                state = SynchronousState.Success;
            }
            catch { }
            finally
            {
                if (state == SynchronousState.Success && index != listFileItem.Length)
                {
                    checkTimeoutSeconds = AutoCSer.Date.NowTime.CurrentSeconds;
                    if (!onCreated(new UploadFileIdentity(Identity)))
                    {
                        server.Remove(Identity);
                        fileStream.Dispose();
                    }
                }
                else removeCreate(state);
            }
        }
        /// <summary>
        /// 删除同步文件
        /// </summary>
        /// <param name="state">文件同步状态</param>
        private void remove(SynchronousState state)
        {
            server.Remove(Identity);
            if (fileStream != null) fileStream.Dispose();
            onUploaded(state);
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="data"></param>
        /// <param name="onUploaded"></param>
        internal void Upload(ref SubArray<byte> data, ref Func<ReturnValue<SynchronousState>, bool> onUploaded)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                this.onUploaded = onUploaded;
                this.data = data;
                checkTimeoutSeconds = AutoCSer.Date.NowTime.CurrentSeconds;
                onUploaded = null;

                if (onWriteHandle == null) onWriteHandle = onWrite;
                fileStream.BeginWrite(data.Array, data.Start, data.Length, onWriteHandle, this);
                state = SynchronousState.Asynchronous;
            }
            finally
            {
                if (state != SynchronousState.Asynchronous) remove(state);
            }
        }
        /// <summary>
        /// 文件写入回调处理
        /// </summary>
        /// <param name="result"></param>
        private void onWrite(IAsyncResult result)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                fileStream.EndWrite(result);
                if ((index += data.Length) == listFileItem.Length)
                {
                    fileStream.Dispose();
                    fileStream = null;
                }
                if (fileInfo.LastWriteTimeUtc != listFileItem.LastWriteTime) fileInfo.LastWriteTimeUtc = listFileItem.LastWriteTime;
                state = SynchronousState.Success;
            }
            catch { }
            finally
            {
                if (state == SynchronousState.Success && index != listFileItem.Length)
                {
                    checkTimeoutSeconds = AutoCSer.Date.NowTime.CurrentSeconds;
                    if (!onUploaded(state))
                    {
                        server.Remove(Identity);
                        fileStream.Dispose();
                    }
                }
                else remove(state);
            }
        }
    }
}
