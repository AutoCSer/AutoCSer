using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// API调用http://wiki.opensns.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91API%E6%96%87%E6%A1%A3
    /// </summary>
    public class API
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        private Config config;
        /// <summary>
        /// 访问令牌
        /// </summary>
        private Token token;
        /// <summary>
        /// 用户身份的标识
        /// </summary>
        private OpenId openId;
        /// <summary>
        /// 用户身份的标识
        /// </summary>
        public string OpenId
        {
            get { return openId.openid; }
        }
        /// <summary>
        /// 访问令牌+用户身份的标识
        /// </summary>
        public TokenOpenId TokenOpenId
        {
            get
            {
                return new TokenOpenId { Token = token.access_token, OpenId = openId.openid };
            }
        }
        /// <summary>
        /// 请求字符串
        /// </summary>
        private string query;
        /// <summary>
        /// API调用
        /// </summary>
        /// <param name="config">应用配置</param>
        /// <param name="token">访问令牌</param>
        /// <param name="openId">用户身份的标识</param>
        internal API(Config config, ref Token token, ref OpenId openId)
        {
            this.config = config;
            this.token = token;
            this.openId = openId;
            query = "access_token=" + token.access_token + "&oauth_consumer_key=" + config.client_id + "&openid=" + openId.openid;
        }
        /// <summary>
        /// 登录用户在QQ空间的信息[仅网站]
        /// </summary>
        /// <returns>登录用户在QQ空间的信息,失败返回null</returns>
        public UserInfo GetUserInfo()
        {
            return Config.Client.Request<UserInfo>(@"https://graph.qq.com/user/get_user_info?" + query + "&format=json");
        }
        /// <summary>
        /// 登录用户在QQ空间的简单个人信息gde
        /// </summary>
        /// <returns>登录用户在QQ空间的简单个人信息,失败返回null</returns>
        public SimpleUserInfo GetSimpleUserInfo()
        {
            return Config.Client.Request<SimpleUserInfo>(@"https://graph.qq.com/user/get_simple_userinfo?" + query);
        }
        /// <summary>
        /// 表单提交
        /// </summary>
        /// <typeparam name="jsonType">json数据数据类型</typeparam>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="form">POST表单</param>
        /// <returns>数据对象,失败放回null</returns>
        private jsonType format<jsonType, formType>(string url, formType form)
            where jsonType : class, IReturn
            where formType : Format
        {
            form.format = "json";
            return this.form<jsonType, formType>(url, form);
        }
        /// <summary>
        /// 表单提交
        /// </summary>
        /// <typeparam name="jsonType">json数据数据类型</typeparam>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="form">POST表单</param>
        /// <returns>数据对象,失败放回null</returns>
        private jsonType form<jsonType, formType>(string url, formType form)
            where jsonType : class, IReturn
            where formType : Format
        {
            form.access_token = token.access_token;
            form.oauth_consumer_key = config.client_id;
            form.openid = openId.openid;
            return Config.Client.RequestForm<jsonType, formType>(url, form);
        }
        /// <summary>
        /// 发表一个网页分享，分享应用中的内容给好友
        /// </summary>
        /// <param name="value">网页分享</param>
        /// <returns>是否成功</returns>
        public bool AddShare(Feeds value)
        {
            value.site = config.site;
            return format<Return, Feeds>(@"https://graph.qq.com/share/add_share", value);
        }
        ///// <summary>
        ///// 发表日志到QQ空间
        ///// </summary>
        ///// <param name="value">空间日志</param>
        ///// <returns>空间日志地址,失败返回null</returns>
        //public BlogReturn AddBlog(Blog value)
        //{
        //    return form<BlogReturn, Blog, Blog.MemberMap>(@"https://graph.qq.com/blog/add_one_blog", value);
        //}
        ///// <summary>
        ///// 拉取第三方分享的评论列表
        ///// </summary>
        ///// <param name="value">评论列表请求</param>
        ///// <returns>第三方分享的评论列表</returns>
        //public LeftArray<Comment> GetComment(CommentQuery value)
        //{
        //    //需要xml解析
        //    value.oauth_consumer_key = config.client_id;
        //    //https://graph.qq.com/share/get_comment
        //    return default(LeftArray<Comment>);
        //}
        /// <summary>
        /// 发表一条微博信息（纯文本）到腾讯微博平台上
        /// </summary>
        /// <param name="value">微博信息</param>
        /// <returns>微博ID,失败返回null</returns>
        public string AddMicroblog(Microblog value)
        {
            return format<MicroblogReturn, Microblog>(@"https://graph.qq.com/t/add_t", value);
        }
    }
}
