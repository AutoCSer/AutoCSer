using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 成员状态
    /// </summary>
    public enum MemberState : byte
    {
        /// <summary>
        /// 空状态
        /// </summary>
        Unknown,
        /// <summary>
        /// 本地对象
        /// </summary>
        Local,
        /// <summary>
        /// 远程对象
        /// </summary>
        Remote,
        /// <summary>
        /// TCP 返回值错误
        /// </summary>
        TcpError,
        /// <summary>
        /// 反序列化失败
        /// </summary>
        DeSerializeError,
        /// <summary>
        /// 磁盘块编号错误
        /// </summary>
        BlockIndexError,
        /// <summary>
        /// 数据索引错误
        /// </summary>
        IndexError,
        /// <summary>
        /// 数据字节数量不匹配
        /// </summary>
        SizeError,
        /// <summary>
        /// 服务端异常
        /// </summary>
        ServerException,

        /// <summary>
        /// 没有找到磁盘块客户端
        /// </summary>
        NoClient,
    }
}
