using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

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
        public void Call(ServerSocketSender socket, object serverValue, TcpServer.ServerTaskType taskType)
        {
            this.Sender = socket;
            this.serverValue = serverValue;
            CommandIndex = socket.ServerSocket.CommandIndex;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            switch (taskType)
            {
                case TcpServer.ServerTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(ThreadPoolCall)) AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.Timeout: AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.TcpTask: TcpServer.ServerCallThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueue: TcpServer.ServerCallQueue.Default.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueueLink: TcpServer.ServerCallQueue.DefaultLink.Add(this); return;
                case TcpServer.ServerTaskType.Queue: socket.Server.CallQueue.Add(this); return;
                case TcpServer.ServerTaskType.QueueLink: socket.Server.CallQueueLink.Add(this); return;
            }
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="serverValue">服务器目标对象</param>
        /// <param name="taskType"></param>
        /// <param name="callQueueIndex">独占 TCP 服务器端同步调用队列编号</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ServerSocketSender socket, object serverValue, TcpServer.ServerTaskType taskType, byte callQueueIndex)
        {
            this.Sender = socket;
            this.serverValue = serverValue;
            CommandIndex = socket.ServerSocket.CommandIndex;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            switch (taskType)
            {
                case TcpServer.ServerTaskType.Queue: socket.Server.CallQueueArray[callQueueIndex].Key.Add(this); return;
                case TcpServer.ServerTaskType.QueueLink: socket.Server.CallQueueArray[callQueueIndex].Value.Add(this); return;
            }
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
        /// <summary>
        /// 获取服务器端调用
        /// </summary>
        /// <typeparam name="callType">服务器端调用类型</typeparam>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static callType PopNew<callType>() where callType : ServerCall
        {
            return AutoCSer.Threading.RingPool<callType>.Default.Pop() ?? AutoCSer.Metadata.DefaultConstructor<callType>.Constructor();
        }
        /// <summary>
        /// 空任务
        /// </summary>
        internal sealed class Null: ServerCall
        {
            /// <summary>
            /// 执行任务
            /// </summary>
            public override void RunTask()
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 空任务
            /// </summary>
            internal static readonly Null Default = new Null();
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
        public void CallEmit(ServerSocketSender sender, object serverValue, TcpServer.ServerTaskType taskType, ref inputParameterType inputParameter)
        {
            this.Sender = sender;
            this.serverValue = serverValue;
            CommandIndex = sender.ServerSocket.CommandIndex;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            this.inputParameter = inputParameter;
            switch (taskType)
            {
                case TcpServer.ServerTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(ThreadPoolCall)) AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.Timeout: AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.TcpTask: TcpServer.ServerCallThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueue: TcpServer.ServerCallQueue.Default.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueueLink: TcpServer.ServerCallQueue.DefaultLink.Add(this); return;
                case TcpServer.ServerTaskType.Queue: sender.Server.CallQueue.Add(this); return;
                case TcpServer.ServerTaskType.QueueLink: sender.Server.CallQueueLink.Add(this); return;
            }
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sender">套接字</param>
        /// <param name="serverValue">服务器目标对象</param>
        /// <param name="taskType">任务类型</param>
        /// <param name="callQueueIndex">独占 TCP 服务器端同步调用队列编号</param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CallQueueEmit(ServerSocketSender sender, object serverValue, TcpServer.ServerTaskType taskType, byte callQueueIndex, ref inputParameterType inputParameter)
        {
            this.Sender = sender;
            this.serverValue = serverValue;
            CommandIndex = sender.ServerSocket.CommandIndex;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            this.inputParameter = inputParameter;
            switch (taskType)
            {
                case TcpServer.ServerTaskType.Queue: sender.Server.CallQueueArray[callQueueIndex].Key.Add(this); return;
                case TcpServer.ServerTaskType.QueueLink: sender.Server.CallQueueArray[callQueueIndex].Value.Add(this); return;
            }
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sender">套接字</param>
        /// <param name="serverValue">服务器目标对象</param>
        /// <param name="queue">自定义队列</param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CallQueue(ServerSocketSender sender, object serverValue, AutoCSer.Net.TcpServer.ServerCallQueue queue, ref inputParameterType inputParameter)
        {
            this.Sender = sender;
            this.serverValue = serverValue;
            CommandIndex = sender.ServerSocket.CommandIndex;
            //CommandFlags = CommandIdentity.GetCommandFlags();
            this.inputParameter = inputParameter;
            queue.Add(this);
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
        /// <summary>
        /// 获取服务器端调用
        /// </summary>
        /// <typeparam name="callType">服务器端调用类型</typeparam>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static new callType PopNew<callType>() where callType : ServerCall<inputParameterType>
        {
            return AutoCSer.Threading.RingPool<callType>.Default.Pop() ?? AutoCSer.Metadata.DefaultConstructor<callType>.Constructor();
        }
    }
}
