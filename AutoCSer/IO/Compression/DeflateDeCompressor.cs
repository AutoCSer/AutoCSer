using System;
using System.IO;
using System.IO.Compression;

namespace AutoCSer.IO.Compression
{
    /// <summary>
    /// deflate 解压缩处理
    /// </summary>
    internal static class DeflateDeCompressor
    {
        /// <summary>
        /// 数据解压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="buffer">输出缓冲区</param>
        internal static void Get(byte[] data, int startIndex, int count, ref SubBuffer.PoolBufferFull buffer)
        {
            using (MemoryStream memoryStream = new MemoryStream(data, startIndex, count))
            using (DeflateStream compressStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true))
            //using (DeflateStream compressStream = new DeflateStream(memoryStream, CompressionMode.Decompress, false))
            {
                int size = buffer.StartIndex;
                SubBuffer.Pool.GetBuffer(ref buffer, size);
                try
                {
                    if (compressStream.Read(buffer.Buffer, buffer.StartIndex, size) == size) size = 0;
                }
                finally
                {
                    if (size != 0) buffer.Free();
                }
            }
        }
    }
}
