using System;
using AutoCSer.TestCase.TcpServerPerformance;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// 服务端自定义序列化
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct ServerCustomSerialize
    {
        /// <summary>
        /// 自定义序列化计算回调输出
        /// </summary>
        internal ServerCustomSerializeOutput Output;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        public SubArray<byte> Buffer;

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
