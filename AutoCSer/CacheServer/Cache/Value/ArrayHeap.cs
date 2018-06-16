using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 数组模拟最小堆 数据节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class ArrayHeap<keyType, valueType> : Node, IDisposable
        where keyType : IEquatable<keyType>, IComparable<keyType>
    {
        /// <summary>
        /// 数组模拟最小堆
        /// </summary>
        private readonly AutoCSer.ArrayHeap<keyType, valueType> heap = new AutoCSer.ArrayHeap<keyType, valueType>();
        /// <summary>
        /// 数组模拟最小堆 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private ArrayHeap(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            heap.Dispose();
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
                case OperationParameter.OperationType.SetValue: setValue(ref parser); break;
                default: parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError; break;
            }
            return null;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="parser"></param>
        private void setValue(ref OperationParameter.NodeParser parser)
        {
            keyType key;
            if (HashCodeKey<keyType>.Get(ref parser, out key) && parser.LoadValueData() && parser.IsEnd && parser.ValueData.Type == ValueData.Data<valueType>.DataType)
            {
                valueType value = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                heap.Push(key, ref value);
                parser.SetOperationReturnParameter();
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.Remove: removeTop(ref parser); return;
                case OperationParameter.OperationType.GetRemove: getRemoveTop(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (heap.Count != 0)
                    {
                        heap.Clear();
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.ReturnParameterSet(true);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 弹出数据
        /// </summary>
        /// <param name="parser"></param>
        private void removeTop(ref OperationParameter.NodeParser parser)
        {
            if (heap.Count != 0)
            {
                heap.RemoveTop();
                parser.SetOperationReturnParameter();
            }
            else parser.ReturnParameter.ReturnType = ReturnType.HeapIsEmpty;
        }
        /// <summary>
        /// 弹出数据并返回
        /// </summary>
        /// <param name="parser"></param>
        private unsafe void getRemoveTop(ref OperationParameter.NodeParser parser)
        {
            if (parser.ValueData.Type == ValueData.DataType.Bool)
            {
                if (heap.Count != 0)
                {
                    setReturnParameter(ref parser);
                    heap.RemoveTop();
                    parser.IsOperation = true;
                    parser.ReturnParameter.ReturnType = ReturnType.Success;
                }
                else parser.ReturnParameter.ReturnType = ReturnType.HeapIsEmpty;
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parser"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe void setReturnParameter(ref OperationParameter.NodeParser parser)
        {
            if (parser.ValueData.Int64.Bool) ValueData.Data<keyType>.SetData(ref parser.ReturnParameter, heap.Array[heap.Heap.Int[1]].Key);
            else ValueData.Data<valueType>.SetData(ref parser.ReturnParameter, heap.Array[heap.Heap.Int[1]].Value);
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
        internal override unsafe void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.ReturnParameterSet(heap.Count); return;
                case OperationParameter.OperationType.GetValue:
                    if (parser.ValueData.Type == ValueData.DataType.Bool)
                    {
                        if (heap.Count != 0)
                        {
                            setReturnParameter(ref parser);
                            parser.ReturnParameter.ReturnType = ReturnType.Success;
                        }
                        else parser.ReturnParameter.ReturnType = ReturnType.HeapIsEmpty;
                    }
                    else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
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
            Dispose();
        }
        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal unsafe override Snapshot.Node CreateSnapshot()
        {
            int* heapFixed = heap.Heap.Int;
            KeyValue<keyType, valueType>[] array = new KeyValue<keyType, valueType>[*heapFixed], heapArray = heap.Array;
            ++heapFixed;
            for (int index = 0; index != array.Length; ++index) array[index] = heapArray[heapFixed[index]];
            return new Snapshot.Value.Dictionary<keyType, valueType>(array);
        }
#if NOJIT
        /// <summary>
        /// 数组模拟最小堆 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static ArrayHeap<keyType, valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new ArrayHeap<keyType, valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<ArrayHeap<keyType, valueType>> nodeInfo;
        static ArrayHeap()
        {
            nodeInfo = new NodeInfo<ArrayHeap<keyType, valueType>>
            {
#if NOJIT
                Constructor = (Constructor<ArrayHeap<keyType, valueType>>)Delegate.CreateDelegate(typeof(Constructor<ArrayHeap<keyType, valueType>>), typeof(ArrayHeap<keyType, valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<ArrayHeap<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(ArrayHeap<keyType, valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
