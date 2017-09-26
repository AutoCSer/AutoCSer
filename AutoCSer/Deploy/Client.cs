using System;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署服务客户端
    /// </summary>
    public class Client
    {
        /// <summary>
        /// 部署服务客户端集合
        /// </summary>
        private readonly Dictionary<HashString, TcpClient> clients = DictionaryCreator.CreateHashString<TcpClient>();
        /// <summary>
        /// 部署服务客户端配置
        /// </summary>
        internal ClientConfig Config;
        /// <summary>
        /// 客户端部署信息集合
        /// </summary>
        private ClientDeploy[] deploys;
        /// <summary>
        /// 部署信息数量
        /// </summary>
        public int Count
        {
            get { return deploys.Length; }
        }
        /// <summary>
        /// 获取部署信息
        /// </summary>
        /// <param name="index">部署信息索引位置</param>
        /// <returns>部署信息</returns>
        public ClientDeploy this[int index]
        {
            get { return deploys[index]; }
        }
        /// <summary>
        /// 忽略文件名称集合
        /// </summary>
        internal HashSet<string> IgnoreFileNames;
        /// <summary>
        /// 部署服务客户端就绪
        /// </summary>
        private Action<string> onClientReady;
        /// <summary>
        /// 部署任务状态更新日志回调处理
        /// </summary>
        public event Action<string, AutoCSer.Deploy.Log> OnLog;
        /// <summary>
        /// 部署服务客户端
        /// </summary>
        /// <param name="config">部署服务客户端配置</param>
        /// <param name="onClientReady">部署服务客户端就绪</param>
        public Client(ClientConfig config = null, Action<string> onClientReady = null)
        {
            this.Config = config ?? ConfigLoader.GetUnion(typeof(ClientConfig)).ClientConfig;
            this.onClientReady = onClientReady;
            if ((deploys = config.Deploys).Length == 0) throw new ArgumentNullException();
            IgnoreFileNames = config.IgnoreFileNames.getHash(value => value) ?? HashSetCreator.CreateOnly<string>();

            Dictionary<HashString, AutoCSer.Net.TcpInternalServer.ServerAttribute> attributes;
            if (config.ServerAttributes.length() == 0) attributes = null;
            else 
            {
                attributes = DictionaryCreator.CreateHashString<AutoCSer.Net.TcpInternalServer.ServerAttribute>();
                foreach (KeyValue<string, AutoCSer.Net.TcpInternalServer.ServerAttribute> attribute in config.ServerAttributes.notNull())
                {
                    attributes.Add(attribute.Key ?? string.Empty, attribute.Value);
                }
            }
            TcpClient tcpClient;
            int deployIndex = 0;
            foreach (ClientDeploy deploy in deploys)
            {
                if (deploy.Tasks.length() == 0) throw new ArgumentNullException("deploys[" + deployIndex.toString() + "].Tasks");
                HashString serverName = deploy.ServerName ?? string.Empty;
                if (!clients.TryGetValue(serverName, out tcpClient)) 
                {
                    if (serverName.String.Length == 0) clients.Add(serverName, tcpClient = new TcpClient(this, string.Empty, null));
                    else
                    {
                        AutoCSer.Net.TcpInternalServer.ServerAttribute attribute;
                        if (attributes.TryGetValue(serverName, out attribute)) clients.Add(serverName, tcpClient = new TcpClient(this, serverName.String, attribute));
                        else throw new ArgumentNullException("缺少服务命名 " + serverName);
                    }
                }
                deploys[deployIndex++].TcpClient = tcpClient;
            }
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="serverName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnClient(string serverName)
        {
            if (onClientReady != null) onClientReady(serverName);
        }
        /// <summary>
        /// 部署任务状态更新日志回调
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="log"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnGetLog(string serverName, AutoCSer.Deploy.Log log)
        {
            if (OnLog != null) OnLog(serverName, log);
        }
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <param name="index">客户端部署信息集合索引</param>
        /// <returns>部署信息索引，失败返回 -1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public DeployResult Deploy(int index)
        {
            return deploys[index].Deploy();
        }
    }
}
