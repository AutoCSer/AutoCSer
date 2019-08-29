using System;

namespace AutoCSer.Net.TcpStreamServer
{
    /// <summary>
    /// TCP 服务端套接字
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    /// <typeparam name="serverType">TCP 服务类型</typeparam>
    /// <typeparam name="socketType">TCP 服务端套接字类型</typeparam>
    /// <typeparam name="socketSenderType">TCP 服务套接字数据发送</typeparam>
    public abstract class ServerSocket<attributeType, serverType, socketType, socketSenderType> : TcpServer.ServerSocket
        where attributeType : ServerAttribute
        where serverType : Server<attributeType, serverType, socketSenderType>
        where socketType : ServerSocket<attributeType, serverType, socketType, socketSenderType>
        where socketSenderType : ServerSocketSender<attributeType, serverType, socketType, socketSenderType>
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        internal long TaskTicks;
        /// <summary>
        /// 下一个任务
        /// </summary>
        internal socketType NextTask;
        /// <summary>
        /// TCP 服务
        /// </summary>
        internal readonly serverType Server;
        /// <summary>
        /// TCP 服务端套接字
        /// </summary>
        /// <param name="server">TCP调用服务端</param>
        internal ServerSocket(serverType server)
            : base(server.BinaryDeSerializeConfig)
        {
            Server = server;
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private unsafe bool setCommand(int command)
        {
            if ((uint)command <= (uint)Server.MaxCommand
                && (commandData.Data != null || (commands.Map == null && Server.CreateCommandData(ref commandData, ref commands))))
            {
                commands.Set(command);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        internal override bool SetCommand(int methodIndex)
        {
            return setCommand(methodIndex + TcpServer.Server.CommandStartIndex);
        }
        /// <summary>
        /// 设置基础命令索引信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal override bool SetBaseCommand(int command)
        {
            return command < TcpServer.Server.CommandStartIndex && setCommand(command);
        }
    }
}
