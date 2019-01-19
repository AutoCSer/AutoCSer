using System;
using AutoCSer.Extension;
using AutoCSer.Metadata;
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
    internal unsafe struct SerializeMemberDynamicMethod
    {
        /// <summary>
        /// 获取字符串输出缓冲区属性方法信息
        /// </summary>
        internal static readonly FieldInfo CharStreamField = typeof(Serializer).GetField("CharStream", BindingFlags.Instance | BindingFlags.Public);
        /// <summary>
        /// 集合子节点名称字段
        /// </summary>
        internal static readonly FieldInfo ItemNameField = typeof(Serializer).GetField("itemName", BindingFlags.Instance | BindingFlags.NonPublic);
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
            dynamicMethod = new DynamicMethod("XmlSerializer", null, new Type[] { typeof(Serializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(CharStream));

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, CharStreamField);
            generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <param name="attribute">XML序列化成员配置</param>
        private void nameStart(string name, MemberAttribute attribute)
        {
            generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNameStartPool(name), name.Length + 2);

            if (attribute != null && attribute.ItemName != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldstr, attribute.ItemName);
                generator.Emit(OpCodes.Stfld, ItemNameField);
            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="attribute">XML序列化成员配置</param>
        public void Push(FieldIndex field, MemberAttribute attribute)
        {
            Label end = default(Label);
            MethodInfo isOutputMethod = SerializeMethodCache.GetIsOutputMethod(field.Member.FieldType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.Member);
                generator.call(isOutputMethod);
                generator.Emit(OpCodes.Brfalse_S, end = generator.DefineLabel());
            }

            string name = field.AnonymousName;
            nameStart(name, attribute);
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

            generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNameEndPool(name), name.Length + 3);

            if (isOutputMethod != null) generator.MarkLabel(end);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        /// <param name="attribute">XML序列化成员配置</param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod, MemberAttribute attribute)
        {
            Label end = default(Label);
            MethodInfo isOutputMethod = SerializeMethodCache.GetIsOutputMethod(property.Member.PropertyType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
                generator.call(isOutputMethod);
                generator.Emit(OpCodes.Brfalse_S, end = generator.DefineLabel());
            }

            nameStart(property.Member.Name, attribute);
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

            generator.charStreamSimpleWriteNotNull(OpCodes.Ldloc_0, SerializeMethodCache.GetNameEndPool(property.Member.Name), property.Member.Name.Length + 3);

            if (isOutputMethod != null) generator.MarkLabel(end);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void PushBox(FieldIndex field)
        {
            Label end = default(Label);
            MethodInfo isOutputMethod = SerializeMethodCache.GetIsOutputMethod(field.Member.FieldType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.Member);
                generator.call(isOutputMethod);
                generator.Emit(OpCodes.Brfalse_S, end = generator.DefineLabel());
            }
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

            if (isOutputMethod != null) generator.MarkLabel(end);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        public void PushBox(PropertyIndex property, MethodInfo propertyMethod)
        {
            Label end = default(Label);
            MethodInfo isOutputMethod = SerializeMethodCache.GetIsOutputMethod(property.Member.PropertyType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
                generator.call(isOutputMethod);
                generator.Emit(OpCodes.Brfalse_S, end = generator.DefineLabel());
            }

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

            if (isOutputMethod != null) generator.MarkLabel(end);
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
