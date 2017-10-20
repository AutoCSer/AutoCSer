using System;
using System.Threading;
using AutoCSer.Log;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer
{
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    public abstract unsafe class Server<attributeType> : TcpServer.ServerBase<attributeType>
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// 是否支持自定义数据包
        /// </summary>
        protected override bool isCustomData
        {
            get { return false; }
        }
        /// <summary>
        /// 是否支持保持回调
        /// </summary>
        protected override bool isKeepCallback
        {
            get { return false; }
        }
        /// <summary>
        /// 是否支持合并命令处理
        /// </summary>
        protected override bool isMergeCommand
        {
            get { return false; }
        }
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="log">日志接口</param>
        internal Server(attributeType attribute, Func<System.Net.Sockets.Socket, bool> verify, ILog log)
            : base(attribute, verify, log, attribute.ServerTaskType == ServerTaskType.Queue)
        {
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>是否成功</returns>
        public virtual bool Start()
        {
            if (start())
            {
                startGetSocket();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取客户端套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void startGetSocket()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(GetSocket);
            Thread.Sleep(0);
            AutoCSer.DomainUnload.Unloader.Add(this, DomainUnload.Type.TcpCommandBaseDispose);
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        internal abstract void GetSocket();
    }
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    /// <typeparam name="serverType"></typeparam>
    /// <typeparam name="serverSocketSenderType"></typeparam>
    public abstract unsafe class Server<attributeType, serverType, serverSocketSenderType> : Server<attributeType>
        where serverType : Server<attributeType, serverType, serverSocketSenderType>
        where attributeType : ServerAttribute
        where serverSocketSenderType : TcpServer.ServerSocketSenderBase
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="log">日志接口</param>
        internal Server(attributeType attribute, Func<System.Net.Sockets.Socket, bool> verify, ILog log)
            : base(attribute, verify, log)
        {
        }
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="index">命令序号</param>
        /// <param name="sender">TCP 内部服务套接字数据发送</param>
        /// <param name="data">命令数据</param>
        public abstract void DoCommand(int index, serverSocketSenderType sender, ref SubArray<byte> data);
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="index"></param>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="dataSize"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DoCommand(int index, serverSocketSenderType socket, ref SubBuffer.PoolBufferFull buffer, int dataSize)
        {
            SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = dataSize };
            DoCommand(index, socket, ref data);
            buffer.PoolBuffer.Free();
        }
    }
}
