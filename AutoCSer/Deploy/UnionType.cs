using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Deploy
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
        /// 部署服务客户端配置
        /// </summary>
        [FieldOffset(0)]
        public ClientConfig ClientConfig;
    }
}
