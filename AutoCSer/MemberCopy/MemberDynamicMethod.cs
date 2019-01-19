using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.MemberCopy
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberDynamicMethod
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
        /// <param name="dynamicMethod"></param>
        public MemberDynamicMethod(Type type, DynamicMethod dynamicMethod)
        {
            this.dynamicMethod = dynamicMethod;
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldIndex field)
        {
            generator.Emit(OpCodes.Ldarg_0);
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
            else
            {
                generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Ldarg_1);
            }
            generator.Emit(OpCodes.Ldfld, field.Member);
            generator.Emit(OpCodes.Stfld, field.Member);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void PushMemberMap(FieldIndex field)
        {
            Label isMember = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_2, field.MemberIndex);
            generator.Emit(OpCodes.Brfalse_S, isMember);
            Push(field);
            generator.MarkLabel(isMember);
        }
        /// <summary>
        /// 创建成员复制委托
        /// </summary>
        /// <returns>成员复制委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
