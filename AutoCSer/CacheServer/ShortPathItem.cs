using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 短路径数组元素
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct ShortPathItem
    {
        /// <summary>
        /// 短路径标识
        /// </summary>
        private ulong identity;
        /// <summary>
        /// 数据节点
        /// </summary>
        private Cache.Value.Node node;
        /// <summary>
        /// 数据包
        /// </summary>
        private byte[] packet;
        /// <summary>
        /// 设置短路径
        /// </summary>
        /// <param name="node"></param>
        /// <param name="packet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong Set(Cache.Value.Node node, byte[] packet)
        {
            this.node = node;
            this.packet = packet;
            return ++identity;
        }
        /// <summary>
        /// 获取数据节点
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="packet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Cache.Value.Node Get(ulong identity, out byte[] packet)
        {
            if (identity == this.identity)
            {
                packet = this.packet;
                return node;
            }
            packet = NullValue<byte>.Array;
            return null;
        }
    }
}
