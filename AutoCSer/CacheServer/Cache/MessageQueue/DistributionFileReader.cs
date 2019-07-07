using System;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息分发 读文件
    /// </summary>
    internal unsafe sealed class DistributionFileReader : FileReaderBase, IDisposable
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        private static readonly DistributionConfig defaultConfig = ConfigLoader.GetUnion(typeof(DistributionConfig)).DistributionConfig ?? new DistributionConfig();

        /// <summary>
        /// 消息分发节点
        /// </summary>
        internal readonly Distributor Node;
        /// <summary>
        /// 消息分发索引
        /// </summary>
        private int readerIndex;
        /// <summary>
        /// 消息数据集合
        /// </summary>
        private DistributionMessageItem[] messages;
        /// <summary>
        /// 消息回调集合
        /// </summary>
        private DistributionMessageGetter[] getters = NullValue<DistributionMessageGetter>.Array;
        /// <summary>
        /// 当前消息回调索引
        /// </summary>
        private int getterIndex;
        /// <summary>
        /// 消息回调数量
        /// </summary>
        private int getterCount;
        /// <summary>
        /// 处理超时时钟周期
        /// </summary>
        private long timeoutTicks;
        /// <summary>
        /// 消息分发 读文件
        /// </summary>
        /// <param name="node">消息分发节点</param>
        /// <param name="config">消息分发 读取配置</param>
        internal DistributionFileReader(Distributor node, DistributionConfig config) : base(node.Writer, config ?? defaultConfig)
        {
            Node = node;
            timeoutTicks = Math.Max((config ?? defaultConfig).TimeoutSeconds, 1) * -TimeSpan.TicksPerSecond;
            readerIndex = -1;

            int isReader = 0;
            writerAppendIdentity = Writer.NewRead();
            try
            {
                loadStateFile();
                long fileIndex = 0;
                if (loadIndex(ref fileIndex))
                {
                    messages = new DistributionMessageItem[(int)Config.MemoryCacheNodeCount + FileWriter.MaxPacketCount + 1];
                    if (Identity == writerAppendIdentity) isReader = 1;
                    else
                    {
                        loadFile(fileIndex);
                        if (loadFile())
                        {
                            sendIdentity = Identity;
                            sendMessageIndex = 0;
                            isReader = 1;
                        }
                    }
                }
            }
            finally
            {
                if (isReader == 0) Dispose();
                else addReader(this);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                removeReader(this);
                dispose();
                while (messageIndex != writeMessageIndex)
                {
                    messages[messageIndex].FreeBuffer();
                    if (++messageIndex == messages.Length) messageIndex = 0;
                }
                while (getterCount != 0) getters[getterCount--].OnDispose();
                getterIndex = 0;
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
                if (getterCount != 0 && (uint)(sendIdentity - Identity) != Config.SendClientCount) sendMessage(ref messages[sendMessageIndex]);
                return;
            }
            buffer.FreeBuffer();
        }
        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="message"></param>
        private void sendMessage(ref DistributionMessageItem message)
        {
            bool isNext = false;
            do
            {
                if (getters[getterIndex].Send(sendIdentity, ref message.Data, ref isNext))
                {
                    message.OnSend();
                    nextSendIndex();
                    if (isNext && ++getterIndex == getterCount) getterIndex = 0;
                    return;
                }
                if (--getterCount == 0)
                {
                    getters[0].OnGetMessage = null;
                    return;
                }
                if (getterIndex == getterCount) getterIndex = 0;
                else getters[getterIndex] = getters[getterCount];
                getters[getterCount].OnGetMessage = null;
            }
            while (true);
        }
        ///// <summary>
        ///// 消息回调
        ///// </summary>
        ///// <param name="message"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //private void checkSendMessage(ref DistributionMessageItem message)
        //{
        //    if (message.State == DistributionMessageState.None) sendMessage(ref message);
        //}
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="getMessage"></param>
        internal void Get(QueueTaskThread.GetDistributionMessage getMessage)
        {
            try
            {
                if (isDisposed == 0)
                {
                    if (getterCount == getters.Length) getters = getters.copyNew(Math.Max(getterCount << 1, sizeof(int)));
                    getMessage.Set(ref getters[getterCount]);
                    getMessage = null;
                    ++getterCount;

                    ulong endIdentity = Math.Min(Identity + Config.SendClientCount, writeIdentity);
                    while (sendIdentity != endIdentity)
                    {
                        sendMessage(ref messages[sendMessageIndex]);
                        if (getterCount == 0) return;
                    }
                }
            }
            finally
            {
                if (getMessage != null) getMessage.OnReturn(new IdentityReturnParameter(ReturnType.MessageQueueNotFoundReader));
            }
        }
        /// <summary>
        /// 设置已完成消息标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetIdentity(ulong identity)
        {
            if (identity == Identity)
            {
                messages[messageIndex].OnMessage();
                onSetIdentity();
            }
            else if (identity > Identity && identity < sendIdentity)
            {
                int messageIndex = this.messageIndex + (int)(uint)(identity - Identity);
                messages[messageIndex >= messages.Length ? messageIndex - messages.Length : messageIndex].OnMessage();
            }
        }
        /// <summary>
        /// 设置已完成消息标识
        /// </summary>
        private void onSetIdentity()
        {
            int endMessageIndex = this.messageIndex + (int)(uint)(sendIdentity - Identity), messageIndex = this.messageIndex;
            if (endMessageIndex >= messages.Length) endMessageIndex -= messages.Length;
            do
            {
                if (++messageIndex == messages.Length)
                {
                    for (messageIndex = 0; messageIndex != endMessageIndex && messages[messageIndex].State == DistributionMessageState.Consumed; ++messageIndex) ;
                    break;
                }
            }
            while (messageIndex != endMessageIndex && messages[messageIndex].State == DistributionMessageState.Consumed);
            ulong identity = Identity + (messageIndex > this.messageIndex ? (uint)(messageIndex - this.messageIndex) : (uint)(messageIndex + messages.Length - this.messageIndex));
            saveState(identity);

            if (isDisposed == 0)
            {
                this.messageIndex = messageIndex;
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
                if (isDisposed == 0 && getterCount != 0)
                {
                    ulong endIdentity = Math.Min(Identity + Config.SendClientCount, writeIdentity);
                    while (sendIdentity != endIdentity)
                    {
                        sendMessage(ref messages[sendMessageIndex]);
                        if (getterCount == 0) return;
                    }
                }
            }
        }
        /// <summary>
        /// 设置已完成消息标识
        /// </summary>
        /// <param name="identitys">确认已完成消息标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetIdentity(ulong[] identitys)
        {
            foreach (ulong identity in identitys)
            {
                if (identity - Identity < sendIdentity - Identity)
                {
                    int messageIndex = this.messageIndex + (int)(uint)(identity - Identity);
                    messages[messageIndex >= messages.Length ? messageIndex - messages.Length : messageIndex].OnMessage();
                }
            }
            if (messages[this.messageIndex].State == DistributionMessageState.Consumed && sendIdentity != Identity) onSetIdentity();
        }
        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        private void onTimer()
        {
            int count = (int)(uint)(sendIdentity - Identity);
            if (count != 0)
            {
                ulong identity = Identity;
                DateTime time = Date.NowTime.Now.AddTicks(timeoutTicks);
                int endMessageIndex = this.messageIndex + count, messageIndex = this.messageIndex;
                if (endMessageIndex >= messages.Length) endMessageIndex -= messages.Length;
                do
                {
                    if (++messageIndex == messages.Length) messageIndex = 0;
                    if (messageIndex == endMessageIndex || messages[messageIndex].DistributionTime > time) break;
                    if (messages[messageIndex].State == DistributionMessageState.Sended) Writer.Append(new Buffer(Node, new QueueTaskThread.SetDistributionIdentity(this, identity).OnAppendFile, ref messages[messageIndex]));
                    ++identity;
                }
                while (true);
            }
        }

        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        private static readonly QueueTaskThread.DistributionOnTimer queueOnTimer = new QueueTaskThread.DistributionOnTimer();
        /// <summary>
        /// 消息分发集合
        /// </summary>
        private static DistributionFileReader[] readers = NullValue<DistributionFileReader>.Array;
        /// <summary>
        /// 消息分发数量
        /// </summary>
        private static int readerCount;
        /// <summary>
        /// 是否正在检测消息超时
        /// </summary>
        private static int isTimer;
        /// <summary>
        /// 添加消息分发
        /// </summary>
        /// <param name="reader"></param>
        private static void addReader(DistributionFileReader reader)
        {
            if (readerCount == readers.Length) readers = readers.copyNew(Math.Max(readerCount << 1, sizeof(int)));
            reader.readerIndex = readerCount;
            readers[readerCount++] = reader;
            OnTime.Set(Date.NowTime.OnTimeFlag.CacheDistributionTimeout);
        }
        /// <summary>
        /// 删除消息分发
        /// </summary>
        /// <param name="reader"></param>
        private static void removeReader(DistributionFileReader reader)
        {
            int readerIndex = reader.readerIndex;
            if (readerIndex >= 0 && Object.ReferenceEquals(readers[readerIndex], reader))
            {
                if (--readerCount != readerIndex) (readers[readerIndex] = readers[readerCount]).readerIndex = readerIndex;
                readers[readerCount] = null;
            }
        }
        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void OnTimer()
        {
            if (readerCount != 0 && Interlocked.CompareExchange(ref isTimer, 1, 0) == 0) QueueTaskThread.Thread.Default.Add(queueOnTimer);
        }
        /// <summary>
        /// 定时器触发消息处理超时检测
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void QueueOnTimer()
        {
            try
            {
                if (readerCount != 0)
                {
                    int count = readerCount;
                    foreach (DistributionFileReader reader in readers)
                    {
                        reader.onTimer();
                        if (--count == 0) return;
                    }
                }
            }
            finally { Interlocked.Exchange(ref isTimer, 0); }
        }
    }
}
