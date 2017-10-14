using System;
using System.Text;

namespace AutoCSer.OpenAPI.Weibo
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 编码绑定请求
        /// </summary>
        internal static readonly EncodingClient Client = new EncodingClient(WebClient.Default, Encoding.UTF8);
        /// <summary>
        /// appid
        /// </summary>
        protected string client_id;
        /// <summary>
        /// appkey
        /// </summary>
        protected string client_secret;
        /// <summary>
        /// 登陆成功回调地址
        /// </summary>
        protected string redirect_uri;
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="code">authorization_code</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApi(string code)
        {
            Token token = Client.RequestForm<Token, TokenRequest>(@"https://api.weibo.com/oauth2/access_token", new TokenRequest
            {
                client_id = client_id,
                client_secret = client_secret,
                redirect_uri = redirect_uri,
                code = code
            });
            return token != null ? new API(this, token) : null;
        }
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="tokenUid">访问令牌+用户身份的标识</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApi(TokenUid tokenUid)
        {
            return GetApi(ref tokenUid);
        }
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="tokenUid">访问令牌+用户身份的标识</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApi(ref TokenUid tokenUid)
        {
            if (string.IsNullOrEmpty(tokenUid.Token) || string.IsNullOrEmpty(tokenUid.Uid)) return null;
            return new API(this, new Token { access_token = tokenUid.Token, uid = tokenUid.Uid, expires_in = -1 });
        }
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="tokenOpenId">访问令牌+用户身份的标识</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApiByJson(string tokenOpenId)
        {
            TokenUid value = new TokenUid();
            return AutoCSer.Json.Parser.Parse(tokenOpenId, ref value) ? GetApi(value) : null;
        }
        /// <summary>
        /// 默认配置
        /// </summary>
        public static readonly Config Default = AutoCSer.Config.Loader.Get<Config>() ?? new Config();
    }
}
