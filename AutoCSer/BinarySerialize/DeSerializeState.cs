using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 反序列化状态
    /// </summary>
    public enum DeSerializeState : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 数据不可识别
        /// </summary>
        UnknownData,
        /// <summary>
        /// 成员位图检测失败
        /// </summary>
        MemberMap,
        /// <summary>
        /// 成员位图类型错误
        /// </summary>
        MemberMapType,
        /// <summary>
        /// 成员位图数量验证失败
        /// </summary>
        MemberMapVerify,
        /// <summary>
        /// 头部数据不匹配
        /// </summary>
        HeaderError,
        /// <summary>
        /// 结束验证错误
        /// </summary>
        EndVerify,
        /// <summary>
        /// 数据完整检测失败
        /// </summary>
        FullDataError,
        /// <summary>
        /// 没有命中历史对象
        /// </summary>
        NoPoint,
        /// <summary>
        /// 数据长度不足
        /// </summary>
        IndexOutOfRange,
        /// <summary>
        /// 不支持对象 null 解析检测失败
        /// </summary>
        NotNull,
        /// <summary>
        /// 成员索引检测失败
        /// </summary>
        MemberIndex,
        /// <summary>
        /// JSON反序列化失败
        /// </summary>
        JsonError,
        /// <summary>
        /// 远程类型加载失败
        /// </summary>
        RemoteTypeError,
        /// <summary>
        /// 类型解析错误
        /// </summary>
        ErrorType,
        /// <summary>
        /// 数组大小超出范围
        /// </summary>
        ArraySizeOutOfRange,
        /// <summary>
        /// 自定义序列化失败
        /// </summary>
        Custom,
    }
}
