using System;

namespace AutoCSer.Log
{
    /// <summary>
    /// 异常类型
    /// </summary>
    internal enum ErrorType : byte
    {
        /// <summary>
        /// 没有异常(不输出)
        /// </summary>
        None,
        ///// <summary>
        ///// AutoCSer 项目编译未启用代码生成
        ///// </summary>
        //NotAutoCSerCode,
        /// <summary>
        /// 关键值为空
        /// </summary>
        Null,
        /// <summary>
        /// 索引超出范围
        /// </summary>
        IndexOutOfRange,
        /// <summary>
        /// 操作不可用
        /// </summary>
        ErrorOperation,
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown
    }
}
