using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 文件流写入器
    /// </summary>
    internal unsafe sealed class FileStreamWriter : AutoCSer.Threading.DoubleLink<FileStreamWriter>
    {
        /// <summary>
        /// 文件头部
        /// </summary>
        internal const int FileHeader = 'a' + ('m' << 8) + ('c' << 16) + (' ' << 24);
        /// <summary>
        /// 文件头部长度
        /// </summary>
        private const int fileHeaderSize = sizeof(int) * 2 + sizeof(ulong);
        /// <summary>
        /// 文件流写入器链表
        /// </summary>
        internal static YieldLink Writers;

        /// <summary>
        /// 缓存管理
        /// </summary>
        private readonly CacheManager cache;
        /// <summary>
        /// 缓存服务配置
        /// </summary>
        internal readonly MasterServerConfig Config;
        /// <summary>
        /// 缓冲区池
        /// </summary>
        private readonly SubBuffer.Pool bufferPool;
        /// <summary>
        /// 文件版本
        /// </summary>
        internal ulong Version = 1;
        /// <summary>
        /// 文件名称
        /// </summary>
        internal string FileName;
        /// <summary>
        /// 文件流
        /// </summary>
        private FileStream fileStream;
        /// <summary>
        /// 大数据缓冲区
        /// </summary>
        private byte[] bigBuffer = NullValue<byte>.Array;
        /// <summary>
        /// 操作数据链表
        /// </summary>
        internal Buffer.YieldQueue BufferLink = new Buffer.YieldQueue(new Buffer());
        /// <summary>
        /// 文件读取器集合
        /// </summary>
        internal FileReader.YieldLink Readers;
        /// <summary>
        /// 当前文件长度
        /// </summary>
        internal long FileLength;
        /// <summary>
        /// 物理文件刷新秒数
        /// </summary>
        private int fileFlushSeconds;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        internal volatile int IsDisposed;
        /// <summary>
        /// 是否需要尝试读取
        /// </summary>
        private bool isTryRead;
        /// <summary>
        /// 是否切换文件
        /// </summary>
        private readonly bool isSwitchFile;
        /// <summary>
        /// 文件流写入器
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="config"></param>
        internal FileStreamWriter(CacheManager cache, MasterServerConfig config)
        {
            this.cache = cache;
            Config = config;
            bufferPool = SubBuffer.Pool.GetPool(config.BufferSize);
            FileInfo file = new FileInfo(config.GetFileName);
            FileName = file.FullName;
            IsDisposed = 1;
            FileMode createMode = FileMode.CreateNew;
            FileBuffers buffer = default(FileBuffers);
            try
            {
                if (file.Exists)
                {
                    if (file.Length == 0) createMode = FileMode.Create;
                    else
                    {
                        fileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, bufferPool.Size, FileOptions.None);
                        bufferPool.Get(ref buffer.Buffer);
                        if (fileStream.Read(buffer.Buffer.Buffer, buffer.Buffer.StartIndex, fileHeaderSize) == fileHeaderSize)
                        {
                            fixed (byte* bufferFixed = buffer.Buffer.Buffer)
                            {
                                byte* bufferStart = bufferFixed + buffer.Buffer.StartIndex;
                                if (*(int*)bufferStart == FileHeader)
                                {
                                    Version = *(ulong*)(bufferStart + sizeof(int) * 2);
                                    FileInfo switchFile = new FileInfo(config.GetSwitchFileName);
                                    if (switchFile.Exists)
                                    {
                                        FileStream switchFileStream = new FileStream(switchFile.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, bufferPool.Size, FileOptions.None);
                                        if (switchFileStream.Read(buffer.Buffer.Buffer, buffer.Buffer.StartIndex, fileHeaderSize) == fileHeaderSize && *(int*)bufferStart == FileHeader
                                            && *(ulong*)(bufferStart + sizeof(int) * 2) > Version)
                                        {
                                            fileStream.Dispose();
                                            Version = *(ulong*)(bufferStart + sizeof(int) * 2);
                                            fileStream = switchFileStream;
                                            FileName = switchFile.FullName;
                                            isSwitchFile = true;
                                        }
                                    }
                                    if (Version > 0)
                                    {
                                        LoadData loadData = new LoadData { Buffer = BufferLink.Head };
                                        int count = fileStream.Read(buffer.Buffer.Buffer, buffer.Buffer.StartIndex, buffer.Buffer.Length), index = 0, compressionDataSize;
                                        long nextLength = (FileLength = fileStream.Length) - fileHeaderSize - count;
                                        do
                                        {
                                            while (count >= sizeof(int) * 2)
                                            {
                                                byte* read = bufferStart + index;
                                                if ((compressionDataSize = *(int*)read) < 0)
                                                {
                                                    if (count >= (compressionDataSize = -compressionDataSize) + sizeof(int) * 2)
                                                    {
                                                        buffer.CompressionBuffer.StartIndex = *(int*)(read + sizeof(int));
                                                        AutoCSer.IO.Compression.DeflateDeCompressor.Get(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + (index += sizeof(int) * 2), compressionDataSize, ref buffer.CompressionBuffer);
                                                        if (buffer.CompressionBuffer.Buffer != null)
                                                        {
                                                            fixed (byte* dataFixed = buffer.CompressionBuffer.Buffer)
                                                            {
                                                                loadData.Set(buffer.CompressionBuffer.Buffer, buffer.CompressionBuffer.StartIndex, *(int*)(read + sizeof(int)), dataFixed);
                                                                if (!cache.Load(ref loadData)) throw new InvalidDataException();
                                                            }
                                                            index += compressionDataSize;
                                                            count -= compressionDataSize + sizeof(int) * 2;
                                                            buffer.CompressionBuffer.Free();
                                                        }
                                                        else throw new InvalidDataException();
                                                    }
                                                    else if (count + nextLength >= compressionDataSize + sizeof(int) * 2)
                                                    {
                                                        if (compressionDataSize + sizeof(int) * 2 <= buffer.Buffer.StartIndex) break;
                                                        if (bigBuffer.Length < compressionDataSize) bigBuffer = new byte[Math.Max(compressionDataSize, bigBuffer.Length << 1)];
                                                        System.Buffer.BlockCopy(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + (index + sizeof(int) * 2), bigBuffer, 0, count -= sizeof(int) * 2);
                                                        do
                                                        {
                                                            index = fileStream.Read(bigBuffer, count, compressionDataSize - count);
                                                            nextLength -= index;
                                                            count += index;
                                                        }
                                                        while (count != compressionDataSize);
                                                        buffer.CompressionBuffer.StartIndex = *(int*)(read + sizeof(int));
                                                        AutoCSer.IO.Compression.DeflateDeCompressor.Get(bigBuffer, 0, compressionDataSize, ref buffer.CompressionBuffer);
                                                        if (buffer.CompressionBuffer.Buffer != null)
                                                        {
                                                            fixed (byte* dataFixed = buffer.CompressionBuffer.Buffer)
                                                            {
                                                                loadData.Set(buffer.CompressionBuffer.Buffer, buffer.CompressionBuffer.StartIndex, *(int*)(read + sizeof(int)), dataFixed);
                                                                if (!cache.Load(ref loadData)) throw new InvalidDataException();
                                                            }
                                                            index = count = 0;
                                                            buffer.CompressionBuffer.Free();
                                                        }
                                                        else throw new InvalidDataException();
                                                    }
                                                    else
                                                    {
                                                        endError(ref buffer.Buffer, nextLength, index, count);
                                                        return;
                                                    }
                                                }
                                                else if (count >= compressionDataSize + sizeof(int))
                                                {
                                                    loadData.Set(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + (index += sizeof(int)), compressionDataSize, bufferFixed);
                                                    if (!cache.Load(ref loadData)) throw new InvalidDataException();
                                                    index += compressionDataSize;
                                                    count -= compressionDataSize + sizeof(int);
                                                }
                                                else if (count + nextLength >= compressionDataSize + sizeof(int))
                                                {
                                                    if (compressionDataSize + sizeof(int) <= buffer.Buffer.StartIndex) break;
                                                    if (bigBuffer.Length < compressionDataSize) bigBuffer = new byte[Math.Max(compressionDataSize, bigBuffer.Length << 1)];
                                                    System.Buffer.BlockCopy(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + (index + sizeof(int)), bigBuffer, 0, count -= sizeof(int));
                                                    do
                                                    {
                                                        index = fileStream.Read(bigBuffer, count, compressionDataSize - count);
                                                        nextLength -= index;
                                                        count += index;
                                                    }
                                                    while (count != compressionDataSize);
                                                    fixed (byte* dataFixed = bigBuffer)
                                                    {
                                                        loadData.Set(bigBuffer, 0, compressionDataSize, dataFixed);
                                                        if (!cache.Load(ref loadData)) throw new InvalidDataException();
                                                    }
                                                    index = count = 0;
                                                }
                                                else
                                                {
                                                    endError(ref buffer.Buffer, nextLength, index, count);
                                                    return;
                                                }
                                            }
                                            if (nextLength == 0)
                                            {
                                                if (count == 0)
                                                {
                                                    fileStream.Seek(0, SeekOrigin.End);
                                                    IsDisposed = 0;
                                                }
                                                else endError(ref buffer.Buffer, nextLength, index, count);
                                                return;
                                            }
                                            if (count != 0) System.Buffer.BlockCopy(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + index, buffer.Buffer.Buffer, buffer.Buffer.StartIndex, count);
                                            index = fileStream.Read(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + count, buffer.Buffer.Length - count);
                                            nextLength -= index;
                                            count += index;
                                            index = 0;
                                        }
                                        while (true);
                                    }
                                }
                            }
                        }
                        if (!config.IsMoveBakUnknownFile) throw new InvalidDataException();
                        fileStream.Dispose();
                        AutoCSer.IO.File.MoveBak(FileName);
                    }
                }
                fileStream = new FileStream(FileName, createMode, FileAccess.Write, FileShare.Read, bufferPool.Size, FileOptions.None);
                create(ref buffer.Buffer, Version);
            }
            finally
            {
                buffer.Free();
                BufferLink.Head.Array.SetNull();
                if (IsDisposed == 0)
                {
                    fileFlushSeconds = config.FileFlushSeconds;
                    Writers.PushNotNull(this);
                    OnTime.Set(Date.NowTime.OnTimeFlag.CacheFile);
                }
                else if (fileStream != null) fileStream.Dispose();
            }
        }
        /// <summary>
        /// 文件流写入器
        /// </summary>
        /// <param name="cacheFile"></param>
        internal FileStreamWriter(FileStreamWriter cacheFile)
        {
            cache = cacheFile.cache;
            Config = cacheFile.Config;
            bufferPool = SubBuffer.Pool.GetPool(Config.BufferSize);
        }
        /// <summary>
        /// 创建文件流
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="version"></param>
        private void create(ref SubBuffer.PoolBufferFull buffer, ulong version)
        {
            if (buffer.Buffer == null) bufferPool.Get(ref buffer);
            fixed (byte* bufferFixed = buffer.Buffer)
            {
                byte* bufferStart = bufferFixed + buffer.StartIndex;
                *(int*)bufferStart = FileHeader;
                *(ulong*)(bufferStart + sizeof(int) * 2) = Version;
            }
            fileStream.Write(buffer.Buffer, buffer.StartIndex, fileHeaderSize);
            FileLength = fileHeaderSize;
            IsDisposed = 0;
        }
        /// <summary>
        /// 文件尾部错误备份
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="nextLength"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        private void endError(ref SubBuffer.PoolBufferFull buffer, long nextLength, int index, int count)
        {
            if (!Config.IsIgnoreFileEndError) throw new InvalidDataException();
            FileLength -= nextLength + count;
            using (FileStream endFileStream = new FileStream(FileName + ".ERROR" + Version.toString() + "_" + FileLength.toString(), FileMode.Create, FileAccess.Write, FileShare.None, bufferPool.Size, FileOptions.None))
            {
                endFileStream.Write(buffer.Buffer, buffer.StartIndex + index, count);
                while (nextLength != 0)
                {
                    count = fileStream.Read(buffer.Buffer, buffer.StartIndex, buffer.Length);
                    endFileStream.Write(buffer.Buffer, buffer.StartIndex, count);
                    nextLength -= count;
                }
            }
            fileStream.Seek(sizeof(int) * 2, SeekOrigin.Begin);
            fixed (byte* bufferFixed = buffer.Buffer) *(ulong*)(bufferFixed + (buffer.StartIndex + sizeof(int) * 2)) = ++Version;
            fileStream.Write(buffer.Buffer, buffer.StartIndex + sizeof(int) * 2, sizeof(ulong));
            fileStream.SetLength(FileLength);
            fileStream.Flush(true);
            fileStream.Seek(0, SeekOrigin.End);
            IsDisposed = 0;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void Close()
        {
            if (Interlocked.CompareExchange(ref IsDisposed, 1, 0) == 0)
            {
                Writers.PopNotNull(this);
                if (System.Threading.Interlocked.Exchange(ref fileFlushSeconds, 0) != 0) free();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void free()
        {
            write();
            fileStream.Dispose();
            for (FileReader reader = Readers.End; reader != null; reader = reader.DoubleLinkPrevious) reader.Free();
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns></returns>
        private bool write()
        {
            Buffer data = BufferLink.GetClear();
            if (data != null)
            {
                FileBuffers buffer = default(FileBuffers);
                bufferPool.Get(ref buffer.Buffer);
                long writeLength = 0;
                try
                {
                    fixed (byte* bufferFixed = buffer.Buffer.Buffer)
                    {
                        byte* bufferStart = bufferFixed + buffer.Buffer.StartIndex;
                        int index = sizeof(int), bigBufferSize;
                        START:
                        if (data.Array.Length + index <= buffer.Buffer.Length)
                        {
                            if ((data = data.Copy(ref buffer.Buffer, ref index) ?? BufferLink.GetClear()) != null) goto START;
                        }
                        else if (index == sizeof(int))
                        {
                            if (bigBuffer.Length < (bigBufferSize = data.Array.Length + sizeof(int))) bigBuffer = new byte[Math.Max(bigBufferSize, bigBuffer.Length << 1)];
                            fixed (byte* bigBufferFixed = bigBuffer)
                            {
                                data = data.Copy(bigBuffer);
                                if (bigBufferSize > Config.MinCompressSize)
                                {
                                    if (AutoCSer.IO.Compression.DeflateCompressor.Get(bigBuffer, sizeof(int), bigBufferSize - sizeof(int), ref buffer.CompressionBuffer, ref buffer.CompressionData, sizeof(int) * 2, sizeof(int) * 2))
                                    {
                                        writeCompression(ref buffer.CompressionData, bigBufferSize - sizeof(int));
                                        buffer.CompressionBuffer.TryFree();
                                        writeLength += buffer.CompressionData.Length;
                                        if (data == null && (data = BufferLink.GetClear()) == null) return true;
                                        index = sizeof(int);
                                        goto START;
                                    }
                                    buffer.CompressionBuffer.TryFree();
                                }
                                *(int*)bigBufferFixed = bigBufferSize - sizeof(int);
                            }
                            fileStream.Write(bigBuffer, 0, bigBufferSize);
                            writeLength += bigBufferSize;
                            if (data == null && (data = BufferLink.GetClear()) == null) return true;
                            index = sizeof(int);
                            goto START;
                        }
                        if (index > Config.MinCompressSize)
                        {
                            if (AutoCSer.IO.Compression.DeflateCompressor.Get(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + sizeof(int), index - sizeof(int), ref buffer.CompressionBuffer, ref buffer.CompressionData, sizeof(int) * 2, sizeof(int) * 2))
                            {
                                writeCompression(ref buffer.CompressionData, index - sizeof(int));
                                buffer.CompressionBuffer.TryFree();
                                writeLength += buffer.CompressionData.Length;
                                if (data == null && (data = BufferLink.GetClear()) == null) return true;
                                index = sizeof(int);
                                goto START;
                            }
                            buffer.CompressionBuffer.TryFree();
                        }
                        *(int*)bufferStart = index - sizeof(int);
                        fileStream.Write(buffer.Buffer.Buffer, buffer.Buffer.StartIndex, index);
                        writeLength += index;
                        if (data == null && (data = BufferLink.GetClear()) == null) return true;
                        index = sizeof(int);
                        goto START;
                    }
                }
                catch (Exception error)
                {
                    writeLength = 0;
                    AutoCSer.Log.Pub.Log.Add(Log.LogType.Fatal, error);
                    fileStream.Dispose();
                    return false;
                }
                finally
                {
                    buffer.Free();
                    if (writeLength != 0)
                    {
                        fileStream.Flush(true);
                        FileLength += writeLength;
                        for (FileReader reader = Readers.End; reader != null; reader = reader.DoubleLinkPrevious) isTryRead |= reader.TryRead();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 写入压缩数据
        /// </summary>
        /// <param name="compressionData"></param>
        /// <param name="dataSize"></param>
        private void writeCompression(ref SubArray<byte> compressionData, int dataSize)
        {
            int compressionDataSize = -compressionData.Length;
            compressionData.MoveStart(-(sizeof(int) * 2));
            fixed (byte* dataFixed = compressionData.Array)
            {
                byte* write = dataFixed + compressionData.Start;
                *(int*)write = compressionDataSize;
                *(int*)(write + sizeof(int)) = dataSize;
            }
            fileStream.Write(compressionData.Array, compressionData.Start, compressionData.Length);
        }

        /// <summary>
        /// 定时器触发文件流写入
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimer()
        {
            if (!BufferLink.IsEmpty && System.Threading.Interlocked.Decrement(ref fileFlushSeconds) == 0)
            {
                write();
                onWrite();
            }
            else if (isTryRead)
            {
                isTryRead = false;
                for (FileReader reader = Readers.End; reader != null; reader = reader.DoubleLinkPrevious) isTryRead |= reader.TryRead(FileLength);
            }
        }
        /// <summary>
        /// 文件写入后恢复文件刷新秒数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onWrite()
        {
            System.Threading.Interlocked.Exchange(ref fileFlushSeconds, Config.FileFlushSeconds);
            if (IsDisposed != 0 && System.Threading.Interlocked.Exchange(ref fileFlushSeconds, 0) != 0) free();
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        internal void Write()
        {
            while (System.Threading.Interlocked.Exchange(ref fileFlushSeconds, 0) <= 0) System.Threading.Thread.Sleep(1);
            if (!BufferLink.IsEmpty) write();
            onWrite();
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="version"></param>
        /// <param name="index"></param>
        /// <param name="onRead"></param>
        /// <returns></returns>
        internal bool Read(ulong version, long index, Func<AutoCSer.Net.TcpServer.ReturnValue<ReadFileParameter>, bool> onRead)
        {
            if (Version == version)
            {
                try
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new FileReader(this, index, onRead).Start);
                    return true;
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 缓存快照
        /// </summary>
        private Snapshot.Cache snapshot;
        /// <summary>
        /// 重建文件流
        /// </summary>
        /// <param name="cacheFile"></param>
        /// <returns></returns>
        internal ReturnType Start(FileStreamWriter cacheFile)
        {
            IsDisposed = 1;
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
            try
            {
                snapshot = new Snapshot.Cache(cache, true);
                FileInfo file = new FileInfo(cacheFile.isSwitchFile ? Config.GetFileName : Config.GetSwitchFileName);
                FileName = file.FullName;
                if (file.Exists) AutoCSer.IO.File.MoveBak(FileName);
                fileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, bufferPool.Size, FileOptions.None);
                create(ref buffer, 0);
                return ReturnType.Success;
            }
            catch (Exception error)
            {
                cache.TcpServer.AddLog(error);
            }
            finally
            {
                buffer.Free();
                if (IsDisposed == 0)
                {
                    Interlocked.Exchange(ref fileFlushSeconds, Config.FileFlushSeconds);
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(writeCache);
                }
                else
                {
                    cache.NextGetter();
                    if (fileStream != null) fileStream.Dispose();
                }
            }
            return ReturnType.SnapshotFileStartError;
        }
        /// <summary>
        /// 写缓存数据
        /// </summary>
        private void writeCache()
        {
            bool isError = true;
            FileBuffers buffer = default(FileBuffers);
            try
            {
                bufferPool.Get(ref buffer.Buffer);
                fixed (byte* bufferFixed = buffer.Buffer.Buffer)
                {
                    byte* bufferStart = bufferFixed + buffer.Buffer.StartIndex;
                    int index = sizeof(int), snapshotSize;
                    long writeLength = 0;
                    while ((snapshotSize = snapshot.NextSize()) != 0)
                    {
                        CHECK:
                        if (snapshotSize + index <= buffer.Buffer.Length)
                        {
                            snapshot.CopyTo(bufferStart + index);
                            index += snapshotSize;
                        }
                        else if (index == sizeof(int))
                        {
                            if (bigBuffer.Length < (index = snapshotSize + sizeof(int))) bigBuffer = new byte[Math.Max(index, bigBuffer.Length << 1)];
                            fixed (byte* bigBufferFixed = bigBuffer)
                            {
                                snapshot.CopyTo(bigBufferFixed + sizeof(int));
                                if (snapshotSize >= Config.MinCompressSize)
                                {
                                    if (AutoCSer.IO.Compression.DeflateCompressor.Get(bigBuffer, sizeof(int), snapshotSize, ref buffer.CompressionBuffer, ref buffer.CompressionData, sizeof(int) * 2, sizeof(int) * 2))
                                    {
                                        writeCompression(ref buffer.CompressionData, snapshotSize);
                                        buffer.CompressionBuffer.TryFree();
                                        writeLength += buffer.CompressionData.Length;
                                        index = sizeof(int);
                                        continue;
                                    }
                                    buffer.CompressionBuffer.TryFree();
                                }
                                *(int*)bigBufferFixed = snapshotSize;
                            }
                            fileStream.Write(bigBuffer, 0, index);
                            writeLength += index;
                            index = sizeof(int);
                        }
                        else
                        {
                            if (index > Config.MinCompressSize)
                            {
                                if (AutoCSer.IO.Compression.DeflateCompressor.Get(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + sizeof(int), index - sizeof(int), ref buffer.CompressionBuffer, ref buffer.CompressionData, sizeof(int) * 2, sizeof(int) * 2))
                                {
                                    writeCompression(ref buffer.CompressionData, index - sizeof(int));
                                    buffer.CompressionBuffer.TryFree();
                                    writeLength += buffer.CompressionData.Length;
                                    index = sizeof(int);
                                    goto CHECK;
                                }
                                buffer.CompressionBuffer.TryFree();
                            }
                            *(int*)bufferStart = index - sizeof(int);
                            fileStream.Write(buffer.Buffer.Buffer, buffer.Buffer.StartIndex, index);
                            writeLength += index;
                            index = sizeof(int);
                            goto CHECK;
                        }
                    }
                    if (index != sizeof(int))
                    {
                        if (index > Config.MinCompressSize)
                        {
                            if (AutoCSer.IO.Compression.DeflateCompressor.Get(buffer.Buffer.Buffer, buffer.Buffer.StartIndex + sizeof(int), index - sizeof(int), ref buffer.CompressionBuffer, ref buffer.CompressionData, sizeof(int) * 2, sizeof(int) * 2))
                            {
                                writeCompression(ref buffer.CompressionData, index - sizeof(int));
                                buffer.CompressionBuffer.TryFree();
                                writeLength += buffer.CompressionData.Length;
                                goto FLUSH;
                            }
                            buffer.CompressionBuffer.TryFree();
                        }
                        *(int*)bufferStart = index - sizeof(int);
                        fileStream.Write(buffer.Buffer.Buffer, buffer.Buffer.StartIndex, index);
                        writeLength += index;
                    }
                    FLUSH:
                    fileStream.Flush(true);
                    FileLength += writeLength;
                    isError = false;
                }
            }
            finally
            {
                buffer.Free();
                snapshot = null;
                if (isError)
                {
                    cache.TcpServer.CallQueue.Add(new ServerCall.CacheManager(cache, ServerCall.CacheManagerServerCallType.CreateNewFileStreamError));
                    if (fileStream != null) fileStream.Dispose();
                }
            }
            if (!isError) writeQueue();
        }
        /// <summary>
        /// 写缓存队列
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void writeQueue()
        {
            if (write()) cache.TcpServer.CallQueue.Add(new ServerCall.CacheManager(cache, ServerCall.CacheManagerServerCallType.CreateNewFileStreamCheckQueue));
            else cache.TcpServer.CallQueue.Add(new ServerCall.CacheManager(cache, ServerCall.CacheManagerServerCallType.CreateNewFileStreamError));
        }
        /// <summary>
        /// 重建文件流检测缓存队列
        /// </summary>
        internal void CheckQueue()
        {
            if (BufferLink.IsEmpty)
            {
                bool isError = true;
                SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
                try
                {
                    bufferPool.Get(ref buffer);
                    fixed (byte* bufferFixed = buffer.Buffer) *(ulong*)(bufferFixed + buffer.StartIndex) = Version;
                    fileStream.Seek(sizeof(int) * 2, SeekOrigin.Begin);
                    fileStream.Write(buffer.Buffer, buffer.StartIndex, sizeof(ulong));
                    fileStream.Flush();
                    fileStream.Seek(0, SeekOrigin.End);
                    cache.OnCreatedNewFileStream();
                    isError = false;
                    Writers.PushNotNull(this);
                    OnTime.Set(Date.NowTime.OnTimeFlag.CacheFile);
                }
                finally
                {
                    buffer.Free();
                    if (isError)
                    {
                        cache.CreateNewFileStreamError();
                        if (fileStream != null) fileStream.Dispose();
                    }
                }
            }
            else AutoCSer.Threading.ThreadPool.TinyBackground.Start(writeQueue);
        }
    }
}
