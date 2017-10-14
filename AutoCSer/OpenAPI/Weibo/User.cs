using System;

namespace AutoCSer.OpenAPI.Weibo
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public sealed class User : IReturn
    {
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return id != 0 && !string.IsNullOrEmpty(screen_name); }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return null; }
        }
        /// <summary>
        /// 用户UID
        /// </summary>
        public long id;
        /// <summary>
        /// 字符串型的用户UID
        /// </summary>
        public string idstr;
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string screen_name;
        /// <summary>
        /// 友好显示名称
        /// </summary>
        public string name;
        /// <summary>
        /// 用户所在省级ID
        /// </summary>
        public string province;
        /// <summary>
        /// 用户所在城市ID
        /// </summary>
        public string city;
        /// <summary>
        /// 用户所在地
        /// </summary>
        public string location;
        /// <summary>
        /// 用户个人描述
        /// </summary>
        public string description;
        /// <summary>
        /// 用户博客地址
        /// </summary>
        public string url;
        /// <summary>
        /// 用户头像地址，50×50像素
        /// </summary>
        public string profile_image_url;
        /// <summary>
        /// 用户的微博统一URL地址
        /// </summary>
        public string profile_url;
        /// <summary>
        /// 用户的个性化域名
        /// </summary>
        public string domain;
        /// <summary>
        /// 用户的微号
        /// </summary>
        public string weihao;
        /// <summary>
        /// 性别，m：男、f：女、n：未知
        /// </summary>
        public string gender;
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int followers_count;
        /// <summary>
        /// 关注数
        /// </summary>
        public int friends_count;
        /// <summary>
        /// 微博数
        /// </summary>
        public int statuses_count;
        /// <summary>
        /// 收藏数
        /// </summary>
        public int favourites_count;
        /// <summary>
        /// 用户创建（注册）时间
        /// </summary>
        public string created_at;
        /// <summary>
        /// 暂未支持
        /// </summary>
        public bool following;
        /// <summary>
        /// 是否允许所有人给我发私信，true：是，false：否
        /// </summary>
        public bool allow_all_act_msg;
        /// <summary>
        /// 是否允许标识用户的地理位置，true：是，false：否
        /// </summary>
        public bool geo_enabled;
        /// <summary>
        /// 是否是微博认证用户，即加V用户，true：是，false：否
        /// </summary>
        public bool verified;
        /// <summary>
        /// 暂未支持
        /// </summary>
        public int verified_type;
        /// <summary>
        /// 用户备注信息，只有在查询用户关系时才返回此字段
        /// </summary>
        public string remark;
        /// <summary>
        /// 用户的最近一条微博信息字段
        /// </summary>
        public Status status;
        /// <summary>
        /// 是否允许所有人对我的微博进行评论，true：是，false：否
        /// </summary>
        public bool allow_all_comment;
        /// <summary>
        /// 用户大头像地址
        /// </summary>
        public string avatar_large;
        /// <summary>
        /// 认证原因
        /// </summary>
        public string verified_reason;
        /// <summary>
        /// 该用户是否关注当前登录用户，true：是，false：否
        /// </summary>
        public bool follow_me;
        /// <summary>
        /// 用户的在线状态，0：不在线、1：在线
        /// </summary>
        public int online_status;
        /// <summary>
        /// 用户的互粉数
        /// </summary>
        public int bi_followers_count;
        /// <summary>
        /// 用户当前的语言版本，zh-cn：简体中文，zh-tw：繁体中文，en：英语
        /// </summary>
        public string lang;
        //public int star;
        //public int mbtype;
        //public int mbrank;
        //public int block_word;
    }
}
