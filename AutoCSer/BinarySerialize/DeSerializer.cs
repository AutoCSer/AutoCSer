using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public unsafe sealed partial class DeSerializer : AutoCSer.Threading.Link<DeSerializer>
    {
        /// <summary>
        /// 反序列化配置参数
        /// </summary>
        internal DeSerializeConfig Config;
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap MemberMap;
        /// <summary>
        /// 数据字节数组
        /// </summary>
        internal byte[] Buffer;
        /// <summary>
        /// 数据字节长度
        /// </summary>
        internal int DataLength;
        /// <summary>
        /// 上下文
        /// </summary>
        internal object Context;
        /// <summary>
        /// 全局版本编号
        /// </summary>
        internal uint GlobalVersion;
        /// <summary>
        /// 序列化数据起始位置
        /// </summary>
        private byte* start;
        /// <summary>
        /// 序列化数据结束位置
        /// </summary>
        internal byte* End;
        /// <summary>
        /// 当前读取数据位置
        /// </summary>
        internal byte* Read;
        /// <summary>
        /// 自定义序列化获取当前读取数据位置
        /// </summary>
        public byte* CustomRead
        {
            get { return Read; }
        }
        /// <summary>
        /// 反序列化状态
        /// </summary>
        internal DeSerializeState State;
        /// <summary>
        /// 历史对象指针位置
        /// </summary>
        private ReusableDictionary<int, object> points;
        /// <summary>
        /// 是否检测相同的引用成员
        /// </summary>
        private bool isReferenceMember;
        /// <summary>
        /// 是否检测引用类型对象的真实类型
        /// </summary>
        internal bool IsObjectRealType;
        /// <summary>
        /// 是否检测数组引用
        /// </summary>
        private bool isReferenceArray;
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="value"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private DeSerializeResult deSerialize<valueType>(byte[] data, byte* start, byte* end, ref valueType value, DeSerializeConfig config)
        {
            Config = config;
            Buffer = data;
            this.start = start;
            this.End = end;
            getGlobalVersion();
            if (isReferenceMember != TypeDeSerializer<valueType>.IsReferenceMember)
            {
                if (isReferenceMember) isReferenceMember = false;
                else
                {
                    isReferenceMember = true;
                    if (points == null) points = ReusableDictionary.CreateInt<object>();
                }
            }
            isReferenceArray = true;
            State = DeSerializeState.Success;
            TypeDeSerializer<valueType>.DeSerialize(this, ref value);
            checkState();
            return new DeSerializeResult { State = State, DataLength = DataLength, MemberMap = MemberMap };
        }
        /// <summary>
        /// 获取全局版本编号
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getGlobalVersion()
        {
            int headerValue = *(int*)start;
            IsObjectRealType = (headerValue & SerializeConfig.ObjectRealTypeValue) != 0;
            if ((headerValue & SerializeConfig.GlobalVersionValue) == 0)
            {
                GlobalVersion = 0;
                Read = start + sizeof(int);
            }
            else
            {
                GlobalVersion = *(uint*)(start + sizeof(int));
                Read = start + (sizeof(int) + sizeof(uint));
            }
        }
        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TypeDeSerialize<valueType>(ref valueType value)
        {
            if (CheckNullValue() == 0) value = default(valueType);
            else TypeDeSerializer<valueType>.DeSerialize(this, ref value);
            return State == DeSerializeState.Success;
        }
        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType TypeDeSerialize<valueType>()
        {
            if (CheckNullValue() != 0)
            {
                valueType value = default(valueType);
                TypeDeSerializer<valueType>.DeSerialize(this, ref value);
                if (State == DeSerializeState.Success) return value;
            }
            return default(valueType);
        }
        /// <summary>
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">设置的自定义序列化成员位图</param>
        /// <returns>序列化成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public MemberMap SetCustomMemberMap(MemberMap memberMap)
        {
            MemberMap oldMemberMap = MemberMap;
            MemberMap = memberMap;
            return oldMemberMap;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            Context = null;
            if (Config.IsDisposeMemberMap)
            {
                if (MemberMap != null)
                {
                    MemberMap.Dispose();
                    MemberMap = null;
                }
            }
            else MemberMap = null;
            if (points != null) points.ClearValue();
            YieldPool.Default.PushNotNull(this);
        }
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void checkState()
        {
            if (State == DeSerializeState.Success)
            {
                if (Config.IsFullData)
                {
                    if (Read != End) State = DeSerializeState.FullDataError;
                }
                else if (Read <= End)
                {
                    int length = *(int*)Read;
                    if (length == Read - start) DataLength = length + sizeof(int);
                    State = DeSerializeState.EndVerify;
                }
                else State = DeSerializeState.EndVerify;
            }
        }
        /// <summary>
        /// 获取历史对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckPoint<valueType>(ref valueType value)
        {
            if (isReferenceMember && *(int*)Read < 0)
            {
                object pointValue;
                if (points.TryGetValue(*(int*)Read, out pointValue))
                {
                    value = (valueType)pointValue;
                    Read += sizeof(int);
                    return false;
                }
                if (*(int*)Read != Serializer.RealTypeValue)
                {
                    State = DeSerializeState.NoPoint;
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 是否真实类型处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsRealType()
        {
            if (IsObjectRealType && *(int*)Read == Serializer.RealTypeValue)
            {
                Read += sizeof(int);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加历史对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddPoint<valueType>(ref valueType value)
        {
            if (value == null) value = AutoCSer.Emit.Constructor<valueType>.New();
            if (isReferenceMember) points.Set((int)(start - Read), value);
        }
        /// <summary>
        /// 检测成员数量
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckMemberCount(int count)
        {
            if (*(int*)Read == count)
            {
                Read += sizeof(int);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检测成员位图
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <returns></returns>
        internal MemberMap GetMemberMap<valueType>()
        {
            if ((*(uint*)Read & 0xc0000000U) == 0)
            {
                if (MemberMap == null)
                {
                    MemberMap = new MemberMap<valueType>();
                    if (*Read == 0)
                    {
                        Read += sizeof(int);
                        return MemberMap;
                    }
                }
                else
                {
                    if (MemberMap.Type != MemberMap<valueType>.TypeInfo)
                    {
                        State = DeSerializeState.MemberMapType;
                        return null;
                    }
                    if (*Read == 0)
                    {
                        MemberMap.Dispose();
                        Read += sizeof(int);
                        return MemberMap;
                    }
                }
                byte* map = MemberMap.GetFieldDeSerialize();
                if (map != null)
                {
                    int fieldCount = MemberMap.Type.FieldCount, size = MemberMap.Type.BinarySerializeSize;
                    if (*(int*)Read == fieldCount)
                    {
                        if (size <= (int)(End - (Read += sizeof(int))))
                        {
                            for (byte* mapEnd = map + (size & (int.MaxValue - sizeof(ulong) + 1)); map != mapEnd; map += sizeof(ulong), Read += sizeof(ulong)) *(ulong*)map = *(ulong*)Read;
                            if ((size & sizeof(int)) != 0)
                            {
                                *(uint*)map = *(uint*)Read;
                                Read += sizeof(uint);
                            }
                        }
                        else State = DeSerializeState.IndexOutOfRange;
                    }
                    else State = DeSerializeState.MemberMapVerify;
                }
                return State == DeSerializeState.Success ? MemberMap : null;
            }
            State = DeSerializeState.MemberMap;
            return null;
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <param name="value"></param>
        internal void ParseJson<valueType>(ref valueType value)
        {
            int size = *(int*)Read;
            if (size == 0)
            {
                Read += sizeof(int);
                return;
            }
            if (size > 0 && (size & 1) == 0)
            {
                byte* start = Read;
                if ((Read += (size + (2 + sizeof(int))) & (int.MaxValue - 3)) <= End)
                {
                    if (!AutoCSer.Json.Parser.UnsafeParse((char*)(start + sizeof(int)), size >> 1, ref value)) State = DeSerializeState.JsonError;
                    return;
                }
            }
            State = DeSerializeState.IndexOutOfRange;
        }
        /// <summary>
        /// 不支持对象null解析检测
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CheckNull()
        {
            if (*(int*)Read == Serializer.NullValue) Read += sizeof(int);
            else State = DeSerializeState.NotNull;
        }
        /// <summary>
        /// 对象null值检测
        /// </summary>
        /// <returns>返回 0 表示 null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int CheckNullValue()
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                return 0;
            }
            return 1;
        }
        /// <summary>
        /// 自定义序列化重置当前读取数据位置，允许向前移动
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool MoveReadAny(int size)
        {
            if ((Read += size) <= End) return true;
            State = DeSerializeState.Custom;
            return false;
        }
        /// <summary>
        /// 移动当前读取数据位置，负数表示自定义序列化失败
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool MoveRead(int size)
        {
            if (size >= 0)
            {
                if ((Read += size) <= End) return true;
            }
            if (State == DeSerializeState.Success) State = DeSerializeState.Custom;
            return false;
        }
        /// <summary>
        /// 读取一个整数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int ReadInt()
        {
            int value = *(int*)Read;
            Read += sizeof(int);
            return value;
        }
        /// <summary>
        /// 读取一个整数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ulong ReadULong()
        {
            ulong value = *(ulong*)Read;
            Read += sizeof(ulong);
            return value;
        }

        /// <summary>
        /// 创建数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool createArray<valueType>(ref valueType[] array, int length)
        {
            if (length <= Config.MaxArraySize)
            {
                array = new valueType[length];
                if (isReferenceArray)
                {
                    if (isReferenceMember) points.Set((int)(start - Read), array);
                }
                else isReferenceArray = true;
                return true;
            }
            State = DeSerializeState.ArraySizeOutOfRange;
            return false;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns>数组长度</returns>
        private int deSerializeArray<valueType>(ref valueType[] value)
        {
            if (isReferenceArray && !CheckPoint(ref value)) return 0;
            if (*(int*)Read != 0) return *(int*)Read;
            isReferenceArray = true;
            value = NullValue<valueType>.Array;
            Read += sizeof(int);
            return 0;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, bool[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            for (int index = 0; index != value.Length; ++index) value[index] = arrayMap.Next() != 0;
            return arrayMap.Read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, bool?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data, 2);
            for (int index = 0; index != value.Length; ++index) value[index] = arrayMap.NextBool();
            return arrayMap.Read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, byte[] value)
        {
            AutoCSer.Memory.CopyNotNull(data, value, value.Length);
            return data + ((value.Length + 3) & (int.MaxValue - 3));
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, byte?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            byte* start = (data += ((value.Length + 31) >> 5) << 2);
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *data++;
            }
            return data + ((int)(start - data) & 3);
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, sbyte[] value)
        {
            fixed (sbyte* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, value.Length);
            return data + ((value.Length + 3) & (int.MaxValue - 3));
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(sbyte* data, sbyte?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap((byte*)data);
            sbyte* start = (data += ((value.Length + 31) >> 5) << 2);
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *data++;
            }
            return (byte*)(data + ((int)(start - data) & 3));
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, short[] value)
        {
            int length = value.Length * sizeof(short);
            fixed (short* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + ((length + 3) & (int.MaxValue - 3));
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, short?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            short* read = (short*)(data + (((value.Length + 31) >> 5) << 2)), start = read;
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return ((int)((byte*)read - (byte*)start) & 2) == 0 ? (byte*)read : (byte*)(read + 1);
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, ushort[] value)
        {
            int length = value.Length * sizeof(ushort);
            fixed (ushort* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + ((length + 3) & (int.MaxValue - 3));
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, ushort?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            ushort* read = (ushort*)(data + (((value.Length + 31) >> 5) << 2)), start = read;
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return ((int)((byte*)read - (byte*)start) & 2) == 0 ? (byte*)read : (byte*)(read + 1);
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, int[] value)
        {
            int length = value.Length * sizeof(int);
            fixed (int* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, int?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            int* read = (int*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, uint[] value)
        {
            int length = value.Length * sizeof(uint);
            fixed (uint* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, uint?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            uint* read = (uint*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, long[] value)
        {
            int length = value.Length * sizeof(long);
            fixed (long* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, long?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            long* read = (long*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, ulong[] value)
        {
            int length = value.Length * sizeof(ulong);
            fixed (ulong* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, ulong?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            ulong* read = (ulong*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, float[] value)
        {
            int length = value.Length * sizeof(float);
            fixed (float* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, float?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            float* read = (float*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, double[] value)
        {
            int length = value.Length * sizeof(double);
            fixed (double* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, double?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            double* read = (double*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, decimal[] value)
        {
            int length = value.Length * sizeof(decimal);
            fixed (decimal* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, decimal?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            decimal* read = (decimal*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        ///// <summary>
        ///// 反序列化数据
        ///// </summary>
        ///// <param name="data">起始位置</param>
        ///// <param name="value">目标数据</param>
        ///// <returns>结束位置</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal static byte* DeSerialize(byte* data, char[] value)
        //{
        //    int length = value.Length * sizeof(char);
        //    fixed (char* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
        //    return data + ((length + 3) & (int.MaxValue - 3));
        //}
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, char?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            char* read = (char*)(data + (((value.Length + 31) >> 5) << 2)), start = read;
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return ((int)((byte*)read - (byte*)start) & 2) == 0 ? (byte*)read : (byte*)(read + 1);
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, DateTime[] value)
        {
            int length = value.Length * sizeof(DateTime);
            fixed (DateTime* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, DateTime?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            DateTime* read = (DateTime*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* DeSerialize(byte* data, Guid[] value)
        {
            int length = value.Length * sizeof(Guid);
            fixed (Guid* valueFixed = value) AutoCSer.Memory.CopyNotNull(data, valueFixed, length);
            return data + length;
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data">起始位置</param>
        /// <param name="value">目标数据</param>
        /// <returns>结束位置</returns>
        internal static byte* DeSerialize(byte* data, Guid?[] value)
        {
            DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(data);
            Guid* read = (Guid*)(data + (((value.Length + 31) >> 5) << 2));
            for (int index = 0; index != value.Length; ++index)
            {
                if (arrayMap.Next() == 0) value[index] = null;
                else value[index] = *read++;
            }
            return (byte*)read;
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="write">写入位置</param>
        /// <param name="length">写入长度</param>
        /// <param name="lengthSize">写入长度字节大小</param>
        /// <returns>是否成功</returns>
        internal static byte* DeSerialize(byte* start, byte* end, char* write, int length, int lengthSize)
        {
            byte* read = start + sizeof(int);
            int charCount;
            do
            {
                switch (lengthSize)
                {
                    case 1: length -= (charCount = *read++); break;
                    case sizeof(ushort):
                        length -= (charCount = *(ushort*)read);
                        read += sizeof(ushort);
                        if (length <= byte.MaxValue) lengthSize = 1;
                        break;
                    default:
                        length -= (charCount = *(int*)read);
                        read += sizeof(int);
                        if (length <= ushort.MaxValue) lengthSize = length <= byte.MaxValue ? 1 : sizeof(ushort);
                        break;
                }
                byte* readEnd = read + charCount;
                if (length < 0 || readEnd > end) return null;
                while (read != readEnd) *write++ = (char)*read++;
                if (length == 0) return read + ((int)(start - read) & 3);
                switch (lengthSize)
                {
                    case 1: length -= (charCount = *read++); break;
                    case sizeof(ushort):
                        length -= (charCount = *(ushort*)read);
                        read += sizeof(ushort);
                        if (length <= byte.MaxValue) lengthSize = 1;
                        break;
                    default:
                        length -= (charCount = *(int*)read);
                        read += sizeof(int);
                        if (length <= ushort.MaxValue) lengthSize = length <= byte.MaxValue ? 1 : sizeof(ushort);
                        break;
                }
                char* readCharEnd = (char*)read + charCount;
                if (length < 0 || readCharEnd > end) return null;
                while (read != readCharEnd)
                {
                    *write++ = *(char*)read;
                    read += sizeof(char);
                }
                if (length == 0) return read + ((int)(start - read) & 3);
            }
            while (true);
        }
        ///// <summary>
        ///// 反序列化数据
        ///// </summary>
        ///// <param name="data">起始位置</param>
        ///// <param name="end">结束位置</param>
        ///// <param name="value">目标数据</param>
        ///// <returns>结束位置,失败返回null</returns>
        //internal static byte* DeSerialize(byte* data, byte* end, ref string value)
        //{
        //    int length = *(int*)data;
        //    if ((length & 1) == 0)
        //    {
        //        if (length != 0)
        //        {
        //            int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
        //            if (dataLength <= end - data)
        //            {
        //                value = new string((char*)(data + sizeof(int)), 0, length >> 1);
        //                return data + dataLength;
        //            }
        //        }
        //        else
        //        {
        //            value = string.Empty;
        //            return data + sizeof(int);
        //        }
        //    }
        //    else
        //    {
        //        int dataLength = ((length >>= 1) + (3 + sizeof(int))) & (int.MaxValue - 3);
        //        if (dataLength <= end - data)
        //        {
        //            fixed (char* valueFixed = (value = AutoCSer.Extension.StringExtension.FastAllocateString(length)))
        //            {
        //                byte* start = data + sizeof(int);
        //                char* write = valueFixed;
        //                end = start + length;
        //                do
        //                {
        //                    *write++ = (char)*start++;
        //                }
        //                while (start != end);
        //            }
        //            return data + dataLength;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 基类反序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void baseSerialize<valueType, childType>(DeSerializer deSerializer, ref childType value) where childType : valueType
        {
            TypeDeSerializer<valueType>.BaseDeSerialize(deSerializer, ref value);
        }
        /// <summary>
        /// 真实类型反序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="objectValue"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static object realTypeObject<valueType>(DeSerializer deSerializer, object objectValue)
        {
            valueType value = (valueType)objectValue;
            TypeDeSerializer<valueType>.RealType(deSerializer, ref value);
            return value;
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void EnumByte<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, byte>.FromInt(*Read);
            Read += sizeof(int);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void EnumSByte<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, sbyte>.FromInt((sbyte)*(int*)Read);
            Read += sizeof(int);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void EnumShort<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, short>.FromInt((short)*(int*)Read);
            Read += sizeof(int);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void EnumUShort<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, ushort>.FromInt(*(ushort*)Read);
            Read += sizeof(int);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void EnumInt<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, int>.FromInt(*(int*)Read);
            Read += sizeof(int);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void EnumUInt<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, uint>.FromInt(*(uint*)Read);
            Read += sizeof(uint);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void EnumLong<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, long>.FromInt(*(long*)Read);
            Read += sizeof(long);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void EnumULong<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, ulong>.FromInt(*(ulong*)Read);
            Read += sizeof(ulong);
        }
        /// <summary>
        /// 数组对象序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void leftArrayDeSerialize<valueType>(ref LeftArray<valueType> value)
        {
            valueType[] array = null;
            isReferenceArray = false;
            TypeDeSerializer<valueType[]>.DefaultDeSerializer(this, ref array);
            value.Set(array, array.Length);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionaryDeSerialize<keyType, valueType>(ref Dictionary<keyType, valueType> value)
        {
            dictionaryArrayDeSerialize(value = DictionaryCreator.CreateAny<keyType, valueType>());
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        private void dictionaryArrayDeSerialize<keyType, valueType>(IDictionary<keyType, valueType> value)
        {
            if (isReferenceMember) points.Set((int)(start - Read), value);
            keyType[] keys = null;
            isReferenceArray = false;
            TypeDeSerializer<keyType[]>.DefaultDeSerializer(this, ref keys);
            if (State == DeSerializeState.Success)
            {
                valueType[] values = null;
                isReferenceArray = false;
                TypeDeSerializer<valueType[]>.DefaultDeSerializer(this, ref values);
                if (State == DeSerializeState.Success)
                {
                    int index = 0;
                    foreach (valueType nextValue in values) value.Add(keys[index++], nextValue);
                }
            }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableDeSerialize<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            if (*(int*)Read == Serializer.NullableHasValue)
            {
                Read += sizeof(int);
                valueType newValue = value.HasValue ? value.Value : default(valueType);
                TypeDeSerializer<valueType>.StructDeSerialize(this, ref newValue);
                value = newValue;
            }
            else State = DeSerializeState.UnknownData;
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void keyValuePairDeSerialize<keyType, valueType>(ref KeyValuePair<keyType, valueType> value)
        {
            KeyValue<keyType, valueType> keyValue = default(KeyValue<keyType, valueType>);
            TypeDeSerializer<KeyValue<keyType, valueType>>.GetDeSerializer(GlobalVersion).MemberDeSerialize(this, ref keyValue);
            value = new KeyValuePair<keyType, valueType>(keyValue.Key, keyValue.Value);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void sortedDictionaryDeSerialize<keyType, valueType>(ref SortedDictionary<keyType, valueType> value)
        {
            dictionaryArrayDeSerialize(value = new SortedDictionary<keyType, valueType>());
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void sortedListDeSerialize<keyType, valueType>(ref SortedList<keyType, valueType> value)
        {
            dictionaryArrayDeSerialize(value = new SortedList<keyType, valueType>());
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        private void collection<valueType, argumentType>(ref valueType value) where valueType : ICollection<argumentType>
        {
            argumentType[] values = null;
            isReferenceArray = false;
            TypeDeSerializer<argumentType[]>.DefaultDeSerializer(this, ref values);
            if (State == DeSerializeState.Success)
            {
                foreach (argumentType nextValue in values) value.Add(nextValue);
            }
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structCollection<valueType, argumentType>(ref valueType value) where valueType : ICollection<argumentType>
        {
            value = AutoCSer.Emit.Constructor<valueType>.New();
            collection<valueType, argumentType>(ref value);
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void classCollection<valueType, argumentType>(ref valueType value) where valueType : ICollection<argumentType>
        {
            if (CheckPoint(ref value))
            {
                value = AutoCSer.Emit.Constructor<valueType>.New();
                if (isReferenceMember) points.Set((int)(start - Read), value);
                collection<valueType, argumentType>(ref value);
            }
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        private void dictionaryConstructorDeSerialize<dictionaryType, keyType, valueType>(ref dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
        {
            keyType[] keys = null;
            isReferenceArray = false;
            TypeDeSerializer<keyType[]>.DefaultDeSerializer(this, ref keys);
            if (State == DeSerializeState.Success)
            {
                valueType[] values = null;
                isReferenceArray = false;
                TypeDeSerializer<valueType[]>.DefaultDeSerializer(this, ref values);
                if (State == DeSerializeState.Success)
                {
                    int index = 0;
                    foreach (valueType nextValue in values) value.Add(keys[index++], nextValue);
                }
            }
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structDictionaryDeSerialize<dictionaryType, keyType, valueType>(ref dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
        {
            value = AutoCSer.Emit.Constructor<dictionaryType>.New();
            dictionaryConstructorDeSerialize<dictionaryType, keyType, valueType>(ref value);
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void classDictionaryDeSerialize<dictionaryType, keyType, valueType>(ref dictionaryType value) where dictionaryType : IDictionary<keyType, valueType>
        {
            if (CheckPoint(ref value))
            {
                value = AutoCSer.Emit.Constructor<dictionaryType>.New();
                if (isReferenceMember) points.Set((int)(start - Read), value);
                dictionaryConstructorDeSerialize<dictionaryType, keyType, valueType>(ref value);
            }
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structDeSerialize<valueType>(ref valueType value) where valueType : struct
        {
            TypeDeSerializer<valueType>.StructDeSerialize(this, ref value);
        }
        /// <summary>
        /// 引用类型成员反序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void memberClassDeSerialize<valueType>(ref valueType value) where valueType : class
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else TypeDeSerializer<valueType>.ClassDeSerialize(this, ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByteMember<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, byte>.FromInt(*Read++);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByteMember<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, sbyte>.FromInt(*(sbyte*)Read++);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShortMember<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, short>.FromInt(*(short*)Read);
            Read += sizeof(short);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShortMember<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, ushort>.FromInt(*(ushort*)Read);
            Read += sizeof(ushort);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionaryMember<keyType, valueType>(ref Dictionary<keyType, valueType> value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else dictionaryDeSerialize(ref value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableMemberDeSerialize<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else nullableDeSerialize(ref value);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void sortedDictionaryMember<keyType, valueType>(ref SortedDictionary<keyType, valueType> value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else sortedDictionaryDeSerialize(ref value);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void sortedListMember<keyType, valueType>(ref SortedList<keyType, valueType> value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else sortedListDeSerialize(ref value);
        }

        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByteArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; array[index++] = Emit.EnumCast<valueType, byte>.FromInt(*data++)) ;
                        Read += dataLength;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByteArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; array[index++] = Emit.EnumCast<valueType, sbyte>.FromInt((sbyte)*data++)) ;
                        Read += dataLength;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShortArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = ((length << 1) + (3 + sizeof(int))) & (int.MaxValue - 3);
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        short* data = (short*)(Read + sizeof(int));
                        for (int index = 0; index != array.Length; array[index++] = Emit.EnumCast<valueType, short>.FromInt(*data++)) ;
                        Read += dataLength;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShortArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = ((length << 1) + (3 + sizeof(int))) & (int.MaxValue - 3);
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        ushort* data = (ushort*)(Read + sizeof(int));
                        for (int index = 0; index != array.Length; array[index++] = Emit.EnumCast<valueType, ushort>.FromInt(*data++)) ;
                        Read += dataLength;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumIntArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                if ((length + 1) * sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        Read += sizeof(int);
                        for (int index = 0; index != array.Length; Read += sizeof(int)) array[index++] = Emit.EnumCast<valueType, int>.FromInt(*(int*)Read);
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUIntArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                if ((length + 1) * sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        Read += sizeof(int);
                        for (int index = 0; index != array.Length; Read += sizeof(uint)) array[index++] = Emit.EnumCast<valueType, uint>.FromInt(*(uint*)Read);
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumLongArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                if (length * sizeof(long) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        Read += sizeof(int);
                        for (int index = 0; index != array.Length; Read += sizeof(long)) array[index++] = Emit.EnumCast<valueType, long>.FromInt(*(long*)Read);
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumULongArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                if (length * sizeof(ulong) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        Read += sizeof(int);
                        for (int index = 0; index != array.Length; Read += sizeof(ulong)) array[index++] = Emit.EnumCast<valueType, ulong>.FromInt(*(ulong*)Read);
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableArray<valueType>(ref Nullable<valueType>[] array) where valueType : struct
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int mapLength = ((length + (31 + 32)) >> 5) << 2;
                if (mapLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(Read + sizeof(int));
                        Read += mapLength;
                        for (int index = 0; index != array.Length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                valueType value = default(valueType);
                                TypeDeSerializer<valueType>.StructDeSerialize(this, ref value);
                                if (State != DeSerializeState.Success) return;
                                array[index] = value;
                            }
                        }
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structArray<valueType>(ref valueType[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                if ((length + 1) * sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        Read += sizeof(int);
                        for (int index = 0; index != array.Length; ++index)
                        {
                            TypeDeSerializer<valueType>.StructDeSerialize(this, ref array[index]);
                            if (State != DeSerializeState.Success) return;
                        }
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void array<valueType>(ref valueType[] array) where valueType : class
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int mapLength = ((length + (31 + 32)) >> 5) << 2;
                if (mapLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(Read + sizeof(int));
                        Read += mapLength;
                        for (int index = 0; index != array.Length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                TypeDeSerializer<valueType>.ClassDeSerialize(this, ref array[index]);
                                if (State != DeSerializeState.Success) return;
                            }
                        }
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }

        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableArrayMember<valueType>(ref Nullable<valueType>[] array) where valueType : struct
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else nullableArray(ref array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else structArray(ref array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void arrayMember<valueType>(ref valueType[] array) where valueType : class
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else this.array(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByteArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumByteArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByteArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumSByteArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShortArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumShortArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShortArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumUShortArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumIntArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumIntArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUIntArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumUIntArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumLongArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumLongArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <param name="array">枚举数组序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumULongArrayMember<valueType>(ref valueType[] array)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumULongArray(ref array);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult DeSerialize<valueType>(byte[] data, ref valueType value, DeSerializeConfig config = null)
        {
            if (data == null) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            fixed (byte* dataFixed = data) return DeSerialize<valueType>(data, dataFixed, data.Length, ref value, config);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType DeSerialize<valueType>(byte[] data, DeSerializeConfig config = null)
        {
            if (data != null)
            {
                fixed (byte* dataFixed = data)
                {
                    valueType value = default(valueType);
                    if (DeSerialize<valueType>(data, dataFixed, data.Length, ref value, config)) return value;
                }
            }
            return default(valueType);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult DeSerialize<valueType>(LeftArray<byte> data, ref valueType value, DeSerializeConfig config = null)
        {
            if (data.Length == 0) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            fixed (byte* dataFixed = data.Array) return DeSerialize<valueType>(data.Array, dataFixed, data.Length, ref value, config);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult DeSerialize<valueType>(ref LeftArray<byte> data, ref valueType value, DeSerializeConfig config = null)
        {
            if (data.Length == 0) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            fixed (byte* dataFixed = data.Array) return DeSerialize<valueType>(data.Array, dataFixed, data.Length, ref value, config);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType DeSerialize<valueType>(LeftArray<byte> data, DeSerializeConfig config = null)
        {
            return DeSerialize<valueType>(ref data, config);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType DeSerialize<valueType>(ref LeftArray<byte> data, DeSerializeConfig config = null)
        {
            if (data.Length != 0)
            {
                fixed (byte* dataFixed = data.Array)
                {
                    valueType value = default(valueType);
                    if (DeSerialize<valueType>(data.Array, dataFixed, data.Length, ref value, config)) return value;
                }
            }
            return default(valueType);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="startIndex">数据起始位置</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult DeSerialize<valueType>(UnmanagedStream data, ref valueType value, int startIndex = 0, DeSerializeConfig config = null)
        {
            if (data == null) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            return DeSerialize<valueType>(null, data.Data.Byte + startIndex, data.ByteSize - startIndex, ref value, config);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="startIndex">数据起始位置</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType DeSerialize<valueType>(UnmanagedStream data, int startIndex = 0, DeSerializeConfig config = null)
        {
            if (data != null)
            {
                valueType value = default(valueType);
                if (DeSerialize<valueType>(null, data.Data.Byte + startIndex, data.ByteSize - startIndex, ref value, config)) return value;
            }
            return default(valueType);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="size">数据字节长度</param>
        /// <param name="value">目标对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult DeSerialize<valueType>(byte* data, int size, ref valueType value, DeSerializeConfig config = null)
        {
            if (data == null) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            return DeSerialize<valueType>(null, data, size, ref value, config);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="size">数据字节长度</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType DeSerialize<valueType>(byte* data, int size, DeSerializeConfig config = null)
        {
            if (data != null)
            {
                valueType value = default(valueType);
                if (DeSerialize<valueType>(null, data, size, ref value, config)) return value;
            }
            return default(valueType);
        }
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly DeSerializeConfig DefaultConfig = ConfigLoader.GetUnion(typeof(DeSerializeConfig)).DeSerializeConfig ?? new DeSerializeConfig { IsDisposeMemberMap = true };
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <param name="config"></param>
        /// <returns>是否成功</returns>
        internal static DeSerializeResult DeSerialize<valueType>(byte[] buffer, byte* data, int size, ref valueType value, DeSerializeConfig config)
        {
            int length = size - sizeof(int);
            if (length >= 0)
            {
                if (config == null) config = DefaultConfig;
                if (config.IsFullData)
                {
                    if ((size & 3) == 0)
                    {
                        if (length != 0)
                        {
                            byte* end = data + length;
                            if (*(int*)end == length)
                            {
                                if ((*(uint*)data & SerializeConfig.HeaderMapAndValue) == SerializeConfig.HeaderMapValue)
                                {
                                    DeSerializer deSerializer = YieldPool.Default.Pop() ?? new DeSerializer();
                                    try
                                    {
                                        return deSerializer.deSerialize(buffer, data, end, ref value, config);
                                    }
                                    finally { deSerializer.Free(); }
                                }
                                return new DeSerializeResult { State = DeSerializeState.HeaderError };
                            }
                            return new DeSerializeResult { State = DeSerializeState.EndVerify };
                        }
                        if (*(int*)data == Serializer.NullValue)
                        {
                            value = default(valueType);
                            return new DeSerializeResult { State = DeSerializeState.Success, DataLength = sizeof(int) };
                        }
                    }
                }
                else
                {
                    if ((*(uint*)data & SerializeConfig.HeaderMapAndValue) == SerializeConfig.HeaderMapValue)
                    {
                        DeSerializer deSerializer = YieldPool.Default.Pop() ?? new DeSerializer();
                        try
                        {
                            return deSerializer.deSerialize(buffer, data, data + length, ref value, config);
                        }
                        finally { deSerializer.Free(); }
                    }
                    if (*(int*)data == Serializer.NullValue)
                    {
                        value = default(valueType);
                        return new DeSerializeResult { State = DeSerializeState.Success, DataLength = sizeof(int) };
                    }
                    return new DeSerializeResult { State = DeSerializeState.HeaderError };
                }
            }
            return new DeSerializeResult { State = DeSerializeState.UnknownData };
        }

        static DeSerializer()
        {
            deSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            memberDeSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            memberMapDeSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(DeSerializer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                Type parameterType = null;
                if (method.IsDefined(typeof(DeSerializeMethod), false))
                {
                    deSerializeMethods.Add(parameterType = method.GetParameters()[0].ParameterType.GetElementType(), method);
                }
                if (method.IsDefined(typeof(DeSerializeMemberMethod), false))
                {
                    if (parameterType == null) parameterType = method.GetParameters()[0].ParameterType.GetElementType();
                    memberDeSerializeMethods.Add(parameterType, method);
                }
                if (method.IsDefined(typeof(DeSerializeMemberMapMethod), false))
                {
                    memberMapDeSerializeMethods.Add(parameterType ?? method.GetParameters()[0].ParameterType.GetElementType(), method);
                }
            }
        }
    }
}
