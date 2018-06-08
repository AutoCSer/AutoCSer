using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.OperationParameter
{
    /// <summary>
    /// 操作数据序列化
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct Serializer
    {
        /// <summary>
        /// 操作类型位置
        /// </summary>
        internal const int OperationTypeOffset = sizeof(int);
        /// <summary>
        /// 头部字节大小
        /// </summary>
        internal const int HeaderSize = OperationTypeOffset + sizeof(uint);

        /// <summary>
        /// 序列化流
        /// </summary>
        internal UnmanagedStream Stream;
        /// <summary>
        /// 数据起始位置
        /// </summary>
        private int startIndex;
        /// <summary>
        /// 操作数据序列化
        /// </summary>
        /// <param name="stream">序列化流</param>
        internal Serializer(UnmanagedStream stream)
        {
            Stream = stream;
            startIndex = stream.ByteSize;
            stream.PrepLength(HeaderSize + IndexIdentity.SerializeSize + sizeof(int));
            stream.ByteSize += HeaderSize;
        }
        /// <summary>
        /// 操作数据序列化
        /// </summary>
        /// <param name="stream">序列化流</param>
        /// <param name="identitySize"></param>
        internal Serializer(UnmanagedStream stream, int identitySize)
        {
            Stream = stream;
            startIndex = stream.ByteSize;
            stream.PrepLength(HeaderSize + identitySize);
            stream.ByteSize += HeaderSize;
        }
        /// <summary>
        /// 序列化结束处理
        /// </summary>
        /// <param name="type">操作类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void End(OperationType type)
        {
            byte* write = Stream.Data.Byte + startIndex;
            *(int*)write = Stream.ByteSize - startIndex;
            *(uint*)(write + OperationTypeOffset) = (ushort)type;
        }
        /// <summary>
        /// 获取操作数据
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <returns></returns>
        internal static Buffer GetOperationData(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            byte* read = deSerializer.Read;
            int size = *(int*)read;
            if (deSerializer.MoveRead(size))
            {
                Buffer buffer = BufferCount.GetBuffer(size);
                deSerializer.CopyTo(read, ref buffer.Array);
                return buffer;
            }
            return null;
        }
        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="queryData"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void GetQueryData(AutoCSer.BinarySerialize.DeSerializer deSerializer, ref SubArray<byte> queryData)
        {
            byte* read = deSerializer.Read;
            if (deSerializer.MoveRead(*(int*)read)) deSerializer.DeSerializeTcpServer(ref queryData, read, *(int*)read);
        }
    }
}
