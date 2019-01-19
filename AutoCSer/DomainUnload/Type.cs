using System;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 卸载处理类型
    /// </summary>
    internal enum Type : byte
    {
        None,
        /// <summary>
        /// 委托
        /// </summary>
        Action,
        /// <summary>
        /// 释放文件日志
        /// </summary>
        LogFileDispose,
        /// <summary>
        /// 释放 TCP 组件基类
        /// </summary>
        TcpCommandBaseDispose,
        /// <summary>
        /// 关闭 TCP 内部注册服务客户端
        /// </summary>
        TcpRegisterClientClose,
        /// <summary>
        /// 释放线程池
        /// </summary>
        ThreadPoolDispose,
    }
}
