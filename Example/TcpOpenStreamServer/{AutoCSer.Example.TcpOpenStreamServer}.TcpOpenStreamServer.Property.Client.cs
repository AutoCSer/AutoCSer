//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpOpenStreamServer.TcpStreamClient
{
        internal partial class Property
        {
            /// <summary>
            /// AutoCSer.Example.TcpOpenStreamServer.Property TCP服务参数
            /// </summary>
            public sealed class TcpOpenStreamServer
            {

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public int Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public int Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (int)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int index;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int index;
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int value;
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpOpenStreamClient : AutoCSer.Net.TcpOpenStreamServer.MethodClient<TcpOpenStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpOpenStreamClient(AutoCSer.Net.TcpOpenStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenStreamServer.ServerAttribute>("AutoCSer.Example.TcpOpenStreamServer.Property") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "AutoCSer.Example.TcpOpenStreamServer.Property";
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenStreamServer.Client<TcpOpenStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpOpenStreamServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenStreamServer.ServerAttribute>(@"{""BinaryDeSerializeMaxArraySize"":1024,""CheckSeconds"":59,""ClientFirstTryCreateSleep"":1000,""ClientOutputSleep"":-1,""ClientRouteType"":null,""ClientSegmentationCopyPath"":null,""ClientSendBufferMaxSize"":1048576,""ClientTryCreateSleep"":1000,""GenericType"":null,""Host"":""127.0.0.1"",""IsAttribute"":true,""IsAutoClient"":false,""IsAutoServer"":true,""IsBaseTypeAttribute"":false,""IsClientAwaiter"":false,""IsCompileSerialize"":true,""IsJsonSerialize"":true,""IsMarkData"":false,""IsRemoteExpression"":false,""IsSegmentation"":true,""IsServerBuildOutputThread"":false,""IsSimpleSerialize"":true,""MaxInputSize"":16372,""MaxVerifyDataSize"":256,""MemberFilters"":""Instance"",""MinCompressSize"":0,""Name"":null,""Port"":13703,""ReceiveBufferSize"":""Kilobyte8"",""ReceiveVerifyCommandSeconds"":9,""RemoteExpressionServerTask"":""Timeout"",""SendBufferSize"":""Kilobyte8"",""ServerOutputSleep"":-1,""ServerSendBufferMaxSize"":0,""ServerTaskType"":""Queue"",""VerifyString"":null,""TypeId"":{}}"); }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                /// <summary>
                /// 只读属性支持
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> GetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenStreamServer._p1>(_c0, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                                {
                                    
                                    index = index,
                                };
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenStreamServer._p2, TcpOpenStreamServer._p1>(_c1, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenStreamServer._p3 _inputParameter_ = new TcpOpenStreamServer._p3
                                {
                                    
                                    index = index,
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c2, ref _wait_, ref _inputParameter_);
                                if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                                throw new Exception(_returnType_.ToString());
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                /// <summary>
                /// 可写属性支持
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> SetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenStreamServer._p1>(_c3, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenStreamServer._p4 _inputParameter_ = new TcpOpenStreamServer._p4
                                {
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c4, ref _wait_, ref _inputParameter_);
                                if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                                throw new Exception(_returnType_.ToString());
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                static TcpOpenStreamClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p2), typeof(TcpOpenStreamServer._p3), typeof(TcpOpenStreamServer._p4), null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p1), null });
                }
            }
        }
}
#endif