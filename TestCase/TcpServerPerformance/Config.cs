using System;
using System.IO;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// 测试服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    public static class Config
    {
        /// <summary>
        /// 测试服务 IP 地址配置文件，用于非换回地址测试
        /// </summary>
        private const string ipFile = @"..\..\..\..\Showjim\Config\TcpInternalPerformanceServerIP.txt";
        /// <summary>
        /// 测试服务 TCP 服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer.ServerName)]
        public static AutoCSer.Net.TcpInternalServer.ServerAttribute InternalServerAttribute
        {
            get
            {
                AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer), false);
                if (File.Exists(ipFile)) attribute.Host = File.ReadAllText(ipFile);
                return attribute;
            }
        }
    }
}
