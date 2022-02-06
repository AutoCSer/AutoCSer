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
        [AutoCSer.BinarySerializeCustom]
        private unsafe void serialize(AutoCSer.BinarySerializer serializer)
        {
            Output.Serialize(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerializeCustom]
        private void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
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
