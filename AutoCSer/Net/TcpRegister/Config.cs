using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务配置
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// 服务注册心跳间隔秒数，默认为 0 表示不心跳
        /// </summary>
        public int CheckRegisterServerSeconds;
    }
}
