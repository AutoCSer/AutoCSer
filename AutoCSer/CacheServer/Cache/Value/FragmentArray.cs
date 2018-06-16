using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 32768 基分段 数组 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class FragmentArray<valueType> : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private valueType[][] arrays = NullValue<valueType[]>.Array;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 32768 基分段 数组 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private FragmentArray(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
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
                case OperationParameter.OperationType.Update: update(ref parser); break;
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
            int indexParameter = parser.GetValueData(-1);
            if (indexParameter >= 0 && parser.LoadValueData() && parser.IsEnd)
            {
                if (parser.ValueData.Type == ValueData.Data<valueType>.DataType)
                {
                    valueType value = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                    int arrayIndex = indexParameter >> FragmentArray.ArrayShift;
                    if (arrayIndex >= arrays.Length)
                    {
                        valueType[][] newArrays = arrays.copyNew(arrayIndex + 1, arrays.Length);
                        for (int newIndex = arrays.Length; newIndex != newArrays.Length; newArrays[newIndex++] = new valueType[FragmentArray.ArraySize]) ;
                        arrays = newArrays;
                    }
                    arrays[arrayIndex][indexParameter & FragmentArray.ArraySizeAnd] = value;
                    if (indexParameter >= count) count = indexParameter + 1;
                    parser.SetOperationReturnParameter();
                }
                else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="parser"></param>
        private unsafe void update(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(-1);
            if ((uint)index < count)
            {
                byte* read = parser.Read;
                if (parser.LoadValueData() && !parser.IsEnd)
                {
                    valueType updateValue = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                    if (parser.LoadValueData() && parser.ValueData.Type == ValueData.DataType.UInt)
                    {
                        valueType[] array = arrays[index >> FragmentArray.ArrayShift];
                        valueType value = array[index &= FragmentArray.ArraySizeAnd];
                        uint type = parser.ValueData.Int64.UInt;
                        OperationUpdater.LogicType logicType = (OperationUpdater.LogicType)(byte)(type >> 16);
                        if (logicType != OperationUpdater.LogicType.None && parser.LoadValueData() && parser.IsEnd)
                        {
                            if (OperationUpdater.Data<valueType>.IsLogicData(logicType, value, ValueData.Data<valueType>.GetData(ref parser.ValueData))) logicType = OperationUpdater.LogicType.None;
                            else
                            {
                                parser.ReturnParameter.ReturnType = ReturnType.Success;
                                ValueData.Data<valueType>.SetData(ref parser.ReturnParameter, value);
                                return;
                            }
                        }
                        if (logicType == OperationUpdater.LogicType.None && parser.IsEnd)
                        {
                            switch (parser.ReturnParameter.ReturnType = OperationUpdater.Data<valueType>.UpdateData((OperationUpdater.OperationType)(ushort)type, ref value, updateValue))
                            {
                                case ReturnType.Success:
                                    array[index] = value;
                                    parser.UpdateOperation(read, value, OperationParameter.OperationType.SetValue);
                                    goto SETDATA;
                                case ReturnType.Unknown:
                                    parser.ReturnParameter.ReturnType = ReturnType.Success;
                                    SETDATA:
                                    ValueData.Data<valueType>.SetData(ref parser.ReturnParameter, value);
                                    return;
                            }
                            return;
                        }
                    }
                }
                parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
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
                    if (arrays.Length != 0)
                    {
                        arrays = NullValue<valueType[]>.Array;
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
            int index = parser.GetValueData(-1);
            if ((uint)index < count)
            {
                arrays[index >> FragmentArray.ArrayShift][index & FragmentArray.ArraySizeAnd] = default(valueType);
                parser.SetOperationReturnParameter();
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
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
                    int index = parser.GetValueData(-1);
                    if ((uint)index < count)
                    {
                        ValueData.Data<valueType>.SetData(ref parser.ReturnParameter, arrays[index >> FragmentArray.ArrayShift][index & FragmentArray.ArraySizeAnd]);
                        parser.ReturnParameter.ReturnType = ReturnType.Success;
                    }
                    else parser.ReturnParameter.ReturnType = ReturnType.ArrayIndexOutOfRange;
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
            foreach (valueType[] nodeArray in arrays)
            {
                if (nodeArray != null) Array.Copy(nodeArray, 0, array, index, Math.Min(FragmentArray.ArraySize, count - index));
                if ((index += FragmentArray.ArraySize) >= count) break;
            }
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
        private static FragmentArray<valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new FragmentArray<valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<FragmentArray<valueType>> nodeInfo;
        static FragmentArray()
        {
            nodeInfo = new NodeInfo<FragmentArray<valueType>>
            {
#if NOJIT
                Constructor = (Constructor<FragmentArray<valueType>>)Delegate.CreateDelegate(typeof(Constructor<FragmentArray<valueType>>), typeof(FragmentArray<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<FragmentArray<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(FragmentArray<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
