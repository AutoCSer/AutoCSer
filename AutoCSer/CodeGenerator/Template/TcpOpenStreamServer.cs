using System;
#pragma warning disable 649
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class TcpOpenStreamServer : Pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        #region IF IsSetTcpServer
        #region IF IsServerCode
#if !NOJIT
              : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpOpenStreamServer.Server, AutoCSer.Net.TcpOpenStreamServer.ServerAttribute>
#endif
        #endregion IF IsServerCode
        #endregion IF IsSetTcpServer
        {
            #region NOTE
            private const int MaxCommandLength = 0;
            private static FullName[] MethodIndexs = null;
            private const int MethodIndex = 0;
            private static FullName ParameterName = null;
            private const AutoCSer.Net.TcpServer.ClientTaskType ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Timeout;
            private const int CommandStartIndex = 0;
            private const int InputParameterIndex = 0;
            private const int OutputParameterIndex = 0;
            public void SetTcpServer(AutoCSer.Net.TcpOpenStreamServer.Server commandServer) { }
            #endregion NOTE
            #region IF IsServerCode
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
            #endregion IF IsServerCode
            /// <summary>
            /// @ServerRegisterName TCP服务/*NOT:IsServerCode*/参数/*NOT:IsServerCode*/
            /// </summary>
            public sealed class TcpOpenStreamServer/*IF:IsServerCode*/ : AutoCSer.Net.TcpOpenStreamServer.Server/*IF:IsServerCode*/
            {
                #region IF IsServerCode
                public readonly @Type.FullName Value/*NOTE*/ = null/*NOTE*/;
                /// <summary>
                /// @ServerRegisterName TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                #region IF Type.Type.IsPublic
                /// <param name="value">TCP服务目标对象</param>
                #endregion IF Type.Type.IsPublic
                /// <param name="log">日志接口</param>
                public TcpOpenStreamServer(AutoCSer.Net.TcpOpenStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null/*IF:Type.Type.IsPublic*/, @Type.FullName value = null/*IF:Type.Type.IsPublic*/, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName))), verify, log)
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
                /// <param name="sender">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                public override void DoCommand(int index, AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
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
                                    #region IF IsMethodServerCall
                                    (@MethodStreamName/**/.Pop() ?? new @MethodStreamName()).Set(sender, Value/*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
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
                                    #region IF Method.PropertyParameter
                                    @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/Value[/*IF:ClientParameterName*/sender/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/];
                                    #endregion IF Method.PropertyParameter
                                    #region NOT Method.PropertyParameter
                                    @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/Value.@PropertyName;
                                    #endregion NOT Method.PropertyParameter
                                    #endregion IF Method.IsGetMember
                                    #region NOT Method.IsGetMember
                                    #region IF Method.PropertyParameter
                                    Value[/*IF:ClientParameterName*/sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*NOT:MethodParameter.IsPropertyValue*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*NOT:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/] = /*LOOP:InputParameters*//*IF:MethodParameter.IsPropertyValue*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*IF:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/;
                                    #endregion IF Method.PropertyParameter
                                    #region NOT Method.PropertyParameter
                                    Value.@PropertyName = /*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/;
                                    #endregion NOT Method.PropertyParameter
                                    #endregion NOT Method.IsGetMember
                                    #endregion IF MemberIndex
                                    #region NOT MemberIndex
                                    /*IF:MethodIsReturn*/
                                    @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/sender/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/_outputParameter_.@ParameterName/*PUSH:InputParameter*//*IF:MethodParameter.IsOut*//*NOTE*/,/*NOTE*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*//*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/);
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
                                    sender.Push(@MethodIdentityCommand, ref _outputParameter_);
                                    #endregion IF OutputParameterIndex
                                    #region NOT OutputParameterIndex
                                    sender.Push();
                                    #endregion NOT OutputParameterIndex
                                    #endregion NOT IsClientSendOnly
                                    #endregion NOT IsMethodServerCall
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
                #region IF IsMethodServerCall
                sealed class @MethodStreamName : AutoCSer.Net.TcpOpenStreamServer.ServerCall<@MethodStreamName, @Type.FullName/*IF:InputParameterIndex*/, @InputParameterTypeName/*IF:InputParameterIndex*/>
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
                            @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/serverValue[/*IF:ClientParameterName*/Sender/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*LOOP:InputParameters*/];
                            #endregion IF Method.PropertyParameter
                            #region NOT Method.PropertyParameter
                            @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*/serverValue.@PropertyName;
                            #endregion NOT Method.PropertyParameter
                            #endregion IF Method.IsGetMember
                            #region NOT Method.IsGetMember
                            #region IF Method.PropertyParameter
                            serverValue[/*IF:ClientParameterName*/Sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*NOT:MethodParameter.IsPropertyValue*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName/*AT:ParameterJoin*//*PUSH:Parameter*//*NOT:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/] = /*LOOP:InputParameters*//*IF:MethodParameter.IsPropertyValue*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*IF:MethodParameter.IsPropertyValue*//*LOOP:InputParameters*/;
                            #endregion IF Method.PropertyParameter
                            #region NOT Method.PropertyParameter
                            serverValue.@PropertyName = /*LOOP:InputParameters*/inputParameter./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*//*LOOP:InputParameters*/;
                            #endregion NOT Method.PropertyParameter
                            #endregion NOT Method.IsGetMember
                            #endregion IF MemberIndex

                            #region NOT MemberIndex
                            /*IF:MethodIsReturn*/
                            @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/serverValue.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/Sender/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/value.Value.@ParameterName/*PUSH:InputParameter*//*NOTE*/,/*NOTE*//*IF:MethodParameter.IsOut*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*AT:MethodParameter.ParameterJoin*//*LOOP:InputParameters*/);
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
                        push(this);
                        #endregion IF IsClientSendOnly
                        #region NOT IsClientSendOnly
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            #region IF OutputParameterIndex
                            sender.Push(@MethodIdentityCommand, ref value);
                            #endregion IF OutputParameterIndex
                            #region NOT OutputParameterIndex
                            sender.Push(value.Type);
                            #endregion NOT OutputParameterIndex
                        }
                        else push(this);
                        #endregion NOT IsClientSendOnly
                    }
                }
                #endregion IF IsMethodServerCall
                private static readonly AutoCSer.Net.TcpServer.OutputInfo @MethodIdentityCommand = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = @OutputParameterIndex/*IF:IsJsonSerialize*/, IsKeepCallback = 1/*IF:IsJsonSerialize*//*IF:IsClientSendOnly*/, IsClientSendOnly = 1/*IF:IsClientSendOnly*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
                #region IF Attribute.IsCompileSerialize
                static TcpOpenStreamServer()
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
            public partial class TcpOpenStreamClient : AutoCSer.Net.TcpOpenStreamServer.MethodClient<TcpOpenStreamClient>
            {
                #region IF IsTimeVerify
                private bool _timerVerify_(TcpOpenStreamClient client, AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender sender)
                {
                    return AutoCSer.Net.TcpOpenStreamServer.TimeVerifyClient.Verify(verify, sender, _TcpClient_);
                }
                #region NOTE
                public AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks) { return false; }
                #endregion NOTE
                #endregion IF IsTimeVerify
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                #region IF IsVerifyMethod
                /// <param name="verifyMethod">TCP验证方法</param>
                #endregion IF IsVerifyMethod
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpOpenStreamClient(AutoCSer.Net.TcpOpenStreamServer.ServerAttribute attribute = null/*IF:IsVerifyMethod*/, Func<TcpOpenStreamClient, AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender, bool> verifyMethod = null/*IF:IsVerifyMethod*/, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        #region IF IsServerCode
                        attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName));
                        #endregion IF IsServerCode
                        #region NOT IsServerCode
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenStreamServer.ServerAttribute>("@ServerRegisterName") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "@ServerRegisterName";
                        #endregion NOT IsServerCode
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenStreamServer.Client<TcpOpenStreamClient>(this, attribute, log, clientRoute/*IF:OpenStreamClientRouteType*/ ?? new @OpenStreamClientRouteType()/*IF:OpenStreamClientRouteType*//*IF:IsVerifyMethod*/, verifyMethod/*IF:IsTimeVerify*/ ?? (Func<TcpOpenStreamClient, AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender, bool>)_timerVerify_/*IF:IsTimeVerify*//*IF:IsVerifyMethod*/);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }
                #region NOT IsServerCode
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpOpenStreamServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenStreamServer.ServerAttribute>(@"@AttributeJson"); }
                }
                #endregion NOT IsServerCode

                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                #region IF IsSynchronousMethodIdentityCommand
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @MethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsClientSendOnly*/, IsSendOnly = 1/*IF:IsClientSendOnly*/, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous/*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion IF IsSynchronousMethodIdentityCommand
                #region IF IsAwaiterMethodIdentityCommand
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @AwaiterMethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*/, TaskType = @ClientTask/*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion IF IsAwaiterMethodIdentityCommand

                #region NOT MemberIndex
                #region IF IsClientSendOnly
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
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    #region IF Attribute.IsExpired
                    throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    #region IF InputParameterIndex
                    TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
                    {
                        #region LOOP InputParameters
                        /*PUSH:Parameter*/
                        @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                        #endregion LOOP InputParameters
                    };
                    #endregion IF InputParameterIndex
                    _TcpClient_.Sender.CallOnly(@MethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                    #endregion NOT Attribute.IsExpired
                }
                #endregion IF IsClientSendOnly
                #region NOT IsClientSendOnly
                #region IF IsClientSynchronous
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
                public AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
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
                            TcpOpenStreamServer.@OutputParameterTypeName _outputParameter_ = new TcpOpenStreamServer.@OutputParameterTypeName
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
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet</*IF:InputParameterIndex*/TcpOpenStreamServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenStreamServer.@OutputParameterTypeName>(@MethodIdentityCommand, ref _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                            #region LOOP OutputParameters
                            #region IF InputMethodParameter.IsRefOrOut
                            /*PUSH:MethodParameter*/
                            @ParameterName/*PUSH:MethodParameter*/ = _outputParameter_./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*/;
                            #endregion IF InputMethodParameter.IsRefOrOut
                            #endregion LOOP OutputParameters
                            return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnType_/*IF:MethodIsReturn*/, Value = _outputParameter_.Return/*IF:MethodIsReturn*/ };
                            #endregion IF OutputParameterIndex
                            #region NOT OutputParameterIndex
                            return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _socket_.WaitCall(@MethodIdentityCommand, ref _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/) };
                            #endregion NOT OutputParameterIndex
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
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
                public AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@AwaiterMethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ _awaiter_ = new AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/();
                    #region IF Attribute.IsExpired
                    _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                    if (_socket_ != null)
                    {
                        #region IF InputParameterIndex
                        TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
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
                        _returnType_ = _socket_.GetAwaiter</*IF:InputParameterIndex*/TcpOpenStreamServer.@InputParameterTypeName, /*IF:InputParameterIndex*/AutoCSer.Net.TcpServer.@AwaiterReturnValue<@MethodReturnType.FullName>>(@AwaiterMethodIdentityCommand, _awaiter_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                        #endregion IF MethodIsReturn
                        #region NOT MethodIsReturn
                        _returnType_ = _socket_.GetAwaiter(@AwaiterMethodIdentityCommand, _awaiter_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
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
                public async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> /*PUSH:Method*/@TaskAsyncMethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                #region IF InputParameterIndex
                        TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
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
                        TcpOpenStreamServer.@OutputParameterTypeName _outputParameter_ = new TcpOpenStreamServer.@OutputParameterTypeName
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
                        if ((_returnType_ = _socket_.GetAsync</*IF:InputParameterIndex*/TcpOpenStreamServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenStreamServer.@OutputParameterTypeName>(@AwaiterMethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _returnOutputParameter_ = await _wait_;
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
                        _returnType_ = _socket_.CallAsync(@AwaiterMethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @MethodAsynchronousIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex, TaskType = @ClientTask/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
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
                /// <param name="_onReturn_">@Method.ReturnXmlDocument</param>
                #endregion IF Method.ReturnXmlDocument
                #region IF MethodIsReturn
                public void /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/{ Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/> _onOutput_ = _TcpClient_.GetCallback</*IF:MethodIsReturn*/@MethodReturnType.FullName, /*IF:MethodIsReturn*/TcpOpenStreamServer.@OutputParameterTypeName>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            _socket_.Get</*IF:InputParameterIndex*/TcpOpenStreamServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenStreamServer.@OutputParameterTypeName>(@MethodAsynchronousIdentityCommand, ref _onOutput_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    #endregion NOT Attribute.IsExpired
                }
                #endregion IF MethodIsReturn
                #region NOT MethodIsReturn
                public void /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            _socket_.Call(@MethodAsynchronousIdentityCommand, _onReturn_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            _onReturn_ = null;
                        }
                    }
                    finally
                    {
                        if (_onReturn_ != null) _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException });
                    }
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
                public AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> this[/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/]
                {
                    get
                    {
                        #region NAME GetProperty
                        #region IF Attribute.IsExpired
                        return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired };
                        #endregion IF Attribute.IsExpired
                        #region NOT Attribute.IsExpired
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                #region IF InputParameterIndex
                                TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
                                {
                                    #region LOOP InputParameters
                                    /*PUSH:Parameter*/
                                    @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                    #endregion LOOP InputParameters
                                };
                                #endregion IF InputParameterIndex
                                AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = _socket_.WaitGet</*IF:InputParameterIndex*/TcpOpenStreamServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenStreamServer.@OutputParameterTypeName>(@MethodIdentityCommand, ref _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                                return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenStreamServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
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
                        #region IF IsClientSendOnly
                        TcpOpenStreamServer.@InputParameterTypeName _sendOnlyInputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion LOOP InputParameters
                        };
                        _TcpClient_.Sender.CallOnly(@MethodIdentityCommand, ref _sendOnlyInputParameter_);
                        #endregion IF IsClientSendOnly
                        #region NOT IsClientSendOnly
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenStreamServer.@InputParameterTypeName _inputParameter_ = new TcpOpenStreamServer.@InputParameterTypeName
                                {
                                    #region LOOP InputParameters
                                    /*PUSH:Parameter*/
                                    @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                    #endregion LOOP InputParameters
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(@MethodIdentityCommand, ref _wait_, ref _inputParameter_);
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
                        #endregion NAME SetProperty
                    }
                    #endregion PUSH SetMethod
                }
                #endregion IF InputParameterIndex
                #region NOT InputParameterIndex
                public AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> @PropertyName
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
                static TcpOpenStreamClient()
                {
                    _compileSerialize_(new System.Type[] { /*LOOP:SimpleSerializeMethods*/typeof(TcpOpenStreamServer.@InputParameterTypeName), /*LOOP:SimpleSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SimpleDeSerializeMethods*/typeof(TcpOpenStreamServer.@OutputParameterTypeName), /*LOOP:SimpleDeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SerializeMethods*/typeof(TcpOpenStreamServer.@InputParameterTypeName), /*LOOP:SerializeMethods*/null }
                        , new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(TcpOpenStreamServer.@OutputParameterTypeName), /*LOOP:DeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonSerializeMethods*/typeof(TcpOpenStreamServer.@InputParameterTypeName), /*LOOP:JsonSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonDeSerializeMethods*/typeof(TcpOpenStreamServer.@OutputParameterTypeName), /*LOOP:JsonDeSerializeMethods*/null });
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
            : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpOpenStreamServer.Server, AutoCSer.Net.TcpOpenStreamServer.ServerAttribute>
#endif
        {
            /// <summary>
            /// 设置TCP服务端
            /// </summary>
            /// <param name="tcpServer">TCP服务端</param>
            public void SetTcpServer(AutoCSer.Net.TcpOpenStreamServer.Server tcpServer) { }
        }
        /// <summary>
        /// TCP 客户端路由
        /// </summary>
        public sealed class OpenStreamClientRouteType : ClientRouteType<AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender>
        {
        }
    }
    #endregion NOTE
}
