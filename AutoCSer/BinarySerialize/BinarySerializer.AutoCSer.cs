using System;
using System.Reflection;
using AutoCSer.Extensions;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
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
        public void CallSerialize(SubArray<byte> value)
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
            int dataSize = data.Length;
            byte* write = stream.GetBeforeMove((dataSize + (3 + sizeof(int))) & (int.MaxValue - 3));
            *(int*)write = dataSize;
            data.CopyTo(new Span<byte>(write + sizeof(int), dataSize));
            stream.Data.SerializeFillLeftByteSize(dataSize);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SubArraySerialize<valueType>(BinarySerializer binarySerializer, SubArray<valueType> value)
        {
            valueType[] array = value.ToArray();
            binarySerializer.isReferenceArray = false;
            AutoCSer.BinarySerialize.TypeSerializer<valueType[]>.DefaultSerializer(binarySerializer, array);
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
        internal AutoCSer.BinarySerialize.SerializeWarning SerializeNotNull<valueType>(ref valueType value, byte* data, int length, AutoCSer.BinarySerialize.SerializeConfig config)
        {
            Stream.Reset(data, length);
            Config = config;
            serialize(ref value);
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
            if (isReferenceMember == AutoCSer.BinarySerialize.TypeSerializer<valueType>.IsReferenceMember)
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
            streamStartIndex = Stream.Data.CurrentIndex;
            isReferenceArray = true;
            Config.UnsafeWriteHeader(Stream);
            AutoCSer.BinarySerialize.TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
            //Stream.Write(Stream.OffsetLength - streamStartIndex);
            Stream.Write(Stream.Data.CurrentIndex - streamStartIndex);
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
            streamStartIndex = Stream.Data.CurrentIndex;
            if (points != null) points.Clear();
            isReferenceArray = true;
            Config.UnsafeWriteHeader(Stream);
            AutoCSer.BinarySerialize.TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
            //Stream.Write(Stream.OffsetLength - streamStartIndex);
            Stream.Write(Stream.Data.CurrentIndex - streamStartIndex);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="stream"></param>
        internal void SerializeTcpServer<valueType>(ref valueType value, UnmanagedStream stream)
        {
            //CurrentMemberMap = MemberMap = null;
            //Warning = SerializeWarning.None;
            if (isReferenceMember == AutoCSer.BinarySerialize.TypeSerializer<valueType>.IsReferenceMember)
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
            streamStartIndex = stream.Data.CurrentIndex;
            isReferenceArray = true;
            Config.UnsafeWriteHeader(stream);
            Stream.From(stream);
            try
            {
                //Warning = SerializeWarning.None;
                AutoCSer.BinarySerialize.TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
            }
            finally { stream.From(Stream); }
            //Stream.Write(Stream.OffsetLength - streamStartIndex);
            stream.Write(stream.Data.CurrentIndex - streamStartIndex);
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
