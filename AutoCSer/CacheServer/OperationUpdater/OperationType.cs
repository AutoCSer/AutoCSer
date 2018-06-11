using System;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 更新类型
    /// </summary>
    internal enum OperationType : ushort
    {
        /// <summary>
        /// 设置数据
        /// </summary>
        Set,
        /// <summary>
        /// 加法运算
        /// </summary>
        Add,
        /// <summary>
        /// 减法运算
        /// </summary>
        Sub,
        /// <summary>
        /// 乘法运算
        /// </summary>
        Mul,
        /// <summary>
        /// 除法运算
        /// </summary>
        Div,
        /// <summary>
        /// 取模运算
        /// </summary>
        Mod,
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        Xor,
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        And,
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        Or,
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        Not,
    }
}
