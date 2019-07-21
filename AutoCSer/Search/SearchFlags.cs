using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 搜索选项
    /// </summary>
    [Flags]
    public enum SearchFlags : uint
    {
        /// <summary>
        /// 不保留数据
        /// </summary>
        None = 0,
        /// <summary>
        /// 是否保留原始文本信息
        /// </summary>
        Text = 1,
        /// <summary>
        /// 是否保存关键字在文本中的位置集合
        /// </summary>
        ResultIndexs = 2,
    }
}
