using System;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP命令服务接口
    /// </summary>
    public interface ISetTcpServer<serverType, attributeType>
        where serverType : TcpServer.ServerBase<attributeType>
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// 设置 TCP 命令服务端
        /// </summary>
        /// <param name="commandServer">TCP 命令服务端</param>
        void SetTcpServer(serverType commandServer);
    }
}
