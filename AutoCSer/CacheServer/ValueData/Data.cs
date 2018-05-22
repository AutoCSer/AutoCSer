using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 参数数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe partial struct Data// : IEquatable<Parameter>, IComparable<Parameter>
    {
        /// <summary>
        /// 空值
        /// </summary>
        private const uint nullValue = 0x1000000;

        /// <summary>
        /// 整数数据
        /// </summary>
        internal Bit64 Int64;
        /// <summary>
        /// 对象数据
        /// </summary>
        internal object Value;
        /// <summary>
        /// 数据类型 [byte]
        /// </summary>
        internal DataType Type;
        /// <summary>
        /// 设置操作是否返回原数据
        /// </summary>
        internal bool IsSetReturn
        {
            get
            {
                switch (Type)
                {
                    case DataType.Guid:
                    case DataType.Decimal:
                    case DataType.ULong:
                    case DataType.Long:
                    case DataType.UInt:
                    case DataType.Int:
                    case DataType.UShort:
                    case DataType.Short:
                    case DataType.SByte:
                    case DataType.Byte:
                    case DataType.Char:
                    case DataType.Float:
                    case DataType.Double:
                    case DataType.DateTime:
                    case DataType.Bool:
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否缓冲区数据
        /// </summary>
        internal bool IsBuffer;

        /// <summary>
        /// 操作类型（临时附加数据） [ushort]
        /// </summary>
        internal OperationParameter.OperationType OperationType;
        /// <summary>
        /// TCP 返回值类型 [byte]
        /// </summary>
        internal AutoCSer.Net.TcpServer.ReturnType TcpReturnType;

        /// <summary>
        /// Guid
        /// </summary>
        internal System.Guid Guid
        {
            get
            {
                if (IsBuffer)
                {
                    fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) return *(System.Guid*)(dataFixed + Int64.Index);
                }
                return new UnionType { Value = Value }.Guid.Value;
            }
        }
        /// <summary>
        /// decimal
        /// </summary>
        internal decimal Decimal
        {
            get
            {
                if (IsBuffer)
                {
                    fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) return *(decimal*)(dataFixed + Int64.Index);
                }
                return new UnionType { Value = Value }.Decimal.Value;
            }
        }
        /// <summary>
        /// 字节数组
        /// </summary>
        internal byte[] ByteArray
        {
            get
            {
                if (IsBuffer) return new SubArray<byte>(Int64.Index, Int64.Length, new UnionType { Value = Value }.ByteArray).GetArray();
                return new UnionType { Value = Value }.ByteArray;
            }
        }
        /// <summary>
        /// 字符串
        /// </summary>
        internal string String
        {
            get
            {
                if (IsBuffer) deSerializeString();
                return new UnionType { Value = Value }.String;
            }
        }
        /// <summary>
        /// 字节数组
        /// </summary>
        internal SubArray<byte> SubByteArray
        {
            get
            {
                if (IsBuffer) return new SubArray<byte>(Int64.Index, Int64.Length, new UnionType { Value = Value }.ByteArray);
                return new SubArray<byte>(new UnionType { Value = Value }.ByteArray);
            }
        }

        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ulong value)
        {
            Int64.ULong = value;
            Type = DataType.ULong;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(long value)
        {
            Int64.Long = value;
            Type = DataType.Long;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(uint value)
        {
            Int64.UInt = value;
            Type = DataType.UInt;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int value)
        {
            Int64.Int = value;
            Type = DataType.Int;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ushort value)
        {
            Int64.UShort = value;
            Type = DataType.UShort;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(short value)
        {
            Int64.Short = value;
            Type = DataType.Short;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte value)
        {
            Int64.Byte = value;
            Type = DataType.Byte;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(sbyte value)
        {
            Int64.SByte = value;
            Type = DataType.SByte;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(char value)
        {
            Int64.Char = value;
            Type = DataType.Char;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(bool value)
        {
            Int64.Bool = value;
            Type = DataType.Bool;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(float value)
        {
            Int64.Float = value;
            Type = DataType.Float;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(double value)
        {
            Int64.Double = value;
            Type = DataType.Double;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(DateTime value)
        {
            Int64.DateTime = value;
            Type = DataType.DateTime;
        }

        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ref decimal value)
        {
            Value = new Decimal(value);
            Type = DataType.Decimal;
            IsBuffer = false;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ref System.Guid value)
        {
            Value = new Guid(value);
            Type = DataType.Guid;
            IsBuffer = false;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(string value)
        {
            Value = value;
            Type = DataType.String;
            IsBuffer = false;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] value)
        {
            Value = value;
            Type = DataType.ByteArray;
            IsBuffer = false;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetBinary<valueType>(valueType value)
        {
            Value = new BinarySerializer<valueType>(value);
            Type = DataType.BinarySerialize;
            IsBuffer = false;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetJson<valueType>(valueType value)
        {
            Value = new JsonSerializer<valueType>(value);
            Type = DataType.Json;
            IsBuffer = false;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="type"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] value, DataType type)
        {
            Value = value;
            Int64.Set(0, value.Length);
            Type = type;
            IsBuffer = true;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        internal void Serialize(UnmanagedStream stream)
        {
            byte* write;
            switch (Type)
            {
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                    write = stream.GetPrepSizeCurrent(sizeof(uint) + sizeof(ulong));
                    *(uint*)write = (byte)Type;
                    *(ulong*)(write + sizeof(uint)) = Int64.ULong;
                    stream.ByteSize += sizeof(uint) + sizeof(ulong);
                    return;
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                    write = stream.GetPrepSizeCurrent(sizeof(uint) + sizeof(uint));
                    *(uint*)write = (byte)Type;
                    *(uint*)(write + sizeof(uint)) = Int64.UInt;
                    stream.ByteSize += sizeof(uint) + sizeof(uint);
                    return;
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                    stream.Write((byte)Type + ((uint)Int64.UShort << 16));
                    return;
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    stream.Write((byte)Type + ((uint)Int64.Byte << 16));
                    return;
                case DataType.Decimal:
                    write = stream.GetPrepSizeCurrent(sizeof(uint) + sizeof(decimal));
                    *(uint*)write = (byte)Type;
                    if (IsBuffer)
                    {
                        fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) *(decimal*)(write + sizeof(uint)) = *(decimal*)(dataFixed + Int64.Index);
                    }
                    else *(decimal*)(write + sizeof(uint)) = new UnionType { Value = Value }.Decimal.Value;
                    stream.ByteSize += sizeof(uint) + sizeof(decimal);
                    return;
                case DataType.Guid:
                    write = stream.GetPrepSizeCurrent(sizeof(uint) + sizeof(System.Guid));
                    *(uint*)write = (byte)Type;
                    if (IsBuffer) 
                    {
                        fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) *(System.Guid*)(write + sizeof(uint)) = *(System.Guid*)(dataFixed + Int64.Index);
                    }
                    else *(System.Guid*)(write + sizeof(uint)) = new UnionType { Value = Value }.Guid.Value;
                    stream.ByteSize += sizeof(uint) + sizeof(System.Guid);
                    return;
                case DataType.String:
                    if (Value != null)
                    {
                        if (IsBuffer) serializeBuffer(stream);
                        else
                        {
                            string value = new UnionType { Value = Value }.String;
                            int length = stream.AddSize((sizeof(uint) + sizeof(int)));
                            fixed (char* valueFixed = value) AutoCSer.BinarySerialize.Serializer.Serialize(valueFixed, stream, value.Length);
                            write = stream.Data.Byte + length;
                            *(int*)(write - sizeof(int)) = stream.ByteSize - length;
                            *(uint*)(write - (sizeof(uint) + sizeof(int))) = (byte)Type;
                        }
                    }
                    else stream.Write((byte)Type + nullValue);
                    return;
                case DataType.ByteArray:
                    if (Value != null)
                    {
                        if (IsBuffer)
                        {
                            int length = Int64.Length, size = (length + (3 + (sizeof(uint) + sizeof(int)))) & (int.MaxValue - 3);
                            write = stream.GetPrepSizeCurrent(size);
                            *(uint*)write = (byte)Type;
                            *(int*)(write + sizeof(uint)) = length;
                            fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) AutoCSer.Memory.CopyNotNull(dataFixed + Int64.Index, write + (sizeof(uint) + sizeof(int)), length);
                            stream.ByteSize += size;
                        }
                        else
                        {
                            byte[] value = new UnionType { Value = Value }.ByteArray;
                            int size = (value.Length + (3 + (sizeof(uint) + sizeof(int)))) & (int.MaxValue - 3);
                            write = stream.GetPrepSizeCurrent(size);
                            *(uint*)write = (byte)Type;
                            *(int*)(write + sizeof(uint)) = value.Length;
                            fixed (byte* dataFixed = value) AutoCSer.Memory.CopyNotNull(dataFixed, write + (sizeof(uint) + sizeof(int)), value.Length);
                            stream.ByteSize += size;
                        }
                    }
                    else stream.Write((byte)Type + nullValue);
                    return;
                case DataType.BinarySerialize:
                case DataType.Json:
                    if (Value != null)
                    {
                        if (IsBuffer) serializeBuffer(stream);
                        else
                        {
                            int length = stream.AddSize((sizeof(uint) + sizeof(int)));
                            new UnionType { Value = Value }.BinarySerializer.Serialize(stream);
                            write = stream.Data.Byte + length;
                            *(int*)(write - sizeof(int)) = stream.ByteSize - length;
                            *(uint*)(write - (sizeof(uint) + sizeof(int))) = (byte)Type;
                        }
                    }
                    else stream.Write((byte)Type + nullValue);
                    return;
            }
            stream.Write((uint)(byte)DataType.Null);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        private void serializeBuffer(UnmanagedStream stream)
        {
            int length = Int64.Length;
            byte* write = stream.GetPrepSizeCurrent(length + (sizeof(uint) + sizeof(int)));
            *(uint*)write = (byte)Type;
            *(int*)(write + sizeof(uint)) = length;
            fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) Memory.CopyNotNull(dataFixed + Int64.Index, write + (sizeof(uint) + sizeof(int)), length);
            stream.ByteSize += length + (sizeof(uint) + sizeof(int));
        }

        /// <summary>
        /// 反序列化字符串
        /// </summary>
        private void deSerializeString()
        {
            fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray)
            {
                byte* start = dataFixed + Int64.Index;
                Value = AutoCSer.BinarySerialize.DeSerializer.DeSerializeString(ref start, start + Int64.Length);
            }
            IsBuffer = false;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <returns></returns>
        internal bool DeSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            uint type = (uint)deSerializer.ReadInt();
            switch (Type = (DataType)(byte)type)
            {
                case DataType.Null: return true;
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                    Int64.ULong = *(ulong*)deSerializer.Read;
                    return deSerializer.MoveRead(sizeof(ulong));
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                    Int64.UInt = *(uint*)deSerializer.Read;
                    return deSerializer.MoveRead(sizeof(uint));
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    Int64.UInt = type >> 16;
                    return true;
                case DataType.Decimal:
                    Value = new Decimal(*(decimal*)deSerializer.Read);
                    return deSerializer.MoveRead(sizeof(decimal));
                case DataType.Guid:
                    Value = new Guid(*(System.Guid*)deSerializer.Read);
                    return deSerializer.MoveRead(sizeof(System.Guid));
                case DataType.String:
                    if ((type & nullValue) == 0) return deSerializeString(deSerializer);
                    return true;
                case DataType.ByteArray:
                    if ((type & nullValue) == 0)
                    {
                        int length = deSerializer.ReadInt();
                        if (length != 0) return deSerializeByteArray(deSerializer, length);
                        Value = NullValue<byte>.Array;
                    }
                    return true;
                case DataType.BinarySerialize:
                case DataType.Json:
                    if ((type & nullValue) == 0) return deSerializeByteArray(deSerializer, deSerializer.ReadInt());
                    return true;
            }
            deSerializer.MoveRead(-1);
            return false;
        }
        /// <summary>
        /// 反序列化字符串
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <returns></returns>
        private bool deSerializeString(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            deSerializer.Read += sizeof(int);
            string value = null;
            if (deSerializer.DeSerializeString(ref value))
            {
                Value = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 反序列化字节数组
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private bool deSerializeByteArray(AutoCSer.BinarySerialize.DeSerializer deSerializer, int length)
        {
            byte* read = deSerializer.Read;
            if (deSerializer.MoveRead((length + 3) & (int.MaxValue - 3)))
            {
                byte[] value = new byte[length];
                AutoCSer.Memory.CopyNotNull(read, value, length);
                Value = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 反序列化网络流
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <returns></returns>
        internal bool DeSerializeSynchronous(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            uint type = (uint)deSerializer.ReadInt();
            switch (Type = (DataType)(byte)type)
            {
                case DataType.Null: return true;
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                    Int64.ULong = *(ulong*)deSerializer.Read;
                    return deSerializer.MoveRead(sizeof(ulong));
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                    Int64.UInt = *(uint*)deSerializer.Read;
                    return deSerializer.MoveRead(sizeof(uint));
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    Int64.UInt = type >> 16;
                    return true;
                case DataType.Decimal:
                    return deSerializeSynchronous(deSerializer, sizeof(decimal));
                case DataType.Guid:
                    return deSerializeSynchronous(deSerializer, sizeof(System.Guid));
                case DataType.String:
                    if ((type & nullValue) == 0) return deSerializeString(deSerializer);
                    return true;
                case DataType.ByteArray:
                    if ((type & nullValue) == 0)
                    {
                        int length = deSerializer.ReadInt();
                        if (length != 0) return deSerializeSynchronous(deSerializer, length);
                        Value = NullValue<byte>.Array;
                    }
                    return true;
                case DataType.BinarySerialize:
                case DataType.Json:
                    if ((type & nullValue) == 0) return deSerializeSynchronous(deSerializer, deSerializer.ReadInt());
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 反序列化网络流
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool deSerializeSynchronous(AutoCSer.BinarySerialize.DeSerializer deSerializer, int size)
        {
            byte* read = deSerializer.Read;
            if (deSerializer.MoveRead((size + 3) & (int.MaxValue - 3)))
            {
                IsBuffer = true;
                SubArray<byte> data = default(SubArray<byte>);
                deSerializer.DeSerializeTcpServer(ref data, read, size);
                Value = data.Array;
                Int64.Set(data.Start, size);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="parser"></param>
        internal void Load(ref OperationParameter.NodeParser parser)
        {
            uint type = (uint)parser.ReadInt();
            switch (Type = (DataType)(byte)type)
            {
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                    Int64.ULong = (ulong)parser.ReadLong();
                    return;
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                    Int64.UInt = (uint)parser.ReadInt();
                    return;
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    Int64.UInt = type >> 16;
                    return;
                case DataType.Decimal:
                    parser.SetValueData(sizeof(decimal));
                    IsBuffer = true;
                    return;
                case DataType.Guid:
                    parser.SetValueData(sizeof(System.Guid));
                    IsBuffer = true;
                    return;
                case DataType.ByteArray:
                    if ((type & nullValue) == 0)
                    {
                        parser.SetByteArray();
                        IsBuffer = true;
                    }
                    return;
                case DataType.String:
                case DataType.BinarySerialize:
                case DataType.Json:
                    if ((type & nullValue) == 0)
                    {
                        parser.SetString();
                        IsBuffer = true;
                    }
                    return;
            }
        }

        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, ulong value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, long value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, int value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, uint value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, short value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, ushort value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, byte value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, sbyte value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, char value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, float value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, double value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, DateTime value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, decimal value)
        {
            parameter.Set(ref value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, System.Guid value)
        {
            parameter.Set(ref value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, string value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, bool value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, byte[] value)
        {
            parameter.Set(value);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, Cache.Value.Json value)
        {
            parameter.Set(value.Data, DataType.Json);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="parameter">参数数据</param>
        /// <param name="value">数据</param>
        internal static void Set(ref Data parameter, Cache.Value.Binary value)
        {
            parameter.Set(value.Data, DataType.BinarySerialize);
        }

        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static ulong GetULong(ref Data parameter)
        {
            return parameter.Int64.ULong;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static long GetLong(ref Data parameter)
        {
            return parameter.Int64.Long;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static uint GetUInt(ref Data parameter)
        {
            return parameter.Int64.UInt;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static int GetInt(ref Data parameter)
        {
            return parameter.Int64.Int;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static ushort GetUShort(ref Data parameter)
        {
            return parameter.Int64.UShort;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static short GetShort(ref Data parameter)
        {
            return parameter.Int64.Short;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static byte GetByte(ref Data parameter)
        {
            return parameter.Int64.Byte;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static sbyte GetSByte(ref Data parameter)
        {
            return parameter.Int64.SByte;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static char GetChar(ref Data parameter)
        {
            return parameter.Int64.Char;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static float GetFloat(ref Data parameter)
        {
            return parameter.Int64.Float;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static double GetDouble(ref Data parameter)
        {
            return parameter.Int64.Double;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static DateTime GetDateTime(ref Data parameter)
        {
            return parameter.Int64.DateTime;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static decimal GetDecimal(ref Data parameter)
        {
            return parameter.Decimal;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static System.Guid GetGuid(ref Data parameter)
        {
            return parameter.Guid;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static bool GetBool(ref Data parameter)
        {
            return parameter.Int64.Bool;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static byte[] GetByteArray(ref Data parameter)
        {
            return parameter.ByteArray;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static string GetString(ref Data parameter)
        {
            return parameter.String;
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static Cache.Value.Json GetJson(ref Data parameter)
        {
            return new Cache.Value.Json { Data = parameter.ByteArray };
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static Cache.Value.Binary GetBinary(ref Data parameter)
        {
            return new Cache.Value.Binary { Data = parameter.ByteArray };
        }
    }
    /// <summary>
    /// 参数数据
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal static class Data<valueType>
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        internal static readonly DataType DataType;
        /// <summary>
        /// 是否 HASH 键值类型
        /// </summary>
        internal static bool IsHashKey
        {
            get
            {
                switch (DataType)
                {
                    case DataType.String:
                    case DataType.Int:
                    case DataType.Long:
                    case DataType.ULong:
                    case DataType.UInt:
                    case DataType.Short:
                    case DataType.UShort:
                    case DataType.Byte:
                    case DataType.SByte:
                    case DataType.Char:
                    case DataType.DateTime:
                    case DataType.Float:
                    case DataType.Double:
                    case DataType.Decimal:

                    case DataType.Guid:
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否排序关键字类型
        /// </summary>
        internal static bool IsSortKey
        {
            get
            {
                switch (DataType)
                {
                    case DataType.String:
                    case DataType.Int:
                    case DataType.Long:
                    case DataType.ULong:
                    case DataType.UInt:
                    case DataType.Short:
                    case DataType.UShort:
                    case DataType.Byte:
                    case DataType.SByte:
                    case DataType.Char:
                    case DataType.DateTime:
                    case DataType.Float:
                    case DataType.Double:
                    case DataType.Decimal:
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否数字类型
        /// </summary>
        internal static bool IsNumber
        {
            get
            {
                switch (DataType)
                {
                    case DataType.Int:
                    case DataType.Long:
                    case DataType.ULong:
                    case DataType.UInt:
                    case DataType.Short:
                    case DataType.UShort:
                    case DataType.Byte:
                    case DataType.SByte:
                    case DataType.Float:
                    case DataType.Double:
                    case DataType.Decimal:
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否整数类型
        /// </summary>
        internal static bool IsInteger
        {
            get
            {
                switch (DataType)
                {
                    case DataType.Int:
                    case DataType.Long:
                    case DataType.ULong:
                    case DataType.UInt:
                    case DataType.Short:
                    case DataType.UShort:
                    case DataType.Byte:
                    case DataType.SByte:
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 设置参数数据委托
        /// </summary>
        internal static readonly SetData<valueType> SetData;
        /// <summary>
        /// 服务端返回值设置参数数据委托
        /// </summary>
        internal static readonly SetData<valueType> SetReturn;
        /// <summary>
        /// 获取参数数据委托
        /// </summary>
        internal static readonly GetData<valueType> GetData;

        static Data()
        {
            Type type = typeof(valueType);
            if (type == typeof(Cache.Value.Json))
            {
                DataType = DataType.Json;
                SetData = (SetData<valueType>)(object)(SetData<Cache.Value.Json>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<Cache.Value.Json>)Data.GetJson;
                return;
            }
            if (type == typeof(Cache.Value.Binary))
            {
                DataType = DataType.BinarySerialize;
                SetData = (SetData<valueType>)(object)(SetData<Cache.Value.Binary>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<Cache.Value.Binary>)Data.GetBinary;
                return;
            }
            if (type == typeof(string))
            {
                DataType = DataType.String;
                SetData = (SetData<valueType>)(object)(SetData<string>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<string>)Data.GetString;
                return;
            }
            if (type == typeof(int))
            {
                DataType = DataType.Int;
                SetData = (SetData<valueType>)(object)(SetData<int>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<int>)Data.GetInt;
                return;
            }
            if (type == typeof(long))
            {
                DataType = DataType.Long;
                SetData = (SetData<valueType>)(object)(SetData<long>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<long>)Data.GetLong;
                return;
            }
            if (type == typeof(ulong))
            {
                DataType = DataType.ULong;
                SetData = (SetData<valueType>)(object)(SetData<ulong>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<ulong>)Data.GetULong;
                return;
            }
            if (type == typeof(uint))
            {
                DataType = DataType.UInt;
                SetData = (SetData<valueType>)(object)(SetData<uint>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<uint>)Data.GetUInt;
                return;
            }
            if (type == typeof(short))
            {
                DataType = DataType.Short;
                SetData = (SetData<valueType>)(object)(SetData<short>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<short>)Data.GetShort;
                return;
            }
            if (type == typeof(ushort))
            {
                DataType = DataType.UShort;
                SetData = (SetData<valueType>)(object)(SetData<ushort>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<ushort>)Data.GetUShort;
                return;
            }
            if (type == typeof(byte))
            {
                DataType = DataType.Byte;
                SetData = (SetData<valueType>)(object)(SetData<byte>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<byte>)Data.GetByte;
                return;
            }
            if (type == typeof(sbyte))
            {
                DataType = DataType.SByte;
                SetData = (SetData<valueType>)(object)(SetData<sbyte>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<sbyte>)Data.GetSByte;
                return;
            }
            if (type == typeof(char))
            {
                DataType = DataType.Char;
                SetData = (SetData<valueType>)(object)(SetData<char>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<char>)Data.GetChar;
                return;
            }
            if (type == typeof(DateTime))
            {
                DataType = DataType.DateTime;
                SetData = (SetData<valueType>)(object)(SetData<DateTime>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<DateTime>)Data.GetDateTime;
                return;
            }
            if (type == typeof(float))
            {
                DataType = DataType.Float;
                SetData = (SetData<valueType>)(object)(SetData<float>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<float>)Data.GetFloat;
                return;
            }
            if (type == typeof(double))
            {
                DataType = DataType.Double;
                SetData = (SetData<valueType>)(object)(SetData<double>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<double>)Data.GetDouble;
                return;
            }
            if (type == typeof(decimal))
            {
                DataType = DataType.Decimal;
                SetData = (SetData<valueType>)(object)(SetData<decimal>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<decimal>)Data.GetDecimal;
                return;
            }
            if (type == typeof(System.Guid))
            {
                DataType = DataType.Guid;
                SetData = (SetData<valueType>)(object)(SetData<System.Guid>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<System.Guid>)Data.GetGuid;
                return;
            }
            if (type == typeof(bool))
            {
                DataType = DataType.Bool;
                SetData = (SetData<valueType>)(object)(SetData<bool>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<bool>)Data.GetBool;
                return;
            }
            if (type == typeof(byte[]))
            {
                DataType = DataType.ByteArray;
                SetData = (SetData<valueType>)(object)(SetData<byte[]>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<byte[]>)Data.GetByteArray;
                return;
            }
        }
    }
}
