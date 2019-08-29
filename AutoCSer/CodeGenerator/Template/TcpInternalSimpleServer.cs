using System;
using AutoCSer.Net.TcpInternalSimpleServer;
#pragma warning disable 649
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class TcpInternalSimpleServer : Pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        #region IF IsSetTcpServer
        #region IF IsServerCode
#if !NOJIT
             : AutoCSer.Net.TcpSimpleServer.ISetTcpServer<AutoCSer.Net.TcpInternalSimpleServer.Server, AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute>
#endif
        #endregion IF IsServerCode
        #endregion IF IsSetTcpServer
        {
            #region NOTE
            private static FullName[] MethodIndexs = null;
            private const int MethodIndex = 0;
            private static FullName ParameterName = null;
            private const AutoCSer.Net.TcpServer.ServerTaskType ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous;
            private const int CommandStartIndex = 0;
            private const int InputParameterIndex = 0;
            private const int OutputParameterIndex = 0;
            private const bool IsCallQueue = false;
            public void SetTcpServer(AutoCSer.Net.TcpInternalSimpleServer.Server commandServer) { }
            #endregion NOTE
            #region IF IsServerCode
            #region IF ServiceAttribute.IsRememberCommand
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] @RememberIdentityCommeandName()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[@MethodIndexs.Length];
                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                names[@MethodIndex].Set(@"@Method.MethodKeyName", @MethodIndex);
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
                return names;
            }
            #endregion IF ServiceAttribute.IsRememberCommand
            #endregion IF IsServerCode
            /// <summary>
            /// @ServerRegisterName TCP服务/*NOT:IsServerCode*/参数/*NOT:IsServerCode*/
            /// </summary>
            public sealed class TcpInternalSimpleServer/*IF:IsServerCode*/ : AutoCSer.Net.TcpInternalSimpleServer.Server/*IF:IsServerCode*/
            {
                #region IF IsServerCode
                public readonly @Type.FullName Value/*NOTE*/ = null/*NOTE*/;
                /// <summary>
                /// @ServerRegisterName TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
#region IF Type.Type.IsPublic
                /// <param name="value">TCP 服务目标对象</param>
#endregion IF Type.Type.IsPublic
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null/*IF:Type.Type.IsPublic*/, @Type.FullName value = null/*IF:Type.Type.IsPublic*/, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName))), verify, log, @IsCallQueue)
                {
                    Value =/*IF:Type.Type.IsPublic*/ value ?? /*IF:Type.Type.IsPublic*/new @Type.FullName();
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
                    #region IF IsSetTcpServer
                    Value.SetTcpServer(this);
                    #endregion IF IsSetTcpServer
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
                                    /*PUSH:Method*/
                                    Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/socket, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName>, bool>)/*NOTE*/socket.GetCallback<@OutputParameterTypeName/*NOT:IsVerifyMethod*/, @MethodReturnType.FullName/*NOT:IsVerifyMethod*/>(@MethodIdentityCommand, ref outputParameter));
                                    #endregion IF MethodIsReturn
                                    #region NOT MethodIsReturn
                                    /*PUSH:Method*/
                                    Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/socket, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue, bool>)/*NOTE*/socket.GetCallback());
                                    #endregion NOT MethodIsReturn
                                    return true;
                                    #endregion IF IsAsynchronousCallback
                                    #region NOT IsAsynchronousCallback
                                    #region IF IsMethodServerCall
                                    (@MethodStreamName/**/.Pop() ?? new @MethodStreamName()).Set(socket, Value, @ServerTask/*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
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
                                    #region IF Method.PropertyParameter
                                    @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/Value[/*IF:ClientParameterName*/socket/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/];
                                    #endregion IF Method.PropertyParameter
                                    #region NOT Method.PropertyParameter
                                    @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/Value.@PropertyName;
                                    #endregion NOT Method.PropertyParameter
                                    #endregion IF Method.IsGetMember
                                    #region NOT Method.IsGetMember
                                    #region IF Method.PropertyParameter
                                    Value[/*IF:ClientParameterName*/socket, /*IF:ClientParameterName*//*LOOP:InputParameters*//*NOT:MethodParameter.IsPropertyValue*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*NOT:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/] = /*LOOP:InputParameters*//*IF:MethodParameter.IsPropertyValue*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*IF:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/;
                                    #endregion IF Method.PropertyParameter
                                    #region NOT Method.PropertyParameter
                                    Value.@PropertyName = /*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/;
                                    #endregion NOT Method.PropertyParameter
                                    #endregion NOT Method.IsGetMember
                                    #endregion IF MemberIndex
                                    #region NOT MemberIndex
                                    /*IF:MethodIsReturn*/
                                    @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/socket/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/_outputParameter_.@ParameterName/*PUSH:InputParameter*//*IF:MethodParameter.IsOut*//*NOTE*/,/*NOTE*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*//*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/);
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
                                    return socket.Send(@MethodIdentityCommand, ref _outputParameter_);
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
                sealed class @MethodStreamName : AutoCSer.Net.TcpInternalSimpleServer.ServerCall<@MethodStreamName, @Type.FullName/*IF:InputParameterIndex*/, @InputParameterTypeName/*IF:InputParameterIndex*/>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ value)
                    {
                        try
                        {
                            /*IF:MethodIsReturn*/
                            @MethodReturnType.FullName @ReturnName;/*IF:MethodIsReturn*/
                            #region IF MemberIndex
                            #region IF Method.IsGetMember
                            #region IF Method.PropertyParameter
                            @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/serverValue[/*IF:ClientParameterName*/Socket/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/];
                            #endregion IF Method.PropertyParameter
                            #region NOT Method.PropertyParameter
                            @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/serverValue.@PropertyName;
                            #endregion NOT Method.PropertyParameter
                            #endregion IF Method.IsGetMember
                            #region NOT Method.IsGetMember
                            #region IF Method.PropertyParameter
                            serverValue[/*IF:ClientParameterName*/Socket, /*IF:ClientParameterName*//*LOOP:InputParameters*//*NOT:MethodParameter.IsPropertyValue*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*NOT:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/] = /*LOOP:InputParameters*//*IF:MethodParameter.IsPropertyValue*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*IF:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/;
                            #endregion IF Method.PropertyParameter
                            #region NOT Method.PropertyParameter
                            serverValue.@PropertyName = /*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/;
                            #endregion NOT Method.PropertyParameter
                            #endregion NOT Method.IsGetMember
                            #endregion IF MemberIndex

                            #region NOT MemberIndex
                            /*IF:MethodIsReturn*/
                            @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/serverValue.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/Socket/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/value.Value.@ParameterName/*PUSH:InputParameter*//*NOTE*/,/*NOTE*//*IF:MethodParameter.IsOut*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*AT:MethodParameter.ParameterJoin*//*LOOP:InputParameters*/);
                            #endregion NOT MemberIndex

                            #region IF OutputParameterIndex
                            #region IF IsVerifyMethod
                            if (/*NOTE*/(bool)(object)/*NOTE*/@ReturnName) Socket.SetVerifyMethod();
                            #endregion IF IsVerifyMethod
                            #region LOOP OutputParameters
                            #region NOT InputMethodParameter.IsOut
                            /*PUSH:Parameter*/
                            value.Value.@ParameterName/*PUSH:Parameter*/ = inputParameter/*PUSH:InputParameter*/.@ParameterName/*PUSH:InputParameter*/;
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
                        socket.SendAsync(@MethodIdentityCommand, ref value);
                        #endregion IF OutputParameterIndex
                        #region NOT OutputParameterIndex
                        socket.SendAsync(value.Type);
                        #endregion NOT OutputParameterIndex
                    }
                }
                #endregion IF IsMethodServerCall
                #endregion NOT IsAsynchronousCallback
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo @MethodIdentityCommand = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = @OutputParameterIndex/*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
                #region IF Attribute.IsCompileSerialize
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { /*LOOP:SimpleSerializeMethods*/typeof(@InputParameterTypeName), /*LOOP:SimpleSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SimpleDeSerializeMethods*/typeof(@OutputParameterTypeName), /*LOOP:SimpleDeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SerializeMethods*/typeof(@InputParameterTypeName), /*LOOP:SerializeMethods*/null }
                        , new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(@OutputParameterTypeName), /*LOOP:DeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonSerializeMethods*/typeof(@InputParameterTypeName), /*LOOP:JsonSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonDeSerializeMethods*/typeof(@OutputParameterTypeName), /*LOOP:JsonDeSerializeMethods*/null });
                }
                #endregion IF Attribute.IsCompileSerialize
                #endregion IF IsServerCode

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
            }
            #region IF IsClientCode
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                #region IF IsTimeVerify
                private bool _timerVerify_(TcpInternalSimpleClient client)
                {
                    return AutoCSer.Net.TcpInternalSimpleServer.TimeVerifyClient.Verify(verify, client._TcpClient_);
                }
                #region NOTE
                public AutoCSer.Net.TcpServer.ReturnValue<bool> verify(string userID, ulong randomPrefix, byte[] md5Data, ref long ticks) { return false; }
                #endregion NOTE
                #endregion IF IsTimeVerify
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                #region IF IsVerifyMethod
                /// <param name="verifyMethod">TCP 验证方法</param>
                #endregion IF IsVerifyMethod
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null/*IF:IsVerifyMethod*/, Func<TcpInternalSimpleClient, bool> verifyMethod = null/*IF:IsVerifyMethod*/, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        #region IF IsServerCode
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName));
                        #endregion IF IsServerCode
                        #region NOT IsServerCode
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute>("@ServerRegisterName") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "@ServerRegisterName";
                        #endregion NOT IsServerCode
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log/*IF:IsVerifyMethod*/, verifyMethod/*IF:IsTimeVerify*/ ?? (Func<TcpInternalSimpleClient, bool>)_timerVerify_/*IF:IsTimeVerify*//*IF:IsVerifyMethod*/);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }
                #region NOT IsServerCode
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute>(@"@AttributeJson"); }
                }
                #endregion NOT IsServerCode

                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase @MethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };

                #region NOT MemberIndex
                #region IF Method.XmlDocument
                /// <summary>
                /// @Method.XmlDocument/*IF:Attribute.IsExpired*/ [ Expired ]/*IF:Attribute.IsExpired*/
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
                /*AT:IsInternalClient*/
                AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    if (_isDisposed_ == 0)
                    {
                        #region IF InputParameterIndex
                        TcpInternalSimpleServer.@InputParameterTypeName _inputParameter_ = new TcpInternalSimpleServer.@InputParameterTypeName
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
                        TcpInternalSimpleServer.@OutputParameterTypeName _outputParameter_ = new TcpInternalSimpleServer.@OutputParameterTypeName
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
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get</*IF:InputParameterIndex*/TcpInternalSimpleServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpInternalSimpleServer.@OutputParameterTypeName>(@MethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                        #region LOOP OutputParameters
                        #region IF InputMethodParameter.IsRefOrOut
                        /*PUSH:MethodParameter*/
                        @ParameterName/*PUSH:MethodParameter*/ = _outputParameter_./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*/;
                        #endregion IF InputMethodParameter.IsRefOrOut
                        #endregion LOOP OutputParameters
                        return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnType_/*IF:MethodIsReturn*/, Value = _outputParameter_.Return/*IF:MethodIsReturn*/ };
                        #endregion IF OutputParameterIndex
                        #region NOT OutputParameterIndex
                        return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _TcpClient_.Call(@MethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/) };
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
                #endregion NOT MemberIndex

                #region IF MemberIndex
                #region IF MethodIsReturn
                #region IF Method.XmlDocument
                /// <summary>
                /// @Method.XmlDocument/*IF:Attribute.IsExpired*/ [ Expired ]/*IF:Attribute.IsExpired*/
                /// </summary>
                #endregion IF Method.XmlDocument
                #region LOOP InputParameters
                #region PUSH MethodParameter
                #region IF XmlDocument
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion IF XmlDocument
                #endregion PUSH MethodParameter
                #endregion LOOP InputParameters
                #region IF InputParameterIndex
                /*AT:IsInternalClient*/
                AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> this[/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/]
                {
                    get
                    {
                        #region NAME GetProperty
                        #region IF Attribute.IsExpired
                        return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired };
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        if (_isDisposed_ == 0)
                        {
                            #region IF InputParameterIndex
                            TcpInternalSimpleServer.@InputParameterTypeName _inputParameter_ = new TcpInternalSimpleServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            TcpInternalSimpleServer.@OutputParameterTypeName _outputParameter_ = default(TcpInternalSimpleServer.@OutputParameterTypeName);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get</*IF:InputParameterIndex*/TcpInternalSimpleServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpInternalSimpleServer.@OutputParameterTypeName>(@MethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                        #endregion NOT Attribute.IsExpired
                        #endregion NAME GetProperty
                    }
                    #region PUSH SetMethod
                    set
                    {
                        #region NAME SetProperty
                        #region IF Attribute.IsExpired
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer.@InputParameterTypeName _inputParameter_ = new TcpInternalSimpleServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(@MethodIdentityCommand, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                        #endregion NOT Attribute.IsExpired
                        #endregion NAME SetProperty
                    }
                    #endregion PUSH SetMethod
                }
                #endregion IF InputParameterIndex
                #region NOT InputParameterIndex
                /*AT:IsInternalClient*/
                AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> @PropertyName
                {
                    get
                    {
                        #region FROMNAME GetProperty
                        #endregion FROMNAME GetProperty
                        #region NOTE
                        return null;
                        #endregion NOTE
                    }
                    #region PUSH SetMethod
                    set
                    {
                        #region FROMNAME SetProperty
                        #endregion FROMNAME SetProperty
                    }
                    #endregion PUSH SetMethod
                }
                #endregion NOT InputParameterIndex
                #endregion IF MethodIsReturn
                #endregion IF MemberIndex
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
                #region IF Attribute.IsCompileSerialize
                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { /*LOOP:SimpleSerializeMethods*/typeof(TcpInternalSimpleServer.@InputParameterTypeName), /*LOOP:SimpleSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SimpleDeSerializeMethods*/typeof(TcpInternalSimpleServer.@OutputParameterTypeName), /*LOOP:SimpleDeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SerializeMethods*/typeof(TcpInternalSimpleServer.@InputParameterTypeName), /*LOOP:SerializeMethods*/null }
                        , new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(TcpInternalSimpleServer.@OutputParameterTypeName), /*LOOP:DeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonSerializeMethods*/typeof(TcpInternalSimpleServer.@InputParameterTypeName), /*LOOP:JsonSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonDeSerializeMethods*/typeof(TcpInternalSimpleServer.@OutputParameterTypeName), /*LOOP:JsonDeSerializeMethods*/null });
                }
                #endregion IF Attribute.IsCompileSerialize
            }
            #endregion IF IsClientCode
        }
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName
#if !NOJIT
            : AutoCSer.Net.TcpSimpleServer.ISetTcpServer<AutoCSer.Net.TcpInternalSimpleServer.Server, AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute>
#endif
        {
            /// <summary>
            /// 设置TCP服务端
            /// </summary>
            /// <param name="tcpServer">TCP服务端</param>
            public void SetTcpServer(AutoCSer.Net.TcpInternalSimpleServer.Server tcpServer) { }
        }
    }
    #endregion NOTE
}

