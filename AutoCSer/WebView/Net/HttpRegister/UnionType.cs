using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// HTTP 服务相关参数
        /// </summary>
        [FieldOffset(0)]
        public Config Config;
        /// <summary>
        /// TCP 内部注册写服务配置
        /// </summary>
        [FieldOffset(0)]
        public Server ServerRegister;
    }
}
