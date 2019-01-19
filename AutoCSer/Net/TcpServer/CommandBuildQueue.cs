using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 命令队列
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal sealed class CommandBuildQueue : ClientCommand.Command.SingleReadQueue
    {
        /// <summary>
        /// 命令队列
        /// </summary>
        /// <param name="command">空闲命令</param>
        internal CommandBuildQueue(ClientCommand.Command command) : base(command) { }
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns>是否正在等待读取</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Push(ClientCommand.Command command)
        {
            //command.LinkNext = null;
            AutoCSer.Threading.Interlocked.CompareExchangeYieldOnly(ref pushLock);
            int isReadWait = this.isReadWait;
            end.LinkNext = command;
            end = command;
            this.isReadWait = 0;
            pushLock = 0;
            return isReadWait;
        }
        /// <summary>
        /// 设置等待读取
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte TrySetWait()
        {
            if (Head.LinkNext == null)
            {
                AutoCSer.Threading.Interlocked.CompareExchangeYieldOnly(ref pushLock);
                if (Head.LinkNext == null)
                {
                    isReadWait = 1;
                    pushLock = 0;
                    return 1;
                }
                pushLock = 0;
            }
            return 0;
        }
        /// <summary>
        /// 弹出命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ClientCommand.Command Pop()
        {
            ClientCommand.Command value = Head.TryFreeBuildQueue();
            if (value != null)
            {
                Head = value;
                return value;
            }
            return null;
        }
    }
}
