using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务更新日志类型
    /// </summary>
    internal enum LogType : byte
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        RegisterServer,
        /// <summary>
        /// 注销服务
        /// </summary>
        RemoveServer,
    }
}
