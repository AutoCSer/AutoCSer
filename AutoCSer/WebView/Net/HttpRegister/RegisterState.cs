using System;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// HTTP 服务启动状态
    /// </summary>
    public enum RegisterState : byte
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        Unknown,
        /// <summary>
        /// HTTP 注册管理服务已经关闭
        /// </summary>
        Disposed,
        /// <summary>
        /// 主机名称合法
        /// </summary>
        HostError,
        /// <summary>
        /// 域名不合法
        /// </summary>
        DomainError,
        /// <summary>
        /// 域名冲突
        /// </summary>
        DomainExists,
        /// <summary>
        /// 安全连接匹配错误
        /// </summary>
        SslMatchError,
        /// <summary>
        /// 服务创建失败
        /// </summary>
        CreateServerError,
        /// <summary>
        /// 安全证书获取失败
        /// </summary>
        CertificateError,
        /// <summary>
        /// 程序集文件未找到
        /// </summary>
        NotFoundAssembly,
        /// <summary>
        /// 服务启动失败
        /// </summary>
        StartError,
        /// <summary>
        /// TCP监听服务启动失败
        /// </summary>
        TcpError,
        /// <summary>
        /// 启动成功
        /// </summary>
        Success,
    }
}
