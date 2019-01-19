using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 异步返回值
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReturnValue
    {
        /// <summary>
        /// 返回值参数名称
        /// </summary>
        internal const string RetParameterName = "Ret";
        /// <summary>
        /// 返回值参数名称
        /// </summary>
        internal const string ReturnParameterName = "Return";
        /// <summary>
        /// 返回值类型
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public ReturnType Type;
        /// <summary>
        /// 是否存在返回值
        /// </summary>
        public bool IsReturn
        {
            get { return Type == ReturnType.Success; }
        }
        /// <summary>
        /// 是否存在返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(ReturnValue value)
        {
            return value.IsReturn;
        }
        /// <summary>
        /// 异步返回值
        /// </summary>
        /// <param name="type">返回值类型</param>
        /// <returns></returns>
        public static implicit operator ReturnValue(ReturnType type)
        {
            return new ReturnValue { Type = type };
        }
    }
    /// <summary>
    /// 异步返回值
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReturnValue<returnType>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public returnType Value;
        /// <summary>
        /// 返回值类型
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public ReturnType Type;
        /// <summary>
        /// 是否存在返回值
        /// </summary>
        public bool IsReturn
        {
            get { return Type == ReturnType.Success; }
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value">异步返回值</param>
        /// <returns>返回值</returns>
        public static implicit operator ReturnValue<returnType>(returnType value)
        {
            return new ReturnValue<returnType> { Type = ReturnType.Success, Value = value };
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value">返回值</param>
        /// <returns>异步返回值</returns>
        public static implicit operator returnType(ReturnValue<returnType> value)
        {
            if (value.Type == ReturnType.Success) return value.Value;
            throw new InvalidCastException(value.Type.ToString());
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public void Null()
        {
            Type = ReturnType.Unknown;
            Value = default(returnType);
        }
    }
}
