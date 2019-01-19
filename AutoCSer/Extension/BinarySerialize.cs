using System;
using System.Runtime.CompilerServices;
using AutoCSer.BinarySerialize;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 二进制序列化扩展操作
    /// </summary>
    public static partial class BinarySerialize
    {
        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        public static SerializeResult serialize<valueType>(this valueType value, SerializeConfig config = null)
        {
            return Serializer.Serialize(value, config);
        }

        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="data">数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult deSerialize<valueType>(this valueType value, byte[] data, DeSerializeConfig config = null)
        {
            return DeSerializer.DeSerialize(data, ref value, config);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="data">数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static DeSerializeResult deSerialize<valueType>(this valueType value, LeftArray<byte> data, DeSerializeConfig config = null)
        {
            if (data.Length == 0) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            fixed (byte* dataFixed = data.Array) return DeSerializer.DeSerialize<valueType>(data.Array, dataFixed, data.Length, ref value, config);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="data">数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static DeSerializeResult deSerialize<valueType>(this valueType value, ref LeftArray<byte> data, DeSerializeConfig config = null)
        {
            if (data.Length == 0) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            fixed (byte* dataFixed = data.Array) return DeSerializer.DeSerialize<valueType>(data.Array, dataFixed, data.Length, ref value, config);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="startIndex">数据起始位置</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static DeSerializeResult deSerialize<valueType>(this valueType value, UnmanagedStream data, int startIndex = 0, DeSerializeConfig config = null)
        {
            if (data == null) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            return DeSerializer.DeSerialize<valueType>(null, data.Data.Byte + startIndex, data.ByteSize - startIndex, ref value, config);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">目标对象</param>
        /// <param name="data">数据</param>
        /// <param name="size">数据字节长度</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static DeSerializeResult deSerialize<valueType>(this valueType value, byte* data, int size, DeSerializeConfig config = null)
        {
            if (data == null) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            return DeSerializer.DeSerialize<valueType>(null, data, size, ref value, config);
        }
    }
}
