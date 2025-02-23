﻿using System;
using AutoCSer.Net.TcpInternalServer;
using AutoCSer.Net.TcpRegister;
using AutoCSer.Net.TcpServer;
#pragma warning disable 649
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class TcpInternalServer : Pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        #region IF IsSetTcpServer
        #region IF IsServerCode
#if !NOJIT
             : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalServer.Server>
#endif
        #endregion IF IsServerCode
        #endregion IF IsSetTcpServer
        {
            #region NOTE
            private const int MaxCommandLength = 0;
            private static FullName[] MethodIndexs = null;
            private const int MethodIndex = 0;
            private static FullName ParameterName = null;
            private const AutoCSer.Net.TcpServer.ServerTaskType ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout;
            private const AutoCSer.Net.TcpServer.ClientTaskType ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Timeout;
            private const int CommandStartIndex = 0;
            private const int InputParameterIndex = 0;
            private const int OutputParameterIndex = 0;
            private const ushort TimeoutSeconds = 0;
            private const ushort MaxTimeoutSeconds = 0;
            private const int CallQueueCount = 0;
            private const byte CallQueueIndex = 0;
            private const bool IsCallQueueLink = false;
            private const bool IsSynchronousVerifyMethod = false;
            public void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server commandServer) { }
            #endregion NOTE
            #region IF IsServerCode
            #region IF IsRememberCommand
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
            #endregion IF IsRememberCommand
            #endregion IF IsServerCode
            /// <summary>
            /// @ServerRegisterName TCP服务/*NOT:IsServerCode*/参数/*NOT:IsServerCode*/
            /// </summary>
            public sealed class TcpInternalServer/*IF:IsServerCode*/ : AutoCSer.Net.TcpInternalServer.Server/*IF:IsServerCode*/
            {
                #region IF IsServerCode
                public readonly @Type.FullName Value/*NOTE*/ = null/*NOTE*/;
                #region LOOP ServerCallQueueTypes
#if NOJIT
                private readonly AutoCSer.Net.TcpServer.IServerCallQueue @QueueName;
#else
                private readonly AutoCSer.Net.TcpServer.IServerCallQueue<@ServerCallQueueType.FullName> @QueueName;
#endif
                #endregion LOOP ServerCallQueueTypes
                /// <summary>
                /// @ServerRegisterName TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                #region IF Type.Type.IsPublic
                /// <param name="value">TCP 服务目标对象</param>
                #endregion IF Type.Type.IsPublic
                #region IF ServerCallQueueType
                /// <param name="serverCallQueue">自定义队列</param>
                #endregion IF ServerCallQueueType
                /// <param name="extendCommandBits">扩展服务命令二进制位数</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null/*IF:Type.Type.IsPublic*/, @Type.FullName value = null/*IF:Type.Type.IsPublic*//*IF:ServerCallQueueType*/, @ServerCallQueueType.FullName serverCallQueue = null/*IF:ServerCallQueueType*/, byte extendCommandBits = 0, Action<SubArray<byte>> onCustomData = null, AutoCSer.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName))), verify, /*IF:ServerCallQueueType*/serverCallQueue ?? new @ServerCallQueueType.FullName()/*IF:ServerCallQueueType*//*NOTE*/ ?? /*NOTE*//*NOT:ServerCallQueueType*/null/*NOT:ServerCallQueueType*/, extendCommandBits, onCustomData, log, @CallQueueCount, @IsCallQueueLink, @IsSynchronousVerifyMethod)
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
                    #region LOOP ServerCallQueueTypes
                    @QueueName = getServerCallQueue<@ServerCallQueueType.FullName>();
                    #endregion LOOP ServerCallQueueTypes
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
                                    #endregion IF MethodIsReturn
                                    #region IF IsMethodServerCall
                                    @MethodStreamName serverCall = @MethodStreamName/**/.Pop() ?? new @MethodStreamName();
                                    #region IF MethodIsReturn
                                    serverCall.AsynchronousCallback = sender.GetCallback<@OutputParameterTypeName, @MethodReturnType.FullName>(@MethodIdentityCommand, ref outputParameter);
                                    #endregion IF MethodIsReturn
                                    #region NOT MethodIsReturn
                                    serverCall.AsynchronousCallback = /*NOTE*/(AutoCSer.Net.TcpServer.ServerCallback<MethodReturnType.FullName>)(object)/*NOTE*/sender.GetCallback(@MethodIdentityCommand);
                                    #endregion NOT MethodIsReturn
                                    #region PUSH QueueType
                                    serverCall.Set(sender, Value, @QueueName/**/.Get(sender, ref inputParameter./*PUSH:ServerCallQueueKeyParameter*/@ParameterName/*PUSH:ServerCallQueueKeyParameter*/)/*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
                                    #endregion PUSH QueueType
                                    #region NOT QueueType
                                    serverCall.Set(sender, Value, @ServerTask/*IF:CallQueueIndex*/, @CallQueueIndex/*IF:CallQueueIndex*//*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
                                    #endregion NOT QueueType
                                    #endregion IF IsMethodServerCall
                                    #region NOT IsMethodServerCall
                                    #region IF MethodIsReturn
                                    /*PUSH:Method*/
                                    Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(AutoCSer.Net.TcpServer.ServerCallback<MethodReturnType.FullName>)(object)/*NOTE*/sender.GetCallback<@OutputParameterTypeName, @MethodReturnType.FullName>(@MethodIdentityCommand, ref outputParameter));
                                    #endregion IF MethodIsReturn
                                    #region NOT MethodIsReturn
                                    /*PUSH:Method*/
                                    Value.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(AutoCSer.Net.TcpServer.ServerCallback<MethodReturnType.FullName>)(object)/*NOTE*/sender.GetCallback(@MethodIdentityCommand));
                                    #endregion NOT MethodIsReturn
                                    #endregion NOT IsMethodServerCall
                                    #endregion IF IsAsynchronousCallback
                                    #region NOT IsAsynchronousCallback
                                    #region IF IsMethodServerCall
                                    #region PUSH QueueType
                                    (@MethodStreamName/**/.Pop() ?? new @MethodStreamName()).Set(sender, Value, @QueueName/**/.Get(sender, ref inputParameter./*PUSH:ServerCallQueueKeyParameter*/@ParameterName/*PUSH:ServerCallQueueKeyParameter*/)/*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
                                    #endregion PUSH QueueType
                                    #region NOT QueueType
                                    (@MethodStreamName/**/.Pop() ?? new @MethodStreamName()).Set(sender, Value, @ServerTask/*IF:CallQueueIndex*/, @CallQueueIndex/*IF:CallQueueIndex*//*IF:InputParameterIndex*/, ref inputParameter/*IF:InputParameterIndex*/);
                                    #endregion NOT QueueType
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
                sealed class @MethodStreamName : AutoCSer.Net.TcpInternalServer.ServerCall<@MethodStreamName, @Type.FullName/*IF:InputParameterIndex*/, @InputParameterTypeName/*IF:InputParameterIndex*/>
                {
                    #region IF IsAsynchronousCallback
                    internal AutoCSer.Net.TcpServer.ServerCallback/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ AsynchronousCallback;
                    #endregion IF IsAsynchronousCallback
                    #region NOT IsAsynchronousCallback
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
                            Sender.AddLog(error);
                        }
                    }
                    #endregion NOT IsAsynchronousCallback
                    public override void RunTask()
                    {
                        #region IF IsAsynchronousCallback
                        #region IF MethodIsReturn
                        /*PUSH:Method*/
                        serverValue.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/Sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*/AsynchronousCallback);
                        #endregion IF MethodIsReturn
                        #region NOT MethodIsReturn
                        /*PUSH:Method*/
                        serverValue.@MethodName/*PUSH:Method*/(/*IF:ClientParameterName*/Sender, /*IF:ClientParameterName*//*LOOP:InputParameters*//*AT:ParameterRef*//*PUSH:Parameter*/inputParameter.@ParameterName, /*PUSH:Parameter*//*LOOP:InputParameters*//*NOTE*/(Func<AutoCSer.Net.TcpServer.ReturnValue<MethodReturnType.FullName>, bool>)(object)/*NOTE*/AsynchronousCallback);
                        #endregion NOT MethodIsReturn
                        #endregion IF IsAsynchronousCallback
                        #region NOT IsAsynchronousCallback
                        AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/ value = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                        #region IF IsClientSendOnly
                        if (Sender.IsSocket) get(ref value);
                        #endregion IF IsClientSendOnly
                        #region NOT IsClientSendOnly
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            #region IF OutputParameterIndex
                            Sender.Push(CommandIndex, @MethodIdentityCommand, ref value);
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
                        #endregion NOT IsAsynchronousCallback
                    }
                }
                #endregion IF IsMethodServerCall
                private static readonly AutoCSer.Net.TcpServer.OutputInfo @MethodIdentityCommand = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = @OutputParameterIndex/*IF:IsKeepCallback*/, IsKeepCallback = 1/*IF:IsKeepCallback*//*IF:IsClientSendOnly*/, IsClientSendOnly = 1/*IF:IsClientSendOnly*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*//*IF:IsServerBuildOutputThread*/, IsBuildOutputThread = true/*IF:IsServerBuildOutputThread*/ };
                #endregion NOT IsNullMethod
                #endregion LOOP MethodIndexs
                #region IF Attribute.IsCompileSerialize
                static TcpInternalServer()
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
                [AutoCSer.BinarySerialize(IsMemberMap = false/*NOT:IsSerializeReferenceMember*/, IsReferenceMember = false/*NOT:IsSerializeReferenceMember*/)]
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
            public partial class TcpInternalClient : AutoCSer.Net.TcpInternalServer.MethodClient<TcpInternalClient>
            {
                #region IF IsTimeVerify
                private bool _timerVerify_(TcpInternalClient client, AutoCSer.Net.TcpInternalServer.ClientSocketSender sender)
                {
                    return AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verify(verify, sender, _TcpClient_);
                }
                #region NOTE
                public AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks) { return false; }
                #endregion NOTE
                #endregion IF IsTimeVerify
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                #region IF IsVerifyMethod
                /// <param name="verifyMethod">TCP 验证方法</param>
                #endregion IF IsVerifyMethod
                #region IF IsCreateClientWaitConnected
                /// <param name="waitConnectedOnCheckSocketVersion">等待连接套接字初始化处理</param>
                #endregion IF IsCreateClientWaitConnected
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null/*IF:IsVerifyMethod*/, Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod = null/*IF:IsVerifyMethod*//*IF:IsCreateClientWaitConnected*/, Action<AutoCSer.Net.TcpServer.ClientSocketEventParameter> waitConnectedOnCheckSocketVersion = null/*IF:IsCreateClientWaitConnected*/, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.ILog log = null)
                {
                    if (attribute == null)
                    {
                        #region IF IsServerCode
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("@ServerRegisterName", typeof(@Type.FullName));
                        #endregion IF IsServerCode
                        #region NOT IsServerCode
                        attribute = (AutoCSer.Net.TcpInternalServer.ServerAttribute)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Net.TcpInternalServer.ServerAttribute), "@ServerRegisterName") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "@ServerRegisterName";
                        #endregion NOT IsServerCode
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, @MaxTimeoutSeconds, onCustomData, log, clientRoute/*IF:ClientRouteType*/ ?? new @ClientRouteType()/*IF:ClientRouteType*//*IF:IsVerifyMethod*/, verifyMethod/*IF:IsTimeVerify*/ ?? (Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool>)_timerVerify_/*IF:IsTimeVerify*//*IF:IsVerifyMethod*/);
                    #region IF IsCreateClientWaitConnected
                    _TcpClient_.CreateWaitConnected(out _WaitConnected_, waitConnectedOnCheckSocketVersion);
                    #endregion IF IsCreateClientWaitConnected
                    #region NOT IsCreateClientWaitConnected
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                    #endregion NOT IsCreateClientWaitConnected
                }
                #region NOT IsServerCode
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpInternalServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.JsonDeSerializer.DeSerialize<AutoCSer.Net.TcpInternalServer.ServerAttribute>(@"@AttributeJson"); }
                }
                #endregion NOT IsServerCode

                #region LOOP MethodIndexs
                #region NOT IsNullMethod
                #region IF IsSynchronousMethodIdentityCommand
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @MethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:TimeoutSeconds*/, TimeoutSeconds = @TimeoutSeconds/*IF:TimeoutSeconds*//*IF:IsJsonSerialize*/, CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsClientSendOnly*/, IsSendOnly = 1/*IF:IsClientSendOnly*/, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous/*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
                #endregion IF IsSynchronousMethodIdentityCommand
                #region IF IsAwaiterMethodIdentityCommand
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @AwaiterMethodIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex/*IF:TimeoutSeconds*/, TimeoutSeconds = @TimeoutSeconds/*IF:TimeoutSeconds*//*IF:IsJsonSerialize*/, CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*/, TaskType = @ClientTask/*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
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
                /*AT:IsInternalClient*/
                void /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    #region IF Attribute.IsExpired
                    throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    #region IF IsClientWaitConnected
                    if (_WaitConnected_.WaitConnected())
                    #endregion IF IsClientWaitConnected
                    {
                        #region IF InputParameterIndex
                        TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion LOOP InputParameters
                        };
                        #endregion IF InputParameterIndex
                        _TcpClient_.Sender.CallOnly(@MethodIdentityCommand/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                    }
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
                /*AT:IsInternalClient*/
                AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    #region IF IsClientWaitConnected
                    if (!_WaitConnected_.WaitConnected())
                    {
                        #region LOOP InputParameters
                        #region PUSH MethodParameter
                        #region IF IsOut
                        @ParameterName = default(@ParameterType.FullName);
                        #endregion IF IsOut
                        #endregion PUSH MethodParameter
                        #endregion LOOP InputParameters
                        return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout };
                    }
                    #endregion IF IsClientWaitConnected
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.Pop();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = null;
                    try
                    {
                        _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
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
                            TcpInternalServer.@OutputParameterTypeName _outputParameter_ = new TcpInternalServer.@OutputParameterTypeName
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
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet</*IF:InputParameterIndex*/TcpInternalServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpInternalServer.@OutputParameterTypeName>(@MethodIdentityCommand, ref _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
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
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
                    }
                    #region LOOP InputParameters
                    #region PUSH MethodParameter
                    #region IF IsOut
                    @ParameterName = default(@ParameterType.FullName);
                    #endregion IF IsOut
                    #endregion PUSH MethodParameter
                    #endregion LOOP InputParameters
                    return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _socket_ == null ? AutoCSer.Net.TcpServer.ReturnType.ClientSocketNull : AutoCSer.Net.TcpServer.ReturnType.ClientException };
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
                /*AT:IsInternalClient*/
                AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ /*PUSH:Method*/@AwaiterMethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
                {
                    AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ _awaiter_ = new AutoCSer.Net.TcpServer.@Awaiter/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/();
                    #region IF Attribute.IsExpired
                    _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.VersionExpired);
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    #region IF IsClientWaitConnected
                    if (!_WaitConnected_.WaitConnected()) _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout);
                    #endregion IF IsClientWaitConnected
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                    if (_socket_ != null)
                    {
                        #region IF InputParameterIndex
                        TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
                        {
                            #region LOOP InputParameters
                            /*PUSH:Parameter*/
                            @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                            #endregion LOOP InputParameters
                        };
                        #endregion IF InputParameterIndex
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        #region IF MethodIsReturn
                        AutoCSer.Net.TcpServer.@AwaiterReturnValue<@MethodReturnType.FullName> _outputParameter_ = default(AutoCSer.Net.TcpServer.@AwaiterReturnValue<@MethodReturnType.FullName>);
                        _returnType_ = _socket_.GetAwaiter</*IF:InputParameterIndex*/TcpInternalServer.@InputParameterTypeName, /*IF:InputParameterIndex*/AutoCSer.Net.TcpServer.@AwaiterReturnValue<@MethodReturnType.FullName>>(@AwaiterMethodIdentityCommand, _awaiter_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_);
                        #endregion IF MethodIsReturn
                        #region NOT MethodIsReturn
                        _returnType_ = _socket_.GetAwaiter(@AwaiterMethodIdentityCommand, _awaiter_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                        #endregion NOT MethodIsReturn
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientSocketNull);
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
                /*AT:IsInternalClient*/
                async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> /*PUSH:Method*/@TaskAsyncMethodName/*PUSH:Method*/(/*IF:IsVerifyMethod*/AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, /*IF:IsVerifyMethod*//*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterJoinName/*PUSH:MethodParameter*//*LOOP:InputParameters*/)
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
                    #region IF IsClientWaitConnected
                    if (!_WaitConnected_.WaitConnected())
                    {
                        #region LOOP InputParameters
                        #region PUSH MethodParameter
                        #region IF IsOut
                        @ParameterName = default(@ParameterType.FullName);
                        #endregion IF IsOut
                        #endregion PUSH MethodParameter
                        #endregion LOOP InputParameters
                        return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout };
                    }
                    #endregion IF IsClientWaitConnected
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = /*NOT:IsVerifyMethod*/_TcpClient_.Sender/*NOT:IsVerifyMethod*//*NOTE*/ ?? /*NOTE*//*IF:IsVerifyMethod*/_sender_/*IF:IsVerifyMethod*/;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/();
                        #region IF InputParameterIndex
                        TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
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
                        TcpInternalServer.@OutputParameterTypeName _outputParameter_ = new TcpInternalServer.@OutputParameterTypeName
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
                        if ((_returnType_ = _socket_.GetAsync</*IF:InputParameterIndex*/TcpInternalServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpInternalServer.@OutputParameterTypeName>(@AwaiterMethodIdentityCommand, _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _returnOutputParameter_ = await _wait_;
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
                    return new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/ { Type = _socket_ == null ? AutoCSer.Net.TcpServer.ReturnType.ClientSocketNull : AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    #endregion NOT Attribute.IsExpired
                }
#endif
                #endregion IF IsClientTaskAsync
                #region IF IsClientAsynchronous
                private static readonly AutoCSer.Net.TcpServer.CommandInfo @MethodAsynchronousIdentityCommand = new AutoCSer.Net.TcpServer.CommandInfo { Command = @MethodIndex + @CommandStartIndex, InputParameterIndex = @InputParameterIndex, TaskType = @ClientTask/*IF:TimeoutSeconds*/, TimeoutSeconds = @TimeoutSeconds/*IF:TimeoutSeconds*//*IF:IsJsonSerialize*/, CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize/*IF:IsJsonSerialize*//*IF:IsKeepCallback*/, IsKeepCallback = 1/*IF:IsKeepCallback*//*IF:IsVerifyMethod*/, IsVerifyMethod = true/*IF:IsVerifyMethod*//*IF:IsSimpleSerializeInputParamter*/, IsSimpleSerializeInputParamter = true/*IF:IsSimpleSerializeInputParamter*//*IF:IsSimpleSerializeOutputParamter*/, IsSimpleSerializeOutputParamter = true/*IF:IsSimpleSerializeOutputParamter*/ };
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
                #region IF IsKeepCallback
                /// <returns>保持异步回调</returns>
                #endregion IF IsKeepCallback
                #region IF MethodIsReturn
                /*AT:IsInternalClient*/
                @KeepCallbackType /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/{ Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    #region IF IsClientWaitConnected
                    if (!_WaitConnected_.WaitConnected())
                    {
                        if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout.ToString());
                        _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/{ Type = AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout });
                    }
                    #endregion IF IsClientWaitConnected
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/> _onOutput_ = _TcpClient_.GetCallback</*IF:MethodIsReturn*/@MethodReturnType.FullName, /*IF:MethodIsReturn*/TcpInternalServer.@OutputParameterTypeName>(_onReturn_);
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = null;
                    try
                    {
                        _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            #region IF IsKeepCallback
                            return /*NOTE*/(KeepCallbackType)(object)/*NOTE*/_socket_.GetKeep</*IF:InputParameterIndex*/TcpInternalServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpInternalServer.@OutputParameterTypeName>(@MethodAsynchronousIdentityCommand, ref _onOutput_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            #endregion IF IsKeepCallback
                            #region NOT IsKeepCallback
                            _socket_.Get</*IF:InputParameterIndex*/TcpInternalServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpInternalServer.@OutputParameterTypeName>(@MethodAsynchronousIdentityCommand, ref _onOutput_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            #endregion NOT IsKeepCallback
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ { Type = _socket_ == null ? AutoCSer.Net.TcpServer.ReturnType.ClientSocketNull : AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    #endregion NOT Attribute.IsExpired
                    #region IF IsKeepCallback
                    return null;
                    #endregion IF IsKeepCallback
                }
                #endregion IF MethodIsReturn
                #region NOT MethodIsReturn
                /*AT:IsInternalClient*/
                @KeepCallbackType /*PUSH:Method*/@MethodName/*PUSH:Method*/(/*LOOP:InputParameters*//*PUSH:MethodParameter*/@ParameterTypeRefName @ParameterName, /*PUSH:MethodParameter*//*LOOP:InputParameters*/Action<AutoCSer.Net.TcpServer.ReturnValue> _onReturn_)
                {
                    #region IF Attribute.IsExpired
                    if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.VersionExpired.ToString());
                    _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.VersionExpired });
                    #endregion IF Attribute.IsExpired
                    #region NOT Attribute.IsExpired
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = null;
                    try
                    {
                        #region IF IsClientWaitConnected
                        if (!_WaitConnected_.WaitConnected())
                        {
                            if (_onReturn_ == null) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout.ToString());
                            _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout });
                        }
                        #endregion IF IsClientWaitConnected
                        _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            #region IF InputParameterIndex
                            TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            #endregion IF InputParameterIndex
                            #region IF IsKeepCallback
                            AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.CallKeep(@MethodAsynchronousIdentityCommand, _onReturn_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            _onReturn_ = null;
                            return /*NOTE*/(KeepCallbackType)(object)/*NOTE*/_keepCallback_;
                            #endregion IF IsKeepCallback
                            #region NOT IsKeepCallback
                            _socket_.Call(@MethodAsynchronousIdentityCommand, _onReturn_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                            _onReturn_ = null;
                            #endregion NOT IsKeepCallback
                        }
                    }
                    finally
                    {
                        if (_onReturn_ != null) _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_ == null ? AutoCSer.Net.TcpServer.ReturnType.ClientSocketNull : AutoCSer.Net.TcpServer.ReturnType.ClientException });
                    }
                    #endregion NOT Attribute.IsExpired
                    #region IF IsKeepCallback
                    return null;
                    #endregion IF IsKeepCallback
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
                        #region IF IsClientWaitConnected
                        if (!_WaitConnected_.WaitConnected())
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout };
                        }
                        #endregion IF IsClientWaitConnected
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.Pop();
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = null;
                        try
                        {
                            _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                #region IF InputParameterIndex
                                TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
                                {
                                    #region LOOP InputParameters
                                    /*PUSH:Parameter*/
                                    @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                    #endregion LOOP InputParameters
                                };
                                #endregion IF InputParameterIndex
                                AutoCSer.Net.TcpServer.ReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/ _outputParameter_ = _socket_.WaitGet</*IF:InputParameterIndex*/TcpInternalServer.@InputParameterTypeName, /*IF:InputParameterIndex*/TcpInternalServer.@OutputParameterTypeName>(@MethodIdentityCommand, ref _wait_/*IF:InputParameterIndex*/, ref _inputParameter_/*IF:InputParameterIndex*/);
                                return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue/*IF:OutputParameterIndex*/<TcpInternalServer.@OutputParameterTypeName>/*IF:OutputParameterIndex*/.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<@MethodReturnType.FullName> { Type = _socket_ == null ? AutoCSer.Net.TcpServer.ReturnType.ClientSocketNull : AutoCSer.Net.TcpServer.ReturnType.ClientException };
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
                        #region IF IsClientWaitConnected
                        if (_WaitConnected_.WaitConnected())
                        #endregion IF IsClientWaitConnected
                        {
                            TcpInternalServer.@InputParameterTypeName _sendOnlyInputParameter_ = new TcpInternalServer.@InputParameterTypeName
                            {
                                #region LOOP InputParameters
                                /*PUSH:Parameter*/
                                @ParameterName/*PUSH:Parameter*/ = /*NOTE*/(FullName)(object)/*NOTE*//*PUSH:MethodParameter*/@ParameterName/*PUSH:MethodParameter*/,
                                #endregion LOOP InputParameters
                            };
                            _TcpClient_.Sender.CallOnly(@MethodIdentityCommand, ref _sendOnlyInputParameter_);
                        }
                        #endregion IF IsClientSendOnly
                        #region NOT IsClientSendOnly
                        #region IF IsClientWaitConnected
                        if (!_WaitConnected_.WaitConnected()) throw new Exception(AutoCSer.Net.TcpServer.ReturnType.WaitConnectedTimeout.ToString());
                        #endregion IF IsClientWaitConnected
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalServer.@InputParameterTypeName _inputParameter_ = new TcpInternalServer.@InputParameterTypeName
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
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientSocketNull.ToString());
                        #endregion NOT IsClientSendOnly
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
                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { /*LOOP:SimpleSerializeMethods*/typeof(TcpInternalServer.@InputParameterTypeName), /*LOOP:SimpleSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SimpleDeSerializeMethods*/typeof(TcpInternalServer.@OutputParameterTypeName), /*LOOP:SimpleDeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:SerializeMethods*/typeof(TcpInternalServer.@InputParameterTypeName), /*LOOP:SerializeMethods*/null }
                        , new System.Type[] { /*LOOP:DeSerializeMethods*/typeof(TcpInternalServer.@OutputParameterTypeName), /*LOOP:DeSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonSerializeMethods*/typeof(TcpInternalServer.@InputParameterTypeName), /*LOOP:JsonSerializeMethods*/null }
                        , new System.Type[] { /*LOOP:JsonDeSerializeMethods*/typeof(TcpInternalServer.@OutputParameterTypeName), /*LOOP:JsonDeSerializeMethods*/null });
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
        /// 保持异步回调类型
        /// </summary>
        public class KeepCallbackType { }
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : AutoCSer.Net.TcpServer.IServerCallQueueSet
#if !NOJIT
            , AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalServer.Server>
