using System;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 测试回调类型
    /// </summary>
    internal enum TestType : byte
    {
        /// <summary>
        /// 设置数组数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        ArraySetNodeCache,
        /// <summary>
        /// 设置数组数据（客户端会产生 1 个数据节点对象与 1 个操作节点对象）
        /// </summary>
        ArraySet,
        /// <summary>
        /// 获取数组数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        ArrayGetNodeCache,
        /// <summary>
        /// 获取数组数据（客户端会产生 1 个查询节点对象）
        /// </summary>
        ArrayGet,

        /// <summary>
        /// 设置字典数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        DictionarySetNodeCache,
        /// <summary>
        /// 设置字典数据（客户端会产生 1 个数据节点对象与 1 个操作节点对象）
        /// </summary>
        DictionarySet,
        /// <summary>
        /// 获取字典数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        DictionaryGetNodeCache,
        /// <summary>
        /// 获取字典数据（客户端会产生 1 个查询节点对象）
        /// </summary>
        DictionaryGet,
        /// <summary>
        /// 删除字典数据（客户端会产生 1 个数据节点对象与 1 个操作节点对象）
        /// </summary>
        DictionaryRemove,

        /// <summary>
        /// 设置二进制序列化数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        BinarySetNodeCache,
        /// <summary>
        /// 设置二进制序列化数据（相同的数据，客户端会产生 1 个操作节点对象）
        /// </summary>
        BinarySetValueCache,
        /// <summary>
        /// 设置二进制序列化数据（客户端会产生 1 个数据节点对象与 1 个操作节点对象）
        /// </summary>
        BinarySet,
        /// <summary>
        /// 获取二进制序列化数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        BinaryGetNodeCache,
        /// <summary>
        /// 获取二进制序列化数据（客户端会产生 1 个查询节点对象）
        /// </summary>
        BinaryGet,

        /// <summary>
        /// 设置 Json 数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        JsonSetNodeCache,
        /// <summary>
        /// 设置 Json 数据（相同的数据，客户端会产生 1 个操作节点对象）
        /// </summary>
        JsonSetValueCache,
        /// <summary>
        /// 设置 Json 数据（客户端会产生 1 个数据节点对象与 1 个操作节点对象）
        /// </summary>
        JsonSet,
        /// <summary>
        /// 获取 Json 数据（执行相同指令，客户端不会产生临时节点对象）
        /// </summary>
        JsonGetNodeCache,
        /// <summary>
        /// 获取 Json 数据（客户端会产生 1 个查询节点对象）
        /// </summary>
        JsonGet,

        /// <summary>
        /// 消息队列同时添加与处理消息（生产消费实时并行测试）
        /// </summary>
        MessageQueueMixing,
        /// <summary>
        /// 消息队列添加消息
        /// </summary>
        MessageQueueEnqueue,
        /// <summary>
        /// 消息队列处理消息
        /// </summary>
        MessageQueueDequeue,

        /// <summary>
        /// 消息分发同时添加与处理消息（生产消费实时并行测试）
        /// </summary>
        MessageDistributionMixing,
        /// <summary>
        /// 消息分发添加消息
        /// </summary>
        MessageDistributionEnqueue,
        /// <summary>
        /// 消息分发处理消息
        /// </summary>
        MessageDistributionDequeue,
    }
}
