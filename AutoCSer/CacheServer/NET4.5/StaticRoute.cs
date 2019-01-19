using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务集群客户端静态路由
    /// </summary>
    public sealed partial class StaticRoute
    {
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> RemoveDataStructureTask(string cacheName)
        {
            return await getClient(cacheName).RemoveDataStructureTask(cacheName);
        }

        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <returns>错误节点数量</returns>
        public async Task<int> WriteFileTask()
        {
            int errorCount = 0;
            for (int index = clients.Length; index != 0;)
            {
                --index;
                if ((await (clients[index] ?? createClient(index)).MasterClient.WriteFileAwaiter()).Type != AutoCSer.Net.TcpServer.ReturnType.Success) ++errorCount;
            }
            return errorCount;
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.Awaiter WriteFileAwaiter(string cacheName)
        {
            return getClient(cacheName).MasterClient.WriteFileAwaiter();
        }
        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <param name="index">节点编号</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.Awaiter WriteFileAwaiter(int index)
        {
            return (clients[index] ?? createClient(index)).MasterClient.WriteFileAwaiter();
        }
    }
}
