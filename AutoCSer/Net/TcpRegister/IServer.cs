using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务客户端 绑定 TCP 服务端
    /// </summary>
    internal interface IServer
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="error"></param>
        void AddLog(Exception error);
        /// <summary>
        /// 创建 TCP 服务注册信息
        /// </summary>
        /// <param name="logType">TCP 内部注册服务更新日志类型</param>
        /// <returns></returns>
        ServerLog CreateServerLog(LogType logType);
    }
}
