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
        /// 回调保持访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock keepLock;
        /// <summary>
        /// 命令会话标识
        /// </summary>
        private int commandIndex;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// TCP 调用客户端回调保持
        /// </summary>
        /// <param name="command">客户端命令</param>
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
            keepLock.EnterYield();
            if (isDisposed == 0)
            {
                this.commandIndex = commandIndex;
                keepLock.Exit();
                return true;
            }
            keepLock.Exit();
            return false;
        }
        /// <summary>
        /// 取消回调
        /// </summary>
        internal void BuildCancel()
        {
            isDisposed = 1;
        }
        /// <summary>
        /// 客户端强制终止回调（不通知服务端）
        /// </summary>
        internal void Cancel()
        {
            if (isDisposed == 0)
            {
                keepLock.EnterYield();
                if (isDisposed == 0)
                {
                    isDisposed = 1;
                    keepLock.Exit();
                    if (commandIndex != 0) command.Socket.CommandPool.CancelKeep(commandIndex, command);
                }
                else keepLock.Exit();
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
                keepLock.EnterYield();
                if (isDisposed == 0)
                {
                    isDisposed = 1;
                    keepLock.Exit();
                    if (commandIndex != 0) command.CancelKeep(commandIndex);
                }
                else keepLock.Exit();
            }
        }
    }
}
