using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Email
{
    /// <summary>
    /// SMTP信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Smtp
    {
        /// <summary>
        /// 默认SMTP服务端口
        /// </summary>
        public const int DefaultServerPort = 25;

        /// <summary>
        /// 发件SMTP,如"smtp.163.com"
        /// </summary>
        public string Server;
        /// <summary>
        /// SMTP服务端口
        /// </summary>
        public int Port;
        /// <summary>
        /// 是否SSL
        /// </summary>
        public bool IsSsl;
    }
}
