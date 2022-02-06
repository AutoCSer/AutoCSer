using AutoCSer.Memory;
using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 客户端缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ClientBuffer
    {
        /// <summary>
        /// 客户端缓冲区
        /// </summary>
        internal SubArray<byte> Buffer;
        /// <summary>
        /// 成员状态
        /// </summary>
        internal MemberState State;
        /// <summary>
        /// 是否客户端对象
        /// </summary>
        internal bool IsClient;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void serialize(AutoCSer.BinarySerializer serializer)
        {
            int size = Buffer.Length;
            if (IsClient) serializer.Stream.Write(-size);
            else
            {
                UnmanagedStream stream = serializer.Stream;
                if (size == 0)
                {
                    byte* write = stream.GetBeforeMove(sizeof(int) * 2);
                    *(int*)write = size;
                    *(int*)(write + sizeof(int)) = (int)(byte)State;
                }
                else
                {
                    int offset = -size & 3;
                    byte* write = stream.GetBeforeMove(size + offset + sizeof(int));
                    *(int*)write = size;
                    Buffer.CopyTo(new Span<byte>(write + sizeof(int), size));
                }
            }
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
            if (size <= 0)
            {
                if (size == 0)
                {
                    Buffer.SetNull();
                    State = (MemberState)(byte)*(int*)(read + sizeof(int));
                    deSerializer.Read += sizeof(int) * 2;
                }
                else
                {
                    Buffer.Start = -size;
                    deSerializer.Read += sizeof(int);
                }
            }
            else if (size == Buffer.Length)
            {
                if (deSerializer.MoveReadAny(sizeof(int) + size + (-size & 3)))
                {
                    fixed (byte* bufferFixed = Buffer.GetFixedBuffer()) AutoCSer.Memory.Common.CopyNotNull(read + sizeof(int), bufferFixed + Buffer.Start, size);
                    State = MemberState.Remote;
                }
            }
            else deSerializer.State = BinarySerialize.DeSerializeState.IndexOutOfRange;
        }
    }
}
