using System;
using System.Text;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.QQ
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
#pragma warning disable 649
        /// <summary>
        /// 申请接入时注册的网站名称
        /// </summary>
        internal string site;
        /// <summary>
        /// appid
        /// </summary>
        internal string client_id;
        /// <summary>
        /// appkey
        /// </summary>
        private string client_secret;
        /// <summary>
        /// 登陆成功回调地址
        /// </summary>
        private string redirect_uri;
#pragma warning restore 649
        /// <summary>
        /// URL编码 登陆成功回调地址
        /// </summary>
        private string encodeRedirectUri;
        /// <summary>
        /// URL编码 登陆成功回调地址
        /// </summary>
        internal string EncodeRedirectUri
        {
            get
            {
                if (encodeRedirectUri == null) encodeRedirectUri = AutoCSer.Net.WebClient.UriCreator.Encode(redirect_uri);
                return encodeRedirectUri;
            }
        }
        /// <summary>
        /// 访问令牌 查询字符串
        /// </summary>
        private const string access_token = "access_token=";
        /// <summary>
        /// 有效期，单位为秒 查询字符串
        /// </summary>
        private const string expires_in = "expires_in=";
        /// <summary>
        /// 获取一个新令牌
        /// </summary>
        /// <param name="code">authorization_code</param>
        /// <returns>一个新令牌</returns>
        private Token getToken(string code)
        {
            string data = Client.RequestForm("https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id=" + client_id + "&client_secret=" + client_secret + "&code=" + code + "&redirect_uri=" + EncodeRedirectUri);
            Token token = new Token();
            if (data != null)
            {
                foreach (SubString query in data.split('&'))
                {
                    if (query.StartsWith(access_token))
                    {
                        query.MoveStart(access_token.Length);
                        token.access_token = query;
                    }
                    else if (query.StartsWith(expires_in))
                    {
                        query.MoveStart(expires_in.Length);
                        int.TryParse(query, out token.expires_in);
                    }
                }
                if (!token.IsToken) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, data);
            }
            return token;
        }
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="code">authorization_code</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApi(string code)
        {
            if (string.IsNullOrEmpty(site)) AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, "网站名称不能为空");
            else
            {
                Token token = getToken(code);
                OpenId openId = token.GetOpenId();
                if (openId.openid != null) return new API(this, ref token, ref openId);
            }
            return null;
        }
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="tokenOpenId">访问令牌+用户身份的标识</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApi(TokenOpenId tokenOpenId)
        {
            return GetApi(ref tokenOpenId);
        }
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="tokenOpenId">访问令牌+用户身份的标识</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApi(ref TokenOpenId tokenOpenId)
        {
            if (string.IsNullOrEmpty(tokenOpenId.Token) || string.IsNullOrEmpty(tokenOpenId.OpenId)) return null;
            Token token = new Token { access_token = tokenOpenId.Token, expires_in = -1 };
            OpenId openId = new OpenId { openid = tokenOpenId.OpenId, client_id = client_id };
            return new API(this, ref token, ref openId);
        }
        /// <summary>
        /// 获取api调用
        /// </summary>
        /// <param name="tokenOpenId">访问令牌+用户身份的标识</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApiByJson(string tokenOpenId)
        {
            TokenOpenId value = new TokenOpenId();
            return AutoCSer.Json.Parser.Parse(tokenOpenId, ref value) ? GetApi(value) : null;
        }
        /// <summary>
        /// 默认配置
        /// </summary>
        public static readonly Config Default = AutoCSer.Config.Loader.Get<Config>() ?? new Config();
    }
}
