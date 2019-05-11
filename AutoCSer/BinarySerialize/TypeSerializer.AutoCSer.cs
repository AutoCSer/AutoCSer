using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SerializeTcpServer(Serializer serializer, ref valueType value)
        {
            if (DefaultSerializer == null)
            {
                UnmanagedStream stream = serializer.Stream;
                stream.PrepLength(fixedSize);
                stream.UnsafeWrite(memberCountVerify);
                fixedMemberSerializer(serializer, value);
                stream.ByteSize += fixedFillSize;
                //stream.PrepLength();
                if (memberSerializer != null) memberSerializer(serializer, value);
                if (jsonMemberMap == null)
                {
                    if (isJson) stream.Write(0);
                }
                else AutoCSer.Json.Serializer.Serialize(value, stream, serializer.GetJsonConfig(jsonMemberMap));
            }
            else DefaultSerializer(serializer, value);
        }
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Compile() { }
    }
}
