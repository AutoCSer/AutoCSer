using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpInternalServer.Emit.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ReturnParameterGenericType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 输出参数泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public Emit.ReturnParameterGenericType Value;
    }
}
