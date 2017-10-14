using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 获取令牌参数
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct GetParameter
    {
        /// <summary>
        /// 注册邮箱
        /// </summary>
        public string email;
        /// <summary>
        /// 用户密码
        /// </summary>
        public string password;
        /// <summary>
        /// 随机前缀
        /// </summary>
        public string randomPrefix;
        /// <summary>
        /// 令牌超时
        /// </summary>
        public int timeoutSeconds;
        /// <summary>
        /// 获取令牌参数
        /// </summary>
        /// <param name="email">注册邮箱</param>
        /// <param name="password">用户密码</param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="timeoutSeconds">令牌超时</param>
        internal GetParameter(string email, string password, string randomPrefix, int timeoutSeconds)
        {
            this.email = email;
            this.randomPrefix = randomPrefix ?? AutoCSer.Random.Default.NextULong().toHex();
            this.timeoutSeconds = timeoutSeconds;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                this.password = md5.ComputeHash(Encoding.Unicode.GetBytes(this.randomPrefix + md5.ComputeHash(Encoding.Unicode.GetBytes(password)).toLowerHex())).toLowerHex();
            }
        }
    }
}
