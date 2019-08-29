//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Diagnostics
{
        public partial class ProcessCopyServer
#if !NOJIT
             : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalServer.Server, AutoCSer.Net.TcpInternalServer.ServerAttribute>
#endif
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[4];
                names[0].Set(@"(AutoCSer.Diagnostics.ProcessCopyer)copy", 0);
                names[1].Set(@"(AutoCSer.Net.TcpInternalServer.ServerSocketSender,string,ulong,byte[],ref long)verify", 1);
                names[2].Set(@"(AutoCSer.Diagnostics.ProcessCopyer)guard", 2);
                names[3].Set(@"(AutoCSer.Diagnostics.ProcessCopyer)remove", 3);
                return names;
            }
            /// <summary>
            /// ProcessCopy TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.Diagnostics.ProcessCopyServer Value;
                /// <summary>
                /// ProcessCopy TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Diagnostics.ProcessCopyServer value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("ProcessCopy", typeof(AutoCSer.Diagnostics.ProcessCopyServer))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.Diagnostics.ProcessCopyServer();
                    setCommandData(4);
                    setCommand(0);
                    setVerifyCommand(1);
                    setCommand(2);
                    setCommand(3);
                    Value.SetTcpServer(this);
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
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.copy(inputParameter.p0);
                                    sender.Push();
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    bool Return;
                                    
                                    Return = Value.verify(sender, inputParameter.p2, inputParameter.p3, inputParameter.p0, ref inputParameter.p1);
                                    if (Return) sender.SetVerifyMethod();
                                    
                                    _outputParameter_.p0 = inputParameter.p1;
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c1, ref _outputParameter_);
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.guard(inputParameter.p0);
                                    sender.Push();
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.remove(inputParameter.p0);
                                    sender.Push();
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        default: return;
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { typeof(_p3), null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public AutoCSer.Diagnostics.ProcessCopyer p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public byte[] p0;
                    public long p1;
                    public string p2;
                    public ulong p3;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<bool>
#endif
                {
                    public long p0;
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

            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalClient : AutoCSer.Net.TcpInternalServer.MethodClient<TcpInternalClient>
            {
                private bool _timerVerify_(TcpInternalClient client, AutoCSer.Net.TcpInternalServer.ClientSocketSender sender)
                {
                    return AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verify(verify, sender, _TcpClient_);
                }
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verifyMethod">TCP 验证方法</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("ProcessCopy", typeof(AutoCSer.Diagnostics.ProcessCopyServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute, verifyMethod ?? (Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool>)_timerVerify_);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 复制重启进程
                /// </summary>
                /// <param name="copyer">文件复制器</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue copy(AutoCSer.Diagnostics.ProcessCopyer copyer)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                p0 = copyer,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c0, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 复制重启进程
                /// </summary>
                /// <param name="copyer">文件复制器</param>
                public 
                AutoCSer.Net.TcpServer.Awaiter copyAwaiter(AutoCSer.Diagnostics.ProcessCopyer copyer)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            p0 = copyer,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a0, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public 
                AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                            {
                                
                                p2 = userID,
                                
                                p3 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            TcpInternalServer._p3 _outputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p2, TcpInternalServer._p3>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 守护进程
                /// </summary>
                /// <param name="copyer">文件信息</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue guard(AutoCSer.Diagnostics.ProcessCopyer copyer)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                p0 = copyer,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c2, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 守护进程
                /// </summary>
                /// <param name="copyer">文件信息</param>
                public 
                AutoCSer.Net.TcpServer.Awaiter guardAwaiter(AutoCSer.Diagnostics.ProcessCopyer copyer)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            p0 = copyer,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a2, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 删除守护进程
                /// </summary>
                /// <param name="copyer">文件信息</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue remove(AutoCSer.Diagnostics.ProcessCopyer copyer)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                p0 = copyer,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c3, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 删除守护进程
                /// </summary>
                /// <param name="copyer">文件信息</param>
                public 
                AutoCSer.Net.TcpServer.Awaiter removeAwaiter(AutoCSer.Diagnostics.ProcessCopyer copyer)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            p0 = copyer,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a3, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalServer._p3), null }
                        , new System.Type[] { typeof(TcpInternalServer._p1), typeof(TcpInternalServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Net.TcpRegister
{
        public partial class Server
#if !NOJIT
             : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalServer.Server, AutoCSer.Net.TcpInternalServer.ServerAttribute>
#endif
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[8];
                names[1].Set(@"(AutoCSer.Net.TcpInternalServer.ServerSocketSender,string,ulong,byte[],ref long)verify", 1);
                names[2].Set(@"()register", 2);
                names[3].Set(@"(AutoCSer.Net.TcpRegister.ClientId,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpRegister.Log>,bool>)getLog", 3);
                names[4].Set(@"(AutoCSer.Net.TcpRegister.ClientId,string)getPort", 4);
                names[5].Set(@"(long,AutoCSer.Net.TcpRegister.ServerInfo)register", 5);
                names[6].Set(@"(long,AutoCSer.Net.TcpRegister.ServerInfo)checkRegister", 6);
                names[7].Set(@"(long,AutoCSer.Net.TcpRegister.ServerInfo)removeRegister", 7);
                return names;
            }
            /// <summary>
            /// TcpRegister TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.Net.TcpRegister.Server Value;
                /// <summary>
                /// TcpRegister TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Net.TcpRegister.Server value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("TcpRegister", typeof(AutoCSer.Net.TcpRegister.Server))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.Net.TcpRegister.Server();
                    setCommandData(8);
                    setVerifyCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    Value.SetTcpServer(this);
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
                    switch (index - 128)
                    {
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    bool Return;
                                    
                                    Return = Value.verify(sender, inputParameter.p2, inputParameter.p3, inputParameter.p0, ref inputParameter.p1);
                                    if (Return) sender.SetVerifyMethod();
                                    
                                    _outputParameter_.p0 = inputParameter.p1;
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c1, ref _outputParameter_);
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    AutoCSer.Net.TcpRegister.ClientId Return;
                                    
                                    Return = Value.register();
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c2, ref _outputParameter_);
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p5 outputParameter = new _p5();
                                    
                                    Value.getLog(inputParameter.p0, sender.GetCallback<_p5, AutoCSer.Net.TcpRegister.Log>(_c3, ref outputParameter));
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 4:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 _outputParameter_ = new _p7();
                                    
                                    int Return;
                                    
                                    Return = Value.getPort(inputParameter.p0, inputParameter.p1);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c4, ref _outputParameter_);
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 5:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 _outputParameter_ = new _p9();
                                    
                                    bool Return;
                                    
                                    Return = Value.register(inputParameter.p1, inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c5, ref _outputParameter_);
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 _outputParameter_ = new _p9();
                                    
                                    bool Return;
                                    
                                    Return = Value.checkRegister(inputParameter.p1, inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c6, ref _outputParameter_);
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 7:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.removeRegister(inputParameter.p1, inputParameter.p0);
                                    sender.Push();
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        default: return;
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { typeof(_p2), typeof(_p7), typeof(_p9), null }
                        , new System.Type[] { typeof(_p1), typeof(_p4), typeof(_p6), typeof(_p8), null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public byte[] p0;
                    public long p1;
                    public string p2;
                    public ulong p3;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<bool>
#endif
                {
                    public long p0;
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
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Net.TcpRegister.ClientId>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.Net.TcpRegister.ClientId Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.Net.TcpRegister.ClientId Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.Net.TcpRegister.ClientId)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public AutoCSer.Net.TcpRegister.ClientId p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Net.TcpRegister.Log>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.Net.TcpRegister.Log Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.Net.TcpRegister.Log Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.Net.TcpRegister.Log)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p6
                {
                    public AutoCSer.Net.TcpRegister.ClientId p0;
                    public string p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p7
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p8
                {
                    public AutoCSer.Net.TcpRegister.ServerInfo p0;
                    public long p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p9
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

            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalClient : AutoCSer.Net.TcpInternalServer.MethodClient<TcpInternalClient>
            {
                private bool _timerVerify_(TcpInternalClient client, AutoCSer.Net.TcpInternalServer.ClientSocketSender sender)
                {
                    return AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verify(verify, sender, _TcpClient_);
                }
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verifyMethod">TCP 验证方法</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("TcpRegister", typeof(AutoCSer.Net.TcpRegister.Server));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute, verifyMethod ?? (Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool>)_timerVerify_);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public 
                AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                p2 = userID,
                                
                                p3 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            TcpInternalServer._p2 _outputParameter_ = new TcpInternalServer._p2
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p1, TcpInternalServer._p2>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// TCP 服务端注册
                /// </summary>
                /// <returns>TCP 服务端标识</returns>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpRegister.ClientId> register()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _outputParameter_ = new TcpInternalServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p3>(_c2, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpRegister.ClientId> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpRegister.ClientId> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.TcpQueue, IsKeepCallback = 1 };
                /// <summary>
                /// TCP 服务端轮询
                /// </summary>
                /// <param name="clientId">TCP 服务端标识</param>
                /// <returns>保持异步回调</returns>
                public 
                AutoCSer.Net.TcpServer.KeepCallback getLog(AutoCSer.Net.TcpRegister.ClientId clientId, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpRegister.Log>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p5>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.Net.TcpRegister.Log, TcpInternalServer._p5>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p4 _inputParameter_ = new TcpInternalServer._p4
                            {
                                
                                p0 = clientId,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p4, TcpInternalServer._p5>(_ac3, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p5> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p5> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 分配服务端口号
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> getPort(AutoCSer.Net.TcpRegister.ClientId clientId, string host)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = clientId,
                                
                                p1 = host,
                            };
                            TcpInternalServer._p7 _outputParameter_ = new TcpInternalServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p6, TcpInternalServer._p7>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 分配服务端口号
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBox<int> getPortAwaiter(AutoCSer.Net.TcpRegister.ClientId clientId, string host)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                        {
                            
                            p0 = clientId,
                            
                            p1 = host,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p6, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int>>(_a4, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 注册 TCP 服务信息
                /// </summary>
                /// <param name="ticks">全局注册标识</param>
                /// <param name="server">TCP 服务信息</param>
                /// <returns>注册状态</returns>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<bool> register(long ticks, AutoCSer.Net.TcpRegister.ServerInfo server)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p1 = ticks,
                                
                                p0 = server,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p8, TcpInternalServer._p9>(_c5, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 注册 TCP 服务信息
                /// </summary>
                /// <param name="ticks">全局注册标识</param>
                /// <param name="server">TCP 服务信息</param>
                /// <returns>注册状态</returns>
                public 
                AutoCSer.Net.TcpServer.AwaiterBox<bool> registerAwaiter(long ticks, AutoCSer.Net.TcpRegister.ServerInfo server)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<bool>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                        {
                            
                            p1 = ticks,
                            
                            p0 = server,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<bool>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p8, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<bool>>(_a5, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 失败重连检测 TCP 服务信息
                /// </summary>
                /// <param name="ticks">全局注册标识</param>
                /// <param name="server">TCP 服务信息</param>
                /// <returns>注册状态</returns>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<bool> checkRegister(long ticks, AutoCSer.Net.TcpRegister.ServerInfo server)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p1 = ticks,
                                
                                p0 = server,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p8, TcpInternalServer._p9>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 失败重连检测 TCP 服务信息
                /// </summary>
                /// <param name="ticks">全局注册标识</param>
                /// <param name="server">TCP 服务信息</param>
                /// <returns>注册状态</returns>
                public 
                AutoCSer.Net.TcpServer.AwaiterBox<bool> checkRegisterAwaiter(long ticks, AutoCSer.Net.TcpRegister.ServerInfo server)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<bool>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                        {
                            
                            p1 = ticks,
                            
                            p0 = server,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<bool>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p8, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<bool>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 注销TCP服务信息
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue removeRegister(long ticks, AutoCSer.Net.TcpRegister.ServerInfo server)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p1 = ticks,
                                
                                p0 = server,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c7, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 注销TCP服务信息
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.Awaiter removeRegisterAwaiter(long ticks, AutoCSer.Net.TcpRegister.ServerInfo server)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                        {
                            
                            p1 = ticks,
                            
                            p0 = server,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a7, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalServer._p2), typeof(TcpInternalServer._p7), typeof(TcpInternalServer._p9), null }
                        , new System.Type[] { typeof(TcpInternalServer._p1), typeof(TcpInternalServer._p4), typeof(TcpInternalServer._p6), typeof(TcpInternalServer._p8), null }
                        , new System.Type[] { typeof(TcpInternalServer._p3), typeof(TcpInternalServer._p5), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}
#endif