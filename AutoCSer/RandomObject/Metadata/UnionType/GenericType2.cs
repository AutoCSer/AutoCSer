﻿using System;
using System.Runtime.InteropServices;

namespace AutoCSer.RandomObject.Metadata.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct GenericType2
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public Metadata.GenericType2 Value;
    }
}
