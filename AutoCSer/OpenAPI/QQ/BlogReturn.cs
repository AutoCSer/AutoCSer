using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 空间日志地址
    /// </summary>
    public sealed class BlogReturn : Return
    {
        /// <summary>
        /// 空间日志ID
        /// </summary>
        public int blogid;
        /// <summary>
        /// 固定为http://i.qq.com。用户在登录态下，可以通过该URL直接进入空间首页（这里不直接返回新发表日志的URL是为了避免泄漏用户信息）
        /// </summary>
        public string url;
    }
}
