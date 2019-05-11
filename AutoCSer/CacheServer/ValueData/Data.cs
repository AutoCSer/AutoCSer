using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 参数数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public unsafe partial struct Data// : IEquatable<Parameter>, IComparable<Parameter>
    {
        /// <summary>
        /// 空值
        /// </summary>
        private const uint nullValue = 0x100;

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
        /// 设置为 null
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetNull()
        {
            Value = null;
            Type = DataType.Null;
        }

        /// <summary>
        /// 操作类型（临时附加数据） [ushort]
        /// </summary>
        internal OperationParameter.OperationType OperationType;
        /// <summary>
        /// 尝试设置操作类型
        /// </summary>
        /// <param name="operationType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool TrySetOperationType(OperationParameter.OperationType operationType)
        {
            if (OperationType == OperationParameter.OperationType.Unknown)
            {
                OperationType = operationType;
                return true;
            }
            return OperationType == operationType;
        }

        /// <summary>
        /// 返回值类型（临时附加数据） [byte]
        /// </summary>
        internal ReturnType ReturnType;
        /// <summary>
        /// 返回值类型
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        internal Data(ReturnType returnType)
        {
            Int64 = default(Bit64);
            Value = null;
            Type = DataType.Null;
            IsBuffer = IsReturnDeSerializeStream = false;
            OperationType = OperationParameter.OperationType.Unknown;

            ReturnType = returnType;
        }
        /// <summary>
        /// 是否反序列化网络流，否则需要 Copy 数据
        /// </summary>
        internal bool IsReturnDeSerializeStream;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeReturnParameter(UnmanagedStream stream)
        {
            stream.Write(IsReturnDeSerializeStream ? (uint)(byte)ReturnType + 0x100U : (uint)(byte)ReturnType);
            if (ReturnType == ReturnType.Success) Serialize(stream);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DeSerializeReturnParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            uint type = (uint)deSerializer.ReadInt();
            ReturnType = (ReturnType)(byte)type;
            if (ReturnType == ReturnType.Success)
            {
                if ((IsReturnDeSerializeStream = (type & 0x100U) != 0) ? DeSerializeSynchronous(deSerializer) : DeSerialize(deSerializer)) return;
                ReturnType = ReturnType.DeSerializeError;
            }
        }

        ///// <summary>
        ///// TCP 返回值类型 [byte]
        ///// </summary>
        //internal AutoCSer.Net.TcpServer.ReturnType TcpReturnType;

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
        /// 获取 JSON 反序列化数据
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool GetJson<valueType>(ref valueType value)
        {
            if (IsBuffer)
            {
                fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray)
                {
                    byte* start = dataFixed + Int64.Index;
                    return AutoCSer.Json.Parser.UnsafeParse<valueType>((char*)start, Int64.Length >> 1, ref value).State == Json.ParseState.Success;
                }
            }
            if (Value != null)
            {
                byte[] data = Value as byte[];
                if (data != null)
                {
                    fixed (byte* dataFixed = data)
                    {
                        return AutoCSer.Json.Parser.UnsafeParse<valueType>((char*)dataFixed, data.Length >> 1, ref value).State == Json.ParseState.Success;
                    }
                }
                return false;
            }
            value = default(valueType);
            return true;
        }
        /// <summary>
        /// 获取二进制反序列化数据
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool GetBinary<valueType>(ref valueType value)
        {
            if (IsBuffer)
            {
                return AutoCSer.BinarySerialize.DeSerializer.DeSerialize(new SubArray<byte>(Int64.Index, Int64.Length, new UnionType { Value = Value }.ByteArray), ref value).State == AutoCSer.BinarySerialize.DeSerializeState.Success;
            }
            if (Value != null)
            {
                byte[] data = Value as byte[];
                return data != null && AutoCSer.BinarySerialize.DeSerializer.DeSerialize(data, ref value).State == AutoCSer.BinarySerialize.DeSerializeState.Success;
            }
            value = default(valueType);
            return true;
        }

        /// <summary>
        /// ulong
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<ulong> GetULong(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.ULong;
            return new ReturnValue<ulong> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// long
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<long> GetLong(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.Long;
            return new ReturnValue<long> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// uint
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<uint> GetUInt(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.UInt;
            return new ReturnValue<uint> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// int
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<int> GetInt(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.Int;
            return new ReturnValue<int> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// ushort
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<ushort> GetUShort(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.UShort;
            return new ReturnValue<ushort> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// short
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<short> GetShort(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.Short;
            return new ReturnValue<short> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// byte
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<byte> GetByte(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.Byte;
            return new ReturnValue<byte> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// sbyte
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<sbyte> GetSByte(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.SByte;
            return new ReturnValue<sbyte> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// bool
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<bool> GetBool(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.Bool;
            return new ReturnValue<bool> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// float
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<float> GetFloat(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.Float;
            return new ReturnValue<float> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// double
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<double> GetDouble(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Int64.Double;
            return new ReturnValue<double> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// decimal
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<decimal> GetDecimal(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success) return Decimal;
            return new ReturnValue<decimal> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// 获取二进制反序列化数据
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        internal ReturnValue<valueType> GetBinary<valueType>(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (ReturnType == ReturnType.Success)
            {
                valueType value = default(valueType);
                if (GetBinary(ref value)) return value;
                return new ReturnValue<valueType> { Type = ReturnType.DeSerializeError };
            }
            return new ReturnValue<valueType> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// bool
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<valueType> GetBool<valueType>(AutoCSer.Net.TcpServer.ReturnType tcpReturnType, valueType returnValue)
            where valueType : class
        {
            if (ReturnType == ReturnType.Success) return Int64.Bool ? returnValue : null;
            return new ReturnValue<valueType> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? ReturnType : ReturnType.TcpError, TcpReturnType = tcpReturnType };
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
            Value = value != null ? new BinarySerializer<valueType>(value) : null;
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
            Value = value != null ? new JsonSerializer<valueType>(value) : null;
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
        /// 设置参数数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="type"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] value, int start, int length, DataType type)
        {
            Value = value;
            Int64.Set(start, length);
            Type = type;
            IsBuffer = true;
        }

        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ReturnParameterSet(ulong parameter)
        {
            Set(parameter);
            ReturnType = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ReturnParameterSet(int parameter)
        {
            Set(parameter);
            ReturnType = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ReturnParameterSet(uint parameter)
        {
            Set(parameter);
            ReturnType = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ReturnParameterSet(bool parameter)
        {
            Set(parameter);
            ReturnType = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="value">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ReturnParameterSetBinary<valueType>(valueType value)
        {
            SetBinary(value);
            ReturnType = ReturnType.Success;
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
                    else
                    {
                        IsBuffer = false;
                        Value = null;
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
                    else
                    {
                        IsBuffer = false;
                        Value = null;
                    }
                    return;
            }
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <returns></returns>
        internal BufferCount CopyToMessageQueueBufferCount()
        {
            switch (Type)
            {
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    return null;
                case DataType.Decimal:
                    fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) Value = new Decimal(*(decimal*)(dataFixed + Int64.Index));
                    IsBuffer = false;
                    return null;
                case DataType.Guid:
                    fixed (byte* dataFixed = new UnionType { Value = Value }.ByteArray) Value = new Guid(*(System.Guid*)(dataFixed + Int64.Index));
                    IsBuffer = false;
                    return null;
                case DataType.ByteArray:
                    if (IsBuffer)
                    {
                        if (Int64.Length != 0)
                        {
                            SubArray<byte> buffer = new SubArray<byte>(0, (Int64.Length + 3) & (int.MaxValue - 3), null);
                            BufferCount bufferCount = Cache.MessageQueue.BufferCount.GetBufferCount(ref buffer);
                            System.Buffer.BlockCopy(new UnionType { Value = Value }.ByteArray, Int64.Index, buffer.Array, buffer.Start, buffer.Length);
                            Value = buffer.Array;
                            Int64.Index = buffer.Start;
                            return bufferCount;
                        }
                    }
                    return null;
                case DataType.String:
                case DataType.BinarySerialize:
                case DataType.Json:
                    if (IsBuffer)
                    {
                        SubArray<byte> buffer = new SubArray<byte>(0, Int64.Length, null);
                        BufferCount bufferCount = Cache.MessageQueue.BufferCount.GetBufferCount(ref buffer);
                        System.Buffer.BlockCopy(new UnionType { Value = Value }.ByteArray, Int64.Index, buffer.Array, buffer.Start, Int64.Length);
                        Value = buffer.Array;
                        Int64.Index = buffer.Start;
                        return bufferCount;
                    }
                    return null;
            }
            return null;
        }
        /// <summary>
        /// 缓冲区字节大小
        /// </summary>
        internal int GetSerializeBufferSize(out bool isCopyBuffer)
        {
            switch (Type)
            {
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                    isCopyBuffer = false;
                    return sizeof(uint) + sizeof(ulong);
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                    isCopyBuffer = false;
                    return sizeof(uint) + sizeof(uint);
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    isCopyBuffer = false;
                    return sizeof(uint);
                case DataType.Decimal:
                    isCopyBuffer = false;
                    return sizeof(uint) + sizeof(decimal);
                case DataType.Guid:
                    isCopyBuffer = false;
                    return sizeof(uint) + sizeof(System.Guid);
                case DataType.ByteArray:
                    isCopyBuffer = IsBuffer;
                    return IsBuffer ? (Int64.Length + (3 + sizeof(int) * 2)) & (int.MaxValue - 3) : sizeof(uint);
                case DataType.String:
                case DataType.BinarySerialize:
                case DataType.Json:
                    isCopyBuffer = IsBuffer;
                    return IsBuffer ? Int64.Length + sizeof(int) * 2 : sizeof(uint);
            }
            isCopyBuffer = IsBuffer;
            return sizeof(int);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="write"></param>
        internal void SerializeBuffer(byte* write)
        {
            switch (Type)
            {
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                    *(uint*)write = (byte)Type;
                    *(ulong*)(write + sizeof(uint)) = Int64.ULong;
                    return;
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                    *(uint*)write = (byte)Type;
                    *(uint*)(write + sizeof(uint)) = Int64.UInt;
                    return;
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                    *(uint*)write = (byte)Type + ((uint)Int64.UShort << 16);
                    return;
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    *(uint*)write = (byte)Type + ((uint)Int64.Byte << 16);
                    return;
                case DataType.Decimal:
                    *(uint*)write = (byte)Type;
                    *(decimal*)(write + sizeof(uint)) = new UnionType { Value = Value }.Decimal.Value;
                    return;
                case DataType.Guid:
                    *(uint*)write = (byte)Type;
                    *(System.Guid*)(write + sizeof(uint)) = new UnionType { Value = Value }.Guid.Value;
                    return;
                case DataType.String:
                case DataType.ByteArray:
                case DataType.BinarySerialize:
                case DataType.Json:
                    if (IsBuffer)
                    {
                        *(uint*)write = (byte)Type;
                        *(int*)(write + sizeof(uint)) = Int64.Length;
                    }
                    else *(uint*)write = (byte)Type + nullValue;
                    return;
            }
            *(uint*)write = (byte)DataType.Null;
        }
        /// <summary>
        /// 复制缓冲区数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        internal void SerializeBuffer(ref SubBuffer.PoolBufferFull buffer, int index)
        {
            switch (Type)
            {
                case DataType.ByteArray:
                case DataType.String:
                case DataType.BinarySerialize:
                case DataType.Json:
                    System.Buffer.BlockCopy(new UnionType { Value = Value }.ByteArray, Int64.Index, buffer.Buffer, buffer.StartIndex + index + sizeof(int) * 2, Int64.Length);
                    return;
            }
        }
        /// <summary>
        /// 复制缓冲区数据
        /// </summary>
        /// <param name="write"></param>
        /// <param name="bigBuffer"></param>
        internal void SerializeBuffer(byte* write, byte[] bigBuffer)
        {
            SerializeBuffer(write);
            switch (Type)
            {
                case DataType.ByteArray:
                case DataType.String:
                case DataType.BinarySerialize:
                case DataType.Json:
                    System.Buffer.BlockCopy(new UnionType { Value = Value }.ByteArray, Int64.Index, bigBuffer, Cache.MessageQueue.FileWriter.PacketHeaderSize + sizeof(int) * 2, Int64.Length);
                    return;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="read"></param>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal int DeSerializeBuffer(byte* read, byte[] buffer, int startIndex)
        {
            ReturnType = ReturnType.Success;
            uint type = *(uint*)read;
            switch (Type = (DataType)(byte)type)
            {
                case DataType.ULong:
                case DataType.Long:
                case DataType.Double:
                case DataType.DateTime:
                    Int64.ULong = *(ulong*)(read + sizeof(uint));
                    return sizeof(uint) + sizeof(ulong);
                case DataType.UInt:
                case DataType.Int:
                case DataType.Float:
                    Int64.UInt = *(uint*)(read + sizeof(uint));
                    return sizeof(uint) + sizeof(uint);
                case DataType.UShort:
                case DataType.Short:
                case DataType.Char:
                case DataType.Byte:
                case DataType.SByte:
                case DataType.Bool:
                    Int64.UInt = type >> 16;
                    return sizeof(uint);
                case DataType.Decimal:
                    Value = new Decimal(*(decimal*)(read + sizeof(uint)));
                    IsBuffer = false;
                    return sizeof(uint) + sizeof(decimal);
                case DataType.Guid:
                    Value = new Guid(*(System.Guid*)(read + sizeof(uint)));
                    IsBuffer = false;
                    return sizeof(uint) + sizeof(System.Guid);
                case DataType.ByteArray:
                    if ((type & nullValue) == 0)
                    {
                        Int64.Set(startIndex + (sizeof(uint) + sizeof(int)), *(int*)(read + sizeof(uint)));
                        Value = buffer;
                        IsBuffer = true;
                        return ((Int64.Length + (sizeof(uint) + sizeof(int) + 3)) & (int.MaxValue - 3));
                    }
                    Value = null;
                    IsBuffer = false;
                    return sizeof(uint);
                case DataType.String:
                case DataType.BinarySerialize:
                case DataType.Json:
                    if ((type & nullValue) == 0)
                    {
                        Int64.Set(startIndex + (sizeof(uint) + sizeof(int)), *(int*)(read + sizeof(uint)));
                        Value = buffer;
                        IsBuffer = true;
                        return sizeof(uint) + sizeof(int) + Int64.Length;
                    }
                    Value = null;
                    IsBuffer = false;
                    return sizeof(uint);
            }
            return sizeof(uint);
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
        /// <summary>
        /// 引用类型
        /// </summary>
        internal static readonly Type RefType = typeof(Data).MakeByRefType();
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
        /// 是否支持修改
        /// </summary>
        internal static bool IsUpdate
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
        /// 获取参数数据委托
        /// </summary>
        internal static readonly GetData<valueType> GetData;

        ///// <summary>
        ///// 设置参数数据
        ///// </summary>
        ///// <param name="parameter">参数数据</param>
        ///// <param name="value">数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        //private static void setJson(ref Data parameter, valueType value)
        //{
        //    parameter.SetJson(value);
        //}
        ///// <summary>
        ///// 设置参数数据
        ///// </summary>
        ///// <param name="parameter">参数数据</param>
        ///// <param name="value">数据</param>
        //private static void setBinary(ref Data parameter, valueType value)
        //{
        //    parameter.SetBinary(value);
        //}
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static DataStructure.Value.Json<valueType> getJson(ref Data parameter)
        {
            return new DataStructure.Value.Json<valueType>(ref parameter);
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static DataStructure.Value.Binary<valueType> getBinary(ref Data parameter)
        {
            return new DataStructure.Value.Binary<valueType>(ref parameter);
        }
        /// <summary>
        /// 检测数据类型
        /// </summary>
        internal static void CheckValueType()
        {
            if (DataType == ValueData.DataType.Null) throw new InvalidCastException("不支持数据类型 " + typeof(valueType).fullName() + "，请考虑序列化类型如 AutoCSer.CacheServer.DataStructure.Value.Json<valueType> 或者 AutoCSer.CacheServer.DataStructure.Value.Binary<valueType>");
        }
        /// <summary>
        /// 转换为数据结构定义节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DataStructure.Abstract.Node ToNode(DataStructure.Abstract.Node parent, valueType value, OperationParameter.OperationType operationType)
        {
            switch (DataType)
            {
                case DataType.Json:
                case DataType.BinarySerialize:
                    DataStructure.Abstract.Node valueNode = value as DataStructure.Abstract.Node;
                    if (valueNode.TrySetParent(parent) && valueNode.Parameter.TrySetOperationType(operationType)) return valueNode;
                    break;
            }
            DataStructure.Parameter.Value node = new DataStructure.Parameter.Value(parent);
            SetData(ref node.Parameter, value);
            node.Parameter.OperationType = operationType;
            return node;
        }
        /// <summary>
        /// 转换为数据结构定义节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DataStructure.Parameter.OperationBool GetOperationBool(DataStructure.Abstract.Node parent, valueType value, OperationParameter.OperationType operationType)
        {
            switch (DataType)
            {
                case DataType.Json:
                case DataType.BinarySerialize:
                    return new DataStructure.Parameter.OperationBool(parent, value as DataStructure.Abstract.Node, operationType);
                default:
                    DataStructure.Parameter.OperationBool node = new DataStructure.Parameter.OperationBool(parent);
                    SetData(ref node.Parameter, value);
                    node.Parameter.OperationType = operationType;
                    return node;
            }
        }
        /// <summary>
        /// 转换为短路径查询参数节点
        /// </summary>
        /// <param name="node">短路径查询节点</param>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ShortPath.Parameter.Value ToNode(ShortPath.Node node, valueType value, OperationParameter.OperationType operationType)
        {
            switch (DataType)
            {
                case DataType.Json:
                case DataType.BinarySerialize:
                    return new ShortPath.Parameter.Value(node, value as DataStructure.Abstract.Node, operationType);
                default:
                    ShortPath.Parameter.Value parameter = new ShortPath.Parameter.Value(node);
                    SetData(ref parameter.Parameter, value);
                    parameter.Parameter.OperationType = operationType;
                    return parameter;
            }
        }
        /// <summary>
        /// 转换为短路径查询参数节点
        /// </summary>
        /// <param name="node">短路径查询节点</param>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ShortPath.Parameter.OperationBool GetOperationBool(ShortPath.Parameter.Node node, valueType value, OperationParameter.OperationType operationType)
        {
            switch (DataType)
            {
                case DataType.Json:
                case DataType.BinarySerialize:
                    return new ShortPath.Parameter.OperationBool(node, value as DataStructure.Abstract.Node, operationType);
                default:
                    ShortPath.Parameter.OperationBool parameter = new ShortPath.Parameter.OperationBool(node);
                    SetData(ref parameter.Parameter, value);
                    parameter.Parameter.OperationType = operationType;
                    return parameter;
            }
        }
        
        static Data()
        {
            Type type = typeof(valueType);
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(DataStructure.Value.Json<>))
                {
                    DataType = DataType.Json;
                    //GetData = (GetData<valueType>)Delegate.CreateDelegate(typeof(GetData<valueType>), typeof(Data<>).MakeGenericType(type.GetGenericArguments()).GetMethod("getJson", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { Data.RefType }, null));
                    GetData = (GetData<valueType>)AutoCSer.CacheServer.Metadata.GenericType.Get(type.GetGenericArguments()[0]).ValueDataGetJsonDelegate;
                    return;
                }
                if (genericType == typeof(DataStructure.Value.Binary<>))
                {
                    DataType = DataType.BinarySerialize;
                    //GetData = (GetData<valueType>)Delegate.CreateDelegate(typeof(GetData<valueType>), typeof(Data<>).MakeGenericType(type.GetGenericArguments()).GetMethod("getBinary", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { Data.RefType }, null));
                    GetData = (GetData<valueType>)AutoCSer.CacheServer.Metadata.GenericType.Get(type.GetGenericArguments()[0]).ValueDataGetBinaryDelegate;
                    return;
                }
                return;
            }
            if (type == typeof(byte[]))
            {
                DataType = DataType.ByteArray;
                SetData = (SetData<valueType>)(object)(SetData<byte[]>)Data.Set;
                GetData = (GetData<valueType>)(object)(GetData<byte[]>)Data.GetByteArray;
                return;
            }
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
        }
    }
}
