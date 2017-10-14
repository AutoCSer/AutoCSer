using System;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.WebClient
{
    /// <summary>
    /// 压缩流处理
    /// </summary>
    public abstract class CompressionStream
    {
        /// <summary>
        /// 获取压缩流
        /// </summary>
        /// <param name="dataStream">原始数据流</param>
        /// <returns>压缩流</returns>
        protected abstract Stream getStream(Stream dataStream);
        /// <summary>
        /// 获取解压缩流
        /// </summary>
        /// <param name="dataStream">压缩数据流</param>
        /// <returns>解压缩流</returns>
        protected abstract Stream getDecompressStream(Stream dataStream);
        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="compressData">压缩数据</param>
        /// <returns>解压缩后的数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public byte[] GetDeCompress(byte[] compressData)
        {
            if (compressData.length() > 0)
            {
                return GetDeCompressUnsafe(compressData, 0, compressData.Length);
            }
            return null;
        }
        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="compressData">压缩数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">解压缩字节数</param>
        /// <returns>解压缩后的数据</returns>
        internal byte[] GetDeCompressUnsafe(byte[] compressData, int startIndex, int count)
        {
            using (Stream memoryStream = new MemoryStream(compressData, startIndex, count))
            using (Stream compressStream = getDecompressStream(memoryStream))
            {
                return new DeCompressor { CompressStream = compressStream }.Get();
            }
        }

        /// <summary>
        /// GZip 压缩流处理
        /// </summary>
        public static readonly CompressionStream GZip = new GZipStream();
        /// <summary>
        /// deflate 压缩流处理
        /// </summary>
        public static readonly CompressionStream Deflate = new DeflateStream();
    }
}
