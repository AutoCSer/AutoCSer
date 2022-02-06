using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    internal unsafe partial class TypeDeSerializer<T>
    {
        /// <summary>
        /// 对象反序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerializeTcpServer(BinaryDeSerializer deSerializer, ref T value)
        {
            if (DefaultDeSerializer == null) GetDeSerializer(deSerializer.GlobalVersion).deSerializeTcpServer(deSerializer, ref value);
            else DefaultDeSerializer(deSerializer, ref value);
        }
        /// <summary>
        /// 对象反序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        private void deSerializeTcpServer(BinaryDeSerializer deSerializer, ref T value)
        {
            if (deSerializer.CheckMemberCount(memberCountVerify))
            {
                fixedMemberDeSerializer(deSerializer, ref value);
                deSerializer.Read += fixedFillSize;
                if (memberDeSerializer != null) memberDeSerializer(deSerializer, ref value);
                if (isJson || jsonMemberMap != null) deSerializer.DeSerializeJson(ref value);
            }
            else deSerializer.State = DeSerializeState.MemberMap;
        }
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Compile() { }
    }
}
