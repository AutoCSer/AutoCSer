using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 索引结果
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ResultIndexArray
    {
        /// <summary>
        /// 文本匹配索引位置
        /// </summary>
        public int[] Indexs;
        /// <summary>
        /// 文本长度
        /// </summary>
        public int TextLength;
        /// <summary>
        /// 设置文本长度与分词类型
        /// </summary>
        /// <param name="indexs">文本匹配索引位置</param>
        /// <param name="textLength">文本长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int[] indexs, int textLength)
        {
            Indexs = indexs;
            TextLength = textLength;
        }
    }
}
