//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
        public partial class InternalServer
        {
            /// <summary>
            /// TcpInternalServerPerformance TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer Value;
                /// <summary>
                /// TcpInternalServerPerformance TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("TcpInternalServerPerformance", typeof(AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer();
                    setCommandData(13);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
                    setCommand(9);
                    setCommand(10);
                    setCommand(11);
                    setCommand(12);
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
                                    _p4 outputParameter = new _p4();
                                    
                                    Value.addAsynchronous(inputParameter.p0, inputParameter.p1, sender.GetCallback<_p4, AutoCSer.TestCase.TcpServerPerformance.Add>(_c1, ref outputParameter));
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
                            try
                            {
                                _p5 inputParameter = new _p5();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.addCustomSerialize(inputParameter.p0);
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                sender.AddLog(error);
                            }
                            return;
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p6 outputParameter = new _p6();
                                    
                                    Value.addCustomSerialize(sender.GetCallback<_p6, AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize>(_c3, ref outputParameter));
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
                        case 4:
                            try
                            {
                                {
                                    
                                    Value.addCustomSerializeFinally();
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                sender.AddLog(error);
                            }
                            return;
                        case 5:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, ref inputParameter);
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
                                {
                                    _p4 outputParameter = new _p4();
                                    
                                    Value.addRegister(sender.GetCallback<_p4, AutoCSer.TestCase.TcpServerPerformance.Add>(_c6, ref outputParameter));
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
                        case 7:
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    
                                    Value.addRegister(inputParameter.p0, inputParameter.p1);
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                sender.AddLog(error);
                            }
                            return;
                        case 8:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p4 _outputParameter_ = new _p4();
                                    
                                    AutoCSer.TestCase.TcpServerPerformance.Add Return;
                                    
                                    Return = Value.addSynchronous(inputParameter.p0, inputParameter.p1);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c8, ref _outputParameter_);
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
                        case 9:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s9/**/.Pop() ?? new _s9()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpTask, ref inputParameter);
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
                        case 10:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s10/**/.Pop() ?? new _s10()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ref inputParameter);
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
                        case 11:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s11/**/.Pop() ?? new _s11()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                        case 12:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s12/**/.Pop() ?? new _s12()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1, IsBuildOutputThread = true };
                sealed class _s5 : AutoCSer.Net.TcpInternalServer.ServerCall<_s5, AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addQueue(inputParameter.p0, inputParameter.p1);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s9 : AutoCSer.Net.TcpInternalServer.ServerCall<_s9, AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addTcpTask(inputParameter.p0, inputParameter.p1);

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
                            Sender.Push(CommandIndex, _c9, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s10 : AutoCSer.Net.TcpInternalServer.ServerCall<_s10, AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addThreadPool(inputParameter.p0, inputParameter.p1);

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
                            Sender.Push(CommandIndex, _c10, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4 };
                sealed class _s11 : AutoCSer.Net.TcpInternalServer.ServerCall<_s11, AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                    {
                        try
                        {
                            
                            AutoCSer.TestCase.TcpServerPerformance.Add Return;

                            
                            Return = serverValue.addTimeoutTask(inputParameter.p0, inputParameter.p1);

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
                            Sender.Push(CommandIndex, _c11, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s12 : AutoCSer.Net.TcpInternalServer.ServerCall<_s12, AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer>
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), typeof(_p3), null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { typeof(_p5), null }
                        , new System.Type[] { typeof(_p4), typeof(_p6), null }
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
                {
                    public AutoCSer.TestCase.TcpInternalServerPerformance.ClientCustomSerialize p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize)value; }
                    }
#endif
                }

            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalClient : AutoCSer.Net.TcpInternalServer.MethodClient<TcpInternalClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("TcpInternalServerPerformance", typeof(AutoCSer.TestCase.TcpInternalServerPerformance.InternalServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute);
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
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalServer._p2 _outputParameter_ = new TcpInternalServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p1, TcpInternalServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.PushNotNull(_wait_);
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
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
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
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalServer._p2> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalServer._p2>();
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        TcpInternalServer._p2 _outputParameter_ = new TcpInternalServer._p2
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<TcpInternalServer._p1, TcpInternalServer._p2>(_a0, _wait_, ref _inputParameter_, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p2> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 异步计算测试
                /// </summary>
                public 
                void addAsynchronous(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalServer._p3, TcpInternalServer._p4>(_ac1, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 计算回调测试
                /// </summary>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void addCustomSerialize(AutoCSer.TestCase.TcpInternalServerPerformance.ClientCustomSerialize value)
                {
                    TcpInternalServer._p5 _inputParameter_ = new TcpInternalServer._p5
                    {
                        
                        p0 = value,
                    };
                    _TcpClient_.Sender.CallOnly(_c2, ref _inputParameter_);
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsKeepCallback = 1 };
                /// <summary>
                /// 计算回调测试
                /// </summary>
                /// <returns>保持异步回调</returns>
                public 
                AutoCSer.Net.TcpServer.KeepCallback addCustomSerialize(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p6>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize, TcpInternalServer._p6>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<TcpInternalServer._p6>(_ac3, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p6> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p6> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 计算测试结束
                /// </summary>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void addCustomSerializeFinally()
                {
                    _TcpClient_.Sender.CallOnly(_c4);
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 计算队列测试
                /// </summary>
                public 
                void addQueue(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalServer._p3, TcpInternalServer._p4>(_ac5, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsKeepCallback = 1 };
                /// <summary>
                /// 计算回调测试
                /// </summary>
                /// <returns>保持异步回调</returns>
                public 
                AutoCSer.Net.TcpServer.KeepCallback addRegister(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<TcpInternalServer._p4>(_ac6, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 3, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 计算回调测试
                /// </summary>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void addRegister(int left, int right)
                {
                    TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    _TcpClient_.Sender.CallOnly(_c7, ref _inputParameter_);
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 简单计算测试
                /// </summary>
                public 
                void addSynchronous(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalServer._p3, TcpInternalServer._p4>(_ac8, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 9 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 计算任务测试
                /// </summary>
                public 
                void addTcpTask(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalServer._p3, TcpInternalServer._p4>(_ac9, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 10 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 计算任务测试
                /// </summary>
                public 
                void addThreadPool(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalServer._p3, TcpInternalServer._p4>(_ac10, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 11 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 计算任务测试
                /// </summary>
                public 
                void addTimeoutTask(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.TcpServerPerformance.Add, TcpInternalServer._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalServer._p3, TcpInternalServer._p4>(_ac11, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// GC 垃圾回收
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue gcCollect()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c12, ref _wait_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalServer._p1), typeof(TcpInternalServer._p3), null }
                        , new System.Type[] { typeof(TcpInternalServer._p2), null }
                        , new System.Type[] { typeof(TcpInternalServer._p5), null }
                        , new System.Type[] { typeof(TcpInternalServer._p4), typeof(TcpInternalServer._p6), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}
#endif