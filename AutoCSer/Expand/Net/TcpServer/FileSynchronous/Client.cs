using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 文件同步客户端
    /// </summary>
    public sealed class Client
    {
        /// <summary>
        /// 最大文件上传字节数
        /// </summary>
        internal readonly int UploadDBufferSize;
        /// <summary>
        /// 文件同步客户端操作接口
        /// </summary>
        internal readonly IClient IClient;
        /// <summary>
        /// 未完成同步文件集合
        /// </summary>
        private readonly Dictionary<FileNameKey, ClientFile> files = DictionaryCreator<FileNameKey>.Create<ClientFile>();
        /// <summary>
        /// 未完成同步文件集合 访问锁
        /// </summary>
        private readonly object fileLock = new object();
        /// <summary>
        /// 文件同步客户端
        /// </summary>
        /// <param name="client">文件同步客户端操作接口</param>
        /// <param name="uploadDBufferSize">最大文件上传字节数，默认为 4KB</param>
        public Client(IClient client, int uploadDBufferSize = 4 << 10)
        {
            if (client == null) throw new ArgumentNullException();
            IClient = client;
            UploadDBufferSize = Math.Max(uploadDBufferSize, 1 << 10);
        }

        /// <summary>
        /// 删除同步文件
        /// </summary>
        /// <param name="fileNameKey"></param>
        internal void Remove(ref FileNameKey fileNameKey)
        {
            Monitor.Enter(fileLock);
            files.Remove(fileNameKey);
            Monitor.Exit(fileLock);
        }
        /// <summary>
        /// 增加同步文件
        /// </summary>
        /// <param name="synchronousFile">客户端文件</param>
        /// <param name="fileNameKey">文件名称关键字</param>
        /// <returns>文件同步状态</returns>
        private SynchronousState synchronous(ClientFile synchronousFile, ref FileNameKey fileNameKey)
        {
            ClientFile clientFile;
            Monitor.Enter(fileLock);
            try
            {
                if (files.TryGetValue(fileNameKey, out clientFile)) return clientFile.IsUpload ? SynchronousState.Uploading : SynchronousState.Downloading;
                files.Add(fileNameKey, synchronousFile);
            }
            finally { Monitor.Exit(fileLock); }
            return SynchronousState.Success;
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名称</param>
        /// <param name="onCompleted">文件同步完成处理委托</param>
        /// <returns>文件同步状态</returns>
        public SynchronousState Upload(string path, string name, Action<ClientFile, SynchronousState> onCompleted)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                FileNameKey fileNameKey = new FileNameKey(path.FileNameToLower(), name.FileNameToLower());
                ClientUploadFile uploadFile = new ClientUploadFile(this, path, name, ref fileNameKey, onCompleted);
                state = synchronous(uploadFile, ref fileNameKey);
                if (state == SynchronousState.Success)
                {
                    onCompleted = null;
                    return uploadFile.Upload();
                }
                return state;
            }
            finally
            {
                if (onCompleted != null) onCompleted(null, state);
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名称</param>
        /// <param name="onCompleted">文件同步完成处理委托</param>
        /// <returns>文件同步状态</returns>
        public SynchronousState Download(string path, string name, Action<ClientFile, SynchronousState> onCompleted)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                FileNameKey fileNameKey = new FileNameKey(path.FileNameToLower(), name.FileNameToLower());
                ClientDownloadFile downloadFile = new ClientDownloadFile(this, path, name, ref fileNameKey, onCompleted);
                state = synchronous(downloadFile, ref fileNameKey);
                if (state == SynchronousState.Success)
                {
                    onCompleted = null;
                    return downloadFile.Download();
                }
                return state;
            }
            finally
            {
                if (onCompleted != null) onCompleted(null, state);
            }
        }
    }
}
