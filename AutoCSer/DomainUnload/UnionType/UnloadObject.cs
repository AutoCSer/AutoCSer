using System;
using System.Runtime.InteropServices;

namespace AutoCSer.DomainUnload.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnloadObject
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 应用程序卸载处理委托回调
        /// </summary>
        [FieldOffset(0)]
        public AutoCSer.DomainUnload.UnloadObject Value;
    }
}
