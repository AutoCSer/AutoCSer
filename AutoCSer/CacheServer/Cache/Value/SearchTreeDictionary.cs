using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 搜索树字典 数据节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class SearchTreeDictionary<keyType, valueType> : Node
        where keyType : IEquatable<keyType>, IComparable<keyType>
    {
        /// <summary>
        /// 搜索树字典
        /// </summary>
        internal readonly AutoCSer.SearchTree.Dictionary<keyType, valueType> Dictionary = new AutoCSer.SearchTree.Dictionary<keyType, valueType>();
        /// <summary>
        /// 搜索树字典 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private SearchTreeDictionary(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
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
            keyType key;
            if (HashCodeKey<keyType>.Get(ref parser, out key) && parser.LoadValueData() && parser.IsEnd && parser.ValueData.Type == ValueData.Data<valueType>.DataType)
            {
                valueType value = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                Dictionary.Set(key, value);
                parser.SetOperationReturnParameter();
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="parser"></param>
        private unsafe void update(ref OperationParameter.NodeParser parser)
        {
            keyType key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                valueType value;
                if (Dictionary.TryGetValue(key, out value))
                {
                    byte* read = parser.Read;
                    if (parser.LoadValueData() && !parser.IsEnd)
                    {
                        valueType updateValue = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                        if (parser.LoadValueData() && parser.ValueData.Type == ValueData.DataType.UInt)
                        {
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
                                        Dictionary.Set(key, value);
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
                }
                else
                {
                    parser.ReturnParameter.ReturnType = ReturnType.NotFoundDictionaryKey;
                    return;
                }
            }
            parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
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
                    if (Dictionary.Count != 0)
                    {
                        Dictionary.Clear();
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
            keyType key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                if (Dictionary.Remove(key)) parser.SetOperationReturnParameter();
                else parser.ReturnParameter.ReturnParameterSet(false);
            }
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
            keyType key;
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.ReturnParameterSet(Dictionary.Count); return;
                case OperationParameter.OperationType.GetValue:
                    if (HashCodeKey<keyType>.Get(ref parser, out key))
                    {
                        valueType value;
                        if (Dictionary.TryGetValue(key, out value))
                        {
                            ValueData.Data<valueType>.SetData(ref parser.ReturnParameter, value);
                            parser.ReturnParameter.ReturnType = ReturnType.Success;
                        }
                        else parser.ReturnParameter.ReturnType = ReturnType.NotFoundDictionaryKey;
                    }
                    return;
                case OperationParameter.OperationType.ContainsKey:
                    if (HashCodeKey<keyType>.Get(ref parser, out key)) parser.ReturnParameter.ReturnParameterSet(Dictionary.ContainsKey(key));
                    return;
                case OperationParameter.OperationType.GetValues:
                    if (parser.OnReturn != null)
                    {
                        if (parser.ValueData.Type == ValueData.DataType.ULong)
                        {
                            ulong count = parser.ValueData.Int64.ULong;
                            int skipCount = (int)(uint)count, getCount = (int)(uint)(count >> 32);
                            if (skipCount >= 0 && getCount != 0)
                            {
                                parser.Cache.TcpServer.CallQueue.Add(new ServerCall.SearchTreeDictionaryGetter<keyType, valueType>(this, skipCount, getCount, ref parser));
                            }
                        }
                        parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
                    }
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
            KeyValue<keyType, valueType>[] array = new KeyValue<keyType, valueType>[Dictionary.Count];
            int index = 0;
            foreach (KeyValue<keyType, valueType> node in Dictionary.KeyValues) array[index++] = node;
            return new Snapshot.Value.Dictionary<keyType, valueType>(array);
        }
#if NOJIT
        /// <summary>
        /// 创建搜索树字典 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static SearchTreeDictionary<keyType, valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new SearchTreeDictionary<keyType, valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<SearchTreeDictionary<keyType, valueType>> nodeInfo;
        static SearchTreeDictionary()
        {
            nodeInfo = new NodeInfo<SearchTreeDictionary<keyType, valueType>>
            {
#if NOJIT
                Constructor = (Constructor<SearchTreeDictionary<keyType, valueType>>)Delegate.CreateDelegate(typeof(Constructor<SearchTreeDictionary<keyType, valueType>>), typeof(SearchTreeDictionary<keyType, valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<SearchTreeDictionary<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(SearchTreeDictionary<keyType, valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
