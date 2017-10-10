using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 分词类型
    /// </summary>
    [Flags]
    public enum WordType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 中文
        /// </summary>
        Chinese = 1,
        /// <summary>
        /// 其它字母
        /// </summary>
        OtherLetter = 2,
        /// <summary>
        /// 字母
        /// </summary>
        Letter = 4,
        /// <summary>
        /// 数字
        /// </summary>
        Number = 8,
        /// <summary>
        /// 保留字符
        /// </summary>
        Keep = 0x10,
        /// <summary>
        /// Trie 图
        /// </summary>
        TrieGraph = 0x20,
    }
}
