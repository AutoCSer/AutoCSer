using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 关键字词频与数据结果数组
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal struct WordCounterIndex
        {
            /// <summary>
            /// 关键字词频与数据结果数组
            /// </summary>
            internal WordCounter[] Array;
            /// <summary>
            /// 数组索引
            /// </summary>
            internal int Index;
            /// <summary>
            /// 设置关键字词频与数据结果数组
            /// </summary>
            /// <param name="array"></param>
            /// <param name="index"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(WordCounter[] array, int index)
            {
                Array = array;
                Index = index;
            }
            /// <summary>
            /// 创建数据结果字典
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void CreateResult()
            {
                Array[Index & ArrayPool.ArraySizeAnd].CreateResult();
            }
            /// <summary>
            /// 清除数据结果字典
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void ClearResult()
            {
                Array[Index & ArrayPool.ArraySizeAnd].Result = null;
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
                Array[Index & ArrayPool.ArraySizeAnd].Add(key, wordType, ref indexs);
            }
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>删除结果数量</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int Remove(keyType key)
            {
                return Array[Index & ArrayPool.ArraySizeAnd].Remove(key);
            }
            /// <summary>
            /// 复制分词结果
            /// </summary>
            /// <param name="result"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void SetResult(ref QueryResult result)
            {
                Array[Index & ArrayPool.ArraySizeAnd].SetResult(ref result);
            }
        }
    }
}
