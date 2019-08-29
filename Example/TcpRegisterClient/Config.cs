using System;

namespace AutoCSer.Example.TcpRegisterClient
{
    /// <summary>
    /// 测试服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// TCP 内部注册服务 标识配置名称
        /// </summary>
        internal const string TcpRegisterConfigName = "127.0.0.1";
        /// <summary>
        /// TCP 内部注册写服务 TCP 服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = TcpRegisterConfigName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute TcpRegisterServerAttribute
        {
            get
            {
                AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.Net.TcpRegister.Server), false);
                attribute.VerifyString = "2";
                return attribute;
            }
        }
    }
}
