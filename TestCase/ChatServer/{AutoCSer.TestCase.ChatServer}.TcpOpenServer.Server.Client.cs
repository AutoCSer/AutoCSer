//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.ChatServer.TcpClient
{
        public partial class Server
        {
            /// <summary>
            /// AutoCSer.TestCase.ChatServer.Server TCP服务参数
            /// </summary>
            public sealed class TcpOpenServer
            {

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.ChatData.Message>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.TestCase.ChatData.Message Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.TestCase.ChatData.Message Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.TestCase.ChatData.Message)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.ChatData.UserLogin>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.TestCase.ChatData.UserLogin Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.TestCase.ChatData.UserLogin Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.TestCase.ChatData.UserLogin)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public string userName;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<bool>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public bool Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public bool Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (bool)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
                {
                    public string content;
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpOpenClient : AutoCSer.Net.TcpOpenServer.MethodClient<TcpOpenClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="verifyMethod">TCP验证方法</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpOpenClient(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<TcpOpenClient, AutoCSer.Net.TcpOpenServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenServer.ServerAttribute>("AutoCSer.TestCase.ChatServer.Server") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "AutoCSer.TestCase.ChatServer.Server";
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log, clientRoute, verifyMethod);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpOpenServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenServer.ServerAttribute>(@"{""BinaryDeSerializeMaxArraySize"":1024,""CheckSeconds"":59,""ClientFirstTryCreateSleep"":1000,""ClientOutputSleep"":-1,""ClientRouteType"":null,""ClientSegmentationCopyPath"":""..\\..\\..\\..\\ChatClient\\"",""ClientSendBufferMaxSize"":1048576,""ClientTryCreateSleep"":1000,""CommandPoolBitSize"":3,""GenericType"":null,""Host"":""127.0.0.1"",""IsAttribute"":true,""IsAutoClient"":true,""IsAutoServer"":true,""IsBaseTypeAttribute"":false,""IsClientAwaiter"":false,""IsCompileSerialize"":true,""IsJsonSerialize"":true,""IsMarkData"":false,""IsRemoteExpression"":false,""IsSegmentation"":true,""IsServerBuildOutputThread"":false,""IsSimpleSerialize"":true,""MaxCustomDataSize"":16372,""MaxInputSize"":16372,""MaxVerifyDataSize"":256,""MemberFilters"":""Instance"",""MinCompressSize"":0,""Name"":null,""Port"":12400,""ReceiveBufferSize"":""Kilobyte"",""ReceiveVerifyCommandSeconds"":9,""RemoteExpressionServerTask"":""Timeout"",""SendBufferSize"":""Kilobyte"",""ServerOutputSleep"":-1,""ServerSendBufferMaxSize"":0,""VerifyString"":null,""TypeId"":{}}"); }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsKeepCallback = 1 };
                /// <summary>
                /// 获取消息
                /// </summary>
                /// <returns>保持异步回调</returns>
                public AutoCSer.Net.TcpServer.KeepCallback getMessage(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.ChatData.Message>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.ChatData.Message, TcpOpenServer._p1>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<TcpOpenServer._p1>(_ac0, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsKeepCallback = 1 };
                /// <summary>
                /// 获取用户信息
                /// </summary>
                /// <returns>保持异步回调</returns>
                public AutoCSer.Net.TcpServer.KeepCallback getUser(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.ChatData.UserLogin>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.ChatData.UserLogin, TcpOpenServer._p2>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<TcpOpenServer._p2>(_ac1, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true };

                /// <summary>
                /// 用户登录
                /// </summary>
                /// <param name="userName">用户名称</param>
                /// <returns>是否成功</returns>
                public AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpOpenServer.ClientSocketSender _sender_, string userName)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                            {
                                
                                userName = userName,
                            };
                            TcpOpenServer._p4 _outputParameter_ = new TcpOpenServer._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p3, TcpOpenServer._p4>(_c2, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 用户退出
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue logout()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c3, ref _wait_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 发送消息
                /// </summary>
                /// <param name="content">消息内容</param>
                public AutoCSer.Net.TcpServer.ReturnValue send(string content)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p5 _inputParameter_ = new TcpOpenServer._p5
                            {
                                
                                content = content,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c4, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpOpenClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenServer._p3), typeof(TcpOpenServer._p5), null }
                        , new System.Type[] { typeof(TcpOpenServer._p1), typeof(TcpOpenServer._p2), typeof(TcpOpenServer._p4), null });
                }
            }
        }
}
#endif