using System;
using System.Collections.Generic;
using AutoCSer.Log;
using System.Runtime.CompilerServices;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块基类
    /// </summary>
    public abstract class BlockBase
    {
        /// <summary>
        /// 数据长度检测读取字节数量
        /// </summary>
        internal const int CheckSize = 4 << 10;
        /// <summary>
        /// 默认缓冲区池
        /// </summary>
        internal static readonly SubBuffer.Pool DefaultBufferPool = SubBuffer.Pool.GetPool(SubBuffer.Size.Kilobyte4);
        /// <summary>
        /// 磁盘块编号
        /// </summary>
        protected readonly ulong index64;
        /// <summary>
        /// 日志处理
        /// </summary>
        protected readonly ILog log;
        /// <summary>
        /// 数据缓存
        /// </summary>
        internal readonly FifoPriorityQueue<long, HashBytes> IndexCache = new FifoPriorityQueue<long, HashBytes>();
        /// <summary>
        /// 数据缓存
        /// </summary>
        internal readonly Dictionary<HashBytes, DataCache>[] DataCache;
        /// <summary>
        /// 文件块读取缓冲区大小
        /// </summary>
        private int freeCacheSize;
        /// <summary>
        /// 磁盘块编号
        /// </summary>
        protected readonly int index;
        /// <summary>
        /// 磁盘块基类
        /// </summary>
        /// <param name="index">磁盘块编号</param>
        /// <param name="cacheSize">文件块读取缓冲区大小</param>
        /// <param name="log">日志处理</param>
        /// <param name="isDataCache">是否建立数据缓存</param>
        internal BlockBase(int index, int cacheSize, ILog log, bool isDataCache)
        {
            this.index = index;
            index64 = (ulong)index << Server.IndexShift;
            this.log = log;
            this.freeCacheSize = Math.Max(cacheSize, 1 << 20);
            if (isDataCache) DataCache = new Dictionary<HashBytes, DataCache>[256];
        }
        /// <summary>
        /// 获取磁盘块索引位置
        /// </summary>
        /// <param name="hashData"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong GetCacheIndex(HashBytes hashData)
        {
            Dictionary<HashBytes, DataCache> cache = DataCache[hashData.HashCode & 0xff];
            if (cache != null)
            {
                DataCache data;
                if (cache.TryGetValue(hashData, out data)) return index64 + (ulong)data.Index;
            }
            return 0;
        }
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        internal void SetCache(long index, byte[] data)
        {
            if (DataCache == null)
            {
                IndexCache.UnsafeAdd(ref index, new HashBytes { SubArray = new SubArray<byte> { Array = data } });
                if ((freeCacheSize -= data.Length) < 0)
                {
                    HashBytes removeData;
                    do
                    {
                        removeData = IndexCache.UnsafePopValue();
                    }
                    while ((freeCacheSize += removeData.SubArray.Array.Length) < 0);
                }
            }
            else
            {
                HashBytes hashData = data;
                Dictionary<HashBytes, DataCache> cache = DataCache[hashData.HashCode & 0xff];
                if (cache == null)
                {
                    IndexCache.UnsafeAdd(ref index, hashData);
                    freeCacheSize -= data.Length;
                    DataCache[hashData.HashCode & 0xff] = cache = DictionaryCreator.CreateHashBytes<DataCache>();
                    cache.Add(hashData, new DataCache { Data = hashData, Index = index, Count = 1 });
                }
                else
                {
                    DataCache count;
                    if (cache.TryGetValue(hashData, out count))
                    {
                        IndexCache.UnsafeAdd(ref index, count.Data);
                        ++count.Count;
                        freeCacheSize -= data.Length;
                        cache[hashData] = count;
                    }
                    else
                    {
                        IndexCache.UnsafeAdd(ref index, hashData);
                        freeCacheSize -= data.Length;
                        cache.Add(hashData, new DataCache { Data = hashData, Index = index, Count = 1 });
                    }
                }
                DataCache removeData;
                while (freeCacheSize < 0)
                {
                    removeData.Data = IndexCache.UnsafePopValue();
                    freeCacheSize += removeData.Data.SubArray.Array.Length;
                    cache = DataCache[removeData.Data.HashCode & 0xff];
                    if (cache != null && cache.TryGetValue(removeData.Data, out removeData))
                    {
                        if (--removeData.Count == 0) cache.Remove(removeData.Data);
                        else cache[removeData.Data] = removeData;
                    }
                }
            }
        }
    }
}
