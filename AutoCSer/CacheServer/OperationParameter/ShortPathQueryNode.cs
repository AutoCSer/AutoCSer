using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.OperationParameter
{
    /// <summary>
    /// 短路径 查询参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct ShortPathQueryNode
    {
        /// <summary>
        /// 短路径查询节点
        /// </summary>
        internal ShortPath.Parameter.Node Node;
        /// <summary>
        /// 数据结构定义节点查询数据包
        /// </summary>
        internal SubArray<byte> QueryData;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerializer serializer)
        {
            Serializer operationSerializer = new Serializer(serializer.Stream, ShortPathIdentity.SerializeSize + sizeof(int));
            Node.SerializeParameter(operationSerializer.Stream);
            operationSerializer.End(Node.Parameter.OperationType);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            Serializer.GetQueryData(deSerializer, ref QueryData);
        }
    }
}
