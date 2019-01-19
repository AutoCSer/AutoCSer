using System;
using AutoCSer.Extension;
using System.Threading;
using AutoCSer.Net.HttpRegister;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 基于安全连接的 HTTP 服务
    /// </summary>
    internal sealed class SslServer : Server
    {
        /// <summary>
        /// TCP 服务端口证书信息
        /// </summary>
        private SslCertificate certificate;
        /// <summary>
        /// TCP 服务端口证书绑定域名
        /// </summary>
        private HashBytes certificateDomain;
        /// <summary>
        /// TCP 服务端口证书信息集合
        /// </summary>
        private Dictionary<HashBytes, SslCertificate> certificates;
        /// <summary>
        /// TCP 服务端口证书信息访问锁
        /// </summary>
        private readonly object certificateLock = new object();
        /// <summary>
        /// 是否存在证书
        /// </summary>
        internal bool IsCertificate
        {
            get { return certificate != null || certificates != null; }
        }
        /// <summary>
        /// 获取单证书
        /// </summary>
        internal SslCertificate Certificate
        {
            get { return certificates == null ? certificate : null; }
        }
        /// <summary>
        /// 基于安全连接的 HTTP 服务
        /// </summary>
        /// <param name="server">HTTP 注册管理服务</param>
        /// <param name="domain">域名信息</param>
        internal SslServer(HttpRegister.Server server, Domain domain)
            : base(server, ref domain.SslHost, true, false)
        {
            if (SetCertificate(domain)) start();
        }
        /// <summary>
        /// 设置证书信息
        /// </summary>
        /// <param name="domain">域名信息</param>
        /// <returns></returns>
        internal bool SetCertificate(Domain domain)
        {
            string domainString = domain.DomainData.toStringNotNull();
            SslCertificate sslCertificate = HttpRegister.Server.Config.GetCertificate(domain, domainString, RegisterServer.TcpServer.Log);
            if (sslCertificate == null)
            {
                RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, "安全证书获取失败 " + domainString);
                return false;
            }
            HashBytes domainKey;
            int portIndex = domainString.IndexOf(':');
            if (portIndex != -1)
            {
                domainString = domainString.Substring(0, portIndex);
                domainKey = domainString.getBytes();
            }
            else domainKey = domain.DomainData;
            Monitor.Enter(certificateLock);
            try
            {
                if (certificate == null)
                {
                    certificate = sslCertificate;
                    certificateDomain = domainKey;
                }
                else
                {
                    certificates = DictionaryCreator.CreateHashBytes<SslCertificate>();
                    certificates.Add(certificateDomain, certificate);
                    certificates[domainKey] = sslCertificate;
                }
            }
            finally { Monitor.Exit(certificateLock); }
            return true;
        }
        /// <summary>
        /// 根据域名获取证书
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SslCertificate GetCertificate(ref SubArray<byte> domain)
        {
            SslCertificate certificate;
            return certificates.TryGetValue(domain, out certificate) ? certificate : null;
        }
        /// <summary>
        /// 套接字处理
        /// </summary>
        internal new void OnSocket()
        {
            while (this.socket != null)
            {
                socketHandle.Wait();
                SocketLink socket = Interlocked.Exchange(ref socketHead, null);
                do
                {
                    try
                    {
                        while (socket != null) socket = socket.Start(this);
                        break;
                    }
                    catch (Exception error)
                    {
                        RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Debug, error);
                    }
                    socket = socket.Cancel();
                }
                while (true);
            }
            socketHead = null;
        }
    }
}
