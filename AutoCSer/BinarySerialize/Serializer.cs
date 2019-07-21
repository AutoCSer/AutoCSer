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
    public unsafe sealed partial class Serializer : AutoCSer.Threading.Link<Serializer>, IDisposable
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
        internal static readonly SerializeAttribute DefaultAttribute = ConfigLoader.GetUnion(typeof(SerializeAttribute)).SerializeAttribute ?? new SerializeAttribute { IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly SerializeConfig DefaultConfig = ConfigLoader.GetUnion(typeof(SerializeConfig)).SerializeConfig ?? new SerializeConfig();
        /// <summary>
        /// 序列化输出缓冲区
        /// </summary>
        public readonly UnmanagedStream Stream = new UnmanagedStream(null, 0);
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
        /// <summary>
        /// JSON序列化输出缓冲区
        /// </summary>
        private CharStream jsonStream;
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
            if (jsonStream != null) jsonStream.Dispose();
            Stream.Dispose();
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        private SerializeResult serialize<valueType>(valueType value, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            byte* buffer = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                Stream.Reset(buffer, AutoCSer.UnmanagedPool.DefaultSize);
                using (Stream)
                {
                    serialize(value);
                    return new SerializeResult { Data = Stream.GetArray(), Warning = Warning };
                }
            }
            finally { AutoCSer.UnmanagedPool.Default.Push(buffer); }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        private void serialize<valueType>(valueType value)
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
            //streamStartIndex = Stream.OffsetLength;
            streamStartIndex = Stream.ByteSize;
            Config.WriteHeader(Stream);
            TypeSerializer<valueType>.Serialize(this, value);
            //Stream.Write(Stream.OffsetLength - streamStartIndex);
            Stream.Write(Stream.ByteSize - streamStartIndex);
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
            else TypeSerializer<valueType>.Serialize(this, value);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            MemberMap = CurrentMemberMap = null;
            if (points != null) points.ClearKey();
            YieldPool.Default.PushNotNull(this);
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
                    Stream.Write(-point);
                    return false;
                }
                //points[new AutoCSer.ObjectReference { Value = value }] = Stream.OffsetLength - streamStartIndex;
                points.Set(new AutoCSer.ObjectReference { Value = value }, Stream.ByteSize - streamStartIndex);
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
                if (CurrentMemberMap.Type == MemberMap<valueType>.TypeInfo)
                {
                    CurrentMemberMap.BinarySerialize(Stream);
                    return CurrentMemberMap;
                }
                Warning = SerializeWarning.MemberMap;
            }
            return null;
        }
        /// <summary>
        /// 获取JSON序列化输出缓冲区
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal CharStream ResetJsonStream(void* data, int size)
        {
            if (jsonStream == null) return jsonStream = new CharStream((char*)data, size >> 1);
            jsonStream.Reset((byte*)data, size);
            return jsonStream;
        }
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
            arrayMap.End(stream);
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
            arrayMap.End(stream);
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
            byte* write = stream.GetPrepSizeCurrent((dataSize + (3 + sizeof(int))) & (int.MaxValue - 3));
            *(int*)write = arrayLength;
            AutoCSer.Memory.CopyNotNull(data, write + sizeof(int), dataSize);
            stream.SerializeFillByteSize(dataSize + sizeof(int));
            //stream.PrepLength();
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
            int dataLength = data.Length;
            byte* write = stream.GetPrepSizeCurrent((dataLength + (3 + sizeof(int))) & (int.MaxValue - 3));
            *(int*)write = dataLength;
            fixed (byte* dataFixed = data.Array) AutoCSer.Memory.CopyNotNull(dataFixed, write + sizeof(int), dataLength);
            stream.SerializeFillByteSize(dataLength + sizeof(int));
            //stream.PrepLength();
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, byte?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, (array.Length + 7) & (int.MaxValue - 3));
            byte* write = stream.CurrentData;
            foreach (byte? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (byte)value;
                }
                else arrayMap.Next(false);
            }
            int length = (int)(write - stream.CurrentData);
            if ((length & 3) == 0) stream.ByteSize += length;
            else
            {
                *(int*)write = 0;
                stream.ByteSize += (length + 3) & (int.MaxValue - 3);
            }
            arrayMap.End(stream);
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
            sbyte* write = (sbyte*)stream.CurrentData;
            foreach (sbyte? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (sbyte)value;
                }
                else arrayMap.Next(false);
            }
            int length = (int)((byte*)write - stream.CurrentData);
            if ((length & 3) == 0) stream.ByteSize += length;
            else
            {
                *(int*)write = 0;
                stream.ByteSize += (length + 3) & (int.MaxValue - 3);
            }
            arrayMap.End(stream);
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
            short* write = (short*)stream.CurrentData;
            foreach (short? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (short)value;
                }
                else arrayMap.Next(false);
            }
            int length = (int)((byte*)write - stream.CurrentData);
            if ((length & 2) == 0) stream.ByteSize += length;
            else
            {
                *write = 0;
                stream.ByteSize += length + sizeof(short);
            }
            arrayMap.End(stream);
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
            ushort* write = (ushort*)stream.CurrentData;
            foreach (ushort? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (ushort)value;
                }
                else arrayMap.Next(false);
            }
            int length = (int)((byte*)write - stream.CurrentData);
            if ((length & 2) == 0) stream.ByteSize += length;
            else
            {
                *write = 0;
                stream.ByteSize += length + sizeof(ushort);
            }
            arrayMap.End(stream);
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
            int* write = (int*)stream.CurrentData;
            foreach (int? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (int)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            uint* write = (uint*)stream.CurrentData;
            foreach (uint? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (uint)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            long* write = (long*)stream.CurrentData;
            foreach (long? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (long)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            ulong* write = (ulong*)stream.CurrentData;
            foreach (ulong? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (ulong)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            float* write = (float*)stream.CurrentData;
            foreach (float? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (float)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            double* write = (double*)stream.CurrentData;
            foreach (double? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (double)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            decimal* write = (decimal*)stream.CurrentData;
            foreach (decimal? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (decimal)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
        }
        /// <summary>
        /// 序列化可空数组
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数组数据</param>
        internal static void Serialize(UnmanagedStream stream, char?[] array)
        {
            SerializeArrayMap arrayMap = new SerializeArrayMap(stream, array.Length, array.Length * sizeof(char));
            char* write = (char*)stream.CurrentData;
            foreach (char? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (char)value;
                }
                else arrayMap.Next(false);
            }
            int length = (int)((byte*)write - stream.CurrentData);
            if ((length & 2) == 0) stream.ByteSize += length;
            else
            {
                *write = (char)0;
                stream.ByteSize += length + sizeof(char);
            }
            arrayMap.End(stream);
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
            DateTime* write = (DateTime*)stream.CurrentData;
            foreach (DateTime? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (DateTime)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            Guid* write = (Guid*)stream.CurrentData;
            foreach (Guid? value in array)
            {
                if (value.HasValue)
                {
                    arrayMap.Next(true);
                    *write++ = (Guid)value;
                }
                else arrayMap.Next(false);
            }
            stream.ByteSize += (int)((byte*)write - stream.CurrentData);
            arrayMap.End(stream);
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
            stream.PrepLength(prepLength);
            byte* writeFixed = stream.CurrentData;
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
                stream.ByteSize += lengthSize + (-lengthSize & 3);
                //stream.PrepLength();
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
            AutoCSer.Memory.CopyNotNull(valueFixed, writeFixed + sizeof(int), *(int*)writeFixed = (stringLength << 1));
            stream.ByteSize += prepLength;
            if ((stringLength & 1) != 0) *(char*)(stream.CurrentData - sizeof(char)) = (char)0;
            //stream.PrepLength();
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
        internal static void realTypeObject<valueType>(Serializer serializer, object value)
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
        private static void baseSerialize<valueType, childType>(Serializer serializer, childType value) where childType : valueType
        {
            TypeSerializer<valueType>.BaseSerialize(serializer, value);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumByteArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int arrayLength = array.Length, length = (arrayLength + 7) & (int.MaxValue - 3);
                Stream.PrepLength(length + sizeof(int));
                byte* write = Stream.CurrentData;
                *(int*)write = arrayLength;
                write += sizeof(int);
                foreach (valueType value in array) *write++ = Emit.EnumCast<valueType, byte>.ToInt(value);
                if ((arrayLength & 3) != 0) *(int*)write = 0;
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumSByteArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int arrayLength = array.Length, length = (arrayLength + 7) & (int.MaxValue - 3);
                Stream.PrepLength(length + sizeof(int));
                byte* write = Stream.CurrentData;
                *(int*)write = arrayLength;
                write += sizeof(int);
                foreach (valueType value in array) *(sbyte*)write++ = Emit.EnumCast<valueType, sbyte>.ToInt(value);
                if ((arrayLength & 3) != 0) *(int*)write = 0;
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumShortArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int arrayLength = array.Length, length = ((arrayLength * sizeof(short)) + 7) & (int.MaxValue - 3);
                Stream.PrepLength(length);
                byte* write = Stream.CurrentData;
                *(int*)write = arrayLength;
                write += sizeof(int);
                foreach (valueType value in array)
                {
                    *(short*)write = Emit.EnumCast<valueType, short>.ToInt(value);
                    write += sizeof(short);
                }
                if ((arrayLength & 1) != 0) *(short*)write = 0;
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumUShortArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int arrayLength = array.Length, length = ((arrayLength * sizeof(ushort)) + 7) & (int.MaxValue - 3);
                Stream.PrepLength(length);
                byte* write = Stream.CurrentData;
                *(int*)write = arrayLength;
                write += sizeof(int);
                foreach (valueType value in array)
                {
                    *(ushort*)write = Emit.EnumCast<valueType, ushort>.ToInt(value);
                    write += sizeof(ushort);
                }
                if ((arrayLength & 1) != 0) *(ushort*)write = 0;
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumIntArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int length = (array.Length + 1) * sizeof(int);
                Stream.PrepLength(length);
                byte* write = Stream.CurrentData;
                *(int*)write = array.Length;
                foreach (valueType value in array) *(int*)(write += sizeof(int)) = Emit.EnumCast<valueType, int>.ToInt(value);
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumUIntArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int length = (array.Length + 1) * sizeof(uint);
                Stream.PrepLength(length);
                byte* write = Stream.CurrentData;
                *(int*)write = array.Length;
                foreach (valueType value in array) *(uint*)(write += sizeof(uint)) = Emit.EnumCast<valueType, uint>.ToInt(value);
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumLongArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int length = array.Length * sizeof(long) + sizeof(int);
                Stream.PrepLength(length);
                byte* write = Stream.CurrentData;
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (valueType value in array)
                {
                    *(long*)write = Emit.EnumCast<valueType, long>.ToInt(value);
                    write += sizeof(long);
                }
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumULongArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                int length = array.Length * sizeof(ulong) + sizeof(int);
                Stream.PrepLength(length);
                byte* write = Stream.CurrentData;
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (valueType value in array)
                {
                    *(ulong*)write = Emit.EnumCast<valueType, ulong>.ToInt(value);
                    write += sizeof(ulong);
                }
                Stream.ByteSize += length;
                //Stream.PrepLength();
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableArray<valueType>(Nullable<valueType>[] array) where valueType : struct
        {
            if (checkPoint(array))
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, array.Length);
                foreach (Nullable<valueType> value in array) arrayMap.Next(value.HasValue);
                arrayMap.End(Stream);

                foreach (Nullable<valueType> value in array)
                {
                    if (value.HasValue) TypeSerializer<valueType>.StructSerialize(this, value.Value);
                }
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structArray<valueType>(valueType[] array)
        {
            if (checkPoint(array))
            {
                Stream.Write(array.Length);
                foreach (valueType value in array) TypeSerializer<valueType>.StructSerialize(this, value);
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void array<valueType>(valueType[] array) where valueType : class
        {
            if (checkPoint(array))
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, array.Length);
                foreach (valueType value in array) arrayMap.Next(value != null);
                arrayMap.End(Stream);

                foreach (valueType value in array)
                {
                    if (value != null) TypeSerializer<valueType>.ClassSerialize(this, value);
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void leftArraySerialize<valueType>(LeftArray<valueType> value)
        {
            valueType[] array = value.ToArray();
            isReferenceArray = false;
            TypeSerializer<valueType[]>.DefaultSerializer(this, array);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionarySerialize<dictionaryType, keyType, valueType>(dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
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
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void nullableSerialize<valueType>(Serializer serializer, Nullable<valueType> value) where valueType : struct
        {
            TypeSerializer<valueType>.StructSerialize(serializer, value.Value);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void keyValuePairSerialize<keyType, valueType>(KeyValuePair<keyType, valueType> value)
        {
            TypeSerializer<KeyValue<keyType, valueType>>.MemberSerialize(this, new KeyValue<keyType, valueType>(value.Key, value.Value));
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="dictionary">对象集合</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structDictionary<dictionaryType, keyType, valueType>(dictionaryType dictionary) where dictionaryType : IDictionary<keyType, valueType>
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
        /// <param name="dictionary">对象集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void classDictionary<dictionaryType, keyType, valueType>(dictionaryType dictionary) where dictionaryType : IDictionary<keyType, valueType>
        {
            if (CheckPoint(dictionary)) structDictionary<dictionaryType, keyType, valueType>(dictionary);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumByteCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength(count + 8);
            byte* write = Stream.CurrentData + sizeof(int);
            foreach (valueType value in collection)
            {
                *(byte*)write++ = Emit.EnumCast<valueType, byte>.ToInt(value);
                if (--writeCount == 0) break;
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            if ((count & 3) != 0) *(int*)write = 0;
            Stream.ByteSize += (count + 7) & (int.MaxValue - 3);
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumByteCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumByteCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumSByteCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength(count + 8);
            byte* write = Stream.CurrentData + sizeof(int);
            foreach (valueType value in collection)
            {
                *(sbyte*)write++ = Emit.EnumCast<valueType, sbyte>.ToInt(value);
                if (--writeCount == 0) break;
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            if ((count & 3) != 0) *(int*)write = 0;
            Stream.ByteSize += (count + 7) & (int.MaxValue - 3);
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumSByteCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumSByteCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumShortCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength(((count * sizeof(short)) + 7) & (int.MaxValue - 3));
            byte* write = Stream.CurrentData + sizeof(int);
            foreach (valueType value in collection)
            {
                *(short*)write = Emit.EnumCast<valueType, short>.ToInt(value);
                write += sizeof(short);
                if (--writeCount == 0) break;
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            if ((count & 1) == 0) Stream.ByteSize += count * sizeof(short) + sizeof(int);
            else
            {
                *(short*)write = 0;
                Stream.ByteSize += (count + 1) * sizeof(short) + sizeof(int);
            }
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumShortCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumShortCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumUShortCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength(((count * sizeof(ushort)) + 7) & (int.MaxValue - 3));
            byte* write = Stream.CurrentData + sizeof(int);
            foreach (valueType value in collection)
            {
                *(ushort*)write = Emit.EnumCast<valueType, ushort>.ToInt(value);
                write += sizeof(ushort);
                if (--writeCount == 0) break;
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            if ((count & 1) == 0) Stream.ByteSize += count * sizeof(ushort) + sizeof(int);
            else
            {
                *(ushort*)write = 0;
                Stream.ByteSize += (count + 1) * sizeof(ushort) + sizeof(int);
            }
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumUShortCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumUShortCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumIntCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength((count + 1) * sizeof(int));
            byte* write = Stream.CurrentData + sizeof(int);
            foreach (valueType value in collection)
            {
                *(int*)write = Emit.EnumCast<valueType, int>.ToInt(value);
                if (--writeCount == 0) break;
                write += sizeof(int);
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            Stream.ByteSize += (count + 1) * sizeof(int);
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumIntCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumIntCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumUIntCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength((count + 1) * sizeof(uint));
            byte* write = Stream.CurrentData + sizeof(uint);
            foreach (valueType value in collection)
            {
                *(uint*)write = Emit.EnumCast<valueType, uint>.ToInt(value);
                if (--writeCount == 0) break;
                write += sizeof(uint);
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            Stream.ByteSize += (count + 1) * sizeof(uint);
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumUIntCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumUIntCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumLongCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength(count * sizeof(long) + sizeof(int));
            byte* write = Stream.CurrentData + sizeof(int);
            foreach (valueType value in collection)
            {
                *(long*)write = Emit.EnumCast<valueType, long>.ToInt(value);
                if (--writeCount == 0) break;
                write += sizeof(long);
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            Stream.ByteSize += count * sizeof(long) + sizeof(int);
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumLongCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumLongCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void structEnumULongCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            int count = collection.Count, writeCount = count;
            Stream.PrepLength(count * sizeof(ulong) + sizeof(int));
            byte* write = Stream.CurrentData + sizeof(int);
            foreach (valueType value in collection)
            {
                *(ulong*)write = Emit.EnumCast<valueType, ulong>.ToInt(value);
                if (--writeCount == 0) break;
                write += sizeof(ulong);
            }
            *(int*)Stream.CurrentData = (count -= writeCount);
            Stream.ByteSize += count * sizeof(ulong) + sizeof(int);
            //Stream.PrepLength();
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <param name="collection">枚举集合序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void classEnumULongCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structEnumULongCollection<valueType, collectionType>(collection);
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="collection">对象集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            isReferenceArray = false;
            TypeSerializer<valueType[]>.DefaultSerializer(this, collection.getArray());
        }
        /// <summary>
        /// 集合转换
        /// </summary>
        /// <param name="collection">对象集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void classCollection<valueType, collectionType>(collectionType collection) where collectionType : ICollection<valueType>
        {
            if (CheckPoint(collection)) structCollection<valueType, collectionType>(collection);
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structSerialize<valueType>(valueType value) where valueType : struct
        {
            TypeSerializer<valueType>.StructSerialize(this, value);
        }
        /// <summary>
        /// 引用类型成员序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void MemberClassSerialize<valueType>(valueType value) where valueType : class
        {
            if (value == null) Stream.Write(NullValue);
            else TypeSerializer<valueType>.ClassSerialize(this, value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByteMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, byte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByteMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, sbyte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShortMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, short>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShortMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, ushort>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumIntMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, int>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUIntMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, uint>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumLongMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, long>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumULongMember<valueType>(valueType value)
        {
            Stream.UnsafeWrite(Emit.EnumCast<valueType, ulong>.ToInt(value));
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionaryMember<dictionaryType, keyType, valueType>(dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
        {
            if (value == null) Stream.Write(NullValue);
            else dictionarySerialize<dictionaryType, keyType, valueType>(value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableMemberSerialize<valueType>(Nullable<valueType> value) where valueType : struct
        {
            if (value.HasValue)
            {
                Stream.Write(NullableHasValue);
                TypeSerializer<valueType>.StructSerialize(this, value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumByteArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumSByteArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumSByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumShortArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumUShortArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumUShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumIntArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumUIntArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumUIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumLongArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumLongArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void enumULongArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else enumULongArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableArrayMember<valueType>(Nullable<valueType>[] array) where valueType : struct
        {
            if (array == null) Stream.Write(NullValue);
            else nullableArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structArrayMember<valueType>(valueType[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else structArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void arrayMember<valueType>(valueType[] array) where valueType : class
        {
            if (array == null) Stream.Write(NullValue);
            else this.array(array);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化数据</returns>
        public static SerializeResult Serialize<valueType>(valueType value, SerializeConfig config = null)
        {
            if (value == null) return new SerializeResult { Data = BitConverter.GetBytes(NullValue), Warning = SerializeWarning.None };
            Serializer serializer = YieldPool.Default.Pop() ?? new Serializer();
            try
            {
                return serializer.serialize<valueType>(value, config);
            }
            finally { serializer.Free(); }
        }

        static Serializer()
        {
            serializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            memberSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            memberMapSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(Serializer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
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
