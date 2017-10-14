using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 登录用户在QQ空间的信息
    /// </summary>
    public abstract class UserInfoBase : Return
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;
        /// <summary>
        /// 大小为30×30像素的头像URL
        /// </summary>
        public string figureurl;
        /// <summary>
        /// 大小为50×50像素的头像URL
        /// </summary>
        public string figureurl_1;
        /// <summary>
        /// 大小为100×100像素的头像URL
        /// </summary>
        public string figureurl_2;
    }
}
