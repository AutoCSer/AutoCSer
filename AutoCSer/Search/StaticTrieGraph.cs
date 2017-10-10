using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的 Trie 图
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">节点值类型</typeparam>
    public abstract unsafe partial class StaticTrieGraph<keyType, valueType> : IDisposable
        where keyType : struct, IEquatable<keyType>, IComparable<keyType>
        where valueType : class
    {
        /// <summary>
        /// 子节点
        /// </summary>
        protected int* nodes;
        /// <summary>
        /// 根节点
        /// </summary>
        protected int boot;
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            CancelBuilder();
            int boot = this.boot;
            if (boot != 0)
            {
                this.boot = 0;
                Monitor.Enter(NodePool.Lock);
                try
                {
                    NodePool.Pool[boot >> ArrayPool.ArraySizeBit][boot & ArrayPool.ArraySizeAnd].Free();
                    NodePool.FreeNoLock(boot);
                }
                finally { Monitor.Exit(NodePool.Lock); }
            }
        }
        /// <summary>
        /// 取消创建
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CancelBuilder()
        {
            int* nodes = this.nodes;
            if (nodes != null)
            {
                this.nodes = null;
                Unmanaged.Free((byte*)nodes, (1 << 16) * sizeof(int));
            }
        }

        /// <summary>
        /// 节点池
        /// </summary>
        internal static ArrayPool<Node> NodePool;
        /// <summary>
        /// 获取可用节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>节点索引</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int GetNodeIndex(out Node[] nodes)
        {
            int index = NodePool.GetNoLock(out nodes);
            nodes[index & ArrayPool.ArraySizeAnd].Reset();
            return index;
        }
        static StaticTrieGraph()
        {
            NodePool = new ArrayPool<Node>(256);
            NodePool.Pool[0][0].Nodes = NullValue<KeyValue<keyType, int>>.Array;
            NodePool.CurrentArrayIndex = 1;
        }
    }
}
