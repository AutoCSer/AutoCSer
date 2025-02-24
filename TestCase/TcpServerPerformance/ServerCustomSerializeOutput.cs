﻿using System;
using System.Threading;
using AutoCSer.Memory;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// 自定义序列化计算回调输出
    /// </summary>
    internal unsafe sealed class ServerCustomSerializeOutput
    {
        /// <summary>
        /// 计算回调
        /// </summary>
        private AutoCSer.Net.TcpServer.ServerCallback<ServerCustomSerialize> onCustomSerialize;
        /// <summary>
        /// 第一个输出缓冲区
        /// </summary>
        private ServerCustomSerializeBuffer headBuffer;
        /// <summary>
        /// 最后一个输出缓冲区
        /// </summary>
        private ServerCustomSerializeBuffer endBuffer;
        /// <summary>
        /// 当前输出缓冲区
        /// </summary>
        private ServerCustomSerializeBuffer currentBuffer;
        /// <summary>
        /// 空闲缓冲区
        /// </summary>
        private ServerCustomSerializeBuffer freeBuffer;
        /// <summary>
        /// 是否正在输出
        /// </summary>
        private volatile int isOutput;
        /// <summary>
        /// 自定义序列化计算回调输出缓冲区访问锁
        /// </summary>
        private int bufferLock;
        /// <summary>
        /// 空闲缓冲区访问锁
        /// </summary>
        private int freeBufferLock;
        /// <summary>
        /// 自定义序列化计算回调输出
        /// </summary>
        /// <param name="onCustomSerialize">计算回调</param>
        internal ServerCustomSerializeOutput(AutoCSer.Net.TcpServer.ServerCallback<ServerCustomSerialize> onCustomSerialize)
        {
            this.onCustomSerialize = onCustomSerialize;
        }
        /// <summary>
        /// 添加计算任务
        /// </summary>
        /// <param name="value"></param>
        internal void Append(ref ClientCustomSerialize value)
        {
            if (Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
            {
                append(ref value);
                onCustomSerialize.Callback(new ServerCustomSerialize { Output = this });
            }
            else
            {
                append(ref value);
                if (Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
                {
                    if (headBuffer == null && currentBuffer == null) Interlocked.Exchange(ref isOutput, 0);
                    else onCustomSerialize.Callback(new ServerCustomSerialize { Output = this });
                }
            }
        }
        /// <summary>
        /// 添加计算任务
        /// </summary>
        /// <param name="value"></param>
        private void append(ref ClientCustomSerialize value)
        {
            int length = value.Buffer.Count;
            ServerCustomSerializeBuffer buffer = null;
            while (System.Threading.Interlocked.CompareExchange(ref bufferLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
            if (currentBuffer != null)
            {
                if (currentBuffer.Size + length <= ServerCustomSerializeBuffer.BufferSize) buffer = currentBuffer;
                else
                {
                    if (endBuffer == null) headBuffer = currentBuffer;
                    else endBuffer.LinkNext = currentBuffer;
                    endBuffer = currentBuffer;
                }
                currentBuffer = null;
            }
            System.Threading.Interlocked.Exchange(ref bufferLock, 0);
            if (buffer == null)
            {
                while (System.Threading.Interlocked.CompareExchange(ref freeBufferLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
                if (freeBuffer == null)
                {
                    System.Threading.Interlocked.Exchange(ref freeBufferLock, 0);
                    buffer = new ServerCustomSerializeBuffer();
                }
                else
                {
                    buffer = freeBuffer;
                    freeBuffer = freeBuffer.LinkNext;
                    System.Threading.Interlocked.Exchange(ref freeBufferLock, 0);
                    buffer.LinkNext = null;
                }
            }
            buffer.Copy(ref value.Buffer);
            while (System.Threading.Interlocked.CompareExchange(ref bufferLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
            currentBuffer = buffer;
            System.Threading.Interlocked.Exchange(ref bufferLock, 0);
        }
        /// <summary>
        /// 序列化操作
        /// </summary>
        /// <param name="serializer"></param>
        internal void Serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            byte* start = stream.Current, write = start + sizeof(int);
            int freeCount = (stream.FreeSize - sizeof(int) * 2) / (sizeof(int) * 3), outputCount = 0;
            ServerCustomSerializeBuffer buffer;
            do
            {
                while (System.Threading.Interlocked.CompareExchange(ref bufferLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
                if (headBuffer == null)
                {
                    buffer = currentBuffer;
                    currentBuffer = null;
                    System.Threading.Interlocked.Exchange(ref bufferLock, 0);
                    if (buffer == null)
                    {
                        Interlocked.Exchange(ref isOutput, 0);
                        if ((headBuffer == null && currentBuffer == null) || Interlocked.CompareExchange(ref isOutput, 1, 0) != 0)
                        {
                            stream.MoveSize(*(int*)start = (outputCount * (sizeof(int) * 3)) + sizeof(int));
                            return;
                        }
                        continue;
                    }
                }
                else
                {
                    buffer = headBuffer;
                    if ((headBuffer = headBuffer.LinkNext) == null)
                    {
                        headBuffer = endBuffer = null;
                        System.Threading.Interlocked.Exchange(ref bufferLock, 0);
                    }
                    else
                    {
                        System.Threading.Interlocked.Exchange(ref bufferLock, 0);
                        buffer.LinkNext = null;
                    }
                }
                int bufferCount = buffer.Size / (sizeof(int) * 2);
                if (bufferCount > freeCount)
                {
                    onCustomSerialize.Callback(new ServerCustomSerialize { Output = this });
                    write = buffer.Serialize(write, freeCount);
                    while (System.Threading.Interlocked.CompareExchange(ref bufferLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
                    if (headBuffer == null) endBuffer = buffer;
                    else buffer.LinkNext = headBuffer;
                    headBuffer = buffer;
                    System.Threading.Interlocked.Exchange(ref bufferLock, 0);
                    stream.MoveSize(*(int*)start = ((outputCount + freeCount) * (sizeof(int) * 3)) + sizeof(int));
                    return;
                }
                write = buffer.Serialize(write);
                while (System.Threading.Interlocked.CompareExchange(ref freeBufferLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
                buffer.LinkNext = freeBuffer;
                freeBuffer = buffer;
                System.Threading.Interlocked.Exchange(ref freeBufferLock, 0);
                outputCount += bufferCount;
                if ((freeCount -= bufferCount) == 0)
                {
                    stream.MoveSize(*(int*)start = (outputCount * (sizeof(int) * 3)) + sizeof(int));
                    if (headBuffer == null && currentBuffer == null)
                    {
                        Interlocked.Exchange(ref isOutput, 0);
                        if ((headBuffer == null && currentBuffer == null) || Interlocked.CompareExchange(ref isOutput, 1, 0) != 0) return;
                    }
                    onCustomSerialize.Callback(new ServerCustomSerialize { Output = this });
                    return;
                }
            }
            while (true);
        }
    }
}
