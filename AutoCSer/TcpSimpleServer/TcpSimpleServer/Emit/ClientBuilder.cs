using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    internal sealed partial class Method<attributeType, methodAttributeType, serverSocketType>
    {
        /// <summary>
        /// 创建 TCP 客户端
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct ClientBuilder
        {
            /// <summary>
            /// TCP 客户端元数据
            /// </summary>
            internal ClientMetadata Metadata;
            /// <summary>
            /// 客户端命令信息集合
            /// </summary>
            internal TcpServer.CommandInfoBase[] Commands;
            /// <summary>
            /// 创建 TCP 客户端
            /// </summary>
            /// <param name="type"></param>
            /// <param name="attribute"></param>
            /// <param name="methods"></param>
            /// <param name="getCommandMethod"></param>
            /// <returns></returns>
            internal Type Build(Type type, TcpServer.ServerBaseAttribute attribute, Method<attributeType, methodAttributeType, serverSocketType>[] methods, MethodInfo getCommandMethod)
            {
                TypeBuilder typeBuilder = AutoCSer.Emit.Builder.Module.Builder.DefineType(Metadata.ClientTypeName + ".Emit." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, Metadata.MethodClientType, new Type[] { type });
                typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

                ConstructorBuilder staticConstructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null);
                ILGenerator staticConstructorGenerator = staticConstructorBuilder.GetILGenerator();
                Commands = new TcpServer.CommandInfoBase[methods.Length];
                int parameterIndex;
                foreach (Method<attributeType, methodAttributeType, serverSocketType> nextMethod in methods)
                {
                    if (nextMethod != null)
                    {
                        Method<attributeType, methodAttributeType, serverSocketType> method = nextMethod;
                        METHOD:
                        FieldBuilder commandInfoFieldBuilder;
                        if (method.Attribute.IsExpired) commandInfoFieldBuilder = null;
                        else
                        {
                            commandInfoFieldBuilder = typeBuilder.DefineField("_c" + method.Attribute.CommandIdentity.toString(), typeof(TcpServer.CommandInfoBase), FieldAttributes.Private | FieldAttributes.InitOnly | FieldAttributes.Static);
                            TcpServer.CommandInfoBase commandInfo = new TcpServer.CommandInfoBase { Command = method.Attribute.CommandIdentity + TcpServer.Server.CommandStartIndex, IsVerifyMethod = method.Attribute.IsVerifyMethod };
                            Commands[method.Attribute.CommandIdentity] = commandInfo;
                            if (method.IsJsonSerialize) commandInfo.CommandFlags = TcpServer.CommandFlags.JsonSerialize;
                            if (method.ParameterType != null)
                            {
                                commandInfo.InputParameterIndex = method.ParameterType.Index;
                                if (attribute.IsSimpleSerialize) commandInfo.IsSimpleSerializeInputParamter = method.ParameterType.IsSimpleSerialize;
                            }
                            if (attribute.IsSimpleSerialize && method.OutputParameterType != null) commandInfo.IsSimpleSerializeOutputParamter = method.OutputParameterType.IsSimpleSerialize && SimpleSerialize.Serializer.IsType(method.ReturnType);
                            #region private static readonly AutoCSer.Net.TcpServer.CommandInfoBase @MethodIdentityCommand = AutoCSer.Net.TcpInternalSimpleServer.Emit.Client<interfaceType>.commands[method.Attribute.CommandIdentity];
                            staticConstructorGenerator.int32(method.Attribute.CommandIdentity);
                            staticConstructorGenerator.call(getCommandMethod);
                            staticConstructorGenerator.Emit(OpCodes.Stsfld, commandInfoFieldBuilder);
                            #endregion
                        }
                        if (method.PropertyInfo == null)
                        {
                            ParameterInfo[] parameters = method.MethodInfo.GetParameters();
                            MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.MethodInfo.Name, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, method.MethodInfo.ReturnType, parameters.getArray(parameter => parameter.ParameterType));
                            typeBuilder.DefineMethodOverride(methodBuilder, method.MethodInfo);
                            ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                            if (method.Attribute.IsExpired)
                            {
                                if (method.ReturnValueType == null)
                                {
                                    #region throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                                    methodGenerator.throwString(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                                    #endregion
                                }
                                else
                                {
                                    #region @ParameterName = default(@ParameterType.FullName);
                                    parameterIndex = 0;
                                    foreach (ParameterInfo parameter in parameters)
                                    {
                                        ++parameterIndex;
                                        if (parameter.IsOut) methodGenerator.outParameterDefault(parameter, parameterIndex, method.Attribute.IsInitobj);
                                    }
                                    #endregion
                                    #region return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired };
                                    LocalBuilder returnReturnValueLocalBuilder = methodGenerator.DeclareLocal(method.ReturnValueType);
                                    if (method.ReturnType != typeof(void) && (method.Attribute.IsInitobj || method.ReturnType.isInitobj()))
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                    }
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                    methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                    methodGenerator.Emit(OpCodes.Stfld, method.ReturnType == typeof(void) ? TcpServer.Emit.ClientMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(TcpServer.Emit.ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                    methodGenerator.Emit(OpCodes.Ldloc_S, returnReturnValueLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                }
                            }
                            else
                            {
                                Label clientExceptionLabel = methodGenerator.DefineLabel();
                                #region if (_isDisposed_ == 0)
                                methodGenerator.Emit(OpCodes.Ldarg_0);
                                methodGenerator.Emit(OpCodes.Ldfld, TcpSimpleServer.Emit.ClientMetadata.MethodClientIsDisposedField);
                                methodGenerator.Emit(method.ParameterType == null && method.OutputParameterType == null ? OpCodes.Brtrue_S : OpCodes.Brtrue, clientExceptionLabel);
                                #endregion
                                LocalBuilder inputParameterLocalBuilder;
                                if (method.ParameterType == null) inputParameterLocalBuilder = null;
                                else
                                {
                                    #region TcpInternalSimpleServer.@InputParameterTypeName _inputParameter_ = new TcpInternalSimpleServer.@InputParameterTypeName { @ParameterName = @ParameterName };
                                    inputParameterLocalBuilder = methodGenerator.DeclareLocal(method.ParameterType.Type);
                                    LocalBuilder newInputParameterLocalBuilder = methodGenerator.DeclareLocal(method.ParameterType.Type);
                                    if (method.Attribute.IsInitobj || method.ParameterType.IsInitobj)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, newInputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.ParameterType.Type);
                                    }
                                    parameterIndex = 0;
                                    foreach (ParameterInfo parameter in parameters)
                                    {
                                        ++parameterIndex;
                                        if (SimpleParameterType.IsInputParameter(parameter))
                                        {
                                            methodGenerator.parameterToStructField(parameter, parameterIndex, newInputParameterLocalBuilder, method.ParameterType.GetField(parameter.Name));
                                        }
                                    }
                                    methodGenerator.Emit(OpCodes.Ldloc_S, newInputParameterLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder);
                                    #endregion
                                }
                                if (method.OutputParameterType == null)
                                {
                                    #region AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(@MethodIdentityCommand, ref _inputParameter_);
                                    LocalBuilder returnTypeLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.ReturnType));
                                    methodGenerator.Emit(OpCodes.Ldarg_0);
                                    methodGenerator.call(Metadata.MethodClientGetTcpClientMethod);
                                    methodGenerator.Emit(OpCodes.Ldsfld, commandInfoFieldBuilder);
                                    if (method.ParameterType == null) methodGenerator.call(Metadata.ClientCallMethod);
                                    else
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                        //methodGenerator.call(Metadata.ClientCallInputMethod.MakeGenericMethod(method.ParameterType.Type));
                                        methodGenerator.call(Metadata.GetParameterGenericType(method.ParameterType.Type).ClientCallMethod);
                                    }
                                    methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                                    #endregion
                                    if (method.ReturnValueType == null)
                                    {
                                        Label throwReturnTypeLabel = methodGenerator.DefineLabel();
                                        #region if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Success);
                                        methodGenerator.Emit(OpCodes.Bne_Un_S, throwReturnTypeLabel);
                                        methodGenerator.Emit(OpCodes.Ret);
                                        #endregion
                                        methodGenerator.MarkLabel(throwReturnTypeLabel);
                                        #region throw new Exception(AutoCSer.Net.TcpInternalServer.Emit.Client.ReturnTypeStrings[(byte)_returnType_]);
                                        methodGenerator.Emit(OpCodes.Ldsfld, TcpServer.Emit.ClientMetadata.ReturnTypeStringsField);
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldelem_Ref);
                                        methodGenerator.Emit(OpCodes.Newobj, AutoCSer.Extension.EmitGenerator.StringExceptionConstructor);
                                        methodGenerator.Emit(OpCodes.Throw);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region return new AutoCSer.Net.TcpServer.ReturnValue { Type = _returnType_ };
                                        LocalBuilder returnReturnValueLocalBuilder = methodGenerator.DeclareLocal(method.ReturnValueType);
                                        //methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        //methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Stfld, TcpServer.Emit.ClientMetadata.ReturnValueTypeField);
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ret);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region TcpInternalSimpleServer.@OutputParameterTypeName _outputParameter_ = new TcpInternalSimpleServer.@OutputParameterTypeName { @ParameterName = @ParameterName };
                                    LocalBuilder outputParameterLocalBuilder = methodGenerator.DeclareLocal(method.OutputParameterType.Type);
                                    if (method.Attribute.IsInitobj || method.OutputParameterType.IsInitobj)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.OutputParameterType.Type);
                                    }
                                    parameterIndex = 0;
                                    foreach (ParameterInfo parameter in parameters)
                                    {
                                        ++parameterIndex;
                                        if (parameter.ParameterType.IsByRef) methodGenerator.parameterToStructField(parameter, parameterIndex, outputParameterLocalBuilder, method.OutputParameterType.GetField(parameter.Name));
                                    }
                                    //if (method.ReturnInputParameter != null)
                                    //{
                                    //}
                                    #endregion
                                    #region AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer.@InputParameterTypeName, TcpInternalSimpleServer.@OutputParameterTypeName>(@MethodIdentityCommand, ref _inputParameter_, ref _outputParameter_);
                                    LocalBuilder returnTypeLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.ReturnType));
                                    methodGenerator.Emit(OpCodes.Ldarg_0);
                                    methodGenerator.call(Metadata.MethodClientGetTcpClientMethod);
                                    methodGenerator.Emit(OpCodes.Ldsfld, commandInfoFieldBuilder);
                                    if (method.ParameterType == null)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        //methodGenerator.call(Metadata.ClientGetMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                        methodGenerator.call(Metadata.GetParameterGenericType(method.OutputParameterType.Type).ClientGetMethod);
                                    }
                                    else
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        //methodGenerator.call(Metadata.ClientGetInputMethod.MakeGenericMethod(method.ParameterType.Type, method.OutputParameterType.Type));
                                        methodGenerator.call(Metadata.GetParameterGenericType2(method.ParameterType.Type, method.OutputParameterType.Type).ClientGetMethod);
                                    }
                                    methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                                    #endregion
                                    if (method.ReturnValueType == null)
                                    {
                                        Label throwReturnTypeLabel = methodGenerator.DefineLabel();
                                        #region if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success)
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Success);
                                        methodGenerator.Emit(OpCodes.Bne_Un, throwReturnTypeLabel);
                                        #endregion
                                        #region @ParameterName = _outputParameter_.@ParameterName;
                                        parameterIndex = 0;
                                        foreach (ParameterInfo parameter in parameters)
                                        {
                                            ++parameterIndex;
                                            if (parameter.ParameterType.IsByRef)
                                            {
                                                methodGenerator.outParameterFromValueField(parameter, parameterIndex, outputParameterLocalBuilder, method.OutputParameterType.GetField(parameter.Name));
                                            }
                                        }
                                        #endregion
                                        if (method.ReturnType == typeof(void))
                                        {
                                            #region return;
                                            methodGenerator.Emit(OpCodes.Ret);
                                            #endregion
                                        }
                                        else
                                        {
                                            #region return _outputParameter_.Ret;
                                            methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldfld, method.OutputParameterType.GetField(TcpServer.ReturnValue.RetParameterName));
                                            //methodGenerator.Emit(OpCodes.Stloc_S, returnValueLocalBuilder);
                                            //methodGenerator.Emit(OpCodes.Ldloc_S, returnValueLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ret);
                                            #endregion
                                        }
                                        methodGenerator.MarkLabel(throwReturnTypeLabel);
                                        #region throw new Exception(AutoCSer.Net.TcpServer.Emit.Client.ReturnTypeStrings[(byte)_returnType_]);
                                        methodGenerator.Emit(OpCodes.Ldsfld, TcpServer.Emit.ClientMetadata.ReturnTypeStringsField);
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldelem_Ref);
                                        methodGenerator.Emit(OpCodes.Newobj, AutoCSer.Extension.EmitGenerator.StringExceptionConstructor);
                                        methodGenerator.Emit(OpCodes.Throw);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region @ParameterName = _outputParameter_.@ParameterName;
                                        parameterIndex = 0;
                                        foreach (ParameterInfo parameter in parameters)
                                        {
                                            ++parameterIndex;
                                            if (parameter.ParameterType.IsByRef)
                                            {
                                                methodGenerator.outParameterFromValueField(parameter, parameterIndex, outputParameterLocalBuilder, method.OutputParameterType.GetField(parameter.Name));
                                            }
                                        }
                                        #endregion
                                        #region return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _returnType_, Value = _outputParameter_.Return };
                                        LocalBuilder returnReturnValueLocalBuilder = methodGenerator.DeclareLocal(method.ReturnValueType);
                                        if (method.ReturnType == typeof(void))
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Stfld, TcpServer.Emit.ClientMetadata.ReturnValueTypeField);
                                        }
                                        else
                                        {
                                            if (method.Attribute.IsInitobj || method.ReturnType.isInitobj())
                                            {
                                                methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                                methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                            }
                                            methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Stfld, method.ReturnValueType.GetField(TcpServer.Emit.ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                            methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldfld, method.OutputParameterType.GetField(TcpServer.ReturnValue.RetParameterName));
                                            methodGenerator.Emit(OpCodes.Stfld, method.ReturnValueType.GetField("Value", BindingFlags.Instance | BindingFlags.Public));
                                        }
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ret);
                                        #endregion
                                    }
                                }
                                methodGenerator.MarkLabel(clientExceptionLabel);
                                if (method.ReturnValueType == null)
                                {
                                    #region throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                                    methodGenerator.throwString(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                                    #endregion
                                }
                                else
                                {
                                    #region @ParameterName = default(@ParameterType.FullName);
                                    parameterIndex = 0;
                                    foreach (ParameterInfo parameter in parameters)
                                    {
                                        ++parameterIndex;
                                        if (parameter.IsOut) methodGenerator.outParameterDefault(parameter, parameterIndex, method.Attribute.IsInitobj);
                                    }
                                    #endregion
                                    #region return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                                    LocalBuilder returnReturnValueLocalBuilder = methodGenerator.DeclareLocal(method.ReturnValueType);
                                    if (method.ReturnType != typeof(void) && method.ReturnType.isInitobj())
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                    }
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                    methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ClientException);
                                    methodGenerator.Emit(OpCodes.Stfld, method.ReturnType == typeof(void) ? TcpServer.Emit.ClientMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(TcpServer.Emit.ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                    methodGenerator.Emit(OpCodes.Ldloc_S, returnReturnValueLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                }
                            }
                        }
                        else if (method.IsPropertySetMethod)
                        {
                            if (method.PropertyBuilder != null || method.PropertyGetMethod == null)
                            {
                                ParameterInfo[] parameters = method.MethodInfo.GetParameters();
                                PropertyBuilder propertyBuilder = method.PropertyBuilder ?? typeBuilder.DefineProperty(method.PropertyInfo.Name, PropertyAttributes.HasDefault, parameters[parameters.Length - 1].ParameterType, new LeftArray<ParameterInfo> { Array = parameters, Length = parameters.Length - 1 }.GetArray(parameter => parameter.ParameterType));
                                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(method.MethodInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual, method.MethodInfo.ReturnType, parameters.getArray(parameter => parameter.ParameterType));
                                ILGenerator methodGenerator = setMethodBuilder.GetILGenerator();
                                //XXX
                                propertyBuilder.SetSetMethod(setMethodBuilder);
                                method.PropertyBuilder = null;
                            }
                        }
                        else
                        {
                            Type[] parameterTypes = method.MethodInfo.GetParameters().getArray(parameter => parameter.ParameterType);
                            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(method.PropertyInfo.Name, PropertyAttributes.HasDefault, method.MethodInfo.ReturnType, parameterTypes);
                            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(method.MethodInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual, method.MethodInfo.ReturnType, parameterTypes);
                            ILGenerator methodGenerator = getMethodBuilder.GetILGenerator();
                            //XXX
                            propertyBuilder.SetGetMethod(getMethodBuilder);
                            if (method.PropertySetMethod != null)
                            {
                                method = method.PropertySetMethod;
                                method.PropertyBuilder = propertyBuilder;
                                goto METHOD;
                            }
                        }
                    }
                }
                staticConstructorGenerator.Emit(OpCodes.Ret);
                return typeBuilder.CreateType();
            }
        }
    }
}
