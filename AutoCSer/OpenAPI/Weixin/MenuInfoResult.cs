using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 自定义菜单配置
    /// </summary>
    internal sealed class MenuInfoResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 菜单是否开启，0代表未开启，1代表开启
        /// </summary>
        public byte is_menu_open;
        /// <summary>
        /// 菜单信息
        /// </summary>
        public MenuQeury selfmenu_info;
#pragma warning restore
    }
}
