using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 搜索树字典节点
    /// </summary>
    public sealed partial class ValueSearchTreeDictionary<keyType, valueType>
    {
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<ShortPath.SearchTreeDictionary<keyType, valueType>>> CreateShortPathTask()
        {
            return await new ShortPath.SearchTreeDictionary<keyType, valueType>(this).CreateTask();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(keyType key)
        {
            return new ReturnValue<valueType>(await ClientDataStructure.Client.QueryAwaiter(GetNode(key)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetTask(keyType key, valueType valueNode)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationAwaiter(GetSetNode(key, valueNode)));
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType[]>> GetTask(int skipCount, int getCount)
        {
            if (getCount > 0 && skipCount >= 0) return ReturnArray<valueType>.Get(await ClientDataStructure.Client.QueryAsynchronousAwaiter(GetNode(skipCount, getCount)));
            return new ReturnValue<valueType[]> { Type = ReturnType.SearchTreeDictionaryIndexOutOfRange };
        }
        /// <summary>
        /// 获取逆序数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType[]>> GetDescTask(int skipCount, int getCount)
        {
            if (getCount > 0 && skipCount >= 0) return ReturnArray<valueType>.Get(await ClientDataStructure.Client.QueryAsynchronousAwaiter(GetNode(skipCount, -getCount)));
            return new ReturnValue<valueType[]> { Type = ReturnType.SearchTreeDictionaryIndexOutOfRange };
        }
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="page">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数</param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType[]>> GetPageTask(int page, int pageSize)
        {
            long endIndex = (long)page * (long)pageSize;
            if (page > 0 && pageSize > 0 && (ulong)endIndex < (ulong)(uint)int.MaxValue) return ReturnArray<valueType>.Get(await ClientDataStructure.Client.QueryAsynchronousAwaiter(GetNode((int)endIndex - pageSize, pageSize)));
            return new ReturnValue<valueType[]> { Type = ReturnType.SearchTreeDictionaryIndexOutOfRange };
        }
        /// <summary>
        /// 获取逆序分页数据集合
        /// </summary>
        /// <param name="page">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数</param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType[]>> GetPageDescTask(int page, int pageSize)
        {
            long endIndex = (long)page * (long)pageSize;
            if (page > 0 && pageSize > 0 && (ulong)endIndex < (ulong)(uint)int.MaxValue) return ReturnArray<valueType>.Get(await ClientDataStructure.Client.QueryAsynchronousAwaiter(GetNode((int)endIndex - pageSize, -pageSize)));
            return new ReturnValue<valueType[]> { Type = ReturnType.SearchTreeDictionaryIndexOutOfRange };
        }
    }
}
