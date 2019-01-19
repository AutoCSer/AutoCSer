using System;
using System.Runtime.InteropServices;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 应用程序卸载处理委托回调
        /// </summary>
        [FieldOffset(0)]
        public AutoCSer.DomainUnload.UnloadObject UnloadObject;
    }
}
