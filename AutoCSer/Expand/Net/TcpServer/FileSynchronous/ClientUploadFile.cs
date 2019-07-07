using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 上传文件
    /// </summary>
    internal sealed class ClientUploadFile : ClientFile
    {
        /// <summary>
        /// 文件读取回调处理
        /// </summary>
        private AsyncCallback onReadHandle;
        /// <summary>
        /// 文件数据缓冲区
        /// </summary>
        private byte[] buffer;
        /// <summary>
        /// 服务端返回文件信息
        /// </summary>
        private UploadFileIdentity fileIdentity;
        /// <summary>
        /// 当前读取位置
        /// </summary>
        private long index;
        /// <summary>
        /// 文件读取字节数
        /// </summary>
        private int readSize;
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="client">文件同步客户端</param>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名称</param>
        /// <param name="fileNameKey">文件名称关键字</param>
        /// <param name="onCompleted">文件同步完成处理委托</param>
        internal ClientUploadFile(Client client, string path, string name, ref FileNameKey fileNameKey, Action<ClientFile, SynchronousState> onCompleted)
            : base(client, path, name, ref fileNameKey, onCompleted, true)
        {
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns>文件同步状态</returns>
        internal SynchronousState Upload()
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (fileInfo.Exists)
                {
                    client.IClient.Get(Path.Combine(path, name), onGet);
                    state = SynchronousState.Asynchronous;
                }
                else state = SynchronousState.NotExists;
            }
            finally
            {
                if (state != SynchronousState.Asynchronous) client.Remove(ref FileNameKey);
            }
            return state;
        }
        /// <summary>
        /// 获取服务端文件信息回调处理
        /// </summary>
        /// <param name="serverFile"></param>
        private void onGet(ReturnValue<ListFileItem> serverFile)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (serverFile.Type == ReturnType.Success)
                {
                    if (serverFile.Value.Check(fileInfo)) state = SynchronousState.Success;
                    else if (fileInfo.Length != 0)
                    {
                        int bufferSize = (int)Math.Min(client.UploadDBufferSize, fileInfo.Length);
                        fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
                        if (fileStream.Length == fileInfo.Length)
                        {
                            if (fileStream.Length != 0)
                            {
                                buffer = new byte[bufferSize];
                                if (fileStream.Length <= buffer.Length) fileStream.BeginRead(buffer, 0, bufferSize, onReadAll, this);
                                else
                                {
                                    if (serverFile.Value.LastWriteTime == fileInfo.LastWriteTimeUtc && serverFile.Value.Length < fileStream.Length)
                                    {
                                        fileStream.Seek(index = serverFile.Value.Length, SeekOrigin.Begin);
                                    }
                                    fileStream.BeginRead(buffer, 0, readSize = (int)Math.Min(fileStream.Length - index, bufferSize), onReadFirst, this);
                                }
                            }
                            else client.IClient.UploadAll(Path.Combine(path, name), fileInfo.LastWriteTimeUtc, default(SubArray<byte>), onUploadedAll);
                            state = SynchronousState.Asynchronous;
                        }
                    }
                    else
                    {
                        client.IClient.UploadAll(Path.Combine(path, name), fileInfo.LastWriteTimeUtc, default(SubArray<byte>), onUploadedAll);
                        state = SynchronousState.Asynchronous;
                    }
                }
                else state = SynchronousState.TcpError;
            }
            catch { }
            finally { Remove(state); }
        }
        /// <summary>
        /// 小文件读取回调处理
        /// </summary>
        /// <param name="result"></param>
        private void onReadAll(IAsyncResult result)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                int size = fileStream.EndRead(result);
                if (size == fileStream.Length)
                {
                    fileStream.Dispose();
                    fileStream = null;
                    client.IClient.UploadAll(Path.Combine(path, name), fileInfo.LastWriteTimeUtc, new SubArray<byte>(buffer, 0, size), onUploadedAll);
                    state = SynchronousState.Asynchronous;
                }
                else state = SynchronousState.ReadError;
            }
            catch { }
            finally { Remove(state); }
        }
        /// <summary>
        /// 小文件上传回调处理
        /// </summary>
        /// <param name="synchronousState"></param>
        private void onUploadedAll(ReturnValue<SynchronousState> synchronousState)
        {
            Remove(synchronousState.Type == ReturnType.Success ? synchronousState.Value : SynchronousState.TcpError);
        }
        /// <summary>
        /// 第一次读取文件操作
        /// </summary>
        /// <param name="result"></param>
        private void onReadFirst(IAsyncResult result)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                int size = fileStream.EndRead(result);
                if (size == readSize)
                {
                    if ((index += size) == fileStream.Length)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                    client.IClient.CreateUpload(path, new ListFileItem(name, fileInfo.LastWriteTimeUtc, fileStream.Length), index - size, new SubArray<byte>(buffer, 0, size), onCreated);
                    state = SynchronousState.Asynchronous;
                }
                else state = SynchronousState.ReadError;
            }
            catch { }
            finally { Remove(state); }
        }
        /// <summary>
        /// 文件上传创建回调处理
        /// </summary>
        /// <param name="fileIdentity"></param>
        private void onCreated(ReturnValue<UploadFileIdentity> fileIdentity)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (fileIdentity.Type == ReturnType.Success)
                {
                    this.fileIdentity = fileIdentity.Value;
                    if (this.fileIdentity.Tick != 0)
                    {
                        if (index != fileStream.Length)
                        {
                            read();
                            state = SynchronousState.Asynchronous;
                        }
                    }
                    else state = this.fileIdentity.GetErrorState();
                }
                else state = SynchronousState.TcpError;
            }
            catch { }
            finally { Remove(state); }
        }
        /// <summary>
        /// 继续读取文件
        /// </summary>
        private void read()
        {
            if (onReadHandle == null) onReadHandle = onReaded;
            fileStream.BeginRead(buffer, 0, readSize = (int)Math.Min(fileStream.Length - index, buffer.Length), onReadHandle, this);
        }
        /// <summary>
        /// 文件读取回调处理
        /// </summary>
        /// <param name="result"></param>
        private void onReaded(IAsyncResult result)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                int size = fileStream.EndRead(result);
                if (size == readSize)
                {
                    if ((index += size) == fileStream.Length)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                    client.IClient.Upload(fileIdentity, new SubArray<byte>(buffer, 0, size), onUploaded);
                    state = SynchronousState.Asynchronous;
                }
                else state = SynchronousState.ReadError;
            }
            catch { }
            finally { Remove(state); }
        }
        /// <summary>
        /// 文件上传回调处理
        /// </summary>
        /// <param name="synchronousState"></param>
        private void onUploaded(ReturnValue<SynchronousState> synchronousState)
        {
            SynchronousState state = SynchronousState.Unknown;
            try
            {
                if (synchronousState.Type == ReturnType.Success)
                {
                    if (synchronousState.Value == SynchronousState.Success)
                    {
                        if (index == fileStream.Length) state = SynchronousState.Success;
                        else
                        {
                            read();
                            state = SynchronousState.Asynchronous;
                        }
                    }
                    else state = synchronousState.Value;
                }
                else state = SynchronousState.TcpError;
            }
            catch { }
            finally { Remove(state); }
        }
    }
}
