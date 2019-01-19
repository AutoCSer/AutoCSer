using System;
using System.IO;
using System.IO.Compression;

namespace AutoCSer.IO.Compression
{
    /// <summary>
    /// deflate 压缩处理
    /// </summary>
    internal static class DeflateCompressor
    {
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
        {
            int length = count + seek;
            SubBuffer.Pool.GetBuffer(ref buffer, length);
            using (MemoryStream dataStream = AutoCSer.Extension.MemoryStreamExtension.New(buffer.Buffer, buffer.StartIndex, buffer.Length))
            {
                if (seek != 0) dataStream.Seek(seek, SeekOrigin.Begin);
#if DOTNET2 || DOTNET4 || UNITY3D
                using (DeflateStream compressStream = new DeflateStream(dataStream, CompressionMode.Compress, true)) compressStream.Write(data, startIndex, count);
#else
                using (DeflateStream compressStream = new DeflateStream(dataStream, CompressionLevel.Fastest, true)) compressStream.Write(data, startIndex, count);
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
//        /// <summary>
//        /// 数据压缩
//        /// </summary>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        internal static LeftArray<byte> Get(byte[] data)
//        {
//            using (MemoryStream dataStream = new MemoryStream())
//            {
//#if DOTNET2 || DOTNET4 || UNITY3D
//                using (DeflateStream compressStream = new DeflateStream(dataStream, CompressionMode.Compress, true)) compressStream.Write(data, 0, data.Length);
//#else
//                using (DeflateStream compressStream = new DeflateStream(dataStream, CompressionLevel.Fastest, true)) compressStream.Write(data, 0, data.Length);
//#endif
//                return new LeftArray<byte>((int)dataStream.Position, dataStream.GetBuffer());
//            }
//        }
    }
}
