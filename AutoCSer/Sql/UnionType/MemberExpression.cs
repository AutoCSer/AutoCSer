using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct MemberExpression
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 表示访问字段或属性
        /// </summary>
        [FieldOffset(0)]
        public System.Linq.Expressions.MemberExpression Value;
    }
}
