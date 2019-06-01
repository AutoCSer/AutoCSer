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
        /// 创建 TCP 服务端
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct ServerBuilder
        {
            /// <summary>
            /// TCP 服务端元数据
            /// </summary>
            internal ServerMetadata Metadata;
            /// <summary>
            /// 服务端输出信息集合
            /// </summary>
            internal OutputInfo[] Outputs;
            /// <summary>
            /// 创建 TCP 服务端
            /// </summary>
            /// <param name="type"></param>
            /// <param name="attribute"></param>
            /// <param name="serverInterfaceType"></param>
            /// <param name="serverCallType"></param>
            /// <param name="constructorParameterTypes"></param>
            /// <param name="methods"></param>
            /// <returns></returns>
            internal Type Build(Type type, ServerAttribute attribute, Type serverInterfaceType, Type serverCallType, Type[] constructorParameterTypes, Method<attributeType, methodAttributeType, serverSocketSenderType>[] methods)
            {
                string serverIdentity = Interlocked.Increment(ref Metadata.Identity).toString();
                TypeBuilder typeBuilder = AutoCSer.Emit.Builder.Module.Builder.DefineType(Metadata.ServerTypeName + ".Emit." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, Metadata.ServerType);
                FieldBuilder valueFieldBuilder = typeBuilder.DefineField("_value_", typeof(object), FieldAttributes.Private | FieldAttributes.InitOnly);
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                Label serverAttributeNotNull = constructorGenerator.DefineLabel();
                bool isCallQueue = false;
                foreach (Method<attributeType, methodAttributeType, serverSocketSenderType> method in methods)
                {
                    if (method != null && (isCallQueue |= method.Attribute.ServerTaskType == ServerTaskType.Queue)) break;
                }
                #region base(attribute, verify, onCustomData, log, isCallQueue)
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Ldarg_S, 4);
                constructorGenerator.Emit(OpCodes.Ldarg_S, 5);
                constructorGenerator.int32(isCallQueue ? 1 : 0);
                constructorGenerator.Emit(OpCodes.Call, Metadata.ServerConstructorInfo);
                #endregion
                #region _value_ = value;
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_3);
                constructorGenerator.Emit(OpCodes.Stfld, valueFieldBuilder);
                #endregion
                constructorGenerator.Emit(OpCodes.Ret);

                ConstructorBuilder staticConstructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null);
                ILGenerator staticConstructorGenerator = staticConstructorBuilder.GetILGenerator();
                #region public override void DoCommand(int index, AutoCSer.Net.TcpInternalServer.ServerSocketSender socket, ref SubArray<byte> data)
                MethodBuilder doCommandMethodBuilder = typeBuilder.DefineMethod("DoCommand", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(void), Metadata.DoCommandParameterTypes);
                #endregion
                ILGenerator doCommandGenerator = doCommandMethodBuilder.GetILGenerator();
                doCommandGenerator.DeclareLocal(typeof(int));
                Label doCommandReturnLabel = doCommandGenerator.DefineLabel();
                Label[] doCommandLabels = new Label[methods.Length];
                int doCommandLabelIndex = 0;
                foreach (Method<attributeType, methodAttributeType, serverSocketSenderType> method in methods)
                {
                    doCommandLabels[doCommandLabelIndex++] = method == null ? doCommandReturnLabel : (method.DoCommandLabel = doCommandGenerator.DefineLabel());
                }
                #region switch (index - @CommandStartIndex)
                doCommandGenerator.Emit(OpCodes.Ldarg_1);
                doCommandGenerator.int32(TcpServer.Server.CommandStartIndex);
                doCommandGenerator.Emit(OpCodes.Sub);
                doCommandGenerator.Emit(OpCodes.Stloc_0);
                doCommandGenerator.Emit(OpCodes.Ldloc_0);
                doCommandGenerator.Emit(OpCodes.Switch, doCommandLabels);
                doCommandGenerator.MarkLabel(doCommandReturnLabel);
                doCommandGenerator.Emit(OpCodes.Ret);
                #endregion
                Outputs = new TcpServer.OutputInfo[methods.Length];
                FieldInfo outputsField = serverInterfaceType.GetField("Outputs", BindingFlags.Static | BindingFlags.NonPublic);
                foreach (Method<attributeType, methodAttributeType, serverSocketSenderType> method in methods)
                {
                    if (method != null)
                    {
                        FieldBuilder outputInfoFieldBuilder;
                        if (method.OutputParameterType == null && !method.IsAsynchronousCallback) outputInfoFieldBuilder = null;
                        else
                        {
                            TcpServer.OutputInfo outputInfo = new TcpServer.OutputInfo();
                            Outputs[method.Attribute.CommandIdentity] = outputInfo;
                            if (method.IsKeepCallback) outputInfo.IsKeepCallback = 1;
                            if (method.IsClientSendOnly) outputInfo.IsClientSendOnly = 1;
                            if (method.OutputParameterType != null)
                            {
                                outputInfo.OutputParameterIndex = method.OutputParameterType.Index;
                                if (attribute.IsSimpleSerialize) outputInfo.IsSimpleSerializeOutputParamter = method.OutputParameterType.IsSimpleSerialize && SimpleSerialize.Serializer.IsType(method.ReturnType);
                            }
                            outputInfo.IsBuildOutputThread = method.IsServerBuildOutputThread;
                            #region private static readonly AutoCSer.Net.TcpServer.OutputInfo @MethodIdentityCommand = AutoCSer.Net.TcpInternalServer.Emit.Server<interfaceType>.Outputs[@CommandIdentity];
                            staticConstructorGenerator.Emit(OpCodes.Ldsfld, outputsField);
                            staticConstructorGenerator.int32(method.Attribute.CommandIdentity);
                            staticConstructorGenerator.Emit(OpCodes.Ldelem_Ref);
                            staticConstructorGenerator.Emit(OpCodes.Stsfld, outputInfoFieldBuilder = typeBuilder.DefineField("_o" + method.Attribute.CommandIdentity.toString(), typeof(TcpServer.OutputInfo), FieldAttributes.Private | FieldAttributes.InitOnly | FieldAttributes.Static));
                            #endregion
                        }
                        ParameterInfo[] parameters = method.MethodInfo.GetParameters();
                        TypeBuilder serverCallTypeBuilder;
                        FieldInfo returnTypeField = method.ReturnValueType == null ? null : (method.ReturnValueType == typeof(TcpServer.ReturnValue) ? ServerMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(ServerMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                        FieldInfo returnValueField = method.ReturnValueType == null ? null : method.ReturnValueType.GetField("Value", BindingFlags.Instance | BindingFlags.Public);
                        if (method.IsMethodServerCall)
                        {
                            Type baseType = method.ParameterType == null ? Metadata.ServerCallType : serverCallType.MakeGenericType(method.ParameterType.Type);
                            FieldInfo inputParameterField = method.ParameterType == null ? null : baseType.GetField("inputParameter", BindingFlags.Instance | BindingFlags.NonPublic);
                            serverCallTypeBuilder = typeBuilder.DefineNestedType(Metadata.ServerCallTypeName + ".Emit." + serverIdentity + "_" + method.Attribute.CommandIdentity.toString(), TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.NestedPublic, baseType);
                            Type returnOutputType = method.OutputParameterType == null ? typeof(AutoCSer.Net.TcpServer.ReturnValue) : typeof(AutoCSer.Net.TcpServer.ReturnValue<>).MakeGenericType(method.OutputParameterType.Type);
                            FieldInfo returnOutputValueField = method.OutputParameterType == null ? null : returnOutputType.GetField("Value", BindingFlags.Instance | BindingFlags.Public);
                            Type returnOutputRefType = returnOutputType.MakeByRefType();
                            #region private void get(ref AutoCSer.Net.TcpServer.ReturnValue<@OutputParameterTypeName> value)
                            MethodBuilder getMethodBuilder = serverCallTypeBuilder.DefineMethod("get", MethodAttributes.Private, typeof(void), new Type[] { returnOutputRefType });
                            #endregion
                            ILGenerator getGenerator = getMethodBuilder.GetILGenerator();
                            Label getReturnLabel = getGenerator.DefineLabel();
                            LocalBuilder exceptionErrorLocalBuilder = getGenerator.DeclareLocal(typeof(Exception));
                            #region try
                            getGenerator.BeginExceptionBlock();
                            #endregion
                            #region @MethodReturnType.FullName @ReturnName = serverValue.@MethodName(Sender, inputParameter.@ParameterName);
                            LocalBuilder returnLocalBuilder = method.IsReturnType ? getGenerator.DeclareLocal(method.ReturnValueType ?? method.ReturnType) : null;
                            getGenerator.Emit(OpCodes.Ldarg_0);
                            getGenerator.Emit(OpCodes.Ldfld, Metadata.ServerCallServerValueField);
                            foreach (ParameterInfo parameter in parameters)
                            {
                                if (ParameterType.IsInputParameter(parameter))
                                {
                                    getGenerator.Emit(OpCodes.Ldarg_0);
                                    getGenerator.Emit(OpCodes.Ldflda, inputParameterField);
                                    getGenerator.Emit(parameter.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, method.ParameterType.GetField(parameter.Name));
                                }
                                else if (parameter.IsOut)
                                {
                                    getGenerator.Emit(OpCodes.Ldarg_1);
                                    getGenerator.Emit(OpCodes.Ldflda, returnOutputValueField);
                                    getGenerator.Emit(OpCodes.Ldflda, method.OutputParameterType.GetField(parameter.Name));
                                }
                                else if (parameter.ParameterType == Metadata.SenderType)
                                {
                                    getGenerator.Emit(OpCodes.Ldarg_0);
                                    getGenerator.Emit(OpCodes.Ldfld, Metadata.ServerCallSenderField);
                                }
                                else getGenerator.Emit(OpCodes.Ldnull);
                            }
                            getGenerator.call(method.MethodInfo);
                            if (method.IsReturnType) getGenerator.Emit(OpCodes.Stloc_S, returnLocalBuilder);
                            #endregion
                            FieldInfo returnValueTypeField = method.OutputParameterType == null ? ServerMetadata.ReturnValueTypeField : returnOutputType.GetField(ServerMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public);
                            if (method.OutputParameterType == null)
                            {
                                if (method.ReturnValueType != null)
                                {
                                    Label returnTypeErrorLabel = getGenerator.DefineLabel();
                                    #region if(@ReturnName.Type != AutoCSer.Net.TcpServer.ReturnType.Success)
                                    getGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                    getGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                    getGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Success);
                                    getGenerator.Emit(OpCodes.Beq_S, returnTypeErrorLabel);
                                    #endregion
                                    #region value.Type = @ReturnName.Type;
                                    getGenerator.Emit(OpCodes.Ldarg_1);
                                    getGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                    getGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                    getGenerator.Emit(OpCodes.Stfld, returnValueTypeField);
                                    #endregion
                                    getGenerator.Emit(OpCodes.Leave_S, getReturnLabel);
                                    getGenerator.MarkLabel(returnTypeErrorLabel);
                                }
                                #region value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                                getGenerator.Emit(OpCodes.Ldarg_1);
                                getGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Success);
                                getGenerator.Emit(OpCodes.Stfld, returnValueTypeField);
                                #endregion
                                getGenerator.Emit(OpCodes.Leave_S, getReturnLabel);
                            }
                            else
                            {
                                Label returnTypeErrorLabel;
                                if (method.ReturnValueType == null) returnTypeErrorLabel = default(Label);
                                else
                                {
                                    #region if(@ReturnName.Type != AutoCSer.Net.TcpServer.ReturnType.Success)
                                    getGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                    getGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                    getGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Success);
                                    getGenerator.Emit(OpCodes.Bne_Un, returnTypeErrorLabel = getGenerator.DefineLabel());
                                    #endregion
                                }
                                if (method.Attribute.IsVerifyMethod)
                                {
                                    Label verifyEndLabel = getGenerator.DefineLabel();
                                    #region if (@ReturnName / @ReturnName.Value)
                                    if (method.ReturnValueType == null) getGenerator.Emit(OpCodes.Ldloc_S, returnLocalBuilder);
                                    else
                                    {
                                        getGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                        getGenerator.Emit(OpCodes.Ldfld, returnValueField);
                                    }
                                    getGenerator.Emit(OpCodes.Brfalse_S, verifyEndLabel);
                                    #endregion
                                    #region Sender.SetVerifyMethod();
                                    getGenerator.Emit(OpCodes.Ldarg_0);
                                    getGenerator.Emit(OpCodes.Ldfld, Metadata.ServerCallSenderField);
                                    getGenerator.call(ServerMetadata.ServerSocketSenderSetVerifyMethod);
                                    #endregion
                                    getGenerator.MarkLabel(verifyEndLabel);
                                }
                                LocalBuilder valueLocalBuilder = getGenerator.DeclareLocal(method.OutputParameterType.Type);
                                #region value.Value.@ParameterName = inputParameter.@ParameterName;
                                foreach (ParameterInfo parameter in method.OutputParameters)
                                {
                                    if (!parameter.IsOut)
                                    {
                                        getGenerator.Emit(OpCodes.Ldarg_1);
                                        getGenerator.Emit(OpCodes.Ldflda, returnOutputValueField);
                                        getGenerator.Emit(OpCodes.Ldarg_0);
                                        getGenerator.Emit(OpCodes.Ldflda, inputParameterField);
                                        getGenerator.Emit(OpCodes.Ldfld, method.ParameterType.GetField(parameter.Name));
                                        getGenerator.Emit(OpCodes.Stfld, method.OutputParameterType.GetField(parameter.Name));
                                    }
                                }
                                #endregion
                                if (method.ReturnType != typeof(void))
                                {
                                    #region value.Value.@ReturnName = @ReturnName / @ReturnName.Value;
                                    getGenerator.Emit(OpCodes.Ldarg_1);
                                    getGenerator.Emit(OpCodes.Ldflda, returnOutputValueField);
                                    if (method.ReturnValueType == null) getGenerator.Emit(OpCodes.Ldloc_S, returnLocalBuilder);
                                    else
                                    {
                                        getGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                        getGenerator.Emit(OpCodes.Ldfld, returnValueField);
                                    }
                                    getGenerator.Emit(OpCodes.Stfld, method.OutputParameterType.GetField(TcpServer.ReturnValue.RetParameterName));
                                    #endregion
                                }
                                #region value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                                getGenerator.Emit(OpCodes.Ldarg_1);
                                getGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Success);
                                getGenerator.Emit(OpCodes.Stfld, returnValueTypeField);
                                #endregion
                                getGenerator.Emit(OpCodes.Leave_S, getReturnLabel);
                                if (method.ReturnValueType != null)
                                {
                                    getGenerator.MarkLabel(returnTypeErrorLabel);
                                    #region value.Type = @ReturnName.Type;
                                    getGenerator.Emit(OpCodes.Ldarg_1);
                                    getGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                    getGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                    getGenerator.Emit(OpCodes.Stfld, returnValueTypeField);
                                    #endregion
                                    getGenerator.Emit(OpCodes.Leave_S, getReturnLabel);
                                }
                            }
                            #region catch (Exception error)
                            getGenerator.BeginCatchBlock(typeof(Exception));
                            getGenerator.Emit(OpCodes.Stloc_S, exceptionErrorLocalBuilder);
                            #endregion
                            #region value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            getGenerator.Emit(OpCodes.Ldarg_1);
                            getGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ServerException);
                            getGenerator.Emit(OpCodes.Stfld, returnValueTypeField);
                            #endregion
                            #region Sender.Log(error);
                            getGenerator.Emit(OpCodes.Ldarg_0);
                            getGenerator.Emit(OpCodes.Ldfld, Metadata.ServerCallSenderField);
                            getGenerator.Emit(OpCodes.Ldloc_S, exceptionErrorLocalBuilder);
                            getGenerator.call(Metadata.ServerSocketSenderAddLogMethod);
                            #endregion
                            getGenerator.Emit(OpCodes.Leave_S, getReturnLabel);
                            #region try end
                            getGenerator.EndExceptionBlock();
                            #endregion
                            getGenerator.MarkLabel(getReturnLabel);
                            getGenerator.Emit(OpCodes.Ret);

                            #region public override void Call()
                            MethodBuilder callMethodBuilder = serverCallTypeBuilder.DefineMethod("Call", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(void), null);
                            #endregion
                            ILGenerator callGenerator = callMethodBuilder.GetILGenerator();
                            #region AutoCSer.Net.TcpServer.ReturnValue<@OutputParameterTypeName> value = new AutoCSer.Net.TcpServer.ReturnValue<@OutputParameterTypeName>();
                            LocalBuilder valueBuilder = callGenerator.DeclareLocal(returnOutputType);
                            if (method.OutputParameterType == null || method.Attribute.IsInitobj || method.OutputParameterType.IsInitobj)
                            {
                                callGenerator.Emit(OpCodes.Ldloca_S, valueBuilder);
                                callGenerator.Emit(OpCodes.Initobj, returnOutputType);
                            }
                            else
                            {
                                callGenerator.Emit(OpCodes.Ldloca_S, valueBuilder);
                                callGenerator.int32(0);
                                callGenerator.Emit(OpCodes.Stfld, returnValueTypeField);
                            }
                            #endregion
                            Label pushLabel = callGenerator.DefineLabel();
                            #region if (Sender.IsSocket)
                            callGenerator.Emit(OpCodes.Ldarg_0);
                            callGenerator.Emit(OpCodes.Ldfld, Metadata.ServerCallSenderField);
                            callGenerator.call(ServerMetadata.ServerSocketSenderGetIsSocketMethod);
                            callGenerator.Emit(OpCodes.Brfalse_S, pushLabel);
                            #endregion
                            #region get(ref value);
                            callGenerator.Emit(OpCodes.Ldarg_0);
                            callGenerator.Emit(OpCodes.Ldloca_S, valueBuilder);
                            callGenerator.call(getMethodBuilder);
                            #endregion
                            if (!method.IsClientSendOnly)
                            {
                                #region Sender.Push(CommandIndex, @MethodIdentityCommand, ref value);
                                callGenerator.Emit(OpCodes.Ldarg_0);
                                callGenerator.Emit(OpCodes.Ldfld, Metadata.ServerCallSenderField);
                                callGenerator.Emit(OpCodes.Ldarg_0);
                                callGenerator.Emit(OpCodes.Ldfld, ServerMetadata.ServerCallCommandIndexField);
                                if (method.OutputParameterType == null)
                                {
                                    callGenerator.Emit(OpCodes.Ldloca_S, valueBuilder);
                                    callGenerator.Emit(OpCodes.Ldfld, returnValueTypeField);
                                    callGenerator.call(method.IsServerBuildOutputThread ? Metadata.ServerSocketSenderPushReturnValueMethod : Metadata.ServerSocketSenderPushNoThreadMethod);
                                }
                                else
                                {
                                    callGenerator.Emit(OpCodes.Ldsfld, outputInfoFieldBuilder);
                                    callGenerator.Emit(OpCodes.Ldloca_S, valueBuilder);
                                    //callGenerator.call(Metadata.ServerSocketSenderPushOutputRefMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                    callGenerator.call(Metadata.GetParameterGenericType(method.OutputParameterType.Type).ServerSocketSenderPushCommandMethod);
                                }
                                callGenerator.Emit(OpCodes.Pop);
                                #endregion
                            }
                            callGenerator.MarkLabel(pushLabel);
                            #region push(this);
                            callGenerator.Emit(OpCodes.Ldarg_0);
                            callGenerator.Emit(OpCodes.Ldarg_0);
                            callGenerator.call((method.ParameterType == null ? Metadata.ServerCallPushMethod : serverCallType.MakeGenericType(method.ParameterType.Type).GetMethod("push", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)).MakeGenericMethod(serverCallTypeBuilder));
                            #endregion
                            callGenerator.Emit(OpCodes.Ret);
                            serverCallTypeBuilder.CreateType();
                        }
                        else serverCallTypeBuilder = null;

                        #region private void @MethodIndexName(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, ref SubArray<byte> data)
                        MethodBuilder methodBuilder = typeBuilder.DefineMethod("_m" + method.Attribute.CommandIdentity.toString(), MethodAttributes.Private, typeof(void), Metadata.MethodParameterTypes);
                        #endregion
                        ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                        if (method.Attribute.IsExpired)
                        {
                            if (!method.IsClientSendOnly)
                            {
                                #region sender.Push(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                methodGenerator.call(Metadata.ServerSocketSenderPushReturnTypeMethod);
                                methodGenerator.Emit(OpCodes.Pop);
                                #endregion
                            }
                        }
                        else
                        {
                            Label pushLabel = methodGenerator.DefineLabel(), returnLable = methodGenerator.DefineLabel();
                            LocalBuilder returnTypeLocalBuilder;
                            if (method.IsClientSendOnly) returnTypeLocalBuilder = null;
                            else
                            {
                                #region AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                                returnTypeLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.ReturnType));
                                methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Unknown);
                                methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                                #endregion
                            }
                            #region try
                            methodGenerator.BeginExceptionBlock();
                            #endregion
                            LocalBuilder parameterLocalBuilder, outputParameterLocalBuilder = null;
                            Label serverDeSerializeErrorLabel;
                            if (method.ParameterType == null)
                            {
                                parameterLocalBuilder = null;
                                serverDeSerializeErrorLabel = default(Label);
                            }
                            else
                            {
                                #region @InputParameterTypeName inputParameter = new @InputParameterTypeName();
                                parameterLocalBuilder = methodGenerator.DeclareLocal(method.ParameterType.Type);
                                if (method.Attribute.IsInitobj || method.ParameterType.IsInitobj)
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Initobj, method.ParameterType.Type);
                                }
                                #endregion
                                #region if (sender.DeSerialize(ref data, ref inputParameter))
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.Emit(OpCodes.Ldarg_2);
                                methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                methodGenerator.int32(method.ParameterType.IsSimpleSerialize ? 1 : 0);
                                methodGenerator.call(ServerMetadata.ServerSocketSenderDeSerializeMethod.MakeGenericMethod(method.ParameterType.Type));
                                methodGenerator.Emit(OpCodes.Brfalse, serverDeSerializeErrorLabel = methodGenerator.DefineLabel());
                                #endregion
                            }
                            if (method.IsAsynchronousCallback)
                            {
                                if (method.ReturnType != typeof(void))
                                {
                                    #region @OutputParameterTypeName outputParameter = new @OutputParameterTypeName();
                                    outputParameterLocalBuilder = methodGenerator.DeclareLocal(method.OutputParameterType.Type);
                                    if (method.Attribute.IsInitobj || method.OutputParameterType.IsInitobj)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.OutputParameterType.Type);
                                    }
                                    #endregion
                                }
                                #region _value_.@MethodName(sender, inputParameter.@ParameterName, sender.GetCallback<@OutputParameterTypeName, @MethodReturnType.FullName>(@MethodIdentityCommand, ref outputParameter));
                                methodGenerator.Emit(OpCodes.Ldarg_0);
                                methodGenerator.Emit(OpCodes.Ldfld, valueFieldBuilder);
                                int parameterCount = parameters.Length;
                                foreach (ParameterInfo parameter in parameters)
                                {
                                    if (--parameterCount == 0) break;
                                    if (ParameterType.IsInputParameter(parameter))
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                        methodGenerator.Emit(parameter.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, method.ParameterType.GetField(parameter.Name));
                                    }
                                    else if (parameter.IsOut)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldflda, method.OutputParameterType.GetField(parameter.Name));
                                    }
                                    else if (parameter.ParameterType == Metadata.SenderType) methodGenerator.Emit(OpCodes.Ldarg_1);
                                    else methodGenerator.Emit(OpCodes.Ldnull);
                                }
                                if (method.ReturnType == typeof(void))
                                {
                                    methodGenerator.Emit(OpCodes.Ldarg_1);
                                    methodGenerator.Emit(OpCodes.Ldsfld, outputInfoFieldBuilder);
                                    methodGenerator.call(Metadata.ServerSocketSenderGetCallbackMethod);
                                }
                                else
                                {
                                    methodGenerator.Emit(OpCodes.Ldarg_1);
                                    methodGenerator.Emit(OpCodes.Ldsfld, outputInfoFieldBuilder);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                    //methodGenerator.call(Metadata.ServerSocketSenderGetCallbackReturnMethod.MakeGenericMethod(method.OutputParameterType.Type, method.ReturnType));
                                    methodGenerator.call(Metadata.GetOutputParameterGenericType(method.ReturnType, method.OutputParameterType.Type).ServerSocketSenderGetCallbackMethod);
                                    if (method.ReturnValueType == null)
                                    {
                                        //methodGenerator.call(typeof(ServerCallback<>).MakeGenericType(method.ReturnType).GetMethod("Get", BindingFlags.Static | BindingFlags.Public));
                                        methodGenerator.call(AutoCSer.Metadata.GenericType.Get(method.ReturnType).TcpServerCallbackGetMethod);
                                    }
                                }
                                methodGenerator.call(method.MethodInfo);
                                if (method.IsKeepCallback) methodGenerator.Emit(OpCodes.Pop);
                                #endregion
                            }
                            else if (method.IsMethodServerCall)
                            {
                                #region @MethodStreamName.Call(sender, _value_, @ServerTask, ref inputParameter);
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.Emit(OpCodes.Ldarg_0);
                                methodGenerator.Emit(OpCodes.Ldfld, valueFieldBuilder);
                                methodGenerator.int32((byte)method.ServerTask);
                                if (method.ParameterType == null) methodGenerator.call(Metadata.ServerCallCallMethod.MakeGenericMethod(serverCallTypeBuilder));
                                else
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                    methodGenerator.call(serverCallType.MakeGenericType(method.ParameterType.Type).GetMethod("Call", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).MakeGenericMethod(serverCallTypeBuilder));
                                }
                                #endregion
                            }
                            else
                            {
                                if (method.OutputParameterType != null)
                                {
                                    #region @OutputParameterTypeName outputParameter = new @OutputParameterTypeName();
                                    outputParameterLocalBuilder = methodGenerator.DeclareLocal(method.OutputParameterType.Type);
                                    if (method.Attribute.IsInitobj || method.OutputParameterType.IsInitobj)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Initobj, method.OutputParameterType.Type);
                                    }
                                    #endregion
                                }
                                #region @MethodReturnType.FullName @ReturnName = _value_.@MethodName(sender, inputParameter.@ParameterName);
                                LocalBuilder returnLocalBuilder = method.IsReturnType ? methodGenerator.DeclareLocal(method.ReturnValueType ?? method.ReturnType) : null;
                                methodGenerator.Emit(OpCodes.Ldarg_0);
                                methodGenerator.Emit(OpCodes.Ldfld, valueFieldBuilder);
                                foreach (ParameterInfo parameter in parameters)
                                {
                                    if (ParameterType.IsInputParameter(parameter))
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                        methodGenerator.Emit(parameter.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, method.ParameterType.GetField(parameter.Name));
                                    }
                                    else if (parameter.IsOut)
                                    {
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldflda, method.OutputParameterType.GetField(parameter.Name));
                                    }
                                    else if (parameter.ParameterType == Metadata.SenderType) methodGenerator.Emit(OpCodes.Ldarg_1);
                                    else methodGenerator.Emit(OpCodes.Ldnull);
                                }
                                methodGenerator.call(method.MethodInfo);
                                if (method.IsReturnType) methodGenerator.Emit(OpCodes.Stloc_S, returnLocalBuilder);
                                #endregion
                                if (method.OutputParameterType == null)
                                {
                                    if (method.ReturnValueType == null)
                                    {
                                        if (!method.IsClientSendOnly)
                                        {
                                            #region sender.Push();
                                            methodGenerator.Emit(OpCodes.Ldarg_1);
                                            methodGenerator.call(Metadata.ServerSocketSenderPushMethod);
                                            methodGenerator.Emit(OpCodes.Pop);
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        #region sender.Push(@ReturnName.Type);
                                        methodGenerator.Emit(OpCodes.Ldarg_1);
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                        methodGenerator.call(Metadata.ServerSocketSenderPushReturnTypeMethod);
                                        methodGenerator.Emit(OpCodes.Pop);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    Label returnTypeErrorLabel;
                                    if (method.ReturnValueType == null) returnTypeErrorLabel = default(Label);
                                    else
                                    {
                                        #region if(@ReturnName.Type == AutoCSer.Net.TcpServer.ReturnType.Success)
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                        methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Success);
                                        methodGenerator.Emit(OpCodes.Bne_Un, returnTypeErrorLabel = methodGenerator.DefineLabel());
                                        #endregion
                                    }
                                    if (method.Attribute.IsVerifyMethod)
                                    {
                                        Label verifyEndLabel = methodGenerator.DefineLabel();
                                        #region if (@ReturnName / @ReturnName.Value)
                                        if (method.ReturnValueType == null) methodGenerator.Emit(OpCodes.Ldloc_S, returnLocalBuilder);
                                        else
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldfld, returnValueField);
                                        }
                                        methodGenerator.Emit(OpCodes.Brfalse_S, verifyEndLabel);
                                        #endregion
                                        #region sender.SetVerifyMethod();
                                        methodGenerator.Emit(OpCodes.Ldarg_1);
                                        methodGenerator.call(ServerMetadata.ServerSocketSenderSetVerifyMethod);
                                        #endregion
                                        methodGenerator.MarkLabel(verifyEndLabel);
                                    }
                                    foreach (ParameterInfo parameter in method.OutputParameters)
                                    {
                                        if (!parameter.IsOut)
                                        {
                                            #region outputParameter.@ParameterName = inputParameter.@ParameterName
                                            methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldfld, method.ParameterType.GetField(parameter.Name));
                                            methodGenerator.Emit(OpCodes.Stfld, method.OutputParameterType.GetField(parameter.Name));
                                            #endregion
                                        }
                                    }
                                    if (method.ReturnType != typeof(void))
                                    {
                                        #region _outputParameter_.Ret = @ReturnName / @ReturnName.Value
                                        methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                        if (method.ReturnValueType == null) methodGenerator.Emit(OpCodes.Ldloc_S, returnLocalBuilder);
                                        else
                                        {
                                            methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                            methodGenerator.Emit(OpCodes.Ldfld, returnValueField);
                                        }
                                        methodGenerator.Emit(OpCodes.Stfld, method.OutputParameterType.GetField(TcpServer.ReturnValue.RetParameterName));
                                        #endregion
                                    }

                                    #region sender.Push(@MethodIdentityCommand, ref outputParameter)
                                    methodGenerator.Emit(OpCodes.Ldarg_1);
                                    methodGenerator.Emit(OpCodes.Ldsfld, outputInfoFieldBuilder);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                    //methodGenerator.call(Metadata.ServerSocketSenderPushOutputMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                    methodGenerator.call(Metadata.GetParameterGenericType(method.OutputParameterType.Type).ServerSocketSenderPushMethod);
                                    methodGenerator.Emit(OpCodes.Pop);
                                    #endregion
                                    methodGenerator.Emit(OpCodes.Leave_S, returnLable);
                                    if (method.ReturnValueType != null)
                                    {
                                        methodGenerator.MarkLabel(returnTypeErrorLabel);
                                        #region sender.Push(@ReturnValue.Type);
                                        methodGenerator.Emit(OpCodes.Ldarg_1);
                                        methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                        methodGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                        methodGenerator.call(Metadata.ServerSocketSenderPushReturnTypeMethod);
                                        methodGenerator.Emit(OpCodes.Pop);
                                        #endregion
                                    }
                                }
                            }
                            methodGenerator.Emit(OpCodes.Leave_S, returnLable);
                            if (method.ParameterType != null)
                            {
                                methodGenerator.MarkLabel(serverDeSerializeErrorLabel);
                                if (!method.IsClientSendOnly)
                                {
                                    #region returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                                    methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError);
                                    methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                                    #endregion
                                }
                                methodGenerator.Emit(OpCodes.Leave_S, pushLabel);
                            }
                            #region catch (Exception error)
                            methodGenerator.BeginCatchBlock(typeof(Exception));
                            LocalBuilder errorLocalBuilder = methodGenerator.DeclareLocal(typeof(Exception));
                            methodGenerator.Emit(OpCodes.Stloc_S, errorLocalBuilder);
                            #endregion
                            if (!method.IsClientSendOnly)
                            {
                                #region returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ServerException);
                                methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                                #endregion
                            }
                            #region sender.Log(error);
                            methodGenerator.Emit(OpCodes.Ldarg_1);
                            methodGenerator.Emit(OpCodes.Ldloc_S, errorLocalBuilder);
                            methodGenerator.call(Metadata.ServerSocketSenderAddLogMethod);
                            #endregion
                            methodGenerator.Emit(OpCodes.Leave_S, pushLabel);
                            #region try end
                            methodGenerator.EndExceptionBlock();
                            #endregion
                            methodGenerator.MarkLabel(pushLabel);
                            if (!method.IsClientSendOnly)
                            {
                                #region sender.Push(returnType);
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                methodGenerator.call(Metadata.ServerSocketSenderPushReturnTypeMethod);
                                methodGenerator.Emit(OpCodes.Pop);
                                #endregion
                            }
                            methodGenerator.MarkLabel(returnLable);
                        }
                        methodGenerator.Emit(OpCodes.Ret);

                        #region case @MethodIndex: @MethodIndexName(socket, ref data);
                        doCommandGenerator.MarkLabel(method.DoCommandLabel);
                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                        doCommandGenerator.Emit(OpCodes.Ldarg_2);
                        doCommandGenerator.Emit(OpCodes.Ldarg_3);
                        doCommandGenerator.call(methodBuilder);
                        #endregion
                        doCommandGenerator.Emit(OpCodes.Ret);
                        method.DoCommandLabel = default(Label);
                    }
                }
                doCommandGenerator.Emit(OpCodes.Ret);
                staticConstructorGenerator.Emit(OpCodes.Ret);

                return typeBuilder.CreateType();
            }
        }
    }
}
