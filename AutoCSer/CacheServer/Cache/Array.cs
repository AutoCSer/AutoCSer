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
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private Node getNext(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(-1);
            if ((uint)index < count)
            {
                nodeType node = array[index];
                if (node != null) return node;
                parser.ReturnParameter.Type = ReturnType.NullArrayNode;
            }
            else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
            return null;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
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
                        array = NullValue<nodeType>.Array;
                        count = 0;
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.Set(true);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
                    array[index] = nodeConstructor();
                    parser.IsOperation = true;
                    if (index >= count) count = index + 1;
                }
                parser.ReturnParameter.Set(true);
            }
            else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
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
                if (array[index] != null)
                {
                    parser.IsOperation = true;
                    array[index] = null;
                }
                parser.ReturnParameter.Set(true);
            }
            else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
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
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(count); return;
                case OperationParameter.OperationType.ContainsKey:
                    int index = parser.GetValueData(-1);
                    parser.ReturnParameter.Set((uint)index < count && array[index] != null);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Array<nodeType> create()
        {
            return new Array<nodeType>();
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Array<nodeType>> constructor;
        /// <summary>
        /// 子节点构造函数
        /// </summary>
        private static readonly Func<nodeType> nodeConstructor;
        static Array()
        {
#if NOJIT
            constructor = (Func<Array<nodeType>>)Delegate.CreateDelegate(typeof(Func<Array<nodeType>>), typeof(Array<nodeType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NullValue<Type>.Array, null));
#else
            constructor = (Func<Array<nodeType>>)AutoCSer.Emit.Constructor.Create(typeof(Array<nodeType>));
#endif
            nodeConstructor = (Func<nodeType>)typeof(nodeType).GetField(ConstructorFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
        }
    }
}
