using System;
using System.Reflection;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 序列化字段信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeField
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        internal FieldInfo Field;
        /// <summary>
        /// 序列化函数信息
        /// </summary>
        internal Action<Serializer, object> SerializeMethod;
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
            SerializeMethod = (Action<Serializer, object>)typeof(AutoCSer.Reflection.InvokeMethod<,>).MakeGenericType(typeof(Serializer), Field.FieldType).GetMethod("getTypeObject", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { Serializer.GetMemberMapSerializeMethod(Field.FieldType) ?? SerializeMethodCache.GetMember(Field.FieldType) });
        }
    }
}
