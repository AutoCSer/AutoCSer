using System;
using System.Threading;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 数据序列化
    /// </summary>
    internal abstract class BinarySerializer
    {
        /// <summary>
        /// 数据序列化
        /// </summary>
        /// <param name="stream"></param>
        internal abstract void Serialize(UnmanagedStream stream);
        /// <summary>
        /// 二进制数据序列化
        /// </summary>
        internal static AutoCSer.BinarySerialize.Serializer Serializer;
    }
    /// <summary>
    /// 数据序列化
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class BinarySerializer<valueType> : BinarySerializer
    {
        /// <summary>
        /// 数据
        /// </summary>
        internal valueType Value;
        /// <summary>
        /// 数据序列化
        /// </summary>
        /// <param name="value">数据</param>
        internal BinarySerializer(valueType value)
        {
            Value = value;
        }
        /// <summary>
        /// 数据序列化
        /// </summary>
        /// <param name="stream"></param>
        internal override void Serialize(UnmanagedStream stream)
        {
            AutoCSer.BinarySerialize.Serializer binarySerializer = Interlocked.Exchange(ref Serializer, null);
            if (binarySerializer == null)
            {
                binarySerializer = AutoCSer.BinarySerialize.Serializer.YieldPool.Default.Pop() ?? new AutoCSer.BinarySerialize.Serializer();
                binarySerializer.SetTcpServer();
            }
            binarySerializer.SerializeTcpServer(Value, stream);
            if (Interlocked.CompareExchange(ref Serializer, binarySerializer, null) != null) binarySerializer.Free();
        }
    }
}
