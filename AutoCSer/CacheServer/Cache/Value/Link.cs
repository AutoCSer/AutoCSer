using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 链表 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Link<valueType> : Node
    {
        /// <summary>
        /// 首节点
        /// </summary>
        private LinkNode<valueType> head;
        /// <summary>
        /// 尾节点
        /// </summary>
        private LinkNode<valueType> end;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 链表 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Link(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Remove()
        {
            head = end = null;
        }
        /// <summary>
        /// 设置首节点
        /// </summary>
        /// <param name="head"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveSetHead(LinkNode<valueType> head)
        {
            head.Previous = null;
            this.head = head;
        }
        /// <summary>
        /// 设置尾节点
        /// </summary>
        /// <param name="end"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveSetEnd(LinkNode<valueType> end)
        {
            end.Next = null;
            this.end = end;
        }
        /// <summary>
        /// 获取链表节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private LinkNode<valueType> getNode(int index)
        {
            if (index >= 0)
            {
                if ((uint)index < count)
                {
                    LinkNode<valueType> node = head;
                    while (index != 0)
                    {
                        node = node.Next;
                        --index;
                    }
                    return node;
                }
            }
            else
            {
                index = -(index + 1);
                if ((uint)index < count)
                {
                    LinkNode<valueType> node = end;
                    while (index != 0)
                    {
                        node = node.Previous;
                        --index;
                    }
                    return node;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.InsertBefore: insertBefore(ref parser); break;
                case OperationParameter.OperationType.InsertAfter: insertAfter(ref parser); break;
                default: parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError; break;
            }
            return null;
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="parser"></param>
        private void insertBefore(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(int.MinValue);
            LinkNode<valueType> node = getNode(index);
            if (node != null)
            {
                if (parser.LoadValueData() && parser.IsEnd && parser.ValueData.Type == ValueData.Data<valueType>.DataType)
                {
                    LinkNode<valueType> newNode = new LinkNode<valueType>(node.Previous, node, ValueData.Data<valueType>.GetData(ref parser.ValueData));
                    if (node.InsertBefore(newNode)) head = newNode;
                    ++count;
                    parser.SetOperationReturnParameter();
                }
                else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
            else if ((count | index) == 0)
            {
                if (parser.LoadValueData() && parser.IsEnd && parser.ValueData.Type == ValueData.Data<valueType>.DataType) setHeadEnd(ref parser, ValueData.Data<valueType>.GetData(ref parser.ValueData));
                else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
            else parser.ReturnParameter.ReturnType = ReturnType.LinkIndexOutOfRange;
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="parser"></param>
        private void insertAfter(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(int.MinValue);
            LinkNode<valueType> node = getNode(index);
            if (node != null)
            {
                if (parser.LoadValueData() && parser.IsEnd && parser.ValueData.Type == ValueData.Data<valueType>.DataType)
                {
                    LinkNode<valueType> newNode = new LinkNode<valueType>(node, node.Next, ValueData.Data<valueType>.GetData(ref parser.ValueData));
                    if (node.InsertAfter(newNode)) end = newNode;
                    ++count;
                    parser.SetOperationReturnParameter();
                }
                else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
            else if ((count | (index ^ -1)) == 0)
            {
                if (parser.LoadValueData() && parser.IsEnd && parser.ValueData.Type == ValueData.Data<valueType>.DataType) setHeadEnd(ref parser, ValueData.Data<valueType>.GetData(ref parser.ValueData));
                else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
            else parser.ReturnParameter.ReturnType = ReturnType.LinkIndexOutOfRange;
        }
        /// <summary>
        /// 添加第一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        private void setHeadEnd(ref OperationParameter.NodeParser parser, valueType value)
        {
            head = end = new LinkNode<valueType>(value);
            count = 1;
            parser.SetOperationReturnParameter();
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.Remove: remove(ref parser); return;
                case OperationParameter.OperationType.GetRemove: getRemove(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (count != 0)
                    {
                        head = end = null;
                        count = 0;
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.ReturnParameterSet(true);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="parser"></param>
        private void remove(ref OperationParameter.NodeParser parser)
        {
            LinkNode<valueType> node = getNode(parser.GetValueData(int.MinValue));
            if (node != null)
            {
                node.Remove(this);
                --count;
                parser.SetOperationReturnParameter();
            }
            else parser.ReturnParameter.ReturnType = ReturnType.LinkIndexOutOfRange;
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="parser"></param>
        private void getRemove(ref OperationParameter.NodeParser parser)
        {
            LinkNode<valueType> node = getNode(parser.GetValueData(int.MinValue));
            if (node != null)
            {
                node.Remove(this);
                --count;
                parser.IsOperation = true;
                ValueData.Data<valueType>.SetData(ref parser.ReturnParameter, node.Value);
                parser.ReturnParameter.ReturnType = ReturnType.Success;
            }
            else parser.ReturnParameter.ReturnType = ReturnType.LinkIndexOutOfRange;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.ReturnParameterSet(count); return;
                case OperationParameter.OperationType.GetValue:
                    LinkNode<valueType> node = getNode(parser.GetValueData(int.MinValue));
                    if (node != null)
                    {
                        ValueData.Data<valueType>.SetData(ref parser.ReturnParameter, node.Value);
                        parser.ReturnParameter.ReturnType = ReturnType.Success;
                    }
                    else parser.ReturnParameter.ReturnType = ReturnType.LinkIndexOutOfRange;
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            valueType[] array = new valueType[count];
            int index = 0;
            LinkNode<valueType> node = head;
            while (index != array.Length)
            {
                array[index++] = node.Value;
                node = node.Next;
            }
            return new Snapshot.Value.Link<valueType>(array);
        }
#if NOJIT
        /// <summary>
        /// 创建链表节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Link<valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Link<valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Link<valueType>> nodeInfo;
        static Link()
        {
            nodeInfo = new NodeInfo<Link<valueType>>
            {
#if NOJIT
                Constructor = (Constructor<Link<valueType>>)Delegate.CreateDelegate(typeof(Constructor<Link<valueType>>), typeof(Link<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Link<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Link<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
