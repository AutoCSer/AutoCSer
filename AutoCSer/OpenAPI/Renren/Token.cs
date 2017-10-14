using System;

namespace AutoCSer.OpenAPI.Renren
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    internal sealed class Token : IReturn
    {
#pragma warning disable
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token;
        /// <summary>
        /// 访问令牌用户
        /// </summary>
        public TokenUser user;
        /// <summary>
        /// 刷新访问令牌
        /// </summary>
        public string refresh_token;
        /// <summary>
        /// 生命周期，单位是秒数
        /// </summary>
        public int expires_in;
        /// <summary>
        /// 访问范围
        /// </summary>
        public string scope;
        /// <summary>
        /// 错误码
        /// </summary>
        public string error;
        /// <summary>
        /// 一段人类可读的文字，用来帮助理解和解决发生的错误
        /// </summary>
        public string error_description;
        /// <summary>
        /// 一个人类可读的网页URI，带有关于错误的信息，用来为终端用户提供与错误有关的额外信息
        /// </summary>
        public string error_uri;
#pragma warning restore
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return !string.IsNullOrEmpty(access_token) && expires_in != 0 && user.id != 0; }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get
            {
                return error + " | " + error_uri + @"
" + error_description;
            }
        }
    }
}
