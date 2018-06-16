using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct MessageItem
    {
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        internal AutoCSer.CacheServer.BufferCount BufferCount;
        /// <summary>
        /// 消息数据
        /// </summary>
        internal ValueData.Data Data;
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
            Data = buffer.Data;
            BufferCount = buffer.BufferCount;
            Data.ReturnType = ReturnType.Success;
            buffer.BufferCount = null;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="bufferCount"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ref BufferCount bufferCount)
        {
            BufferCount = bufferCount.Get(out Data);
        }
        /// <summary>
        /// 消息处理完毕
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnMessage()
        {
            FreeBuffer();
            Data.SetNull();
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
