using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 调用客户端回调保持
    /// </summary>
    public sealed class KeepCallback : IDisposable
    {
        /// <summary>
        /// 客户端命令
        /// </summary>
        private readonly ClientCommand.Command command;
        /// <summary>
        /// 命令会话标识
        /// </summary>
        private int commandIndex;
        /// <summary>
        /// 回调保持访问锁
        /// </summary>
        private int keepLock;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private int isDisposed;
        ///// <summary>
        ///// 保持回调序号
        ///// </summary>
        //private int identity;
        ///// <summary>
        ///// 客户端是否已经强制终止回调
        ///// </summary>
        //private int isCancel;
        ///// <summary>
        ///// 客户端命令
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="identity"></param>
        //internal KeepCallback(ClientCommand.Command command, int identity)
        //{
        //    this.command = command;
        //    this.identity = identity;
        //}
        /// <summary>
        /// 客户端命令
        /// </summary>
        /// <param name="command"></param>
        internal KeepCallback(ClientCommand.Command command)
        {
            this.command = command;
        }
        /// <summary>
        /// 设置命令会话标识
        /// </summary>
        /// <param name="commandIndex">命令会话标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool SetCommandIndex(int commandIndex)
        {
            while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallbackSetCommandIndex);
            if (isDisposed == 0)
            {
                this.commandIndex = commandIndex;
                System.Threading.Interlocked.Exchange(ref keepLock, 0);
                return true;
            }
            System.Threading.Interlocked.Exchange(ref keepLock, 0);
            return false;
        }
        /// <summary>
        /// 取消回调
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void BuildCancel()
        {
            isDisposed = 1;
        }
        ///// <summary>
        ///// 客户端强制终止回调（不通知服务端）
        ///// </summary>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void Cancel()
        //{
        //    if (Interlocked.CompareExchange(ref isCancel, 1, 0) == 0) command.Socket.FreeKeep(command);
        //}
        /// <summary>
        /// 客户端强制终止回调（不通知服务端）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Cancel()
        {
            if (isDisposed == 0)
            {
                while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallbackCancel);
                if (isDisposed == 0)
                {
                    isDisposed = 1;
                    System.Threading.Interlocked.Exchange(ref keepLock, 0);
                    if (commandIndex != 0) command.Socket.CommandPool.Cancel(commandIndex, command);
                }
                else System.Threading.Interlocked.Exchange(ref keepLock, 0);
            }
        }
        /// <summary>
        /// 通知服务端终止回调
        /// </summary>
        public void Dispose()
        {
            //command.CancelKeep(Interlocked.Exchange(ref this.identity, int.MinValue))
            if (isDisposed == 0)
            {
                while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallbackDispose);
                if (isDisposed == 0)
                {
                    isDisposed = 1;
                    System.Threading.Interlocked.Exchange(ref keepLock, 0);
                    if (commandIndex != 0) command.CancelKeep(commandIndex);
                }
                else System.Threading.Interlocked.Exchange(ref keepLock, 0);
            }
        }
    }
}
