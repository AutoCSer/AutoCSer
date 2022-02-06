using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL 操作返回值
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReturnValue
    {
        /// <summary>
        /// 错误异常
        /// </summary>
        public Exception Exception;
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ReturnType ReturnType;

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="error">错误异常</param>
        public static implicit operator ReturnValue(Exception error) { return new ReturnValue { Exception = error, ReturnType = ReturnType.Exception }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="errorType">错误类型</param>
        public static implicit operator ReturnValue(ReturnType errorType) { return new ReturnValue { ReturnType = errorType }; }
    }
    /// <summary>
    /// SQL 操作返回值
    /// </summary>
    /// <typeparam name="valueType">返回值类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReturnValue<valueType>
    {
        /// <summary>
        /// 错误异常
        /// </summary>
        public Exception Exception;
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ReturnType ReturnType;
        /// <summary>
        /// 返回值
        /// </summary>
        public valueType Value;

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="error">错误异常</param>
        public static implicit operator ReturnValue<valueType>(Exception error) { return new ReturnValue<valueType> { Exception = error, ReturnType = ReturnType.Exception }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="errorType">错误类型</param>
        public static implicit operator ReturnValue<valueType>(ReturnType errorType) { return new ReturnValue<valueType> { ReturnType = errorType }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">返回值</param>
        public static implicit operator ReturnValue<valueType>(valueType value) { return new ReturnValue<valueType> { Value = value, ReturnType = ReturnType.Success }; }
    }
}
