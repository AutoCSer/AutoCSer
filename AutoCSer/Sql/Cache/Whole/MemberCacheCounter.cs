using System;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 成员计数缓存
    /// </summary>
    /// <typeparam name="valueType">缓存数据类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    public abstract class MemberCacheCounter<valueType, memberCacheType> : MemberCache<valueType>
        where memberCacheType : MemberCacheCounter<valueType, memberCacheType>
    {
        /// <summary>
        /// 计数缓存
        /// </summary>
        public KeyValue<valueType, int> Counter;

        /// <summary>
        /// 节点内容
        /// </summary>
        public valueType NodeValue;
        /// <summary>
        /// 上一个内容节点
        /// </summary>
        public memberCacheType PreviousNode;
        /// <summary>
        /// 下一个内容节点
        /// </summary>
        public memberCacheType NextNode;
    }
}
