using System;
using System.Data.Common;
using System.Collections.Generic;
using AutoCSer.Json;

namespace AutoCSer.Sql
{
    /// <summary>
    /// DataReader JSON 序列化
    /// </summary>
    public sealed class DataReaderJsonSerialize : IDisposable
    {
        /// <summary>
        /// JSON 序列化
        /// </summary>
        private AutoCSer.JsonSerializer serializer;
        /// <summary>
        /// 数据值序列化委托数组
        /// </summary>
        private Action<AutoCSer.JsonSerializer, object>[] serializers;
        /// <summary>
        /// 是否忽略 null
        /// </summary>
        private readonly bool isIgnoreNull;
        /// <summary>
        /// 是否已经写入数据
        /// </summary>
        private bool isValue;
        /// <summary>
        /// DataReader JSON 序列化
        /// </summary>
        /// <param name="isSignle">是否单条记录，否则为数组</param>
        /// <param name="serializeConfig"></param>
        /// <param name="isIgnoreNull">是否忽略 null</param>
        public DataReaderJsonSerialize(bool isSignle, SerializeConfig serializeConfig = null, bool isIgnoreNull = true)
        {
            this.isIgnoreNull = isIgnoreNull;
            serializer = AutoCSer.JsonSerializer.YieldPool.Default.Pop() ?? new AutoCSer.JsonSerializer();
            serializer.SetTcpServer();
            serializer.Config = serializeConfig ?? AutoCSer.JsonSerializer.DefaultConfig;
            serializer.CharStream.Reset();
            if (!isSignle)
            {
                serializer.CustomArrayStart();
                serializers = EmptyArray<Action<AutoCSer.JsonSerializer, object>>.Array;
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="reader"></param>
        public void Serialize(DbDataReader reader)
        {
            if (this.isValue) serializer.CharStream.Write(',');
            else this.isValue = true;
            int fieldCount = reader.FieldCount;
            bool isValue = false;
            if (fieldCount != 0)
            {
                for (int index = 0; index != fieldCount; ++index)
                {
                    if (reader.IsDBNull(index))
                    {
                        if (!isIgnoreNull)
                        {
                            if (isValue) serializer.CustomWriteNextName(reader.GetName(index));
                            else
                            {
                                serializer.CustomWriteFirstName(reader.GetName(index));
                                isValue = true;
                            }
                            serializer.CharStream.WriteJsonNull();
                        }
                    }
                    else
                    {
                        if (isValue) serializer.CustomWriteNextName(reader.GetName(index));
                        else
                        {
                            serializer.CustomWriteFirstName(reader.GetName(index));
                            isValue = true;
                        }
                        object value = reader.GetValue(index);

                        Action<AutoCSer.JsonSerializer, object> typeSerializer;
                        if (serializers == null) typeSerializer = typeSerializers[value.GetType()];
                        else
                        {
                            if (serializers.Length == 0) serializers = new Action<AutoCSer.JsonSerializer, object>[reader.FieldCount];
                            if ((typeSerializer = serializers[index]) == null) serializers[index] = typeSerializer = typeSerializers[value.GetType()];
                        }
                        typeSerializer(serializer, value);
                    }
                }
            }
            if (isValue) serializer.CustomObjectEnd();
            else serializer.CharStream.WriteJsonObject();
        }
        /// <summary>
        /// 结束并获取字符串
        /// </summary>
        /// <returns></returns>
        public string End()
        {
            if (serializers != null)
            {
                serializer.CustomArrayEnd();
                serializers = null;
            }
            string json = serializer.CharStream.ToString();
            Dispose();
            return json;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (serializer != null)
            {
                serializer.CharStream.Dispose();
                serializer.Free();
                serializer = null;
            }
        }

        /// <summary>
        /// JSON 序列化委托集合
        /// </summary>
        private static readonly Dictionary<Type, Action<AutoCSer.JsonSerializer, object>> typeSerializers;
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeInt(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((int)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeString(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((string)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeDateTime(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((DateTime)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeDouble(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((double)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeFloat(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((float)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeDecimal(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((decimal)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeGuid(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((Guid)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeBool(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((bool)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeByte(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((byte)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeByteArray(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((byte[])value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeSByte(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((sbyte)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeShort(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((short)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeUShort(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((ushort)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeUInt(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((uint)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeLong(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((long)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeULong(AutoCSer.JsonSerializer serializer, object value)
        {
            serializer.CallSerialize((ulong)value);
        }
        static DataReaderJsonSerialize()
        {
            typeSerializers = DictionaryCreator.CreateOnly<Type, Action<AutoCSer.JsonSerializer, object>>();
            typeSerializers.Add(typeof(int), serializeInt);
            typeSerializers.Add(typeof(string), serializeString);
            typeSerializers.Add(typeof(DateTime), serializeDateTime);
            typeSerializers.Add(typeof(double), serializeDouble);
            typeSerializers.Add(typeof(float), serializeFloat);
            typeSerializers.Add(typeof(decimal), serializeDecimal);
            typeSerializers.Add(typeof(Guid), serializeGuid);
            typeSerializers.Add(typeof(bool), serializeBool);
            typeSerializers.Add(typeof(byte), serializeByte);
            typeSerializers.Add(typeof(byte[]), serializeByteArray);
            typeSerializers.Add(typeof(sbyte), serializeSByte);
            typeSerializers.Add(typeof(short), serializeShort);
            typeSerializers.Add(typeof(ushort), serializeUShort);
            typeSerializers.Add(typeof(uint), serializeUInt);
            typeSerializers.Add(typeof(long), serializeLong);
            typeSerializers.Add(typeof(ulong), serializeULong);
        }
    }
}
