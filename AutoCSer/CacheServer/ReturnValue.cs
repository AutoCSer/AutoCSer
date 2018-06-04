using System;
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
            Type = value.Value.Type;
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
            Type = value.Value.Type;
            TcpReturnType = value.Type;
            if (Type == ReturnType.Success) Value = ValueData.Data<returnType>.GetData(ref value.Value.Parameter);
            else Value = default(returnType);
        }
    }
}
