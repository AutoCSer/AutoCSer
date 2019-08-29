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
    public unsafe sealed partial class DeSerializer
    {
        /// <summary>
        /// 缓冲区起始位置
        /// </summary>
        private int bufferIndex;
        /// <summary>
        /// 未处理数据字节数
        /// </summary>
        public int NextSize
        {
            get { return (int)(Read - End); }
        }
        ///// <summary>
        ///// 当前读取数据位置
        ///// </summary>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public byte* GetRead(out byte* end)
        //{
        //    end = this.end;
        //    return Read;
        //}
        /// <summary>
        /// 获取数据缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool GetBuffer(ref SubArray<byte> buffer)
        {
            if (Buffer == null) return false;
            fixed (byte* bufferFixed = Buffer) buffer.Set(Buffer, (int)(Read - bufferFixed), (int)(End - Read));
            return true;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref SubArray<byte> value)
        {
            int length = *(int*)Read;
            if (length == 0)
            {
                value.Length = 0;
                Read += sizeof(int);
            }
            else
            {
                if (((length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    byte[] array = new byte[length];
                    Read = DeSerialize(Read + sizeof(int), array);
                    value.Set(array, 0, length);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组对象序列化
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void subArrayDeSerialize<valueType>(ref SubArray<valueType> value)
        {
            valueType[] array = null;
            isReferenceArray = false;
            TypeDeSerializer<valueType[]>.DefaultDeSerializer(this, ref array);
            value.Set(array, 0, array.Length);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType DeSerialize<valueType>(SubArray<byte> data, DeSerializeConfig config = null)
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
        public static valueType DeSerialize<valueType>(ref SubArray<byte> data, DeSerializeConfig config = null)
        {
            if (data.Length != 0)
            {
                fixed (byte* dataFixed = data.Array)
                {
                    valueType value = default(valueType);
                    if (DeSerialize<valueType>(data.Array, dataFixed + data.Start, data.Length, ref value, config)) return value;
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
        public static DeSerializeResult DeSerialize<valueType>(SubArray<byte> data, ref valueType value, DeSerializeConfig config = null)
        {
            return DeSerialize(ref data, ref value, config);
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
        public static DeSerializeResult DeSerialize<valueType>(ref SubArray<byte> data, ref valueType value, DeSerializeConfig config = null)
        {
            if (data.Length == 0) return new DeSerializeResult { State = DeSerializeState.UnknownData };
            fixed (byte* dataFixed = data.Array) return DeSerialize<valueType>(data.Array, dataFixed + data.Start, data.Length, ref value, config);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        /// <param name="context"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetTcpServer(DeSerializeConfig config, object context)
        {
            Config = config;
            Context = context;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <returns>是否成功</returns>
        internal bool DeSerializeTcpServer<valueType>(ref SubArray<byte> data, ref valueType value)
        {
            if (data.Length >= sizeof(int) * 3 && (data.Length & 3) == 0)
            {
                fixed (byte* dataFixed = (Buffer = data.Array))
                {
                    start = dataFixed + (bufferIndex = data.Start);
                    int length = data.Length - sizeof(int);
                    End = start + length;
                    if (((uint)(*(int*)End ^ length) | ((*(uint*)start & SerializeConfig.HeaderMapAndValue) ^ SerializeConfig.HeaderMapValue)) == 0)
                    {
                        //MemberMap = null;
                        getGlobalVersion();
                        if (isReferenceMember == TypeDeSerializer<valueType>.IsReferenceMember)
                        {
                            if (points != null) points.Clear();
                        }
                        else
                        {
                            if (isReferenceMember) isReferenceMember = false;
                            else
                            {
                                isReferenceMember = true;
                                if (points == null) points = ReusableDictionary.CreateInt<object>();
                                else points.Clear();
                            }
                        }
                        isReferenceArray = true;
                        State = DeSerializeState.Success;
                        TypeDeSerializer<valueType>.DeSerializeTcpServer(this, ref value);
                        return State == DeSerializeState.Success && Read == End;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 缓冲区反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="read"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DeSerializeTcpServer(ref SubArray<byte> data, byte* read, int size)
        {
            data.Set(Buffer, (int)(read - start) + bufferIndex, size);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="read"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte[] GetData(byte* read, int size)
        {
            byte[] data = new byte[size];
            System.Buffer.BlockCopy(Buffer, (int)(read - start) + bufferIndex, data, 0, size);
            return data;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="read"></param>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(byte* read, ref SubArray<byte> data)
        {
            System.Buffer.BlockCopy(Buffer, (int)(read - start) + bufferIndex, data.Array, data.Start, data.Length);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="read"></param>
        /// <param name="data"></param>
        /// <param name="dataStart"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(byte* read, ref SubArray<byte> data, int dataStart)
        {
            System.Buffer.BlockCopy(Buffer, (int)(read - start) + bufferIndex, data.Array, data.Start + dataStart, data.Length - dataStart);
        }
        /// <summary>
        /// 设置数据字节数组
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetBuffer(ref SubArray<byte> data, byte* start)
        {
            Buffer = data.Array;
            bufferIndex = data.Start;
            this.start = Read = start;
            End = start + data.Length;
        }
        /// <summary>
        /// 反序列化字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool DeSerializeString(ref string value)
        {
            value = DeSerializeString(ref Read, End);
            if (value != null) return true;
            State = DeSerializeState.IndexOutOfRange;
            return false;
        }
        /// <summary>
        /// 反序列化字符串
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal static string DeSerializeString(ref byte* start, byte* end)
        {
            int length = *(int*)start;
            if ((length & 1) == 0)
            {
                if (length != 0)
                {
                    int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                    if (dataLength <= (int)(end - start))
                    {
                        string value = new string((char*)(start + sizeof(int)), 0, length >> 1);
                        start += dataLength;
                        return value;
                    }
                }
                else
                {
                    start += sizeof(int);
                    return string.Empty;
                }
            }
            else
            {
                length >>= 1;
                int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(end - start))
                {
                    string value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
                    fixed (char* valueFixed = value)
                    {
                        byte* read = DeSerialize(start, end, valueFixed, length, lengthSize);
                        if (read != null)
                        {
                            start = read;
                            return value;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 预编译类型
        /// </summary>
        /// <param name="types"></param>
        internal static void Compile(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type != null) GenericType.Get(type).BinaryDeSerializeCompile();
            }
        }
    }
}
