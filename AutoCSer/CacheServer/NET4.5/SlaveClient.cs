using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务客户端
    /// </summary>
    public sealed partial class SlaveClient
    {
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> OperationTask(DataStructure.Abstract.Node node)
        {
            return new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.SlaveCanNotWrite } };
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> QueryTask(DataStructure.Abstract.Node node)
        {
            return await client.QueryAwaiter(new OperationParameter.QueryNode { Node = node });
        }
    }
}
