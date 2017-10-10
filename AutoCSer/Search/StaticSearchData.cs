using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        internal sealed class SearchData : AutoCSer.Threading.Link<SearchData>
        {
            /// <summary>
            /// 文本
            /// </summary>
            internal string Text;
            /// <summary>
            /// 数据标识
            /// </summary>
            internal keyType Key;
            /// <summary>
            /// 删除文本还是增加文本
            /// </summary>
            internal bool IsRemove;
        }
    }
}
