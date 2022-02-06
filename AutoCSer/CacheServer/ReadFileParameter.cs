using AutoCSer.Memory;
using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 读取文件数据参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct ReadFileParameter
    {
        /// <summary>
        /// 文件读取器
        /// </summary>
        internal FileReader Reader;
        /// <summary>
        /// 数据
        /// </summary>
        internal SubArray<byte> Data;
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            int valueLength = Data.Length;
            byte* data = stream.GetBeforeMove((valueLength + (sizeof(int) + 3)) & (int.MaxValue - 3));
            *(int*)data = valueLength;
            fixed (byte* dataFixed = Data.GetFixedBuffer()) AutoCSer.Memory.Common.CopyNotNull(dataFixed + Data.Start, data += sizeof(int), valueLength);
            Reader.Next();
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            byte* read = deSerializer.Read;
            if (deSerializer.MoveRead((*(int*)read + (sizeof(int) + 3)) & (int.MaxValue - 3))) deSerializer.DeSerializeTcpServer(ref Data, read + sizeof(int), *(int*)read);
        }
    }
}
