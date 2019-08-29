//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
        public partial class InternalStreamQueueServer
        {
            /// <summary>
            /// TcpInternalStreamQueueServerPerformance TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer Value;
                /// <summary>
                /// TcpInternalStreamQueueServerPerformance TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("TcpInternalStreamQueueServerPerformance", typeof(AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer))), verify, log)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer();
                    setCommandData(2);
                    setCommand(0);
                    setCommand(1);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="sender">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                public override void DoCommand(int index, AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.p0, inputParameter.p1);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addAsynchronous(inputParameter.p0, inputParameter.p1);

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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c1, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4 };
                static TcpInternalStreamServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), typeof(_p3), null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p4), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int p0;
                    public int p1;
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
                    public int p0;
                    public int p1;
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("TcpInternalStreamQueueServerPerformance", typeof(AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 客户端同步计算测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 客户端同步计算测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
#if !DOTNET2 && !DOTNET4
                /// <summary>
                /// 客户端同步计算测试
                /// </summary>
                public 
                async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue<int>> addAsync(int left, int right)
                {
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2>();
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_a0, _wait_, ref _inputParameter_, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p2> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 简单计算测试
                /// </summary>
                public 
                void addAsynchronous(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalStreamServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p3 _inputParameter_ = new TcpInternalStreamServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalStreamServer._p3, TcpInternalStreamServer._p4>(_ac1, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), typeof(TcpInternalStreamServer._p3), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p4), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
        public partial class InternalStreamServer
        {
            /// <summary>
            /// TcpInternalStreamServerPerformance TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer Value;
                /// <summary>
                /// TcpInternalStreamServerPerformance TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("TcpInternalStreamServerPerformance", typeof(AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer))), verify, log)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer();
                    setCommandData(2);
                    setCommand(0);
                    setCommand(1);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="sender">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                public override void DoCommand(int index, AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    int Return;
                                    
                                    Return = Value.add(inputParameter.p0, inputParameter.p1);
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
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p4 _outputParameter_ = new _p4();
                                    
                                    AutoCSer.TestCase.TcpServerPerformance.Add Return;
                                    
                                    Return = Value.addAsynchronous(inputParameter.p0, inputParameter.p1);
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
                        default: return;
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4 };
                static TcpInternalStreamServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), typeof(_p3), null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p4), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int p0;
                    public int p1;
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
                    public int p0;
                    public int p1;
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("TcpInternalStreamServerPerformance", typeof(AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
#if !DOTNET2 && !DOTNET4
                public 
                async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue<int>> addAsync(int left, int right)
                {
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2>();
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_a0, _wait_, ref _inputParameter_, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p2> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                public 
                void addAsynchronous(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalStreamServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p3 _inputParameter_ = new TcpInternalStreamServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalStreamServer._p3, TcpInternalStreamServer._p4>(_ac1, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), typeof(TcpInternalStreamServer._p3), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p4), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
        public partial class InternalStreamTcpQueueServer
        {
            /// <summary>
            /// TcpInternalStreamTcpQueueServerPerformance TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer Value;
                /// <summary>
                /// TcpInternalStreamTcpQueueServerPerformance TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("TcpInternalStreamTcpQueueServerPerformance", typeof(AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer))), verify, log)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer();
                    setCommandData(2);
                    setCommand(0);
                    setCommand(1);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="sender">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                public override void DoCommand(int index, AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.p0, inputParameter.p1);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addAsynchronous(inputParameter.p0, inputParameter.p1);

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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c1, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4 };
                static TcpInternalStreamServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), typeof(_p3), null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p4), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int p0;
                    public int p1;
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
                    public int p0;
                    public int p1;
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("TcpInternalStreamTcpQueueServerPerformance", typeof(AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
#if !DOTNET2 && !DOTNET4
                public 
                async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue<int>> addAsync(int left, int right)
                {
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2>();
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_a0, _wait_, ref _inputParameter_, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p2> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                public 
                void addAsynchronous(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalStreamServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p3 _inputParameter_ = new TcpInternalStreamServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalStreamServer._p3, TcpInternalStreamServer._p4>(_ac1, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), typeof(TcpInternalStreamServer._p3), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p4), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}
#endif