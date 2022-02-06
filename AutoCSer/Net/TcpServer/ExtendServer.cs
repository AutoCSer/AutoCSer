using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 扩展服务
    /// </summary>
    internal abstract unsafe class ExtendServer
    {
        /// <summary>
        /// TCP 服务基类
        /// </summary>
        private readonly ServerBase server;
        /// <summary>
        /// 扩展服务名称
        /// </summary>
        private readonly string name;
        /// <summary>
        /// 命令位图
        /// </summary>
        private AutoCSer.Memory.Pointer commandData;
        /// <summary>
        /// 命令位图
        /// </summary>
        private MemoryMap commands;
        /// <summary>
        /// 最大命令
        /// </summary>
        private readonly int maxCommand;
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            Unmanaged.Free(ref commandData);
        }
        /// <summary>
        /// 判断命令是否有效
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool IsCommand(int index)
        {
            if ((uint)index <= (uint)maxCommand)
            {
                if (commands.Get(index) != 0)
                {
                    if (commandData.Data == null) return false;
                    return true;
                }
                if (commandData.Data == null) return false;
            }
            if (server.Log.IsAnyLevel(LogLevel.Info)) server.Log.Info(server.ServerAttribute.ServerName + "." + name + " 缺少命令处理委托 [" + index.toString() + "]", LogLevel.Info | LogLevel.AutoCSer);
            return false;
        }
    }
}
