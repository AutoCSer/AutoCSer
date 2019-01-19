using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ClientInfo
    {
        /// <summary>
        /// 索引编号
        /// </summary>
        internal int Identity;
        /// <summary>
        /// TCP 内部注册服务更新日志回调
        /// </summary>
        private Func<TcpServer.ReturnValue<Log>, bool> onLog;
        /// <summary>
        /// 设置 TCP 内部注册服务更新日志回调
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="onLog"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Set(int identity, Func<TcpServer.ReturnValue<Log>, bool> onLog)
        {
            if (Identity == identity)
            {
                this.onLog = onLog;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新日志回调
        /// </summary>
        /// <param name="log"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnLog(Log log)
        {
            if (onLog != null && !onLog(log))
            {
                ++Identity;
                onLog = null;
            }
        }
    }
}
