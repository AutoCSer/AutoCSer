using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ConstantExpression
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 表示具有常量值的表达式
        /// </summary>
        [FieldOffset(0)]
        public System.Linq.Expressions.ConstantExpression Value;
    }
}
