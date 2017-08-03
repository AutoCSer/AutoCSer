using System;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// 客户端自定义序列化
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct ClientCustomSerialize
    {
        /// <summary>
        /// 
        /// </summary>
        internal ClientCustomSerializeOutput Output;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubArray<byte> Buffer;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">对象序列化器</param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        private unsafe void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            Output.Serialize(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            if (deSerializer.GetBuffer(ref Buffer))
            {
                int size;
                fixed (byte* bufferFixed = Buffer.BufferArray) size = *(int*)(bufferFixed + Buffer.StartIndex);
                if (deSerializer.MoveRead(size)) Buffer = Buffer.GetSub(sizeof(int), size - sizeof(int));
            }
            else deSerializer.MoveRead(-1);
        }
    }
}