#endif
        {
            /// <summary>
            /// 设置TCP服务端
            /// </summary>
            /// <param name="tcpServer">TCP服务端</param>
            public void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server tcpServer) { }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose() { }
#if NOJIT
            /// <summary>
            /// 获取 TCP 服务器端同步调用队列接口
            /// </summary>
            /// <typeparam name="queueKeyType">关键字类型</typeparam>
            /// <returns>TCP 服务器端同步调用队列接口</returns>
            public IServerCallQueue Get<queueKeyType>() { return null; }
#else
            /// <summary>
            /// 获取 TCP 服务器端同步调用队列接口
            /// </summary>
            /// <typeparam name="queueKeyType">关键字类型</typeparam>
            /// <returns>TCP 服务器端同步调用队列接口</returns>
            public IServerCallQueue<queueKeyType> Get<queueKeyType>() { return null; }
#endif
        }
        /// <summary>
        /// TCP 客户端路由
        /// </summary>
        /// <typeparam name="ClientSocketSenderType"></typeparam>
        public abstract class ClientRouteType<ClientSocketSenderType> : AutoCSer.Net.TcpServer.ClientLoadRoute<ClientSocketSenderType>
            where ClientSocketSenderType : AutoCSer.Net.TcpServer.ClientSocketSenderBase
        {
            public override ClientSocketSenderType Sender
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override ClientSocketBase Socket
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void DisposeSocket()
            {
                throw new NotImplementedException();
            }

            public override void OnDisposeSocket(ClientSocketBase socket)
            {
                throw new NotImplementedException();
            }

            public override void OnServerChange(ServerSet serverSet)
            {
                throw new NotImplementedException();
            }

            public override void OnSetSocket()
            {
                throw new NotImplementedException();
            }

            public override void TryCreateSocket()
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// TCP 客户端路由
        /// </summary>
        public sealed class ClientRouteType : ClientRouteType<AutoCSer.Net.TcpInternalServer.ClientSocketSender>
        {
        }
        /// <summary>
        /// 自定义队列
        /// </summary>
        public sealed class ServerCallQueueType : Pub
        {
        }
    }
    #endregion NOTE
}
