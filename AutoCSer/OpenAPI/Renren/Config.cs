using System;
using System.Collections.Specialized;
using System.Text;

namespace AutoCSer.OpenAPI.Renren
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
        /// 默认空表单
        /// </summary>
        private static readonly NameValueCollection defaultForm = new NameValueCollection();
#pragma warning disable 649
        /// <summary>
        /// appid
        /// </summary>
        private string client_id;
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
        /// 获取api调用
        /// </summary>
        /// <param name="code">authorization_code</param>
        /// <returns>API调用,失败返回null</returns>
        public API GetApi(string code)
        {
            Token token = Client.RequestForm<Token>("https://graph.renren.com/oauth/token?grant_type=authorization_code&client_id=" + client_id + "&redirect_uri=" + EncodeRedirectUri + "&client_secret=" + client_secret + "&code=" + code, defaultForm);
            if (token != null) return new API(this, token);
            return null;
        }
        /// <summary>
        /// 默认配置
        /// </summary>
        public static readonly Config Default = AutoCSer.Config.Loader.Get<Config>() ?? new Config();
    }
}
