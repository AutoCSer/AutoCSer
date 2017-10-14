using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Net.WebClient.Form
{
    /// <summary>
    /// web表单生成动态函数
    /// </summary>
    /// <summary>
    /// 动态函数
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct GetterDynamicMethod
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
        public GetterDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("formGetter", null, new Type[] { type, typeof(NameValueCollection) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldInfo field)
        {
            Type type = field.FieldType;
            if (type.IsValueType)
            {
                Type nullType = type.nullableType();
                if (nullType == null) push(field);
                else
                {
                    Label end = generator.DefineLabel();
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldflda, field);
                    generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableHasValue(type));
                    generator.Emit(OpCodes.Brfalse_S, end);
                    push(field, nullType);
                    generator.MarkLabel(end);
                }
            }
            else pushNull(field);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        private void pushNull(FieldInfo field)
        {
            Label end = generator.DefineLabel();
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
            else generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            generator.Emit(OpCodes.Brfalse_S, end);
            push(field);
            generator.MarkLabel(end);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="nullType">可空类型</param>
        private void push(FieldInfo field, Type nullType = null)
        {
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldstr, field.Name);
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
            else generator.Emit(OpCodes.Ldarg_0);
            Type type = nullType ?? field.FieldType;
            if (type == typeof(string)) generator.Emit(OpCodes.Ldfld, field);
            else
            {
                MethodInfo method = Emit.Pub.GetNumberToStringMethod(type);
                if (method == null)
                {
                    generator.Emit(OpCodes.Ldflda, field);
                    if (type.IsEnum)
                    {
                        generator.Emit(OpCodes.Box, type);
                        generator.Emit(OpCodes.Callvirt, Emit.Pub.GetToStringMethod(typeof(object)));
                    }
                    else generator.call(Emit.Pub.GetToStringMethod(type));
                }
                else
                {
                    if (nullType == null) generator.Emit(OpCodes.Ldfld, field);
                    else
                    {
                        generator.Emit(OpCodes.Ldflda, field);
                        generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableValue(field.FieldType));
                    }
                    generator.Emit(OpCodes.Call, method);
                }
            }
            generator.Emit(OpCodes.Callvirt, Emit.Pub.NameValueCollectionAddMethod);
        }
        /// <summary>
        /// 创建web表单委托
        /// </summary>
        /// <returns>web表单委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
