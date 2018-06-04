using System;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 链表节点
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class LinkNode<valueType>
    {
        /// <summary>
        /// 上一个节点
        /// </summary>
        internal LinkNode<valueType> Previous;
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal LinkNode<valueType> Next;
        /// <summary>
        /// 数据
        /// </summary>
        internal valueType Value;
        /// <summary>
        /// 链表节点
        /// </summary>
        /// <param name="value">数据</param>
        internal LinkNode(valueType value)
        {
            Previous = Next = null;
            Value = value;
        }
        /// <summary>
        /// 链表节点
        /// </summary>
        /// <param name="previous">上一个节点</param>
        /// <param name="next">下一个节点</param>
        /// <param name="value">数据</param>
        internal LinkNode(LinkNode<valueType> previous, LinkNode<valueType> next, valueType value)
        {
            Previous = previous;
            Next = next;
            Value = value;
        }
        /// <summary>
        /// 链表节点（构造尾节点）
        /// </summary>
        /// <param name="previous">上一个节点</param>
        /// <param name="value">数据</param>
        internal LinkNode(LinkNode<valueType> previous, valueType value)
        {
            Previous = previous;
            Next = null;
            Value = value;
            previous.Next = this;
        }
        /// <summary>
        /// 删除当前节点
        /// </summary>
        /// <param name="link"></param>
        internal void Remove(Link<valueType> link)
        {
            if (Previous == null)
            {
                if (Next == null) link.Remove();
                else link.RemoveSetHead(Next);
            }
            else if (Next == null) link.RemoveSetEnd(Previous);
            else
            {
                Next.Previous = Previous;
                Previous.Next = Next;
            }
        }
        /// <summary>
        /// 前置插入节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns>是否首节点</returns>
        internal bool InsertBefore(LinkNode<valueType> node)
        {
            if (Previous == null)
            {
                Previous = node;
                return true;
            }
            Previous.Next = node;
            Previous = node;
            return false;
        }
        /// <summary>
        /// 后置插入节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns>是否尾节点</returns>
        internal bool InsertAfter(LinkNode<valueType> node)
        {
            if (Next == null)
            {
                Next = node;
                return true;
            }
            Next.Previous = node;
            Next = node;
            return false;
        }
    }
}
