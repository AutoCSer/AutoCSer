using System;
using System.Runtime.InteropServices;

namespace AutoCSer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Action
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 委托
        /// </summary>
        [FieldOffset(0)]
        public System.Action Value;
    }
}
