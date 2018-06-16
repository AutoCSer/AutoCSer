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
        /// 数组节点（嵌套节点）
        /// </summary>
        Array,
        /// <summary>
        /// 32768 基分段 数组节点（嵌套节点）
        /// </summary>
        FragmentArray,
        /// <summary>
        /// 字典节点（嵌套节点）
        /// </summary>
        Dictionary,
        /// <summary>
        /// 256 基分片 字典节点（嵌套节点）
        /// </summary>
        FragmentDictionary,
        /// <summary>
        /// 搜索树字典节点（嵌套节点）
        /// </summary>
        SearchTreeDictionary,

        /// <summary>
        /// 队列消费节点（数据节点）
        /// </summary>
        MessageQueueConsumer,
        /// <summary>
        /// 多消费者队列消费节点（数据节点）
        /// </summary>
        MessageQueueConsumers,
        /// <summary>
        /// 消息分发节点（数据节点）
        /// </summary>
        MessageDistributor,

        /// <summary>
        /// 数组节点（数据节点）
        /// </summary>
        ValueArray,
        /// <summary>
        /// 32768 基分段 数组节点（数据节点）
        /// </summary>
        ValueFragmentArray,
        /// <summary>
        /// 字典节点（数据节点）
        /// </summary>
        ValueDictionary,
        /// <summary>
        /// 256 基分片 字典节点（数据节点）
        /// </summary>
        ValueFragmentDictionary,
        /// <summary>
        /// 搜索树字典节点（数据节点）
        /// </summary>
        ValueSearchTreeDictionary,

        /// <summary>
        /// 哈希表节点（数据节点）
        /// </summary>
        HashSet,
        /// <summary>
        /// 256 基分片 哈希表节点（数据节点）
        /// </summary>
        FragmentHashSet,
        /// <summary>
        /// 链表节点（数据节点）
        /// </summary>
        Link,
        /// <summary>
        /// 最小堆（数据节点）
        /// </summary>
        ArrayHeap,
        /// <summary>
        /// 位图节点（数据节点）
        /// </summary>
        Bitmap,
        /// <summary>
        /// 锁节点（数据节点）
        /// </summary>
        Lock,
    }
}
