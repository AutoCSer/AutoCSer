using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 登录用户在QQ空间的信息
    /// </summary>
    public sealed class UserInfo : UserInfoBase
    {
        /// <summary>
        /// 性别。 如果获取不到则默认返回"男"
        /// </summary>
        public string gender;
        /// <summary>
        /// 标识用户是否为黄钻用户（0：不是；1：是）
        /// </summary>
        public string vip;
        /// <summary>
        /// 黄钻等级
        /// </summary>
        public string level;
        /// <summary>
        /// 标识是否为年费黄钻用户（0：不是； 1：是）
        /// </summary>
        public string is_yellow_year_vip;
    }
}
