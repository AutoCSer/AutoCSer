using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务客户端
    /// </summary>
    public partial class SlaveClient : Client, IDisposable
    {
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        private SlaveServer.TcpInternalClient client;
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="client">缓存服务客户端</param>
        public SlaveClient(SlaveServer.TcpInternalClient client = null)
        {
            this.client = client ?? new SlaveServer.TcpInternalClient();
        }
        /// <summary>
        /// 缓存服务客户端
        /// </summary>
        /// <param name="serverAttribute">缓存服务端配置</param>
        public SlaveClient(AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute)
        {
            client = new SlaveServer.TcpInternalClient(serverAttribute);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        /// <summary>
        /// 获取或者创建数据结构信息
        /// </summary>
        /// <param name="dataStructure"></param>
        /// <returns></returns>
        internal override AutoCSer.Net.TcpServer.ReturnValue<IndexIdentity> GetOrCreate(ClientDataStructure dataStructure)
        {
            return client.Get(new OperationParameter.ClientDataStructure { DataStructure = dataStructure });
        }
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        public override ReturnValue<bool> RemoveDataStructure(string cacheName)
        {
            return new ReturnValue<bool> { Type = ReturnType.SlaveCanNotWrite, TcpReturnType = AutoCSer.Net.TcpServer.ReturnType.ClientException };
        }
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="onRemove"></param>
        public override void RemoveDataStructure(string cacheName, Action<ReturnValue<bool>> onRemove)
        {
            if (onRemove == null) throw new InvalidOperationException(ReturnType.SlaveCanNotWrite.ToString());
            else onRemove(new ReturnValue<bool> { Type = ReturnType.SlaveCanNotWrite, TcpReturnType = AutoCSer.Net.TcpServer.ReturnType.ClientException });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        internal override AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Query(DataStructure.Abstract.Node node)
        {
            return client.Query(new OperationParameter.QueryNode { Node = node });
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void Query(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            client.Query(new OperationParameter.QueryNode { Node = node }, onReturn);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void QueryStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            client.QueryStream(new OperationParameter.QueryNode { Node = node }, onReturn);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Operation(DataStructure.Abstract.Node node)
        {
            return new Net.TcpServer.ReturnValue<ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.SlaveCanNotWrite } };
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        internal override void OperationOnly(DataStructure.Abstract.Node node)
        {
            throw new InvalidOperationException(ReturnType.SlaveCanNotWrite.ToString());
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void Operation(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            onReturn(new Net.TcpServer.ReturnValue<ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.SlaveCanNotWrite } });
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal override void OperationStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            onReturn(new Net.TcpServer.ReturnValue<ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.SlaveCanNotWrite } });
        }
    }
}
