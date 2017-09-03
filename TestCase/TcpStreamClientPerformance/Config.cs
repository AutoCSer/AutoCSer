using System;
using System.IO;

namespace AutoCSer.TestCase.TcpInternalStreamClientPerformance
{
    /// <summary>
    /// 测试服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// 测试服务 IP 地址配置文件，用于非换回地址测试
        /// </summary>
        private const string ipFile = @"..\..\..\..\Showjim\Config\TcpInternalPerformanceClientIP.txt";
        /// <summary>
        /// 测试服务 TCP 服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer.ServerName)]
        public static AutoCSer.Net.TcpInternalStreamServer.ServerAttribute InternalServerAttribute
        {
            get
            {
                AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalStreamServer.ServerAttribute>(typeof(AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer), false);
                if (File.Exists(ipFile)) attribute.Host = File.ReadAllText(ipFile);
                return attribute;
            }
        }
    }
}
