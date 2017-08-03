using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署服务客户端配置
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        /// 文件更新时间，忽略改时间以前的更新
        /// </summary>
        public DateTime FileLastWriteTime;
        /// <summary>
        /// 文件匹配路径(用于过滤相同的文件名称)
        /// </summary>
        public string[] RunFilePaths;
        /// <summary>
        /// 客户端部署信息集合
        /// </summary>
        public ClientDeploy[] Deploys;
        /// <summary>
        /// 忽略文件名称集合
        /// </summary>
        public string[] IgnoreFileNames;
        /// <summary>
        /// 部署服务 TCP 配置集合 [服务命名 + TCP 配置]
        /// </summary>
        public KeyValue<string, AutoCSer.Net.TcpInternalServer.ServerAttribute>[] ServerAttributes;
    }
}
