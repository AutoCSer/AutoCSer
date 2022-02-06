using System;
using System.Reflection;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 配置创建
    /// </summary>
    internal abstract partial class Creator
    {
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
        internal abstract object Create();

        /// <summary>
        /// 字段配置创建
        /// </summary>
        internal sealed class Field : Creator
        {
            /// <summary>
            /// 目标字段
            /// </summary>
            private readonly FieldInfo field;
            /// <summary>
            /// 字段配置创建
            /// </summary>
            /// <param name="field">目标字段</param>
            internal Field(FieldInfo field)
            {
                this.field = field;
            }
            /// <summary>
            /// 创建配置对象
            /// </summary>
            /// <returns></returns>
            internal override object Create()
            {
                return field.GetValue(null);
            }
        }
        /// <summary>
        /// 属性配置创建
        /// </summary>
        internal sealed class Property : Creator
        {
            /// <summary>
            /// 目标属性
            /// </summary>
            private readonly MethodInfo method;
            /// <summary>
            /// 属性配置创建
            /// </summary>
            /// <param name="method">目标属性</param>
            internal Property(MethodInfo method)
            {
                this.method = method;
            }
            /// <summary>
            /// 创建配置对象
            /// </summary>
            /// <returns></returns>
            internal override object Create()
            {
                return method.Invoke(null, null);
            }
        }
    }
}
