using System;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberDynamicMethod
    {
        /// <summary>
        /// 动态函数
        /// </summary>
        private DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private ILGenerator generator;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public SerializeMemberDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("BinarySerializer", null, new Type[] { typeof(Serializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldSize field)
        {
            generator.Emit(OpCodes.Ldarg_0);
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
            else generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldfld, field.Field);
            generator.call(Serializer.GetMemberSerializeMethod(field.Field.FieldType) ??  SerializeMethodCache.GetMember(field.Field.FieldType));
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <returns>成员转换委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
