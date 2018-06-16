using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 返回值类型
    /// </summary>
    public enum ReturnType : byte
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown,
        /// <summary>
        /// 操作成功
        /// </summary>
        Success,
        /// <summary>
        /// TCP 通讯错误
        /// </summary>
        TcpError,
        /// <summary>
        /// 反序列化错误
        /// </summary>
        DeSerializeError,
        /// <summary>
        /// 缺少必要的输入参数
        /// </summary>
        NullArgument,
        /// <summary>
        /// 操作类型与返回值匹配错误
        /// </summary>
        OperationTypeReturnValueError,
        /// <summary>
        /// 数据结构定义节点缺少指定的构造函数信息
        /// </summary>
        DataStructureNotFoundConstructor,
        /// <summary>
        /// 不允许创建数据节点
        /// </summary>
        CanNotCreateValueNode,
        /// <summary>
        /// 已存在节点的构造函数参数不匹配
        /// </summary>
        CheckConstructorParameterError,

        /// <summary>
        /// 数据结构定义冲突
        /// </summary>
        DataStructureNameExists,
        /// <summary>
        /// 非数据节点不允许创建短路径
        /// </summary>
        CanNotCreateShortPath,
        /// <summary>
        /// 没有找到短路径，客户端需重建（已过期）
        /// </summary>
        NotFoundShortPath,
        /// <summary>
        /// 找不到短路径映射的节点（已删除）
        /// </summary>
        NotFoundShortPathNode,
        /// <summary>
        /// 服务端反序列化错误
        /// </summary>
        ServerDeSerializeError,

        /// <summary>
        /// 缓存节点解析失败
        /// </summary>
        CacheNodeParseError,
        /// <summary>
        /// 没有找到缓存节点信息
        /// </summary>
        NotFoundCacheNodeInfo,
        /// <summary>
        /// 缓存节点需要文件持久化支持
        /// </summary>
        CacheNodeNeedFile,
        /// <summary>
        /// 服务端数据结构定义信息初始化错误
        /// </summary>
        ServerDataStructureCreateError,
        /// <summary>
        /// 没有找到数据结构定义
        /// </summary>
        DataStructureNameNotFound,
        /// <summary>
        /// 数据结构标识错误
        /// </summary>
        DataStructureIdentityError,
        /// <summary>
        /// 不支持的操作类型
        /// </summary>
        OperationTypeError,
        /// <summary>
        /// 参数数据加载错误
        /// </summary>
        ValueDataLoadError,
        /// <summary>
        /// 数组索引超出范围
        /// </summary>
        ArrayIndexOutOfRange,
        /// <summary>
        /// 访问的数组节点为 null
        /// </summary>
        NullArrayNode,
        /// <summary>
        /// 没有找到字典关键字
        /// </summary>
        NotFoundDictionaryKey,
        /// <summary>
        /// 链表索引超出范围
        /// </summary>
        LinkIndexOutOfRange,
        /// <summary>
        /// 搜索树索引超出范围
        /// </summary>
        SearchTreeDictionaryIndexOutOfRange,
        /// <summary>
        /// 堆为空，不可获取数据
        /// </summary>
        HeapIsEmpty,
        /// <summary>
        /// 不支持的更新操作类型
        /// </summary>
        UpdateOperationTypeError,
        /// <summary>
        /// 除 0 错误
        /// </summary>
        DivideByZero,

        /// <summary>
        /// 缓存已经释放
        /// </summary>
        Disposed,
        /// <summary>
        /// 没有找到可用的文件流
        /// </summary>
        NotFoundFileStream,
        /// <summary>
        /// 文件流已经存在
        /// </summary>
        FileStreamExists,
        /// <summary>
        /// 快照文件建立错误
        /// </summary>
        SnapshotFileStartError,
        /// <summary>
        /// 缓存处于不可写状态
        /// </summary>
        CanNotWrite,
        /// <summary>
        /// 缓存从服务不可写
        /// </summary>
        SlaveCanNotWrite,
        /// <summary>
        /// 没有找到缓存管理
        /// </summary>
        NotFoundSlaveCache,

        /// <summary>
        /// 消息队列已经被释放
        /// </summary>
        MessageQueueDisposed,
        /// <summary>
        /// 消息队列缓冲区已经被释放（主要是持久化异常）
        /// </summary>
        MessageQueueBufferDisposed,
        /// <summary>
        /// 消息队列没有找到写文件（可能初始化失败）
        /// </summary>
        MessageQueueNotFoundWriter,
        /// <summary>
        /// 消息队列读取初始化错误
        /// </summary>
        MessageQueueCreateReaderError,
        /// <summary>
        /// 消息队列读取编号超出范围
        /// </summary>
        MessageQueueReaderIndexOutOfRange,
        /// <summary>
        /// 消息队列没有找到读文件（可能已经被释放）
        /// </summary>
        MessageQueueNotFoundReader,
        /// <summary>
        /// 消息队列读文件数据标识不匹配
        /// </summary>
        MessageQueueReaderIdentityError,
        /// <summary>
        /// 消息队列读取委托已经存在
        /// </summary>
        MessageQueueGetMessageExists,

        /// <summary>
        /// 没有找到锁（已经被释放）
        /// </summary>
        NotFoundLock,
        /// <summary>
        /// 锁已经被占用
        /// </summary>
        Locked,
        /// <summary>
        /// 申请锁超时
        /// </summary>
        EnterLockTimeout,
    }
}
