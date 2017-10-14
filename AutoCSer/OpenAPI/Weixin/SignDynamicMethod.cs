using System;
using System.Runtime.InteropServices;
using System.Reflection;
using AutoCSer.Extension;
using AutoCSer.Metadata;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct SignDynamicMethod
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
        public SignDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("SignValueGetter", null, new Type[] { type, typeof(string[]) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fiedIndex"></param>
        /// <param name="index"></param>
        /// <returns>是否需要utf-8编码</returns>
        public bool Push(FieldIndex fiedIndex, int index)
        {
            FieldInfo field = fiedIndex.Member;
            Type type = field.FieldType;
            if (type.IsValueType)
            {
                MethodInfo numberToStringMethod;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Label end = generator.DefineLabel();
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldflda, field);
                    generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableHasValue(type));
                    generator.Emit(OpCodes.Brfalse_S, end);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.int32(index);
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldflda, field);
                    generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableValue(type));
                    Type nullableType = type.GetGenericArguments()[0];
                    if (nullableType.IsEnum)
                    {
                        numberToStringMethod = null;
                        generator.Emit(OpCodes.Box, nullableType);
                        generator.Emit(OpCodes.Callvirt, AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(typeof(object)));
                    }
                    else generator.Emit(OpCodes.Call, (numberToStringMethod = AutoCSer.Net.WebClient.Emit.Pub.GetNumberToStringMethod(nullableType)) ?? AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(nullableType));
                    generator.Emit(OpCodes.Stelem_Ref);
                    generator.MarkLabel(end);
                }
                else
                {
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.int32(index);
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, field);
                    if (type.IsEnum)
                    {
                        numberToStringMethod = null;
                        generator.Emit(OpCodes.Box, type);
                        generator.Emit(OpCodes.Callvirt, AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(typeof(object)));
                    }
                    else generator.Emit(OpCodes.Call, (numberToStringMethod = AutoCSer.Net.WebClient.Emit.Pub.GetNumberToStringMethod(type)) ?? AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(type));
                    generator.Emit(OpCodes.Stelem_Ref);
                }
                if (numberToStringMethod != null) return false;
            }
            else
            {
                Label end = default(Label);
                if (type != typeof(string))
                {
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, field);
                    generator.Emit(OpCodes.Brfalse_S, end = generator.DefineLabel());
                }
                generator.Emit(OpCodes.Ldarg_1);
                generator.int32(index);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                else generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, field);
                if (type != typeof(string)) generator.Emit(OpCodes.Callvirt, AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(type));
                generator.Emit(OpCodes.Stelem_Ref);
                if (type != typeof(string)) generator.MarkLabel(end);
            }
            return (fiedIndex.GetAttribute<SignMemberAttribute>(false) ?? SignMemberAttribute.Default).IsEncodeUtf8;
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property"></param>
        /// <param name="index"></param>
        /// <returns>是否需要utf-8编码</returns>
        public bool Push(PropertyIndex property, int index)
        {
            Type type = property.Member.PropertyType;
            MethodInfo method = property.Member.GetGetMethod(true);
            if (type.IsValueType)
            {
                MethodInfo numberToStringMethod;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Label end = generator.DefineLabel();
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Call, method);
                    generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableHasValue(type));
                    generator.Emit(OpCodes.Brfalse_S, end);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.int32(index);
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Call, method);
                    generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableValue(type));
                    Type nullableType = type.GetGenericArguments()[0];
                    if (nullableType.IsEnum)
                    {
                        numberToStringMethod = null;
                        generator.Emit(OpCodes.Box, nullableType);
                        generator.Emit(OpCodes.Callvirt, AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(typeof(object)));
                    }
                    else generator.Emit(OpCodes.Call, (numberToStringMethod = AutoCSer.Net.WebClient.Emit.Pub.GetNumberToStringMethod(nullableType)) ?? AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(nullableType));
                    generator.Emit(OpCodes.Stelem_Ref);
                    generator.MarkLabel(end);
                }
                else
                {
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.int32(index);
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Call, method);
                    if (type.IsEnum)
                    {
                        numberToStringMethod = null;
                        generator.Emit(OpCodes.Box, type);
                        generator.Emit(OpCodes.Callvirt, AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(typeof(object)));
                    }
                    else generator.Emit(OpCodes.Call, (numberToStringMethod = AutoCSer.Net.WebClient.Emit.Pub.GetNumberToStringMethod(type)) ?? AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(type));
                    generator.Emit(OpCodes.Stelem_Ref);
                }
                if (numberToStringMethod != null) return false;
            }
            else
            {
                Label end = default(Label);
                if (type != typeof(string))
                {
                    if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                    else generator.Emit(OpCodes.Ldarg_0);
                    generator.call(method);
                    generator.Emit(OpCodes.Brfalse_S, end = generator.DefineLabel());
                }
                generator.Emit(OpCodes.Ldarg_1);
                generator.int32(index);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
                else generator.Emit(OpCodes.Ldarg_0);
                generator.call(method);
                if (type != typeof(string)) generator.Emit(OpCodes.Callvirt, AutoCSer.Net.WebClient.Emit.Pub.GetToStringMethod(type));
                generator.Emit(OpCodes.Stelem_Ref);
                if (type != typeof(string)) generator.MarkLabel(end);
            }
            return (property.GetAttribute<SignMemberAttribute>(false) ?? SignMemberAttribute.Default).IsEncodeUtf8;
        }

        /// <summary>
        /// 创建委托
        /// </summary>
        /// <returns>委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
