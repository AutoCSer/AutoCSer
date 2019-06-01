using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpOpenServer.Emit
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal partial struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 输出参数泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public ReturnParameterGenericType ReturnParameterGenericType;
        /// <summary>
        /// 输入+输出参数泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public ParameterGenericType ParameterGenericType;
        /// <summary>
        /// 输入+输出参数泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public ParameterGenericType2 ParameterGenericType2;
    }
}
