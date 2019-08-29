using System;
#pragma warning disable 649
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class TcpStaticServer : Pub
    {
        #region NOTE
        private static FullName[] MethodIndexs = null;
        private static FullName ParameterName = null;
        private static FullName ParameterJoinRefName = null;
        private static FullName ParameterJoinName = null;
        private static ParameterTypeRefName ParameterRefName = null;
        private static FullName ParentIndexName = null;
        private const int MethodIndex = 0;
        private const int CommandStartIndex = 0;
        private const int InputParameterIndex = 0;
        private const int OutputParameterIndex = 0;
        private const AutoCSer.Net.TcpServer.ServerTaskType ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout;
        private const AutoCSer.Net.TcpServer.ClientTaskType ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Timeout;
        private const bool IsCallQueue = false;
        private static Type.FullName GetRemoteMethodName(MemberType.FullName MemberName) { return null; }
        #endregion NOTE

        #region PART CLASS
        #region IF Part=CallType
        #region PART SERVERCALL
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            internal static partial class TcpStaticServer
            {
                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                #region IF IsAsynchronousCallback
                public static void @StaticMethodIndexName(/*IF:ClientParameterName*/AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, /*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Func<AutoCSer.Net.TcpServer.ReturnValue</*PUSH:Method*/@MethodReturnType.FullName/*PUSH:Method*/>, bool> _onReturn_)
                {
                    @Type.FullName/*PUSH:Method*/.@StaticMethodName(/*IF:ClientParameterName*/_sender_, /*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterRefName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/_onReturn_);/*PUSH:Method*/
                }
                #endregion IF IsAsynchronousCallback
                #region NOT IsAsynchronousCallback
                public static /*PUSH:Method*/@MethodReturnType.FullName/*PUSH:Method*/ @StaticMethodIndexName(/*IF:ClientParameterName*/AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    return /*NOTE*/(FullName)/*NOTE*//*IF:MethodIsReturn*/@Type.FullName/*PUSH:Method*/.@StaticMethodName(/*IF:ClientParameterName*/_sender_/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterJoinRefName/*PUSH:MethodParameter*//*LOOP:InputParameters*/);/*PUSH:Method*/
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
        public static partial class TcpCall
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
                #region IF IsSynchronousMethodIdentityCommand
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @StaticMethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsClientSendOnly*/, IsSendOnly = 1/*IF:IsClientSendOnly*/, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous/*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion IF IsSynchronousMethodIdentityCommand
                #region IF IsAwaiterMethodIdentityCommand
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @StaticAwaiterMethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*/, TaskType = @ClientTask/*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion IF IsAwaiterMethodIdentityCommand

                #region NOT MemberIndex
                #region IF IsClientSendOnly
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
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    #region IF Attribute.IsExpired
                    throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    #region IF InputParameterIndex
                    /*PUSH:AutoParameter*/
                    @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                    {
                        #region LOOP InputParameters
                        /*PUSH:Parameter*/
                        @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                        #endregion LOOP InputParameters
                    };
                    #endregion IF InputParameterIndex
                    /*PUSH:AutoParameter*/
                    @DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender.CallOnly(@StaticMethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                    #endregion NOT Attribute.IsExpired
                }
                #endregion IF IsClientSendOnly
                #region NOT IsClientSendOnly
                #region IF IsClientSynchronous
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
                public static AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            /*PUSH:AutoParameter*/
                            @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
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
                            @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName _outputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName
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
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet</*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName, /*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>(@StaticMethodIdentityCommand, ref _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                            #region LOOP OutputParameters
                            #region IF InputMethodParameter.IsRefOrOut
                            /*PUSH:MethodParameter*/
                            @ParameterName/*PUSH:MethodParameter*/ = _outputParameter_./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*/;
                            #endregion IF InputMethodParameter.IsRefOrOut
                            #endregion LOOP OutputParameters
                            return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnType_/*IF:MethodIsReturn*/, Value = _outputParameter_.Return/*IF:MethodIsReturn*/ };
                            #endregion IF OutputParameterIndex
                            #region NOT OutputParameterIndex
                            return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _socket_.WaitCall(@StaticMethodIdentityCommand, ref _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/) };
                            #endregion NOT OutputParameterIndex
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
                    }
                    #region LOOP InputParameters
                    #region PUSH MethodParameter
                    #region IF IsOut
                    @ParameterName = default(@ParameterType.FullName);
                    #endregion IF IsOut
                    #endregion PUSH MethodParameter
                    #endregion LOOP InputParameters
                    return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    #endregion NOT Attribute.IsExpired
                }
                #endregion IF IsClientSynchronous
                #region IF IsClientAwaiter
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
                public static AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@AwaiterMethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ _awaiter_ = new AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/();
                    #region IF Attribute.IsExpired
                    _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                    if (_socket_ != null)
                    {
                        #region IF InputParameterIndex
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            #region NOT MethodParameter.IsOut
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion NOT MethodParameter.IsOut
                            #endregion LOOP InputParameters
                        };
                        #endregion IF InputParameterIndex
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        #region IF MethodIsReturn
                        AutoCSer.Net.TcpServer.@AwaiterReturnValue<@MethodReturnType.FullName> _outputParameter_ = default(AutoCSer.Net.TcpServer.@AwaiterReturnValue<@MethodReturnType.FullName>);
                        _returnType_ = _socket_.GetAwaiter</*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName, /*IF:InputParameterIndex*/AutoCSer.Net.TcpServer.@AwaiterReturnValue<@MethodReturnType.FullName>>(@StaticAwaiterMethodIdentityCommand, _awaiter_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                        #endregion IF MethodIsReturn
                        #region NOT MethodIsReturn
                        _returnType_ = _socket_.GetAwaiter(@StaticAwaiterMethodIdentityCommand, _awaiter_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                        #endregion NOT MethodIsReturn
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    #endregion NOT Attribute.IsExpired
                    return _awaiter_;
                }
                #endregion IF IsClientAwaiter
                #region IF IsClientTaskAsync
#if !DOTNET2 && !DOTNET4
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
                public static async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> /*PUSH:Method*/@TaskAsyncMethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                        #region IF InputParameterIndex
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            #region NOT MethodParameter.IsOut
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion NOT MethodParameter.IsOut
                            #endregion LOOP InputParameters
                        };
                        #endregion IF InputParameterIndex
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        #region IF OutputParameterIndex
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName _outputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName
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
                        if ((_returnType_ = _socket_.GetAsync</*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName, /*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>(@StaticAwaiterMethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_)) == Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _returnOutputParameter_ = await _wait_;
                            #region LOOP OutputParameters
                            #region IF InputMethodParameter.IsRefOrOut
                            /*PUSH:MethodParameter*/
                            @ParameterName/*PUSH:MethodParameter*/ = _returnOutputParameter_.Value./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*/;
                            #endregion IF InputMethodParameter.IsRefOrOut
                            #endregion LOOP OutputParameters
                            return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnOutputParameter_.Type/*IF:MethodIsReturn*/, Value = _returnOutputParameter_.Value.Return/*IF:MethodIsReturn*/ };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnType_ };
                        #endregion IF OutputParameterIndex
                        #region NOT OutputParameterIndex
                        _returnType_ = _socket_.CallAsync(@StaticAwaiterMethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                        return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success ? /*NOTE*/(AutoCSer.Net.TcpServer.ReturnType)(object)/*NOTE*/await _wait_ : _returnType_ };
                        #endregion NOT OutputParameterIndex
                    }
                    #region LOOP InputParameters
                    #region PUSH MethodParameter
                    #region IF IsOut
                    @ParameterName = default(@ParameterType.FullName);
                    #endregion IF IsOut
                    #endregion PUSH MethodParameter
                    #endregion LOOP InputParameters
                    return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    #endregion NOT Attribute.IsExpired
                }
