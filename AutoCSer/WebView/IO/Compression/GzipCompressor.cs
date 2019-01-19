using System;
using System.IO.Compression;
using System.IO;

namespace AutoCSer.IO.Compression
{
    /// <summary>
    /// gzip 压缩处理
    /// </summary>
    internal static class GzipCompressor
    {
#if !DOTNET2 && !DOTNET4
        /// <summary>
        /// 数据压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="buffer">输出缓冲区</param>
        /// <param name="compressData">压缩数据</param>
        /// <param name="seek">起始位置</param>
        /// <param name="compressHeadSize">压缩多余头部</param>
        /// <param name="isFastest"></param>
        /// <returns>是否压缩成功</returns>
        internal static bool Get(byte[] data, int startIndex, int count, ref SubBuffer.PoolBufferFull buffer, ref SubArray<byte> compressData, int seek = 0, int compressHeadSize = 0, bool isFastest = false)
#else
        /// <summary>
        /// 数据压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="buffer">输出缓冲区</param>
        /// <param name="compressData">压缩数据</param>
        /// <param name="seek">起始位置</param>
        /// <param name="compressHeadSize">压缩多余头部</param>
        /// <returns>是否压缩成功</returns>
        internal static bool Get(byte[] data, int startIndex, int count, ref SubBuffer.PoolBufferFull buffer, ref SubArray<byte> compressData, int seek = 0, int compressHeadSize = 0)
#endif
        {
            int length = count + seek;
            SubBuffer.Pool.GetBuffer(ref buffer, length);
            using (MemoryStream dataStream = AutoCSer.Extension.MemoryStreamExtension.New(buffer.Buffer, buffer.StartIndex, buffer.Length))
            {
                if (seek != 0) dataStream.Seek(seek, SeekOrigin.Begin);
#if DOTNET2 || DOTNET4
                using (GZipStream compressStream = new GZipStream(dataStream, CompressionMode.Compress, true)) compressStream.Write(data, startIndex, count);
#else
                using (GZipStream compressStream = isFastest ? new GZipStream(dataStream, CompressionLevel.Fastest, true) : new GZipStream(dataStream, CompressionMode.Compress, true)) compressStream.Write(data, startIndex, count);
#endif
                    if (dataStream.Position + compressHeadSize < length)
                {
                    byte[] streamBuffer = dataStream.GetBuffer();
                    if (streamBuffer == buffer.Buffer && buffer.PoolBuffer.Pool != null) compressData.Set(streamBuffer, buffer.StartIndex + seek, (int)dataStream.Position - seek);
                    else compressData.Set(streamBuffer, seek, (int)dataStream.Position - seek);
                    return true;
                }
            }
            return false;
        }

#if !DOTNET2 && !DOTNET4
        /// <summary>
        /// 数据压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="compressData">压缩数据</param>
        /// <param name="seek">起始位置</param>
        /// <param name="compressHeadSize">压缩多余头部</param>
        /// <param name="isFastest"></param>
        /// <returns>是否压缩成功</returns>
        internal static bool Get(ref SubArray<byte> data, ref SubArray<byte> compressData, int seek = 0, int compressHeadSize = 0, bool isFastest = false)
#else
        /// <summary>
        /// 数据压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="compressData">压缩数据</param>
        /// <param name="seek">起始位置</param>
        /// <param name="compressHeadSize">压缩多余头部</param>
        /// <returns>是否压缩成功</returns>
        internal static bool Get(ref SubArray<byte> data, ref SubArray<byte> compressData, int seek = 0, int compressHeadSize = 0)
#endif

        {
            int length = data.Length + seek;
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
            SubBuffer.Pool.GetBuffer(ref buffer, length);
            try
            {
                using (MemoryStream dataStream = AutoCSer.Extension.MemoryStreamExtension.New(buffer.Buffer, buffer.StartIndex, buffer.Length))
                {
                    if (seek != 0) dataStream.Seek(seek, SeekOrigin.Begin);
#if DOTNET2 || DOTNET4
                    using (GZipStream compressStream = new GZipStream(dataStream, CompressionMode.Compress, true))
#else
                    using (GZipStream compressStream = isFastest ? new GZipStream(dataStream, CompressionLevel.Fastest, true) : new GZipStream(dataStream, CompressionMode.Compress, true))
#endif
                    {
                        compressStream.Write(data.Array, data.Start, data.Length);
                    }
                    if (dataStream.Position + compressHeadSize < length)
                    {
                        byte[] streamBuffer = dataStream.GetBuffer();
                        if (streamBuffer == buffer.Buffer && buffer.PoolBuffer.Pool != null)
                        {
                            byte[] newData = new byte[(int)dataStream.Position];
                            Buffer.BlockCopy(streamBuffer, buffer.StartIndex + seek, newData, seek, compressHeadSize = (int)dataStream.Position - seek);
                            compressData.Set(newData, seek, compressHeadSize);
                        }
                        else compressData.Set(streamBuffer, seek, (int)dataStream.Position - seek);
                        return true;
                    }
                }
            }
            finally { buffer.Free(); }
            return false;
        }
    }
}
