using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    internal sealed class Array<nodeType> : Node where nodeType : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private nodeType[] array = NullValue<nodeType>.Array;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Array(Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private nodeType getNext(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(-1);
            if ((uint)index < count)
            {
                nodeType node = array[index];
                if (node != null) return node;
                parser.ReturnParameter.ReturnType = ReturnType.NullArrayNode;
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
            return null;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            //if (nodeInfo.IsConstructorParameter && parser.OperationType == OperationParameter.OperationType.GetOrCreateNode)
            //{
            //    int index = parser.GetValueData(-1);
            //    if (index >= 0)
            //    {
            //        if (index >= array.Length) array = array.copyNew(Math.Max(index + 1, array.Length << 1), array.Length);
            //        if (array[index] == null)
            //        {
            //            nodeType node = nodeConstructor(this, ref parser);
            //            if (node.IsNode)
            //            {
            //                array[index] = node;
            //                if (index >= count) count = index + 1;
            //                parser.SetOperationReturnParameter();
            //            }
            //            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            //        }
            //        else if (parser.CheckConstructorParameter(new Value.UnionType { Value = array[index] }.Node.ConstructorParameter)) parser.ReturnParameter.ReturnParameterSet(true);
            //        else parser.ReturnParameter.ReturnType = ReturnType.CheckConstructorParameterError;
            //    }
            //    else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
            //    return null;
            //}
            return getNext(ref parser);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetOrCreateNode: getOrCreateNode(ref parser); return;
                case OperationParameter.OperationType.Remove: remove(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (array.Length != 0)
                    {
                        onClear();
                        array = NullValue<nodeType>.Array;
                        count = 0;
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.ReturnParameterSet(true);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 获取或者创建节点
        /// </summary>
        /// <param name="parser"></param>
        private void getOrCreateNode(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(-1);
            if (index >= 0)
            {
                if (index >= array.Length) array = array.copyNew(Math.Max(index + 1, array.Length << 1), array.Length);
                if (array[index] == null)
                {
                    nodeType node = nodeConstructor(this, ref parser);
                    array[index] = node;
                    parser.IsOperation = true;
                    if (index >= count) count = index + 1;
                }
                parser.ReturnParameter.ReturnParameterSet(true);
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="parser"></param>
        private void remove(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(-1);
            if ((uint)index < count)
            {
                nodeType node = array[index];
                if (node != null)
                {
                    parser.IsOperation = true;
                    array[index] = null;
                    node.OnRemoved();
                }
                parser.ReturnParameter.ReturnParameterSet(true);
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            return getNext(ref parser);
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
                case OperationParameter.OperationType.ContainsKey:
                    int index = parser.GetValueData(-1);
                    parser.ReturnParameter.ReturnParameterSet((uint)index < count && array[index] != null);
                    return;
                case OperationParameter.OperationType.CreateShortPath:
                    nodeType node = getNext(ref parser);
                    if (node != null) node.CreateShortPath(ref parser);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            base.OnRemoved();
            onClear();
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        private void onClear()
        {
            if (nodeInfo.IsOnRemovedEvent && this.count != 0)
            {
                int count = this.count;
                foreach (nodeType node in array)
                {
                    if (node != null) node.OnRemoved();
                    if (--count == 0) return;
                }
            }
        }
        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            Snapshot.Node[] array = new Snapshot.Node[count];
            int index = 0;
            foreach (nodeType node in this.array)
            {
                if (node != null) array[index] = node.CreateSnapshot();
                if (++index >= count) break;
            }
            return new Snapshot.Array(array);
        }
#if NOJIT
        /// <summary>
        /// 创建数组节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Array<nodeType> create(Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Array<nodeType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Array<nodeType>> nodeInfo;
        /// <summary>
        /// 子节点构造函数
        /// </summary>
        private static readonly Constructor<nodeType> nodeConstructor;
        static Array()
        {
            NodeInfo<nodeType> nextNodeInfo = (NodeInfo<nodeType>)typeof(nodeType).GetField(NodeInfoFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
            nodeInfo = new NodeInfo<Array<nodeType>>
            {
                IsOnRemovedEvent = nextNodeInfo.IsOnRemovedEvent,
#if NOJIT
                Constructor = (Constructor<Array<nodeType>>)Delegate.CreateDelegate(typeof(Constructor<Array<nodeType>>), typeof(Array<nodeType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Array<nodeType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Array<nodeType>), NodeConstructorParameterTypes)
#endif
            };
            nodeConstructor = nextNodeInfo.Constructor;
        }
    }
}
