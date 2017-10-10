using System;

namespace AutoCSer.Example.Search
{
    /// <summary>
    /// 搜索结果关键字
    /// </summary>
    public struct SearchKey : IEquatable<SearchKey>
    {
        /// <summary>
        /// 搜索结果类型
        /// </summary>
        public SearchType Type;
        /// <summary>
        /// 搜索结果标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SearchKey other)
        {
            return (((int)Type ^ (int)other.Type) | (Id ^ other.Id)) == 0;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((SearchKey)obj);
        }
        /// <summary>
        /// 计算 HASH 值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id ^ (int)Type;
        }
    }
}
