using System;

namespace AutoCSer.Example.TcpRegisterServer
{
    /// <summary>
    /// 测试服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// TCP 内部注册读取服务 TCP 服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Net.TcpRegister.ReaderServer.ServerName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute TcpRegisterReaderServerAttribute
        {
            get
            {
                AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.Net.TcpRegister.ReaderServer), false);
                attribute.VerifyString = "1";
                return attribute;
            }
        }
        /// <summary>
        /// TCP 内部注册写服务 TCP 服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Net.TcpRegister.Server.ServerName)]
        internal static AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute TcpRegisterServerAttribute
        {
            get
            {
                AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute>(typeof(AutoCSer.Net.TcpRegister.Server), false);
                attribute.VerifyString = "2";
                return attribute;
            }
        }
        /// <summary>
        /// TCP 内部注册服务配置
        /// </summary>
        [AutoCSer.Config.Member]
        internal static AutoCSer.Net.TcpRegister.Config TcpRegisterConfig
        {
            get
            {
                return new AutoCSer.Net.TcpRegister.Config { PortStart = 10000, PortEnd = 20000 };
            }
        }
    }
}
