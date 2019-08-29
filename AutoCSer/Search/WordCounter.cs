using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 关键字词频与数据结果
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal struct WordCounter
        {
            /// <summary>
            /// 数据结果
            /// </summary>
            internal Dictionary<keyType, ResultIndexArray> Result;
            /// <summary>
            /// 词频
            /// </summary>
            internal int Count;
            /// <summary>
            /// 分词类型
            /// </summary>
            internal WordType WordType;
            /// <summary>
            /// 创建数据结果字典
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void CreateResult()
            {
                Count = 0;
                Result = DictionaryCreator<keyType>.Create<ResultIndexArray>();
            }
            /// <summary>
            /// 添加数据结果
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="wordType">分词类型</param>
            /// <param name="indexs">数据结果</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Add(keyType key, WordType wordType, ref ResultIndexArray indexs)
            {
                int count = indexs.Indexs.Length;
                WordType = wordType;
                Result[key] = indexs;
                if (count == 0) ++Count;
                else Count += indexs.Indexs.Length;
            }
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>删除结果数量</returns>
            internal int Remove(keyType key)
            {
                ResultIndexArray indexs;
                if (Result.TryGetValue(key, out indexs))
                {
                    int count = indexs.Indexs.Length;
                    Result.Remove(key);
                    if (count == 0)
                    {
                        --Count;
                        return 1;
                    }
                    Count -= count;
                    return count;
                }
                return 0;
            }
            /// <summary>
            /// 复制分词结果
            /// </summary>
            /// <param name="result"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void SetResult(ref QueryResult result)
            {
                result.WordCount = Count;
                result.WordType = WordType;
                result.Dictionary = Result;
            }
        }
    }
}
