using System;

namespace AutoCSer.CacheServer.OperationParameter
{
    /// <summary>
    /// 操作类型
    /// </summary>
    internal enum OperationType : ushort
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 重建缓存数组大小
        /// </summary>
        LoadArraySize,
        /// <summary>
        /// 加载服务端数据结构索引标识信息
        /// </summary>
        LoadIndexIdentity,
        /// <summary>
        /// 添加数据结构定义
        /// </summary>
        GetOrCreateDataStructure,
        /// <summary>
        /// 删除数据结构定义
        /// </summary>
        RemoveDataStructure,

        /// <summary>
        /// 获取或者创建节点
        /// </summary>
        GetOrCreateNode,
        /// <summary>
        /// 删除节点或者数据
        /// </summary>
        Remove,
        /// <summary>
        /// 清除集合数据
        /// </summary>
        Clear,
        /// <summary>
        /// 添加或者设置数据
        /// </summary>
        SetValue,
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        GetRemove,
        /// <summary>
        /// 前置插入数据
        /// </summary>
        InsertBefore,
        /// <summary>
        /// 后置插入数据
        /// </summary>
        InsertAfter,
        /// <summary>
        /// 数据逻辑取反
        /// </summary>
        SetNegate,
        /// <summary>
        /// 修改数据
        /// </summary>
        Update,

        /// <summary>
        /// 创建短路径
        /// </summary>
        CreateShortPath,
        /// <summary>
        /// 获取数据
        /// </summary>
        GetValue,
        /// <summary>
        /// 获取容器元素数量
        /// </summary>
        GetCount,
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        ContainsKey,
        /// <summary>
        /// 获取数据集合
        /// </summary>
        GetValues,

        /// <summary>
        /// 消息队列获取当前读取数据标识
        /// </summary>
        MessageQueueGetDequeueIdentity,
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        MessageQueueSetDequeueIdentity,
        /// <summary>
        /// 消息队列添加数据
        /// </summary>
        MessageQueueEnqueue,
        /// <summary>
        /// 消息队列获取数据
        /// </summary>
        MessageQueueDequeue,
    }
}
