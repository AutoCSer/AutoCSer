using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using AutoCSer.Log;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// HTTP 服务器配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// HTTP 服务程序集运行目录
        /// </summary>
        public string WorkPath = AutoCSer.Config.Pub.Default.CachePath;
        /// <summary>
        /// TCP 服务端口证书集合
        /// </summary>
        public SslCertificate[] Certificates;
        /// <summary>
        /// 获取安全证书
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="domainString"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        internal SslCertificate GetCertificate(Domain domain, string domainString, ILog log)
        {
            if (Certificates != null)
            {
                foreach (SslCertificate certificate in Certificates)
                {
                    if (string.Compare(domainString, certificate.Domain, true) == 0)
                    {
                        return certificate.Load(log) ? certificate : null;
                    }
                }
            }
            return null;
        }
    }
}
