using AutoCSer.Memory;
using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存数据参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct CacheReturnParameter
    {
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        internal CacheGetter Getter;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal Buffer Buffer;
        /// <summary>
        /// 加载数据
        /// </summary>
        internal SubArray<byte> LoadData;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerializer serializer)
        {
            if (Getter != null) Getter.Serialize(serializer.Stream);
            else if (Buffer != null)
            {
                UnmanagedStream stream = serializer.Stream;
                int startIndex = stream.AddSize(sizeof(int));
                stream.Write(ref Buffer.Array);
                *(int*)(stream.Data.Byte + (startIndex - sizeof(int))) = stream.Data.CurrentIndex - startIndex;
                Buffer.FreeReference();
            }
            else serializer.Stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            if (deSerializer.CheckNullValue() != 0)
            {
                byte* read = deSerializer.Read;
                deSerializer.DeSerializeTcpServer(ref LoadData, read + sizeof(int), *(int*)read);
                deSerializer.MoveRead(LoadData.Length + sizeof(int));
            }
        }
    }
}
