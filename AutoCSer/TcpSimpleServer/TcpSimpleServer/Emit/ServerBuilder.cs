using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    internal sealed partial class Method<attributeType, methodAttributeType, serverSocketType>
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
            /// <param name="constructorParameterTypes"></param>
            /// <param name="methods"></param>
            /// <returns></returns>
            internal Type Build(Type type, ServerAttribute attribute, Type serverInterfaceType, Type[] constructorParameterTypes, Method<attributeType, methodAttributeType, serverSocketType>[] methods)
            {
                string serverIdentity = Interlocked.Increment(ref Metadata.Identity).toString();
                TypeBuilder typeBuilder = AutoCSer.Emit.Builder.Module.Builder.DefineType(Metadata.ServerTypeName + ".Emit." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, Metadata.ServerType);
                FieldBuilder valueFieldBuilder = typeBuilder.DefineField("_value_", typeof(object), FieldAttributes.Private | FieldAttributes.InitOnly);
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                Label serverAttributeNotNull = constructorGenerator.DefineLabel();
                bool isSynchronousVerifyMethod = false;
                foreach (Method<attributeType, methodAttributeType, serverSocketType> method in methods)
                {
                    if (method != null && method.Attribute.IsVerifyMethod)
                    {
                        if (method.Attribute.ServerTaskType == TcpServer.ServerTaskType.Synchronous && !method.IsAsynchronousCallback) isSynchronousVerifyMethod = true;
                        break;
                    }
                }
                #region base(attribute, verify, log, isCallQueue, isSynchronousVerifyMethod)
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Ldarg_S, 4);
                constructorGenerator.int32(isSynchronousVerifyMethod ? 1 : 0);
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
                #region public override bool DoCommand(int index, AutoCSer.Net.TcpInternalSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                MethodBuilder doCommandMethodBuilder = typeBuilder.DefineMethod("DoCommand", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(bool), Metadata.DoCommandParameterTypes);
                #endregion
                ILGenerator doCommandGenerator = doCommandMethodBuilder.GetILGenerator();
                doCommandGenerator.DeclareLocal(typeof(int));
                Label doCommandReturnLabel = doCommandGenerator.DefineLabel();
                Label[] doCommandLabels = new Label[methods.Length];
                int doCommandLabelIndex = 0;
                foreach (Method<attributeType, methodAttributeType, serverSocketType> method in methods)
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
                doCommandGenerator.int32(0);
                doCommandGenerator.Emit(OpCodes.Ret);
                #endregion
                Outputs = new TcpSimpleServer.OutputInfo[methods.Length];
                FieldInfo outputsField = serverInterfaceType.GetField("Outputs", BindingFlags.Static | BindingFlags.NonPublic);
                foreach (Method<attributeType, methodAttributeType, serverSocketType> method in methods)
                {
                    if (method != null)
                    {
                        FieldBuilder outputInfoFieldBuilder;
                        if (method.OutputParameterType == null && !method.IsAsynchronousCallback) outputInfoFieldBuilder = null;
                        else
                        {
                            TcpSimpleServer.OutputInfo outputInfo = new TcpSimpleServer.OutputInfo();
                            Outputs[method.Attribute.CommandIdentity] = outputInfo;
                            if (method.OutputParameterType != null)
                            {
                                outputInfo.OutputParameterIndex = method.OutputParameterType.Index;
                                if (attribute.IsSimpleSerialize) outputInfo.IsSimpleSerializeOutputParamter = method.OutputParameterType.IsSimpleSerialize && SimpleSerialize.Serializer.IsType(method.ReturnType);
                            }
                            #region private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo @MethodIdentityCommand = AutoCSer.Net.TcpInternalSimpleServer.Emit.Server<interfaceType>.Outputs[@CommandIdentity];
                            staticConstructorGenerator.Emit(OpCodes.Ldsfld, outputsField);
                            staticConstructorGenerator.int32(method.Attribute.CommandIdentity);
                            staticConstructorGenerator.Emit(OpCodes.Ldelem_Ref);
                            staticConstructorGenerator.Emit(OpCodes.Stsfld, outputInfoFieldBuilder = typeBuilder.DefineField("_o" + method.Attribute.CommandIdentity.toString(), typeof(TcpServer.OutputInfo), FieldAttributes.Private | FieldAttributes.InitOnly | FieldAttributes.Static));
                            #endregion
                        }
                        ParameterInfo[] parameters = method.MethodInfo.GetParameters();
                        FieldInfo returnTypeField = method.ReturnValueType == null ? null : (method.ReturnValueType == typeof(TcpServer.ReturnValue) ? TcpServer.Emit.ServerMetadata.ReturnValueTypeField : method.ReturnValueType.GetField(TcpServer.Emit.ServerMetadata.ReturnValueTypeField.Name, BindingFlags.Instance | BindingFlags.Public));
                        FieldInfo returnValueField = method.ReturnValueType == null ? null : method.ReturnValueType.GetField("Value", BindingFlags.Instance | BindingFlags.Public);

                        #region private bool @MethodIndexName(AutoCSer.Net.TcpInternalSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                        MethodBuilder methodBuilder = typeBuilder.DefineMethod("_m" + method.Attribute.CommandIdentity.toString(), MethodAttributes.Private, typeof(bool), Metadata.MethodParameterTypes);
                        #endregion
                        ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                        if (method.Attribute.IsExpired)
                        {
                            if (method.OutputParameterType == null)
                            {
                                #region return socket.Send(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                methodGenerator.call(Metadata.ServerSocketSendReturnTypeMethod);
                                methodGenerator.Emit(OpCodes.Ret);
                                #endregion
                            }
                            else
                            {
                                #region return socket.SendOutput(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                                methodGenerator.call(Metadata.ServerSocketSendOutputReturnTypeMethod);
                                methodGenerator.Emit(OpCodes.Ret);
                                #endregion
                            }
                        }
                        else
                        {
                            Label pushLabel = methodGenerator.DefineLabel(), returnLable = methodGenerator.DefineLabel();
                            #region bool isReturnLocalBuilder = false;
                            LocalBuilder isReturnLocalBuilder = methodGenerator.DeclareLocal(typeof(bool));
                            methodGenerator.int32(0);
                            methodGenerator.Emit(OpCodes.Stloc_S, isReturnLocalBuilder);
                            #endregion
                            #region AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            LocalBuilder returnTypeLocalBuilder = methodGenerator.DeclareLocal(typeof(AutoCSer.Net.TcpServer.ReturnType));
                            methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.Unknown);
                            methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                            #endregion
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
                                #region if (socket.DeSerialize(ref data, ref inputParameter))
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.Emit(OpCodes.Ldarg_2);
                                methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                methodGenerator.int32(method.ParameterType.IsSimpleSerialize ? 1 : 0);
                                methodGenerator.call(Metadata.GetParameterGenericType(method.ParameterType.Type).ServerSocketDeSerializeMethod);
                                methodGenerator.Emit(OpCodes.Brfalse, serverDeSerializeErrorLabel = methodGenerator.DefineLabel());
                                #endregion
                            }

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
                            #region @MethodReturnType.FullName @ReturnName = _value_.@MethodName(socket, inputParameter.@ParameterName);
                            LocalBuilder returnLocalBuilder = method.IsReturnType ? methodGenerator.DeclareLocal(method.ReturnValueType ?? method.ReturnType) : null;
                            methodGenerator.Emit(OpCodes.Ldarg_0);
                            methodGenerator.Emit(OpCodes.Ldfld, valueFieldBuilder);
                            foreach (ParameterInfo parameter in parameters)
                            {
                                if (SimpleParameterType.IsInputParameter(parameter))
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, parameterLocalBuilder);
                                    methodGenerator.Emit(parameter.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, method.ParameterType.GetField(parameter.Name));
                                }
                                else if (parameter.IsOut)
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldflda, method.OutputParameterType.GetField(parameter.Name));
                                }
                                else if (parameter.ParameterType == Metadata.SocketType) methodGenerator.Emit(OpCodes.Ldarg_1);
                                else methodGenerator.Emit(OpCodes.Ldnull);
                            }
                            methodGenerator.call(method.MethodInfo);
                            if (method.IsReturnType) methodGenerator.Emit(OpCodes.Stloc_S, returnLocalBuilder);
                            #endregion
                            if (method.OutputParameterType == null)
                            {
                                if (method.ReturnValueType == null)
                                {
                                    #region return socket.Send();
                                    methodGenerator.Emit(OpCodes.Ldarg_1);
                                    methodGenerator.call(Metadata.ServerSocketSendMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, isReturnLocalBuilder);
                                    #endregion
                                }
                                else
                                {
                                    #region return socket.Send(@ReturnName.Type);
                                    methodGenerator.Emit(OpCodes.Ldarg_1);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                    methodGenerator.call(Metadata.ServerSocketSendReturnTypeMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, isReturnLocalBuilder);
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
                                    #region socket.SetVerifyMethod();
                                    methodGenerator.Emit(OpCodes.Ldarg_1);
                                    methodGenerator.call(ServerMetadata.ServerSocketSetVerifyMethodMethod);
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

                                #region return socket.Send(@MethodIdentityCommand, ref outputParameter)
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.Emit(OpCodes.Ldsfld, outputInfoFieldBuilder);
                                methodGenerator.Emit(OpCodes.Ldloca_S, outputParameterLocalBuilder);
                                //methodGenerator.call(Metadata.ServerSocketSendOutputMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                methodGenerator.call(Metadata.GetParameterGenericType(method.OutputParameterType.Type).ServerSocketSendParameterMethod);
                                methodGenerator.Emit(OpCodes.Stloc_S, isReturnLocalBuilder);
                                #endregion
                                methodGenerator.Emit(OpCodes.Leave_S, returnLable);
                                if (method.ReturnValueType != null)
                                {
                                    methodGenerator.MarkLabel(returnTypeErrorLabel);
                                    #region return socket.SendOutput(@ReturnValue.Type);
                                    methodGenerator.Emit(OpCodes.Ldarg_1);
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Ldfld, returnTypeField);
                                    methodGenerator.call(Metadata.ServerSocketSendOutputReturnTypeMethod);
                                    methodGenerator.Emit(OpCodes.Stloc_S, isReturnLocalBuilder);
                                    #endregion
                                }
                            }
                            methodGenerator.Emit(OpCodes.Leave_S, returnLable);
                            if (method.ParameterType != null)
                            {
                                methodGenerator.MarkLabel(serverDeSerializeErrorLabel);
                                #region returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                                methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError);
                                methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                                #endregion
                                methodGenerator.Emit(OpCodes.Leave_S, pushLabel);
                            }
                            #region catch (Exception error)
                            methodGenerator.BeginCatchBlock(typeof(Exception));
                            LocalBuilder errorLocalBuilder = methodGenerator.DeclareLocal(typeof(Exception));
                            methodGenerator.Emit(OpCodes.Stloc_S, errorLocalBuilder);
                            #endregion
                            #region returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            methodGenerator.int32((byte)AutoCSer.Net.TcpServer.ReturnType.ServerException);
                            methodGenerator.Emit(OpCodes.Stloc_S, returnTypeLocalBuilder);
                            #endregion
                            #region socket.Log(error);
                            methodGenerator.Emit(OpCodes.Ldarg_1);
                            methodGenerator.Emit(OpCodes.Ldloc_S, errorLocalBuilder);
                            methodGenerator.call(Metadata.ServerSocketLogMethod);
                            #endregion
                            methodGenerator.Emit(OpCodes.Leave_S, pushLabel);
                            #region try end
                            methodGenerator.EndExceptionBlock();
                            #endregion
                            methodGenerator.MarkLabel(pushLabel);
                            if (method.OutputParameterType == null)
                            {
                                #region return socket.Send(returnType);
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                methodGenerator.call(Metadata.ServerSocketSendReturnTypeMethod);
                                methodGenerator.Emit(OpCodes.Ret);
                                #endregion
                            }
                            else
                            {
                                #region return socket.SendOutput(returnType);
                                methodGenerator.Emit(OpCodes.Ldarg_1);
                                methodGenerator.Emit(OpCodes.Ldloc_S, returnTypeLocalBuilder);
                                methodGenerator.call(Metadata.ServerSocketSendOutputReturnTypeMethod);
                                methodGenerator.Emit(OpCodes.Ret);
                                #endregion
                            }
                            methodGenerator.MarkLabel(returnLable);
                            methodGenerator.Emit(OpCodes.Ldloc_S, isReturnLocalBuilder);
                            methodGenerator.Emit(OpCodes.Ret);
                        }

                        #region case @MethodIndex: return @MethodIndexName(socket, ref data);
                        doCommandGenerator.MarkLabel(method.DoCommandLabel);
                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                        doCommandGenerator.Emit(OpCodes.Ldarg_2);
                        doCommandGenerator.Emit(OpCodes.Ldarg_3);
                        doCommandGenerator.call(methodBuilder);
                        doCommandGenerator.Emit(OpCodes.Ret);
                        #endregion
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
