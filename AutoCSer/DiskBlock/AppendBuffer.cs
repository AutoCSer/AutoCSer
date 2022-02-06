using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 添加数据缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct AppendBuffer
    {
        /// <summary>
        /// 默认磁盘块索引位置
        /// </summary>
        internal ulong Index;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubArray<byte> Buffer;
        /// <summary>
        /// 磁盘块客户端编号
        /// </summary>
        internal ushort BlockIndex;
        /// <summary>
        /// 检测磁盘块编号
        /// </summary>
        /// <param name="blockIndex">磁盘块编号</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckBlockIndex(int blockIndex)
        {
            if (BlockIndex == blockIndex)
            {
                if ((int)(Index >> Server.IndexShift) != blockIndex) Index = 0;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            int size = Buffer.Length, offset = -(size + sizeof(ushort)) & 3;
            byte* write = stream.GetBeforeMove(size + offset + (sizeof(int) + sizeof(ulong) + sizeof(ushort)));
            *(int*)write = size;
            *(ulong*)(write + sizeof(int)) = Index;
            Buffer.CopyTo(new Span<byte>(write + (sizeof(int) + sizeof(ulong)), size));
            *(ushort*)(write + (size + (sizeof(int) + sizeof(ulong)))) = BlockIndex;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer">序列化数据</param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            byte* read = deSerializer.Read;
            int size = *(int*)read;
            if (size > 0)
            {
                int offset = -(size + sizeof(ushort)) & 3;
                if (deSerializer.MoveReadAny(size + offset + (sizeof(int) + sizeof(ulong) + sizeof(ushort))))
                {
                    Index = *(ulong*)(read + sizeof(int));
                    deSerializer.DeSerializeTcpServer(ref Buffer, read += sizeof(int) + sizeof(ulong), size);
                    BlockIndex = *(ushort*)(read + size);
                }
            }
            else deSerializer.State = BinarySerialize.DeSerializeState.IndexOutOfRange;
        }
    }
}
