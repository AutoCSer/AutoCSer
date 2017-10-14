using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 自定义菜单
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct MenuQeury
    {
        /// <summary>
        /// 自定义菜单
        /// </summary>
        public Menu[] button;
    }
}
