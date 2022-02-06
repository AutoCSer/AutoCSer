using System;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System.Reflection;
using AutoCSer.Emit;
using AutoCSer.Memory;
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
        internal static readonly MethodInfo GetCharStreamMethod = ((Func<XmlSerializer, CharStream>)XmlSerializer.GetCharStream).Method;
        /// <summary>
        /// 设置集合子节点名称函数信息
        /// </summary>
        internal static readonly MethodInfo SetItemNameMethod = ((Action<XmlSerializer, string>)XmlSerializer.SetItemName).Method;
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
            dynamicMethod = new DynamicMethod("XmlSerializer", null, new Type[] { typeof(XmlSerializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(CharStream));

            generator.Emit(OpCodes.Ldarg_0);
            generator.call(GetCharStreamMethod);
            generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <param name="attribute">XML序列化成员配置</param>
        private void nameStart(string name, XmlSerializeMemberAttribute attribute)
        {
            WriteName(generator, OpCodes.Ldloc_0, name, false);

            if (attribute != null && attribute.ItemName != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldstr, attribute.ItemName);
                generator.call(SetItemNameMethod);
            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="attribute">XML序列化成员配置</param>
        public void Push(FieldIndex field, XmlSerializeMemberAttribute attribute)
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
                generator.Emit(OpCodes.Brfalse, end = generator.DefineLabel());
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

            WriteName(generator, OpCodes.Ldloc_0, name, true);

            if (isOutputMethod != null) generator.MarkLabel(end);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        /// <param name="attribute">XML序列化成员配置</param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod, XmlSerializeMemberAttribute attribute)
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
                generator.Emit(OpCodes.Brfalse, end = generator.DefineLabel());
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

            WriteName(generator, OpCodes.Ldloc_0, property.Member.Name, true);

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
                generator.Emit(OpCodes.Brfalse, end = generator.DefineLabel());
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
                generator.Emit(OpCodes.Brfalse, end = generator.DefineLabel());
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

        /// <summary>
        /// 写入名称
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="isEnd"></param>
        internal static void WriteName(ILGenerator generator, OpCode target, string name, bool isEnd)
        {
            StringWriter stringWriter = new StringWriter(generator, target, (name.Length << 1) + (isEnd ? 6 : 4));
            stringWriter.Write('<');
            if (isEnd) stringWriter.Write('/');
            stringWriter.Write(name);
            stringWriter.Write('>');
            stringWriter.WriteEnd();
        }
    }
}
