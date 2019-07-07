using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 队列数据 读文件
    /// </summary>
    internal unsafe sealed class FileReader : FileReaderBase, IDisposable
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        private static readonly ReaderConfig defaultConfig = ConfigLoader.GetUnion(typeof(ReaderConfig)).ReaderConfig ?? new ReaderConfig();

        /// <summary>
        /// 消息队列节点
        /// </summary>
        internal readonly Node Node;
        /// <summary>
        /// 读文件编号
        /// </summary>
        private readonly int readerIndex;
        /// <summary>
        /// 状态文件名称
        /// </summary>
        protected override string stateFileName
        {
            get { return Writer.FilePath + "reader" + readerIndex.toString(); }
        }
        /// <summary>
        /// 状态备份文件名称
        /// </summary>
        protected override string stateBackupFileName
        {
            get { return Writer.FilePath + "reader_backup" + readerIndex.toString(); }
        }
        /// <summary>
        /// 消息数据集合
        /// </summary>
        private MessageItem[] messages;
        /// <summary>
        /// 设置当前读取数据标识
        /// </summary>
        internal QueueTaskThread.SetIdentity SetIdentity;
        /// <summary>
        /// 当前尝试设置已完成消息标识
        /// </summary>
        private long currentSetIdentity;
        /// <summary>
        /// 获取消息回调委托
        /// </summary>
        private Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onGetMessage;
        /// <summary>
        /// 是否反序列化网络流，否则需要 Copy 数据
        /// </summary>
        private bool isGetMessageStream;
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        /// <param name="node">消息队列节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读文件编号</param>
        internal FileReader(Node node, ReaderConfig config, int readerIndex) : base(node.Writer, config ?? defaultConfig)
        {
            Node = node;
            this.readerIndex = readerIndex;
            int isReader = 0;
            writerAppendIdentity = Writer.NewRead();
            try
            {
                loadStateFile();
                long fileIndex = 0;
                if (loadIndex(ref fileIndex))
                {
                    messages = new MessageItem[(int)config.MemoryCacheNodeCount + FileWriter.MaxPacketCount + 1];
                    if (Identity == writerAppendIdentity) isReader = 1;
                    else
                    {
                        loadFile(fileIndex);
                        if (loadFile()) isReader = 1;
                    }
                }
            }
            finally
            {
                if (isReader == 0) Dispose();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                dispose();
                while (messageIndex != writeMessageIndex)
                {
                    messages[messageIndex].FreeBuffer();
                    if (++messageIndex == messages.Length) messageIndex = 0;
                }
                if (onGetMessage != null)
                {
                    onGetMessage(new ReturnParameter(ReturnType.MessageQueueNotFoundReader));
                    onGetMessage = null;
                }
            }
        }
        /// <summary>
        /// 加载数据文件
        /// </summary>
        /// <returns></returns>
        private bool loadFile()
        {
            ulong endIdentity = Math.Min(memoryEndIdentity, writerAppendIdentity);
            SubBuffer.PoolBufferFull compressionBuffer = default(SubBuffer.PoolBufferFull);
            int dataSize;
            try
            {
                fixed (byte* bufferFixed = buffer)
                {
                    do
                    {
                        switch (bufferSize)
                        {
                            case 0: break;
                            case 1: *bufferFixed = *(bufferFixed + bufferIndex); break;
                            case 2: SIZE2: *(ushort*)bufferFixed = *(ushort*)(bufferFixed + bufferIndex); break;
                            case 3: *(bufferFixed + sizeof(ushort)) = *(bufferFixed + (bufferIndex + sizeof(ushort))); goto SIZE2;
                            case 4: SIZE4: *(uint*)bufferFixed = *(uint*)(bufferFixed + bufferIndex); break;
                            case 5: *(bufferFixed + (sizeof(uint))) = *(bufferFixed + (bufferIndex + sizeof(uint))); goto SIZE4;
                            case 6: SIZE6: *(ushort*)(bufferFixed + sizeof(uint)) = *(ushort*)(bufferFixed + (bufferIndex + sizeof(uint))); goto SIZE4;
                            case 7: *(bufferFixed + (sizeof(uint) + sizeof(ushort))) = *(bufferFixed + (bufferIndex + (sizeof(uint) + sizeof(ushort)))); goto SIZE6;
                            case 8: SIZE8: *(ulong*)bufferFixed = *(ulong*)(bufferFixed + bufferIndex); break;
                            case 9: *(bufferFixed + (sizeof(ulong))) = *(bufferFixed + (bufferIndex + sizeof(ulong))); goto SIZE8;
                            case 10: SIZE10: *(ushort*)(bufferFixed + sizeof(ulong)) = *(ushort*)(bufferFixed + (bufferIndex + sizeof(ulong))); goto SIZE8;
                            case 11: *(bufferFixed + (sizeof(ulong) + sizeof(ushort))) = *(bufferFixed + (bufferIndex + (sizeof(ulong) + sizeof(ushort)))); goto SIZE10;
                            default: goto NEXTDATA;
                        }
                        bufferIndex = 0;
                        READFILE:
                        if (!readFile()) return false;
                        NEXTDATA:
                        int compressionDataSize = *(int*)(bufferFixed + bufferIndex);
                        if (compressionDataSize < 0)
                        {
                            dataSize = *(int*)(bufferFixed + (bufferIndex + sizeof(int)));
                            //dataCount = *(int*)(bufferFixed + (bufferIndex + sizeof(int) * 2));
                            bufferIndex += FileWriter.PacketHeaderSize + sizeof(int);
                            bufferSize -= FileWriter.PacketHeaderSize + sizeof(int);
                            compressionDataSize = -compressionDataSize;
                        }
                        else
                        {
                            //dataCount = *(int*)(bufferFixed + (bufferIndex + sizeof(int) * 1));
                            bufferIndex += FileWriter.PacketHeaderSize;
                            bufferSize -= FileWriter.PacketHeaderSize;
                            dataSize = compressionDataSize;
                        }
                        if (bufferSize < compressionDataSize)
                        {
                            if (compressionDataSize <= buffer.Length)
                            {
                                System.Buffer.BlockCopy(buffer, bufferIndex, buffer, 0, bufferSize);
                                if ((bufferSize += dataFileStream.Read(buffer, bufferSize, buffer.Length - bufferSize)) < compressionDataSize) return false;
                                bufferIndex = 0;
                            }
                            else
                            {
                                #region 大数据块解析
                                if (bigBuffer.Length < compressionDataSize) bigBuffer = new byte[Math.Max(compressionDataSize, bigBuffer.Length << 1)];
                                System.Buffer.BlockCopy(buffer, bufferIndex, bigBuffer, 0, bufferSize);
                                if (dataFileStream.Read(bigBuffer, bufferSize, bufferIndex = compressionDataSize - bufferSize) != bufferIndex) return false;
                                if (compressionDataSize == dataSize)
                                {
                                    bufferIndex = 0;
                                    fixed (byte* bigBufferFixed = bigBuffer)
                                    {
                                        do
                                        {
                                            if (dataFileIdentity == writeIdentity)
                                            {
                                                bufferIndex += messages[writeMessageIndex].DeSerializeBuffer(bigBufferFixed + bufferIndex, bigBuffer, bufferIndex);
                                                nextWriteIndex();
                                            }
                                            else bufferIndex += messages[writeMessageIndex].Data.DeSerializeBuffer(bigBufferFixed + bufferIndex, bigBuffer, bufferIndex);
                                            ++dataFileIdentity;
                                        }
                                        while (bufferIndex < dataSize);
                                    }
                                    if (bufferIndex != dataSize) return false;
                                }
                                else
                                {
                                    compressionBuffer.StartIndex = dataSize;
                                    AutoCSer.IO.Compression.DeflateDeCompressor.Get(bigBuffer, 0, compressionDataSize, ref compressionBuffer);
                                    if (!load(ref compressionBuffer, dataSize)) return false;
                                }
                                bufferIndex = bufferSize = 0;
                                if (dataFileIdentity < endIdentity) goto READFILE;
                                break;
                                #endregion
                            }
                        }
                        if (compressionDataSize == dataSize)
                        {
                            dataSize += bufferIndex;
                            do
                            {
                                if (dataFileIdentity == writeIdentity)
                                {
                                    bufferIndex += messages[writeMessageIndex].DeSerializeBuffer(bufferFixed + bufferIndex, buffer, bufferIndex);
                                    nextWriteIndex();
                                }
                                else bufferIndex += messages[writeMessageIndex].Data.DeSerializeBuffer(bufferFixed + bufferIndex, buffer, bufferIndex);
                                ++dataFileIdentity;
                            }
                            while (bufferIndex < dataSize);
                            if (bufferIndex != dataSize) return false;

                        }
                        else
                        {
                            compressionBuffer.StartIndex = dataSize;
                            AutoCSer.IO.Compression.DeflateDeCompressor.Get(buffer, bufferIndex, compressionDataSize, ref compressionBuffer);
                            if (!load(ref compressionBuffer, dataSize)) return false;
                            bufferIndex += compressionDataSize;
                        }
                        bufferSize -= compressionDataSize;
                    }
                    while (dataFileIdentity < endIdentity);
                    if (writeIdentity >= writerAppendIdentity)
                    {
                        dataFileStream.Dispose();
                        dataFileStream = null;
                    }
                }
            }
            finally { compressionBuffer.TryFree(); }
            return true;
        }
        /// <summary>
        /// 解析压缩数据
        /// </summary>
        /// <param name="compressionBuffer"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        private bool load(ref SubBuffer.PoolBufferFull compressionBuffer, int dataSize)
        {
            if (compressionBuffer.Buffer == null) return false;
            int bufferIndex = compressionBuffer.StartIndex, bufferSize = bufferIndex + dataSize;
            fixed (byte* bigBufferFixed = compressionBuffer.Buffer)
            {
                do
                {
                    if (dataFileIdentity == writeIdentity)
                    {
                        bufferIndex += messages[writeMessageIndex].DeSerializeBuffer(bigBufferFixed + bufferIndex, compressionBuffer.Buffer, bufferIndex);
                        nextWriteIndex();
                    }
                    else bufferIndex += messages[writeMessageIndex].Data.DeSerializeBuffer(bigBufferFixed + bufferIndex, compressionBuffer.Buffer, bufferIndex);
                    ++dataFileIdentity;
                }
                while (bufferIndex < bufferSize);
            }
            compressionBuffer.Free();
            return bufferIndex == bufferSize;
        }
        /// <summary>
        /// 移动写入消息索引位置
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void nextWriteIndex()
        {
            ++writeIdentity;
            if (++writeMessageIndex == messages.Length) writeMessageIndex = 0;
        }
        /// <summary>
        /// 移动发送消息数据标识
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void nextSendIndex()
        {
            ++sendIdentity;
            if (++sendMessageIndex == messages.Length) sendMessageIndex = 0;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer"></param>
        internal void Append(Buffer buffer)
        {
            if (buffer.Identity == writerAppendIdentity) ++writerAppendIdentity;
            if (buffer.Identity == writeIdentity && writeIdentity < memoryEndIdentity && isDisposed == 0)
            {
                messages[writeMessageIndex].Set(buffer);
                nextWriteIndex();
                if (onGetMessage != null && (uint)(sendIdentity - Identity) != Config.SendClientCount)
                {
                    if (onGetMessage(new ReturnParameter(messages[sendMessageIndex].Data, isGetMessageStream))) nextSendIndex();
                    else onGetMessage = null;
                }
                return;
            }
            buffer.FreeBuffer();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="bufferCount"></param>
        internal void Append(ref BufferCount bufferCount)
        {
            if (bufferCount.Identity == writerAppendIdentity) ++writerAppendIdentity;
            if (bufferCount.Identity == writeIdentity && writeIdentity < memoryEndIdentity && isDisposed == 0)
            {
                messages[writeMessageIndex].Set(ref bufferCount);
                nextWriteIndex();
                if (onGetMessage != null && (uint)(sendIdentity - Identity) != Config.SendClientCount)
                {
                    if (onGetMessage(new ReturnParameter(messages[sendMessageIndex].Data, isGetMessageStream))) nextSendIndex();
                    else onGetMessage = null;
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="getMessage"></param>
        internal void Get(QueueTaskThread.GetMessage getMessage)
        {
            ReturnType returnType = ReturnType.MessageQueueNotFoundReader;
            try
            {
                if (isDisposed == 0)
                {
                    if (getMessage.Identity == Identity)
                    {
                        if (onGetMessage == null || !onGetMessage(new ReturnParameter(ReturnType.Success)))
                        {
                            onGetMessage = getMessage.OnReturn;
                            isGetMessageStream = getMessage.IsReturnStream;
                            ulong endIdentity = Math.Min(Identity + Config.SendClientCount, writeIdentity);
                            sendIdentity = Identity;
                            sendMessageIndex = messageIndex;
                            getMessage = null;
                            ReturnParameter returnParameter = default(ReturnParameter);
                            while (sendIdentity != endIdentity)
                            {
                                returnParameter.Set(messages[sendMessageIndex].Data, isGetMessageStream);
                                if (onGetMessage(returnParameter)) nextSendIndex();
                                else
                                {
                                    onGetMessage = null;
                                    return;
                                }
                            }
                            return;
                        }
                        else returnType = ReturnType.MessageQueueGetMessageExists;
                    }
                    else returnType = ReturnType.MessageQueueReaderIdentityError;
                }
            }
            finally
            {
                if (getMessage != null) getMessage.OnReturn(new ReturnParameter(returnType));
            }
        }
        /// <summary>
        /// 设置已完成消息标识
        /// </summary>
        internal void SaveIdentity()
        {
            ulong identity = (ulong)Interlocked.Exchange(ref currentSetIdentity, 0L);
            if (identity > Identity && identity <= sendIdentity)
            {
                saveState(identity);

                if (isDisposed == 0)
                {
                    int endMessageIndex = messageIndex + (int)(uint)(identity - Identity);
                    if(endMessageIndex >= messages.Length) endMessageIndex -= messages.Length;
                    do
                    {
                        messages[messageIndex].OnMessage();
                        if (++messageIndex == messages.Length)
                        {
                            for (messageIndex = 0; messageIndex != endMessageIndex; messages[messageIndex++].OnMessage()) ;
                            break;
                        }
                    }
                    while (messageIndex != endMessageIndex);
                    Identity = identity;
                    memoryEndIdentity = identity + Config.MemoryCacheNodeCount;
                    if (writeIdentity < memoryEndIdentity && writeIdentity < writerAppendIdentity)
                    {
                        byte isReader = 0, isLoadFile = 0;
                        try
                        {
                            if (dataFileStream != null)
                            {
                                if (dataFileIdentity == writeIdentity)
                                {
                                    isLoadFile = 1;
                                    if (loadFile()) isReader = 1;
                                }
                                else
                                {
                                    dataFileStream.Dispose();
                                    dataFileStream = null;
                                }
                            }
                            if (isLoadFile == 0)
                            {
                                long fileIndex = 0;
                                if (Writer.GetIndex(writeIdentity, ref dataFileIdentity, ref fileIndex))
                                {
                                    loadFile(fileIndex);
                                    if (loadFile()) isReader = 1;
                                }
                            }
                        }
                        finally
                        {
                            if (isReader == 0) Dispose();
                        }
                    }
                    if (isDisposed == 0 && onGetMessage != null)
                    {
                        ReturnParameter returnParameter = default(ReturnParameter);
                        ulong endIdentity = Math.Min(Identity + Config.SendClientCount, writeIdentity);
                        while (sendIdentity != endIdentity)
                        {
                            returnParameter.Set(messages[sendMessageIndex].Data, isGetMessageStream);
                            if (onGetMessage(returnParameter)) nextSendIndex();
                            else
                            {
                                onGetMessage = null;
                                return;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 尝试设置已完成消息标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        internal void TrySetIdentity(ulong identity)
        {
            while (identity > Identity)
            {
                ulong oldIdentity = (ulong)Interlocked.Exchange(ref currentSetIdentity, (long)identity);
                if (oldIdentity == 0)
                {
                    if(isDisposed == 0)  Writer.OnStart(Interlocked.Exchange(ref SetIdentity, null) ?? new QueueTaskThread.SetIdentity(this));
                    return;
                }
                if (oldIdentity <= identity) return;
                identity = oldIdentity;
            }
        }
    }
}
