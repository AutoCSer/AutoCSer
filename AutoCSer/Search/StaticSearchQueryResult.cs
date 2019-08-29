using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 分词结果
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct QueryResult
        {
            /// <summary>
            /// 数据结果
            /// </summary>
            internal Dictionary<keyType, ResultIndexArray> Dictionary;
            /// <summary>
            /// 数据结果
            /// </summary>
            public IEnumerable<KeyValuePair<keyType, ResultIndexArray>> Result
            {
                get { return Dictionary; }
            }
            /// <summary>
            /// 分词的结果数量
            /// </summary>
            public int Count
            {
                get { return Dictionary.Count; }
            }
            /// <summary>
            /// 总词频（在所有文本中出现次数的总和，用于权重计算）
            /// </summary>
            public int WordCount;
            /// <summary>
            /// 分词类型
            /// </summary>
            public WordType WordType;

            /// <summary>
            /// 复制数据
            /// </summary>
            /// <param name="array"></param>
            public void CopyTo(ref LeftArray<KeyValuePair<keyType, ResultIndexArray>> array)
            {
                array.PrepLength(Dictionary.Count);
                foreach (KeyValuePair<keyType, ResultIndexArray> value in Dictionary) array.UnsafeAdd(value);
            }
        }
    }
}
