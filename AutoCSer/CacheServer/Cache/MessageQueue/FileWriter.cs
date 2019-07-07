using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 队列数据 写文件
    /// </summary>
    internal unsafe sealed class FileWriter : IDisposable
    {
        /// <summary>
        /// 单次打包最大数据量
        /// </summary>
        internal const int MaxPacketCount = (128 << 10) / sizeof(uint) - 1;
        /// <summary>
        /// 数据包头字节大小
        /// </summary>
        internal const int PacketHeaderSize = sizeof(int) * 2;
        /// <summary>
        /// 状态缓存区大小
        /// </summary>
        private const int stateBufferSize = sizeof(ulong) * 4;
        /// <summary>
        /// 重建索引缓冲区大小
        /// </summary>
        private const int createIndexBufferSize = 4 << 10;
        /// <summary>
        /// 每个文件保存数据数量 二进制位数
        /// </summary>
        private const int dataCountPerFileBits = 20;
        /// <summary>
        /// 每个文件保存数据数量
        /// </summary>
        internal const uint DataCountPerFile = 1U << dataCountPerFileBits;
        /// <summary>
        /// 状态文件最大字节数据
        /// </summary>
        internal const int MaxStateFileSize = 1 << 20;

        /// <summary>
        /// 消息队列节点
        /// </summary>
        internal readonly Node Node;
        /// <summary>
        /// 缓存主服务配置
        /// </summary>
        internal readonly MasterServerConfig Config;
        /// <summary>
        /// 缓冲区池
        /// </summary>
        private readonly SubBuffer.Pool bufferPool;
        /// <summary>
        /// 释放资源
        /// </summary>
        private readonly Action disposeHandle;
        /// <summary>
        /// 文件路径
        /// </summary>
        internal readonly string FilePath;
        /// <summary>
        /// 等待初始化的读取操作节点集合访问锁
        /// </summary>
        private readonly object onStartQueueLock = new object();
        /// <summary>
        /// 等待初始化的读取操作节点集合
        /// </summary>
        private QueueTaskThread.Node.Queue onStartQueue;
        /// <summary>
        /// 写入数据
        /// </summary>
        private Action writeHandle;
        /// <summary>
        /// 状态文件
        /// </summary>
        private FileStream stateFileStream;
        /// <summary>
        /// 状态文件名称
        /// </summary>
        private string stateFileName
        {
            get { return FilePath + "state"; }
        }
        /// <summary>
        /// 状态备份文件名称
        /// </summary>
        private string stateBackupFileName
        {
            get { return FilePath + "state_backup"; }
        }
        /// <summary>
        /// 大数据缓冲区
        /// </summary>
        private byte[] bigBuffer = NullValue<byte>.Array;
        /// <summary>
        /// 数据文件
        /// </summary>
        private FileStream dataFileStream;
        /// <summary>
        /// 获取数据文件名称
        /// </summary>
        /// <returns></returns>
        internal string dataFileName
        {
            get
            {
                return GetFileName(identity);
            }
        }
        /// <summary>
        /// 当前数据文件长度
        /// </summary>
        private long dataFileLength;
        /// <summary>
        /// 当前写入数据标识
        /// </summary>
        private ulong identity;
        /// <summary>
        /// 当前数据文件起始数据标识
        /// </summary>
        private ulong baseIdentity
        {
            get { return identity & (ulong.MaxValue - (DataCountPerFile - 1)); }
        }
        /// <summary>
        /// 读取队列数量
        /// </summary>
        internal volatile int ReadCount;
        /// <summary>
        /// 数据缓冲区链表
        /// </summary>
        private Buffer.YieldQueue bufferQueue = new Threading.Link<Buffer>.YieldQueue(new Buffer());
        /// <summary>
        /// 当前数据文件数据包索引信息
        /// </summary>
        private LeftArray<PacketIndex> indexs;
        /// <summary>
        /// 已保存状态的数据包索引信息
        /// </summary>
        internal PacketIndex StatePacketIndex;
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private volatile bool isStartQueue;
        /// <summary>
        /// 是否正在写入操作
        /// </summary>
        private int isWrite;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 是否需要释放资源
        /// </summary>
        private volatile int isNeedDispose;
        /// <summary>
        /// 是否正在或者已经释放资源
        /// </summary>
        internal bool IsDisposed
        {
            get { return (isDisposed | isNeedDispose) != 0; }
        }
        /// <summary>
        /// 队列数据 写文件
        /// </summary>
        /// <param name="node">消息队列节点</param>
        internal FileWriter(Node node)
        {
            Node = node;
            Config = node.Cache.MasterConfig;
            bufferPool = SubBuffer.Pool.GetPool(Config.BufferSize);
            DirectoryInfo directory = new DirectoryInfo(node.FilePath);
            if (!directory.Exists) directory.Create();
            FilePath = directory.fullName();
            disposeHandle = Dispose;
            onStartQueue = new QueueTaskThread.Node.Queue(new QueueTaskThread.Null(this));
            isWrite = 1;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isNeedDispose = 0;
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                AutoCSer.DomainUnload.Unloader.Remove(disposeHandle, DomainUnload.Type.Action, false);
                if (!isStartQueue) onStart();
                if (dataFileStream != null)
                {
                    dataFileStream.Dispose();
                    dataFileStream = null;
                }
                if (stateFileStream != null)
                {
                    stateFileStream.Dispose();
                    stateFileStream = null;
                }
                int count = indexs.Length;
                if (count > 1)
                {
                    PacketIndex[] indexArray = indexs.Array;
                    while (count != 0)
                    {
                        long fileIndex = indexArray[--count].FileIndex;
                        if (fileIndex == StatePacketIndex.FileIndex)
                        {
                            if (indexArray[count].Identity == StatePacketIndex.Identity && count != 0)
                            {
                                indexs.Length = count + 1;
                                SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
                                bufferPool.Get(ref buffer);
                                try
                                {
                                    writeIndex(ref buffer);
                                }
                                finally { buffer.Free(); }
                            }
                            break;
                        }
                        if (fileIndex < StatePacketIndex.FileIndex) break;
                    }
                }
            }
        }
        /// <summary>
        /// 尝试释放资源
        /// </summary>
        internal void TryDispose()
        {
            isNeedDispose = 1;
            if (Interlocked.CompareExchange(ref isWrite, 1, 0) == 0) Dispose();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        internal void Start()
        {
            int isDisposed = 1;
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
            try
            {
                if (checkStateFile())
                {
                    byte[] stateData = new byte[stateBufferSize];
                    stateFileStream = new FileStream(stateFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, stateBufferSize, FileOptions.None);
                    stateFileStream.Seek(-stateBufferSize, SeekOrigin.End);
                    stateFileStream.Read(stateData, 0, stateBufferSize);
                    fixed (byte* stateDataFixed = stateData)
                    {
                        identity = *(ulong*)stateDataFixed;
                        dataFileLength = *(long*)(stateDataFixed + sizeof(ulong));
                    }
                    if (((uint)identity & (DataCountPerFile - 1)) == 0) dataFileLength = 0;
                    if (dataFileLength == 0)
                    {
                        FileInfo dataFileInfo = new FileInfo(dataFileName);
                        if (dataFileInfo.Exists)
                        {
                            if (dataFileInfo.Length == 0) dataFileInfo.Delete();
                            else AutoCSer.IO.File.MoveBak(dataFileInfo.FullName);
                        }
                        dataFileStream = new FileStream(dataFileInfo.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, bufferPool.Size, FileOptions.None);
                    }
                    else
                    {
                        FileInfo dataFileInfo = new FileInfo(dataFileName);
                        if (!dataFileInfo.Exists)
                        {
                            Node.Cache.TcpServer.Log.Add(Log.LogType.Error, "没有找到消息队列数据文件 " + dataFileInfo.FullName);
                            return;
                        }
                        if (dataFileInfo.Length < dataFileLength)
                        {
                            Node.Cache.TcpServer.Log.Add(Log.LogType.Error, "消息队列数据文件 " + dataFileInfo.FullName + " 大小错误 " + dataFileInfo.Length.toString() + " < " + dataFileLength.toString());
                            return;
                        }
                        dataFileStream = new FileStream(dataFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.Read, bufferPool.Size, FileOptions.None);
                        if (dataFileStream.Length > dataFileLength)
                        {
                            dataFileStream.SetLength(dataFileLength);
                            dataFileStream.Flush(true);
                        }
                        dataFileStream.Seek(0, SeekOrigin.End);

                        FileInfo indexFileInfo = new FileInfo(getIndexFileName(identity));
                        bufferPool.Get(ref buffer);
                        fixed (byte* bufferFixed = buffer.Buffer)
                        {
                            byte* bufferStart = bufferFixed + buffer.StartIndex, end = bufferStart + buffer.Length;
                            ulong baseIdentity = this.baseIdentity;
                            if (indexFileInfo.Exists && indexFileInfo.Length >= sizeof(int) * 2)
                            {
                                #region 初始化数据文件数据包索引信息
                                int count = (int)(indexFileInfo.Length >> 3), index = 0;
                                long fileIndex = 0;
                                indexs = new LeftArray<PacketIndex>(count);
                                PacketIndex[] indexArray = indexs.Array;
                                using (FileStream indexFileStream = new FileStream(indexFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferPool.Size, FileOptions.None))
                                {
                                    do
                                    {
                                        indexFileStream.Read(buffer.Buffer, buffer.StartIndex, buffer.Length);
                                        byte* read = bufferStart;
                                        do
                                        {
                                            indexArray[index].Set(baseIdentity + *(uint*)read, fileIndex += *(int*)(read + sizeof(int)));
                                            if (++index == count) break;
                                        }
                                        while ((read += sizeof(int) * 2) != end);
                                    }
                                    while (index != count);
                                }
                                while (index != 0)
                                {
                                    if ((fileIndex = indexArray[--index].FileIndex) == dataFileLength)
                                    {
                                        if (indexArray[index].Identity == identity) indexs.Length = index + 1;
                                        break;
                                    }
                                    if (fileIndex < dataFileLength) break;
                                }
                                #endregion
                            }
                            if (indexs.Length == 0)
                            {
                                #region 重建数据文件数据包索引信息
                                if (indexs.Array == null) indexs = new LeftArray<PacketIndex>(1 << 10);
                                indexs.Array[0].Set(baseIdentity);
                                indexs.Length = 1;
                                int bufferIndex = 0, readBufferSize = Math.Min(buffer.Length, createIndexBufferSize), bufferEndIndex, dataSize;
                                long nextFileSize = dataFileLength, fileIndex = 0;
                                using (FileStream fileStream = new FileStream(dataFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, readBufferSize, FileOptions.None))
                                {
                                    readBufferSize -= sizeof(int);
                                    do
                                    {
                                        bufferEndIndex = fileStream.Read(buffer.Buffer, buffer.StartIndex + bufferIndex, readBufferSize - bufferIndex);
                                        nextFileSize -= bufferEndIndex;
                                        bufferEndIndex += bufferIndex - sizeof(int) * 3;
                                        bufferIndex = 0;
                                        do
                                        {
                                            byte* read = bufferStart + bufferIndex;
                                            dataSize = *(int*)read;
                                            if (dataSize < 0)
                                            {
                                                baseIdentity += *(uint*)(read + sizeof(int) * 2);
                                                bufferIndex += -dataSize + PacketHeaderSize + sizeof(int);
                                            }
                                            else
                                            {
                                                baseIdentity += *(uint*)(read + sizeof(int));
                                                bufferIndex += dataSize + PacketHeaderSize;
                                            }
                                            indexs.PrepLength(1);
                                            indexs.Array[indexs.Length].Set(baseIdentity, fileIndex + bufferIndex);
                                            ++indexs.Length;
                                        }
                                        while (bufferIndex <= bufferEndIndex);
                                        fileIndex += bufferIndex;
                                        switch (dataSize = bufferIndex - bufferEndIndex)
                                        {
                                            case 1:
                                            case 2:
                                            case 3:
                                                *(ulong*)bufferStart = *(ulong*)(bufferStart + bufferIndex);
                                                *(uint*)(bufferStart + sizeof(ulong)) = *(uint*)(bufferStart + (bufferIndex + sizeof(ulong)));
                                                bufferIndex = sizeof(int) * 3 - dataSize;
                                                break;
                                            case 4:
                                            case 5:
                                            case 6:
                                            case 7:
                                                *(ulong*)bufferStart = *(ulong*)(bufferStart + bufferIndex);
                                                bufferIndex = sizeof(int) * 3 - dataSize;
                                                break;
                                            case 8:
                                            case 9:
                                            case 10:
                                            case 11:
                                                *(uint*)bufferStart = *(uint*)(bufferStart + bufferIndex);
                                                bufferIndex = sizeof(int) * 3 - dataSize;
                                                break;
                                            case 12: bufferIndex = 0; break;
                                            default:
                                                fileStream.Seek(dataSize -= sizeof(int) * 3, SeekOrigin.Current);
                                                nextFileSize -= dataSize;
                                                bufferIndex = 0;
                                                break;
                                        }
                                    }
                                    while (nextFileSize > 0);
                                }
                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    stateFileStream = new FileStream(stateFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, stateBufferSize, FileOptions.None);
                    FileInfo dataFileInfo = new FileInfo(dataFileName);
                    if (dataFileInfo.Exists)
                    {
                        if (dataFileInfo.Length == 0) dataFileInfo.Delete();
                        else AutoCSer.IO.File.MoveBak(dataFileInfo.FullName);
                    }
                    dataFileStream = new FileStream(dataFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, bufferPool.Size, FileOptions.None);
                }
                if (indexs.Array == null) indexs = new LeftArray<PacketIndex>(1 << 10);
                if (indexs.Length == 0)
                {
                    indexs.Array[0].Set(baseIdentity);
                    indexs.Length = 1;
                }
                writeHandle = write;
                StatePacketIndex.Set(identity, dataFileLength);
                AutoCSer.DomainUnload.Unloader.Add(disposeHandle, DomainUnload.Type.Action);
                isDisposed = 0;
            }
            finally
            {
                buffer.TryFree();
                if (isDisposed == 0)
                {
                    Interlocked.Exchange(ref isWrite, 0);
                    onStart();
                    if (!bufferQueue.IsEmpty && Interlocked.CompareExchange(ref isWrite, 1, 0) == 0) write();
                }
                else
                {
                    indexs.Length = 0;
                    Dispose();
                }
            }
        }
        /// <summary>
        /// 等待初始化的读取操作
        /// </summary>
        private void onStart()
        {
            QueueTaskThread.Node end, head;
            Monitor.Enter(onStartQueueLock);
            head = onStartQueue.GetClear(out end);
            if (head != null) QueueTaskThread.Thread.Default.Add(head, end);
            isStartQueue = true;
            Monitor.Exit(onStartQueueLock);
        }
        /// <summary>
        /// 等待初始化的读取操作
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnStart(QueueTaskThread.Node node)
        {
            if (isStartQueue) QueueTaskThread.Thread.Default.Add(node);
            else onStart(node);
        }
        /// <summary>
        /// 等待初始化的读取操作
        /// </summary>
        /// <param name="node"></param>
        private void onStart(QueueTaskThread.Node node)
        {
            Monitor.Enter(onStartQueueLock);
            try
            {
                if (isStartQueue) QueueTaskThread.Thread.Default.Add(node);
                else onStartQueue.Push(node);
            }
            finally { Monitor.Exit(onStartQueueLock); }
        }
        /// <summary>
        /// 检测状态文件是否存在
        /// </summary>
        /// <returns></returns>
        private bool checkStateFile()
        {
            FileInfo stateFileInfo = new FileInfo(stateFileName);
            if (stateFileInfo.Exists)
            {
                if (stateFileInfo.Length >= stateBufferSize) return true;
                stateFileInfo.Delete();
            }
            FileInfo stateBackupFileInfo = new FileInfo(stateBackupFileName);
            if (stateBackupFileInfo.Exists && stateBackupFileInfo.Length >= stateBufferSize)
            {
                stateBackupFileInfo.MoveTo(stateFileName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取数据文件名称
        /// </summary>
        /// <param name="identity">数据标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string GetFileName(ulong identity)
        {
            return FilePath + (identity >> dataCountPerFileBits).toHex();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(Buffer buffer)
        {
            if (isDisposed == 0)
            {
                if (bufferQueue.IsPushHead(buffer))
                {
                    if (Interlocked.CompareExchange(ref isWrite, 1, 0) == 0) AutoCSer.Threading.ThreadPool.Tiny.Start(writeHandle);
                }
            }
            else buffer.Error(ReturnType.MessageQueueDisposed);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        private unsafe void write()
        {
            FileBuffers buffer = default(FileBuffers);
            int bigBufferSize;
            bool isCopyBuffer, isNeedDispose = false;
            try
            {
                bufferPool.Get(ref buffer.Buffer);
                DateTime callbackTime = Date.NowTime.Now;
                fixed (byte* bufferFixed = buffer.Buffer.Buffer)
                {
                    byte* bufferStart = bufferFixed + buffer.Buffer.StartIndex;
                    GETQUEUE:
                    //Thread.Sleep(0);
                    Buffer head = bufferQueue.GetClear();
                    if (head != null)
                    {
                        int index = PacketHeaderSize, dataCount = 0;
                        Buffer data = head;
                        do
                        {
                            data.Identity = identity;
                            int dataSize = data.Data.GetSerializeBufferSize(out isCopyBuffer);
                            CHECKSIZE:
                            if (dataSize + index <= buffer.Buffer.Length)
                            {
                                data.Data.SerializeBuffer(bufferStart + index);
                                if (isCopyBuffer) data.Data.SerializeBuffer(ref buffer.Buffer, index);
                                index += dataSize;
                                ++dataCount;
                                ++identity;
                            }
                            else if (dataCount != 0)
                            {
                                *(int*)bufferStart = index - PacketHeaderSize;
                                *(int*)(bufferStart + sizeof(int)) = dataCount;
                                write(ref buffer, bufferStart);
                                index = PacketHeaderSize;
                                dataCount = 0;
                                goto CHECKSIZE;
                            }
                            else
                            {
                                #region 写入大数据
                                ++identity;
                                if (bigBuffer.Length < (bigBufferSize = dataSize + PacketHeaderSize)) bigBuffer = new byte[Math.Max(bigBufferSize, bigBuffer.Length << 1)];
                                fixed (byte* bigBufferFixed = bigBuffer)
                                {
                                    *(int*)bigBufferFixed = dataSize;
                                    *(int*)(bigBufferFixed + sizeof(int)) = 1;
                                    data.Data.SerializeBuffer(bigBufferFixed + PacketHeaderSize, bigBuffer);
                                    if (dataSize >= Config.MinCompressSize)
                                    {
                                        if (AutoCSer.IO.Compression.DeflateCompressor.Get(bigBuffer, PacketHeaderSize, dataSize, ref buffer.CompressionBuffer, ref buffer.CompressionData, PacketHeaderSize + sizeof(int), PacketHeaderSize + sizeof(int)))
                                        {
                                            writeCompression(ref buffer.CompressionData, bigBufferFixed);
                                            buffer.CompressionBuffer.TryFree();
                                            goto CHECKIDENTITY;
                                        }
                                        buffer.CompressionBuffer.TryFree();
                                    }
                                }
                                dataFileStream.Write(bigBuffer, 0, bigBufferSize);
                                dataFileLength += bigBufferSize;
                                setIndex();
                                #endregion
                            }
                            CHECKIDENTITY:
                            if (((uint)identity & (DataCountPerFile - 1)) == 0)
                            {
                                #region 切换数据文件
                                if (dataCount != 0)
                                {
                                    *(int*)bufferStart = index - PacketHeaderSize;
                                    *(int*)(bufferStart + sizeof(int)) = dataCount;
                                    write(ref buffer, bufferStart);
                                    index = PacketHeaderSize;
                                    dataCount = 0;
                                }
                                dataFileStream.Flush(true);
                                dataFileStream.Dispose();
                                dataFileStream = null;
                                writeState(ref buffer.Buffer);
                                writeIndex(ref buffer.Buffer);

                                FileInfo dataFileInfo = new FileInfo(dataFileName);
                                if (dataFileInfo.Exists)
                                {
                                    if (dataFileInfo.Length == 0) dataFileInfo.Delete();
                                    else AutoCSer.IO.File.MoveBak(dataFileInfo.FullName);
                                }
                                dataFileStream = new FileStream(dataFileInfo.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, bufferPool.Size, FileOptions.None);
                                dataFileLength = 0;
                                #endregion
                            }
                            else if (dataCount == MaxPacketCount)
                            {
                                *(int*)bufferStart = index - PacketHeaderSize;
                                *(int*)(bufferStart + sizeof(int)) = MaxPacketCount;
                                write(ref buffer, bufferStart);
                                index = PacketHeaderSize;
                                dataCount = 0;
                            }

                            if (data.LinkNext == null)
                            {
                                if (bufferQueue.IsEmpty || callbackTime != Date.NowTime.Now) break;
                                data.LinkNext = bufferQueue.GetClear();
                            }
                            data = data.LinkNext;
                        }
                        while (true);

                        if (dataCount != 0)
                        {
                            *(int*)bufferStart = index - PacketHeaderSize;
                            *(int*)(bufferStart + sizeof(int)) = dataCount;
                            write(ref buffer, bufferStart);
                        }
                        dataFileStream.Flush(true);
                        writeState(ref buffer.Buffer);
                        if (ReadCount == 0)
                        {
                            do
                            {
                                head = head.Callback();
                            }
                            while (head != null);
                        }
                        else QueueTaskThread.Thread.Default.Add(new QueueTaskThread.Append(head));
                        callbackTime = Date.NowTime.Now;
                    }
                    if (this.isNeedDispose != 0)
                    {
                        isNeedDispose = true;
                        return;
                    }
                    if (!bufferQueue.IsEmpty) goto GETQUEUE;
                    Interlocked.Exchange(ref isWrite, 0);
                    if (!bufferQueue.IsEmpty && Interlocked.CompareExchange(ref isWrite, 1, 0) == 0) goto GETQUEUE;
                }
            }
            catch (Exception error)
            {
                isNeedDispose = true;
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Fatal, error);
            }
            finally
            {
                buffer.Free();
                if (isNeedDispose) Dispose();
            }
        }
        /// <summary>
        /// 写入压缩数据
        /// </summary>
        /// <param name="compressionData"></param>
        /// <param name="bufferStart"></param>
        private void writeCompression(ref SubArray<byte> compressionData, byte* bufferStart)
        {
            int compressionDataSize = -compressionData.Length;
            compressionData.MoveStart(-(PacketHeaderSize + sizeof(int)));
            fixed (byte* dataFixed = compressionData.Array)
            {
                byte* write = dataFixed + compressionData.Start;
                *(int*)write = compressionDataSize;
                *(ulong*)(write + sizeof(int)) = *(ulong*)bufferStart;
            }
            dataFileStream.Write(compressionData.Array, compressionData.Start, compressionData.Length);
            dataFileLength += compressionData.Length;
            setIndex();
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferStart"></param>
        private void write(ref FileBuffers buffer, byte* bufferStart)
        {
            if (*(int*)bufferStart >= Config.MinCompressSize)
            {
                if (AutoCSer.IO.Compression.DeflateCompressor.Get(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + PacketHeaderSize, *(int*)bufferStart, ref buffer.CompressionBuffer, ref buffer.CompressionData, PacketHeaderSize + sizeof(int), PacketHeaderSize + sizeof(int)))
                {
                    writeCompression(ref buffer.CompressionData, bufferStart);
                    buffer.CompressionBuffer.TryFree();
                    return;
                }
                buffer.CompressionBuffer.TryFree();
            }
            int size = *(int*)bufferStart + PacketHeaderSize;
            dataFileStream.Write(buffer.Buffer.Buffer, buffer.Buffer.StartIndex, size);
            dataFileLength += size;
            setIndex();
        }
        /// <summary>
        /// 写入状态
        /// </summary>
        /// <param name="buffer"></param>
        private void writeState(ref SubBuffer.PoolBufferFull buffer)
        {
            fixed (byte* stateBufferFixed = buffer.Buffer)
            {
                byte* start = stateBufferFixed + buffer.StartIndex;
                *(ulong*)start = identity;
                *(long*)(start + sizeof(ulong)) = dataFileLength;
            }
            if (stateFileStream.Length >= MaxStateFileSize)
            {
                stateFileStream.Dispose();
                stateFileStream = null;
                FileInfo bakFile = new FileInfo(stateBackupFileName);
                if (bakFile.Exists) bakFile.Delete();
                System.IO.File.Move(stateFileName, stateBackupFileName);
                stateFileStream = new FileStream(stateFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, stateBufferSize, FileOptions.None);
            }
            stateFileStream.Write(buffer.Buffer, buffer.StartIndex, stateBufferSize);
            stateFileStream.Flush(true);
            StatePacketIndex.Set(identity, dataFileLength);
        }
        /// <summary>
        /// 设置数据包索引信息
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setIndex()
        {
            indexs.PrepLength(1);
            indexs.Array[indexs.Length].Set(identity, dataFileLength);
            ++indexs.Length;
        }
        /// <summary>
        /// 获取数据包索引信息文件名称
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private string getIndexFileName(ulong identity)
        {
            return FilePath + (identity >> dataCountPerFileBits).toHex() + "_index";
        }
        /// <summary>
        /// 写入数据包索引信息
        /// </summary>
        /// <param name="buffer"></param>
        private void writeIndex(ref SubBuffer.PoolBufferFull buffer)
        {
            if (indexs.Length > 1)
            {
                FileInfo indexFileInfo = new FileInfo(getIndexFileName(identity - 1));
                if (indexFileInfo.Exists) indexFileInfo.Delete();
                using (FileStream indexFileStream = new FileStream(indexFileInfo.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, bufferPool.Size, FileOptions.None))
                {
                    int count = indexs.Length;
                    fixed (byte* bufferFixed = buffer.Buffer)
                    {
                        byte* bufferStart = bufferFixed + buffer.StartIndex, write = bufferStart, end = bufferStart + buffer.Length;
                        long fileIndex = 0;
                        foreach (PacketIndex index in indexs.Array)
                        {
                            *(uint*)write = index.FileIdentity;
                            *(int*)(write + sizeof(int)) = (int)(index.FileIndex - fileIndex);
                            write += sizeof(int) * 2;
                            fileIndex = index.FileIndex;
                            if (write == end)
                            {
                                indexFileStream.Write(buffer.Buffer, buffer.StartIndex, buffer.Buffer.Length);
                                write = bufferStart;
                            }
                            if (--count == 0) break;
                        }
                        if ((count = (int)(write - bufferStart)) != 0) indexFileStream.Write(buffer.Buffer, buffer.StartIndex, count);
                    }
                }
                indexs.Array[0].Set(baseIdentity);
                indexs.Length = 1;
            }
        }
        /// <summary>
        /// 获取数据包索引信息
        /// </summary>
        /// <param name="identity">需要定位的数据标识</param>
        /// <param name="startIdentity"></param>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        internal bool GetIndex(ulong identity, ref ulong startIdentity, ref long fileIndex)
        {
            startIdentity = identity & (ulong.MaxValue - (DataCountPerFile - 1));
            uint fileIdentity = (uint)identity & (DataCountPerFile - 1);
            if (fileIdentity == 0)
            {
                fileIndex = 0;
                return true;
            }
            ulong baseIdentity = startIdentity;
            if (this.baseIdentity == startIdentity)
            {
                int length = indexs.Length;
                if (length > 1)
                {
                    #region 二分查找当前数据文件数据包索引信息
                    int start = 0, average;
                    PacketIndex[] indexArray = indexs.Array;
                    do
                    {
                        if (identity > indexArray[average = start + ((length - start) >> 1)].Identity) start = average + 1;
                        else length = average;
                    }
                    while (start != length);
                    if (identity != indexArray[start].Identity) --start;
                    indexArray[start].Get(out startIdentity, out fileIndex);
                    if (this.baseIdentity == baseIdentity) return true;
                    #endregion
                }
            }
            FileInfo dataFileInfo = new FileInfo(GetFileName(identity));
            if (dataFileInfo.Exists && dataFileInfo.Length != 0)
            {
                SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
                SubBuffer.Pool.GetBuffer(ref buffer, createIndexBufferSize + sizeof(int));
                try
                {
                    fixed (byte* bufferFixed = buffer.Buffer)
                    {
                        byte* bufferStart = bufferFixed + buffer.StartIndex;
                        #region 从索引数据文件获取数据包索引信息
                        FileInfo indexFileInfo = new FileInfo(getIndexFileName(identity));
                        if (indexFileInfo.Exists && indexFileInfo.Length >= sizeof(int) * 2)
                        {
                            uint lastFileIdentity = 0;
                            fileIndex = 0;
                            using (FileStream indexFileStream = new FileStream(indexFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, createIndexBufferSize, FileOptions.None))
                            {
                                do
                                {
                                    int readSize = indexFileStream.Read(buffer.Buffer, buffer.StartIndex, createIndexBufferSize);
                                    if (readSize <= 0) break;
                                    byte* read = bufferStart, end = bufferStart + readSize;
                                    do
                                    {
                                        if (*(uint*)read < fileIdentity)
                                        {
                                            lastFileIdentity = *(uint*)read;
                                            fileIndex += *(int*)(read + sizeof(int));
                                        }
                                        else
                                        {
                                            if (*(uint*)read == fileIdentity)
                                            {
                                                lastFileIdentity = *(uint*)read;
                                                fileIndex += *(int*)(read + sizeof(int));
                                            }
                                            startIdentity = baseIdentity + lastFileIdentity;
                                            return true;
                                        }
                                    }
                                    while ((read += sizeof(int) * 2) < end);
                                }
                                while (true);
                            }
                            startIdentity = baseIdentity + lastFileIdentity;
                            return true;
                        }
                        #endregion

                        #region 从数据文件获取数据包索引信息
                        int bufferIndex = 0, dataSize;
                        fileIndex = 0;
                        startIdentity = baseIdentity;
                        using (FileStream fileStream = new FileStream(dataFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, createIndexBufferSize, FileOptions.None))
                        {
                            long nextFileSize = fileStream.Length;
                            do
                            {
                                int endBufferIndex = fileStream.Read(buffer.Buffer, buffer.StartIndex + bufferIndex, createIndexBufferSize - bufferIndex);
                                nextFileSize -= endBufferIndex;
                                endBufferIndex += bufferIndex - sizeof(int) * 3;
                                bufferIndex = 0;
                                do
                                {
                                    byte* read = bufferStart + bufferIndex;
                                    dataSize = *(int*)read;
                                    ulong nextIdentity = startIdentity + *(uint*)(read + (dataSize < 0 ? sizeof(int) * 2 : sizeof(int)));
                                    if (nextIdentity >= identity)
                                    {
                                        if (nextIdentity == identity)
                                        {
                                            startIdentity = nextIdentity;
                                            bufferIndex += dataSize < 0 ? (-dataSize + PacketHeaderSize + sizeof(int)) : (dataSize + PacketHeaderSize);
                                        }
                                        fileIndex += bufferIndex;
                                        return true;
                                    }
                                    startIdentity = nextIdentity;
                                    bufferIndex += dataSize < 0 ? (-dataSize + PacketHeaderSize + sizeof(int)) : (dataSize + PacketHeaderSize);
                                }
                                while (bufferIndex <= endBufferIndex);
                                fileIndex += bufferIndex;
                                switch (dataSize = bufferIndex - endBufferIndex)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                        *(ulong*)bufferStart = *(ulong*)(bufferStart + bufferIndex);
                                        *(uint*)(bufferStart + sizeof(ulong)) = *(uint*)(bufferStart + (bufferIndex + sizeof(ulong)));
                                        bufferIndex = sizeof(int) * 3 - dataSize;
                                        break;
                                    case 4:
                                    case 5:
                                    case 6:
                                    case 7:
                                        *(ulong*)bufferStart = *(ulong*)(bufferStart + bufferIndex);
                                        bufferIndex = sizeof(int) * 3 - dataSize;
                                        break;
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                        *(uint*)bufferStart = *(uint*)(bufferStart + bufferIndex);
                                        bufferIndex = sizeof(int) * 3 - dataSize;
                                        break;
                                    case 12: bufferIndex = 0; break;
                                    default:
                                        fileStream.Seek(dataSize -= sizeof(int) * 3, SeekOrigin.Current);
                                        nextFileSize -= dataSize;
                                        bufferIndex = 0;
                                        break;
                                }
                            }
                            while (nextFileSize > 0);
                        }
                        return true;
                        #endregion
                    }
                }
                finally { buffer.Free(); }
            }
            return false;
        }
        /// <summary>
        /// 增加读取队列计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong NewRead()
        {
            ++ReadCount;
            return StatePacketIndex.Identity;
        }
    }
}
