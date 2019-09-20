using System;
using System.IO;
using System.Text;

namespace AutoCSer.Web.DeployClient.Project
{
    /// <summary>
    /// 项目配置
    /// </summary>
    internal struct Config
    {
        /// <summary>
        /// 属性分组
        /// </summary>
        public PropertyGroup PropertyGroup;

        /// <summary>
        /// XML 解析参数
        /// </summary>
        internal static readonly AutoCSer.Xml.ParseConfig XmlConfig = new AutoCSer.Xml.ParseConfig { BootNodeName = "Project" };
    }
}
