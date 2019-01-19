using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpInternalServer.Emit
{
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    [AutoCSer.IOS.Preserve(AllMembers = true)]
    public abstract class ServerCall : TcpServer.ServerCall
    {
        /// <summary>
        /// 套接字
        /// </summary>
        public ServerSocketSender Sender;
        /// <summary>
        /// 服务器目标对象
        /// </summary>
        protected object serverValue;
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="serverValue">服务器目标对象</param>
        /// <param name="taskType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void set(ServerSocketSender socket, object serverValue, TcpServer.ServerTaskType taskType)
        {
            this.Sender = socket;
            this.serverValue = serverValue;
            CommandIndex = socket.ServerSocket.CommandIndex;
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
        /// <typeparam name="callType">服务器端调用类型</typeparam>
        /// <param name="socket"></param>
        /// <param name="serverValue"></param>
        /// <param name="taskType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Call<callType>(ServerSocketSender socket, object serverValue, TcpServer.ServerTaskType taskType)
            where callType : ServerCall
        {
            (AutoCSer.Threading.RingPool<callType>.Default.Pop() ?? AutoCSer.Emit.Constructor<callType>.New()).set(socket, serverValue, taskType);
        }
        /// <summary>
        /// 服务器端调用入池
        /// </summary>
        /// <typeparam name="callType">服务器端调用类型</typeparam>
        /// <param name="call">服务器端调用</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void push<callType>(callType call)
            where callType : ServerCall
        {
            Sender = null;
            serverValue = null;
            AutoCSer.Threading.RingPool<callType>.Default.PushNotNull(call);
        }
    }
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    /// <typeparam name="inputParameterType">输入参数类型</typeparam>
    [AutoCSer.IOS.Preserve(AllMembers = true)]
    public abstract class ServerCall<inputParameterType> : ServerCall
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        protected inputParameterType inputParameter;
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sender">套接字</param>
        /// <param name="serverValue">服务器目标对象</param>
        /// <param name="taskType">任务类型</param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void set(ServerSocketSender sender, object serverValue, TcpServer.ServerTaskType taskType, ref inputParameterType inputParameter)
        {
            this.Sender = sender;
            this.serverValue = serverValue;
            CommandIndex = sender.ServerSocket.CommandIndex;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            this.inputParameter = inputParameter;
            switch (taskType)
            {
                case TcpServer.ServerTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(threadPoolCall)) AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.Timeout: AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpTask: TcpServer.ServerCallTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueue: TcpServer.ServerCallQueue.Default.Add(this); return;
                case TcpServer.ServerTaskType.Queue: sender.Server.CallQueue.Add(this); return;
            }
        }
        /// <summary>
        /// 获取服务器端调用
        /// </summary>
        /// <typeparam name="callType">服务器端调用类型</typeparam>
        /// <param name="socket"></param>
        /// <param name="serverValue"></param>
        /// <param name="taskType"></param>
        /// <param name="inputParameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Call<callType>(ServerSocketSender socket, object serverValue, TcpServer.ServerTaskType taskType, ref inputParameterType inputParameter)
            where callType : ServerCall<inputParameterType>
        {
            (AutoCSer.Threading.RingPool<callType>.Default.Pop() ?? AutoCSer.Emit.Constructor<callType>.New()).set(socket, serverValue, taskType, ref inputParameter);
        }
        /// <summary>
        /// 服务器端调用入池
        /// </summary>
        /// <typeparam name="callType">服务器端调用类型</typeparam>
        /// <param name="call">服务器端调用</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected new void push<callType>(callType call)
            where callType : ServerCall<inputParameterType>
        {
            Sender = null;
            serverValue = null;
            inputParameter = default(inputParameterType);
            AutoCSer.Threading.RingPool<callType>.Default.PushNotNull(call);
        }
    }
}
