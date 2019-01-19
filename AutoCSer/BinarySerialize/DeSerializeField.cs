using System;
using System.Reflection;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 反序列化字段信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeSerializeField
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        internal FieldInfo Field;
        /// <summary>
        /// 序列化函数信息
        /// </summary>
        internal Func<DeSerializer, object, object> DeSerializeMethod;
        /// <summary>
        /// 成员编号
        /// </summary>
        internal int MemberIndex;
        /// <summary>
        /// 设置字段信息
        /// </summary>
        /// <param name="field"></param>
        internal void Set(FieldSize field)
        {
            Field = field.Field;
            MemberIndex = field.MemberIndex;
            DeSerializeMethod = (Func<DeSerializer, object, object>)typeof(AutoCSer.Reflection.InvokeMethodRef2<,>).MakeGenericType(typeof(DeSerializer), Field.FieldType).GetMethod("getTypeObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { DeSerializer.GetMemberDeSerializeMethod(Field.FieldType) ?? DeSerializeMethodCache.GetMember(Field.FieldType) });
        }
    }
}
