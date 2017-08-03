using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ClientIdentity
    {
        /// <summary>
        /// 索引编号
        /// </summary>
        internal int Identity;
        /// <summary>
        /// 部署任务状态更新回调
        /// </summary>
        private Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> onLog;
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="identity"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear(int identity)
        {
            if (Identity == identity)
            {
                onLog = null;
                ++Identity;
            }
        }
        /// <summary>
        /// 设置部署任务状态更新回调
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="onLog"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Set(int identity, Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> onLog)
        {
            if (Identity == identity)
            {
                this.onLog = onLog;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取部署任务状态更新回调
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> GetLog(int identity)
        {
            return Identity == identity ? onLog : null;
        }
    }
}
