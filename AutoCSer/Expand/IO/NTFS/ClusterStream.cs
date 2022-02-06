using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 簇流
    /// </summary>
    public struct ClusterStream
    {
        /// <summary>
        /// 占用簇的个数
        /// </summary>
        internal long ClusterCount;
        /// <summary>
        /// 偏移（单位为簇）
        /// </summary>
        internal long Offset;
        /// <summary>
        /// 簇流
        /// </summary>
        /// <param name="clusterCount">占用簇的个数</param>
        /// <param name="offset">偏移（单位为簇）</param>
        internal ClusterStream(long clusterCount, long offset)
        {
            ClusterCount = clusterCount;
            Offset = offset;
        }
        /// <summary>
        /// 计算下一个偏移
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Next(ClusterStream next)
        {
            ClusterCount = next.ClusterCount;
            Offset += next.Offset;
        }
    }
}
