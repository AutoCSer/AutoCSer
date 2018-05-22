using System;
using System.IO;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 文件块
    /// </summary>
    public unsafe sealed class File : BlockBase, IBlock, IDisposable
    {
        /// <summary>
        /// 文件扩展名 AutoCSer File Block
        /// </summary>
        public const string ExtensionName = ".afb";
        /// <summary>
        /// 文件有效长度(已经写入)
        /// </summary>
        private long fileLength;
        /// <summary>
        /// 文件名称
        /// </summary>
        private string fileName;
        /// <summary>
        /// 写文件流
        /// </summary>
        private FileStream writeStream;
        /// <summary>
        /// 数据读取请求链表
        /// </summary>
        private ReadRequest.YieldQueue readRequests = new ReadRequest.YieldQueue(new ReadRequest());
        /// <summary>
        /// 数据读取请求等待访问锁
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle readWait;
        /// <summary>
        /// 数据写入请求链表
        /// </summary>
        private WriteRequest.YieldQueue writeRequests = new WriteRequest.YieldQueue(new WriteRequest());
        /// <summary>
        /// 数据写入请求等待访问锁
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle writeWait;
        /// <summary>
        /// 等待写入请求集合
        /// </summary>
        private WriteRequest waitFlushRequest;
        /// <summary>
        /// 等待写入字节数
        /// </summary>
        private int waitFlushFileSize;
        /// <summary>
        /// 最大等待写入字节数
        /// </summary>
        private int maxFlushSize;
        /// <summary>
        /// 文件流缓冲区大小
        /// </summary>
        private SubBuffer.Size bufferSize;
        /// <summary>
        /// 磁盘块编号
        /// </summary>
        int IBlock.Index { get { return index; } }
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 文件块
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="index">磁盘块编号</param>
        /// <param name="maxFlushSize">最大等待写入字节数</param>
        /// <param name="cacheSize">文件块读取缓冲区大小</param>
        /// <param name="bufferSize">文件流缓冲区大小</param>
        /// <param name="log">日志处理</param>
        /// <param name="isDataCache">是否建立数据缓存</param>
        public File(string fileName, int index, int maxFlushSize = 1 << 20, int cacheSize = 100 << 20, SubBuffer.Size bufferSize = SubBuffer.Size.Kilobyte4, ILog log = null, bool isDataCache = true)
            : base(index, cacheSize, log, isDataCache)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName is null");
            this.bufferSize = bufferSize;
            this.maxFlushSize = waitFlushFileSize = Math.Max(maxFlushSize, 0);
            FileInfo file = new FileInfo(fileName);
            this.fileName = file.FullName;
            if (file.Extension.toLower() != ExtensionName) this.fileName += ExtensionName;
            if (file.Exists)
            {
                writeStream = new FileStream(this.fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, (int)bufferSize, FileOptions.None);
                fileLength = writeStream.Length;
            }
            else
            {
                writeStream = new FileStream(this.fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, (int)bufferSize, FileOptions.None);
                fileLength = 0;
            }
            if (fileLength == 0)
            {
                writeWait.Set(1);
                writeRequests.Push(new WriteRequest(index));
            }
            else
            {
                byte[] data = new byte[sizeof(int) * 3];
                if (writeStream.Read(data, 0, sizeof(int) * 3) != sizeof(int) * 3)
                {
                    Dispose();
                    throw new FileLoadException(this.fileName);
                }
                fixed (byte* dataFixed = data)
                {
                    if (((*(int*)dataFixed ^ Pub.PuzzleValue) | (*(int*)(dataFixed + sizeof(int)) ^ (int)AutoCSer.IO.FileHead.DiskBlockFile) | (*(int*)(dataFixed + sizeof(int) * 2) ^ index)) != 0)
                    {
                        Dispose();
                        throw new FileLoadException(this.fileName);
                    }
                }
                writeWait.Set(0);
            }
            writeStream.Seek(0, SeekOrigin.End);
            readWait.Set(0);
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(read);
            AutoCSer.Threading.ThreadPool.Tiny.FastStart(write);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                if (readRequests.IsEmpty) readWait.Set();
                if (writeRequests.IsEmpty) writeWait.Set();
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="size">字节数量</param>
        /// <param name="onRead">获取数据回调委托</param>
        void IBlock.Read(long index, int size, Func<AutoCSer.Net.TcpServer.ReturnValue<ClientBuffer>, bool> onRead)
        {
            if (index + size + sizeof(int) <= fileLength)
            {
                if (readRequests.IsPushHead(new ReadRequest { Index = index, Size = size, OnRead = onRead })) readWait.Set();
            }
            else onRead(new ClientBuffer { State = MemberState.IndexError });
        }
        /// <summary>
        /// 读取数据线程
        /// </summary>
        private void read()
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, (int)bufferSize, FileOptions.RandomAccess))
            {
                do
                {
                    if (isDisposed == 0) readWait.Wait();
                    ReadRequest head = readRequests.GetClear();
                    if (head == null) return;
                    do
                    {
                        try
                        {
                            do
                            {
                                head = head.Read(this, fileStream);
                            }
                            while (head != null);
                            break;
                        }
                        catch (Exception error)
                        {
                            log.Add(LogType.Error, error);
                        }
                        head = head.Error();
                    }
                    while (head != null);
                }
                while (true);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="onWrite">添加数据回调委托</param>
        void IBlock.Append(ref AppendBuffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>, bool> onWrite)
        {
            if (isDisposed == 0)
            {
                ulong index = buffer.Index & Server.MaxIndex;
                if (index == 0 || (long)index + (buffer.Buffer.Length + sizeof(int)) > fileLength)
                {
                    if (writeRequests.IsPushHead(new WriteRequest(ref buffer, onWrite))) writeWait.Set();
                    if (isDisposed != 0)
                    {
                        for (WriteRequest head = writeRequests.GetClear(); head != null; head = head.Error()) ;
                    }
                }
                else
                {
                    ReadRequest readRequest = new ReadRequest { Index = (long)index, Size = buffer.Buffer.Length };
                    readRequest.WriteRequest = new FileWriteRequest(ref buffer, onWrite, this);
                    if (readRequests.IsPushHead(readRequest)) readWait.Set();
                    if (isDisposed != 0)
                    {
                        for (ReadRequest head = readRequests.GetClear(); head != null; head = head.Error()) ;
                        for (WriteRequest head = writeRequests.GetClear(); head != null; head = head.Error()) ;
                    }
                }
            }
            else onWrite(0);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="writeRequest"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(FileWriteRequest writeRequest)
        {
            if (isDisposed == 0)
            {
                if (writeRequests.IsPushHead(writeRequest)) writeWait.Set();
                if (isDisposed != 0)
                {
                    for (WriteRequest head = writeRequests.GetClear(); head != null; head = head.Error()) ;
                }
            }
            else writeRequest.Error();
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong Write(ref SubBuffer.PoolBufferFull Buffer, int size)
        {
            ulong index = index64 + (ulong)fileLength;
            writeStream.Write(Buffer.Buffer, Buffer.StartIndex, size += sizeof(int));
            fileLength += size;
            waitFlushFileSize -= size;
            return index;
        }
        /// <summary>
        /// 写入数据线程
        /// </summary>
        private void write()
        {
            WriteRequest next;
            using (writeStream)
            {
                do
                {
                    if (isDisposed == 0) writeWait.Wait();
                    WriteRequest head = writeRequests.GetClear();
                    if (head == null) return;
                    do
                    {
                        try
                        {
                            do
                            {
                                next = head.Write(this);
                                head.LinkNext = waitFlushRequest;
                                waitFlushRequest = head;
                                head = next;
                                if (waitFlushFileSize < 0)
                                {
                                    while (waitFlushRequest != null) waitFlushRequest = waitFlushRequest.OnFlush();
                                    waitFlushFileSize = maxFlushSize;
                                }
                            }
                            while (head != null);
                            break;
                        }
                        catch (Exception error)
                        {
                            log.Add(LogType.Error, error);
                        }
                        if (head == null) break;
                        head = head.Error();
                    }
                    while (head != null);
                    if (waitFlushRequest != null)
                    {
                        try
                        {
                            writeStream.Flush(true);
                            do
                            {
                                waitFlushRequest = waitFlushRequest.OnFlush();
                            }
                            while (waitFlushRequest != null);
                            waitFlushFileSize = maxFlushSize;
                        }
                        catch (Exception error)
                        {
                            log.Add(LogType.Error, error);
                            while (waitFlushRequest != null) waitFlushRequest = waitFlushRequest.Error();
                        }
                    }
                }
                while (true);
            }
        }
    }
}
