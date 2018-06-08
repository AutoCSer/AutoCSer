using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.OperationParameter
{
    /// <summary>
    /// 短路径操作参数
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct ShortPathOperationNode
    {
        /// <summary>
        /// 短路径查询节点
        /// </summary>
        internal ShortPath.Parameter.Node Node;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal Buffer Buffer;
        /// <summary>
        /// 短路径索引标识
        /// </summary>
        internal ShortPathIdentity Identity;
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
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            byte* read = deSerializer.Read;
            int size = *(int*)read;
            if (deSerializer.MoveRead(size))
            {
                Identity = new ShortPathIdentity(read + Serializer.HeaderSize);
                int bufferSize = size + Identity.PacketSize - ShortPathIdentity.SerializeSize;
                if (bufferSize >= Serializer.HeaderSize + IndexIdentity.SerializeSize)
                {
                    Buffer = BufferCount.GetBuffer(bufferSize);
                    fixed (byte* bufferFixed = Buffer.Array.Array)
                    {
                        byte* start = bufferFixed + Buffer.Array.Start;
                        *(int*)start = Buffer.Array.Length;
                        *(uint*)(start + sizeof(int)) = *(uint*)(read + sizeof(int));
                    }
                    deSerializer.CopyTo(read + (Serializer.HeaderSize + ShortPathIdentity.SerializeSize), ref Buffer.Array, Serializer.HeaderSize + Identity.PacketSize);
                }
            }
        }
    }
}
