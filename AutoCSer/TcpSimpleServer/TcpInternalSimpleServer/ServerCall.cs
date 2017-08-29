using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    public abstract class ServerCall : TcpServer.ServerCallBase
    {
        /// <summary>
        /// 套接字
        /// </summary>
        public ServerSocket Socket;
    }
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    /// <typeparam name="callType">调用类型</typeparam>
    /// <typeparam name="serverType">服务器目标对象类型</typeparam>
    public abstract class ServerCall<callType, serverType> : ServerCall
        where callType : ServerCall<callType, serverType>
        where serverType : class
    {
        /// <summary>
        /// 服务器目标对象
        /// </summary>
        protected serverType serverValue;
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="serverValue">服务器目标对象</param>
        /// <param name="taskType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(ServerSocket socket, serverType serverValue, TcpServer.ServerTaskType taskType)
        {
            this.Socket = socket;
            this.serverValue = serverValue;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            switch (taskType)
            {
                case TcpServer.ServerTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(threadPoolCall)) AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.Timeout: AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpTask: TcpServer.ServerCallTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueue: TcpServer.ServerCallQueue.Default.Add(this); return;
                case TcpServer.ServerTaskType.Queue: socket.Server.CallQueue.Add(this); return;
            }
        }
        /// <summary>
        /// 获取服务器端调用
        /// </summary>
        /// <returns>服务器端调用</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static callType Pop()
        {
            return AutoCSer.Threading.RingPool<callType>.Default.Pop();
        }
        /// <summary>
        /// 服务器端调用入池
        /// </summary>
        /// <param name="call">服务器端调用</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void push(callType call)
        {
            Socket = null;
            serverValue = null;
            AutoCSer.Threading.RingPool<callType>.Default.PushNotNull(call);
        }
    }
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    /// <typeparam name="callType">调用类型</typeparam>
    /// <typeparam name="serverType">服务器目标对象类型</typeparam>
    /// <typeparam name="inputParameterType">输入参数类型</typeparam>
    public abstract class ServerCall<callType, serverType, inputParameterType> : ServerCall<callType, serverType>
        where callType : ServerCall<callType, serverType, inputParameterType>
        where serverType : class
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        protected inputParameterType inputParameter;
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="serverValue">服务器目标对象</param>
        /// <param name="taskType">任务类型</param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(ServerSocket socket, serverType serverValue, TcpServer.ServerTaskType taskType, ref inputParameterType inputParameter)
        {
            this.Socket = socket;
            this.serverValue = serverValue;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            this.inputParameter = inputParameter;
            switch (taskType)
            {
                case TcpServer.ServerTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(threadPoolCall)) AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.Timeout: AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpTask: TcpServer.ServerCallTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueue: TcpServer.ServerCallQueue.Default.Add(this); return;
                case TcpServer.ServerTaskType.Queue: socket.Server.CallQueue.Add(this); return;
            }
        }
        /// <summary>
        /// 服务器端调用入池
        /// </summary>
        /// <param name="call">服务器端调用</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected new void push(callType call)
        {
            Socket = null;
            serverValue = null;
            inputParameter = default(inputParameterType);
            AutoCSer.Threading.RingPool<callType>.Default.PushNotNull(call);
        }
    }
}
