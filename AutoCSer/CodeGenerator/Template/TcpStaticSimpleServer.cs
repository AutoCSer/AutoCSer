using System;
#pragma warning disable 649
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class TcpStaticSimpleServer : Pub
    {
        #region NOTE
        private static FullName[] MethodIndexs = null;
        private static FullName ParameterName = null;
        private static FullName ParameterJoinRefName = null;
        private static ParameterTypeRefName ParameterRefName = null;
        private const int MethodIndex = 0;
        private const int CommandStartIndex = 0;
        private const int InputParameterIndex = 0;
        private const int OutputParameterIndex = 0;
        private const AutoCSer.Net.TcpServer.ServerTaskType ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout;
        private const bool IsCallQueue = false;
        #endregion NOTE

        #region PART CLASS
        #region NOT IsAllType
        #region PART SERVERCALL
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            internal static partial class TcpStaticSimpleServer
            {
                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                #region IF IsAsynchronousCallback
                public static void @StaticMethodIndexName(/*IF:ClientParameterName*/AutoCSer.Net.TcpInternalSimpleServer.ServerSocket _socket_, /*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Func<AutoCSer.Net.TcpServer.ReturnValue</*PUSH:Method*/@MethodReturnType.FullName/*PUSH:Method*/>, bool> _onReturn_)
                {
                    @Type.FullName/*PUSH:Method*/.@StaticMethodName(/*IF:ClientParameterName*/_socket_, /*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterRefName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/_onReturn_);/*PUSH:Method*/
                }
                #endregion IF IsAsynchronousCallback
                #region NOT IsAsynchronousCallback
                public static /*PUSH:Method*/@MethodReturnType.FullName/*PUSH:Method*/ @StaticMethodIndexName(/*IF:ClientParameterName*/AutoCSer.Net.TcpInternalSimpleServer.ServerSocket _socket_/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    #region IF MemberIndex
                    #region NOT Method.IsGetMember
                    @Type.FullName/**/.@StaticPropertyName = /*LOOP:Method.Parameters*/@ParameterName/*LOOP:Method.Parameters*/;
                    #endregion NOT Method.IsGetMember
                    #region IF Method.IsGetMember
                    return /*NOTE*/(FullName)/*NOTE*/@Type.FullName/**/.@StaticPropertyName;
                    #endregion IF Method.IsGetMember
                    #endregion IF MemberIndex

                    #region NOT MemberIndex
                    /*IF:MethodIsReturn*/
                    return /*NOTE*/(FullName)/*NOTE*//*IF:MethodIsReturn*/@Type.FullName/*PUSH:Method*/.@StaticMethodName(/*IF:ClientParameterName*/_socket_/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterJoinRefName/*PUSH:MethodParameter*//*LOOP:InputParameters*/);/*PUSH:Method*/
                    #endregion NOT MemberIndex
                }
                #endregion NOT IsAsynchronousCallback
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
            }
        }
        #endregion PART SERVERCALL
        #region PART CLIENTCALL
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            #region IF Type.XmlDocument
            /// <summary>
            /// @Type.XmlDocument
            /// </summary>
            #endregion IF Type.XmlDocument
            public /*NOTE*/partial class /*NOTE*/@NoAccessTypeNameDefinition
            {
                #region NOTE
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks) { return false; }
                #endregion NOTE
                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase @StaticMethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };

                #region NOT MemberIndex
                #region IF Method.XmlDocument
                /// <summary>
                /// @Method.XmlDocument
                /// </summary>
                #endregion IF Method.XmlDocument
                #region LOOP InputParameters
                #region PUSH MethodParameter
                #region IF XmlDocument
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion IF XmlDocument
                #endregion PUSH MethodParameter
                #endregion LOOP InputParameters
                #region IF Method.ReturnXmlDocument
                /// <returns>@Method.ReturnXmlDocument</returns>
                #endregion IF Method.ReturnXmlDocument
                public static AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    #region IF Attribute.IsExpired
                    #region LOOP InputParameters
                    #region PUSH MethodParameter
                    #region IF IsOut
                    @ParameterName = default(@ParameterType.FullName);
                    #endregion IF IsOut
                    #endregion PUSH MethodParameter
                    #endregion LOOP InputParameters
                    return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired };
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    #region IF InputParameterIndex
                    /*PUSH:AutoParameter*/
                    @DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                    {
                        #region LOOP InputParameters
                        #region NOT MethodParameter.IsOut
                        /*PUSH:Parameter*/
                        @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                        #endregion NOT MethodParameter.IsOut
                        #endregion LOOP InputParameters
                    };
                    #endregion IF InputParameterIndex
                    #region IF OutputParameterIndex
                    /*PUSH:AutoParameter*/
                    @DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName _outputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName
                    {
                        #region LOOP OutputParameters
                        #region IF MethodParameter.IsRef
                        /*PUSH:Parameter*/
                        @ParameterName/*PUSH:Parameter*/ = /*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                        #endregion IF MethodParameter.IsRef
                        #endregion LOOP OutputParameters
                        #region PUSH ReturnInputParameter
                        Ret = @ParameterName
                        #endregion PUSH ReturnInputParameter
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleClientPart/**/.@ServerName/**/.TcpClient.Get</*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName, /*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>(@StaticMethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                    #region LOOP OutputParameters
                    #region IF InputMethodParameter.IsRefOrOut
                    /*PUSH:MethodParameter*/
                    @ParameterName/*PUSH:MethodParameter*/ = _outputParameter_./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*/;
                    #endregion IF InputMethodParameter.IsRefOrOut
                    #endregion LOOP OutputParameters
                    return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnType_/*IF:MethodIsReturn*/, Value = _outputParameter_.Return/*IF:MethodIsReturn*/ };
                    #endregion IF OutputParameterIndex
                    #region NOT OutputParameterIndex
                    return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleClientPart/**/.@ServerName/**/.TcpClient.Call(@StaticMethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/) };
                    #endregion NOT OutputParameterIndex
                    #endregion NOT Attribute.IsExpired
                }
                #endregion NOT MemberIndex

                #region IF MemberIndex
                #region IF MethodIsReturn
                #region IF Method.XmlDocument
                /// <summary>
                /// @Method.XmlDocument
                /// </summary>
                #endregion IF Method.XmlDocument
                public static AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> @PropertyName
                {
                    get
                    {
                        #region IF Attribute.IsExpired
                        return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired };
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName _outputParameter_ = default(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleClientPart/**/.@ServerName/**/.TcpClient.Get(@StaticMethodIdentityCommand, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _returnType_, Value = _outputParameter_.Return };
                        #endregion NOT Attribute.IsExpired
                    }
                    #region PUSH SetMethod
                    set
                    {
                        #region IF Attribute.IsExpired
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion LOOP InputParameters
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleClientPart/**/.@ServerName/**/.TcpClient.Call(@StaticMethodIdentityCommand, ref _inputParameter_);
                        if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                        throw new Exception(_returnType_.ToString());
                        #endregion NOT Attribute.IsExpired
                    }
                    #endregion PUSH SetMethod
                }
                #endregion IF MethodIsReturn
                #endregion IF MemberIndex
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
            }
        }
        #endregion PART CLIENTCALL
        #endregion NOT IsAllType

        #region IF IsAllType
        #region PART SERVER
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class @ServerName : AutoCSer.Net.TcpInternalSimpleServer.Server
        {
            #region IF ServiceAttribute.IsRememberCommand
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] @RememberIdentityCommeandName()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[@MethodIndexs.Length];
                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                names[@MethodIndex].Set(@"@Method.MethodKeyFullName", @MethodIndex);
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
                return names;
            }
            #endregion IF ServiceAttribute.IsRememberCommand
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public @ServerName(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("@ServerRegisterName"/*IF:TcpServerAttributeType*/, typeof(@TcpServerAttributeType)/*IF:TcpServerAttributeType*/, true)), verify, log, @IsCallQueue)
            {
                setCommandData(@MethodIndexs.Length);
                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                #region IF IsVerifyMethod
                setVerifyCommand(@MethodIndex);
                #endregion IF IsVerifyMethod
                #region NOT IsVerifyMethod
                setCommand(@MethodIndex);
                #endregion NOT IsVerifyMethod
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
                if (attribute.IsAutoServer) Start();
            }
            /// <summary>
            /// 命令处理
            /// </summary>
            /// <param name="index">命令序号</param>
            /// <param name="socket">TCP 内部服务套接字数据发送</param>
            /// <param name="data">命令数据</param>
            /// <returns>是否成功</returns>
            public override bool DoCommand(int index, AutoCSer.Net.TcpInternalSimpleServer.ServerSocket socket, ref SubArray<byte> data)
            {
                AutoCSer.Net.TcpServer.ReturnType returnType;
                switch (index - @CommandStartIndex)
                {
                    #region LOOP MethodIndexs
                    #region NOT IsNullMethod
                    case @MethodIndex:
                        #region IF Attribute.IsExpired
                        #region IF OutputParameterIndex
                        return socket.SendOutput(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                        #endregion IF OutputParameterIndex
                        #region NOT OutputParameterIndex
                        return socket.Send(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                        #endregion NOT OutputParameterIndex
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            #region IF InputParameterIndex
                            @InputParameterTypeName inputParameter = new @InputParameterTypeName();
                            if (socket.DeSerialize(ref data, ref inputParameter/*IF:IsSimpleSerializeInputParamter*/, true/*IF:IsSimpleSerializeInputParamter*/))
                            #endregion IF InputParameterIndex
                            {
                                #region IF IsAsynchronousCallback
                                #region IF MethodIsReturn
                                @OutputParameterTypeName outputParameter = new @OutputParameterTypeName();
                                @MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName(/*IF:ClientParameterName*/socket, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName>, bool>)/*NOTE*/socket.GetCallback<@OutputParameterTypeName/*NOT:IsVerifyMethod*/, @MethodReturnType.FullName/*NOT:IsVerifyMethod*/>(@StaticMethodIdentityCommand, ref outputParameter));
                                #endregion IF MethodIsReturn
                                #region NOT MethodIsReturn
                                @MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName(/*IF:ClientParameterName*/socket, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue, bool>)/*NOTE*/socket.GetCallback());
                                #endregion NOT MethodIsReturn
                                return true;
                                #endregion IF IsAsynchronousCallback
                                #region NOT IsAsynchronousCallback
                                #region IF IsMethodServerCall
                                (@MethodStreamName/**/.Pop() ?? new @MethodStreamName()).Set(socket, @ServerTask/*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
                                return true;
                                #endregion IF IsMethodServerCall
                                #region NOT IsMethodServerCall
                                #region IF OutputParameterIndex
                                @OutputParameterTypeName _outputParameter_ = new @OutputParameterTypeName();
                                #endregion IF OutputParameterIndex
                                /*IF:MethodIsReturn*/
                                @MethodReturnType.FullName @ReturnName;/*IF:MethodIsReturn*/
                                #region IF MemberIndex
                                #region IF Method.IsGetMember
                                @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/@MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName();
                                #endregion IF Method.IsGetMember
                                #region NOT Method.IsGetMember
                                @MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName(/*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/);
                                #endregion NOT Method.IsGetMember
                                #endregion IF MemberIndex
                                #region NOT MemberIndex
                                /*IF:MethodIsReturn*/
                                @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/ @MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName/*PUSH:Method*/(/*IF:ClientParameterName*/socket/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/_outputParameter_.@ParameterName/*PUSH:InputParameter*//*IF:MethodParameter.IsOut*//*NOTE*/,/*NOTE*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*//*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/);
                                #endregion NOT MemberIndex
                                #region IF OutputParameterIndex
                                #region IF IsVerifyMethod
                                if (/*NOTE*/(bool)(object)/*NOTE*/@ReturnName) socket.SetVerifyMethod();
                                #endregion IF IsVerifyMethod
                                #region LOOP OutputParameters
                                #region NOT InputMethodParameter.IsOut
                                /*PUSH:Parameter*/
                                _outputParameter_.@ParameterName/*PUSH:Parameter*/ = inputParameter./*PUSH:InputParameter*/@ParameterName/*PUSH:InputParameter*/;
                                #endregion NOT InputMethodParameter.IsOut
                                #endregion LOOP OutputParameters
                                #region IF MethodIsReturn
                                _outputParameter_.@ReturnName = @ReturnName;
                                #endregion IF MethodIsReturn
                                return socket.Send(@StaticMethodIdentityCommand, ref _outputParameter_);
                                #endregion IF OutputParameterIndex
                                #region NOT OutputParameterIndex
                                return socket.Send();
                                #endregion NOT OutputParameterIndex
                                #endregion NOT IsMethodServerCall
                                #endregion NOT IsAsynchronousCallback
                            }
                            #region IF InputParameterIndex
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            #endregion IF InputParameterIndex
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        #region IF OutputParameterIndex
                        return socket.SendOutput(returnType);
                        #endregion IF OutputParameterIndex
                        #region NOT OutputParameterIndex
                        return socket.Send(returnType);
                    #endregion NOT OutputParameterIndex
                    #endregion NOT Attribute.IsExpired
                    #endregion NOT IsNullMethod
                    #endregion LOOP MethodIndexs
                    default: return false;
                }
            }
            #region LOOP MethodIndexs
            #region NOT IsNullMethod
            #region NOT IsAsynchronousCallback
            #region IF IsMethodServerCall
            sealed class @MethodStreamName : AutoCSer.Net.TcpStaticSimpleServer.ServerCall<@MethodStreamName/*IF:InputParameterIndex*/, @InputParameterTypeName/*IF:InputParameterIndex*/>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ value)
                {
                    try
                    {
                        /*IF:MethodIsReturn*/
                        @MethodReturnType.FullName @ReturnName;/*IF:MethodIsReturn*/
                        #region IF MemberIndex
                        #region IF Method.IsGetMember
                        @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/@MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName();
                        #endregion IF Method.IsGetMember
                        #region NOT Method.IsGetMember
                        @MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName(/*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/);
                        #endregion NOT Method.IsGetMember
                        #endregion IF MemberIndex

                        #region NOT MemberIndex
                        /*IF:MethodIsReturn*/
                        @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/@MethodType.FullName/**/.TcpStaticSimpleServer.@StaticMethodIndexName/*PUSH:Method*/(/*IF:ClientParameterName*/Socket/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/value.Value.@ParameterName/*PUSH:InputParameter*//*NOTE*/,/*NOTE*//*IF:MethodParameter.IsOut*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*AT:MethodParameter.ParameterJoin*//*LOOP:InputParameters*/);
                        #endregion NOT MemberIndex

                        #region IF OutputParameterIndex
                        #region IF IsVerifyMethod
                        if (/*NOTE*/(bool)(object)/*NOTE*/@ReturnName) Socket.SetVerifyMethod();
                        #endregion IF IsVerifyMethod
                        #region LOOP OutputParameters
                        #region NOT InputMethodParameter.IsOut
                        /*PUSH:Parameter*/
                        value.Value.@ParameterName/*PUSH:Parameter*/ = inputParameter./*PUSH:InputParameter*/@ParameterName/*PUSH:InputParameter*/;
                        #endregion NOT InputMethodParameter.IsOut
                        #endregion LOOP OutputParameters
                        #region IF MethodIsReturn
                        value.Value.@ReturnName = @ReturnName;
                        #endregion IF MethodIsReturn
                        #endregion IF OutputParameterIndex
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                    }
                    catch (Exception error)
                    {
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                        Socket.Log(error);
                    }
                }
                public override void Call()
                {
                    AutoCSer.Net.TcpInternalSimpleServer.ServerSocket socket = Socket;
                    AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ value = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                    get(ref value);
                    push(this);
                    #region IF OutputParameterIndex
                    socket.SendAsync(@StaticMethodIdentityCommand, ref value);
                    #endregion IF OutputParameterIndex
                    #region NOT OutputParameterIndex
                    socket.SendAsync(value.Type);
                    #endregion NOT OutputParameterIndex
                }
            }
            #endregion IF IsMethodServerCall
            #endregion NOT IsAsynchronousCallback
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo @StaticMethodIdentityCommand = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = @OutputParameterIndex/*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
            #endregion NOT IsNullMethod
            #endregion LOOP MethodIndexs

            #region NAME Parameter
            #region LOOP ParameterTypes
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false/*NOT:IsSerializeReferenceMember*/, IsReferenceMember = false/*NOT:IsSerializeReferenceMember*/)]
            #region IF IsSerializeBox
            [AutoCSer.Metadata.BoxSerialize]
            #endregion IF IsSerializeBox
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct @ParameterTypeName
            #region IF MethodReturnType.Type
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<@MethodReturnType.FullName>
#endif
            #endregion IF MethodReturnType.Type
            {
                #region LOOP Parameters
                public @ParameterType.FullName @ParameterName;
                #endregion LOOP Parameters
                #region IF MethodReturnType.Type
                [AutoCSer.Json.IgnoreMember]
                public @MethodReturnType.FullName Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public @MethodReturnType.FullName Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (@MethodReturnType.FullName)value; }
                }
