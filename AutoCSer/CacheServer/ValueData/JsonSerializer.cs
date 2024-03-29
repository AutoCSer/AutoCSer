﻿using AutoCSer.Memory;
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
        internal static AutoCSer.JsonSerializer Serializer;
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
            if (Value == null)
            {
                *(long*)stream.GetBeforeMove(4 * sizeof(char)) = 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48);
            }
            else
            {
                AutoCSer.JsonSerializer jsonSerializer = Interlocked.Exchange(ref JsonSerializer.Serializer, null);
                if (jsonSerializer == null)
                {
                    jsonSerializer = AutoCSer.JsonSerializer.YieldPool.Default.Pop() ?? new AutoCSer.JsonSerializer();
                    jsonSerializer.SetTcpServer();
                }
                jsonSerializer.SerializeTcpServer(ref Value, stream);
                if (Interlocked.CompareExchange(ref JsonSerializer.Serializer, jsonSerializer, null) != null) jsonSerializer.Free();
            }
        }
    }
}
