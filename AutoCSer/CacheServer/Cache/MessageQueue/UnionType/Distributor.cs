using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Distributor
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 消息分发 数据节点
        /// </summary>
        [FieldOffset(0)]
        public MessageQueue.Distributor Value;
    }
}
