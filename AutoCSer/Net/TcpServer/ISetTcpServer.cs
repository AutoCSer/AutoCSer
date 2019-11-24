using System;

namespace AutoCSer.Net.TcpServer
{
#if NOJIT
    /// <summary>
    /// TCP命令服务接口
    /// </summary>
    public interface ISetTcpServer
    {
        /// <summary>
        /// 设置 TCP 命令服务端
        /// </summary>
        /// <param name="commandServer">TCP 命令服务端</param>
        void SetTcpServer(CommandBase commandServer);
    }
#else
    /// <summary>
    /// TCP命令服务接口
    /// </summary>
    public interface ISetTcpServer<serverType>
        where serverType : ServerBase
    {
        /// <summary>
        /// 设置 TCP 命令服务端
        /// </summary>
        /// <param name="commandServer">TCP 命令服务端</param>
        void SetTcpServer(serverType commandServer);
    }
#endif
}
