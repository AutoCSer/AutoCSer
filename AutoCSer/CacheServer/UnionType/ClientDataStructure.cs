﻿using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ClientDataStructure
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 客户端数据结构定义信息
        /// </summary>
        [FieldOffset(0)]
        public CacheServer.ClientDataStructure Value;
    }
}
