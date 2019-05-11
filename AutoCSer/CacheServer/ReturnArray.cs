using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 返回数组
    /// </summary>
    internal static unsafe class ReturnArray
    {
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="value">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, char[] value)
        {
            fixed (char* valueFixed = value) AutoCSer.BinarySerialize.Serializer.Serialize(valueFixed, stream, value.Length);
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, string[] array)
        {
            AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(stream, array.Length, 0);
            foreach (string value in array) arrayMap.Next(value != null);
            arrayMap.End(stream);
            foreach (string value in array)
            {
                if (value != null)
                {
                    if (value.Length == 0) stream.Write(0);
                    else
                    {
                        fixed (char* valueFixed = value) AutoCSer.BinarySerialize.Serializer.Serialize(valueFixed, stream, value.Length);
                    }
                }
            }
        }
        /// <summary>
        /// 字符数组序列化
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, byte[][] array)
        {
            AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(stream, array.Length, 0);
            foreach (byte[] value in array) arrayMap.Next(value != null);
            arrayMap.End(stream);
            foreach (byte[] value in array)
            {
                if (value != null) AutoCSer.BinarySerialize.Serializer.Serialize(stream, value);
            }
        }
        /// <summary>
        /// 二进制数组序列化
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, Cache.Value.Binary[] array)
        {
            AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(stream, array.Length, 0);
            foreach (Cache.Value.Binary value in array) arrayMap.Next(value.Data != null);
            arrayMap.End(stream);
            foreach (Cache.Value.Binary value in array)
            {
                if (value.Data != null) AutoCSer.BinarySerialize.Serializer.Serialize(stream, value.Data);
            }
        }
        /// <summary>
        /// JSON 数组序列化
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="array">数据,不能为null</param>
        internal static void Serialize(UnmanagedStream stream, Cache.Value.Json[] array)
        {
            AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(stream, array.Length, 0);
            foreach (Cache.Value.Json value in array) arrayMap.Next(value.Data != null);
            arrayMap.End(stream);
            foreach (Cache.Value.Json value in array)
            {
                if (value.Data != null) AutoCSer.BinarySerialize.Serializer.Serialize(stream, value.Data);
            }
        }
    }
    /// <summary>
    /// 返回数组
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal struct ReturnArray<valueType>
    {
        /// <summary>
        /// 数组
        /// </summary>
        private valueType[] array;
        /// <summary>
        /// 数组
        /// </summary>
        /// <param name="array"></param>
        internal ReturnArray(valueType[] array)
        {
            this.array = array;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            if (array != null) Serialize(stream, array);
            else stream.Write(AutoCSer.BinarySerialize.Serializer.NullValue);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 获取数组返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static ReturnValue<valueType[]> Get(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            if (value.Value.Parameter.ReturnType == ReturnType.Success)
            {
                if (value.Value.Parameter.Type == ValueData.DataType.BinarySerialize)
                {
                    if (DeSerialize != null) return DeSerialize(value.Value.Parameter.SubByteArray);
                    valueType[] returnValue = null;
                    if (AutoCSer.BinarySerialize.DeSerializer.DeSerialize(value.Value.Parameter.SubByteArray, ref returnValue).State == AutoCSer.BinarySerialize.DeSerializeState.Success) return returnValue;
                }
                return new ReturnValue<valueType[]> { Type = ReturnType.DeSerializeError };
            }
            return new ReturnValue<valueType[]> { Type = value.Type == Net.TcpServer.ReturnType.Success ? value.Value.Parameter.ReturnType : ReturnType.TcpError, TcpReturnType = value.Type };
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static ReturnValue<DataStructure.Value.Json<valueType>[]> deSerializeJson(SubArray<byte> data)
        {
            byte[] dataArray = data.Array;
            fixed (byte* dataFixed = dataArray)
            {
                byte* start = dataFixed + data.Start, end = start + (data.Count - sizeof(int));
                uint globalVersion = 0;
                if (AutoCSer.BinarySerialize.SerializeConfig.CheckHeaderValue(start, ref globalVersion) && *(int*)end == data.Count - sizeof(int))
                {
                    int length = *(int*)(start += (globalVersion == 0 ? sizeof(int) : (sizeof(int) + sizeof(uint))));
                    if (length != 0)
                    {
                        int mapLength = ((length + (31 + 32)) >> 5) << 2;
                        if (mapLength <= (int)(end - start))
                        {
                            DataStructure.Value.Json<valueType>[] array = new DataStructure.Value.Json<valueType>[length];
                            AutoCSer.BinarySerialize.DeSerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeSerializeArrayMap(start + sizeof(int));
                            ValueData.Data parameter = default(ValueData.Data);
                            start += mapLength;
                            for (int index = 0; index != array.Length; ++index)
                            {
                                if (arrayMap.Next() != 0)
                                {
                                    if ((length = *(int*)start) != 0)
                                    {
                                        if ((mapLength = ((length + (3 + sizeof(int))) & (int.MaxValue - 3))) <= (int)(end - start))
                                        {
                                            parameter.Set(dataArray, (int)(start - dataFixed) + sizeof(int), length, ValueData.DataType.Json);
                                            start += mapLength;
                                            array[index] = new DataStructure.Value.Json<valueType>(ref parameter);
                                        }
                                        else return new ReturnValue<DataStructure.Value.Json<valueType>[]> { Type = ReturnType.DeSerializeError };
                                    }
                                    else return new ReturnValue<DataStructure.Value.Json<valueType>[]> { Type = ReturnType.DeSerializeError };
                                }
                                else
                                {
                                    parameter.Set(null, ValueData.DataType.Json);
                                    array[index] = new DataStructure.Value.Json<valueType>(ref parameter);
                                }
                            }
                            if (start == end) return array;
                        }
                    }
                    else if ((end - start) == sizeof(int)) return NullValue<DataStructure.Value.Json<valueType>>.Array;
                }
            }
            return new ReturnValue<DataStructure.Value.Json<valueType>[]> { Type = ReturnType.DeSerializeError };
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static ReturnValue<DataStructure.Value.Binary<valueType>[]> deSerializeBinary(SubArray<byte> data)
        {
            byte[] dataArray = data.Array;
            fixed (byte* dataFixed = dataArray)
            {
                byte* start = dataFixed + data.Start, end = start + (data.Count - sizeof(int));
                uint globalVersion = 0;
                if (AutoCSer.BinarySerialize.SerializeConfig.CheckHeaderValue(start, ref globalVersion) && *(int*)end == data.Count - sizeof(int))
                {
                    int length = *(int*)(start += (globalVersion == 0 ? sizeof(int) : (sizeof(int) + sizeof(uint))));
                    if (length != 0)
                    {
                        int mapLength = ((length + (31 + 32)) >> 5) << 2;
                        if (mapLength <= (int)(end - start))
                        {
                            DataStructure.Value.Binary<valueType>[] array = new DataStructure.Value.Binary<valueType>[length];
                            AutoCSer.BinarySerialize.DeSerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeSerializeArrayMap(start + sizeof(int));
                            ValueData.Data parameter = default(ValueData.Data);
                            start += mapLength;
                            for (int index = 0; index != array.Length; ++index)
                            {
                                if (arrayMap.Next() != 0)
                                {
                                    if ((length = *(int*)start) != 0)
                                    {
                                        if ((mapLength = ((length + (3 + sizeof(int))) & (int.MaxValue - 3))) <= (int)(end - start))
                                        {
                                            parameter.Set(dataArray, (int)(start - dataFixed) + sizeof(int), length, ValueData.DataType.BinarySerialize);
                                            start += mapLength;
                                            array[index] = new DataStructure.Value.Binary<valueType>(ref parameter);
                                        }
                                        else return new ReturnValue<DataStructure.Value.Binary<valueType>[]> { Type = ReturnType.DeSerializeError };
                                    }
                                    else return new ReturnValue<DataStructure.Value.Binary<valueType>[]> { Type = ReturnType.DeSerializeError };
                                }
                                else
                                {
                                    parameter.Set(null, ValueData.DataType.BinarySerialize);
                                    array[index] = new DataStructure.Value.Binary<valueType>(ref parameter);
                                }
                            }
                            if (start == end) return array;
                        }
                    }
                    else if ((end - start) == sizeof(int)) return NullValue<DataStructure.Value.Binary<valueType>>.Array;
                }
            }
            return new ReturnValue<DataStructure.Value.Binary<valueType>[]> { Type = ReturnType.DeSerializeError };
        }

        /// <summary>
        /// 序列化委托
        /// </summary>
        private static readonly Action<UnmanagedStream, valueType[]> Serialize;
        /// <summary>
        /// 反序列化委托
        /// </summary>
        private static readonly Func<SubArray<byte>, ReturnValue<valueType[]>> DeSerialize;
        static ReturnArray()
        {
            switch (ValueData.Data<valueType>.DataType)
            {
                case ValueData.DataType.Json:
                    //if (typeof(valueType).IsGenericType) DeSerialize = (Func<SubArray<byte>, ReturnValue<valueType[]>>)Delegate.CreateDelegate(typeof(Func<SubArray<byte>, ReturnValue<valueType[]>>), typeof(ReturnArray<>).MakeGenericType(typeof(valueType).GetGenericArguments()).GetMethod("deSerializeJson", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(SubArray<byte>) }, null));
                    if (typeof(valueType).IsGenericType) DeSerialize = (Func<SubArray<byte>, ReturnValue<valueType[]>>)AutoCSer.CacheServer.Metadata.GenericType.Get(typeof(valueType).GetGenericArguments()[0]).ValueDataGetJsonDelegate;
                    else Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, Cache.Value.Json[]>)ReturnArray.Serialize;
                    break;
                case ValueData.DataType.BinarySerialize:
                    //if (typeof(valueType).IsGenericType) DeSerialize = (Func<SubArray<byte>, ReturnValue<valueType[]>>)Delegate.CreateDelegate(typeof(Func<SubArray<byte>, ReturnValue<valueType[]>>), typeof(ReturnArray<>).MakeGenericType(typeof(valueType).GetGenericArguments()).GetMethod("deSerializeBinary", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(SubArray<byte>) }, null));
                    if (typeof(valueType).IsGenericType) DeSerialize = (Func<SubArray<byte>, ReturnValue<valueType[]>>)AutoCSer.CacheServer.Metadata.GenericType.Get(typeof(valueType).GetGenericArguments()[0]).ValueDataGetBinaryDelegate;
                    else Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, Cache.Value.Binary[]>)ReturnArray.Serialize;
                    break;
                case ValueData.DataType.ByteArray:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, byte[][]>)ReturnArray.Serialize;
                    break;
                case ValueData.DataType.String:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, string[]>)ReturnArray.Serialize;
                    break;
                case ValueData.DataType.Char:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, char[]>)ReturnArray.Serialize;
                    break;

                case ValueData.DataType.Decimal:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, decimal[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.Guid:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, Guid[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.ULong:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, ulong[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.Long:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, long[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.UInt:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, uint[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.Int:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, int[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.UShort:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, ushort[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.Short:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, short[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break; 
                case ValueData.DataType.Byte:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, byte[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.SByte:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, sbyte[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.Bool:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, bool[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.Float:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, float[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.Double:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, double[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
                case ValueData.DataType.DateTime:
                    Serialize = (Action<UnmanagedStream, valueType[]>)(object)(Action<UnmanagedStream, DateTime[]>)AutoCSer.BinarySerialize.Serializer.Serialize;
                    break;
            }
        }
    }
}
