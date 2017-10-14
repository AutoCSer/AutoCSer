using System;

namespace AutoCSer.OpenAPI.Weibo
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    internal sealed class Token : IReturn
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token;
        /// <summary>
        /// 当前授权用户的UID
        /// </summary>
        public string uid;
        /// <summary>
        /// 生命周期，单位是秒数
        /// </summary>
        public int expires_in;
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return expires_in != 0 && !string.IsNullOrEmpty(access_token) && !string.IsNullOrEmpty(uid); }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return null; }
        }
    }
}
