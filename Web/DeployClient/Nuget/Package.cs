using System;

namespace AutoCSer.Web.DeployClient.Nuget
{
    /// <summary>
    /// 包信息
    /// </summary>
    internal struct Package
    {
        /// <summary>
        /// 包信息
        /// </summary>
        public Metadata metadata;

        /// <summary>
        /// XML 解析参数
        /// </summary>
        internal static readonly AutoCSer.Xml.DeSerializeConfig XmlConfig = new AutoCSer.Xml.DeSerializeConfig { BootNodeName = "package" };
    }
}
