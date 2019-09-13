using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberMapDynamicMethod
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
        public SerializeMemberMapDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("JsonMemberMapSerializer", null, new Type[] { typeof(MemberMap), typeof(Serializer), type, typeof(CharStream) }, type, true);
            generator = dynamicMethod.GetILGenerator();

            generator.DeclareLocal(typeof(int));
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <param name="memberIndex"></param>
        /// <param name="end"></param>
        private unsafe void push(string name, int memberIndex, Label end)
        {
            Label next = generator.DefineLabel(), value = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_0, memberIndex);
            generator.Emit(OpCodes.Brfalse, end);

            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(name.Length > 40 ? OpCodes.Brtrue : OpCodes.Brtrue_S, next);

            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Stloc_0);
            SerializeMemberDynamicMethod.WriteName(generator, OpCodes.Ldarg_3, name, false);
            generator.Emit(OpCodes.Br_S, value);

            generator.MarkLabel(next);
            SerializeMemberDynamicMethod.WriteName(generator, OpCodes.Ldarg_3, name, true);

            generator.MarkLabel(value);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldIndex field)
        {
            Label end = generator.DefineLabel();
            push(field.AnonymousName, field.MemberIndex, end);
            bool isCustom = false;
            MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(field.Member.FieldType, ref isCustom);
            if (isCustom)
            {
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldflda, field.Member);
                generator.Emit(OpCodes.Ldarg_1);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.Member);
            }
            generator.call(method);
            generator.MarkLabel(end);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod)
        {
            Label end = generator.DefineLabel();
            push(property.Member.Name, property.MemberIndex, end);
            bool isCustom = false;
            MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(property.Member.PropertyType, ref isCustom);
            if (isCustom)
            {
                LocalBuilder loadMember = generator.DeclareLocal(property.Member.PropertyType);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.call(propertyMethod);
                generator.Emit(OpCodes.Stloc_0);
                generator.Emit(OpCodes.Ldloca_S, loadMember); 
                generator.Emit(OpCodes.Ldarg_1);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.call(propertyMethod);
            }
            generator.call(method);
            generator.MarkLabel(end);
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
