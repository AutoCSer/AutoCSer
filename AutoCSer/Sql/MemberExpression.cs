using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 成员表达式
    /// </summary>
    /// <typeparam name="targetType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberExpression<targetType, valueType>
        where targetType : class
    {
        /// <summary>
        /// 字段成员
        /// </summary>
        public FieldInfo Field;
        /// <summary>
        /// 获取成员
        /// </summary>
        public Func<targetType, valueType> GetMember
        {
            get
            {
                return AutoCSer.Emit.Field.UnsafeGetField<targetType, valueType>(Field);
            }
        }
        /// <summary>
        /// 设置成员
        /// </summary>
        public Action<targetType, valueType> SetMember
        {
            get
            {
                return AutoCSer.Emit.Field.UnsafeSetField<targetType, valueType>(Field);
            }
        }
        /// <summary>
        /// 成员表达式
        /// </summary>
        /// <param name="member">成员表达式</param>
        public MemberExpression(Expression<Func<targetType, valueType>> member)
        {
            Expression expression = member.Body;
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                FieldInfo field = ((MemberExpression)expression).Member as FieldInfo;
                if (field != null && !field.IsStatic && field.DeclaringType.IsAssignableFrom(typeof(targetType)) && field.FieldType == typeof(valueType)) Field = field;
                else Field = null;
            }
            else Field = null;
        }
    }
}
