using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using AutoCSer.Log;
using AutoCSer.Extension;
using AutoCSer.Net.Http;
using System.Net.Security;
using System.Net.Sockets;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// TCP 服务端口证书信息
    /// </summary>
    public sealed class SslCertificate
    {
        /// <summary>
        /// 域名信息
        /// </summary>
        public string Domain;
        /// <summary>
                                     /// TCP服务端口信息
                                     /// </summary>
        public HostPort Host;
        /// <summary>
        /// 安全证书文件
        /// </summary>
        public string FileName;
#pragma warning disable 649
        /// <summary>
        /// 安全证书文件密码
        /// </summary>
        public string Password;
        /// <summary>
        /// 是否验证客户端证书
        /// </summary>
        public bool IsClientCertificateRequired;
        /// <summary>
        /// 是否检查证书吊销列表
        /// </summary>
        public bool IsCheckCertificateRevocation;
        /// <summary>
        /// 协议
        /// </summary>
        internal SslProtocols Protocol;
#pragma warning restore 649
        /// <summary>
        /// 安全证书 
        /// </summary>
        internal X509Certificate Certificate;
        /// <summary>
        /// 加载安全证书
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        internal bool Load(ILog log)
        {
            if (Certificate == null)
            {
                if (FileName != null)
                {
                    if (System.IO.File.Exists(FileName))
                    {
                        try
                        {
                            Certificate = Password == null ? X509Certificate2.CreateFromCertFile(FileName) : new X509Certificate2(System.IO.File.ReadAllBytes(FileName), Password);
                            if (Protocol == SslProtocols.None) Protocol = Http.Config.SslProtocol;
                            return true;
                        }
                        catch (Exception error)
                        {
                            (log ?? AutoCSer.Log.Pub.Log).Add(AutoCSer.Log.LogType.Error, error);
                        }
                    }
                    else (log ?? AutoCSer.Log.Pub.Log).Add(AutoCSer.Log.LogType.Error, "没有找到安全证书文件 [" + Host.Host + ":" + Host.Port.toString() + "] " + FileName);
                }
            }
            return Certificate != null;
        }
        /// <summary>
        /// 创建安全连接
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        internal SslStream CreateSslStream(SslSocket socket)
        {
            SslStream sslStream = new SslStream(socket.NetworkStream = new NetworkStream(socket.Socket, true), false);
            sslStream.BeginAuthenticateAsServer(Certificate, IsClientCertificateRequired, Protocol, IsCheckCertificateRevocation, socket.AuthenticateCallback, socket);
            return sslStream;
        }
        /// <summary>
        /// 创建安全连接
        /// </summary>
        /// <param name="helloStream"></param>
        /// <returns></returns>
        internal SslStream CreateSslStream(Http.ServerNameIndication.HelloStream helloStream)
        {
            SslStream sslStream = new SslStream(helloStream, false);
            sslStream.BeginAuthenticateAsServer(Certificate, IsClientCertificateRequired, Protocol, IsCheckCertificateRevocation, helloStream.Socket.AuthenticateCallback, helloStream);
            return sslStream;
        }
    }
}
