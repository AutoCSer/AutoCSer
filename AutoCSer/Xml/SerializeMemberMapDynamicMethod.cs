using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Xml
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct SerializeMemberMapDynamicMethod
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
            dynamicMethod = new DynamicMethod("XmlMemberMapSerializer", null, new Type[] { typeof(MemberMap), typeof(Serializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(CharStream));

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldfld, SerializeMemberDynamicMethod.CharStreamField);
            generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="attribute">XML序列化成员配置</param>
        public void Push(FieldIndex field, MemberAttribute attribute)
        {
            Label end = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_0, field.MemberIndex);
            generator.Emit(OpCodes.Brfalse_S, end);

            MethodInfo isOutputMethod = SerializeMethodCache.GetIsOutputMethod(field.Member.FieldType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.Member);
                generator.call(isOutputMethod);
                generator.Emit(OpCodes.Brfalse_S, end);
            }
            string name = field.AnonymousName;
            generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNameStartPool(name), name.Length + 2);

            if (attribute != null && attribute.ItemName != null)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldstr, attribute.ItemName);
                generator.Emit(OpCodes.Stfld, SerializeMemberDynamicMethod.ItemNameField);
            }
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

            generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNameEndPool(name), name.Length + 3);

            generator.MarkLabel(end);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        /// <param name="attribute">XML序列化成员配置</param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod, MemberAttribute attribute)
        {
            Label end = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_0, property.MemberIndex);
            generator.Emit(OpCodes.Brfalse_S, end);

            MethodInfo isOutputMethod = SerializeMethodCache.GetIsOutputMethod(property.Member.PropertyType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.call(propertyMethod);
                generator.call(isOutputMethod);
                generator.Emit(OpCodes.Brfalse_S, end);
            }
            generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNameStartPool(property.Member.Name), property.Member.Name.Length + 2);

            if (attribute != null && attribute.ItemName != null)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldstr, attribute.ItemName);
                generator.Emit(OpCodes.Stfld, SerializeMemberDynamicMethod.ItemNameField);
            }
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

            generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNameEndPool(property.Member.Name), property.Member.Name.Length + 3);

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
