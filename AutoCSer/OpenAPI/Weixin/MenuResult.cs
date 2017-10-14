using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 获取自定义菜单
    /// </summary>
    internal sealed class MenuResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 自定义菜单
        /// </summary>
        public MenuQeury menu;
#pragma warning restore
    }
}
