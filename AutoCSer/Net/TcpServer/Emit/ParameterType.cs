using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Threading;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 参数类型
    /// </summary>
    internal sealed class ParameterType
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        internal Type Type;
        /// <summary>
        /// 类型编号
        /// </summary>
        internal int Index;
        /// <summary>
        /// 是否简单序列化
        /// </summary>
        internal bool IsSimpleSerialize;
        /// <summary>
        /// 是否需要初始化对象
        /// </summary>
        internal bool IsInitobj;
        /// <summary>
        /// 参数字段集合
        /// </summary>
        private FieldInfo[] fields;
        /// <summary>
        /// 设置参数字段集合
        /// </summary>
        /// <param name="fields"></param>
        private void setFields(FieldInfo[] fields)
        {
            this.fields = fields;
            IsSimpleSerialize = true;
            foreach (FieldInfo field in fields)
            {
                if (IsSimpleSerialize && !SimpleSerialize.Serializer.IsType(field.FieldType)) IsSimpleSerialize = false;
                if (!IsInitobj && field.FieldType.isInitobj()) IsInitobj = true;
            }
        }
        /// <summary>
        /// 获取参数字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal FieldInfo GetField(string name)
        {
            foreach (FieldInfo field in fields)
            {
                if (field.Name == name) return field;
            }
            return null;
        }

        /// <summary>
        /// 参数排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(ParameterInfo left, ParameterInfo right)
        {
            return string.CompareOrdinal(left.Name, right.Name);
        }
        /// <summary>
        /// 二进制数据序列化类型配置构造函数
        /// </summary>
        private static readonly ConstructorInfo serializeAttributeConstructorInfo = typeof(AutoCSer.BinarySerialize.SerializeAttribute).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(bool) }, null);
        /// <summary>
        /// 序列化包装处理配置构造函数
        /// </summary>
        private static readonly ConstructorInfo boxSerializeAttributeConstructorInfo = typeof(AutoCSer.Metadata.BoxSerializeAttribute).GetConstructor(NullValue<Type>.Array);
        /// <summary>
        /// JSON 序列化成员配置构造函数
        /// </summary>
        private static readonly ConstructorInfo jsonIgnoreMemberAttributeConstructorInfo = typeof(AutoCSer.Json.IgnoreMemberAttribute).GetConstructor(NullValue<Type>.Array);
        /// <summary>
        /// 逻辑真值参数
        /// </summary>
        private static readonly object[] trueParameter = new object[] { true };
        /// <summary>
        /// 逻辑假值参数
        /// </summary>
        private static readonly object[] falseParameter = new object[] { false };
        /// <summary>
        /// 返回值接口类型 AutoCSer.Net.IReturnParameter[]
        /// </summary>
        private static readonly Dictionary<Type, Type> returnInterfaceTypes = DictionaryCreator.CreateOnly<Type, Type>();
        /// <summary>
        /// 返回值接口类型
        /// </summary>
        private static readonly Type[] returnInterfaces = new Type[1];
        /// <summary>
        /// 返回值类型
        /// </summary>
        private static readonly Type[] returnTypes = new Type[1];
        /// <summary>
        /// TCP 参数类型集合
        /// </summary>
        private static readonly Dictionary<ParameterHash, ParameterType> types = DictionaryCreator<ParameterHash>.Create<ParameterType>();
        /// <summary>
        /// TCP 参数类型编号
        /// </summary>
        private static int typeIndex;
        /// <summary>
        /// TCP 参数类型集合访问锁
        /// </summary>
        private static readonly object typeLock = new object();
        /// <summary>
        /// 获取 TCP 参数类型
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <param name="returnType">类型</param>
        /// <param name="isSerializeReferenceMember">是否检测相同的引用成员</param>
        /// <param name="isSerializeBox">IsSerializeBox</param>
        /// <returns></returns>
        internal static ParameterType Get(ParameterInfo[] parameters, Type returnType, bool isSerializeReferenceMember, bool isSerializeBox)
        {
            ParameterFlag flag = isSerializeReferenceMember ? ParameterFlag.IsSerializeReferenceMember : ParameterFlag.None;
            if (isSerializeBox && parameters.Length == (returnType == typeof(void) ? 1 : 0)) flag |= ParameterFlag.IsSerializeBox;
            ParameterHash parameterType = new ParameterHash(parameters.Length > 1 ? parameters.copy().sort(compare) : parameters, returnType, flag);
            ParameterType type;
            Monitor.Enter(typeLock);
            if (types.TryGetValue(parameterType, out type))
            {
                Monitor.Exit(typeLock);
                return type;
            }
            try
            {
                Type[] interfaces;
                if (returnType == typeof(void)) interfaces = null;
                else
                {
                    interfaces = returnInterfaces;
                    if (!returnInterfaceTypes.TryGetValue(returnType, out interfaces[0]))
                    {
                        returnInterfaceTypes.Add(returnType, interfaces[0] = typeof(AutoCSer.Net.IReturnParameter<>).MakeGenericType(returnType));
                    }
                }
                TypeBuilder typeBuilder = AutoCSer.Emit.Builder.Module.Builder.DefineType("AutoCSer.Net.TcpServer.Emit.ParameterType" + (++typeIndex).toString(), TypeAttributes.AutoLayout | TypeAttributes.Public, typeof(ValueType), interfaces);
                foreach (ParameterInfo parameter in parameters) typeBuilder.DefineField(parameter.Name, parameter.elementType(), FieldAttributes.Public);
                if (returnType != typeof(void))
                {
                    FieldBuilder returnFieldBuilder = typeBuilder.DefineField(ReturnValue.RetParameterName, returnType, FieldAttributes.Public);
                    returnFieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(jsonIgnoreMemberAttributeConstructorInfo, NullValue<object>.Array));
                    //returnFieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(jsonSerializeMemberAttributeConstructorInfo, trueParameter));
                    //returnFieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(jsonParseMemberAttributeConstructorInfo, trueParameter));

                    PropertyInfo returnProperty = interfaces[0].GetProperty(ReturnValue.ReturnParameterName);
                    MethodInfo returnPropertyGetMethod = returnProperty.GetGetMethod();
                    MethodBuilder getReturnMethodBuilder = typeBuilder.DefineMethod(returnPropertyGetMethod.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, returnPropertyGetMethod.ReturnType, null);
                    ILGenerator getReturnGenerator = getReturnMethodBuilder.GetILGenerator();
                    getReturnGenerator.Emit(OpCodes.Ldarg_0);
                    getReturnGenerator.Emit(OpCodes.Ldfld, returnFieldBuilder);
                    getReturnGenerator.Emit(OpCodes.Ret);
                    typeBuilder.DefineMethodOverride(getReturnMethodBuilder, returnPropertyGetMethod);

                    MethodInfo returnPropertySetMethod = returnProperty.GetSetMethod();
                    returnTypes[0] = returnType;
                    MethodBuilder setReturnMethodBuilder = typeBuilder.DefineMethod(returnPropertySetMethod.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, returnPropertySetMethod.ReturnType, returnTypes);
                    ILGenerator setReturnGenerator = setReturnMethodBuilder.GetILGenerator();
                    setReturnGenerator.Emit(OpCodes.Ldarg_0);
                    setReturnGenerator.Emit(OpCodes.Ldarg_1);
                    setReturnGenerator.Emit(OpCodes.Stfld, returnFieldBuilder);
                    setReturnGenerator.Emit(OpCodes.Ret);
                    typeBuilder.DefineMethodOverride(setReturnMethodBuilder, returnPropertySetMethod);

                    //AutoCSer.Net.IReturnParameter<@MethodReturnType.FullName>
                    PropertyBuilder returnPropertyBuilder = typeBuilder.DefineProperty(ReturnValue.ReturnParameterName, PropertyAttributes.None, returnProperty.PropertyType, null);
                    returnPropertyBuilder.SetGetMethod(getReturnMethodBuilder);
                    returnPropertyBuilder.SetSetMethod(setReturnMethodBuilder);
                }
                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(serializeAttributeConstructorInfo, (flag & ParameterFlag.IsSerializeReferenceMember) == 0 ? falseParameter : trueParameter));
                if ((flag & ParameterFlag.IsSerializeBox) != 0) typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(boxSerializeAttributeConstructorInfo, NullValue<object>.Array));
                type = new ParameterType { Type = typeBuilder.CreateType(), Index = typeIndex };
                type.setFields(type.Type.GetFields());
                types.Add(parameterType, type);
            }
            finally { Monitor.Exit(typeLock); }
            return type;
        }
        ///// <summary>
        ///// 判断参数类型是否可用
        ///// </summary>
        ///// <param name="parameter"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal static bool IsParameterType(ParameterInfo parameter)
        //{
        //    Type type = parameter.ParameterType;
        //    return type != typeof(TcpInternalServer.ClientSocketSender) && type != typeof(TcpInternalServer.ServerSocketSender)
        //        && type != typeof(TcpOpenServer.ClientSocketSender) && type != typeof(TcpOpenServer.ServerSocketSender);
        //}
        /// <summary>
        /// 判断是否有效输入参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsInputParameter(ParameterInfo parameter)
        {
            if (parameter.IsOut) return false;
            Type type = parameter.ParameterType;
            return type != typeof(TcpInternalServer.ClientSocketSender) && type != typeof(TcpInternalServer.ServerSocketSender)
                && type != typeof(TcpOpenServer.ClientSocketSender) && type != typeof(TcpOpenServer.ServerSocketSender);
        }
    }
}
