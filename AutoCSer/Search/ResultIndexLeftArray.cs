using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 索引中间结果
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ResultIndexLeftArray
    {
        /// <summary>
        /// 文本匹配索引位置
        /// </summary>
        public LeftArray<int> Indexs;
        /// <summary>
        /// 文本长度
        /// </summary>
        public int TextLength;
        /// <summary>
        /// 分词类型
        /// </summary>
        public WordType WordType;
        /// <summary>
        /// 设置文本长度与分词类型
        /// </summary>
        /// <param name="textLength">文本长度</param>
        /// <param name="wordType">分词类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int textLength, WordType wordType)
        {
            TextLength = textLength;
            WordType = wordType;
        }
    }
}
