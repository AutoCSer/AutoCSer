using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 节点索引
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct NodeIndex
    {
        /// <summary>
        /// 节点集合
        /// </summary>
        public LeftArray<Node> Array;
        /// <summary>
        /// 当前访问位置
        /// </summary>
        public int Index;
        /// <summary>
        /// 当前节点
        /// </summary>
        public Node Current
        {
            get { return Array.Array[Index]; }
        }
        /// <summary>
        /// 添加下一个节点
        /// </summary>
        /// <param name="next"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        public Node MoveNext(ref LeftArray<Node> next, ref LeftArray<NodeIndex> indexs)
        {
            if (next.Length == 0)
            {
                if (++Index == Array.Length)
                {
                    if (indexs.Length == 0) return null;
                    this = indexs.UnsafePopOnly();
                }
                return Array.Array[Index];
            }
            if (++Index != Array.Length) indexs.Add(this);
            Array = next;
            Index = 0;
            return next.Array[0];
        }
        /// <summary>
        /// 剩余节点数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int NextCount()
        {
            return Array.Length - ++Index;
        }
        /// <summary>
        /// 设置新的节点信息
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Node SetNext(ref LeftArray<Node> next)
        {
            Array = next;
            Index = 0;
            return next.Array[0];
        }
    }
}
