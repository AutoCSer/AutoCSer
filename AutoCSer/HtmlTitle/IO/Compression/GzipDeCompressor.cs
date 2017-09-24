using System;
using System.IO.Compression;
using System.IO;

namespace AutoCSer.IO.Compression
{
    /// <summary>
    /// gzip 解压缩处理
    /// </summary>
    internal static class GzipDeCompressor
    {
        /// <summary>
        /// 数据解压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="buffer">输出缓冲区</param>
        /// <returns></returns>
        internal static int Get(byte[] data, int startIndex, int count, ref SubBuffer.PoolBufferFull buffer)
        {
            using (MemoryStream memoryStream = new MemoryStream(data, startIndex, count))
            using (GZipStream compressStream = new GZipStream(memoryStream, CompressionMode.Decompress, false))
            {
                return compressStream.Read(buffer.Buffer, buffer.StartIndex, buffer.Length);
            }
        }
    }
}
