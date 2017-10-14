using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 分组名字
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct GroupName
    {
        /// <summary>
        /// 分组名字（30个字符以内）
        /// </summary>
        public string name;
    }
}
