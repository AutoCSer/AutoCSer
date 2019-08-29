using System;
using System.Runtime.CompilerServices;

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

            /// <summary>
            /// 设置删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="text"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void SetRemove(ref keyType key, string text)
            {
                Text = text;
                Key = key;
                IsRemove = true;
            }
        }
    }
}
