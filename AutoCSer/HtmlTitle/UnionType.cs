using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.HtmlTitle
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
        /// 域名 IP 转换配置
        /// </summary>
        [FieldOffset(0)]
        public DomainIPAddressConfig DomainIPAddressConfig;
    }
}
