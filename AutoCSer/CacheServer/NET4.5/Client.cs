using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public abstract partial class Client
    {
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal abstract Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> OperationTask(DataStructure.Abstract.Node node);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal abstract Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> QueryTask(DataStructure.Abstract.Node node);
    }
}
