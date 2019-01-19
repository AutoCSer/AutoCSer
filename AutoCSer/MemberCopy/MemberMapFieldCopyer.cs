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
        private sealed class MemberMapFieldCopyer
        {
            /// <summary>
            /// 字段集合
            /// </summary>
            private KeyValue<int, FieldInfo>[] fields;
            /// <summary>
            /// 字段复制
            /// </summary>
            /// <param name="fields"></param>
            public MemberMapFieldCopyer(FieldIndex[] fields)
            {
                this.fields = new KeyValue<int, FieldInfo>[fields.Length];
                int index = 0;
                foreach (FieldIndex field in fields) this.fields[index++].Set(field.MemberIndex, field.Member);
            }
            /// <summary>
            /// 字段复制委托
            /// </summary>
            /// <returns></returns>
            public memberMapCopyer Copyer()
            {
                return typeof(valueType).IsValueType ? (memberMapCopyer)copyValue : copy;
            }
            /// <summary>
            /// 字段复制
            /// </summary>
            /// <param name="value"></param>
            /// <param name="copyValue"></param>
            /// <param name="memberMap"></param>
            private void copy(ref valueType value, valueType copyValue, MemberMap memberMap)
            {
                foreach (KeyValue<int, FieldInfo> field in fields)
                {
                    if (memberMap.IsMember(field.Key)) field.Value.SetValue(value, field.Value.GetValue(copyValue));
                }
            }
            /// <summary>
            /// 字段复制
            /// </summary>
            /// <param name="value"></param>
            /// <param name="copyValue"></param>
            /// <param name="memberMap"></param>
            private void copyValue(ref valueType value, valueType copyValue, MemberMap memberMap)
            {
                object objectValue = value, copyObject = copyValue;
                foreach (KeyValue<int, FieldInfo> field in fields)
                {
                    if (memberMap.IsMember(field.Key)) field.Value.SetValue(objectValue, field.Value.GetValue(copyObject));
                }
                value = (valueType)objectValue;
            }
        }
    }
}
