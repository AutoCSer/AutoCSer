using System;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 代码生成项目配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// 日志配置
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.Log.Config LogConfig
        {
            //get { return new Log.Config { Type = AutoCSer.Log.LogType.All }; }
            get { return new Log.Config { Type = 0 }; }
        }
#if !NoAutoCSer
        /// <summary>
        /// TCP 静态调用客户端参数
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.TestCase.TcpStaticClient.SessionServer.ClientConfig SessionClientConfig
        {
            get { return new TcpStaticClient.SessionServer.ClientConfig { VerifyMethod = TcpStaticServer.Session.Verify }; }
        }
        /// <summary>
        /// TCP 静态调用客户端参数
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.TestCase.TcpStaticStreamClient.StreamSessionServer.ClientConfig StreamSessionClientConfig
        {
            get { return new TcpStaticStreamClient.StreamSessionServer.ClientConfig { VerifyMethod = TcpStaticStreamServer.Session.Verify }; }
        }
        /// <summary>
        /// TCP 静态调用客户端参数
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.TestCase.TcpStaticSimpleClient.SimpleSessionServer.ClientConfig SimpleSessionClientConfig
        {
            get { return new TcpStaticSimpleClient.SimpleSessionServer.ClientConfig { VerifyMethod = TcpStaticSimpleServer.Session.Verify }; }
        }
        /// <summary>
        /// 文件块服务 TCP 静态服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.DiskBlock.Server.ServerName)]
        public static AutoCSer.Net.TcpInternalServer.ServerAttribute FileBlockConfig
        {
            get
            {
                AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.DiskBlock.Server), false);
                attribute.VerifyString = "!2#4%6&8QwErTyAsDfZx";
                return attribute;
            }
        }
#endif
    }
}
