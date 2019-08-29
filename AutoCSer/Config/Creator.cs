using System;
using System.Reflection;

namespace AutoCSer.Config
{
    /// <summary>
    /// 配置创建
    /// </summary>
    internal abstract class Creator
    {
        /// <summary>
        /// 实列对象
        /// </summary>
        protected readonly object instance;
        /// <summary>
        /// 是否主配置（不可被覆盖）
        /// </summary>
        internal readonly bool IsMain;
        /// <summary>
        /// 配置创建
        /// </summary>
        /// <param name="instance">实列对象</param>
        /// <param name="isMain">是否主配置（不可被覆盖）</param>
        protected Creator(object instance, bool isMain)
        {
            this.instance = instance;
            IsMain = isMain;
        }
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
            /// <param name="instance">实列对象</param>
            /// <param name="isMain">是否主配置（不可被覆盖）</param>
            /// <param name="field">目标字段</param>
            internal Field(object instance, bool isMain, FieldInfo field) :base(instance, isMain)
            {
                this.field = field;
            }
            /// <summary>
            /// 创建配置对象
            /// </summary>
            /// <returns></returns>
            internal override object Create()
            {
                return field.GetValue(instance);
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
            /// 字段配置创建
            /// </summary>
            /// <param name="instance">实列对象</param>
            /// <param name="isMain">是否主配置（不可被覆盖）</param>
            /// <param name="method">目标属性</param>
            internal Property(object instance, bool isMain, MethodInfo method) : base(instance, isMain)
            {
                this.method = method;
            }
            /// <summary>
            /// 创建配置对象
            /// </summary>
            /// <returns></returns>
            internal override object Create()
            {
                return method.Invoke(instance, null);
            }
        }
    }
}
