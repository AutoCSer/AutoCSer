using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 数组 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Array<valueType> : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private valueType[] array = NullValue<valueType>.Array;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 数组 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Array(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.SetValue:
                    if (parser.ValueData.Type == ValueData.Data<valueType>.DataType)
                    {
                        valueType value = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                        if (parser.LoadValueData() && parser.IsEnd)
                        {
                            int index = parser.GetValueData(-1);
                            if (index >= 0)
                            {
                                if (index >= array.Length) array = array.copyNew(Math.Max(index + 1, array.Length << 1), array.Length);
                                array[index] = value;
                                parser.IsOperation = true;
                                if (index >= count) count = index + 1;
                                parser.ReturnParameter.Set(true);
                            }
                            else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
                            return null;
                        }
                    }
                    parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
                    break;
                default: parser.ReturnParameter.Type = ReturnType.OperationTypeError; break;
            }
            return null;
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
                case OperationParameter.OperationType.Clear:
                    if (array.Length != 0)
                    {
                        array = NullValue<valueType>.Array;
                        count = 0;
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.Set(true);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 删除数据
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
                    array[index] = default(valueType);
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
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(count); return;
                case OperationParameter.OperationType.GetValue:
                    int index = parser.GetValueData(-1);
                    if ((uint)index < count)
                    {
                        ValueData.Data<valueType>.SetData(ref parser.ReturnParameter.Parameter, array[index]);
                        parser.ReturnParameter.Type = ReturnType.Success;
                    }
                    else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
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
            valueType[] array = new valueType[count];
            Array.Copy(this.array, array, count);
            return new Snapshot.Value.Array<valueType>(array);
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
        private static Array<valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Array<valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Array<valueType>> nodeInfo;
        static Array()
        {
            nodeInfo = new NodeInfo<Array<valueType>>
            {
#if NOJIT
                Constructor = (Constructor<Array<valueType>>)Delegate.CreateDelegate(typeof(Constructor<Array<valueType>>), typeof(Array<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Array<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Array<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
