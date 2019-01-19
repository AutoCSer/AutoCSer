using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// MemoryStream 扩展操作
    /// </summary>
    internal static class MemoryStreamExtension
    {
        /// <summary>
        /// 内存流扩展设置
        /// </summary>
        private static readonly Action<MemoryStream, bool> memoryStreamExpandable = AutoCSer.Emit.Field.UnsafeSetField<MemoryStream, bool>("_expandable");
        /// <summary>
        /// 内存流转换
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns>内存流</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MemoryStream New(byte[] data, int index, int count)
        {
            MemoryStream memoryStream = new MemoryStream(data, index, count, true, true);
            memoryStreamExpandable(memoryStream, true);
            return memoryStream;
        }
    }
}
