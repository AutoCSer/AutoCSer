﻿using System;
using System.Threading.Tasks;
using AutoCSer.Extensions;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程复制重启服务客户端
    /// </summary>
    public static partial class ProcessCopyClient
    {
        /// <summary>
        /// 守护进程客户端调用
        /// </summary>
        public static async void GuardAsync()
        {
            isGuard = true;
#if NoAutoCSer
            throw new Exception();
#else
            try
            {
                if ((await client.guardAwaiter(ProcessCopyer.Default)).Type == AutoCSer.Net.TcpServer.ReturnType.Success) return;
            }
            catch (Exception error)
            {
                AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
            AutoCSer.LogHelper.Error("守护进程客户端调用失败", LogLevel.Error | LogLevel.AutoCSer);
            callGuardTask();
#endif
        }
        /// <summary>
        /// 守护进程删除客户端调用
        /// </summary>
        public static async void RemoveAsync()
        {
            isGuard = false;
#if NoAutoCSer
            throw new Exception();
#else
            try
            {
                await client.removeAwaiter(ProcessCopyer.Default);
            }
            catch (Exception error)
            {
                AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
#endif
        }
        /// <summary>
        /// 进程复制重启客户端调用
        /// </summary>
        /// <returns>是否成功</returns>
        internal static async Task<bool> CopyAsync()
        {
#if NoAutoCSer
            throw new Exception();
#else
            try
            {
                await client.copyAwaiter(ProcessCopyer.Default);
                return true;
            }
            catch (Exception error)
            {
                AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
#endif
            return false;
        }
    }
}
