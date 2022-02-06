using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 匹配类型
    /// </summary>
    public enum MatchType : byte
    {
        /// <summary>
        /// 无要求
        /// </summary>
        None,
        /// <summary>
        /// 匹配所有分词
        /// </summary>
        All,
        /// <summary>
        /// 记录未匹配分词
        /// </summary>
        Less,
    }
}
