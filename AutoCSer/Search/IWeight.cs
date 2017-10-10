using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 权重计算接口
    /// </summary>
    public interface IWeight
    {
        /// <summary>
        /// 计算词频权重
        /// </summary>
        /// <param name="totalCount">总词频</param>
        /// <param name="wordCount">当前分词总词频</param>
        /// <returns>词频权重</returns>
        byte GetWordCount(long totalCount, int wordCount);
        /// <summary>
        /// 计算分词类型权重
        /// </summary>
        /// <param name="wordType">分词类型</param>
        /// <param name="size">分词长度</param>
        /// <returns>分词权重</returns>
        byte GetWordType(WordType wordType, int size);
        /// <summary>
        /// 计算匹配词频权重
        /// </summary>
        /// <param name="wordCount">当前分词总词频</param>
        /// <param name="indexCount">当前数据匹配数量</param>
        /// <returns>匹配词频权重</returns>
        byte GetIndexCount(int wordCount, int indexCount);
    }
}
