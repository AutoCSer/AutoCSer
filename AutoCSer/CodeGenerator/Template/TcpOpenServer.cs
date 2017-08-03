using System;
#pragma warning disable 649
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class TcpOpenServer : Pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        #region IF IsSetTcpServer
        #region IF IsServerCode
#if NOJIT
#else
              : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpOpenServer.Server, AutoCSer.Net.TcpOpenServer.ServerAttribute>
#endif
        #endregion IF IsServerCode
        #endregion IF IsSetTcpServer
        {
            #region NOTE
            private const int MaxCommandLength = 0;
            private static FullName[] MethodIndexs = null;
            private const int MethodIndex = 0;
            private const byte IsClientSendOnly = 0;
            private static FullName ParameterName = null;
            private const AutoCSer.Net.TcpServer.ServerTaskType ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout;
            private const AutoCSer.Net.TcpServer.ClientTaskType ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Timeout;
            private const int CommandStartIndex = 0;
            private const int InputParameterIndex = 0;
            private const int OutputParameterIndex = 0;
            private const byte IsKeepCallback = 0;
            public void SetTcpServer(AutoCSer.Net.TcpOpenServer.Server commandServer) { }
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
            public sealed class TcpOpenServer/*IF:IsServerCode*/ : AutoCSer.Net.TcpOpenServer.Server/*IF:IsServerCode*/
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
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpOpenServer(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null/*IF:Type.Type.IsPublic*/, @Type.FullName value = null/*IF:Type.Type.IsPublic*/, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName))), verify, onCustomData, log)
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
                public override void DoCommand(int index, AutoCSer.Net.TcpOpenServer.ServerSocketSender sender, ref SubArray<byte> data)
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
                                    Func<AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName>, bool> callbackReturn = sender.GetCallback<@OutputParameterTypeName, @MethodReturnType.FullName>(@MethodIdentityCommand, ref outputParameter);
                                    if (callbackReturn != null)
                                    {
                                        /*PUSH:Method*/
                                        Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName>, bool>)/*NOTE*/callbackReturn);
                                    }
                                    #endregion IF MethodIsReturn
                                    #region NOT MethodIsReturn
                                    Func<AutoCSer.Net.TcpServer.ReturnValue, bool> callback/*IF:IsClientSendOnly*/ = null/*IF:IsClientSendOnly*/;
                                    #region NOT IsClientSendOnly
                                    if ((callback = sender.GetCallback(@MethodIdentityCommand)) != null)
                                    #endregion NOT IsClientSendOnly
                                    {
                                        /*PUSH:Method*/
                                        Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue, bool>)/*NOTE*/callback);
                                    }
                                    #endregion NOT MethodIsReturn
                                    #endregion IF IsAsynchronousCallback
                                    #region NOT IsAsynchronousCallback
                                    #region IF IsMethodServerCall
                                    @MethodStreamName/**/.Call(sender, Value, @ServerTask/*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
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
                                sender.Log(error);
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
                sealed class @MethodStreamName : AutoCSer.Net.TcpOpenServer.ServerCall<@MethodStreamName, @Type.FullName/*IF:InputParameterIndex*/, @InputParameterTypeName/*IF:InputParameterIndex*/>
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
                            @ReturnName = /*NOTE*/(MethodReturnType.FullName)/*NOTE*//*IF:MethodIsReturn*//*PUSH:Method*/serverValue.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/Sender/*IF:InputParameters.Length*/, /*IF:InputParameters.Length*//*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*IF:MethodParameter.IsOut*//*PUSH:InputParameter*/value.Value.@ParameterName/*PUSH:InputParameter*//*NOTE*/,/*NOTE*//*IF:MethodParameter.IsOut*//*NOT:MethodParameter.IsOut*//*PUSH:Parameter*/inputParameter.@ParameterName/*PUSH:Parameter*//*NOT:MethodParameter.IsOut*//*AT:Parameter.ParameterJoin*//*LOOP:InputParameters*/);
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
                            Sender.Log(error);
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
                            Sender.Push(CommandIndex/*IF:OutputParameterIndex*/, @MethodIdentityCommand/*IF:OutputParameterIndex*/, ref value);
                        }
                        #endregion NOT IsClientSendOnly
                        push(this);
                    }
                }
                #endregion IF IsMethodServerCall
                #endregion NOT IsAsynchronousCallback
                private static readonly AutoCSer.Net.TcpServer.OutputInfo @MethodIdentityCommand = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = @OutputParameterIndex/*IF:IsKeepCallback*/, IsKeepCallback = @IsKeepCallback/*IF:IsKeepCallback*//*IF:IsClientSendOnly*/, IsClientSendOnly = @IsClientSendOnly/*IF:IsClientSendOnly*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
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
                public struct OutputParameterTypeName : AutoCSer.Net.IReturnParameter<MethodReturnType.FullName>
                {
                    public FullName ParameterName;
                    public MethodReturnType.FullName ReturnName;
                    public MethodReturnType.FullName Ret;
                    public MethodReturnType.FullName Return { get; set; }
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
            public class TcpOpenClient : AutoCSer.Net.TcpOpenServer.MethodClient<TcpOpenClient>
            {
                #region IF IsTimeVerify
                private bool _timerVerify_(TcpOpenClient client, AutoCSer.Net.TcpOpenServer.ClientSocketSender sender)
                {
                    return AutoCSer.Net.TcpOpenServer.TimeVerifyClient.Verify(verify, sender, _TcpClient_);
                }
                #region NOTE
                public AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpOpenServer.ClientSocketSender sender, ulong randomPrefix, byte[] md5Data, ref long ticks) { return false; }
                #endregion NOTE
                #endregion IF IsTimeVerify
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                #region IF IsVerifyMethod
                /// <param name="verifyMethod">TCP验证方法</param>
                #endregion IF IsVerifyMethod
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpOpenClient(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null/*IF:IsVerifyMethod*/, Func<TcpOpenClient, AutoCSer.Net.TcpOpenServer.ClientSocketSender, bool> verifyMethod = null/*IF:IsVerifyMethod*/, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        #region IF IsServerCode
                        attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName));
                        #endregion IF IsServerCode
                        #region NOT IsServerCode
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenServer.ServerAttribute>("@ServerRegisterName") ?? AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenServer.ServerAttribute>(@"@AttributeJson");
                        if (attribute.Name == null) attribute.Name = "@ServerRegisterName";
                        #endregion NOT IsServerCode
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log/*IF:IsVerifyMethod*/, verifyMethod/*IF:IsTimeVerify*/ ?? (Func<TcpOpenClient, AutoCSer.Net.TcpOpenServer.ClientSocketSender, bool>)_timerVerify_/*IF:IsTimeVerify*//*IF:IsVerifyMethod*/);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                #region IF IsSynchronousMethodIdentityCommand
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @MethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*/, IsSendOnly = @IsClientSendOnly, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous/*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion IF IsSynchronousMethodIdentityCommand

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
                public void /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    #region IF Attribute.IsExpired
                    throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    #region IF InputParameterIndex
                    TcpOpenServer.@InputParameterTypeName _inputParameter_ = new TcpOpenServer.@InputParameterTypeName
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
                public AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpOpenServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = _TcpClient_.GetAutoWait/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                            if (_socket_ != null)
                            {
                                #region IF InputParameterIndex
                                TcpOpenServer.@InputParameterTypeName _inputParameter_ = new TcpOpenServer.@InputParameterTypeName
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
                                TcpOpenServer.@OutputParameterTypeName _outputParameter_ = new TcpOpenServer.@OutputParameterTypeName
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
                                _socket_.Get</*IF:InputParameterIndex*/TcpOpenServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenServer.@OutputParameterTypeName>(@MethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                #region LOOP OutputParameters
                                #region IF InputMethodParameter.IsRefOrOut
                                /*PUSH:MethodParameter*/
                                @ParameterName/*PUSH:MethodParameter*/ = _returnOutputParameter_.Value./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*/;
                                #endregion IF InputMethodParameter.IsRefOrOut
                                #endregion LOOP OutputParameters
                                return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnOutputParameter_.Type/*IF:MethodIsReturn*/, Value = _returnOutputParameter_.Value.Return/*IF:MethodIsReturn*/ };
                                #endregion IF OutputParameterIndex
                                #region NOT OutputParameterIndex
                                _socket_.Call(@MethodIdentityCommand, /*NOTE*/(Action<AutoCSer.Net.TcpServer.ReturnValue>)(object)/*NOTE*/_wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                                _isWait_ = 1;
                                return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _wait_.Wait() };
                                #endregion NOT OutputParameterIndex
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
                        }
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
                #region IF IsClientTaskAsync
