using System;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// 自定义序列化计算回调输出缓冲区
    /// </summary>
    internal unsafe sealed class ServerCustomSerializeBuffer
    {
        /// <summary>
        /// 输出缓冲区大小
        /// </summary>
        internal const int BufferSize = 128 << 10;
        /// <summary>
        /// 下一个缓冲区
        /// </summary>
        internal ServerCustomSerializeBuffer LinkNext;
        /// <summary>
        /// 计算任务数据缓冲区
        /// </summary>
        internal readonly byte[] Buffer = new byte[BufferSize];
        /// <summary>
        /// 缓冲区起始位置
        /// </summary>
        internal int StartIndex;
        /// <summary>
        /// 数据缓冲区字节数
        /// </summary>
        internal int Size;
        /// <summary>
        /// 复制数据到输出缓冲区
        /// </summary>
        /// <param name="data"></param>
        internal void Copy(ref SubArray<byte> data)
        {
            System.Buffer.BlockCopy(data.BufferArray, data.StartIndex, Buffer, Size, data.Count);
            Size += data.Count;
        }
        /// <summary>
        /// 序列化输出
        /// </summary>
        /// <param name="write"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal byte* Serialize(byte* write, int count)
        {
            int size = count * (sizeof(int) * 2);
            fixed (byte* bufferFixed = Buffer)
            {
                byte* start = bufferFixed + StartIndex, end = start + size;
                do
                {
                    *(long*)write = *(long*)start;
                    *(int*)(write + sizeof(int) * 2) = *(int*)start + *(int*)(start + sizeof(int));
                    start += sizeof(int) * 2;
                    write += sizeof(int) * 3;
                }
                while (start != end);
            }
            StartIndex += size;
            Size -= size;
            return write;
        }
        /// <summary>
        /// 序列化输出
        /// </summary>
        /// <param name="write"></param>
        /// <returns></returns>
        internal byte* Serialize(byte* write)
        {
            fixed (byte* bufferFixed = Buffer)
            {
                byte* start = bufferFixed + StartIndex, end = start + Size;
                do
                {
                    *(long*)write = *(long*)start;
                    *(int*)(write + sizeof(int) * 2) = *(int*)start + *(int*)(start + sizeof(int));
                    start += sizeof(int) * 2;
                    write += sizeof(int) * 3;
                }
                while (start != end);
            }
            StartIndex = Size = 0;
            return write;
        }
    }
}
