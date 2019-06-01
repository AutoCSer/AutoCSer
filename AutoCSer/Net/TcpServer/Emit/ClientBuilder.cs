using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    internal sealed partial class Method<attributeType, methodAttributeType, serverSocketSenderType>
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
            internal CommandInfo[] Commands;
            /// <summary>
            /// 创建 TCP 客户端
            /// </summary>
            /// <param name="type"></param>
            /// <param name="attribute"></param>
            /// <param name="methods"></param>
            /// <param name="getCommandMethod"></param>
            /// <returns></returns>
            internal Type Build(Type type, ServerBaseAttribute attribute, Method<attributeType, methodAttributeType, serverSocketSenderType>[] methods, MethodInfo getCommandMethod)
            {
                TypeBuilder typeBuilder = AutoCSer.Emit.Builder.Module.Builder.DefineType(Metadata.ClientTypeName + ".Emit." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, Metadata.MethodClientType, new Type[] { type });
                typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

                ConstructorBuilder staticConstructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null);
                ILGenerator staticConstructorGenerator = staticConstructorBuilder.GetILGenerator();
                Commands = new CommandInfo[methods.Length];
                int parameterIndex;
                foreach (Method<attributeType, methodAttributeType, serverSocketSenderType> nextMethod in methods)
                {
                    if (nextMethod != null)
                    {
                        Method<attributeType, methodAttributeType, serverSocketSenderType> method = nextMethod;
                        METHOD:
                        FieldBuilder commandInfoFieldBuilder;
                        if (method.Attribute.IsExpired) commandInfoFieldBuilder = null;
                        else
                        {
                            commandInfoFieldBuilder = typeBuilder.DefineField("_c" + method.Attribute.CommandIdentity.toString(), typeof(CommandInfo), FieldAttributes.Private | FieldAttributes.InitOnly | FieldAttributes.Static);
                            CommandInfo commandInfo = new CommandInfo { Command = method.Attribute.CommandIdentity + TcpServer.Server.CommandStartIndex, TaskType = method.IsAsynchronousCallback ? method.Attribute.ClientTaskType : AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = method.Attribute.IsVerifyMethod };
                            Commands[method.Attribute.CommandIdentity] = commandInfo;
                            if (method.IsKeepCallback) commandInfo.IsKeepCallback = 1;
                            if (method.IsClientSendOnly) commandInfo.IsSendOnly = 1;
                            if (method.IsJsonSerialize) commandInfo.CommandFlags = CommandFlags.JsonSerialize;
                            if (method.ParameterType != null)
                            {
                                commandInfo.InputParameterIndex = method.ParameterType.Index;
                                if (attribute.IsSimpleSerialize) commandInfo.IsSimpleSerializeInputParamter = method.ParameterType.IsSimpleSerialize;
                            }
                            if (attribute.IsSimpleSerialize && method.OutputParameterType != null) commandInfo.IsSimpleSerializeOutputParamter = method.OutputParameterType.IsSimpleSerialize && SimpleSerialize.Serializer.IsType(method.ReturnType);
                            #region private static readonly AutoCSer.Net.TcpServer.CommandInfo @MethodIdentityCommand = AutoCSer.Net.TcpInternalServer.Emit.Client<interfaceType>.commands[method.Attribute.CommandIdentity];
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
                                if (method.IsAsynchronousCallback)
                                {
                                    #region if (_onReturn_ == null)
                                    Label onReturnLabel = methodGenerator.DefineLabel();
                                    methodGenerator.ldarg(parameters.Length);
                                    methodGenerator.Emit(OpCodes.Brtrue_S, onReturnLabel);
                                    #endregion
                                    #region throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                                    methodGenerator.throwString(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                                    #endregion
                                    methodGenerator.MarkLabel(onReturnLabel);
                                    #region _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                                    LocalBuilder returnValueLocalBuilder = methodGenerator.DeclareLocal(method.ReturnValueType);
                                    methodGenerator.ldarg(parameters.Length);
                                    if (method.ReturnType != typeof(void) && (method.Attribute.IsInitobj || method.ReturnType.isInitobj()))
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                    }
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnValueLocalBuilder);
                                    methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                    methodGenerator.Emit(OpCodes.Stfld, method.ReturnType == typeof(void) ? ClientMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                    methodGenerator.Emit(OpCodes.Ldloc_S, returnValueLocalBuilder);
                                    methodGenerator.call(parameters[parameters.Length - 1].ParameterType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { method.ReturnValueType }, null));
                                    if (!method.IsClientAsynchronousCallback) methodGenerator.Emit(OpCodes.Pop);//Func<int, bool>
                                    #endregion
                                    #region return null;
                                    if (method.IsKeepCallback) methodGenerator.Emit(OpCodes.Ldnull);
                                    methodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                }
                                else if (method.ReturnValueType == null)
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
                                    methodGenerator.Emit(OpCodes.Stfld, method.ReturnType == typeof(void) ? ClientMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                    methodGenerator.Emit(OpCodes.Ldloc_S, returnReturnValueLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                }
                            }
                            else if (method.IsClientSendOnly)
                            {
                                LocalBuilder inputParameterLocalBuilder;
                                if (method.ParameterType == null) inputParameterLocalBuilder = null;
                                else
                                {
                                    #region TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName { @ParameterName = @ParameterName };
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
                                        if (ParameterType.IsInputParameter(parameter))
                                        {
                                            methodGenerator.parameterToStructField(parameter, parameterIndex, newInputParameterLocalBuilder, method.ParameterType.GetField(parameter.Name));
                                        }
                                    }
                                    methodGenerator.Emit(OpCodes.Ldloc_S, newInputParameterLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder);
                                    #endregion
                                }
                                #region _TcpClient_.Sender.CallOnly(@MethodIdentityCommand, ref _inputParameter_);
                                methodGenerator.Emit(OpCodes.Ldarg_0);
                                methodGenerator.call(Metadata.MethodClientGetTcpClientMethod);
                                methodGenerator.call(Metadata.ClientGetSenderMethod);
                                methodGenerator.Emit(OpCodes.Ldsfld, commandInfoFieldBuilder);
                                if (method.ParameterType == null) methodGenerator.call(Metadata.ClientSocketSenderCallOnlyMethod);
                                else
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                    //methodGenerator.call(Metadata.ClientSocketSenderCallOnlyInputMethod.MakeGenericMethod(method.ParameterType.Type));
                                    methodGenerator.call(Metadata.GetParameterGenericType(method.ParameterType.Type).ClientSocketSenderCallOnlyMethod);
                                }
                                #endregion
                                methodGenerator.Emit(OpCodes.Ret);
                            }
                            else if (method.IsAsynchronousCallback)
                            {
                                Label returnLabel = methodGenerator.DefineLabel(), returnKeepCallbackLabel;
                                LocalBuilder keepCallbackLocalBuilder;
                                if (method.IsKeepCallback)
                                {
                                    keepCallbackLocalBuilder = methodGenerator.DeclareLocal(typeof(KeepCallback));
                                    returnKeepCallbackLabel = methodGenerator.DefineLabel();
                                }
                                else
                                {
                                    keepCallbackLocalBuilder = null;
                                    returnKeepCallbackLabel = default(Label);
                                }
                                if (method.ReturnType == typeof(void))
                                {
                                    #region try
                                    methodGenerator.BeginExceptionBlock();
                                    #endregion
                                    Label leaveTryLabel = methodGenerator.DefineLabel();
                                    #region AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                                    LocalBuilder senderLocalBuilder = methodGenerator.DeclareLocal(Metadata.SenderType);
                                    methodGenerator.Emit(OpCodes.Ldarg_0);
                                    methodGenerator.call(Metadata.MethodClientGetTcpClientMethod);
                                    methodGenerator.call(Metadata.ClientGetSenderMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, senderLocalBuilder);
                                    #endregion
                                    #region if (_socket_ != null)
                                    methodGenerator.Emit(OpCodes.Ldloc_S, senderLocalBuilder);
                                    methodGenerator.Emit(method.ParameterType == null ? OpCodes.Brfalse_S : OpCodes.Brfalse, leaveTryLabel);
                                    #endregion
                                    LocalBuilder inputParameterLocalBuilder;
                                    if (method.ParameterType == null) inputParameterLocalBuilder = null;
                                    else
                                    {
                                        #region TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName { @ParameterName = @ParameterName };
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
                                            if (ParameterType.IsInputParameter(parameter))
                                            {
                                                FieldInfo field = method.ParameterType.GetField(parameter.Name);
                                                if (field != null) methodGenerator.parameterToStructField(parameter, parameterIndex, newInputParameterLocalBuilder, field);
                                            }
                                        }
                                        methodGenerator.Emit(OpCodes.Ldloc_S, newInputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder);
                                        #endregion
                                    }
                                    #region _keepCallback_ = _socket_.CallKeep(@MethodIdentityCommand, _onReturn_, ref _inputParameter_);
                                    methodGenerator.Emit(OpCodes.Ldloc_S, senderLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldsfld, commandInfoFieldBuilder);
                                    methodGenerator.ldarg(parameters.Length);
                                    if (!method.IsClientAsynchronousCallback) methodGenerator.call(ClientMetadata.ClientCallbackGetMethod);
                                    if (method.IsKeepCallback)
                                    {
                                        if (method.ParameterType == null) methodGenerator.call(Metadata.ClientSocketSenderCallKeepMethod);
                                        else
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                            //methodGenerator.call(Metadata.ClientSocketSenderCallKeepInputMethod.MakeGenericMethod(method.ParameterType.Type));
                                            methodGenerator.call(Metadata.GetParameterGenericType(method.ParameterType.Type).ClientSocketSenderCallKeepMethod);
                                        }
                                        methodGenerator.Emit(OpCodes.Stloc_S, keepCallbackLocalBuilder);
                                    }
                                    else
                                    {
                                        if (method.ParameterType == null) methodGenerator.call(Metadata.ClientSocketSenderCallMethod);
                                        else
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                            //methodGenerator.call(Metadata.ClientSocketSenderCallInputMethod.MakeGenericMethod(method.ParameterType.Type));
                                            methodGenerator.call(Metadata.GetParameterGenericType(method.ParameterType.Type).ClientSocketSenderCallMethod);
                                        }
                                    }
                                    #endregion
                                    #region _onReturn_ = null;
                                    methodGenerator.Emit(OpCodes.Ldnull);
                                    methodGenerator.Emit(OpCodes.Starg_S, parameters.Length);
                                    #endregion
                                    if (method.IsKeepCallback) methodGenerator.Emit(OpCodes.Leave_S, returnKeepCallbackLabel);
                                    methodGenerator.MarkLabel(leaveTryLabel);
                                    methodGenerator.Emit(OpCodes.Leave_S, returnLabel);
                                    #region finally
                                    methodGenerator.BeginFinallyBlock();
                                    #endregion
                                    Label endfinallyLabel = methodGenerator.DefineLabel();
                                    #region if (_onReturn_ != null)
                                    methodGenerator.ldarg(parameters.Length);
                                    methodGenerator.Emit(OpCodes.Brfalse_S, endfinallyLabel);
                                    #endregion
                                    #region _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException });
                                    LocalBuilder returnValueLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.ReturnValue));
                                    methodGenerator.ldarg(parameters.Length);
                                    //methodGenerator.Emit(OpCodes.Ldloca_S, returnValueLocalBuilder);
                                    //methodGenerator.Emit(OpCodes.Initobj, typeof(AutoCSer.Net.TcpServer.ReturnValue));
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnValueLocalBuilder);
                                    methodGenerator.int32((int)AutoCSer.Net.TcpServer.ReturnType.ClientException);
                                    methodGenerator.Emit(OpCodes.Stfld, ClientMetadata.ReturnValueTypeField);
                                    methodGenerator.Emit(OpCodes.Ldloc_S, returnValueLocalBuilder);
                                    methodGenerator.call(parameters[parameters.Length - 1].ParameterType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { method.ReturnValueType }, null));
                                    if (!method.IsClientAsynchronousCallback) methodGenerator.Emit(OpCodes.Pop);//Func<int, bool>
                                    #endregion
                                    methodGenerator.MarkLabel(endfinallyLabel);
                                    methodGenerator.Emit(OpCodes.Endfinally);
                                    #region try end
                                    methodGenerator.EndExceptionBlock();
                                    #endregion
                                }
                                else
                                {
                                    #region AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer.@OutputParameterTypeName>> _onOutput_ = _TcpClient_.GetCallback<@MethodReturnType.FullName, TcpInternalServer.@OutputParameterTypeName>(_onReturn_);
                                    Type outputParameterType = typeof(AutoCSer.Net.TcpServer.ReturnValue<>).MakeGenericType(method.OutputParameterType.Type);
                                    Type callbackType = typeof(AutoCSer.Net.Callback<>).MakeGenericType(outputParameterType);
                                    LocalBuilder onOuputLocalBuilder = methodGenerator.DeclareLocal(callbackType);
                                    methodGenerator.Emit(OpCodes.Ldarg_0);
                                    methodGenerator.call(Metadata.MethodClientGetTcpClientMethod);
                                    methodGenerator.ldarg(parameters.Length);
                                    //if (!method.IsClientAsynchronousCallback) methodGenerator.call(typeof(ClientCallback<>).MakeGenericType(method.ReturnType).GetMethod(ClientMetadata.ClientCallbackGetMethod.Name, BindingFlags.Public | BindingFlags.Static));
                                    if (!method.IsClientAsynchronousCallback) methodGenerator.call(AutoCSer.Metadata.GenericType.Get(method.ReturnType).TcpClientCallbackGetMethod);
                                    //methodGenerator.call(Metadata.ClientGetCallbackMethod.MakeGenericMethod(method.ReturnType, method.OutputParameterType.Type));
                                    methodGenerator.call(Metadata.GetOutputParameterGenericType(method.ReturnType, method.OutputParameterType.Type).ClientGetCallbackMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, onOuputLocalBuilder);
                                    #endregion
                                    #region try
                                    methodGenerator.BeginExceptionBlock();
                                    #endregion
                                    Label leaveTryLabel = methodGenerator.DefineLabel();
                                    #region AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                                    LocalBuilder senderLocalBuilder = methodGenerator.DeclareLocal(Metadata.SenderType);
                                    methodGenerator.Emit(OpCodes.Ldarg_0);
                                    methodGenerator.call(Metadata.MethodClientGetTcpClientMethod);
                                    methodGenerator.call(Metadata.ClientGetSenderMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, senderLocalBuilder);
                                    #endregion
                                    #region if (_socket_ != null)
                                    methodGenerator.Emit(OpCodes.Ldloc_S, senderLocalBuilder);
                                    methodGenerator.Emit(method.ParameterType == null ? OpCodes.Brfalse_S : OpCodes.Brfalse, leaveTryLabel);
                                    #endregion
                                    LocalBuilder inputParameterLocalBuilder;
                                    if (method.ParameterType == null) inputParameterLocalBuilder = null;
                                    else
                                    {
                                        #region TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName { @ParameterName = @ParameterName };
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
                                            if (ParameterType.IsInputParameter(parameter))
                                            {
                                                FieldInfo field = method.ParameterType.GetField(parameter.Name);
                                                if (field != null) methodGenerator.parameterToStructField(parameter, parameterIndex, newInputParameterLocalBuilder, field);
                                            }
                                        }
                                        methodGenerator.Emit(OpCodes.Ldloc_S, newInputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder);
                                        #endregion
                                    }
                                    #region _keepCallback_ = _socket_.Get(@MethodIdentityCommand, ref _onOutput_, ref _inputParameter_);
                                    methodGenerator.Emit(OpCodes.Ldloc_S, senderLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldsfld, commandInfoFieldBuilder);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, onOuputLocalBuilder);
                                    if (method.IsKeepCallback)
                                    {
                                        //if (method.ParameterType == null) methodGenerator.call(Metadata.ClientSocketSenderGetKeepAsynchronousMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                        if (method.ParameterType == null) methodGenerator.call(Metadata.GetParameterGenericType(method.OutputParameterType.Type).ClientSocketSenderGetKeepMethod);
                                        else
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                            //methodGenerator.call(Metadata.ClientSocketSenderGetKeepInputAsynchronousMethod.MakeGenericMethod(method.ParameterType.Type, method.OutputParameterType.Type));
                                            methodGenerator.call(Metadata.GetParameterGenericType2(method.ParameterType.Type, method.OutputParameterType.Type).ClientSocketSenderGetKeepMethod);
                                        }
                                        methodGenerator.Emit(OpCodes.Stloc_S, keepCallbackLocalBuilder);
                                    }
                                    else
                                    {
                                        //if (method.ParameterType == null) methodGenerator.call(Metadata.ClientSocketSenderGetAsynchronousMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                        if (method.ParameterType == null) methodGenerator.call(Metadata.GetParameterGenericType(method.OutputParameterType.Type).ClientSocketSenderGetMethod);
                                        else
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                            //methodGenerator.call(Metadata.ClientSocketSenderGetInputAsynchronousMethod.MakeGenericMethod(method.ParameterType.Type, method.OutputParameterType.Type));
                                            methodGenerator.call(Metadata.GetParameterGenericType2(method.ParameterType.Type, method.OutputParameterType.Type).ClientSocketSenderGetMethod);
                                        }
                                    }
                                    #endregion
                                    if (method.IsKeepCallback) methodGenerator.Emit(OpCodes.Leave_S, returnKeepCallbackLabel);
                                    methodGenerator.MarkLabel(leaveTryLabel);
                                    methodGenerator.Emit(OpCodes.Leave_S, returnLabel);
                                    #region finally
                                    methodGenerator.BeginFinallyBlock();
                                    #endregion
                                    Label endfinallyLabel = methodGenerator.DefineLabel();
                                    #region if (_onOutput_ != null)
                                    methodGenerator.Emit(OpCodes.Ldloc_S, onOuputLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Brfalse_S, endfinallyLabel);
                                    #endregion
                                    #region AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer.@OutputParameterTypeName> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer.@OutputParameterTypeName> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                                    LocalBuilder outputParameterLocalBuilder = methodGenerator.DeclareLocal(outputParameterType);
                                    if (method.Attribute.IsInitobj || method.OutputParameterType.IsInitobj)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, outputParameterType);
                                    }
                                    methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                    methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ClientException);
                                    methodGenerator.Emit(OpCodes.Stfld, outputParameterType.GetField(ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                    #endregion
                                    #region _onOutput_.Call(ref _outputParameter_);
                                    methodGenerator.Emit(OpCodes.Ldloc_S, onOuputLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                    methodGenerator.call(callbackType.GetMethod("Call", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { outputParameterType.MakeByRefType() }, null));
                                    #endregion
                                    methodGenerator.MarkLabel(endfinallyLabel);
                                    methodGenerator.Emit(OpCodes.Endfinally);
                                    #region try end
                                    methodGenerator.EndExceptionBlock();
                                    #endregion
                                }
                                methodGenerator.MarkLabel(returnLabel);
                                #region return null;
                                if (method.IsKeepCallback) methodGenerator.Emit(OpCodes.Ldnull);
                                methodGenerator.Emit(OpCodes.Ret);
                                #endregion
                                if (method.IsKeepCallback)
                                {
                                    methodGenerator.MarkLabel(returnKeepCallbackLabel);
                                    #region return _keepCallback_;
                                    methodGenerator.Emit(OpCodes.Ldloc_S, keepCallbackLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                }
                            }
                            else
                            {
                                Label clientExceptionLabel = methodGenerator.DefineLabel(), returnLable = methodGenerator.DefineLabel(), returnReturnValueLable, returnValueLable;
                                LocalBuilder returnReturnValueLocalBuilder, returnValueLocalBuilder;
                                if (method.ReturnValueType == null)
                                {
                                    returnReturnValueLocalBuilder = null;
                                    returnReturnValueLable = default(Label);
                                }
                                else
                                {
                                    returnReturnValueLocalBuilder = methodGenerator.DeclareLocal(method.ReturnValueType);
                                    returnReturnValueLable = methodGenerator.DefineLabel();
                                }
                                if (method.ReturnValueType == null && method.ReturnType != typeof(void))
                                {
                                    returnValueLocalBuilder = methodGenerator.DeclareLocal(method.ReturnType);
                                    returnValueLable = methodGenerator.DefineLabel();
                                }
                                else
                                {
                                    returnValueLocalBuilder = null;
                                    returnValueLable = default(Label);
                                }
                                #region AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer.@OutputParameterTypeName> _wait_ = _TcpClient_.GetAutoWait<TcpInternalServer.@OutputParameterTypeName>();
                                //Type waitType = method.OutputParameterType == null ? typeof(AutoCSer.Net.TcpServer.AutoWaitReturnValue) : typeof(AutoCSer.Net.TcpServer.AutoWaitReturnValue<>).MakeGenericType(method.OutputParameterType.Type);
                                //LocalBuilder waitLocalBuilder = methodGenerator.DeclareLocal(waitType);
                                //methodGenerator.call(method.OutputParameterType == null ? ClientMetadata.AutoWaitReturnValuePopMethod : waitType.GetMethod("Pop", BindingFlags.Static | BindingFlags.Public));
                                //methodGenerator.Emit(OpCodes.Stloc_S, waitLocalBuilder);
                                LocalBuilder waitLocalBuilder;
                                if (method.OutputParameterType == null)
                                {
                                    waitLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.AutoWaitReturnValue));
                                    methodGenerator.call(ClientMetadata.AutoWaitReturnValuePopMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, waitLocalBuilder);
                                }
                                else
                                {
                                    AutoCSer.Metadata.GenericType GenericType = AutoCSer.Metadata.GenericType.Get(method.OutputParameterType.Type);
                                    waitLocalBuilder = methodGenerator.DeclareLocal(GenericType.TcpAutoWaitReturnValueType);
                                    methodGenerator.call(GenericType.TcpAutoWaitReturnValuePopMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, waitLocalBuilder);
                                }
                                #endregion
                                #region try
                                methodGenerator.BeginExceptionBlock();
                                #endregion
                                Label leaveTryLabel = methodGenerator.DefineLabel();
                                LocalBuilder senderLocalBuilder = methodGenerator.DeclareLocal(Metadata.SenderType);
                                parameterIndex = 0;
                                if (method.Attribute.IsVerifyMethod)
                                {
                                    foreach (ParameterInfo parameter in parameters)
                                    {
                                        ++parameterIndex;
                                        if (parameter.ParameterType == Metadata.SenderType)
                                        {
                                            #region AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                                            methodGenerator.ldarg(parameterIndex);
                                            methodGenerator.Emit(OpCodes.Stloc_S, senderLocalBuilder);
                                            #endregion
                                            parameterIndex = int.MinValue;
                                            break;
                                        }
                                    }
                                }
                                if (parameterIndex != int.MinValue)
                                {
                                    #region AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                                    methodGenerator.Emit(OpCodes.Ldarg_0);
                                    methodGenerator.call(Metadata.MethodClientGetTcpClientMethod);
                                    methodGenerator.call(Metadata.ClientGetSenderMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, senderLocalBuilder);
                                    #endregion
                                }
                                #region if (_socket_ != null)
                                methodGenerator.Emit(OpCodes.Ldloc_S, senderLocalBuilder);
                                methodGenerator.Emit(method.ParameterType == null ? OpCodes.Brfalse_S : OpCodes.Brfalse, leaveTryLabel);
                                #endregion
                                LocalBuilder inputParameterLocalBuilder;
                                if (method.ParameterType == null) inputParameterLocalBuilder = null;
                                else
                                {
                                    #region TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName { @ParameterName = @ParameterName };
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
                                        if (ParameterType.IsInputParameter(parameter))
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
                                    #region AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(@MethodIdentityCommand, ref _wait_, ref _inputParameter_);
                                    LocalBuilder returnTypeLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.ReturnType));
                                    methodGenerator.Emit(OpCodes.Ldloc_S, senderLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldsfld, commandInfoFieldBuilder);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, waitLocalBuilder);
                                    if (method.ParameterType == null) methodGenerator.call(Metadata.ClientSocketSenderWaitCallMethod);
                                    else
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                        //methodGenerator.call(Metadata.ClientSocketSenderWaitCallInputMethod.MakeGenericMethod(method.ParameterType.Type));
                                        methodGenerator.call(Metadata.GetParameterGenericType(method.ParameterType.Type).ClientSocketSenderWaitCallMethod);
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
                                        methodGenerator.Emit(OpCodes.Leave_S, returnLable);
                                        #endregion
                                        methodGenerator.MarkLabel(throwReturnTypeLabel);
                                        #region throw new Exception(AutoCSer.Net.TcpInternalServer.Emit.Client.ReturnTypeStrings[(byte)_returnType_]);
                                        methodGenerator.Emit(OpCodes.Ldsfld, ClientMetadata.ReturnTypeStringsField);
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldelem_Ref);
                                        methodGenerator.Emit(OpCodes.Newobj, AutoCSer.Extension.EmitGenerator.StringExceptionConstructor);
                                        methodGenerator.Emit(OpCodes.Throw);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region return new AutoCSer.Net.TcpServer.ReturnValue { Type = _returnType_ };
                                        //methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        //methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Stfld, ClientMetadata.ReturnValueTypeField);
                                        methodGenerator.Emit(OpCodes.Leave, returnReturnValueLable);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region TcpInternalServer.@OutputParameterTypeName _outputParameter_ = new TcpInternalServer.@OutputParameterTypeName { @ParameterName = @ParameterName };
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
                                    #region AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer.@InputParameterTypeName, TcpInternalServer.@OutputParameterTypeName>(@MethodIdentityCommand, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                                    LocalBuilder returnTypeLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.ReturnType));
                                    methodGenerator.Emit(OpCodes.Ldloc_S, senderLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldsfld, commandInfoFieldBuilder);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, waitLocalBuilder);
                                    if (method.ParameterType == null)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        //methodGenerator.call(Metadata.ClientSocketSenderWaitGetMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                        methodGenerator.call(Metadata.GetParameterGenericType(method.OutputParameterType.Type).ClientSocketSenderWaitGetMethod);
                                    }
                                    else
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        //methodGenerator.call(Metadata.ClientSocketSenderWaitGetInputMethod.MakeGenericMethod(method.ParameterType.Type, method.OutputParameterType.Type));
                                        methodGenerator.call(Metadata.GetParameterGenericType2(method.ParameterType.Type, method.OutputParameterType.Type).ClientSocketSenderWaitGetMethod);
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
                                            methodGenerator.Emit(OpCodes.Leave_S, returnLable);
                                            #endregion
                                        }
                                        else
                                        {
                                            #region return _outputParameter_.Ret;
                                            methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldfld, method.OutputParameterType.GetField(TcpServer.ReturnValue.RetParameterName));
                                            methodGenerator.Emit(OpCodes.Stloc_S, returnValueLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Leave_S, returnValueLable);
                                            #endregion
                                        }
                                        methodGenerator.MarkLabel(throwReturnTypeLabel);
                                        #region throw new Exception(AutoCSer.Net.TcpServer.Emit.Client.ReturnTypeStrings[(byte)_returnType_]);
                                        methodGenerator.Emit(OpCodes.Ldsfld, ClientMetadata.ReturnTypeStringsField);
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
                                        if (method.ReturnType != typeof(void) && (method.Attribute.IsInitobj || method.ReturnType.isInitobj()))
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                        }
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Stfld, method.ReturnType == typeof(void) ? ClientMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                        if (method.ReturnType != typeof(void))
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldfld, method.OutputParameterType.GetField(ReturnValue.RetParameterName));
                                            methodGenerator.Emit(OpCodes.Stfld, method.ReturnValueType.GetField("Value", BindingFlags.Instance | BindingFlags.Public));
                                        }
                                        methodGenerator.Emit(OpCodes.Leave, returnReturnValueLable);
                                        #endregion
                                    }
                                }
                                methodGenerator.MarkLabel(leaveTryLabel);
                                methodGenerator.Emit(OpCodes.Leave_S, clientExceptionLabel);
                                #region finally
                                methodGenerator.BeginFinallyBlock();
                                #endregion
                                Label endfinallyLabel = methodGenerator.DefineLabel();
                                #region if (_wait_ != null)
                                methodGenerator.Emit(OpCodes.Ldloc_S, waitLocalBuilder);
                                methodGenerator.Emit(OpCodes.Brfalse_S, endfinallyLabel);
                                #endregion
                                #region AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer.@OutputParameterTypeName>.PushNotNull(_wait_);
                                methodGenerator.Emit(OpCodes.Ldloc_S, waitLocalBuilder);
                                if (method.OutputParameterType == null) methodGenerator.call(ClientMetadata.AutoWaitReturnValuePushNotNullMethod);
                                else
                                {
                                    Type autoWaitReturnValueType = typeof(AutoCSer.Net.TcpServer.AutoWaitReturnValue<>).MakeGenericType(method.OutputParameterType.Type);
                                    methodGenerator.call(autoWaitReturnValueType.GetMethod(ClientMetadata.AutoWaitReturnValuePushNotNullMethod.Name, BindingFlags.Static | BindingFlags.Public, null, new Type[] { autoWaitReturnValueType }, null));
                                }
                                #endregion
                                methodGenerator.MarkLabel(endfinallyLabel);
                                methodGenerator.Emit(OpCodes.Endfinally);
                                #region try end
                                methodGenerator.EndExceptionBlock();
                                #endregion
                                methodGenerator.MarkLabel(clientExceptionLabel);
                                if (method.ReturnValueType == null)
                                {
                                    #region throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                                    methodGenerator.throwString(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                                    #endregion
                                    methodGenerator.MarkLabel(returnLable);
                                    methodGenerator.Emit(OpCodes.Ret);
                                    if (method.ReturnType != typeof(void))
                                    {
                                        methodGenerator.MarkLabel(returnValueLable);
                                        #region @MethodReturnType.FullName
                                        methodGenerator.Emit(OpCodes.Ldloc_S, returnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ret);
                                        #endregion
                                    }
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
                                    if (method.ReturnType != typeof(void) && (method.Attribute.IsInitobj || method.ReturnType.isInitobj()))
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.ReturnValueType);
                                    }
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnReturnValueLocalBuilder);
                                    methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ClientException);
                                    methodGenerator.Emit(OpCodes.Stfld, method.ReturnType == typeof(void) ? ClientMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(ClientMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                                    methodGenerator.MarkLabel(returnReturnValueLable);
                                    methodGenerator.Emit(OpCodes.Ldloc_S, returnReturnValueLocalBuilder);
                                    methodGenerator.MarkLabel(returnLable);
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
