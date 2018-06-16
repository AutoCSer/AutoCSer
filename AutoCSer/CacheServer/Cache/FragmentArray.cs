using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 32768 基分段 数组节点
    /// </summary>
    internal static class FragmentArray
    {
        /// <summary>
        /// 数组长度 2 进制位长度
        /// </summary>
        internal const int ArrayShift = 15;
        /// <summary>
        /// 数组长度
        /// </summary>
        internal const int ArraySize = 1 << ArrayShift;
        /// <summary>
        /// 数组位置 and 值
        /// </summary>
        internal const int ArraySizeAnd = ArraySize - 1;
    }
    /// <summary>
    /// 32768 基分段 数组节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    internal sealed class FragmentArray<nodeType> : Node where nodeType : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private nodeType[][] arrays = NullValue<nodeType[]>.Array;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 32768 基分段 数组节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private FragmentArray(Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
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
                nodeType node = arrays[index >> FragmentArray.ArrayShift][index & FragmentArray.ArraySizeAnd];
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
            //    int indexParameter = parser.GetValueData(-1);
            //    if (indexParameter >= 0)
            //    {
            //        int arrayIndex = indexParameter >> FragmentArray.ArrayShift;
            //        if (arrayIndex >= arrays.Length)
            //        {
            //            nodeType[][] newArrays = arrays.copyNew(arrayIndex + 1, arrays.Length);
            //            for (int newIndex = arrays.Length; newIndex != newArrays.Length; newArrays[newIndex++] = new nodeType[FragmentArray.ArraySize]) ;
            //            arrays = newArrays;
            //        }
            //        nodeType[] array = arrays[arrayIndex];
            //        int index = indexParameter & FragmentArray.ArraySizeAnd;
            //        if (array[index] == null)
            //        {
            //            nodeType node = nodeConstructor(this, ref parser);
            //            if (node.IsNode)
            //            {
            //                array[index] = node;
            //                if (indexParameter >= count) count = indexParameter + 1;
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
                    if (arrays.Length != 0)
                    {
                        onClear();
                        arrays = NullValue<nodeType[]>.Array;
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
            int indexParameter = parser.GetValueData(-1);
            if (indexParameter >= 0)
            {
                int arrayIndex = indexParameter >> FragmentArray.ArrayShift;
                if (arrayIndex >= arrays.Length)
                {
                    nodeType[][] newArrays = arrays.copyNew(arrayIndex + 1, arrays.Length);
                    for (int newIndex = arrays.Length; newIndex != newArrays.Length; newArrays[newIndex++] = new nodeType[FragmentArray.ArraySize]) ;
                    arrays = newArrays;
                }
                nodeType[] array = arrays[arrayIndex];
                int index = indexParameter & FragmentArray.ArraySizeAnd;
                if (array[index] == null)
                {
                    array[index] = nodeConstructor(this, ref parser);
                    parser.IsOperation = true;
                    if (indexParameter >= count) count = indexParameter + 1;
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
                nodeType[] array = arrays[index >> FragmentArray.ArrayShift];
                nodeType node = array[index &= FragmentArray.ArraySizeAnd];
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
                    if ((uint)index < count)
                    {
                        nodeType[] array = arrays[index >> FragmentArray.ArrayShift];
                        parser.ReturnParameter.ReturnParameterSet(array[index & FragmentArray.ArraySizeAnd] != null);
                    }
                    else parser.ReturnParameter.ReturnParameterSet(false);
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
            if (nodeInfo.IsOnRemovedEvent && count != 0)
            {
                int index = 0;
                foreach (nodeType[] nodeArray in arrays)
                {
                    if (nodeArray != null)
                    {
                        foreach (nodeType node in nodeArray)
                        {
                            if (node != null) node.OnRemoved();
                            if (++index >= count) return;
                        }
                    }
                    else index += FragmentArray.ArraySize;
                    if (index >= count) return;
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
            foreach (nodeType[] nodeArray in arrays)
            {
                if (nodeArray != null)
                {
                    foreach (nodeType node in nodeArray)
                    {
                        if (node != null) array[index] = node.CreateSnapshot();
                        if (++index >= count) break;
                    }
                }
                else index += FragmentArray.ArraySize;
                if (index >= count) break;
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
        private static FragmentArray<nodeType> create(Node parent, ref OperationParameter.NodeParser parser)
        {
            return new FragmentArray<nodeType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<FragmentArray<nodeType>> nodeInfo;
        /// <summary>
        /// 子节点构造函数
        /// </summary>
        private static readonly Constructor<nodeType> nodeConstructor;
        static FragmentArray()
        {
            NodeInfo<nodeType> nextNodeInfo = (NodeInfo<nodeType>)typeof(nodeType).GetField(NodeInfoFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
            nodeInfo = new NodeInfo<FragmentArray<nodeType>>
            {
                IsOnRemovedEvent = nextNodeInfo.IsOnRemovedEvent,
#if NOJIT
                Constructor = (Constructor<FragmentArray<nodeType>>)Delegate.CreateDelegate(typeof(Constructor<FragmentArray<nodeType>>), typeof(FragmentArray<nodeType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<FragmentArray<nodeType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(FragmentArray<nodeType>), NodeConstructorParameterTypes)
#endif
            };
            nodeConstructor = nextNodeInfo.Constructor;
        }
    }
}
