using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class Serializer
    {
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(SubArray<byte> value)
        {
            if (value.Length == 0) Stream.Write(0);
            else Serialize(Stream, ref value);
        }
        /// <summary>
        /// 预增数据流长度并序列化数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, ref SubArray<byte> data)
        {
            int dataLength = data.Length, length = (dataLength + 7) & (int.MaxValue - 3);
            stream.PrepLength(length);
            byte* write = stream.CurrentData;
            *(int*)(write + (length - sizeof(int))) = 0;
            *(int*)write = dataLength;
            fixed (byte* dataFixed = data.Array) AutoCSer.Memory.CopyNotNull(dataFixed + data.Start, write + sizeof(int), dataLength);
            stream.ByteSize += length;
            //stream.PrepLength();
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void subArraySerialize<valueType>(SubArray<valueType> value)
        {
            valueType[] array = value.ToArray();
            isReferenceArray = false;
            TypeSerializer<valueType[]>.DefaultSerializer(this, array);
        }

        ///// <summary>
        ///// 对象序列化
        ///// </summary>
        ///// <typeparam name="valueType">目标数据类型</typeparam>
        ///// <param name="value">数据对象</param>
        ///// <param name="stream">序列化输出缓冲区</param>
        ///// <param name="config">配置参数</param>
        ///// <returns>警告提示状态</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //private SerializeWarning serialize<valueType>(valueType value, UnmanagedStream stream, SerializeConfig config)
        //{
        //    Config = config ?? DefaultConfig;
        //    this.Stream.From(stream);
        //    try
        //    {
        //        serialize(value);
        //        return Warning;
        //    }
        //    finally { stream.From(this.Stream); }
        //}
        ///// <summary>
        ///// 对象序列化
        ///// </summary>
        ///// <typeparam name="valueType">目标数据类型</typeparam>
        ///// <param name="value">数据对象</param>
        ///// <param name="stream">序列化输出缓冲区</param>
        ///// <param name="config">配置参数</param>
        ///// <returns>警告提示状态</returns>
        //public static SerializeWarning Serialize<valueType>(valueType value, UnmanagedStream stream, SerializeConfig config = null)
        //{
        //    if (value == null)
        //    {
        //        stream.Write(NullValue);
        //        return SerializeWarning.None;
        //    }
        //    Serializer serializer = YieldPool.Default.Pop() ?? new Serializer();
        //    try
        //    {
        //        return serializer.serialize<valueType>(value, stream, config);
        //    }
        //    finally { serializer.Free(); }
        //}
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="data">数据</param>
        /// <param name="length">数据字节长度</param>
        /// <param name="config">序列化配置参数</param>
        /// <returns>警告提示状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SerializeWarning SerializeNotNull<valueType>(valueType value, byte* data, int length, SerializeConfig config)
        {
            Stream.Reset(data, length);
            Config = config;
            serialize(value);
            return Warning;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal UnmanagedStream SetTcpServer()
        {
            Config = DefaultConfig;
            return Stream;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        internal void SerializeTcpServer<valueType>(ref valueType value)
            where valueType : struct
        {
            //CurrentMemberMap = MemberMap = null;
            //Warning = SerializeWarning.None;
            if (isReferenceMember == TypeSerializer<valueType>.IsReferenceMember)
            {
                if (points != null) points.Clear();
            }
            else if (isReferenceMember) isReferenceMember = false;
            else
            {
                isReferenceMember = true;
                if (points == null) points = ReusableDictionary<ObjectReference>.Create<int>();
                else points.Clear();
            }
            //streamStartIndex = Stream.OffsetLength;
            streamStartIndex = Stream.ByteSize;
            isReferenceArray = true;
            Config.UnsafeWriteHeader(Stream);
            TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
            //Stream.Write(Stream.OffsetLength - streamStartIndex);
            Stream.Write(Stream.ByteSize - streamStartIndex);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        internal void SerializeTcpServerNext<valueType>(ref valueType value)
            where valueType : struct
        {
            //CurrentMemberMap = MemberMap = null;
            //Warning = SerializeWarning.None;
            //streamStartIndex = Stream.OffsetLength;
            streamStartIndex = Stream.ByteSize;
            if (points != null) points.Clear();
            isReferenceArray = true;
            Config.UnsafeWriteHeader(Stream);
            TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
            //Stream.Write(Stream.OffsetLength - streamStartIndex);
            Stream.Write(Stream.ByteSize - streamStartIndex);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="stream"></param>
        internal void SerializeTcpServer<valueType>(valueType value, UnmanagedStream stream)
        {
            if (value == null) stream.Write(NullValue);
            else
            {
                //CurrentMemberMap = MemberMap = null;
                //Warning = SerializeWarning.None;
                if (isReferenceMember == TypeSerializer<valueType>.IsReferenceMember)
                {
                    if (points != null) points.Clear();
                }
                else if (isReferenceMember) isReferenceMember = false;
                else
                {
                    isReferenceMember = true;
                    if (points == null) points = ReusableDictionary<ObjectReference>.Create<int>();
                    else points.Clear();
                }
                //streamStartIndex = Stream.OffsetLength;
                streamStartIndex = stream.ByteSize;
                isReferenceArray = true;
                Config.UnsafeWriteHeader(stream);
                Stream.From(stream);
                try
                {
                    //Warning = SerializeWarning.None;
                    TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
                }
                finally { stream.From(Stream); }
                //Stream.Write(Stream.OffsetLength - streamStartIndex);
                stream.Write(stream.ByteSize - streamStartIndex);
            }
        }

        /// <summary>
        /// 预编译类型
        /// </summary>
        /// <param name="types"></param>
        internal static void Compile(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type != null) GenericType.Get(type).BinarySerializeCompile();
            }
        }
    }
}
