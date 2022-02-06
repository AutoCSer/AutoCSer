using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<T>
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SerializeTcpServer(BinarySerializer serializer, ref T value)
        {
            if (DefaultSerializer == null)
            {
                UnmanagedStream stream = serializer.Stream;
                stream.PrepSize(fixedSize);
                stream.Data.Write(memberCountVerify);
                fixedMemberSerializer(serializer, value);
                stream.Data.CurrentIndex += fixedFillSize;
                //stream.PrepLength();
                if (memberSerializer != null) memberSerializer(serializer, value);
                if (jsonMemberMap == null)
                {
                    if (isJson) stream.Write(0);
                }
                else AutoCSer.JsonSerializer.Serialize(ref value, stream, serializer.GetJsonConfig(jsonMemberMap));
            }
            else DefaultSerializer(serializer, value);
        }
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Compile() { }
    }
}
