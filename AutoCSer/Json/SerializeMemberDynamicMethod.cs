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
    /// 序列化动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberDynamicMethod
    {
        /// <summary>
        /// 获取字符串输出缓冲区属性方法信息
        /// </summary>
        private static readonly FieldInfo charStreamField = typeof(Serializer).GetField("CharStream", BindingFlags.Instance | BindingFlags.Public);
        /// <summary>
        /// 动态函数
        /// </summary>
        private DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private ILGenerator generator;
        /// <summary>
        /// 是否第一个字段
        /// </summary>
        private byte isFirstMember;
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
            dynamicMethod = new DynamicMethod("JsonSerializer", null, new Type[] { typeof(Serializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(CharStream));

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, charStreamField);
            generator.Emit(OpCodes.Stloc_0);

            isFirstMember = 1;
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name">成员名称</param>
        private unsafe void push(string name)
        {
            if (isFirstMember == 0) generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNamePool(name), name.Length + 4);
            else
            {
                generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNamePool(name) + 1, name.Length + 3);
                isFirstMember = 0;
            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldIndex field)
        {
            push(field.AnonymousName);
            bool isCustom = false;
            MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(field.Member.FieldType, ref isCustom);
            if (isCustom)
            {
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldflda, field.Member);
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.Member);
            }
            generator.call(method);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod)
        {
            push(property.Member.Name);
            bool isCustom = false;
            MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(property.Member.PropertyType, ref isCustom);
            if (isCustom)
            {
                LocalBuilder loadMember = generator.DeclareLocal(property.Member.PropertyType);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
                generator.Emit(OpCodes.Stloc_0);
                generator.Emit(OpCodes.Ldloca_S, loadMember); 
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
            }
            generator.call(method);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void PushBox(FieldIndex field)
        {
            bool isCustom = false;
            MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(field.Member.FieldType, ref isCustom);
            if (isCustom)
            {
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldflda, field.Member);
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.Member);
            }
            generator.call(method);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        public void PushBox(PropertyIndex property, MethodInfo propertyMethod)
        {
            bool isCustom = false;
            MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(property.Member.PropertyType, ref isCustom);
            if (isCustom)
            {
                LocalBuilder loadMember = generator.DeclareLocal(property.Member.PropertyType);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
                generator.Emit(OpCodes.Stloc_0);
                generator.Emit(OpCodes.Ldloca_S, loadMember);
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
            }
            generator.call(method);
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
