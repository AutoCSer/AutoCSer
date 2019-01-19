using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 命令参数
    /// </summary>
    [Flags]
    public enum CommandFlags : uint
    {
        /// <summary>
        /// 缺省空参数
        /// </summary>
        None = 0,
        /// <summary>
        /// 是否采用JSON序列化,否则使用二进制序列化
        /// </summary>
        JsonSerialize = 0x20000000,
        /// <summary>
        /// 空数据
        /// </summary>
        NullData = 0x40000000,
        /// <summary>
        /// 
        /// </summary>
        _ = 0x80000000,
    }
}
