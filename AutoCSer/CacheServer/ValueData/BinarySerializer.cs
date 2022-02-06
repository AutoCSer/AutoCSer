using AutoCSer.Memory;
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
        internal static AutoCSer.BinarySerializer Serializer;
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
            if (Value == null) stream.Write(AutoCSer.BinarySerializer.NullValue);
            else
            {
                AutoCSer.BinarySerializer binarySerializer = Interlocked.Exchange(ref Serializer, null);
                if (binarySerializer == null)
                {
                    binarySerializer = AutoCSer.BinarySerializer.YieldPool.Default.Pop() ?? new AutoCSer.BinarySerializer();
                    binarySerializer.SetTcpServer();
                }
                binarySerializer.SerializeTcpServer(ref Value, stream);
                if (Interlocked.CompareExchange(ref Serializer, binarySerializer, null) != null) binarySerializer.Free();
            }
        }
    }
}
