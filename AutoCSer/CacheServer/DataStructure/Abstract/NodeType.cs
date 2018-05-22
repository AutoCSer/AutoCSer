using System;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数据结构定义节点类型
    /// </summary>
    internal enum NodeType : byte
    {
        /// <summary>
        /// 未知节点
        /// </summary>
        Unknown,
        /// <summary>
        /// 数据叶子节点
        /// </summary>
        Value,
        /// <summary>
        /// 字典节点
        /// </summary>
        Dictionary,
        /// <summary>
        /// 搜索树字典节点
        /// </summary>
        SearchTreeDictionary,
        /// <summary>
        /// 数组节点
        /// </summary>
        Array,
        /// <summary>
        /// 哈希表节点
        /// </summary>
        HashSet,
        /// <summary>
        /// 链表节点
        /// </summary>
        Link,
        /// <summary>
        /// 256 基分片 字典节点
        /// </summary>
        FragmentDictionary,
        /// <summary>
        /// 32768 基分段 数组节点 
        /// </summary>
        FragmentArray,
        /// <summary>
        /// 256 基分片 哈希表节点
        /// </summary>
        FragmentHashSet,

        /// <summary>
        /// 字典节点（数据节点）
        /// </summary>
        ValueDictionary,
        /// <summary>
        /// 搜索树字典节点（数据节点）
        /// </summary>
        ValueSearchTreeDictionary,
        /// <summary>
        /// 数组节点（数据节点）
        /// </summary>
        ValueArray,
        /// <summary>
        /// 256 基分片 字典节点（数据节点）
        /// </summary>
        ValueFragmentDictionary,
        /// <summary>
        /// 32768 基分段 数组节点（数据节点）
        /// </summary>
        ValueFragmentArray,
    }
}
