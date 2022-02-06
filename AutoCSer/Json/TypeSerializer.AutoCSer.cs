using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<T>
    {
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void SerializeTcpServer(JsonSerializer jsonSerializer, ref T value)
        {
            if (defaultSerializer == null)
            {
                CharStream jsonStream = jsonSerializer.CharStream;
                //jsonStream.PrepLength(2);
                jsonStream.Data.Write('{');
                memberSerializer(jsonSerializer, value);
                jsonStream.Write('}');
            }
            else defaultSerializer(jsonSerializer, value);
        }
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Compile() { }
    }
}
