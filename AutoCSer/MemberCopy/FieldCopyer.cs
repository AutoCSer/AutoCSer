using System;
using System.Reflection;
using AutoCSer.Metadata;

namespace AutoCSer.MemberCopy
{
    /// <summary>
    /// 成员复制
    /// </summary>
    public static partial class Copyer<valueType>
    {
        /// <summary>
        /// 字段复制（反射模式）
        /// </summary>
        private sealed class FieldCopyer
        {
            /// <summary>
            /// 字段集合
            /// </summary>
            private FieldInfo[] fields;
            /// <summary>
            /// 字段复制
            /// </summary>
            /// <param name="fields"></param>
            public FieldCopyer(FieldIndex[] fields)
            {
                this.fields = new FieldInfo[fields.Length];
                int index = 0;
                foreach (FieldIndex field in fields) this.fields[index++] = field.Member;
            }
            /// <summary>
            /// 字段复制委托
            /// </summary>
            /// <returns></returns>
            public copyer Copyer()
            {
                return typeof(valueType).IsValueType ? (copyer)copyValue : copy;
            }
            /// <summary>
            /// 字段复制
            /// </summary>
            /// <param name="value"></param>
            /// <param name="copyValue"></param>
            private void copy(ref valueType value, valueType copyValue)
            {
                foreach (FieldInfo field in fields) field.SetValue(value, field.GetValue(copyValue));
            }
            /// <summary>
            /// 字段复制
            /// </summary>
            /// <param name="value"></param>
            /// <param name="copyValue"></param>
            private void copyValue(ref valueType value, valueType copyValue)
            {
                object objectValue = value, copyObject = copyValue;
                foreach (FieldInfo field in fields) field.SetValue(objectValue, field.GetValue(copyObject));
                value = (valueType)objectValue;
            }
        }
    }
}
