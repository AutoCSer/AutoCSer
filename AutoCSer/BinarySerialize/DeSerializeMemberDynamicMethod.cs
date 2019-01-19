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
    internal struct DeSerializeMemberDynamicMethod
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
        public DeSerializeMemberDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("FieldDeSerializer", null, new Type[] { typeof(DeSerializer), type.MakeByRefType() }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldSize field)
        {
            Type fieldType = field.Field.FieldType;
            generator.Emit(OpCodes.Ldarg_0);
            if (field.Field.IsStatic)
            {
                LocalBuilder staticMember = generator.DeclareLocal(fieldType);
                generator.initobj(fieldType, staticMember);
                generator.Emit(OpCodes.Ldloca, staticMember);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Ldflda, field.Field);
            }
            generator.call(DeSerializer.GetMemberDeSerializeMethod(fieldType) ?? DeSerializeMethodCache.GetMember(fieldType));
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
