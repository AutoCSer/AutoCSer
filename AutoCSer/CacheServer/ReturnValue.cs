using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 返回值
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct ReturnValue<returnType>
    {
        /// <summary>
        /// TCP 操作返回值类型
        /// </summary>
        public AutoCSer.Net.TcpServer.ReturnType TcpReturnType;
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ReturnType Type;
        /// <summary>
        /// 返回值
        /// </summary>
        public returnType Value;
        /// <summary>
        /// 返回值隐式转换
        /// </summary>
        /// <param name="value">返回值</param>
        public static implicit operator ReturnValue<returnType>(returnType value)
        {
            return new ReturnValue<returnType> { Type = ReturnType.Success, TcpReturnType = AutoCSer.Net.TcpServer.ReturnType.Success, Value = value };
        }
        /// <summary>
        /// 隐式转换返回值
        /// </summary>
        /// <param name="value">返回值</param>
        public static implicit operator returnType(ReturnValue<returnType> value)
        {
            if (value.Type == ReturnType.Success) return value.Value;
            throw new InvalidCastException(value.Type.ToString());
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="value">返回值</param>
        internal ReturnValue(ref AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            Type = value.Value.Parameter.ReturnType;
            TcpReturnType = value.Type;
            if (Type == ReturnType.Success) Value = ValueData.Data<returnType>.GetData(ref value.Value.Parameter);
            else Value = default(returnType);
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="value">返回值</param>
        internal ReturnValue(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            Type = value.Value.Parameter.ReturnType;
            TcpReturnType = value.Type;
            if (Type == ReturnType.Success) Value = ValueData.Data<returnType>.GetData(ref value.Value.Parameter);
            else Value = default(returnType);
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="value">返回值</param>
        private ReturnValue(ref AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter> value)
        {
            Type = value.Value.Parameter.ReturnType;
            TcpReturnType = value.Type;
            Value = default(returnType);
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<KeyValue<ulong, returnType>> Get(ref AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter> value)
        {
            if (value.Value.Parameter.ReturnType == ReturnType.Success) return new KeyValue<ulong, returnType>(value.Value.Identity, ValueData.Data<returnType>.GetData(ref value.Value.Parameter));
            return new ReturnValue<KeyValue<ulong, returnType>>(ref value);
        }
    }
}