#if DOTNET2
#else
#if DOTNET4
#else
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
                public async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> /*PUSH:Method*/@TaskAsyncMethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpOpenServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                        if (_wait_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpOpenServer.@InputParameterTypeName _inputParameter_ = new TcpOpenServer.@InputParameterTypeName
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
                            TcpOpenServer.@OutputParameterTypeName _outputParameter_ = new TcpOpenServer.@OutputParameterTypeName
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
                            _socket_.Get</*IF:InputParameterIndex*/TcpOpenServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenServer.@OutputParameterTypeName>(@MethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                            AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _returnOutputParameter_ = await _wait_;
                            #region LOOP OutputParameters
                            #region IF InputMethodParameter.IsRefOrOut
                            /*PUSH:MethodParameter*/
                            @ParameterName/*PUSH:MethodParameter*/ = _returnOutputParameter_.Value./*PUSH:Parameter*/@ParameterName/*PUSH:Parameter*/;
                            #endregion IF InputMethodParameter.IsRefOrOut
                            #endregion LOOP OutputParameters
                            return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _returnOutputParameter_.Type/*IF:MethodIsReturn*/, Value = _returnOutputParameter_.Value.Return/*IF:MethodIsReturn*/ };
                            #endregion IF OutputParameterIndex
                            #region NOT OutputParameterIndex
                            _socket_.Call(@MethodIdentityCommand, /*NOTE*/(Action<AutoCSer.Net.TcpServer.ReturnValue>)(object)/*NOTE*/_wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = /*NOTE*/(AutoCSer.Net.TcpServer.ReturnType)(object)/*NOTE*/await _wait_ };
                            #endregion NOT OutputParameterIndex
                        }
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
#endif
                #endregion IF IsClientTaskAsync
                #endregion IF IsClientSynchronous
                #region IF IsClientAsynchronous
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @MethodAsynchronousIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex, TaskType = @ClientTask/*IF:IsJsonSerialize*/ , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsKeepCallback*/, IsKeepCallback = @IsKeepCallback/*IF:IsKeepCallback*//*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
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
                public @KeepCallbackType /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/{ Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/> _onOutput_ = _TcpClient_.GetCallback</*IF:MethodIsReturn*/@MethodReturnType.FullName, /*IF:MethodIsReturn*/TcpOpenServer.@OutputParameterTypeName>(_onReturn_);
                    if (_onReturn_ == null || _onOutput_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                #region IF InputParameterIndex
                                TcpOpenServer.@InputParameterTypeName _inputParameter_ = new TcpOpenServer.@InputParameterTypeName
                                {
                                    #region LOOP InputParameters
                                    /*PUSH:Parameter*/
                                    @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                    #endregion LOOP InputParameters
                                };
                                #endregion IF InputParameterIndex
                                #region IF IsKeepCallback
                                AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.Get</*IF:InputParameterIndex*/TcpOpenServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenServer.@OutputParameterTypeName>(@MethodAsynchronousIdentityCommand, _onOutput_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                                _isWait_ = 1;
                                return /*NOTE*/(KeepCallbackType)(object)/*NOTE*/_keepCallback_;
                                #endregion IF IsKeepCallback
                                #region NOT IsKeepCallback
                                _socket_.Get</*IF:InputParameterIndex*/TcpOpenServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenServer.@OutputParameterTypeName>(@MethodAsynchronousIdentityCommand, _onOutput_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                                _isWait_ = 1;
                                #endregion NOT IsKeepCallback
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                                if (_onOutput_ != null) _onOutput_.Call(ref _outputParameter_);
                            }
                        }
                    }
                    #region IF IsKeepCallback
                    return null;
                    #endregion IF IsKeepCallback
                    #endregion NOT Attribute.IsExpired
                }
                #endregion IF MethodIsReturn
                #region NOT MethodIsReturn
                public @KeepCallbackType /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    int _isCall_ = 0;
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpOpenServer.@InputParameterTypeName _inputParameter_ = new TcpOpenServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            #region IF IsKeepCallback
                            AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.Call(@MethodAsynchronousIdentityCommand, _onReturn_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            _isCall_ = 1;
                            return /*NOTE*/(KeepCallbackType)(object)/*NOTE*/_keepCallback_;
                            #endregion IF IsKeepCallback
                            #region NOT IsKeepCallback
                            _socket_.Call(@MethodAsynchronousIdentityCommand, _onReturn_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            _isCall_ = 1;
                            #endregion NOT IsKeepCallback
                        }
                    }
                    finally
                    {
                        if (_isCall_ == 0 && _onReturn_ != null) _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException });
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
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = _TcpClient_.GetAutoWait/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                        if (_wait_ != null)
                        {
                            int _isWait_ = 0;
                            try
                            {
                                AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                                if (_socket_ != null)
                                {
                                    #region IF InputParameterIndex
                                    TcpOpenServer.@InputParameterTypeName _inputParameter_ = new TcpOpenServer.@InputParameterTypeName
                                    {
                                        #region LOOP InputParameters
                                        /*PUSH:Parameter*/
                                        @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                        #endregion LOOP InputParameters
                                    };
                                    #endregion IF InputParameterIndex
                                    _socket_.Get</*IF:InputParameterIndex*/TcpOpenServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpOpenServer.@OutputParameterTypeName>(@MethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                                    _isWait_ = 1;
                                    AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                                    _wait_.Get(out _outputParameter_);
                                    return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                                }
                            }
                            finally
                            {
                                if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpOpenServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
                            }
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
                        TcpOpenServer.@InputParameterTypeName _sendOnlyInputParameter_ = new TcpOpenServer.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion LOOP InputParameters
                        };
                        _TcpClient_.Sender.CallOnly(@MethodIdentityCommand, ref _sendOnlyInputParameter_);
                        #endregion IF IsClientSendOnly
                        #region NOT IsClientSendOnly
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = _TcpClient_.GetAutoWait();
                        if (_wait_ != null)
                        {
                            int _isWait_ = 0;
                            try
                            {
                                AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                                if (_socket_ != null)
                                {
                                    TcpOpenServer.@InputParameterTypeName _inputParameter_ = new TcpOpenServer.@InputParameterTypeName
                                    {
                                        #region LOOP InputParameters
                                        /*PUSH:Parameter*/
                                        @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                        #endregion LOOP InputParameters
                                    };
                                    _socket_.Call(@MethodIdentityCommand, _wait_, ref _inputParameter_);
                                    _isWait_ = 1;
                                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = _wait_.Wait();
                                    if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                                    throw new Exception(_returnType_.ToString());
                                }
                            }
                            finally
                            {
                                if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                            }
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
        public partial class FullName : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpOpenServer.Server, AutoCSer.Net.TcpOpenServer.ServerAttribute>
        {
            /// <summary>
            /// 设置TCP服务端
            /// </summary>
            /// <param name="tcpServer">TCP服务端</param>
            public void SetTcpServer(AutoCSer.Net.TcpOpenServer.Server tcpServer) { }
        }
    }
    #endregion NOTE
}
