using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 文件流扩展
    /// </summary>
    internal static class FileStreamExtension
    {
        /// <summary>
        /// 清除该流的所有缓冲区会使得所有缓冲的数据都将写入到文件系统
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="isToDisk"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Flush(this FileStream fileStream, bool isToDisk)
        {
            fileStream.Flush();
        }
    }
}
