using System;

namespace AutoCSer.CacheServer.ServerCall
{
    /// <summary>
    /// 搜索树字典获取数据
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class SearchTreeDictionaryGetter<keyType, valueType> : AutoCSer.Net.TcpServer.ServerCallBase
        where keyType : IEquatable<keyType>, IComparable<keyType>
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        private readonly AutoCSer.CacheServer.CacheManager cache;
        /// <summary>
        /// 搜索树字典
        /// </summary>
        private readonly Cache.Value.SearchTreeDictionary<keyType, valueType> dictionary;
        /// <summary>
        /// 返回调用委托
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> OnReturn;
        /// <summary>
        /// 跳过记录数
        /// </summary>
        private readonly int skipCount;
        /// <summary>
        /// 获取记录数
        /// </summary>
        private readonly int getCount;
        /// <summary>
        /// 搜索树字典获取数据
        /// </summary>
        /// <param name="dictionary">搜索树字典</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <param name="parser">参数解析</param>
        internal SearchTreeDictionaryGetter(Cache.Value.SearchTreeDictionary<keyType, valueType> dictionary, int skipCount, int getCount, ref OperationParameter.NodeParser parser)
        {
            OnReturn = parser.OnReturn;
            cache = parser.Cache;
            this.dictionary = dictionary;
            this.skipCount = skipCount;
            this.getCount = getCount;
            parser.OnReturn = null;
        }

        /// <summary>
        /// 搜索树字典获取数据
        /// </summary>
        public override void Call()
        {
            ReturnParameter returnParameter = default(ReturnParameter);
            try
            {
                returnParameter.Parameter.ReturnParameterSetBinary(new ReturnArray<valueType>(getCount > 0 ? dictionary.Dictionary.GetRange(skipCount, getCount) : dictionary.Dictionary.GetRangeDesc(skipCount, -getCount)));
            }
            catch (Exception error)
            {
                cache.TcpServer.AddLog(error);
            }
            finally { OnReturn(returnParameter); }
        }
    }
}
