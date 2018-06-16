using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 节点解析
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct DataStructureParser
    {
        /// <summary>
        /// 当前解析位置
        /// </summary>
        internal byte* Read;
        /// <summary>
        /// 数据结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 节点解析
        /// </summary>
        /// <param name="start">当前解析位置</param>
        /// <param name="end">数据结束位置</param>
        internal DataStructureParser(byte* start, byte* end)
        {
            Read = start;
            this.end = end;
        }
        /// <summary>
        /// 解析节点
        /// </summary>
        /// <returns></returns>
        internal Type Parse()
        {
            Type valueType = getValueType(*(Read + 1)), keyType;
            byte nodeType = *Read;
            Read += 2;
            switch (nodeType)
            {
                case (byte)DataStructure.Abstract.NodeType.HashSet:
                    if (valueType != null) return parse(typeof(Cache.Value.HashSet<>).MakeGenericType(valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.FragmentHashSet:
                    if (valueType != null) return parse(typeof(Cache.Value.FragmentHashSet<>).MakeGenericType(valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.Link:
                    if (valueType != null) return parse(typeof(Cache.Value.Link<>).MakeGenericType(valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.ValueArray:
                    if (valueType != null) return parse(typeof(Cache.Value.Array<>).MakeGenericType(valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.ValueFragmentArray:
                    if (valueType != null) return parse(typeof(Cache.Value.FragmentArray<>).MakeGenericType(valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.ValueDictionary:
                    if (valueType != null && (keyType = getKeyType(*Read++)) != null) return parse(typeof(Cache.Value.Dictionary<,>).MakeGenericType(keyType, valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.ValueFragmentDictionary:
                    if (valueType != null && (keyType = getKeyType(*Read++)) != null) return parse(typeof(Cache.Value.FragmentDictionary<,>).MakeGenericType(keyType, valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.ValueSearchTreeDictionary:
                    if (valueType != null && (keyType = getKeyType(*Read++)) != null) return parse(typeof(Cache.Value.SearchTreeDictionary<,>).MakeGenericType(keyType, valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.ArrayHeap:
                    if (valueType != null && (keyType = getKeyType(*Read++)) != null) return parse(typeof(Cache.Value.ArrayHeap<,>).MakeGenericType(keyType, valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.Bitmap: return parse(typeof(Cache.Value.Bitmap));
                case (byte)DataStructure.Abstract.NodeType.Lock: return parse(typeof(Cache.Lock.Node));

                case (byte)DataStructure.Abstract.NodeType.MessageQueueConsumer:
                    if (valueType != null) return parse(typeof(Cache.MessageQueue.Consumer<>).MakeGenericType(valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.MessageQueueConsumers:
                    if (valueType != null) return parse(typeof(Cache.MessageQueue.Consumers<>).MakeGenericType(valueType));
                    break;
                case (byte)DataStructure.Abstract.NodeType.MessageDistributor:
                    if (valueType != null) return parse(typeof(Cache.MessageQueue.Distributor<>).MakeGenericType(valueType));
                    break;
            }
            return null;
        }
        /// <summary>
        /// 解析节点
        /// </summary>
        /// <param name="node">子节点类型</param>
        /// <returns></returns>
        private Type parse(Type node)
        {
            if (Read < end)
            {
                Type keyType;
                switch (*Read++)
                {
                    case (byte)DataStructure.Abstract.NodeType.Dictionary:
                        if ((keyType = getKeyType(*Read++)) != null) return parse(typeof(Cache.Dictionary<,>).MakeGenericType(keyType, node));
                        break;
                    case (byte)DataStructure.Abstract.NodeType.SearchTreeDictionary:
                        if ((keyType = getKeyType(*Read++)) != null) return parse(typeof(Cache.SearchTreeDictionary<,>).MakeGenericType(keyType, node));
                        break;
                    case (byte)DataStructure.Abstract.NodeType.Array: return parse(typeof(Cache.Array<>).MakeGenericType(node));
                    case (byte)DataStructure.Abstract.NodeType.FragmentDictionary:
                        if ((keyType = getKeyType(*Read++)) != null) return parse(typeof(Cache.FragmentDictionary<,>).MakeGenericType(keyType, node));
                        break;
                    case (byte)DataStructure.Abstract.NodeType.FragmentArray: return parse(typeof(Cache.FragmentArray<>).MakeGenericType(node));
                    case (byte)DataStructure.Abstract.NodeType.Unknown: return Read <= end ? node : null;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static Type getKeyType(byte dataType)
        {
            switch (dataType)
            {
                case (byte)ValueData.DataType.String: return typeof(string);
                case (byte)ValueData.DataType.Decimal: return typeof(decimal);
                case (byte)ValueData.DataType.Guid: return typeof(Guid);
                case (byte)ValueData.DataType.ULong: return typeof(ulong);
                case (byte)ValueData.DataType.Long: return typeof(long);
                case (byte)ValueData.DataType.UInt: return typeof(uint);
                case (byte)ValueData.DataType.Int: return typeof(int);
                case (byte)ValueData.DataType.UShort: return typeof(ushort);
                case (byte)ValueData.DataType.Short: return typeof(short);
                case (byte)ValueData.DataType.Byte: return typeof(byte);
                case (byte)ValueData.DataType.SByte: return typeof(sbyte);
                case (byte)ValueData.DataType.Char: return typeof(char);
                case (byte)ValueData.DataType.Float: return typeof(float);
                case (byte)ValueData.DataType.Double: return typeof(double);
                case (byte)ValueData.DataType.DateTime: return typeof(DateTime);
            }
            return null;
        }
        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static Type getValueType(byte dataType)
        {
            switch (dataType)
            {
                case (byte)ValueData.DataType.Json: return typeof(Cache.Value.Json);
                case (byte)ValueData.DataType.BinarySerialize: return typeof(Cache.Value.Binary);
                case (byte)ValueData.DataType.ByteArray: return typeof(byte[]);
                case (byte)ValueData.DataType.String: return typeof(string);
                case (byte)ValueData.DataType.Decimal: return typeof(decimal);
                case (byte)ValueData.DataType.Guid: return typeof(Guid);
                case (byte)ValueData.DataType.ULong: return typeof(ulong);
                case (byte)ValueData.DataType.Long: return typeof(long);
                case (byte)ValueData.DataType.UInt: return typeof(uint);
                case (byte)ValueData.DataType.Int: return typeof(int);
                case (byte)ValueData.DataType.UShort: return typeof(ushort);
                case (byte)ValueData.DataType.Short: return typeof(short);
                case (byte)ValueData.DataType.Byte: return typeof(byte);
                case (byte)ValueData.DataType.SByte: return typeof(sbyte);
                case (byte)ValueData.DataType.Char: return typeof(char);
                case (byte)ValueData.DataType.Bool: return typeof(bool);
                case (byte)ValueData.DataType.Float: return typeof(float);
                case (byte)ValueData.DataType.Double: return typeof(double);
                case (byte)ValueData.DataType.DateTime: return typeof(DateTime);
            }
            return null;
        }
    }
}
