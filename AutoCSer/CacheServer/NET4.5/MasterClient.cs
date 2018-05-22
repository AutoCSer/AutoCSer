using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务客户端
    /// </summary>
    public sealed partial class MasterClient
    {
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> OperationTask(DataStructure.Abstract.Node node)
        {
            return await client.OperationAwaiter(new OperationParameter.OperationNode { Node = node });
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
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> RemoveDataStructureTask(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) return new ReturnValue<bool> { Type = ReturnType.NullArgument };
            IndexIdentity identity = default(IndexIdentity);
            identity.Set(await client.RemoveAwaiter(new OperationParameter.RemoveDataStructure { CacheName = cacheName }));
            if (identity.ReturnType == ReturnType.Success) removeDataStructure(cacheName, ref identity);
            return new ReturnValue<bool> { TcpReturnType = identity.TcpReturnType, Type = identity.ReturnType, Value = identity.ReturnType == ReturnType.Success };
        }

        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <returns></returns>
        public async Task<AutoCSer.Net.TcpServer.ReturnValue> WriteFileTask()
        {
            return await client.WriteFileAwaiter();
        }
    }
}
