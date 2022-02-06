using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    internal unsafe static partial class TypeDeSerializer<T>
    {
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="deSerializer">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerializeTcpServer(JsonDeSerializer deSerializer, ref T value)
        {
            if (DefaultDeSerializer == null)
            {
                if (deSerializer.SearchObject()) DeSerializeMembers(deSerializer, ref value);
                else value = default(T);
            }
            else DefaultDeSerializer(deSerializer, ref value);
        }
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Compile() { }
    }
}
