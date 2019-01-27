using System;
using System.IO;
using System.IO.Compression;

namespace AutoCSer.IO.Compression
{
    /// <summary>
    /// deflate 压缩
    /// </summary>
    public static class Deflate
    {
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
            try
            {
                SubArray<byte> compressData = default(SubArray<byte>);
                if (DeflateCompressor.Get(data, 0, data.Length, ref buffer, ref compressData, sizeof(int), 0))
                {
                    compressData.MoveStart(-sizeof(int));
                    return setSize(compressData.GetArray(), data.Length);
                }
                byte[] newData = new byte[data.Length + sizeof(int)];
                Buffer.BlockCopy(data, 0, newData, sizeof(int), data.Length);
                return setSize(newData, data.Length);
            }
            finally { buffer.TryFree(); }
        }
        /// <summary>
        /// 设置压缩数据原始数据长度
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private unsafe static byte[] setSize(byte[] data, int size)
        {
            fixed(byte* dataFixed = data) *(int*)dataFixed = size;
            return data;
        }
        /// <summary>
        /// 解压缩压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public unsafe static byte[] DeCompress(byte[] data)
        {
            byte[] newData;
            fixed (byte* dataFixed = data) newData = new byte[*(int*)dataFixed];
            using (MemoryStream memoryStream = new MemoryStream(data, sizeof(int), data.Length - sizeof(int)))
            using (DeflateStream compressStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true))
            {
                if (compressStream.Read(newData, 0, newData.Length) == newData.Length) return newData;
            }
            return null;
        }
    }
}
