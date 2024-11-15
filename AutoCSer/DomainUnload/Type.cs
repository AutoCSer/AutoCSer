﻿using System;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 卸载处理类型
    /// </summary>
    internal enum Type : byte
    {
        None,
        /// <summary>
        /// 委托
        /// </summary>
        Action,
        /// <summary>
        /// 释放文件日志
        /// </summary>
        LogFileDispose,
        /// <summary>
        /// 释放 TCP 组件基类
        /// </summary>
        TcpCommandBaseDispose,
    }
}
