using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStaticSimpleServer
{
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    /// <typeparam name="callType">调用类型</typeparam>
    public abstract class ServerCall<callType> : TcpInternalSimpleServer.ServerCall
        where callType : ServerCall<callType>
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="taskType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(TcpInternalSimpleServer.ServerSocket socket, TcpServer.ServerTaskType taskType)
        {
            Socket = socket;
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
            AutoCSer.Threading.RingPool<callType>.Default.PushNotNull(call);
        }
    }
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    /// <typeparam name="callType">调用类型</typeparam>
    /// <typeparam name="inputParameterType">输入参数类型</typeparam>
    public abstract class ServerCall<callType, inputParameterType> : ServerCall<callType>
        where callType : ServerCall<callType, inputParameterType>
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        protected inputParameterType inputParameter;
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="taskType"></param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(TcpInternalSimpleServer.ServerSocket socket, TcpServer.ServerTaskType taskType, ref inputParameterType inputParameter)
        {
            Socket = socket;
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
            inputParameter = default(inputParameterType);
            AutoCSer.Threading.RingPool<callType>.Default.PushNotNull(call);
        }
    }
}
