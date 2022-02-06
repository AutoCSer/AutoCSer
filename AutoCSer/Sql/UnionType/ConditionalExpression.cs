using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ConditionalExpression
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 表示包含条件运算符的表达式
        /// </summary>
        [FieldOffset(0)]
        public System.Linq.Expressions.ConditionalExpression Value;
    }
}
