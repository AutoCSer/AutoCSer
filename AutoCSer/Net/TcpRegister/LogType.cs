using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务更新日志类型
    /// </summary>
    internal enum LogType : byte
    {
        /// <summary>
        /// 客户端错误
        /// </summary>
        ClientError,
        /// <summary>
        /// 注册服务
        /// </summary>
        RegisterServer,
        /// <summary>
        /// 注册服务加载完毕
        /// </summary>
        RegisterLoaded,
        /// <summary>
        /// 注销服务
        /// </summary>
        RemoveServer,
    }
}
