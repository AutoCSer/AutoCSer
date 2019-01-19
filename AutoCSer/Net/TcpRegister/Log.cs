using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务更新日志
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public sealed class Log
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        internal LogType Type;
        /// <summary>
        /// TCP 服务注册信息
        /// </summary>
        internal ServerInfo Server;
        /// <summary>
        /// 客户端错误
        /// </summary>
        internal static readonly Log ClientError = new Log { Type = LogType.ClientError };
        /// <summary>
        /// 注册服务加载完毕
        /// </summary>
        internal static readonly Log RegisterLoaded = new Log { Type = LogType.RegisterLoaded };
    }
}
