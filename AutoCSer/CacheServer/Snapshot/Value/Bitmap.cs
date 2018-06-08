using System;

namespace AutoCSer.CacheServer.Snapshot.Value
{
    /// <summary>
    /// 位图 数据节点
    /// </summary>
    internal sealed class Bitmap : Node
    {
        /// <summary>
        /// 位图
        /// </summary>
        private byte[][] maps;
        /// <summary>
        /// 当前处理位图
        /// </summary>
        private byte[] map;
        /// <summary>
        /// 当前处理位图的位索引
        /// </summary>
        private int bitIndex;
        /// <summary>
        /// 当前处理位图的基本位索引
        /// </summary>
        private uint bitIndexBase;
        /// <summary>
        /// 位图节点
        /// </summary>
        /// <param name="maps"></param>
        internal Bitmap(byte[][] maps)
        {
            this.maps = maps;
            index = maps.Length;
            nextMap();
        }
        /// <summary>
        /// 设置下一个位图
        /// </summary>
        private void nextMap()
        {
            if (index == 0) map = null;
            else
            {
                map = maps[--index];
                bitIndex = map.Length << 3;
                bitIndexBase = (uint)index * AutoCSer.CacheServer.Cache.Value.Bitmap.MaxMapBitSize;
            }
        }
        /// <summary>
        /// 缓存快照序列化
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        internal override bool Serialize(Cache cache)
        {
            while (map != null)
            {
                while (bitIndex != 0)
                {
                    --bitIndex;
                    if ((map[bitIndex >> 3] & (1 << (bitIndex & 7))) != 0)
                    {
                        cache.Parameter.Set(bitIndexBase + (uint)bitIndex);
                        cache.SerializeParameter(OperationParameter.OperationType.SetValue);
                        return true;
                    }
                }
                nextMap();
            }
            maps = null;
            return false;
        }
    }
}
