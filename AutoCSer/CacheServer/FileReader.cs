using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 文件读取器
    /// </summary>
    internal sealed class FileReader : AutoCSer.Threading.DoubleLink<FileReader>
    {
        /// <summary>
        /// 当前读取位置
        /// </summary>
        private long index;
        /// <summary>
        /// 文件流写入器
        /// </summary>
        private readonly FileStreamWriter file;
        /// <summary>
        /// 读取文件回调委托
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReadFileParameter>, bool> onRead;
        /// <summary>
        /// 压缩数据缓冲区
        /// </summary>
        private readonly byte[] buffer;
        /// <summary>
        /// 读取数据
        /// </summary>
        private readonly Action readHandle;
        /// <summary>
        /// 文件流
        /// </summary>
        private FileStream fileStream;
        /// <summary>
        /// 是否正在读取数据
        /// </summary>
        private int isReading;
        /// <summary>
        /// 读取文件回调委托调用是否出错
        /// </summary>
        private bool isOnReadError;
        /// <summary>
        /// 文件读取器
        /// </summary>
        /// <param name="file">文件流写入器</param>
        /// <param name="index">当前读取位置</param>
        /// <param name="onRead">读取文件回调委托</param>
        internal FileReader(FileStreamWriter file, long index, Func<AutoCSer.Net.TcpServer.ReturnValue<ReadFileParameter>, bool> onRead)
        {
            this.file = file;
            this.index = index;
            this.onRead = onRead;
            buffer = new byte[(int)file.Config.ReaderBufferSize];
            readHandle = read;
        }
        /// <summary>
        /// 开始读取文件
        /// </summary>
        internal void Start()
        {
            bool isFree = true;
            try
            {
                fileStream = new FileStream(file.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, buffer.Length, FileOptions.SequentialScan | FileOptions.Asynchronous);
                fileStream.Seek(index, SeekOrigin.Begin);
                Interlocked.Exchange(ref isReading, 1);
                file.Readers.PushNotNull(this);
                isFree = false;
                read();
            }
            finally
            {
                if (isFree) Free();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void Free()
        {
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
            if (!isOnReadError)
            {
                isOnReadError = true;
                onRead(default(ReadFileParameter));
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        private void read()
        {
            bool isFree = true;
            try
            {
                if (index < file.FileLength)
                {
                    int count = fileStream.Read(buffer, 0, (int)Math.Min(buffer.Length, file.FileLength - index));
                    index += count;
                    ReadFileParameter parameter = new ReadFileParameter { Reader = this };
                    parameter.Data.Set(buffer, 0, count);
                    if (onRead(parameter)) isFree = false;
                    else isOnReadError = true;
                }
                else
                {
                    Interlocked.Exchange(ref isReading, 0);
                    isFree = false;
                }
            }
            finally
            {
                if (isFree) Free();
            }
        }
        /// <summary>
        /// 继续读取数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Next()
        {
            if (index < file.FileLength) AutoCSer.Threading.ThreadPool.TinyBackground.Start(readHandle);
            else Interlocked.Exchange(ref isReading, 0);
        }
        /// <summary>
        /// 尝试读取数据
        /// </summary>
        /// <returns>是否需要尝试读取</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool TryRead()
        {
            if (Interlocked.CompareExchange(ref isReading, 1, 0) == 0)
            {
                AutoCSer.Threading.ThreadPool.TinyBackground.Start(readHandle);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 尝试读取数据
        /// </summary>
        /// <param name="fileLength">当前文件长度</param>
        /// <returns>是否需要尝试读取</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool TryRead(long fileLength)
        {
            return index < fileLength && TryRead();
        }
    }
}
