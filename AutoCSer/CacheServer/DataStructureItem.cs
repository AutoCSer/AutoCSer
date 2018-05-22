using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 数据结构数组元素
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DataStructureItem
    {
        /// <summary>
        /// 数据结构标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 服务端数据结构定义信息
        /// </summary>
        internal ServerDataStructure DataStructure;
        /// <summary>
        /// 设置数据结构数组元素
        /// </summary>
        /// <param name="index">数据结构索引</param>
        /// <param name="dataStructure">服务端数据结构定义信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int index, ServerDataStructure dataStructure)
        {
            dataStructure.Identity.Set(index, Identity);
            DataStructure = dataStructure;
        }
        /// <summary>
        /// 释放数据结构数组元素
        /// </summary>
        /// <returns>数据结构标识</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong Free()
        {
            DataStructure = null;
            return ++Identity;
        }
        /// <summary>
        /// 获取数据结构节点信息
        /// </summary>
        /// <param name="identity">数据结构标识</param>
        /// <returns>数据结构节点信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerDataStructure Get(ulong identity)
        {
            return Identity == identity ? DataStructure : null;
        }
        /// <summary>
        /// 释放数据结构数组元素
        /// </summary>
        /// <param name="identity">数据结构标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void LoadFree(ulong identity)
        {
            DataStructure = null;
            Identity = identity;
        }
        /// <summary>
        /// 加载数据结构数组元素
        /// </summary>
        /// <param name="dataStructure">数据结构节点信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Load(ServerDataStructure dataStructure)
        {
            Identity = dataStructure.Identity.SetSuccess();
            DataStructure = dataStructure;
        }
        /// <summary>
        /// 加载数据结构标识
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="dataStructure"></param>
        /// <returns></returns>
        internal bool Load(ulong identity, out ServerDataStructure dataStructure)
        {
            dataStructure = DataStructure;
            if (DataStructure == null)
            {
                Identity = identity;
                return true;
            }
            return Identity == identity;
        }
    }
}
