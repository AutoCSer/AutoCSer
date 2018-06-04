using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.OperationParameter
{
    /// <summary>
    /// 查询参数
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct QueryNode
    {
        /// <summary>
        /// 数据结构定义节点
        /// </summary>
        internal DataStructure.Abstract.Node Node;
        /// <summary>
        /// 数据结构定义节点查询数据包
        /// </summary>
        internal SubArray<byte> QueryData;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            Serializer operationSerializer = new Serializer(serializer.Stream);
            Node.SerializeParameter(operationSerializer.Stream);
            operationSerializer.End(Node.Parameter.OperationType);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            Serializer.GetQueryData(deSerializer, ref QueryData);
        }
    }
}
