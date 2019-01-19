using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// TCP 组件基类
        /// </summary>
        [FieldOffset(0)]
        public CommandBase CommandBase;
        ///// <summary>
        ///// TCP 客户端命令回调任务
        ///// </summary>
        //[FieldOffset(0)]
        //public CommandKeepTask CommandKeepTask;
        /// <summary>
        /// TCP 客户端命令
        /// </summary>
        [FieldOffset(0)]
        public ClientCommand.Command ClientCommand;
        /// <summary>
        /// TCP 客户端命令
        /// </summary>
        [FieldOffset(0)]
        public ClientCommand.CommandBase ClientCommandBase;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        [FieldOffset(0)]
        public ClientSocketBase ClientSocketBase;
        /// <summary>
        /// TCP 客户端套接字数据发送
        /// </summary>
        [FieldOffset(0)]
        public ClientSocketSenderBase ClientSocketSenderBase;
        /// <summary>
        /// TCP 服务器端同步调用
        /// </summary>
        [FieldOffset(0)]
        public ServerCall ServerCall;
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        [FieldOffset(0)]
        public ServerSocketSenderBase ServerSocketSenderBase;
        /// <summary>
        /// TCP 任务处理配置
        /// </summary>
        [FieldOffset(0)]
        public TaskConfig TaskConfig;
    }
}
