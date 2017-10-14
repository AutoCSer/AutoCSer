using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.WebClient
{
    /// <summary>
    /// 解压器
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct DeCompressor
    {
        /// <summary>
        /// 缓冲区长度
        /// </summary>
        private const int bufferSize = 4 << 10;
        /// <summary>
        /// 压缩输出流
        /// </summary>
        public Stream CompressStream;
        /// <summary>
        /// 输出数据流
        /// </summary>
        private UnmanagedStream dataStream;
        /// <summary>
        /// 获取解压数据
        /// </summary>
        /// <returns>解压数据</returns>
        internal byte[] Get()
        {
            AutoCSer.SubBuffer.PoolBufferFull buffer = default(AutoCSer.SubBuffer.PoolBufferFull);
            byte* data = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                AutoCSer.SubBuffer.Pool.GetBuffer(ref buffer, bufferSize);
                using (dataStream = new UnmanagedStream(data, AutoCSer.UnmanagedPool.DefaultSize))
                {
                    fixed (byte* bufferFixed = buffer.Buffer)
                    {
                        byte* start = bufferFixed + buffer.StartIndex;
                        do
                        {
                            int length = CompressStream.Read(buffer.Buffer, buffer.StartIndex, bufferSize);
                            if (length == 0) break;
                            dataStream.WriteNotEmpty(start, length);
                        }
                        while (true);
                    }
                    return dataStream.GetArray();
                }
            }
            finally
            {
                AutoCSer.UnmanagedPool.Default.Push(data);
                buffer.Free();
            }
        }
        /// <summary>
        /// 获取解压数据
        /// </summary>
        private unsafe void get()
        {
        }
    }
}
