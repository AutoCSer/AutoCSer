using System;
using System.IO;
using System.Text;
using AutoCSer.Log;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.IO
{
    /// <summary>
    /// 文件流写入器
    /// </summary>
    internal unsafe sealed class FileStreamWriter : IDisposable
    {
        /// <summary>
        /// 等待缓冲区
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct WaitBuffer
        {
            /// <summary>
            /// 缓冲区
            /// </summary>
            public SubBuffer.PoolBufferFull Buffer;
            /// <summary>
            /// 当前写入位置
            /// </summary>
            public int WriteIndex;
            /// <summary>
            /// 是否等待
            /// </summary>
            public bool IsWait;
            /// <summary>
            /// 是否需要处理写入数据
            /// </summary>
            internal bool IsBuffer
            {
                get { return Buffer.Buffer != null || IsWait; }
            }
            ///// <summary>
            ///// 设置等待缓冲区
            ///// </summary>
            ///// <param name="buffer">缓冲区</param>
            //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            //public void Set(ref SubBuffer.PoolBufferFull buffer)
            //{
            //    Buffer = buffer;
            //    WaitLength = 0;
            //}
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Clear()
            {
                Buffer.Buffer = null;
                Buffer.PoolBuffer.Pool = null;
                IsWait = false;
            }
            /// <summary>
            /// 释放缓冲区
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Free()
            {
                Buffer.PoolBuffer.Free();
                Buffer.Buffer = null;
            }
            /// <summary>
            /// 写入文件编码BOM
            /// </summary>
            /// <param name="writer">文件流写入器</param>
            /// <param name="encoding"></param>
            internal void WriteBom(FileStreamWriter writer, Encoding encoding)
            {
                if (encoding != null)
                {
                    FileBom bom = default(FileBom);
                    FileBom.Get(encoding, ref bom);
                    if (bom.Length != 0)
                    {
                        Get(writer.bufferPool);
                        IsWait = false;
                        fixed (byte* bufferFixed = Buffer.Buffer) *(uint*)(bufferFixed + Buffer.StartIndex) = bom.Bom;
                        writer.fileBufferLength = writer.openLength = bom.Length;
                        WriteIndex += bom.Length;
                    }
                }
            }
            /// <summary>
            /// 获取缓冲区
            /// </summary>
            /// <param name="bufferPool"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Get(SubBuffer.Pool bufferPool)
            {
                bufferPool.Get(ref Buffer);
                WriteIndex = Buffer.StartIndex;
                IsWait = false;
            }
            /// <summary>
            /// 写入字符串
            /// </summary>
            /// <param name="value"></param>
            /// <param name="encoding"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Write(string value, ref EncodingCache encoding)
            {
                WriteIndex += encoding.WriteBytesNotEmpty(value, Buffer.Buffer, WriteIndex);
            }
            /// <summary>
            /// 写入字符串
            /// </summary>
            /// <param name="value"></param>
            /// <param name="buffer"></param>
            /// <param name="encoding"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void WriteNotPool(string value, byte[] buffer, ref EncodingCache encoding)
            {
                Buffer.Set(buffer, 0);
                WriteIndex = encoding.WriteBytesNotEmpty(value, buffer);
                IsWait = false;
            }
            /// <summary>
            /// 判断是否存在写入空间
            /// </summary>
            /// <param name="length"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool CanWrite(int length)
            {
                if (Buffer.Buffer == null) return false;
                int endIndex = WriteIndex + length;
                return endIndex <= Buffer.StartIndex + Buffer.PoolBuffer.Pool.Size;
            }
            /// <summary>
            /// 写入文件
            /// </summary>
            /// <param name="writer"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void WriteFile(FileStreamWriter writer)
            {
                if (Buffer.Buffer != null)
                {
                    int length = WriteIndex - Buffer.StartIndex;
                    writer.fileStream.Write(Buffer.Buffer, Buffer.StartIndex, length);
                    writer.fileLength += length;
                    Free();
                }
                if (IsWait) writer.pulseWait();
            }
            /// <summary>
            /// 释放缓冲区
            /// </summary>
            /// <param name="writer"></param>
            /// <returns>是否等待</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool Error(FileStreamWriter writer)
            {
                if (Buffer.Buffer != null)
                {
                    writer.fileLength += WriteIndex - Buffer.StartIndex;
                    Free();
                }
                if (IsWait) writer.setErrorWaitFileLength();
                return IsWait;
            }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 缓冲区池
        /// </summary>
        private readonly SubBuffer.Pool bufferPool;
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly ILog log;
        /// <summary>
        /// 缓存操作锁
        /// </summary>
        private readonly object bufferLock = new object();
        /// <summary>
        /// 等待缓存写入操作锁
        /// </summary>
        private readonly object waitLock = new object();
        /// <summary>
        /// 文件共享方式
        /// </summary>
        private readonly FileShare fileShare;
        /// <summary>
        /// 附加选项
        /// </summary>
        private readonly FileOptions fileOption;
        /// <summary>
        /// 文件流
        /// </summary>
        private FileStream fileStream;

        /// <summary>
        /// 缓冲区
        /// </summary>
        private WaitBuffer buffer;
        /// <summary>
        /// 待写入文件缓存集合
        /// </summary>
        private LeftArray<WaitBuffer> buffers = new LeftArray<WaitBuffer>(sizeof(int));
        /// <summary>
        /// 正在写入文件缓存集合
        /// </summary>
        private LeftArray<WaitBuffer> currentBuffers = new LeftArray<WaitBuffer>(sizeof(int));
        /// <summary>
        /// 文件打开时字节长度（包含未写入的 BOM）
        /// </summary>
        private long openLength;
        /// <summary>
        /// 文件有效长度(已经写入)
        /// </summary>
        private long fileLength;
        /// <summary>
        /// 当前写入缓存后的文件长度
        /// </summary>
        private long fileBufferLength;
        /// <summary>
        /// 等待文件长度
        /// </summary>
        private long waitFileLength;
        /// <summary>
        /// 新写入文件长度
        /// </summary>
        internal long NewLength
        {
            get { return fileBufferLength - openLength; }
        }
        /// <summary>
        /// 文件编码
        /// </summary>
        private EncodingCache encoding;
        /// <summary>
        /// 是否正在写文件
        /// </summary>
        private bool isWritting;
        /// <summary>
        /// 是否出现异常错误
        /// </summary>
        private bool isError;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 文件流写入器
        /// </summary>
        /// <param name="fileName">文件全名</param>
        /// <param name="mode">打开方式</param>
        /// <param name="fileShare">共享访问方式</param>
        /// <param name="fileOption">附加选项</param>
        /// <param name="bufferSize">缓冲区字节大小</param>
        /// <param name="log">日志处理</param>
        /// <param name="encoding">文件编码</param>
        public FileStreamWriter(string fileName, FileMode mode = FileMode.CreateNew, FileShare fileShare = FileShare.None, FileOptions fileOption = FileOptions.None, SubBuffer.Size bufferSize = SubBuffer.Size.Kilobyte4, ILog log = null, Encoding encoding = null)
            : this(fileName, mode, fileShare, fileOption, bufferSize, log, new EncodingCache(encoding ?? Encoding.UTF8))
        {
        }
        /// <summary>
        /// 文件流写入器
        /// </summary>
        /// <param name="fileName">文件全名</param>
        /// <param name="mode">打开方式</param>
        /// <param name="fileShare">共享访问方式</param>
        /// <param name="fileOption">附加选项</param>
        /// <param name="bufferSize">缓冲区字节大小</param>
        /// <param name="log">日志处理</param>
        /// <param name="encoding">文件编码</param>
        internal FileStreamWriter(string fileName, FileMode mode, FileShare fileShare, FileOptions fileOption, SubBuffer.Size bufferSize, ILog log, EncodingCache encoding)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName is null");
            FileName = fileName;
            this.log = log;
            this.fileShare = fileShare;
            this.fileOption = fileOption;
            this.encoding = encoding;
            bufferPool = SubBuffer.Pool.GetPool(bufferSize);
            open(mode);
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="mode">打开方式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe void open(FileMode mode)
        {
            fileStream = new FileStream(FileName, mode, FileAccess.Write, fileShare, bufferPool.Size, fileOption);
            fileLength = fileBufferLength = waitFileLength = openLength = fileStream.Length;
            if (fileLength == 0) buffer.WriteBom(this, encoding.Encoding);
            else fileStream.Seek(0, SeekOrigin.End);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Monitor.Enter(bufferLock);
            if (isDisposed)
            {
                Monitor.Exit(bufferLock);
                return;
            }
            isDisposed = true;
            Monitor.Exit(bufferLock);
            GC.SuppressFinalize(this);
            Monitor.Enter(bufferLock);
            wait();
            fileStream.Dispose();
            fileStream = null;
        }
        /// <summary>
        /// 等待缓存写入
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Flush()
        {
            Monitor.Enter(bufferLock);
            if (isDisposed || isError)
            {
                Monitor.Exit(bufferLock);
                return;
            }
            wait();
        }
        /// <summary>
        /// 等待缓存写入
        /// </summary>
        private void wait()
        {
            if (isWritting)
            {
                long fileBufferLength = this.fileBufferLength;
                buffer.IsWait = true;
                Monitor.Exit(bufferLock);
                do
                {
                    Monitor.Enter(waitLock);
                    if (this.waitFileLength >= fileBufferLength)
                    {
                        Monitor.Exit(waitLock);
                        return;
                    }
                    Monitor.Wait(waitLock);
                    Monitor.Exit(waitLock);
                }
                while (true);
            }
            else Monitor.Exit(bufferLock);
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal long WriteNotEmpty(string value)
        {
            int dataLength = encoding.GetByteCountNotNull(value);
            Monitor.Enter(bufferLock);
            if (isDisposed || isError)
            {
                Monitor.Exit(bufferLock);
                return -1;
            }
            long fileBufferLength = this.fileBufferLength;
            bool isWritting = this.isWritting;
            if (buffer.IsBuffer)
            {
                if (buffer.CanWrite(dataLength))
                {
                    buffer.Write(value, ref encoding);
                    this.fileBufferLength += dataLength;
                    this.isWritting = true;
                    Monitor.Exit(bufferLock);
                }
                else
                {
                    try
                    {
                        buffers.PrepLength(2);
                        buffers.Array[buffers.Length++] = buffer;
                        buffer.Clear();
                        buffers.Array[buffers.Length++].WriteNotPool(value, new byte[dataLength], ref encoding);
                        this.fileBufferLength += dataLength;
                        this.isWritting = true;
                    }
                    finally { Monitor.Exit(bufferLock); }
                }
            }
            else
            {
                try
                {
                    if (dataLength < bufferPool.Size)
                    {
                        buffer.Get(bufferPool);
                        buffer.Write(value, ref encoding);
                    }
                    else
                    {
                        buffers.PrepLength(1);
                        buffers.Array[buffers.Length++].WriteNotPool(value, new byte[dataLength], ref encoding);
                    }
                    this.fileBufferLength += dataLength;
                    this.isWritting = true;
                }
                finally { Monitor.Exit(bufferLock); }
            }
            if (!isWritting) AutoCSer.Threading.ThreadPool.Tiny.FastStart(this, AutoCSer.Threading.Thread.CallType.FileStreamWriteFile);
            return fileBufferLength;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        internal void WriteFile()
        {
            bool isErrorWait = isError;
            do
            {
                Monitor.Enter(bufferLock);
                if (buffers.Length == 0)
                {
                    if (buffer.IsBuffer)
                    {
                        currentBuffers.Array[0] = buffer;
                        buffer.Clear();
                        ++currentBuffers.Length;
                    }
                    else
                    {
                        isWritting = false;
                        Monitor.Exit(bufferLock);
                        if (isErrorWait)
                        {
                            Monitor.Enter(waitLock);
                            Monitor.PulseAll(waitLock);
                            Monitor.Exit(waitLock);
                        }
                        return;
                    }
                }
                else currentBuffers.Exchange(ref buffers);
                Monitor.Exit(bufferLock);
                WaitBuffer[] bufferArray = currentBuffers.Array;
                int count = currentBuffers.Length, index = 0;
                if (!isError)
                {
                    try
                    {
                        do
                        {
                            bufferArray[index].WriteFile(this);
                        }
                        while (++index != count);
                    }
                    catch (Exception error)
                    {
                        isError = true;
                        log.Add(Log.LogType.Error, error);
                        AutoCSer.Threading.ThreadPool.Tiny.FastStart(this, AutoCSer.Threading.Thread.CallType.FileStreamWriterDispose);
                    }
                }
                while (index != count) isErrorWait |= bufferArray[index++].Error(this);
                currentBuffers.Length = 0;
            }
            while (true);
        }
        /// <summary>
        /// 唤醒等待
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void pulseWait()
        {
            fileStream.Flush(true);
            waitFileLength = fileLength;
            Monitor.Enter(waitLock);
            Monitor.PulseAll(waitLock);
            Monitor.Exit(waitLock);
        }
        /// <summary>
        /// 设置等待文件长度
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setErrorWaitFileLength()
        {
            waitFileLength = fileLength;
        }
    }
}
