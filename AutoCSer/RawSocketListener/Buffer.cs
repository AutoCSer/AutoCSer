using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.RawSocketListener
{
    /// <summary>
    /// 数据缓冲区
    /// </summary>
    public sealed class Buffer : AutoCSer.Threading.Link<Buffer>, IEnumerable<byte>, IDisposable
    {
        /// <summary>
        /// 数据
        /// </summary>
        private SubArray<byte> array;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        private BufferCount bufferCount;
        /// <summary>
        /// IPv4 数据包
        /// </summary>
        public Packet.Ip Ip
        {
            get { return new Packet.Ip(ref array); }
        }
        /// <summary>
        /// IPv6 数据包
        /// </summary>
        public Packet.Ip6 Ip6
        {
            get { return new Packet.Ip6(ref array); }
        }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        /// <param name="bufferCount">数据缓冲区计数</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">字节数量</param>
        internal Buffer(BufferCount bufferCount, int index, int count)
        {
            this.bufferCount = bufferCount;
            array.Set(bufferCount.Buffer.Buffer, index, count);
            ++bufferCount.Count;
        }
        /// <summary>
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>数据值</returns>
        public byte this[int index]
        {
            get { return array[index]; }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            BufferCount bufferCount = Interlocked.Exchange(ref this.bufferCount, null);
            if (bufferCount != null)
            {
                bufferCount.Free();
                array.SetNull();
            }
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
        {
            if (array.Length != 0) return new Enumerator<byte>.Array(array.Array, array.Start, array.EndIndex);
            return Enumerator<byte>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (array.Length != 0) return new Enumerator<byte>.Array(array.Array, array.Start, array.EndIndex);
            return Enumerator<byte>.Empty;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CopyTo(byte[] values, int index)
        {
            array.CopyTo(values, index);
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public byte[] GetArray()
        {
            return array.GetArray();
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="arrayType">数组类型</typeparam>
        /// <param name="getValue">数据获取委托</param>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public arrayType[] GetArray<arrayType>(Func<byte, arrayType> getValue)
        {
            return array.GetArray(getValue);
        }
    }
}
