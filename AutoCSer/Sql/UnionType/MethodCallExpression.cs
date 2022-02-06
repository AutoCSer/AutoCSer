using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct MethodCallExpression
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 表示对静态方法或实例方法的调用
        /// </summary>
        [FieldOffset(0)]
        public System.Linq.Expressions.MethodCallExpression Value;
    }
}
