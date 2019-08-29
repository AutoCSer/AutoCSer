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
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            int size = Buffer.Length;
            if (IsClient) serializer.Stream.Write(-size);
            else
            {
                UnmanagedStream stream = serializer.Stream;
                if (size == 0)
                {
                    stream.PrepLength(sizeof(int) * 2);
                    stream.UnsafeWrite(size);
                    stream.UnsafeWrite((int)(byte)State);
                }
                else
                {
                    int offset = -size & 3;
                    stream.PrepLength(size + offset + sizeof(int));
                    stream.UnsafeWrite(size);
                    stream.UnsafeWriteNotEmpty(ref Buffer);
                    stream.ByteSize += offset;
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer">序列化数据</param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
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
                    fixed (byte* bufferFixed = Buffer.Array) AutoCSer.Memory.CopyNotNull(read + sizeof(int), bufferFixed + Buffer.Start, size);
                    State = MemberState.Remote;
                }
            }
            else deSerializer.State = BinarySerialize.DeSerializeState.IndexOutOfRange;
        }
    }
}
