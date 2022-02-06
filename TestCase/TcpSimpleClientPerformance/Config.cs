using System;
using System.Collections.Generic;
using System.IO;

namespace AutoCSer.TestCase.TcpInternalSimpleClientPerformance
{
    /// <summary>
    /// 测试服务配置
    /// </summary>
    internal sealed class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// 测试服务 IP 地址配置文件，用于非换回地址测试
        /// </summary>
        private const string ipFile = @"..\..\..\..\Showjim\Config\TcpInternalPerformanceClientIP.txt";
        /// <summary>
        /// 测试服务 TCP 服务配置
        /// </summary>
        [AutoCSer.Configuration.Member(AutoCSer.TestCase.TcpInternalSimpleServerPerformance.InternalSimpleServer.ServerName)]
        public static AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute InternalSimpleServerAttribute
        {
            get
            {
                AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute>(typeof(AutoCSer.TestCase.TcpInternalSimpleServerPerformance.InternalSimpleServer), false);
                if (File.Exists(ipFile)) attribute.Host = File.ReadAllText(ipFile);
                return attribute;
            }
        }
    }
}
