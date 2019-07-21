using System;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 初始化添加数据
        /// </summary>
        public sealed unsafe class InitializeAdder : WordSegmenter, IDisposable
        {
            /// <summary>
            /// 绑定结果池的分词搜索器
            /// </summary>
            private new readonly StaticSearcher<keyType> searcher;
            /// <summary>
            /// 是否已经释放资源
            /// </summary>
            private int isDisposed;
            /// <summary>
            /// 初始化添加数据
            /// </summary>
            /// <param name="searcher">绑定结果池的分词搜索器</param>
            internal InitializeAdder(StaticSearcher<keyType> searcher) : base(searcher)
            {
                this.searcher = searcher;
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public new void Dispose()
            {
                base.Dispose();
                if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0) searcher.FreeInitializeAdder();
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="texts"></param>
            public void Add(IEnumerable<KeyValue<keyType, string>> texts)
            {
                if (texts != null)
                {
                    foreach (KeyValue<keyType, string> text in texts) Add(text.Key, text.Value);
                }
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="text"></param>
            public void Add(keyType key, string text)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    getResult(text);
                    if (result.Count != 0)
                    {
                        searcher.initializeAdd(ref key, text, result);
                        if ((searcher.flags & SearchFlags.ResultIndexs) != 0)
                        {
                            indexArrays.PrepLength(result.Count);
                            foreach (ResultIndexLeftArray indexArray in result.Values) indexArrays.UnsafeAdd(indexArray.Indexs.Array);
                        }
                    }
                }
            }
        }
    }
}
