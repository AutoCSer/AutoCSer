using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 自定义 TCP 服务器端同步调用任务
    /// </summary>
    internal sealed class CustomTaskAsyncServerCall : ServerCall, INotifyCompletion
    {
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        private readonly ServerSocketSenderBase sender;
        /// <summary>
        /// 自定义任务
        /// </summary>
        private readonly Action task;
        /// <summary>
        /// 异步回调
        /// </summary>
        private Action continuation;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return false; } }
        /// <summary>
        /// 自定义 TCP 服务器端同步调用任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="task"></param>
        internal CustomTaskAsyncServerCall(ServerSocketSenderBase sender, Action task)
        {
            this.sender = sender;
            this.task = task;
        }
        /// <summary>
        /// 调用处理
        /// </summary>
        public override void Call()
        {
            try
            {
                task();
            }
            catch (Exception error)
            {
                sender.VirtualAddLog(error);
            }
            finally
            {
                if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
            }
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetResult() { }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public CustomTaskAsyncServerCall GetAwaiter()
        {
            return this;
        }
    }
}