#endif
                #endregion IF IsClientTaskAsync
                #region IF IsClientAsynchronous
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @AsynchronousStaticMethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex, TaskType = @ClientTask/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsKeepCallback*/, IsKeepCallback = 1/*IF:IsKeepCallback*//*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
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
                /// <param name="_onReturn_">@Method.ReturnXmlDocument</param>
                #endregion IF Method.ReturnXmlDocument
                #region IF IsKeepCallback
                /// <returns>保持异步回调</returns>
                #endregion IF IsKeepCallback
                #region IF MethodIsReturn
                public static @KeepCallbackType /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/{ Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/> _onOutput_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.GetCallback</*IF:MethodIsReturn*/@MethodReturnType.FullName, /*IF:MethodIsReturn*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            /*PUSH:AutoParameter*/
                            @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            #region IF IsKeepCallback
                            return /*NOTE*/(KeepCallbackType)(object)/*NOTE*/_socket_.GetKeep</*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName, /*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>(@AsynchronousStaticMethodIdentityCommand, ref _onOutput_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            #endregion IF IsKeepCallback
                            #region NOT IsKeepCallback
                            _socket_.Get</*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName, /*IF:InputParameterIndex*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>(@AsynchronousStaticMethodIdentityCommand, ref _onOutput_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            #endregion NOT IsKeepCallback
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    #region IF IsKeepCallback
                    return null;
                    #endregion IF IsKeepCallback
                    #endregion NOT Attribute.IsExpired
                }
                #endregion IF MethodIsReturn
                #region NOT MethodIsReturn
                public static @KeepCallbackType /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            /*PUSH:AutoParameter*/
                            @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            #region IF IsKeepCallback
                            AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.CallKeep(@AsynchronousStaticMethodIdentityCommand, _onReturn_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            _onReturn_ = null;
                            return /*NOTE*/(KeepCallbackType)(object)/*NOTE*/_keepCallback_;
                            #endregion IF IsKeepCallback
                            #region NOT IsKeepCallback
                            _socket_.Call(@AsynchronousStaticMethodIdentityCommand, _onReturn_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            _onReturn_ = null;
                            #endregion NOT IsKeepCallback
                        }
                    }
                    finally
                    {
                        if (_onReturn_ != null) _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException });
                    }
                    #region IF IsKeepCallback
                    return null;
                    #endregion IF IsKeepCallback
                    #endregion NOT Attribute.IsExpired
                }
                #endregion NOT MethodIsReturn
                #endregion IF IsClientAsynchronous
                #endregion NOT IsClientSendOnly
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
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = _socket_.WaitGet(@StaticMethodIdentityCommand, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                        #endregion NOT Attribute.IsExpired
                    }
                    #region PUSH SetMethod
                    set
                    {
                        #region IF Attribute.IsExpired
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        #region IF IsClientSendOnly
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _sendOnlyInputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion LOOP InputParameters
                        };
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender.CallOnly(@StaticMethodIdentityCommand, ref _sendOnlyInputParameter_);
                        #endregion IF IsClientSendOnly
                        #region NOT IsClientSendOnly
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServerName/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                /*PUSH:AutoParameter*/
                                @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName
                                {
                                    #region LOOP InputParameters
                                    /*PUSH:Parameter*/
                                    @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                    #endregion LOOP InputParameters
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(@StaticMethodIdentityCommand, ref _wait_, ref _inputParameter_);
                                if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                                throw new Exception(_returnType_.ToString());
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                        #endregion NOT IsClientSendOnly
                        #endregion NOT Attribute.IsExpired
                    }
                    #endregion PUSH SetMethod
                }
                #endregion IF MethodIsReturn
                #endregion IF MemberIndex
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
            }
            #region NOTE
            public class TypeName : Pub { }
            #endregion NOTE
        }
        #endregion PART CLIENTCALL
        #endregion IF Part=CallType

        #region IF Part=ServerType
        #region PART SERVER
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class @ServerName : AutoCSer.Net.TcpInternalServer.Server
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
            /// <param name="onCustomData">自定义数据包处理</param>
            public @ServerName(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("@ServerRegisterName"/*IF:TcpServerAttributeType*/, typeof(@TcpServerAttributeType)/*IF:TcpServerAttributeType*/, true)), verify, onCustomData, log, @IsCallQueue)
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
            /// <param name="sender">TCP 内部服务套接字数据发送</param>
            /// <param name="data">命令数据</param>
            public override void DoCommand(int index, AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, ref SubArray<byte> data)
            {
                AutoCSer.Net.TcpServer.ReturnType returnType;
                switch (index - @CommandStartIndex)
                {
                    #region LOOP MethodIndexs
                    #region NOT IsNullMethod
                    case @MethodIndex:
                        #region IF Attribute.IsExpired
                        #region NOT IsClientSendOnly
                        sender.Push(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                        #endregion NOT IsClientSendOnly
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        #region NOT IsClientSendOnly
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        #endregion NOT IsClientSendOnly
                        try
                        {
                            #region IF InputParameterIndex
                            @InputParameterTypeName inputParameter = new @InputParameterTypeName();
                            if (sender.DeSerialize(ref data, ref inputParameter/*IF:IsSimpleSerializeInputParamter*/, true/*IF:IsSimpleSerializeInputParamter*/))
                            #endregion IF InputParameterIndex
                            {
                                #region IF IsAsynchronousCallback
                                #region IF MethodIsReturn
                                @OutputParameterTypeName outputParameter = new @OutputParameterTypeName();
                                @MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName(/*IF:ClientParameterName*/sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName>, bool>)/*NOTE*/sender.GetCallback<@OutputParameterTypeName, @MethodReturnType.FullName>(@StaticMethodIdentityCommand, ref outputParameter));
                                #endregion IF MethodIsReturn
                                #region NOT MethodIsReturn
                                @MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName(/*IF:ClientParameterName*/sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue, bool>)/*NOTE*/sender.GetCallback(@StaticMethodIdentityCommand));
                                #endregion NOT MethodIsReturn
                                #endregion IF IsAsynchronousCallback
                                #region NOT IsAsynchronousCallback
                                #region IF IsMethodServerCall
                                (@MethodStreamName/**/.Pop() ?? new @MethodStreamName()).Set(sender, @ServerTask/*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
                                #endregion IF IsMethodServerCall
                                #region NOT IsMethodServerCall
                                #region NOT IsClientSendOnly
                                #region IF OutputParameterIndex
                                @OutputParameterTypeName _outputParameter_ = new @OutputParameterTypeName();
                                #endregion IF OutputParameterIndex
                                #endregion NOT IsClientSendOnly
                                /*IF:MethodIsReturn*/
                                @MethodReturnType.FullName @ReturnName;/*IF:MethodIsReturn*/
                                #region IF MemberIndex
                                #region IF Method.IsGetMember
                                @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/@MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName();
                                #endregion IF Method.IsGetMember
                                #region NOT Method.IsGetMember
                                @MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName(/*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/);
                                #endregion NOT Method.IsGetMember
                                #endregion IF MemberIndex
                                #region NOT MemberIndex
                                /*IF:MethodIsReturn*/
                                @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/ @MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName/*PUSH:Method*/(/*IF:ClientParameterName*/sender/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/_outputParameter_.@ParameterName/*PUSH:InputParameter*//*IF:MethodParameter.IsOut*//*NOTE*/,/*NOTE*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*//*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/);
                                #endregion NOT MemberIndex
                                #region NOT IsClientSendOnly
                                #region IF OutputParameterIndex
                                #region IF IsVerifyMethod
                                if (/*NOTE*/(bool)(object)/*NOTE*/@ReturnName) sender.SetVerifyMethod();
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
                                sender.Push(@StaticMethodIdentityCommand, ref _outputParameter_);
                                #endregion IF OutputParameterIndex
                                #region NOT OutputParameterIndex
                                sender.Push();
                                #endregion NOT OutputParameterIndex
                                #endregion NOT IsClientSendOnly
                                #endregion NOT IsMethodServerCall
                                #endregion NOT IsAsynchronousCallback
                                return;
                            }
                            #region IF InputParameterIndex
                            #region NOT IsClientSendOnly
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            #endregion NOT IsClientSendOnly
                            #endregion IF InputParameterIndex
                        }
                        catch (Exception error)
                        {
                            #region NOT IsClientSendOnly
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            #endregion NOT IsClientSendOnly
                            sender.AddLog(error);
                        }
                        #region NOT IsClientSendOnly
                        sender.Push(returnType);
                        #endregion NOT IsClientSendOnly
                        #endregion NOT Attribute.IsExpired
                        return;
                    #endregion NOT IsNullMethod
                    #endregion LOOP MethodIndexs
                    default: return;
                }
            }
            #region LOOP MethodIndexs
            #region NOT IsNullMethod
            #region NOT IsAsynchronousCallback
            #region IF IsMethodServerCall
            sealed class @MethodStreamName : AutoCSer.Net.TcpStaticServer.ServerCall<@MethodStreamName/*IF:InputParameterIndex*/, @InputParameterTypeName/*IF:InputParameterIndex*/>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ value)
                {
                    try
                    {
                        /*IF:MethodIsReturn*/
                        @MethodReturnType.FullName @ReturnName;/*IF:MethodIsReturn*/
                        #region IF MemberIndex
                        #region IF Method.IsGetMember
                        @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/@MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName();
                        #endregion IF Method.IsGetMember
                        #region NOT Method.IsGetMember
                        @MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName(/*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/);
                        #endregion NOT Method.IsGetMember
                        #endregion IF MemberIndex

                        #region NOT MemberIndex
                        /*IF:MethodIsReturn*/
                        @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/@MethodType.FullName/**/.TcpStaticServer.@StaticMethodIndexName/*PUSH:Method*/(/*IF:ClientParameterName*/Sender/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/value.Value.@ParameterName/*PUSH:InputParameter*//*NOTE*/,/*NOTE*//*IF:MethodParameter.IsOut*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*AT:MethodParameter.ParameterJoin*//*LOOP:InputParameters*/);
                        #endregion NOT MemberIndex

                        #region IF OutputParameterIndex
                        #region IF IsVerifyMethod
                        if (/*NOTE*/(bool)(object)/*NOTE*/@ReturnName) Sender.SetVerifyMethod();
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
                        Sender.AddLog(error);
                    }
                }
                public override void Call()
                {
                    AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ value = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                    #region IF IsClientSendOnly
                    if (Sender.IsSocket) get(ref value);
                    #endregion IF IsClientSendOnly
                    #region NOT IsClientSendOnly
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        #region IF OutputParameterIndex
                        Sender.Push(CommandIndex, @StaticMethodIdentityCommand, ref value);
                        #endregion IF OutputParameterIndex
                        #region NOT OutputParameterIndex
                        #region IF IsServerBuildOutputThread
                        Sender.Push(CommandIndex, value.Type);
                        #endregion IF IsServerBuildOutputThread
                        #region NOT IsServerBuildOutputThread
                        Sender.PushNoThread(CommandIndex, value.Type);
                        #endregion NOT IsServerBuildOutputThread
                        #endregion NOT OutputParameterIndex
                    }
                    #endregion NOT IsClientSendOnly
                    push(this);
                }
            }
            #endregion IF IsMethodServerCall
            #endregion NOT IsAsynchronousCallback
            private static readonly AutoCSer.Net.TcpServer.OutputInfo @StaticMethodIdentityCommand = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = @OutputParameterIndex/*IF:IsKeepCallback*/, IsKeepCallback = 1/*IF:IsKeepCallback*//*IF:IsClientSendOnly*/, IsClientSendOnly = 1/*IF:IsClientSendOnly*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*//*IF:IsServerBuildOutputThread*/, IsBuildOutputThread = true/*IF:IsServerBuildOutputThread*/ };
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
                public AutoCSer.Net.TcpInternalServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 自定义数据包处理
                /// </summary>
                public Action<AutoCSer.SubArray<byte>> OnCustomData;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> ClientRoute/*IF:ClientRouteType*/ = new @ClientRouteType()/*IF:ClientRouteType*/;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod/*PUSH:TimeVerifyMethod*/ = verify/*PUSH:TimeVerifyMethod*/;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            #region PUSH TimeVerifyMethod
            private/*NOTE*/ new/*NOTE*/ static bool verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender sender)
            {
                return AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verify(@TimeVerifyClientType/**/.verify, sender, TcpClient);
            }
            #endregion PUSH TimeVerifyMethod
            static @ServerName()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    #region IF ServiceAttribute.IsSegmentation
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("@ServerName"/*IF:TcpServerAttributeType*/, typeof(@TcpServerAttributeType)/*IF:TcpServerAttributeType*/, false);
                    #endregion IF ServiceAttribute.IsSegmentation
                    #region NOT ServiceAttribute.IsSegmentation
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("@ServerName"/*IF:TcpServerAttributeType*/, typeof(@TcpServerAttributeType)/*IF:TcpServerAttributeType*/);
                    #endregion NOT ServiceAttribute.IsSegmentation
                }
                #region NOT ServiceAttribute.IsSegmentation
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 @ServerName 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                #endregion NOT ServiceAttribute.IsSegmentation
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                #region IF Attribute.IsCompileSerialize
                TcpClient.ClientCompileSerialize(new System.Type[] { /*LOOP:SimpleSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName), /*LOOP:SimpleSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:SimpleDeSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName), /*LOOP:SimpleDeSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:SerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName), /*LOOP:SerializeMethods*/null }
                    , new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName), /*LOOP:DeSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:JsonSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@InputParameterTypeName), /*LOOP:JsonSerializeMethods*/null }
                    , new System.Type[] { /*LOOP:JsonDeSerializeMethods*/typeof(/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServerName/**/.@OutputParameterTypeName), /*LOOP:JsonDeSerializeMethods*/null });
                #endregion IF Attribute.IsCompileSerialize
            }
        }
        #endregion PART CLIENT
        #endregion IF Part=ServerType

        #region IF Part=RemoteLink
        #region PUSH CurrentRemoteLinkType
        #region PART SERVERREMOTE
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            #region IF RemoteKeyMember
            #region LOOP RemoteMembers
            #region PUSH Member
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH RemoteKeyMember
            #region IF XmlDocument
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion PUSH RemoteKeyMember
            /// <returns>@XmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MemberType.FullName @GetMemberName(/*PUSH:RemoteKeyMember*/@MemberType.FullName @MemberName/*PUSH:RemoteKeyMember*/)
            {
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/@GetRemoteMethodName(/*PUSH:RemoteKeyMember*/@MemberName/*PUSH:RemoteKeyMember*/).@MemberName;
            }
            #endregion PUSH Member
            #region PUSH Method
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH RemoteKeyMember
            #region IF XmlDocument
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion PUSH RemoteKeyMember
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MethodReturnType.FullName @RemoteMethodName(/*PUSH:RemoteKeyMember*/@MemberType.FullName @MemberName/*PUSH:RemoteKeyMember*//*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                /*IF:IsMethodReturn*/
                return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/@GetRemoteMethodName(/*PUSH:RemoteKeyMember*/@MemberName/*PUSH:RemoteKeyMember*/).@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
            }
            #endregion PUSH Method
            #endregion LOOP RemoteMembers

            #region LOOP RemoteLinks
            #region PUSH Member
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH RemoteKeyMember
            #region IF XmlDocument
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion PUSH RemoteKeyMember
            #region LOOP PropertyParameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP PropertyParameters
            /// <returns>@XmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MemberType.FullName @GetLinkMemberName(/*PUSH:RemoteKeyMember*/@MemberType.FullName @MemberName/*PUSH:RemoteKeyMember*//*LOOP:PropertyParameters*/, @ParameterTypeRefName @ParameterName/*LOOP:PropertyParameters*/)
            {
                @Type.FullName _value_ = @GetRemoteMethodName(/*PUSH:RemoteKeyMember*/@MemberName/*PUSH:RemoteKeyMember*/);
                #region LOOP Members
                #region PUSH Member
                @MemberType.FullName @IndexName = /*NOTE*/(@MemberType.FullName)/*NOTE*/@ParentIndexName/**/.@MemberName;
                #endregion PUSH Member
                #region IF IsNull
                if (@IndexName != null)
                {
                    #endregion IF IsNull
                    #endregion LOOP Members
                    return /*NOTE*/(@MemberType.FullName)/*NOTE*/@IndexName/*NOT:PropertyParameters.Length*/.@MemberName/*NOT:PropertyParameters.Length*//*IF:PropertyParameters.Length*/[/*LOOP:PropertyParameters*/@ParameterJoinName/*LOOP:PropertyParameters*/]/*IF:PropertyParameters.Length*/;
                    #region LOOP Members
                    #region IF IsNull
                }
                #endregion IF IsNull
                #endregion LOOP Members
                #region IF IsAnyNull
                return default(@MemberType.FullName);
                #endregion IF IsAnyNull
            }
            #endregion PUSH Member
            #region PUSH Method
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH RemoteKeyMember
            #region IF XmlDocument
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion PUSH RemoteKeyMember
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MethodReturnType.FullName @RemoteLinkMethodName(/*PUSH:RemoteKeyMember*/@MemberType.FullName @MemberName/*PUSH:RemoteKeyMember*//*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                @Type.FullName _value_ = @GetRemoteMethodName(/*PUSH:RemoteKeyMember*/@MemberName/*PUSH:RemoteKeyMember*/);
                #region LOOP Members
                #region PUSH Member
                @MemberType.FullName @IndexName = /*NOTE*/(@MemberType.FullName)/*NOTE*/@ParentIndexName/**/.@MemberName;
                #endregion PUSH Member
                #region IF IsNull
                if (@IndexName != null)
                {
                    #endregion IF IsNull
                    #endregion LOOP Members
                    /*IF:IsMethodReturn*/
                    return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/@IndexName/**/.@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
                    #region LOOP Members
                    #region IF IsNull
                }
                #endregion IF IsNull
                #endregion LOOP Members
                #region IF IsAnyNull
                #region LOOP Parameters
                #region IF IsOut
                @ParameterName = /*NOTE*/(ParameterTypeRefName)(object)/*NOTE*/default(@ParameterType.FullName);
                #endregion IF IsOut
                #endregion LOOP Parameters
                #region IF IsMethodReturn
                return default(@MemberType.FullName);
                #endregion IF IsMethodReturn
                #endregion IF IsAnyNull
            }
            #endregion PUSH Method
            #endregion LOOP RemoteLinks
            #endregion IF RemoteKeyMember
            #region NOT RemoteKeyMember
            #region LOOP RemoteMembers
            #region PUSH Member
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="value">@Type.XmlDocument</param>
            /// <returns>@XmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MemberType.FullName @GetTypeMemberName(@Type.FullName value)
            {
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@MemberName;
            }
            #endregion PUSH Member
            #region PUSH Method
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="value">@Type.XmlDocument</param>
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MethodReturnType.FullName @RemoteTypeMethodName(@Type.FullName _value_/*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                /*IF:IsMethodReturn*/
                return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/_value_.@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
            }
            #endregion PUSH Method
            #endregion LOOP RemoteMembers

            #region LOOP RemoteLinks
            #region PUSH Member
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="value">@Type.XmlDocument</param>
            #region LOOP PropertyParameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP PropertyParameters
            /// <returns>@XmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MemberType.FullName @GetLinkTypeMemberName(@Type.FullName _value_/*LOOP:PropertyParameters*/, @ParameterTypeRefName @ParameterName/*LOOP:PropertyParameters*/)
            {
                #region LOOP Members
                #region PUSH Member
                @MemberType.FullName @IndexName = /*NOTE*/(@MemberType.FullName)/*NOTE*/@ParentIndexName/**/.@MemberName;
                #endregion PUSH Member
                #region IF IsNull
                if (@IndexName != null)
                {
                    #endregion IF IsNull
                    #endregion LOOP Members
                    return /*NOTE*/(@MemberType.FullName)/*NOTE*/@IndexName/*NOT:PropertyParameters.Length*/.@MemberName/*NOT:PropertyParameters.Length*//*IF:PropertyParameters.Length*/[/*LOOP:PropertyParameters*/@ParameterJoinName/*LOOP:PropertyParameters*/]/*IF:PropertyParameters.Length*/;
                    #region LOOP Members
                    #region IF IsNull
                }
                #endregion IF IsNull
                #endregion LOOP Members
                #region IF IsAnyNull
                return default(@MemberType.FullName);
                #endregion IF IsAnyNull
            }
            #endregion PUSH Member
            #region PUSH Method
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="_value_">@Type.XmlDocument</param>
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@LinkMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MethodReturnType.FullName @RemoteLinkTypeMethodName(@Type.FullName _value_/*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                #region LOOP Members
                #region PUSH Member
                @MemberType.FullName @IndexName = /*NOTE*/(@MemberType.FullName)/*NOTE*/@ParentIndexName/**/.@MemberName;
                #endregion PUSH Member
                #region IF IsNull
                if (@IndexName != null)
                {
                    #endregion IF IsNull
                    #endregion LOOP Members
                    /*IF:IsMethodReturn*/
                    return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/@IndexName/**/.@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
                    #region LOOP Members
                    #region IF IsNull
                }
                #endregion IF IsNull
                #endregion LOOP Members
                #region IF IsAnyNull
                #region LOOP Parameters
                #region IF IsOut
                @ParameterName = /*NOTE*/(ParameterTypeRefName)(object)/*NOTE*/default(@ParameterType.FullName);
                #endregion IF IsOut
                #endregion LOOP Parameters
                #region IF IsMethodReturn
                return default(@MemberType.FullName);
                #endregion IF IsMethodReturn
                #endregion IF IsAnyNull
            }
            #endregion PUSH Method
            #endregion LOOP RemoteLinks
            #endregion NOT RemoteKeyMember
        }
        #endregion PART SERVERREMOTE
        #region PART CLIENTREMOTE
        #region IF RemoteMethods.Length
        #region IF ServiceAttribute.IsSegmentation
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            #region IF Type.XmlDocument
            /// <summary>
            /// @Type.XmlDocument
            /// </summary>
            #endregion IF Type.XmlDocument
            public /*NOTE*/partial class /*NOTE*/@NoAccessTypeNameDefinition
            {
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                public partial struct RemoteExtension
                {
                    #region IF IsRemoteMember
                    #region PUSH RemoteKeyMember
                    #region IF XmlDocument
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    #endregion IF XmlDocument
                    public @MemberType.FullName @MemberName;
                    #endregion PUSH RemoteKeyMember
                    #endregion IF IsRemoteMember
                    #region IF RemoteKeyMember
                    #region LOOP RemoteMethods
                    #region PUSH Method
                    #region IF IsMethod
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    #region LOOP NextParameters
                    #region IF XmlDocument
                    /// <param name="@ParameterName">@XmlDocument</param>
                    #endregion IF XmlDocument
                    #endregion LOOP NextParameters
                    public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeMethodName(/*LOOP:NextParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:NextParameters*/)
                    {
                        /*IF:IsMethodReturn*/
                        return /*IF:IsMethodReturn*/ /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(/*PUSH:RemoteKeyMember*/@MemberName/*PUSH:RemoteKeyMember*//*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                    }
                    #endregion IF IsMethod
                    #region NOT IsMethod
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeMemberName
                    {
                        get
                        {
                            return /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(/*PUSH:RemoteKeyMember*/@MemberName/*PUSH:RemoteKeyMember*/);
                        }
                    }
                    #endregion NOT IsMethod
                    #endregion PUSH Method
                    #endregion LOOP RemoteMethods
                    #endregion IF RemoteKeyMember
                    #region NOT RemoteKeyMember
                    #region IF IsRemoteMember
                    /// <summary>
                    /// @Type.XmlDocument
                    /// </summary>
                    public @Type.FullName Value;
                    #endregion IF IsRemoteMember
                    #region LOOP RemoteMethods
                    #region PUSH Method
                    #region IF IsMethod
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    #region LOOP NextParameters
                    #region IF XmlDocument
                    /// <param name="@ParameterName">@XmlDocument</param>
                    #endregion IF XmlDocument
                    #endregion LOOP NextParameters
                    public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeTypeMethodName(/*LOOP:NextParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:NextParameters*/)
                    {
                        /*IF:IsMethodReturn*/
                        return /*IF:IsMethodReturn*/ /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value/*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                    }
                    #endregion IF IsMethod
                    #region NOT IsMethod
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeTypeMemberName
                    {
                        get
                        {
                            return /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value);
                        }
                    }
                    #endregion NOT IsMethod
                    #endregion PUSH Method
                    #endregion LOOP RemoteMethods
                    #endregion NOT RemoteKeyMember
                }
                #region IF IsRemoteMember
                #region PUSH RemoteKeyMember
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                #region IF XmlDocument
                /// <param name="@MemberName">@XmlDocument</param>
                #endregion IF XmlDocument
                /// <returns>远程对象扩展</returns>
                public static RemoteExtension Remote(@MemberType.FullName @MemberName)
                {
                    return new RemoteExtension { @MemberName = @MemberName };
                }
                #endregion PUSH RemoteKeyMember
                #region NOT RemoteKeyMember
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                #region IF Type.XmlDocument
                /// <param name="value">@Type.XmlDocument</param>
                #endregion IF Type.XmlDocument
                /// <returns>远程对象扩展</returns>
                public static RemoteExtension RemoteType(@Type.FullName value)
                {
                    return new RemoteExtension { Value = value };
                }
                #endregion NOT RemoteKeyMember
                #endregion IF IsRemoteMember
            }
        }
        #endregion IF ServiceAttribute.IsSegmentation
        #region NOT ServiceAttribute.IsSegmentation
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                #region IF IsRemoteMember
                /// <summary>
                /// @Type.XmlDocument
                /// </summary>
                internal @Type.FullName Value;
                #endregion IF IsRemoteMember
                #region IF RemoteKeyMember
                #region LOOP RemoteMethods
                #region PUSH Method
                #region IF IsMethod
                /// <summary>
                /// @XmlDocument
                /// </summary>
                #region LOOP NextParameters
                #region IF XmlDocument
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion IF XmlDocument
                #endregion LOOP NextParameters
                public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeMethodName(/*LOOP:NextParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:NextParameters*/)
                {
                    /*IF:IsMethodReturn*/
                    return /*IF:IsMethodReturn*/ /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value/*PUSH:RemoteKeyMember*/.@MemberName/*PUSH:RemoteKeyMember*//*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                }
                #endregion IF IsMethod
                #region NOT IsMethod
                /// <summary>
                /// @XmlDocument
                /// </summary>
                public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeMemberName
                {
                    get
                    {
                        return /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value/*PUSH:RemoteKeyMember*/.@MemberName/*PUSH:RemoteKeyMember*/);
                    }
                }
                #endregion NOT IsMethod
                #endregion PUSH Method
                #endregion LOOP RemoteMethods
                #endregion IF RemoteKeyMember
                #region NOT RemoteKeyMember
                #region LOOP RemoteMethods
                #region PUSH Method
                #region IF IsMethod
                /// <summary>
                /// @XmlDocument
                /// </summary>
                #region LOOP NextParameters
                #region IF XmlDocument
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion IF XmlDocument
                #endregion LOOP NextParameters
                public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeTypeMethodName(/*LOOP:NextParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:NextParameters*/)
                {
                    /*IF:IsMethodReturn*/
                    return /*IF:IsMethodReturn*/ /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value/*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                }
                #endregion IF IsMethod
                #region NOT IsMethod
                /// <summary>
                /// @XmlDocument
                /// </summary>
                public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeTypeMemberName
                {
                    get
                    {
                        return /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value);
                    }
                }
                #endregion NOT IsMethod
                #endregion PUSH Method
                #endregion LOOP RemoteMethods
                #endregion NOT RemoteKeyMember
            }
            #region IF IsRemoteMember
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            [AutoCSer.BinarySerialize.IgnoreMember]
            [AutoCSer.Json.IgnoreMember]
            public RemoteExtension Remote
            {
                get { return new RemoteExtension { Value = /*NOTE*/(Type.FullName)(object)/*NOTE*/this }; }
            }
            #endregion IF IsRemoteMember
        }
        #endregion NOT ServiceAttribute.IsSegmentation
        #endregion IF RemoteMethods.Length
        #endregion PART CLIENTREMOTE
        #endregion PUSH CurrentRemoteLinkType
        #endregion IF Part=RemoteLink
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
            public class ParameterPart : AutoCSer.CodeGenerator.Template.TcpStaticServer { }
            /// <summary>
            /// 客服端代码
            /// </summary>
            public class ClientPart : AutoCSer.CodeGenerator.Template.TcpStaticServer { }
        }
        /// <summary>
        /// TCP配置类型
        /// </summary>
        public class TcpServerAttributeType { }
        /// <summary>
        /// 获取该函数的类型
        /// </summary>
        public class MethodType : Pub { }
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : Pub
        {
            /// <summary>
            /// TCP调用
            /// </summary>
            public class TcpStaticServer
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
        public class TimeVerifyClientType : Pub
        {
            /// <summary>
            /// 验证委托
            /// </summary>
            public static AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verifier verify = null;
        }
    }
    #endregion NOTE
}
