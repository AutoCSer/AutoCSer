using System;
using System.IO;
using System.IO.Compression;

namespace AutoCSer.Net.WebClient
{
    /// <summary>
    /// GZip压缩流处理
    /// </summary>
    internal class GZipStream : CompressionStream
    {
        /// <summary>
        /// 获取压缩流
        /// </summary>
        /// <param name="dataStream">原始数据流</param>
        /// <returns>压缩流</returns>
        protected override Stream getStream(Stream dataStream)
        {
            return new System.IO.Compression.GZipStream(dataStream, CompressionMode.Compress, true);
        }
        /// <summary>
        /// 获取解压缩流
        /// </summary>
        /// <param name="dataStream">压缩数据流</param>
        /// <returns>解压缩流</returns>
        protected override Stream getDecompressStream(Stream dataStream)
        {
            return new System.IO.Compression.GZipStream(dataStream, CompressionMode.Decompress, false);
        }
    }
}
