using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 客户端文件
    /// </summary>
    public abstract class ClientFile
    {
        /// <summary>
        /// 文件同步客户端
        /// </summary>
        protected readonly Client client;
        /// <summary>
        /// 文件路径
        /// </summary>
        protected readonly string path;
        /// <summary>
        /// 文件名称
        /// </summary>
        protected readonly string name;
        /// <summary>
        /// 文件同步完成处理委托
        /// </summary>
        private readonly Action<ClientFile, SynchronousState> onCompleted;
        /// <summary>
        /// 文件信息
        /// </summary>
        protected readonly FileInfo fileInfo;
        /// <summary>
        /// 是否上传，否则为下载
        /// </summary>
        internal readonly bool IsUpload;
        /// <summary>
        /// 文件名称关键字
        /// </summary>
        internal FileNameKey FileNameKey;
        /// <summary>
        /// 文件流
        /// </summary>
        protected FileStream fileStream;
        /// <summary>
        /// 客户端文件
        /// </summary>
        /// <param name="client">文件同步客户端</param>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名称</param>
        /// <param name="fileNameKey">文件名称关键字</param>
        /// <param name="onCompleted">文件同步完成处理委托</param>
        /// <param name="isUpload">是否上传，否则为下载</param>
        internal ClientFile(Client client, string path, string name, ref FileNameKey fileNameKey, Action<ClientFile, SynchronousState> onCompleted, bool isUpload)
        {
            this.client = client;
            this.path = path;
            this.name = name;
            this.FileNameKey = fileNameKey;
            this.onCompleted = onCompleted;
            this.IsUpload = isUpload;
            fileInfo = new FileInfo(Path.Combine(path, name));
        }
        /// <summary>
        /// 删除同步文件
        /// </summary>
        /// <param name="state"></param>
        internal void Remove(SynchronousState state)
        {
            if (state != SynchronousState.Asynchronous)
            {
                client.Remove(ref FileNameKey);
                if (fileStream != null) fileStream.Dispose();
                if (onCompleted != null) onCompleted(this, state);
            }
        }
    }
}
