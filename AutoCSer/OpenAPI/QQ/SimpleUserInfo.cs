using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 登录用户在QQ空间的简单个人信息
    /// </summary>
    public sealed class SimpleUserInfo : UserInfoBase
    {
        /// <summary>
        /// 大小为40×40像素的QQ头像URL
        /// </summary>
        public string figureurl_qq_1;
        /// <summary>
        /// 大小为100×100像素的QQ头像URL
        /// </summary>
        public string figureurl_qq_2;
        /// <summary>
        /// 标识用户是否为黄钻用户（0：不是；1：是）。
        /// </summary>
        public string is_yellow_vip;
    }
}
