using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 256 基分片 字典 数据节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class FragmentDictionary<keyType, valueType> : Node
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private readonly System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType>[] dictionarys = new System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType>[256];
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 256 基分片 字典 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private FragmentDictionary(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
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
                default: parser.ReturnParameter.Type = ReturnType.OperationTypeError; break;
            }
            return null;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="parser"></param>
        private void setValue(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key) && parser.LoadValueData() && parser.IsEnd && parser.ValueData.Type == ValueData.Data<valueType>.DataType)
            {
                valueType value = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType> dictionary = dictionarys[key.HashCode & 0xff];
                if (dictionary == null) dictionarys[key.HashCode & 0xff] = dictionary = AutoCSer.DictionaryCreator<HashCodeKey<keyType>>.Create<valueType>();
                int count = dictionary.Count;
                dictionary[key] = value;
                this.count += dictionary.Count - count;
                parser.SetOperationReturnParameter();
            }
            else parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="parser"></param>
        private unsafe void update(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                valueType value;
                System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType> dictionary = dictionarys[key.HashCode & 0xff];
                if (dictionary != null && dictionary.TryGetValue(key, out value))
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
                                    parser.ReturnParameter.Type = ReturnType.Success;
                                    ValueData.Data<valueType>.SetData(ref parser.ReturnParameter.Parameter, value);
                                    return;
                                }
                            }
                            if (logicType == OperationUpdater.LogicType.None && parser.IsEnd)
                            {
                                switch (parser.ReturnParameter.Type = OperationUpdater.Data<valueType>.UpdateData((OperationUpdater.OperationType)(ushort)type, ref value, updateValue))
                                {
                                    case ReturnType.Success:
                                        dictionary[key] = value;
                                        parser.UpdateOperation(read, value, OperationParameter.OperationType.SetValue);
                                        goto SETDATA;
                                    case ReturnType.Unknown:
                                        parser.ReturnParameter.Type = ReturnType.Success;
                                        SETDATA:
                                        ValueData.Data<valueType>.SetData(ref parser.ReturnParameter.Parameter, value);
                                        return;
                                }
                                return;
                            }
                        }
                    }
                }
                else
                {
                    parser.ReturnParameter.Type = ReturnType.NotFoundDictionaryKey;
                    return;
                }
            }
            parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
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
                    if (count != 0)
                    {
                        Array.Clear(dictionarys, 0, 256);
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
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType> dictionary = dictionarys[key.HashCode & 0xff];
                if (dictionary != null && dictionary.Remove(key))
                {
                    --count;
                    parser.SetOperationReturnParameter();
                }
                else parser.ReturnParameter.Set(false);
            }
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
            HashCodeKey<keyType> key;
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(count); return;
                case OperationParameter.OperationType.GetValue:
                    if (HashCodeKey<keyType>.Get(ref parser, out key))
                    {
                        System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType> dictionary = dictionarys[key.HashCode & 0xff];
                        valueType value;
                        if (dictionary != null && dictionary.TryGetValue(key, out value))
                        {
                            ValueData.Data<valueType>.SetData(ref parser.ReturnParameter.Parameter, value);
                            parser.ReturnParameter.Type = ReturnType.Success;
                        }
                        else parser.ReturnParameter.Type = ReturnType.NotFoundDictionaryKey;
                    }
                    return;
                case OperationParameter.OperationType.ContainsKey:
                    if (HashCodeKey<keyType>.Get(ref parser, out key))
                    {
                        System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType> dictionary = dictionarys[key.HashCode & 0xff];
                        parser.ReturnParameter.Set(dictionary != null && dictionary.ContainsKey(key));
                    }
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
            KeyValue<keyType, valueType>[] array = new KeyValue<keyType, valueType>[count];
            int index = 0;
            foreach (System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType> dictionary in dictionarys)
            {
                if (dictionary != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<HashCodeKey<keyType>, valueType> node in dictionary) array[index++].Set(node.Key.Value, node.Value);
                }
            }
            return new Snapshot.Value.Dictionary<keyType, valueType>(array);
        }
#if NOJIT
        /// <summary>
        /// 创建字典 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static FragmentDictionary<keyType, valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new FragmentDictionary<keyType, valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<FragmentDictionary<keyType, valueType>> nodeInfo;
        static FragmentDictionary()
        {
            nodeInfo = new NodeInfo<FragmentDictionary<keyType, valueType>>
            {
#if NOJIT
                Constructor = (Constructor<FragmentDictionary<keyType, valueType>>)Delegate.CreateDelegate(typeof(Constructor<FragmentDictionary<keyType, valueType>>), typeof(FragmentDictionary<keyType, valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<FragmentDictionary<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(FragmentDictionary<keyType, valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
