﻿using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.ValueData.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Guid
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// Guid 参数
        /// </summary>
        [FieldOffset(0)]
        public ValueData.Guid Value;
    }
}
