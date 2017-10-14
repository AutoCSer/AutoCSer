using System;
using System.Text;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// 编码绑定请求
        /// </summary>
        internal static readonly EncodingClient Client = new EncodingClient(WebClient.Default, Encoding.UTF8);
#pragma warning disable
        /// <summary>
        /// 调用域名地址
        /// </summary>
        internal string Domain = "http://www.51nod.com/";
        /// <summary>
        /// 注册邮箱
        /// </summary>
        private string email;
        /// <summary>
        /// 用户密码
        /// </summary>
        private string password;
        /// <summary>
        /// 随机前缀
        /// </summary>
        private string randomPrefix;
        /// <summary>
        /// 令牌超时
        /// </summary>
        private int timeoutSeconds = 60 * 60;
        /// <summary>
        /// 获取令牌超时
        /// </summary>
        private int getTokenTimeoutSeconds;
#pragma warning restore
        /// <summary>
        /// 令牌超时
        /// </summary>
        private long timeoutTicks;
        /// <summary>
        /// 应用配置
        /// </summary>
        public Config() { }
        /// <summary>
        /// 测试应用配置
        /// </summary>
        /// <param name="email">注册邮箱</param>
        /// <param name="password">用户密码</param>
        /// <param name="domain">调用域名地址</param>
        public Config(string email, string password, string domain = "http://www.51nod.com/")
        {
            this.email = email;
            this.password = password;
            Domain = domain;
        }
        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        internal string GetToken(out long timeout)
        {
            if (timeoutTicks == 0)
            {
                if (timeoutSeconds <= 0) timeoutSeconds = 60;
                timeoutTicks = (long)timeoutSeconds * Date.SecondTicks;
                getTokenTimeoutSeconds = timeoutSeconds + Math.Min(timeoutSeconds, 10 * 60);
                if (getTokenTimeoutSeconds < 0) getTokenTimeoutSeconds = int.MaxValue;
            }
            timeout = (Date.NowTime.UtcNow.Ticks / timeoutTicks) * timeoutTicks + timeoutTicks;
            ReturnValue<string> value = Client.RequestJson<ReturnValue<string>, GetParameter>(Domain + "ajax?n=pub.GetToken", new GetParameter(email, password, randomPrefix, getTokenTimeoutSeconds));
            return value == null ? null : value.Value;
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        public static readonly Config Default = AutoCSer.Config.Loader.Get<Config>() ?? new Config();
    }
}
