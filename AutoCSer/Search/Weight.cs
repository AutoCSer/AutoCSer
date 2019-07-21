using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Search
{
    /// <summary>
    /// 默认权重计算
    /// </summary>
    public sealed class Weight : AutoCSer.Search.IWeight
    {
        /// <summary>
        /// 默认权重计算
        /// </summary>
        private Weight() { }
        /// <summary>
        /// 计算词频权重
        /// </summary>
        /// <param name="totalCount">总词频</param>
        /// <param name="wordCount">当前分词总词频</param>
        /// <returns>词频权重</returns>
        public byte GetWordCount(long totalCount, int wordCount)
        {
            return getWordCount(totalCount / Math.Max(wordCount, 1));
        }
        /// <summary>
        /// 计算词频权重
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private byte getWordCount(long weight)
        {
            return Math.Max((byte)((ulong)weight).bits(), (byte)1);
        }
        /// <summary>
        /// 计算分词类型权重
        /// </summary>
        /// <param name="wordType">分词类型</param>
        /// <param name="size">分词长度</param>
        /// <returns>分词权重</returns>
        public byte GetWordType(AutoCSer.Search.WordType wordType, int size)
        {
            if ((wordType & AutoCSer.Search.WordType.TrieGraph) != 0) return getWordType(size);
            if ((wordType & AutoCSer.Search.WordType.OtherLetter) != 0) return getWordType(Math.Max(size, 1));
            if ((wordType &= (AutoCSer.Search.WordType.Letter | AutoCSer.Search.WordType.Number | AutoCSer.Search.WordType.Keep)) != 0)
            {
                return getWordType(((byte)wordType & ((byte)wordType - 1)) == 0 ? Math.Min(2, size) : size);
            }
            return 1;
        }
        /// <summary>
        /// 计算分词类型权重
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static byte getWordType(int weight)
        {
            return (byte)Math.Min(weight, 255);
        }
        /// <summary>
        /// 计算匹配词频权重
        /// </summary>
        /// <param name="wordCount">当前分词总词频</param>
        /// <param name="indexCount">当前数据匹配数量</param>
        /// <returns>匹配词频权重</returns>
        public byte GetIndexCount(int wordCount, int indexCount)
        {
            return getWordCount((long)indexCount * (long)indexCount / Math.Max(wordCount, 1));
        }
        /// <summary>
        /// 默认权重计算
        /// </summary>
        public static readonly Weight Default = new Weight();
    }
}