#endif
                #endregion IF MethodReturnType.Type
                #region NOTE
                public object ParameterJoinName;
                public object ParameterRealName;
                #endregion NOTE
            }
            #endregion LOOP ParameterTypes
            #endregion NAME Parameter
            #region NOTE
            public struct OutputParameterTypeName : AutoCSer.Net.IReturnParameter
#if !NOJIT
                <MethodReturnType.FullName>
#endif
            {
                public FullName ParameterName;
                public MethodReturnType.FullName ReturnName;
                public MethodReturnType.FullName Ret;
                public MethodReturnType.FullName Return { get; set; }
                public object ReturnObject { get; set; }
            }
            public struct InputParameterTypeName
            {
                public FullName ParameterName;
            }
            #endregion NOTE
            #region IF Attribute.IsCompileSerialize
            static /*NOTE*/void /*NOTE*/@StaticServerName()
            {
                CompileSerialize(new System.Type[] { /*LOOP:SimpleSerializeMethods*/typeof(@InputParameterTypeName), /*LOOP:SimpleSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:SimpleDeSerializeMethods*/typeof(@OutputParameterTypeName), /*LOOP:SimpleDeSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:SerializeMethods*/typeof(@InputParameterTypeName), /*LOOP:SerializeMethods*/null }
                    , new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(@OutputParameterTypeName), /*LOOP:DeSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:JsonSerializeMethods*/typeof(@InputParameterTypeName), /*LOOP:JsonSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:JsonDeSerializeMethods*/typeof(@OutputParameterTypeName), /*LOOP:JsonDeSerializeMethods*/null });
            }
            #endregion IF Attribute.IsCompileSerialize
        }
        #endregion PART SERVER
        #region PART CLIENT
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public/*NOTE*/ partial/*NOTE*/ class @ServerName
        {
            #region IF ServiceAttribute.IsSegmentation
            #region FROMNAME Parameter
            #endregion FROMNAME Parameter
            #endregion IF ServiceAttribute.IsSegmentation
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<bool> VerifyMethod/*PUSH:TimeVerifyMethod*/ = verify/*PUSH:TimeVerifyMethod*/;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticSimpleServer.Client TcpClient;
            #region PUSH TimeVerifyMethod
            private/*NOTE*/ new/*NOTE*/ static bool verify()
            {
                return AutoCSer.Net.TcpInternalSimpleServer.TimeVerifyClient.Verify(@TimeVerifySimpleClientType/**/.verify, TcpClient);
            }
            #endregion PUSH TimeVerifyMethod
            static @ServerName()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    #region IF ServiceAttribute.IsSegmentation
                    config.ServerAttribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("@ServerName"/*IF:TcpServerAttributeType*/, typeof(@TcpServerAttributeType)/*IF:TcpServerAttributeType*/, false);
                    #endregion IF ServiceAttribute.IsSegmentation
                    #region NOT ServiceAttribute.IsSegmentation
                    config.ServerAttribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("@ServerName"/*IF:TcpServerAttributeType*/, typeof(@TcpServerAttributeType)/*IF:TcpServerAttributeType*/);
                    #endregion NOT ServiceAttribute.IsSegmentation
                }
                #region NOT ServiceAttribute.IsSegmentation
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 @ServerName 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                #endregion NOT ServiceAttribute.IsSegmentation
                TcpClient = new AutoCSer.Net.TcpStaticSimpleServer.Client(config.ServerAttribute, config.Log, config.VerifyMethod);
                #region IF Attribute.IsCompileSerialize
                TcpClient.ClientCompileSerialize(new System.Type[] { /*LOOP:SimpleSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName), /*LOOP:SimpleSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:SimpleDeSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName), /*LOOP:SimpleDeSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:SerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName), /*LOOP:SerializeMethods*/null }
                    , new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName), /*LOOP:DeSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:JsonSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@InputParameterTypeName), /*LOOP:JsonSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:JsonDeSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@SimpleParameterPart/**/.@ServerName/**/.@OutputParameterTypeName), /*LOOP:JsonDeSerializeMethods*/null });
                #endregion IF Attribute.IsCompileSerialize
            }
        }
        #endregion PART CLIENT
        #endregion IF IsAllType
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        /// <summary>
        /// 默认命名空间
        /// </summary>
        public partial class DefaultNamespace
        {
            /// <summary>
            /// 调用参数代码
            /// </summary>
            public class SimpleParameterPart : AutoCSer.CodeGenerator.Template.TcpStaticSimpleServer { }
            /// <summary>
            /// 客服端代码
            /// </summary>
            public class SimpleClientPart : AutoCSer.CodeGenerator.Template.TcpStaticSimpleServer { }
        }
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : Pub
        {
            /// <summary>
            /// TCP调用
            /// </summary>
            public class TcpStaticSimpleServer
            {
                /// <summary>
                /// 字段/属性调用
                /// </summary>
                public static object PropertyName = null;
                /// <summary>
                /// TCP函数调用
                /// </summary>
                /// <param name="value">调用参数</param>
                /// <returns>返回值</returns>
                public static object StaticMethodIndexName(params object[] value)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 客户端时间验证类型
        /// </summary>
        public class TimeVerifySimpleClientType : Pub
        {
            /// <summary>
            /// 验证委托
            /// </summary>
            public static AutoCSer.Net.TcpInternalSimpleServer.TimeVerifyClient.Verifier verify = null;
        }
    }
    #endregion NOTE
}
