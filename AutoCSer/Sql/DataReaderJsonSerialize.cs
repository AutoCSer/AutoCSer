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
        private AutoCSer.Json.Serializer serializer;
        /// <summary>
        /// 数据值序列化委托数组
        /// </summary>
        private Action<AutoCSer.Json.Serializer, object>[] serializers;
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
            serializer = Json.Serializer.YieldPool.Default.Pop() ?? new Json.Serializer();
            serializer.SetTcpServer();
            serializer.Config = serializeConfig ?? Json.Serializer.DefaultConfig;
            serializer.CharStream.Reset();
            if (!isSignle)
            {
                serializer.CustomArrayStart();
                serializers = NullValue<Action<AutoCSer.Json.Serializer, object>>.Array;
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

                        Action<AutoCSer.Json.Serializer, object> typeSerializer;
                        if (serializers == null) typeSerializer = typeSerializers[value.GetType()];
                        else
                        {
                            if (serializers.Length == 0) serializers = new Action<Json.Serializer, object>[reader.FieldCount];
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
        private static readonly Dictionary<Type, Action<AutoCSer.Json.Serializer, object>> typeSerializers;
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeInt(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((int)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeString(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((string)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeDateTime(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((DateTime)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeDouble(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((double)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeFloat(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((float)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeDecimal(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((decimal)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeGuid(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((Guid)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeBool(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((bool)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeByte(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((byte)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeByteArray(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.CustomSerialize((byte[])value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeSByte(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((sbyte)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeShort(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((short)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeUShort(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((ushort)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeUInt(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((uint)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeLong(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((long)value);
        }
        /// <summary>
        /// 序列化数据值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private static void serializeULong(AutoCSer.Json.Serializer serializer, object value)
        {
            serializer.Serialize((ulong)value);
        }
        static DataReaderJsonSerialize()
        {
            typeSerializers = DictionaryCreator.CreateOnly<Type, Action<AutoCSer.Json.Serializer, object>>();
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
