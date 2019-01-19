using System;

namespace AutoCSer.Net.Http.ServerNameIndication
{
    /// <summary>
    /// 握手阶段 Byte[2]
    /// </summary>
    internal enum HandshakeType : byte
    {
        /// <summary>
        /// Hello 请求
        /// </summary>
        HelloRequest = 0,
        /// <summary>
        /// 客户端 Hello
        /// </summary>
        ClientHello = 1,
        /// <summary>
        /// 服务端 Hello
        /// </summary>
        ServerHello = 2,
        /// <summary>
        /// 证书
        /// </summary>
        Certificate = 11,
        /// <summary>
        /// 服务器密钥交换
        /// </summary>
        ServerKeyExchange = 12,
        /// <summary>
        /// 证书申请
        /// </summary>
        CertificateRequest = 13,
        /// <summary>
        /// 服务器 Hello 完成
        /// </summary>
        ServerHelloDone = 14,
        /// <summary>
        /// 证书验证
        /// </summary>
        CertificateVerify = 15,
        /// <summary>
        /// 客户密钥交换
        /// </summary>
        ClientKeyExchange = 16,
        /// <summary>
        /// 结束
        /// </summary>
        Finished = 20,
    }
}
