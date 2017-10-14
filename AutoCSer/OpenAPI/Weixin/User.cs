using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public sealed class User : Return
    {
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public long subscribe_time;
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        /// </summary>
        public string unionid;
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;
        /// <summary>
        /// 所在城市
        /// </summary>
        public string city;
        /// <summary>
        /// 所在省份
        /// </summary>
        public string province;
        /// <summary>
        /// 所在国家
        /// </summary>
        public string country;
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string headimgurl;
        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        public string remark;
        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public int groupid;
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息
        /// </summary>
        public byte subscribe;
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public byte sex;
        /// <summary>
        /// 用户的语言
        /// </summary>
        public Language language;
    }
}
