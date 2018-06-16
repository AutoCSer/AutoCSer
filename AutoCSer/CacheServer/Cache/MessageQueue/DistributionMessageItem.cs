using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 分发消息数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct DistributionMessageItem
    {
        /// <summary>
        /// 分发时间
        /// </summary>
        internal DateTime DistributionTime;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        internal AutoCSer.CacheServer.BufferCount BufferCount;
        /// <summary>
        /// 消息数据
        /// </summary>
        internal ValueData.Data Data;
        /// <summary>
        /// 消息状态
        /// </summary>
        internal DistributionMessageState State;
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="read"></param>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int DeSerializeBuffer(byte* read, byte[] buffer, int startIndex)
        {
            int size = Data.DeSerializeBuffer(read, buffer, startIndex);
            BufferCount = Data.CopyToMessageQueueBufferCount();
            return size;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(Buffer buffer)
        {
            BufferCount = buffer.BufferCount;
            Data = buffer.Data;
            buffer.BufferCount = null;
            Data.ReturnType = ReturnType.Success;
        }
        /// <summary>
        /// 消息发送完毕
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnSend()
        {
            DistributionTime = Date.NowTime.Now;
            State = DistributionMessageState.Sended;
        }
        /// <summary>
        /// 消息处理完毕
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnMessage()
        {
            FreeBuffer();
            State = DistributionMessageState.Consumed;
            Data.SetNull();
        }
        /// <summary>
        /// 超时追加到文件
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.CacheServer.BufferCount OnAppendFile()
        {
            AutoCSer.CacheServer.BufferCount bufferCount = BufferCount;
            State = DistributionMessageState.AppendFile;
            BufferCount = null;
            return bufferCount;
        }
        /// <summary>
        /// 释放数据缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeBuffer()
        {
            if (BufferCount != null)
            {
                BufferCount.Free();
                BufferCount = null;
            }
        }
    }
}
