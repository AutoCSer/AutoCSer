using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnaryExpression
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 表示包含一元运算符的表达式
        /// </summary>
        [FieldOffset(0)]
        public System.Linq.Expressions.UnaryExpression Value;
    }
}
