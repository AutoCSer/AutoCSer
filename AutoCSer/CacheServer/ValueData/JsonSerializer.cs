using System;
using System.Threading;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 数据序列化
    /// </summary>
    internal static class JsonSerializer
    {
        /// <summary>
        /// 输出数据 JSON 序列化
        /// </summary>
        internal static Json.Serializer Serializer;
    }
    /// <summary>
    /// 数据序列化
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class JsonSerializer<valueType> : BinarySerializer
    {
        /// <summary>
        /// 数据
        /// </summary>
        internal valueType Value;
        /// <summary>
        /// 数据序列化
        /// </summary>
        /// <param name="value">数据</param>
        internal JsonSerializer(valueType value)
        {
            Value = value;
        }
        /// <summary>
        /// 数据序列化
        /// </summary>
        /// <param name="stream"></param>
        internal override unsafe void Serialize(UnmanagedStream stream)
        {
            AutoCSer.Json.Serializer jsonSerializer = Interlocked.Exchange(ref JsonSerializer.Serializer, null);
            if (jsonSerializer == null)
            {
                jsonSerializer = AutoCSer.Json.Serializer.YieldPool.Default.Pop() ?? new AutoCSer.Json.Serializer();
                jsonSerializer.SetTcpServer();
            }
            jsonSerializer.SerializeTcpServer(Value, stream);
            if (Interlocked.CompareExchange(ref JsonSerializer.Serializer, jsonSerializer, null) != null) jsonSerializer.Free();
        }
    }
}
