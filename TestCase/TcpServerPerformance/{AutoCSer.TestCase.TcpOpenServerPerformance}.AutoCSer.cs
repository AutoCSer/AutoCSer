//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.TcpOpenServerPerformance
{
        public partial class OpenServer
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[8];
                names[0].Set(@"(int,int)add", 0);
                names[1].Set(@"(int,int,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>,bool>)addAsynchronous", 1);
                names[2].Set(@"(int,int)addQueue", 2);
                names[3].Set(@"(int,int)addSynchronous", 3);
                names[4].Set(@"(int,int)addTcpTask", 4);
                names[5].Set(@"(int,int)addThreadPool", 5);
                names[6].Set(@"(int,int)addTimeoutTask", 6);
                names[7].Set(@"()gcCollect", 7);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer TCP服务
            /// </summary>
            public sealed class TcpOpenServer : AutoCSer.Net.TcpOpenServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="value">TCP服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpOpenServer(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer", typeof(AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer();
                    setCommandData(8);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
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
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    int Return;
                                    
                                    Return = Value.add(inputParameter.left, inputParameter.right);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c0, ref _outputParameter_);
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p4 outputParameter = new _p4();
                                    
                                    Value.addAsynchronous(inputParameter.left, inputParameter.right,  sender.GetCallback<_p4, AutoCSer.TestCase.TcpServerPerformance.Add>(_c1, ref outputParameter));
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, ref inputParameter);
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p4 _outputParameter_ = new _p4();
                                    
                                    AutoCSer.TestCase.TcpServerPerformance.Add Return;
                                    
                                    Return = Value.addSynchronous(inputParameter.left, inputParameter.right);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c3, ref _outputParameter_);
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpTask, ref inputParameter);
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ref inputParameter);
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        default: return;
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s2 : AutoCSer.Net.TcpOpenServer.ServerCall<_s2, AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addQueue(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
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
                        AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c2, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s4 : AutoCSer.Net.TcpOpenServer.ServerCall<_s4, AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addTcpTask(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
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
                        AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c4, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s5 : AutoCSer.Net.TcpOpenServer.ServerCall<_s5, AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addThreadPool(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
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
                        AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c5, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4 };
                sealed class _s6 : AutoCSer.Net.TcpOpenServer.ServerCall<_s6, AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addTimeoutTask(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
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
                        AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c6, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s7 : AutoCSer.Net.TcpOpenServer.ServerCall<_s7, AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.gcCollect();

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
                        AutoCSer.Net.TcpServer.ReturnValue value = new AutoCSer.Net.TcpServer.ReturnValue();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, value.Type);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                static TcpOpenServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), typeof(_p3), null }
                        , new System.Type[] { typeof(_p2), typeof(_p4), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
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
                internal struct _p3
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.TcpServerPerformance.Add>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.TestCase.TcpServerPerformance.Add Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.TestCase.TcpServerPerformance.Add Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.TestCase.TcpServerPerformance.Add)value; }
                    }
#endif
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
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpOpenClient(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer", typeof(AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log, clientRoute);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 客户端同步计算测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            TcpOpenServer._p2 _outputParameter_ = new TcpOpenServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p1, TcpOpenServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 客户端同步计算测试
                /// </summary>
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpOpenServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
#if !DOTNET2 && !DOTNET4
                /// <summary>
                /// 客户端同步计算测试
                /// </summary>
                public async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue<int>> addAsync(int left, int right)
                {
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpOpenServer._p2> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpOpenServer._p2>();
                        TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        TcpOpenServer._p2 _outputParameter_ = new TcpOpenServer._p2
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<TcpOpenServer._p1, TcpOpenServer._p2>(_a0, _wait_, ref _inputParameter_, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };
                /// <summary>
                /// 异步计算测试
                /// </summary>
                public void addAsynchronous(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpOpenServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            _socket_.Get<TcpOpenServer._p3, TcpOpenServer._p4>(_ac1, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };
                /// <summary>
                /// 计算队列测试
                /// </summary>
                public void addQueue(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpOpenServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            _socket_.Get<TcpOpenServer._p3, TcpOpenServer._p4>(_ac2, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };
                /// <summary>
                /// 简单计算测试
                /// </summary>
                public void addSynchronous(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpOpenServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            _socket_.Get<TcpOpenServer._p3, TcpOpenServer._p4>(_ac3, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };
                /// <summary>
                /// 计算任务测试
                /// </summary>
                public void addTcpTask(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpOpenServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            _socket_.Get<TcpOpenServer._p3, TcpOpenServer._p4>(_ac4, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };
                /// <summary>
                /// 计算任务测试
                /// </summary>
                public void addThreadPool(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpOpenServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            _socket_.Get<TcpOpenServer._p3, TcpOpenServer._p4>(_ac5, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };
                /// <summary>
                /// 计算任务测试
                /// </summary>
                public void addTimeoutTask(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpOpenServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            _socket_.Get<TcpOpenServer._p3, TcpOpenServer._p4>(_ac6, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// GC 垃圾回收
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue gcCollect()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c7, ref _wait_) };
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
                        , new System.Type[] { typeof(TcpOpenServer._p1), typeof(TcpOpenServer._p3), null }
                        , new System.Type[] { typeof(TcpOpenServer._p2), typeof(TcpOpenServer._p4), null });
                }
            }
        }
}
#endif