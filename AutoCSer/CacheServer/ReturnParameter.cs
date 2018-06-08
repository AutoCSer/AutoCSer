using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 返回值参数
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    public struct ReturnParameter
    {
        /// <summary>
        /// 返回值参数
        /// </summary>
        internal ValueData.Data Parameter;
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal ReturnType Type;
        /// <summary>
        /// 是否反序列化网络流，否则需要 Copy 数据
        /// </summary>
        internal bool IsDeSerializeStream;
        /// <summary>
        /// ulong
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<ulong> GetULong(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.ULong;
            return new ReturnValue<ulong> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// long
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<long> GetLong(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Long;
            return new ReturnValue<long> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// uint
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<uint> GetUInt(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.UInt;
            return new ReturnValue<uint> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// int
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<int> GetInt(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Int;
            return new ReturnValue<int> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// ushort
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<ushort> GetUShort(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.UShort;
            return new ReturnValue<ushort> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// short
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<short> GetShort(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Short;
            return new ReturnValue<short> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// byte
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<byte> GetByte(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Byte;
            return new ReturnValue<byte> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// sbyte
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<sbyte> GetSByte(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.SByte;
            return new ReturnValue<sbyte> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// char
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<char> GetChar(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Char;
            return new ReturnValue<char> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// bool
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<bool> GetBool(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Bool;
            return new ReturnValue<bool> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
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
            if (Type == ReturnType.Success) return Parameter.Int64.Bool ? returnValue : null;
            return new ReturnValue<valueType> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// float
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<float> GetFloat(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Float;
            return new ReturnValue<float> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// double
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<double> GetDouble(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.Double;
            return new ReturnValue<double> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// 时间数据
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<DateTime> GetDateTime(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Int64.DateTime;
            return new ReturnValue<DateTime> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// Guid
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<Guid> GetGuid(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Guid;
            return new ReturnValue<Guid> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// decimal
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<decimal> GetDecimal(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.Decimal;
            return new ReturnValue<decimal> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// 字节数组
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<byte[]> GetByteArray(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.ByteArray;
            return new ReturnValue<byte[]> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<string> GetString(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success) return Parameter.String;
            return new ReturnValue<string> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }
        ///// <summary>
        ///// 获取数据
        ///// </summary>
        ///// <param name="getValue"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal ReturnValue<valueType> Get<valueType>(ValueData.GetData<valueType> getValue)
        //{
        //    if (Type == ReturnType.Success) return getValue(ref Parameter);
        //    return new ReturnValue<valueType> { Type = Parameter.TcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = Parameter.TcpReturnType };
        //}
        ///// <summary>
        ///// 获取 JSON 反序列化数据
        ///// </summary>
        ///// <returns></returns>
        //internal unsafe ReturnValue<valueType> GetJson<valueType>()
        //{
        //    if (Type == ReturnType.Success)
        //    {
        //        valueType value = default(valueType);
        //        if (Parameter.GetJson(ref value)) return value;
        //        return new ReturnValue<valueType> { Type = ReturnType.DeSerializeError, TcpReturnType = Parameter.TcpReturnType };
        //    }
        //    return new ReturnValue<valueType> { Type = Parameter.TcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = Parameter.TcpReturnType };
        //}
        /// <summary>
        /// 获取二进制反序列化数据
        /// </summary>
        /// <param name="tcpReturnType"></param>
        /// <returns></returns>
        internal ReturnValue<valueType> GetBinary<valueType>(AutoCSer.Net.TcpServer.ReturnType tcpReturnType)
        {
            if (Type == ReturnType.Success)
            {
                valueType value = default(valueType);
                if (Parameter.GetBinary(ref value)) return value;
                return new ReturnValue<valueType> { Type = ReturnType.DeSerializeError };
            }
            return new ReturnValue<valueType> { Type = tcpReturnType == Net.TcpServer.ReturnType.Success ? Type : ReturnType.TcpError, TcpReturnType = tcpReturnType };
        }

        ///// <summary>
        ///// 设置返回值
        ///// </summary>
        ///// <param name="parameter">返回值</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void SetIsReturn(ref ValueData.Data parameter)
        //{
        //    if (parameter.IsSetReturn) Parameter = parameter;
        //    Type = ReturnType.Success;
        //}
        ///// <summary>
        ///// 设置返回值
        ///// </summary>
        ///// <param name="parameter">返回值</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void Set(ref ValueData.Data parameter)
        //{
        //    Parameter = parameter;
        //    Type = ReturnType.Success;
        //}
        ///// <summary>
        ///// 设置默认返回值
        ///// </summary>
        ///// <param name="parameterType">数据类型</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void SetDefault(ValueData.Data parameterType)
        //{
        //    Parameter.SetDefault(parameterType);
        //    Type = ReturnType.Success;
        //}
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ulong parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(long parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(uint parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ushort parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(short parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(sbyte parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="parameter">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(bool parameter)
        {
            Parameter.Set(parameter);
            Type = ReturnType.Success;
        }
        ///// <summary>
        ///// 设置返回值
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="startIndex"></param>
        ///// <param name="length"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void SetByteArray(object value, int startIndex, int length)
        //{
        //    Type = ReturnType.Success;
        //    Parameter.SetByteArray(value, startIndex, length);
        //}
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="value">返回值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetBinary<valueType>(valueType value)
        {
            Parameter.SetBinary(value);
            Type = ReturnType.Success;
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
            stream.Write(IsDeSerializeStream ? (uint)(byte)Type + 0x100U : (uint)(byte)Type);
            if (Type == ReturnType.Success) Parameter.Serialize(stream);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            uint type = (uint)deSerializer.ReadInt();
            Type = (ReturnType)(byte)type;
            if (Type == ReturnType.Success)
            {
                if ((IsDeSerializeStream = (type & 0x100U) != 0) ? Parameter.DeSerializeSynchronous(deSerializer) : Parameter.DeSerialize(deSerializer)) return;
                Type = ReturnType.DeSerializeError;
            }
        }

        /// <summary>
        /// 获取回调委托包装
        /// </summary>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        public static Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> GetCallback(Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn != null) return value => onReturn(value.Value.GetBool(value.Type));
            return null;
        }
        /// <summary>
        /// 获取回调委托包装
        /// </summary>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        public static Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> GetCallback<valueType>(Action<ReturnValue<valueType>> onReturn)
        {
            if (onReturn != null) return value => onReturn(new ReturnValue<valueType>(ref value));
            return null;
        }
    }
}
