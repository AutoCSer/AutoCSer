using System;
using System.Reflection;
using AutoCSer.Extensions;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
using AutoCSer.BinarySerialize;

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer : AutoCSer.Threading.Link<BinarySerializer>, IDisposable
    {
        /// <summary>
        /// 空对象
        /// </summary>
        public const int NullValue = int.MinValue;
        /// <summary>
        /// 可空类型存在数据
        /// </summary>
        public const int NullableHasValue = int.MaxValue;
        /// <summary>
        /// 真实类型
        /// </summary>
        public const int RealTypeValue = NullValue + 1;
        /// <summary>
        /// 默认二进制数据序列化类型配置
        /// </summary>
        internal static readonly BinarySerializeAttribute DefaultAttribute = (BinarySerializeAttribute)AutoCSer.Configuration.Common.Get(typeof(BinarySerializeAttribute)) ?? new BinarySerializeAttribute { IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly SerializeConfig DefaultConfig = (SerializeConfig)AutoCSer.Configuration.Common.Get(typeof(SerializeConfig)) ?? new SerializeConfig();
        /// <summary>
        /// 序列化输出缓冲区
        /// </summary>
        public readonly UnmanagedStream Stream = new UnmanagedStream(default(AutoCSer.Memory.Pointer));
        /// <summary>
        /// 序列化配置参数
        /// </summary>
        internal SerializeConfig Config;
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap MemberMap;
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap CurrentMemberMap;
        ///// <summary>
        ///// JSON序列化输出缓冲区
        ///// </summary>
        //private CharStream jsonStream;
        /// <summary>
        /// JSON序列化成员位图
        /// </summary>
        internal MemberMap JsonMemberMap;
        /// <summary>
        /// JSON序列化配置参数
        /// </summary>
        private AutoCSer.Json.SerializeConfig jsonConfig;
        /// <summary>
        /// 警告提示状态
        /// </summary>
        internal SerializeWarning Warning;
        /// <summary>
        /// 历史对象指针位置
        /// </summary>
        private ReusableDictionary<ObjectReference, int> points;
        /// <summary>
        /// 是否支持循环引用处理
        /// </summary>
        private bool isReferenceMember;
        /// <summary>
        /// 是否检测数组引用
        /// </summary>
        private bool isReferenceArray;
        /// <summary>
        /// 数据流起始位置
        /// </summary>
        private int streamStartIndex;
        /// <summary>
        /// 释放资源
        /// </summary>
        void IDisposable.Dispose()
        {
            if (MemberMap != null) MemberMap.Dispose();
            if (CurrentMemberMap != null) CurrentMemberMap.Dispose();
            if (JsonMemberMap != null) JsonMemberMap.Dispose();
            //if (jsonStream != null) jsonStream.Dispose();
            Stream.Dispose();
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private SerializeResult serializeResult<valueType>(ref valueType value, SerializeConfig config)
        {
            return new SerializeResult { Data = serialize(ref value, config), Warning = Warning };
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        private byte[] serialize<valueType>(ref valueType value, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            AutoCSer.Memory.Pointer buffer = UnmanagedPool.Default.GetPointer();
            try
            {
                Stream.Reset(ref buffer);
                using (Stream)
                {
                    serialize(ref value);
                    return Stream.Data.GetArray();
                }
            }
            finally { UnmanagedPool.Default.PushOnly(ref buffer); }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        private void serialize<valueType>(ref valueType value)
        {
            Warning = SerializeWarning.None;
            if (isReferenceMember != TypeSerializer<valueType>.IsReferenceMember)
            {
                if (isReferenceMember) isReferenceMember = false;
                else
                {
                    isReferenceMember = true;
                    if (points == null) points = ReusableDictionary<ObjectReference>.Create<int>();
                }
            }
            isReferenceArray = true;
            MemberMap = Config.MemberMap;
            streamStartIndex = Stream.Data.CurrentIndex;
            Config.WriteHeader(Stream);
            TypeSerializer<valueType>.Serialize(this, ref value);
            Stream.Write(Stream.Data.CurrentIndex - streamStartIndex);
        }
        /// <summary>
        /// 自定义序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TypeSerialize<valueType>(valueType value)
        {
            if (value == null) Stream.Write(NullValue);
            else TypeSerializer<valueType>.Serialize(this, ref value);
        }
        /// <summary>
        /// 自定义序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TypeSerialize<valueType>(ref valueType value) where valueType : struct
        {
            TypeSerializer<valueType>.StructSerialize(this, ref value);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            MemberMap = CurrentMemberMap = null;
            if (points != null) points.ClearKey();
            YieldPool.Default.Push(this);
        }
        /// <summary>
        /// 添加历史对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool CheckPoint<valueType>(valueType value)
        {
            if (isReferenceMember)
            {
                int point;
                if (points.TryGetValue(new AutoCSer.ObjectReference { Value = value }, out point))
                {
                    Stream.Write(point);
                    return false;
                }
                points.Set(new AutoCSer.ObjectReference { Value = value }, streamStartIndex - Stream.Data.CurrentIndex);
            }
            return true;
        }
        /// <summary>
        /// 添加历史对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool checkPoint<valueType>(valueType[] value)
        {
            if (value.Length == 0)
            {
                Stream.Write(0);
                isReferenceArray = true;
                return false;
            }
            if (isReferenceArray) return CheckPoint<valueType[]>(value);
            return isReferenceArray = true;
        }
        /// <summary>
        /// 序列化成员位图
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public MemberMap SerializeMemberMap<valueType>()
        {
            if (MemberMap != null)
            {
                CurrentMemberMap = MemberMap;
                MemberMap = null;
                if (object.ReferenceEquals(CurrentMemberMap.Type, MemberMap<valueType>.MemberMapType))
                {
                    CurrentMemberMap.BinarySerialize(Stream);
                    return CurrentMemberMap;
                }
                Warning = SerializeWarning.MemberMap;
            }
            return null;
        }
        ///// <summary>
        ///// 获取JSON序列化输出缓冲区
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="size"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal CharStream ResetJsonStream(void* data, int size)
        //{
        //    if (jsonStream == null) return jsonStream = new CharStream((char*)data, size >> 1);
        //    jsonStream.Reset((byte*)data, size);
        //    return jsonStream;
        //}
        /// <summary>
        /// 获取JSON序列化配置参数
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Json.SerializeConfig GetJsonConfig(MemberMap memberMap)
        {
            if (jsonConfig == null) jsonConfig = AutoCSer.Json.SerializeConfig.CreateInternal();
            jsonConfig.MemberMap = memberMap;
            return jsonConfig;
        }
        /// <summary>
        /// 获取JSON成员位图
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="memberMap"></param>
        /// <param name="memberIndexs"></param>
        /// <returns></returns>
        internal MemberMap GetJsonMemberMap<valueType>(MemberMap memberMap, int[] memberIndexs)
        {
            int count = 0;
            foreach (int memberIndex in memberIndexs)
            {
                if (memberMap.IsMember(memberIndex))
                {
                    if (count == 0 && JsonMemberMap == null) JsonMemberMap = MemberMap<valueType>.NewEmpty();
                    JsonMemberMap.SetMember(memberIndex);
                    ++count;
                }
            }
            return count == 0 ? null : JsonMemberMap;
        }
        /// <summary>
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">设置的自定义序列化成员位图</param>
        /// <returns>序列化成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public SerializeMemberMap GetCustomMemberMap(MemberMap memberMap)
        {
            MemberMap oldMemberMap = MemberMap;
            MemberMap = memberMap;
            return new SerializeMemberMap { MemberMap = oldMemberMap, CurrentMemberMap = CurrentMemberMap, JsonMemberMap = JsonMemberMap };
        }
        /// <summary>
        /// 恢复自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">序列化成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetCustomMemberMap(ref SerializeMemberMap memberMap)
        {
            MemberMap = memberMap.MemberMap;
            CurrentMemberMap = memberMap.CurrentMemberMap;
            JsonMemberMap = memberMap.JsonMemberMap;
        }

        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, bool[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length);
            foreach (bool value in array) arrayMap.Next(value);
            arrayMap.End();
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, bool?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length << 1);
            foreach (bool? value in array) arrayMap.Next(value);
            arrayMap.End();
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="data">数据,不能为null</param>
        /// <param name="arrayLength">数据数量</param>
        /// <param name="size">单个数据字节数</param>
        internal static void Serialize(UnmanagedStream stream, void* data, int arrayLength, int size)
        {
            int dataSize = arrayLength * size;
            byte* write = stream.GetBeforeMove((dataSize + (3 + sizeof(int))) & (int.MaxValue - 3));
            *(int*)write = arrayLength;
            new Span<byte>(data, dataSize).CopyTo(new Span<byte>(write + sizeof(int), dataSize));
            stream.Data.SerializeFillLeftByteSize(dataSize);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, byte[] data)
        {
            fixed (byte* dataFixed = data) Serialize(stream, dataFixed, data.Length, 1);
        }
        /// <summary>
        /// 预增数据流长度并序列化数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, ref LeftArray<byte> data)
        {
            int dataSize = data.Length;
            byte* write = stream.GetBeforeMove((dataSize + (3 + sizeof(int))) & (int.MaxValue - 3));
            *(int*)write = dataSize;
            data.AsSpan().CopyTo(new Span<byte>(write + sizeof(int), dataSize));
            stream.Data.SerializeFillLeftByteSize(dataSize);
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, byte?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, (array.Length + 7) & (int.MaxValue - 3));
            byte* write = stream.Data.Current;
            foreach (byte? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (byte)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SerializeFillByteSize(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, sbyte[] data)
        {
            fixed (sbyte* dataFixed = data) Serialize(stream, dataFixed, data.Length, 1);
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, sbyte?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, (array.Length + 7) & (int.MaxValue - 3));
            sbyte* write = (sbyte*)stream.Data.Current;
            foreach (sbyte? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (sbyte)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SerializeFillByteSize(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, short[] data)
        {
            fixed (short* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(short));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, short?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, ((array.Length + 1) & (int.MaxValue - 1)) * sizeof(short));
            short* write = (short*)stream.Data.Current;
            foreach (short? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (short)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SerializeFillByteSize2(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, ushort[] data)
        {
            fixed (ushort* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(ushort));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, ushort?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, ((array.Length + 1) & (int.MaxValue - 1)) * sizeof(ushort));
            ushort* write = (ushort*)stream.Data.Current;
            foreach (ushort? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (ushort)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SerializeFillByteSize2(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, int[] data)
        {
            fixed (int* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(int));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, int?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(int));
            int* write = (int*)stream.Data.Current;
            foreach (int? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (int)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, uint[] data)
        {
            fixed (uint* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(uint));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, uint?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(uint));
            uint* write = (uint*)stream.Data.Current;
            foreach (uint? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (uint)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, long[] data)
        {
            fixed (long* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(long));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, long?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(long));
            long* write = (long*)stream.Data.Current;
            foreach (long? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (long)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, ulong[] data)
        {
            fixed (ulong* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(ulong));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, ulong?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(ulong));
            ulong* write = (ulong*)stream.Data.Current;
            foreach (ulong? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (ulong)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, float[] data)
        {
            fixed (float* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(float));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, float?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(float));
            float* write = (float*)stream.Data.Current;
            foreach (float? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (float)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, double[] data)
        {
            fixed (double* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(double));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, double?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(double));
            double* write = (double*)stream.Data.Current;
            foreach (double? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (double)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, decimal[] data)
        {
            fixed (decimal* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(decimal));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, decimal?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(decimal));
            decimal* write = (decimal*)stream.Data.Current;
            foreach (decimal? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (decimal)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, char?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(char));
            char* write = (char*)stream.Data.Current;
            foreach (char? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (char)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SerializeFillByteSize2(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, DateTime[] data)
        {
            fixed (DateTime* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(DateTime));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, DateTime?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(DateTime));
            DateTime* write = (DateTime*)stream.Data.Current;
            foreach (DateTime? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (DateTime)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="data">数据,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(UnmanagedStream stream, Guid[] data)
        {
            fixed (Guid* dataFixed = data) Serialize(stream, dataFixed, data.Length, sizeof(Guid));
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, Guid?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(Guid));
            Guid* write = (Guid*)stream.Data.Current;
            foreach (Guid? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (Guid)value;
                }
                else arrayMap.Next(false);
            }
            arrayMap.End();
            stream.Data.SetCurrent(write);
        }
        ///// <summary>
        ///// 字符串序列化
        ///// </summary>
        ///// <param name="valueFixed"></param>
        ///// <param name="stream"></param>
        ///// <param name="stringLength"></param>
        //private static void serialize(char* valueFixed, UnmanagedStream stream, int stringLength)
        //{
        //    byte* start = (byte*)valueFixed + 1, end = (byte*)(valueFixed + stringLength) + 1;
        //    do
        //    {
        //        if (*start != 0)
        //        {
        //            int prepLength = ((stringLength <<= 1) + (3 + sizeof(int))) & (int.MaxValue - 3);
        //            stream.PrepLength(prepLength);
        //            start = stream.CurrentData;
        //            AutoCSer.Memory.CopyNotNull(valueFixed, start + sizeof(int), *(int*)start = stringLength);
        //            stream.ByteSize += prepLength;
        //            if ((stringLength & 2) != 0) *(char*)(stream.CurrentData - sizeof(char)) = (char)0;
        //            stream.PrepLength();
        //            return;
        //        }
        //    }
        //    while ((start += 2) != end);

        //    int length = (stringLength + (3 + sizeof(int))) & (int.MaxValue - 3);
        //    stream.PrepLength(length + sizeof(int));
        //    byte* write = stream.CurrentData;
        //    *(int*)write = (stringLength << 1) + 1;
        //    write += sizeof(int);
        //    do
        //    {
        //        *write++ = (byte)*valueFixed++;
        //    }
        //    while (valueFixed != end);
        //    if ((stringLength & 3) != 0) *(int*)write = 0;
        //    stream.ByteSize += length;
        //    stream.PrepLength();
        //}
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="valueFixed"></param>
        /// <param name="stream"></param>
        /// <param name="stringLength"></param>
        internal static void Serialize(char* valueFixed, UnmanagedStream stream, int stringLength)
        {
            int prepLength = ((stringLength << 1) + (3 + sizeof(int))) & (int.MaxValue - 3);
            byte* writeFixed = stream.GetPrepSizeCurrent(prepLength);
            if (stringLength >= (*((byte*)valueFixed + 1) == 0 ? 3 : 7))
            {
                byte* writeStart = writeFixed + sizeof(int), writeEnd = writeStart + (prepLength - sizeof(int) * 2), readStart = (byte*)valueFixed, readEnd = (byte*)(valueFixed + stringLength), write, read;
                int lengthSize = (stringLength <= byte.MaxValue ? 1 : (stringLength <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                if (*((byte*)valueFixed + 1) != 0)
                {
                    switch (lengthSize)
                    {
                        case 1: *writeStart++ = 0; break;
                        case sizeof(ushort):
                            *(ushort*)writeStart = 0;
                            writeStart += sizeof(ushort);
                            break;
                        default:
                            *(int*)writeStart = 0;
                            writeStart += sizeof(int);
                            break;
                    }
                    goto CHAR;
                }
            BYTE:
                write = writeStart + lengthSize;
                *write = *readStart;
                read = readStart + sizeof(char);
                ++write;
                if (read != readEnd)
                {
                    while (*(read + 1) == 0)
                    {
                        *write = *read;
                        read += sizeof(char);
                        ++write;
                        if (read == readEnd) goto END;
                    }
                    switch (lengthSize)
                    {
                        case 1: *writeStart = (byte)((read - readStart) >> 1); break;
                        case sizeof(ushort):
                            *(ushort*)writeStart = (ushort)((read - readStart) >> 1);
                            if ((readEnd - read) <= ((int)byte.MaxValue << 1)) lengthSize = 1;
                            break;
                        default:
                            *(int*)writeStart = (int)((read - readStart) >> 1);
                            if ((readEnd - read) <= ((int)ushort.MaxValue << 1)) lengthSize = (readEnd - read) <= ((int)byte.MaxValue << 1) ? 1 : sizeof(ushort);
                            break;
                    }
                    readStart = read;
                    if ((writeEnd - write) > ((readEnd - read) >> 1) + lengthSize)
                    {
                        writeStart = write;
                        goto CHAR;
                    }
                    goto COPY;
                }
            END:
                *(int*)write = 0;
                switch (lengthSize)
                {
                    case 1: *writeStart = (byte)((read - readStart) >> 1); break;
                    case sizeof(ushort): *(ushort*)writeStart = (ushort)((read - readStart) >> 1); break;
                    default: *(int*)writeStart = (int)((read - readStart) >> 1); break;
                }
                lengthSize = (int)(write - writeFixed);
                *(int*)writeFixed = (stringLength << 1) + 1;
                stream.Data.CurrentIndex += lengthSize + (-lengthSize & 3);
                return;
            CHAR:
                write = writeStart + lengthSize;
                *(char*)write = *(char*)readStart;
                read = readStart + sizeof(char);
                write += sizeof(char);
                if (read == readEnd) goto END;
                while (*(read + 1) != 0)
                {
                    *(char*)write = *(char*)read;
                    read += sizeof(char);
                    write += sizeof(char);
                    if (read == readEnd)
                    {
                        if (write > writeEnd) goto COPY;
                        goto END;
                    }
                    if (write >= writeEnd) goto COPY;
                }
                switch (lengthSize)
                {
                    case 1: *writeStart = (byte)((read - readStart) >> 1); break;
                    case sizeof(ushort):
                        *(ushort*)writeStart = (ushort)((read - readStart) >> 1);
                        if ((readEnd - read) <= ((int)byte.MaxValue << 1)) lengthSize = 1;
                        break;
                    default:
                        *(int*)writeStart = (int)((read - readStart) >> 1);
                        if ((readEnd - read) <= ((int)ushort.MaxValue << 1)) lengthSize = (readEnd - read) <= ((int)byte.MaxValue << 1) ? 1 : sizeof(ushort);
                        break;
                }
                readStart = read;
                if ((writeEnd - write) >= ((readEnd - read) >> 1) + lengthSize)
                {
                    writeStart = write;
                    goto BYTE;
                }
            }
        COPY:
            *(int*)writeFixed = (stringLength <<= 1);
            new Span<byte>(valueFixed, stringLength).CopyTo(new Span<byte>(writeFixed + sizeof(int), stringLength));
            stream.Data.CurrentIndex += prepLength;
            if ((stringLength & 2) != 0) *(char*)(stream.Data.Current - sizeof(char)) = (char)0;
        }
        /// <summary>
        /// 预增数据流长度并序列化字符串(4字节对齐)
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="value">字符串,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void Serialize(UnmanagedStream stream, ref SubString value)
        {
            if (value.Length == 0) stream.Write(0);
            else
            {
                fixed (char* valueFixed = value.String) Serialize(valueFixed + value.Start, stream, value.Length);
            }
        }

        /// <summary>
        /// 真实类型序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void realTypeObject<valueType>(BinarySerializer serializer, object value)
        {
            TypeSerializer<valueType>.RealTypeObject(serializer, value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void baseSerialize<valueType, childType>(BinarySerializer serializer, childType value) where childType : valueType
        {
            TypeSerializer<valueType>.BaseSerialize(serializer, value);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        private void nullableArray<valueType>(Nullable<valueType>[] array) where valueType : struct
        {
            if (checkPoint(array))
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, array.Length);
                foreach (Nullable<valueType> value in array) arrayMap.Next(value.HasValue);
                arrayMap.End();

                foreach (Nullable<valueType> value in array)
                {
                    if (value.HasValue) TypeSerializer<valueType>.StructSerialize(this, value.Value);
                }
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableArray<valueType>(BinarySerializer binarySerializer, Nullable<valueType>[] array) where valueType : struct
        {
            binarySerializer.nullableArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        private void structArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                Stream.Write(array.Length);
                for (int index = 0; index != array.Length; TypeSerializer<valueType>.StructSerialize(this, ref array[index++]));
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructArray<valueType>(BinarySerializer binarySerializer, valueType[] array)
        {
            binarySerializer.structArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        private void array<valueType>(valueType[] array) where valueType : class
        {
            if (checkPoint(array))
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, array.Length);
                foreach (valueType value in array) arrayMap.Next(value != null);
                arrayMap.End();

                foreach (valueType value in array)
                {
                    if (value != null) TypeSerializer<valueType>.ClassSerialize(this, value);
                }
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Array<valueType>(BinarySerializer binarySerializer, valueType[] array) where valueType : class
        {
            binarySerializer.array(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void LeftArraySerialize<valueType>(BinarySerializer binarySerializer, LeftArray<valueType> value)
        {
            valueType[] array = value.ToArray();
            binarySerializer.isReferenceArray = false;
            TypeSerializer<valueType[]>.DefaultSerializer(binarySerializer, array);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        private void dictionarySerialize<dictionaryType, keyType, valueType>(dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
        {
            if (CheckPoint(value))
            {
                int index = 0;
                keyType[] keys = new keyType[value.Count];
                valueType[] values = new valueType[keys.Length];
                foreach (KeyValuePair<keyType, valueType> keyValue in value)
                {
                    keys[index] = keyValue.Key;
                    values[index++] = keyValue.Value;
                }
                isReferenceArray = false;
                TypeSerializer<keyType[]>.DefaultSerializer(this, keys);
                isReferenceArray = false;
                TypeSerializer<valueType[]>.DefaultSerializer(this, values);
            }
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DictionarySerialize<dictionaryType, keyType, valueType>(BinarySerializer binarySerializer, dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
        {
            binarySerializer.dictionarySerialize<dictionaryType, keyType, valueType>(value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableSerialize<valueType>(BinarySerializer serializer, Nullable<valueType> value) where valueType : struct
        {
            TypeSerializer<valueType>.StructSerialize(serializer, value.Value);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void keyValuePairSerialize<keyType, valueType>(BinarySerializer serializer, KeyValuePair<keyType, valueType> value)
        {
            KeyValue<keyType, valueType> keyValue = new KeyValue<keyType, valueType>(value.Key, value.Value);
            TypeSerializer<KeyValue<keyType, valueType>>.MemberSerialize(serializer, ref keyValue);
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="dictionary">对象集合</param>
        private void structDictionary<dictionaryType, keyType, valueType>(dictionaryType dictionary) where dictionaryType : IDictionary<keyType, valueType>
        {
            keyType[] keys = new keyType[dictionary.Count];
            valueType[] values = new valueType[keys.Length];
            int index = 0;
            foreach (KeyValuePair<keyType, valueType> keyValue in dictionary)
            {
                keys[index] = keyValue.Key;
                values[index++] = keyValue.Value;
            }
            isReferenceArray = false;
            TypeSerializer<keyType[]>.DefaultSerializer(this, keys);
            isReferenceArray = false;
            TypeSerializer<valueType[]>.DefaultSerializer(this, values);
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="dictionary">对象集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructDictionary<dictionaryType, keyType, valueType>(BinarySerializer serializer, dictionaryType dictionary) where dictionaryType : IDictionary<keyType, valueType>
        {
            serializer.structDictionary<dictionaryType, keyType, valueType>(dictionary);
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="dictionary">对象集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassDictionary<dictionaryType, keyType, valueType>(BinarySerializer serializer, dictionaryType dictionary) where dictionaryType : IDictionary<keyType, valueType>
        {
            if (serializer.CheckPoint(dictionary)) serializer.structDictionary<dictionaryType, keyType, valueType>(dictionary);
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">对象集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructCollection<valueType, collectionType>(BinarySerializer binarySerializer, collectionType collection) where collectionType : ICollection<valueType>
        {
            binarySerializer.isReferenceArray = false;
            TypeSerializer<valueType[]>.DefaultSerializer(binarySerializer, collection.getArray());
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">对象集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassCollection<valueType, collectionType>(BinarySerializer binarySerializer, collectionType collection) where collectionType : ICollection<valueType>
        {
            if (binarySerializer.CheckPoint(collection)) StructCollection<valueType, collectionType>(binarySerializer, collection);
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize<valueType>(BinarySerializer binarySerializer, valueType value) where valueType : struct
        {
            TypeSerializer<valueType>.StructSerialize(binarySerializer, ref value);
        }
        /// <summary>
        /// 引用类型成员序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void MemberClassSerialize<valueType>(BinarySerializer binarySerializer, valueType value) where valueType : class
        {
            if (value == null) binarySerializer.Stream.Write(NullValue);
            else TypeSerializer<valueType>.ClassSerialize(binarySerializer, value);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DictionaryMember<dictionaryType, keyType, valueType>(BinarySerializer binarySerializer, dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
        {
            if (value == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.dictionarySerialize<dictionaryType, keyType, valueType>(value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableMemberSerialize<valueType>(BinarySerializer binarySerializer, Nullable<valueType> value) where valueType : struct
        {
            if (value.HasValue)
            {
                binarySerializer.Stream.Write(NullableHasValue);
                TypeSerializer<valueType>.StructSerialize(binarySerializer, value.Value);
            }
            else binarySerializer.Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableArrayMember<valueType>(BinarySerializer binarySerializer, Nullable<valueType>[] array) where valueType : struct
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.nullableArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructArrayMember<valueType>(BinarySerializer binarySerializer, valueType[] array)
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.structArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ArrayMember<valueType>(BinarySerializer binarySerializer, valueType[] array) where valueType : class
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.array(array);
        }

        /// <summary>
        /// 重新计算序列化字节长度（4字节对齐）
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int GetSize4(int size)
        {
            return (size + 3) & (int.MaxValue - 3);
        }
        /// <summary>
        /// 重新计算序列化字节长度（4字节对齐）
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int GetSize(int size)
        {
            return size;
        }
        /// <summary>
        /// 序列化填充空白字节
        /// </summary>
        /// <param name="write"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FillSize(byte* write, int size) { }
        /// <summary>
        /// 序列化填充空白字节 short / ushort（4字节对齐）
        /// </summary>
        /// <param name="write"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FillSize2(byte* write, int size)
        {
            if ((size & 1) != 0) *(short*)write = 0;
        }
        /// <summary>
        /// 序列化填充空白字节 byte / sbyte（4字节对齐）
        /// </summary>
        /// <param name="write"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FillSize4(byte* write, int size)
        {
            switch(size & 3)
            {
                case 1:
                    *write = 0;
                    *(short*)(write + 1) = 0;
                    return;
                case 2: *(short*)write = 0; return;
                case 3: *write = 0; return;
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        public static byte[] Serialize<valueType>(valueType value, SerializeConfig config = null)
        {
            if (value == null) return BitConverter.GetBytes(NullValue);
            BinarySerializer serializer = YieldPool.Default.Pop() ?? new BinarySerializer();
            try
            {
                return serializer.serialize<valueType>(ref value, config);
            }
            finally { serializer.Free(); }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        public static SerializeResult SerializeResult<valueType>(valueType value, SerializeConfig config = null)
        {
            if (value == null) return new SerializeResult { Data = BitConverter.GetBytes(NullValue), Warning = SerializeWarning.None };
            BinarySerializer serializer = YieldPool.Default.Pop() ?? new BinarySerializer();
            try
            {
                return serializer.serializeResult<valueType>(ref value, config);
            }
            finally { serializer.Free(); }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        public static byte[] Serialize<valueType>(ref valueType value, SerializeConfig config = null)
        {
            if (value == null) return BitConverter.GetBytes(NullValue);
            BinarySerializer serializer = YieldPool.Default.Pop() ?? new BinarySerializer();
            try
            {
                return serializer.serialize<valueType>(ref value, config);
            }
            finally { serializer.Free(); }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        public static SerializeResult SerializeResult<valueType>(ref valueType value, SerializeConfig config = null)
        {
            if (value == null) return new SerializeResult { Data = BitConverter.GetBytes(NullValue), Warning = SerializeWarning.None };
            BinarySerializer serializer = YieldPool.Default.Pop() ?? new BinarySerializer();
            try
            {
                return serializer.serializeResult<valueType>(ref value, config);
            }
            finally { serializer.Free(); }
        }

        static BinarySerializer()
        {
            serializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            memberSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            memberMapSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(BinarySerializer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                Type parameterType = null;
                if (method.IsDefined(typeof(SerializeMethod), false))
                {
                    serializeMethods.Add(parameterType = method.GetParameters()[0].ParameterType, method);
                }
                if (method.IsDefined(typeof(SerializeMemberMethod), false))
                {
                    if (parameterType == null) parameterType = method.GetParameters()[0].ParameterType;
                    memberSerializeMethods.Add(parameterType, method);
                }
                if (method.IsDefined(typeof(SerializeMemberMapMethod), false))
                {
                    memberMapSerializeMethods.Add(parameterType ?? method.GetParameters()[0].ParameterType, method);
                }
            }
        }
    }
}
