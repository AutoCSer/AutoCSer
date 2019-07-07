//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.TcpInternalServer
{
        internal partial class Json
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalServer.Json TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalServer.Json Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalServer.Json TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalServer.Json", typeof(AutoCSer.TestCase.TcpInternalServer.Json))), verify, onCustomData, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpInternalServer.Json();
                    setCommandData(9);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
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
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                        case 8:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s8/**/.Pop() ?? new _s8()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalServer.ServerCall<_s0, AutoCSer.TestCase.TcpInternalServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Add(inputParameter.a, inputParameter.b);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Inc();

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s2 : AutoCSer.Net.TcpInternalServer.ServerCall<_s2, AutoCSer.TestCase.TcpInternalServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Set(inputParameter.a);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s3 : AutoCSer.Net.TcpInternalServer.ServerCall<_s3, AutoCSer.TestCase.TcpInternalServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c3, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                sealed class _s4 : AutoCSer.Net.TcpInternalServer.ServerCall<_s4, AutoCSer.TestCase.TcpInternalServer.Json, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p5> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b, out value.Value.c);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p5> value = new AutoCSer.Net.TcpServer.ReturnValue<_p5>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c4, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsBuildOutputThread = true };
                sealed class _s5 : AutoCSer.Net.TcpInternalServer.ServerCall<_s5, AutoCSer.TestCase.TcpInternalServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc();

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c5, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                sealed class _s6 : AutoCSer.Net.TcpInternalServer.ServerCall<_s6, AutoCSer.TestCase.TcpInternalServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c6, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                sealed class _s7 : AutoCSer.Net.TcpInternalServer.ServerCall<_s7, AutoCSer.TestCase.TcpInternalServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(out value.Value.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c7, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
                sealed class _s8 : AutoCSer.Net.TcpInternalServer.ServerCall<_s8, AutoCSer.TestCase.TcpInternalServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a, out value.Value.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p7> value = new AutoCSer.Net.TcpServer.ReturnValue<_p7>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c8, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), typeof(_p4), null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), typeof(_p6), typeof(_p7), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int a;
                    public int b;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int a;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
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
                internal struct _p4
                {
                    public int a;
                    public int b;
                    public int c;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int c;
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
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int a;
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
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int b;
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
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalServer.Json", typeof(AutoCSer.TestCase.TcpInternalServer.Json));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
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
                /// 多参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.Awaiter AddAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a0, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c1, ref _wait_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.Awaiter IncAwaiter()
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a1, _awaiter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                            {
                                
                                a = a,
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
                /// 单参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.Awaiter SetAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a2, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpInternalServer._p3 _outputParameter_ = new TcpInternalServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p1, TcpInternalServer._p3>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p5> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p5>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p4 _inputParameter_ = new TcpInternalServer._p4
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpInternalServer._p5 _outputParameter_ = new TcpInternalServer._p5
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p4, TcpInternalServer._p5>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            c = _outputParameter_.c;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p5>.PushNotNull(_wait_);
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc()
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
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p3>(_c5, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a5, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                            {
                                
                                a = a,
                            };
                            TcpInternalServer._p3 _outputParameter_ = new TcpInternalServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p2, TcpInternalServer._p3>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p2, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                            {
                            };
                            TcpInternalServer._p6 _outputParameter_ = new TcpInternalServer._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p2, TcpInternalServer._p6>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            a = _outputParameter_.a;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6>.PushNotNull(_wait_);
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                a = a,
                            };
                            TcpInternalServer._p7 _outputParameter_ = new TcpInternalServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p1, TcpInternalServer._p7>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            b = _outputParameter_.b;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.PushNotNull(_wait_);
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalServer._p1), typeof(TcpInternalServer._p2), typeof(TcpInternalServer._p4), null }
                        , new System.Type[] { typeof(TcpInternalServer._p3), typeof(TcpInternalServer._p5), typeof(TcpInternalServer._p6), typeof(TcpInternalServer._p7), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalServer
{
        public partial class Member
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalServer.Member TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalServer.Member Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalServer.Member TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpInternalServer.Member value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalServer.Member", typeof(AutoCSer.TestCase.TcpInternalServer.Member))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpInternalServer.Member();
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
                public override void DoCommand(int index, AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalServer.ServerCall<_s0, AutoCSer.TestCase.TcpInternalServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.field;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c0, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.field = inputParameter.p0;


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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s2 : AutoCSer.Net.TcpInternalServer.ServerCall<_s2, AutoCSer.TestCase.TcpInternalServer.Member, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.p0, inputParameter.p1];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c2, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                sealed class _s3 : AutoCSer.Net.TcpInternalServer.ServerCall<_s3, AutoCSer.TestCase.TcpInternalServer.Member, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue[inputParameter.p0, inputParameter.p1] = inputParameter.p2;


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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s4 : AutoCSer.Net.TcpInternalServer.ServerCall<_s4, AutoCSer.TestCase.TcpInternalServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.p0];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c4, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                sealed class _s5 : AutoCSer.Net.TcpInternalServer.ServerCall<_s5, AutoCSer.TestCase.TcpInternalServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.getProperty;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c5, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                sealed class _s6 : AutoCSer.Net.TcpInternalServer.ServerCall<_s6, AutoCSer.TestCase.TcpInternalServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.property;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c6, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                sealed class _s7 : AutoCSer.Net.TcpInternalServer.ServerCall<_s7, AutoCSer.TestCase.TcpInternalServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.property = inputParameter.p0;


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
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p2), typeof(_p3), typeof(_p4), null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

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
                    public int p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int p0;
                    public int p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int p0;
                    public int p1;
                    public int p2;
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
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalServer.Member", typeof(AutoCSer.TestCase.TcpInternalServer.Member));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试字段
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalServer._p1>(_c0, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c1, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int left, int right]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.Pop();
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
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalServer._p3, TcpInternalServer._p1>(_c2, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalServer._p4 _inputParameter_ = new TcpInternalServer._p4
                                {
                                    
                                    p0 = left,
                                    
                                    p1 = right,
                                    
                                    p2 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c3, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                                {
                                    
                                    p0 = index,
                                };
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalServer._p2, TcpInternalServer._p1>(_c4, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性[不支持不可读属性]
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalServer._p1>(_c5, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试属性
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalServer._p1>(_c6, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c7, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalServer._p2), typeof(TcpInternalServer._p3), typeof(TcpInternalServer._p4), null }
                        , new System.Type[] { typeof(TcpInternalServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalServer
{
        internal partial class Session
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalServer.Session TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalServer.Session Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalServer.Session TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalServer.Session", typeof(AutoCSer.TestCase.TcpInternalServer.Session))), verify, onCustomData, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpInternalServer.Session();
                    setCommandData(2);
                    setVerifyCommand(0);
                    setCommand(1);
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
                                    
                                    bool Return;
                                    
                                    Return = Value.login(sender, inputParameter.p0, inputParameter.p1);
                                    if (Return) sender.SetVerifyMethod();
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                sealed class _s1 : AutoCSer.Net.TcpInternalServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalServer.Session>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            string Return;

                            
                            Return = serverValue.myName(Sender);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c1, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public string p0;
                    public string p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public string Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public string Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (string)value; }
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
                /// <param name="verifyMethod">TCP 验证方法</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalServer.Session", typeof(AutoCSer.TestCase.TcpInternalServer.Session));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute, verifyMethod);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                p0 = user,
                                
                                p1 = password,
                            };
                            TcpInternalServer._p2 _outputParameter_ = new TcpInternalServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p1, TcpInternalServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> loginAwaiter(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                        {
                            
                            p0 = user,
                            
                            p1 = password,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<string> myName()
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
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p3>(_c1, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<string> myNameAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<string> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<string>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>>(_a1, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalServer._p2), typeof(TcpInternalServer._p3), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalSimpleServer
{
        internal partial class Json
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalSimpleServer.Json TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalSimpleServer.Json Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalSimpleServer.Json TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalSimpleServer.Json", typeof(AutoCSer.TestCase.TcpInternalSimpleServer.Json))), verify, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpInternalSimpleServer.Json();
                    setCommandData(9);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
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
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.Add(inputParameter.a, inputParameter.b);
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    
                                    Value.Inc();
                                    return socket.Send();
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.Set(inputParameter.a);
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    int Return;
                                    
                                    Return = Value.add(inputParameter.a, inputParameter.b);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c3, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 4:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p4 inputParameter = new _p4();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p5 _outputParameter_ = new _p5();
                                    
                                    int Return;
                                    
                                    Return = Value.add(inputParameter.a, inputParameter.b, out _outputParameter_.c);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c4, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 5:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    int Return;
                                    
                                    Return = Value.inc();
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c5, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    int Return;
                                    
                                    Return = Value.inc(inputParameter.a);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c6, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 7:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p6 _outputParameter_ = new _p6();
                                    
                                    int Return;
                                    
                                    Return = Value.inc(out _outputParameter_.a);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c7, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 8:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 _outputParameter_ = new _p7();
                                    
                                    int Return;
                                    
                                    Return = Value.inc(inputParameter.a, out _outputParameter_.b);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c8, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c5 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c6 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c7 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 6 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c8 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 7 };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), typeof(_p4), null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), typeof(_p6), typeof(_p7), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int a;
                    public int b;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int a;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
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
                internal struct _p4
                {
                    public int a;
                    public int b;
                    public int c;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int c;
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
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int a;
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
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int b;
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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalSimpleServer.Json", typeof(AutoCSer.TestCase.TcpInternalSimpleServer.Json));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        return new AutoCSer.Net.TcpServer.ReturnValue { Type = _TcpClient_.Call(_c0, ref _inputParameter_) };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    if (_isDisposed_ == 0)
                    {
                        return new AutoCSer.Net.TcpServer.ReturnValue { Type = _TcpClient_.Call(_c1) };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                        {
                            
                            a = a,
                        };
                        return new AutoCSer.Net.TcpServer.ReturnValue { Type = _TcpClient_.Call(_c2, ref _inputParameter_) };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        TcpInternalSimpleServer._p3 _outputParameter_ = new TcpInternalSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1, TcpInternalSimpleServer._p3>(_c3, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p4 _inputParameter_ = new TcpInternalSimpleServer._p4
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        TcpInternalSimpleServer._p5 _outputParameter_ = new TcpInternalSimpleServer._p5
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p4, TcpInternalSimpleServer._p5>(_c4, ref _inputParameter_, ref _outputParameter_);
                        
                        c = _outputParameter_.c;
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c5 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p3 _outputParameter_ = new TcpInternalSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p3>(_c5, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c6 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                        {
                            
                            a = a,
                        };
                        TcpInternalSimpleServer._p3 _outputParameter_ = new TcpInternalSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p2, TcpInternalSimpleServer._p3>(_c6, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c7 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                        {
                        };
                        TcpInternalSimpleServer._p6 _outputParameter_ = new TcpInternalSimpleServer._p6
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p2, TcpInternalSimpleServer._p6>(_c7, ref _inputParameter_, ref _outputParameter_);
                        
                        a = _outputParameter_.a;
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c8 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 8 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            a = a,
                        };
                        TcpInternalSimpleServer._p7 _outputParameter_ = new TcpInternalSimpleServer._p7
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1, TcpInternalSimpleServer._p7>(_c8, ref _inputParameter_, ref _outputParameter_);
                        
                        b = _outputParameter_.b;
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p1), typeof(TcpInternalSimpleServer._p2), typeof(TcpInternalSimpleServer._p4), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p3), typeof(TcpInternalSimpleServer._p5), typeof(TcpInternalSimpleServer._p6), typeof(TcpInternalSimpleServer._p7), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalSimpleServer
{
        public partial class Member
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalSimpleServer.Member TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalSimpleServer.Member Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalSimpleServer.Member TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpInternalSimpleServer.Member value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalSimpleServer.Member", typeof(AutoCSer.TestCase.TcpInternalSimpleServer.Member))), verify, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpInternalSimpleServer.Member();
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
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpInternalSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.field;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    
                                    Value.field = inputParameter.p0;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value[inputParameter.p0, inputParameter.p1];
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c2, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p4 inputParameter = new _p4();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    
                                    Value[inputParameter.p0, inputParameter.p1] = inputParameter.p2;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 4:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value[inputParameter.p0];
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c4, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 5:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.getProperty;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c5, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.property;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c6, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 7:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    
                                    Value.property = inputParameter.p0;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c5 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c6 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c7 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p2), typeof(_p3), typeof(_p4), null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

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
                    public int p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int p0;
                    public int p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int p0;
                    public int p1;
                    public int p2;
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalSimpleServer.Member", typeof(AutoCSer.TestCase.TcpInternalSimpleServer.Member));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试字段
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1>(_c0, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                            {
                                
                                p0 = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c1, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 2, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 3, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int left, int right]
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p3 _inputParameter_ = new TcpInternalSimpleServer._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p3, TcpInternalSimpleServer._p1>(_c2, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p4 _inputParameter_ = new TcpInternalSimpleServer._p4
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                                
                                p2 = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c3, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 4, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 2, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                            {
                                
                                p0 = index,
                            };
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p2, TcpInternalSimpleServer._p1>(_c4, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c5 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 5 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性[不支持不可读属性]
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1>(_c5, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c6 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 6 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试属性
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1>(_c6, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                            {
                                
                                p0 = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c7, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c7 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 7 + 128, InputParameterIndex = 2, IsSimpleSerializeInputParamter = true };


                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p2), typeof(TcpInternalSimpleServer._p3), typeof(TcpInternalSimpleServer._p4), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalSimpleServer
{
        internal partial class Session
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalSimpleServer.Session TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalSimpleServer.Session Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalSimpleServer.Session TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalSimpleServer.Session", typeof(AutoCSer.TestCase.TcpInternalSimpleServer.Session))), verify, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpInternalSimpleServer.Session();
                    setCommandData(2);
                    setVerifyCommand(0);
                    setCommand(1);
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
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    bool Return;
                                    
                                    Return = Value.login(socket, inputParameter.p0, inputParameter.p1);
                                    if (Return) socket.SetVerifyMethod();
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    string Return;
                                    
                                    Return = Value.myName(socket);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c1, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public string p0;
                    public string p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public string Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public string Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (string)value; }
                    }
#endif
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verifyMethod">TCP 验证方法</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<TcpInternalSimpleClient, bool> verifyMethod = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalSimpleServer.Session", typeof(AutoCSer.TestCase.TcpInternalSimpleServer.Session));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log, verifyMethod);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<bool> login(string user, string password)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            p0 = user,
                            
                            p1 = password,
                        };
                        TcpInternalSimpleServer._p2 _outputParameter_ = new TcpInternalSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1, TcpInternalSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p3 _outputParameter_ = new TcpInternalSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p3>(_c1, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p2), typeof(TcpInternalSimpleServer._p3), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalStreamServer
{
        internal partial class Json
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalStreamServer.Json TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalStreamServer.Json Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalStreamServer.Json TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalStreamServer.Json", typeof(AutoCSer.TestCase.TcpInternalStreamServer.Json))), verify, log)
                {
                    Value =new AutoCSer.TestCase.TcpInternalStreamServer.Json();
                    setCommandData(9);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
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
                                if (sender.DeSerialize(ref data, ref inputParameter))
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value);
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
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, ref inputParameter);
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
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value, ref inputParameter);
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, ref inputParameter);
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
                        case 8:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s8/**/.Pop() ?? new _s8()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.TestCase.TcpInternalStreamServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Add(inputParameter.a, inputParameter.b);

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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s1 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalStreamServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Inc();

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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s2 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s2, AutoCSer.TestCase.TcpInternalStreamServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Set(inputParameter.a);

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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s3 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s3, AutoCSer.TestCase.TcpInternalStreamServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c3, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsKeepCallback = 1 };
                sealed class _s4 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s4, AutoCSer.TestCase.TcpInternalStreamServer.Json, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p5> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b, out value.Value.c);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p5> value = new AutoCSer.Net.TcpServer.ReturnValue<_p5>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c4, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsKeepCallback = 1 };
                sealed class _s5 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s5, AutoCSer.TestCase.TcpInternalStreamServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc();

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c5, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsKeepCallback = 1 };
                sealed class _s6 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s6, AutoCSer.TestCase.TcpInternalStreamServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c6, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsKeepCallback = 1 };
                sealed class _s7 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s7, AutoCSer.TestCase.TcpInternalStreamServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(out value.Value.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c7, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsKeepCallback = 1 };
                sealed class _s8 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s8, AutoCSer.TestCase.TcpInternalStreamServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a, out value.Value.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p7> value = new AutoCSer.Net.TcpServer.ReturnValue<_p7>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c8, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsKeepCallback = 1 };
                static TcpInternalStreamServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), typeof(_p4), null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), typeof(_p6), typeof(_p7), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int a;
                    public int b;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int a;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
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
                internal struct _p4
                {
                    public int a;
                    public int b;
                    public int c;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int c;
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
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int a;
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
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int b;
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
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalStreamServer.Json", typeof(AutoCSer.TestCase.TcpInternalStreamServer.Json));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
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
                /// 多参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.Awaiter AddAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a0, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c1, ref _wait_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.Awaiter IncAwaiter()
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a1, _awaiter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                            {
                                
                                a = a,
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
                /// 单参数无返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.Awaiter SetAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a2, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpInternalStreamServer._p3 _outputParameter_ = new TcpInternalStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p3>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p5> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p5>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p4 _inputParameter_ = new TcpInternalStreamServer._p4
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpInternalStreamServer._p5 _outputParameter_ = new TcpInternalStreamServer._p5
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p4, TcpInternalStreamServer._p5>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            c = _outputParameter_.c;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p5>.PushNotNull(_wait_);
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p3 _outputParameter_ = new TcpInternalStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p3>(_c5, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a5, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                            {
                                
                                a = a,
                            };
                            TcpInternalStreamServer._p3 _outputParameter_ = new TcpInternalStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p2, TcpInternalStreamServer._p3>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p2, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            TcpInternalStreamServer._p6 _outputParameter_ = new TcpInternalStreamServer._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p2, TcpInternalStreamServer._p6>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            a = _outputParameter_.a;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p6>.PushNotNull(_wait_);
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                a = a,
                            };
                            TcpInternalStreamServer._p7 _outputParameter_ = new TcpInternalStreamServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p7>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            b = _outputParameter_.b;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p7>.PushNotNull(_wait_);
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p1), typeof(TcpInternalStreamServer._p2), typeof(TcpInternalStreamServer._p4), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p3), typeof(TcpInternalStreamServer._p5), typeof(TcpInternalStreamServer._p6), typeof(TcpInternalStreamServer._p7), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalStreamServer
{
        public partial class Member
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalStreamServer.Member TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalStreamServer.Member Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalStreamServer.Member TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpInternalStreamServer.Member value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalStreamServer.Member", typeof(AutoCSer.TestCase.TcpInternalStreamServer.Member))), verify, log)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpInternalStreamServer.Member();
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
                public override void DoCommand(int index, AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value);
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
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
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
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, ref inputParameter);
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value);
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
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.TestCase.TcpInternalStreamServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.field;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalStreamServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.field = inputParameter.p0;


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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
                sealed class _s2 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s2, AutoCSer.TestCase.TcpInternalStreamServer.Member, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.p0, inputParameter.p1];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c2, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s3 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s3, AutoCSer.TestCase.TcpInternalStreamServer.Member, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue[inputParameter.p0, inputParameter.p1] = inputParameter.p2;


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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
                sealed class _s4 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s4, AutoCSer.TestCase.TcpInternalStreamServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.p0];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c4, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s5 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s5, AutoCSer.TestCase.TcpInternalStreamServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.getProperty;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c5, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s6 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s6, AutoCSer.TestCase.TcpInternalStreamServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.property;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c6, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s7 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s7, AutoCSer.TestCase.TcpInternalStreamServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.property = inputParameter.p0;


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
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
                static TcpInternalStreamServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p2), typeof(_p3), typeof(_p4), null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

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
                    public int p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int p0;
                    public int p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int p0;
                    public int p1;
                    public int p2;
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
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalStreamServer.Member", typeof(AutoCSer.TestCase.TcpInternalStreamServer.Member));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试字段
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p1>(_c0, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c1, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int left, int right]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
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
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p3, TcpInternalStreamServer._p1>(_c2, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p4 _inputParameter_ = new TcpInternalStreamServer._p4
                                {
                                    
                                    p0 = left,
                                    
                                    p1 = right,
                                    
                                    p2 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c3, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                                {
                                    
                                    p0 = index,
                                };
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p2, TcpInternalStreamServer._p1>(_c4, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性[不支持不可读属性]
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p1>(_c5, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试属性
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p1>(_c6, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c7, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p2), typeof(TcpInternalStreamServer._p3), typeof(TcpInternalStreamServer._p4), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpInternalStreamServer
{
        internal partial class Session
        {
            /// <summary>
            /// AutoCSer.TestCase.TcpInternalStreamServer.Session TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpInternalStreamServer.Session Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpInternalStreamServer.Session TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalStreamServer.Session", typeof(AutoCSer.TestCase.TcpInternalStreamServer.Session))), verify, log)
                {
                    Value =new AutoCSer.TestCase.TcpInternalStreamServer.Session();
                    setCommandData(2);
                    setVerifyCommand(0);
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
                                    
                                    bool Return;
                                    
                                    Return = Value.login(sender, inputParameter.p0, inputParameter.p1);
                                    if (Return) sender.SetVerifyMethod();
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value);
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpInternalStreamServer.Session>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            string Return;

                            
                            Return = serverValue.myName(Sender);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true };
                static TcpInternalStreamServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public string p0;
                    public string p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public string Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public string Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (string)value; }
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
                /// <param name="verifyMethod">TCP 验证方法</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<TcpInternalStreamClient, AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpInternalStreamServer.Session", typeof(AutoCSer.TestCase.TcpInternalStreamServer.Session));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute, verifyMethod);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = user,
                                
                                p1 = password,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> loginAwaiter(AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _sender_;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = user,
                            
                            p1 = password,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p3 _outputParameter_ = new TcpInternalStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p3>(_c1, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<string> myNameAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<string> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<string>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>>(_a1, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), typeof(TcpInternalStreamServer._p3), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenServer
{
        internal partial class Json
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[9];
                names[0].Set(@"(int,int)Add", 0);
                names[1].Set(@"()Inc", 1);
                names[2].Set(@"(int)Set", 2);
                names[3].Set(@"(int,int)add", 3);
                names[4].Set(@"(int,int,out int)add", 4);
                names[5].Set(@"()inc", 5);
                names[6].Set(@"(int)inc", 6);
                names[7].Set(@"(out int)inc", 7);
                names[8].Set(@"(int,out int)inc", 8);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenServer.Json TCP服务
            /// </summary>
            public sealed class TcpOpenServer : AutoCSer.Net.TcpOpenServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenServer.Json Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenServer.Json TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpOpenServer(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServer.Json", typeof(AutoCSer.TestCase.TcpOpenServer.Json))), verify, onCustomData, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpOpenServer.Json();
                    setCommandData(9);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
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
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                        case 8:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s8/**/.Pop() ?? new _s8()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpOpenServer.ServerCall<_s0, AutoCSer.TestCase.TcpOpenServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Add(inputParameter.a, inputParameter.b);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s1 : AutoCSer.Net.TcpOpenServer.ServerCall<_s1, AutoCSer.TestCase.TcpOpenServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Inc();

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s2 : AutoCSer.Net.TcpOpenServer.ServerCall<_s2, AutoCSer.TestCase.TcpOpenServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Set(inputParameter.a);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s3 : AutoCSer.Net.TcpOpenServer.ServerCall<_s3, AutoCSer.TestCase.TcpOpenServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c3, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                sealed class _s4 : AutoCSer.Net.TcpOpenServer.ServerCall<_s4, AutoCSer.TestCase.TcpOpenServer.Json, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p5> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b, out value.Value.c);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p5> value = new AutoCSer.Net.TcpServer.ReturnValue<_p5>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c4, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsBuildOutputThread = true };
                sealed class _s5 : AutoCSer.Net.TcpOpenServer.ServerCall<_s5, AutoCSer.TestCase.TcpOpenServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc();

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c5, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                sealed class _s6 : AutoCSer.Net.TcpOpenServer.ServerCall<_s6, AutoCSer.TestCase.TcpOpenServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c6, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                sealed class _s7 : AutoCSer.Net.TcpOpenServer.ServerCall<_s7, AutoCSer.TestCase.TcpOpenServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(out value.Value.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c7, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
                sealed class _s8 : AutoCSer.Net.TcpOpenServer.ServerCall<_s8, AutoCSer.TestCase.TcpOpenServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a, out value.Value.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p7> value = new AutoCSer.Net.TcpServer.ReturnValue<_p7>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c8, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                static TcpOpenServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), typeof(_p4), null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), typeof(_p6), typeof(_p7), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int a;
                    public int b;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int a;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
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
                internal struct _p4
                {
                    public int a;
                    public int b;
                    public int c;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int c;
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
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int a;
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
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int b;
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
                        attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServer.Json", typeof(AutoCSer.TestCase.TcpOpenServer.Json));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log, clientRoute);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
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
                public AutoCSer.Net.TcpServer.Awaiter AddAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a0, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c1, ref _wait_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.Awaiter IncAwaiter()
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a1, _awaiter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p2 _inputParameter_ = new TcpOpenServer._p2
                            {
                                
                                a = a,
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
                public AutoCSer.Net.TcpServer.Awaiter SetAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenServer._p2 _inputParameter_ = new TcpOpenServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a2, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpOpenServer._p3 _outputParameter_ = new TcpOpenServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p1, TcpOpenServer._p3>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpOpenServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                public AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p5> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p5>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p4 _inputParameter_ = new TcpOpenServer._p4
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpOpenServer._p5 _outputParameter_ = new TcpOpenServer._p5
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p4, TcpOpenServer._p5>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            c = _outputParameter_.c;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p5>.PushNotNull(_wait_);
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _outputParameter_ = new TcpOpenServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p3>(_c5, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a5, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p2 _inputParameter_ = new TcpOpenServer._p2
                            {
                                
                                a = a,
                            };
                            TcpOpenServer._p3 _outputParameter_ = new TcpOpenServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p2, TcpOpenServer._p3>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenServer._p2 _inputParameter_ = new TcpOpenServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpOpenServer._p2, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p2 _inputParameter_ = new TcpOpenServer._p2
                            {
                            };
                            TcpOpenServer._p6 _outputParameter_ = new TcpOpenServer._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p2, TcpOpenServer._p6>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            a = _outputParameter_.a;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p6>.PushNotNull(_wait_);
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                            {
                                
                                a = a,
                            };
                            TcpOpenServer._p7 _outputParameter_ = new TcpOpenServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p1, TcpOpenServer._p7>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            b = _outputParameter_.b;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p7>.PushNotNull(_wait_);
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpOpenClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenServer._p1), typeof(TcpOpenServer._p2), typeof(TcpOpenServer._p4), null }
                        , new System.Type[] { typeof(TcpOpenServer._p3), typeof(TcpOpenServer._p5), typeof(TcpOpenServer._p6), typeof(TcpOpenServer._p7), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenServer
{
        public partial class Member
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[8];
                names[0].Set(@"()get_field", 0);
                names[1].Set(@"(int)set_field", 1);
                names[2].Set(@"(int,int)get_Item", 2);
                names[3].Set(@"(int,int,int)set_Item", 3);
                names[4].Set(@"(int)get_Item", 4);
                names[5].Set(@"()get_getProperty", 5);
                names[6].Set(@"()get_property", 6);
                names[7].Set(@"(int)set_property", 7);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenServer.Member TCP服务
            /// </summary>
            public sealed class TcpOpenServer : AutoCSer.Net.TcpOpenServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenServer.Member Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenServer.Member TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="value">TCP服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpOpenServer(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpOpenServer.Member value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServer.Member", typeof(AutoCSer.TestCase.TcpOpenServer.Member))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpOpenServer.Member();
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
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                _p5 inputParameter = new _p5();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpOpenServer.ServerCall<_s0, AutoCSer.TestCase.TcpOpenServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.field;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c0, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsBuildOutputThread = true };
                sealed class _s1 : AutoCSer.Net.TcpOpenServer.ServerCall<_s1, AutoCSer.TestCase.TcpOpenServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.field = inputParameter.value;


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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s2 : AutoCSer.Net.TcpOpenServer.ServerCall<_s2, AutoCSer.TestCase.TcpOpenServer.Member, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.left, inputParameter.right];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c2, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsBuildOutputThread = true };
                sealed class _s3 : AutoCSer.Net.TcpOpenServer.ServerCall<_s3, AutoCSer.TestCase.TcpOpenServer.Member, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue[inputParameter.left, inputParameter.right] = inputParameter.value;


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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s4 : AutoCSer.Net.TcpOpenServer.ServerCall<_s4, AutoCSer.TestCase.TcpOpenServer.Member, _p5>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.index];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c4, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsBuildOutputThread = true };
                sealed class _s5 : AutoCSer.Net.TcpOpenServer.ServerCall<_s5, AutoCSer.TestCase.TcpOpenServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.getProperty;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c5, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsBuildOutputThread = true };
                sealed class _s6 : AutoCSer.Net.TcpOpenServer.ServerCall<_s6, AutoCSer.TestCase.TcpOpenServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.property;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c6, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsBuildOutputThread = true };
                sealed class _s7 : AutoCSer.Net.TcpOpenServer.ServerCall<_s7, AutoCSer.TestCase.TcpOpenServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.property = inputParameter.value;


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
                        , new System.Type[] { typeof(_p2), typeof(_p3), typeof(_p4), typeof(_p5), null }
                        , new System.Type[] { typeof(_p1), null });
                }

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
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int left;
                    public int right;
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
                {
                    public int index;
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
                        attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServer.Member", typeof(AutoCSer.TestCase.TcpOpenServer.Member));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log, clientRoute);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };


                public AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenServer._p1>(_c0, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenServer._p2 _inputParameter_ = new TcpOpenServer._p2
                                {
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c1, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> this[int left, int right]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.Pop();
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
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenServer._p3, TcpOpenServer._p1>(_c2, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenServer._p4 _inputParameter_ = new TcpOpenServer._p4
                                {
                                    
                                    left = left,
                                    
                                    right = right,
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c3, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenServer._p5 _inputParameter_ = new TcpOpenServer._p5
                                {
                                    
                                    index = index,
                                };
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenServer._p5, TcpOpenServer._p1>(_c4, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenServer._p1>(_c5, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenServer._p1>(_c6, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenServer._p2 _inputParameter_ = new TcpOpenServer._p2
                                {
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c7, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                static TcpOpenClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenServer._p2), typeof(TcpOpenServer._p3), typeof(TcpOpenServer._p4), typeof(TcpOpenServer._p5), null }
                        , new System.Type[] { typeof(TcpOpenServer._p1), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenServer
{
        internal partial class Session
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[2];
                names[0].Set(@"(AutoCSer.Net.TcpOpenServer.ServerSocketSender,string,string)login", 0);
                names[1].Set(@"(AutoCSer.Net.TcpOpenServer.ServerSocketSender)myName", 1);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenServer.Session TCP服务
            /// </summary>
            public sealed class TcpOpenServer : AutoCSer.Net.TcpOpenServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenServer.Session Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenServer.Session TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpOpenServer(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServer.Session", typeof(AutoCSer.TestCase.TcpOpenServer.Session))), verify, onCustomData, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpOpenServer.Session();
                    setCommandData(2);
                    setVerifyCommand(0);
                    setCommand(1);
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
                                    
                                    bool Return;
                                    
                                    Return = Value.login(sender, inputParameter.user, inputParameter.password);
                                    if (Return) sender.SetVerifyMethod();
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                sealed class _s1 : AutoCSer.Net.TcpOpenServer.ServerCall<_s1, AutoCSer.TestCase.TcpOpenServer.Session>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            string Return;

                            
                            Return = serverValue.myName(Sender);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c1, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
                static TcpOpenServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public string password;
                    public string user;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public string Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public string Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (string)value; }
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
                /// <param name="verifyMethod">TCP验证方法</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpOpenClient(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<TcpOpenClient, AutoCSer.Net.TcpOpenServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenServer.Session", typeof(AutoCSer.TestCase.TcpOpenServer.Session));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log, clientRoute, verifyMethod);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsVerifyMethod = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpOpenServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                            {
                                
                                user = user,
                                
                                password = password,
                            };
                            TcpOpenServer._p2 _outputParameter_ = new TcpOpenServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p1, TcpOpenServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> loginAwaiter(AutoCSer.Net.TcpOpenServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _sender_;
                    if (_socket_ != null)
                    {
                        TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                        {
                            
                            user = user,
                            
                            password = password,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<TcpOpenServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenServer._p3 _outputParameter_ = new TcpOpenServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenServer._p3>(_c1, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<string> myNameAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<string> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<string>();
                    AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>>(_a1, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpOpenClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenServer._p1), null }
                        , new System.Type[] { typeof(TcpOpenServer._p2), typeof(TcpOpenServer._p3), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenSimpleServer
{
        internal partial class Json
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[9];
                names[0].Set(@"(int,int)Add", 0);
                names[1].Set(@"()Inc", 1);
                names[2].Set(@"(int)Set", 2);
                names[3].Set(@"(int,int)add", 3);
                names[4].Set(@"(int,int,out int)add", 4);
                names[5].Set(@"()inc", 5);
                names[6].Set(@"(int)inc", 6);
                names[7].Set(@"(out int)inc", 7);
                names[8].Set(@"(int,out int)inc", 8);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenSimpleServer.Json TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenSimpleServer.Json Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenSimpleServer.Json TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServer.Json", typeof(AutoCSer.TestCase.TcpOpenSimpleServer.Json))), verify, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpOpenSimpleServer.Json();
                    setCommandData(9);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.Add(inputParameter.a, inputParameter.b);
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    
                                    Value.Inc();
                                    return socket.Send();
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.Set(inputParameter.a);
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    int Return;
                                    
                                    Return = Value.add(inputParameter.a, inputParameter.b);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c3, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 4:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p4 inputParameter = new _p4();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p5 _outputParameter_ = new _p5();
                                    
                                    int Return;
                                    
                                    Return = Value.add(inputParameter.a, inputParameter.b, out _outputParameter_.c);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c4, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 5:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    int Return;
                                    
                                    Return = Value.inc();
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c5, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    int Return;
                                    
                                    Return = Value.inc(inputParameter.a);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c6, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 7:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p6 _outputParameter_ = new _p6();
                                    
                                    int Return;
                                    
                                    Return = Value.inc(out _outputParameter_.a);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c7, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 8:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 _outputParameter_ = new _p7();
                                    
                                    int Return;
                                    
                                    Return = Value.inc(inputParameter.a, out _outputParameter_.b);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c8, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c5 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c6 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c7 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 6 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c8 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 7 };
                static TcpOpenSimpleServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), typeof(_p4), null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), typeof(_p6), typeof(_p7), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int a;
                    public int b;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int a;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
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
                internal struct _p4
                {
                    public int a;
                    public int b;
                    public int c;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int c;
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
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int a;
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
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int b;
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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpOpenSimpleClient : AutoCSer.Net.TcpOpenSimpleServer.MethodClient<TcpOpenSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleClient(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServer.Json", typeof(AutoCSer.TestCase.TcpOpenSimpleServer.Json));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenSimpleServer.Client<TcpOpenSimpleClient>(this, attribute, log);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        return new AutoCSer.Net.TcpServer.ReturnValue { Type = _TcpClient_.Call(_c0, ref _inputParameter_) };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    if (_isDisposed_ == 0)
                    {
                        return new AutoCSer.Net.TcpServer.ReturnValue { Type = _TcpClient_.Call(_c1) };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p2 _inputParameter_ = new TcpOpenSimpleServer._p2
                        {
                            
                            a = a,
                        };
                        return new AutoCSer.Net.TcpServer.ReturnValue { Type = _TcpClient_.Call(_c2, ref _inputParameter_) };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        TcpOpenSimpleServer._p3 _outputParameter_ = new TcpOpenSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p3>(_c3, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p4 _inputParameter_ = new TcpOpenSimpleServer._p4
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        TcpOpenSimpleServer._p5 _outputParameter_ = new TcpOpenSimpleServer._p5
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p4, TcpOpenSimpleServer._p5>(_c4, ref _inputParameter_, ref _outputParameter_);
                        
                        c = _outputParameter_.c;
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c5 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p3 _outputParameter_ = new TcpOpenSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p3>(_c5, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c6 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p2 _inputParameter_ = new TcpOpenSimpleServer._p2
                        {
                            
                            a = a,
                        };
                        TcpOpenSimpleServer._p3 _outputParameter_ = new TcpOpenSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p2, TcpOpenSimpleServer._p3>(_c6, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c7 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p2 _inputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        TcpOpenSimpleServer._p6 _outputParameter_ = new TcpOpenSimpleServer._p6
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p2, TcpOpenSimpleServer._p6>(_c7, ref _inputParameter_, ref _outputParameter_);
                        
                        a = _outputParameter_.a;
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c8 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 8 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            a = a,
                        };
                        TcpOpenSimpleServer._p7 _outputParameter_ = new TcpOpenSimpleServer._p7
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p7>(_c8, ref _inputParameter_, ref _outputParameter_);
                        
                        b = _outputParameter_.b;
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpOpenSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p1), typeof(TcpOpenSimpleServer._p2), typeof(TcpOpenSimpleServer._p4), null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p3), typeof(TcpOpenSimpleServer._p5), typeof(TcpOpenSimpleServer._p6), typeof(TcpOpenSimpleServer._p7), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenSimpleServer
{
        public partial class Member
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[8];
                names[0].Set(@"()get_field", 0);
                names[1].Set(@"(int)set_field", 1);
                names[2].Set(@"(int,int)get_Item", 2);
                names[3].Set(@"(int,int,int)set_Item", 3);
                names[4].Set(@"(int)get_Item", 4);
                names[5].Set(@"()get_getProperty", 5);
                names[6].Set(@"()get_property", 6);
                names[7].Set(@"(int)set_property", 7);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenSimpleServer.Member TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenSimpleServer.Member Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenSimpleServer.Member TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="value">TCP服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpOpenSimpleServer.Member value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServer.Member", typeof(AutoCSer.TestCase.TcpOpenSimpleServer.Member))), verify, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpOpenSimpleServer.Member();
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
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.field;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.field = inputParameter.value;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value[inputParameter.left, inputParameter.right];
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c2, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p4 inputParameter = new _p4();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value[inputParameter.left, inputParameter.right] = inputParameter.value;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 4:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p5 inputParameter = new _p5();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value[inputParameter.index];
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c4, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 5:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.getProperty;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c5, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.property;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c6, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 7:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.property = inputParameter.value;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c5 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c6 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c7 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                static TcpOpenSimpleServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), typeof(_p4), typeof(_p5), null }
                        , new System.Type[] { typeof(_p1), null });
                }

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
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int left;
                    public int right;
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
                {
                    public int index;
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpOpenSimpleClient : AutoCSer.Net.TcpOpenSimpleServer.MethodClient<TcpOpenSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleClient(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServer.Member", typeof(AutoCSer.TestCase.TcpOpenSimpleServer.Member));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenSimpleServer.Client<TcpOpenSimpleClient>(this, attribute, log);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                public AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p1 _outputParameter_ = default(TcpOpenSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1>(_c0, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p2 _inputParameter_ = new TcpOpenSimpleServer._p2
                            {
                                
                                value = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c1, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                public AutoCSer.Net.TcpServer.ReturnValue<int> this[int left, int right]
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p3 _inputParameter_ = new TcpOpenSimpleServer._p3
                            {
                                
                                left = left,
                                
                                right = right,
                            };
                            TcpOpenSimpleServer._p1 _outputParameter_ = default(TcpOpenSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p3, TcpOpenSimpleServer._p1>(_c2, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p4 _inputParameter_ = new TcpOpenSimpleServer._p4
                            {
                                
                                left = left,
                                
                                right = right,
                                
                                value = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c3, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                public AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p5 _inputParameter_ = new TcpOpenSimpleServer._p5
                            {
                                
                                index = index,
                            };
                            TcpOpenSimpleServer._p1 _outputParameter_ = default(TcpOpenSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p5, TcpOpenSimpleServer._p1>(_c4, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c5 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                public AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p1 _outputParameter_ = default(TcpOpenSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1>(_c5, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c6 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 6 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                public AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p1 _outputParameter_ = default(TcpOpenSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1>(_c6, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p2 _inputParameter_ = new TcpOpenSimpleServer._p2
                            {
                                
                                value = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c7, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c7 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                static TcpOpenSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p2), typeof(TcpOpenSimpleServer._p3), typeof(TcpOpenSimpleServer._p4), typeof(TcpOpenSimpleServer._p5), null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p1), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenSimpleServer
{
        internal partial class Session
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[2];
                names[0].Set(@"(AutoCSer.Net.TcpOpenSimpleServer.ServerSocket,string,string)login", 0);
                names[1].Set(@"(AutoCSer.Net.TcpOpenSimpleServer.ServerSocket)myName", 1);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenSimpleServer.Session TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenSimpleServer.Session Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenSimpleServer.Session TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServer.Session", typeof(AutoCSer.TestCase.TcpOpenSimpleServer.Session))), verify, log, false)
                {
                    Value =new AutoCSer.TestCase.TcpOpenSimpleServer.Session();
                    setCommandData(2);
                    setVerifyCommand(0);
                    setCommand(1);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    bool Return;
                                    
                                    Return = Value.login(socket, inputParameter.user, inputParameter.password);
                                    if (Return) socket.SetVerifyMethod();
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    string Return;
                                    
                                    Return = Value.myName(socket);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c1, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3 };
                static TcpOpenSimpleServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public string password;
                    public string user;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public string Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public string Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (string)value; }
                    }
#endif
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpOpenSimpleClient : AutoCSer.Net.TcpOpenSimpleServer.MethodClient<TcpOpenSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="verifyMethod">TCP验证方法</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleClient(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<TcpOpenSimpleClient, bool> verifyMethod = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServer.Session", typeof(AutoCSer.TestCase.TcpOpenSimpleServer.Session));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenSimpleServer.Client<TcpOpenSimpleClient>(this, attribute, log, verifyMethod);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsVerifyMethod = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<bool> login(string user, string password)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            user = user,
                            
                            password = password,
                        };
                        TcpOpenSimpleServer._p2 _outputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p3 _outputParameter_ = new TcpOpenSimpleServer._p3
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p3>(_c1, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpOpenSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p1), null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p2), typeof(TcpOpenSimpleServer._p3), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenStreamServer
{
        internal partial class Json
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[9];
                names[0].Set(@"(int,int)Add", 0);
                names[1].Set(@"()Inc", 1);
                names[2].Set(@"(int)Set", 2);
                names[3].Set(@"(int,int)add", 3);
                names[4].Set(@"(int,int,out int)add", 4);
                names[5].Set(@"()inc", 5);
                names[6].Set(@"(int)inc", 6);
                names[7].Set(@"(out int)inc", 7);
                names[8].Set(@"(int,out int)inc", 8);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenStreamServer.Json TCP服务
            /// </summary>
            public sealed class TcpOpenStreamServer : AutoCSer.Net.TcpOpenStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenStreamServer.Json Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenStreamServer.Json TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenStreamServer(AutoCSer.Net.TcpOpenStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenStreamServer.Json", typeof(AutoCSer.TestCase.TcpOpenStreamServer.Json))), verify, log)
                {
                    Value =new AutoCSer.TestCase.TcpOpenStreamServer.Json();
                    setCommandData(9);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
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
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value);
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
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, ref inputParameter);
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
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value, ref inputParameter);
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, ref inputParameter);
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
                        case 8:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s8/**/.Pop() ?? new _s8()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s0, AutoCSer.TestCase.TcpOpenStreamServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Add(inputParameter.a, inputParameter.b);

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
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s1 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpOpenStreamServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Inc();

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
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s2 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s2, AutoCSer.TestCase.TcpOpenStreamServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.Set(inputParameter.a);

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
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s3 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s3, AutoCSer.TestCase.TcpOpenStreamServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c3, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsKeepCallback = 1 };
                sealed class _s4 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s4, AutoCSer.TestCase.TcpOpenStreamServer.Json, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p5> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.add(inputParameter.a, inputParameter.b, out value.Value.c);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p5> value = new AutoCSer.Net.TcpServer.ReturnValue<_p5>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c4, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsKeepCallback = 1 };
                sealed class _s5 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s5, AutoCSer.TestCase.TcpOpenStreamServer.Json>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc();

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c5, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsKeepCallback = 1 };
                sealed class _s6 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s6, AutoCSer.TestCase.TcpOpenStreamServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c6, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsKeepCallback = 1 };
                sealed class _s7 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s7, AutoCSer.TestCase.TcpOpenStreamServer.Json, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(out value.Value.a);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c7, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsKeepCallback = 1 };
                sealed class _s8 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s8, AutoCSer.TestCase.TcpOpenStreamServer.Json, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.inc(inputParameter.a, out value.Value.b);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p7> value = new AutoCSer.Net.TcpServer.ReturnValue<_p7>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c8, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsKeepCallback = 1 };
                static TcpOpenStreamServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), typeof(_p4), null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), typeof(_p6), typeof(_p7), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int a;
                    public int b;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int a;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
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
                internal struct _p4
                {
                    public int a;
                    public int b;
                    public int c;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int c;
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
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int a;
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
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int b;
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
                        attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenStreamServer.Json", typeof(AutoCSer.TestCase.TcpOpenStreamServer.Json));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenStreamServer.Client<TcpOpenStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p1 _inputParameter_ = new TcpOpenStreamServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
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
                public AutoCSer.Net.TcpServer.Awaiter AddAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenStreamServer._p1 _inputParameter_ = new TcpOpenStreamServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a0, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c1, ref _wait_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.Awaiter IncAwaiter()
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a1, _awaiter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                            {
                                
                                a = a,
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
                public AutoCSer.Net.TcpServer.Awaiter SetAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a2, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p1 _inputParameter_ = new TcpOpenStreamServer._p1
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpOpenStreamServer._p3 _outputParameter_ = new TcpOpenStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p1, TcpOpenStreamServer._p3>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenStreamServer._p1 _inputParameter_ = new TcpOpenStreamServer._p1
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpOpenStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                public AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p5> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p5>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p4 _inputParameter_ = new TcpOpenStreamServer._p4
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            TcpOpenStreamServer._p5 _outputParameter_ = new TcpOpenStreamServer._p5
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p4, TcpOpenStreamServer._p5>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            c = _outputParameter_.c;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p5>.PushNotNull(_wait_);
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p3 _outputParameter_ = new TcpOpenStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p3>(_c5, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a5, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                            {
                                
                                a = a,
                            };
                            TcpOpenStreamServer._p3 _outputParameter_ = new TcpOpenStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p2, TcpOpenStreamServer._p3>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpOpenStreamServer._p2, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                            {
                            };
                            TcpOpenStreamServer._p6 _outputParameter_ = new TcpOpenStreamServer._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p2, TcpOpenStreamServer._p6>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            a = _outputParameter_.a;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p6>.PushNotNull(_wait_);
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                public AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p1 _inputParameter_ = new TcpOpenStreamServer._p1
                            {
                                
                                a = a,
                            };
                            TcpOpenStreamServer._p7 _outputParameter_ = new TcpOpenStreamServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p1, TcpOpenStreamServer._p7>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            b = _outputParameter_.b;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p7>.PushNotNull(_wait_);
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpOpenStreamClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p1), typeof(TcpOpenStreamServer._p2), typeof(TcpOpenStreamServer._p4), null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p3), typeof(TcpOpenStreamServer._p5), typeof(TcpOpenStreamServer._p6), typeof(TcpOpenStreamServer._p7), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenStreamServer
{
        public partial class Member
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[8];
                names[0].Set(@"()get_field", 0);
                names[1].Set(@"(int)set_field", 1);
                names[2].Set(@"(int,int)get_Item", 2);
                names[3].Set(@"(int,int,int)set_Item", 3);
                names[4].Set(@"(int)get_Item", 4);
                names[5].Set(@"()get_getProperty", 5);
                names[6].Set(@"()get_property", 6);
                names[7].Set(@"(int)set_property", 7);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenStreamServer.Member TCP服务
            /// </summary>
            public sealed class TcpOpenStreamServer : AutoCSer.Net.TcpOpenStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenStreamServer.Member Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenStreamServer.Member TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="value">TCP服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpOpenStreamServer(AutoCSer.Net.TcpOpenStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpOpenStreamServer.Member value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenStreamServer.Member", typeof(AutoCSer.TestCase.TcpOpenStreamServer.Member))), verify, log)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpOpenStreamServer.Member();
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
                public override void DoCommand(int index, AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value);
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
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
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
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, ref inputParameter);
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
                                _p4 inputParameter = new _p4();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, ref inputParameter);
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
                                _p5 inputParameter = new _p5();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, ref inputParameter);
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
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value);
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
                        case 6:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value);
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
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s7/**/.Pop() ?? new _s7()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s0, AutoCSer.TestCase.TcpOpenStreamServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.field;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsKeepCallback = 1 };
                sealed class _s1 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpOpenStreamServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.field = inputParameter.value;


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
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s2 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s2, AutoCSer.TestCase.TcpOpenStreamServer.Member, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.left, inputParameter.right];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c2, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsKeepCallback = 1 };
                sealed class _s3 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s3, AutoCSer.TestCase.TcpOpenStreamServer.Member, _p4>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue[inputParameter.left, inputParameter.right] = inputParameter.value;


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
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                sealed class _s4 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s4, AutoCSer.TestCase.TcpOpenStreamServer.Member, _p5>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.index];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c4, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsKeepCallback = 1 };
                sealed class _s5 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s5, AutoCSer.TestCase.TcpOpenStreamServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.getProperty;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c5, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsKeepCallback = 1 };
                sealed class _s6 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s6, AutoCSer.TestCase.TcpOpenStreamServer.Member>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.property;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c6, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsKeepCallback = 1 };
                sealed class _s7 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s7, AutoCSer.TestCase.TcpOpenStreamServer.Member, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.property = inputParameter.value;


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
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(value.Type);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
                static TcpOpenStreamServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), typeof(_p4), typeof(_p5), null }
                        , new System.Type[] { typeof(_p1), null });
                }

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
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int left;
                    public int right;
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
                {
                    public int index;
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
                        attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenStreamServer.Member", typeof(AutoCSer.TestCase.TcpOpenStreamServer.Member));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenStreamServer.Client<TcpOpenStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };


                public AutoCSer.Net.TcpServer.ReturnValue<int> field
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
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                                {
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c1, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> this[int left, int right]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenStreamServer._p3 _inputParameter_ = new TcpOpenStreamServer._p3
                                {
                                    
                                    left = left,
                                    
                                    right = right,
                                };
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenStreamServer._p3, TcpOpenStreamServer._p1>(_c2, ref _wait_, ref _inputParameter_);
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
                                    
                                    left = left,
                                    
                                    right = right,
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c3, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


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
                                TcpOpenStreamServer._p5 _inputParameter_ = new TcpOpenStreamServer._p5
                                {
                                    
                                    index = index,
                                };
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenStreamServer._p5, TcpOpenStreamServer._p1>(_c4, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenStreamServer._p1>(_c5, ref _wait_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                public AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpOpenStreamServer._p1>(_c6, ref _wait_);
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
                                TcpOpenStreamServer._p2 _inputParameter_ = new TcpOpenStreamServer._p2
                                {
                                    
                                    value = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c7, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };


                static TcpOpenStreamClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p2), typeof(TcpOpenStreamServer._p3), typeof(TcpOpenStreamServer._p4), typeof(TcpOpenStreamServer._p5), null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p1), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpOpenStreamServer
{
        internal partial class Session
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[2];
                names[0].Set(@"(AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender,string,string)login", 0);
                names[1].Set(@"(AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender)myName", 1);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenStreamServer.Session TCP服务
            /// </summary>
            public sealed class TcpOpenStreamServer : AutoCSer.Net.TcpOpenStreamServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenStreamServer.Session Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenStreamServer.Session TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenStreamServer(AutoCSer.Net.TcpOpenStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenStreamServer.Session", typeof(AutoCSer.TestCase.TcpOpenStreamServer.Session))), verify, log)
                {
                    Value =new AutoCSer.TestCase.TcpOpenStreamServer.Session();
                    setCommandData(2);
                    setVerifyCommand(0);
                    setCommand(1);
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
                                    
                                    bool Return;
                                    
                                    Return = Value.login(sender, inputParameter.user, inputParameter.password);
                                    if (Return) sender.SetVerifyMethod();
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value);
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsKeepCallback = 1 };
                sealed class _s1 : AutoCSer.Net.TcpOpenStreamServer.ServerCall<_s1, AutoCSer.TestCase.TcpOpenStreamServer.Session>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                    {
                        try
                        {
                            
                            string Return;

                            
                            Return = serverValue.myName(Sender);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                        AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender sender = Sender;
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c1, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsKeepCallback = 1 };
                static TcpOpenStreamServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), typeof(_p3), null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public string password;
                    public string user;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public string Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public string Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (string)value; }
                    }
#endif
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
                /// <param name="verifyMethod">TCP验证方法</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpOpenStreamClient(AutoCSer.Net.TcpOpenStreamServer.ServerAttribute attribute = null, Func<TcpOpenStreamClient, AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpOpenStreamServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenStreamServer.Session", typeof(AutoCSer.TestCase.TcpOpenStreamServer.Session));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenStreamServer.Client<TcpOpenStreamClient>(this, attribute, log, clientRoute, verifyMethod);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsVerifyMethod = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p1 _inputParameter_ = new TcpOpenStreamServer._p1
                            {
                                
                                user = user,
                                
                                password = password,
                            };
                            TcpOpenStreamServer._p2 _outputParameter_ = new TcpOpenStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p1, TcpOpenStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> loginAwaiter(AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _sender_;
                    if (_socket_ != null)
                    {
                        TcpOpenStreamServer._p1 _inputParameter_ = new TcpOpenStreamServer._p1
                        {
                            
                            user = user,
                            
                            password = password,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<TcpOpenStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpOpenStreamServer._p3 _outputParameter_ = new TcpOpenStreamServer._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpOpenStreamServer._p3>(_c1, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenStreamServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<string> myNameAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<string> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<string>();
                    AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>>(_a1, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpOpenStreamClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p1), null }
                        , new System.Type[] { typeof(TcpOpenStreamServer._p2), typeof(TcpOpenStreamServer._p3), null });
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        internal partial class GetSession
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static string _M1(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_)
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.GetSession.myName(_sender_);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP 服务客户端识别测试
            /// </summary>
            public partial class GetSession
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p1>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.SessionServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p1 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p1
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p1>(_c1, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p1>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<string> myNameAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<string> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<string>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.SessionServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>>(_a1, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        internal partial class Session
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M2(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, string user, string password)
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.Session.login(_sender_, user, password);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP 服务客户端识别测试
            /// </summary>
            public partial class Session
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p2 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p2
                            {
                                
                                p0 = user,
                                
                                p1 = password,
                            };
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p3 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p2, AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p3>(_c2, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> loginAwaiter(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p2 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p2
                        {
                            
                            p0 = user,
                            
                            p1 = password,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p2, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a2, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}
namespace AutoCSer.TestCase.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class SessionServer : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public SessionServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("SessionServer", typeof(AutoCSer.TestCase.TcpStaticServer.Session), true)), verify, onCustomData, log, false)
            {
                setCommandData(2);
                setCommand(0);
                setVerifyCommand(1);
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
                            {
                                (_s0/**/.Pop() ?? new _s0()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p2 inputParameter = new _p2();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p3 _outputParameter_ = new _p3();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticServer.Session/**/.TcpStaticServer._M2(sender, inputParameter.p0, inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                _outputParameter_.Return = Return;
                                sender.Push(_c2, ref _outputParameter_);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticServer.ServerCall<_s0>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                {
                    try
                    {
                        
                        string Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticServer.GetSession/**/.TcpStaticServer._M1(Sender);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c1, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public string Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public string Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (string)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p2
            {
                public string p0;
                public string p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
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
            static SessionServer()
            {
                CompileSerialize(new System.Type[] { typeof(_p2), null }
                    , new System.Type[] { typeof(_p1), typeof(_p3), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class SessionServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 自定义数据包处理
                /// </summary>
                public Action<AutoCSer.SubArray<byte>> OnCustomData;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            static SessionServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("SessionServer", typeof(AutoCSer.TestCase.TcpStaticServer.Session));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 SessionServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p2), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p1), typeof(AutoCSer.TestCase.TcpStaticServer/**/.SessionServer/**/._p3), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        internal partial class Json
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M5(int a, int b)
                {

                    AutoCSer.TestCase.TcpStaticServer.Json.Add(a, b);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M3()
                {

                    AutoCSer.TestCase.TcpStaticServer.Json.Inc();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M4(int a)
                {

                    AutoCSer.TestCase.TcpStaticServer.Json.Set(a);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M8(int a, int b)
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.Json.add(a, b);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M6()
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.Json.inc();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M7(int a)
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.Json.inc(a);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
            /// </summary>
            public partial class Json
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c5, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.Awaiter AddAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a5, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
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
                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.Awaiter IncAwaiter()
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a3, _awaiter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4
                            {
                                
                                a = a,
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
                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.Awaiter SetAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a4, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5, AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a8, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>(_c6, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a6, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4
                            {
                                
                                a = a,
                            };
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4, AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a7, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        internal partial class JsonOut
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M11(int a, int b, out int c)
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.JsonOut.add(a, b, out c);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M9(out int a)
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.JsonOut.inc(out a);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M10(int a, out int b)
                {

                    
                    return AutoCSer.TestCase.TcpStaticServer.JsonOut.inc(a, out b);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
            /// </summary>
            public partial class JsonOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 9 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p10> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p10>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p9
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p10 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p10
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p9, AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p10>(_c11, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            c = _outputParameter_.c;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p10>.PushNotNull(_wait_);
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4
                            {
                            };
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p7 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4, AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p7>(_c9, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            a = _outputParameter_.a;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p7>.PushNotNull(_wait_);
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p8>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.JsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5
                            {
                                
                                a = a,
                            };
                            
                            AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p8 _outputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p8
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5, AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p8>(_c10, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            b = _outputParameter_.b;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p8>.PushNotNull(_wait_);
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}
namespace AutoCSer.TestCase.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class JsonServer : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[9];
                names[0].Set(@"AutoCSer.TestCase.TcpStaticServer.Json()Inc", 0);
                names[1].Set(@"AutoCSer.TestCase.TcpStaticServer.Json(int)Set", 1);
                names[2].Set(@"AutoCSer.TestCase.TcpStaticServer.Json(int,int)Add", 2);
                names[3].Set(@"AutoCSer.TestCase.TcpStaticServer.Json()inc", 3);
                names[4].Set(@"AutoCSer.TestCase.TcpStaticServer.Json(int)inc", 4);
                names[5].Set(@"AutoCSer.TestCase.TcpStaticServer.Json(int,int)add", 5);
                names[6].Set(@"AutoCSer.TestCase.TcpStaticServer.JsonOut(out int)inc", 6);
                names[7].Set(@"AutoCSer.TestCase.TcpStaticServer.JsonOut(int,out int)inc", 7);
                names[8].Set(@"AutoCSer.TestCase.TcpStaticServer.JsonOut(int,int,out int)add", 8);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public JsonServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("JsonServer", typeof(AutoCSer.TestCase.TcpStaticServer.JsonOut), true)), verify, onCustomData, log, false)
            {
                setCommandData(9);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setCommand(4);
                setCommand(5);
                setCommand(6);
                setCommand(7);
                setCommand(8);
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
                            {
                                (_s0/**/.Pop() ?? new _s0()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s2/**/.Pop() ?? new _s2()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            {
                                (_s3/**/.Pop() ?? new _s3()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s4/**/.Pop() ?? new _s4()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s5/**/.Pop() ?? new _s5()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s6/**/.Pop() ?? new _s6()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s7/**/.Pop() ?? new _s7()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                    case 8:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s8/**/.Pop() ?? new _s8()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticServer.ServerCall<_s0>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.TestCase.TcpStaticServer.Json/**/.TcpStaticServer._M3();

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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
            sealed class _s1 : AutoCSer.Net.TcpStaticServer.ServerCall<_s1, _p4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.TestCase.TcpStaticServer.Json/**/.TcpStaticServer._M4(inputParameter.a);

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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
            sealed class _s2 : AutoCSer.Net.TcpStaticServer.ServerCall<_s2, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.TestCase.TcpStaticServer.Json/**/.TcpStaticServer._M5(inputParameter.a, inputParameter.b);

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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
            sealed class _s3 : AutoCSer.Net.TcpStaticServer.ServerCall<_s3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticServer.Json/**/.TcpStaticServer._M6();

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c6, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
            sealed class _s4 : AutoCSer.Net.TcpStaticServer.ServerCall<_s4, _p4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticServer.Json/**/.TcpStaticServer._M7(inputParameter.a);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c7, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
            sealed class _s5 : AutoCSer.Net.TcpStaticServer.ServerCall<_s5, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticServer.Json/**/.TcpStaticServer._M8(inputParameter.a, inputParameter.b);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c8, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
            sealed class _s6 : AutoCSer.Net.TcpStaticServer.ServerCall<_s6, _p4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticServer.JsonOut/**/.TcpStaticServer._M9(out value.Value.a);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p7> value = new AutoCSer.Net.TcpServer.ReturnValue<_p7>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c9, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
            sealed class _s7 : AutoCSer.Net.TcpStaticServer.ServerCall<_s7, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticServer.JsonOut/**/.TcpStaticServer._M10(inputParameter.a, out value.Value.b);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c10, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsBuildOutputThread = true };
            sealed class _s8 : AutoCSer.Net.TcpStaticServer.ServerCall<_s8, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p10> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticServer.JsonOut/**/.TcpStaticServer._M11(inputParameter.a, inputParameter.b, out value.Value.c);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p10> value = new AutoCSer.Net.TcpServer.ReturnValue<_p10>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c11, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 10, IsBuildOutputThread = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
            {
                public int a;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
            {
                public int a;
                public int b;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
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
            internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int a;
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
            internal struct _p8
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int b;
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
            internal struct _p9
            {
                public int a;
                public int b;
                public int c;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p10
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int c;
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
            static JsonServer()
            {
                CompileSerialize(new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(_p4), typeof(_p5), typeof(_p9), null }
                    , new System.Type[] { typeof(_p6), typeof(_p7), typeof(_p8), typeof(_p10), null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class JsonServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 自定义数据包处理
                /// </summary>
                public Action<AutoCSer.SubArray<byte>> OnCustomData;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            static JsonServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("JsonServer", typeof(AutoCSer.TestCase.TcpStaticServer.JsonOut));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 JsonServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p4), typeof(AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p5), typeof(AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p6), typeof(AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p7), typeof(AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p8), typeof(AutoCSer.TestCase.TcpStaticServer/**/.JsonServer/**/._p10), null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        public partial class Member
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M12()
                {
                    return AutoCSer.TestCase.TcpStaticServer.Member/**/.field;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M13(int value)
                {
                    AutoCSer.TestCase.TcpStaticServer.Member/**/.field = value;

                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP服务字段与属性支持测试
            /// </summary>
            public partial class Member
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试字段
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.MemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8> _outputParameter_ = _socket_.WaitGet(_c12, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.MemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p9
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c13, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        public partial class MemberProperty
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M14()
                {
                    return AutoCSer.TestCase.TcpStaticServer.MemberProperty/**/.getProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M15()
                {
                    return AutoCSer.TestCase.TcpStaticServer.MemberProperty/**/.property;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M16(int value)
                {
                    AutoCSer.TestCase.TcpStaticServer.MemberProperty/**/.property = value;

                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP服务字段与属性支持测试
            /// </summary>
            public partial class MemberProperty
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性[不支持不可读属性]
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.MemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8> _outputParameter_ = _socket_.WaitGet(_c14, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试属性
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.MemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8> _outputParameter_ = _socket_.WaitGet(_c15, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticClient/**/.MemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p9
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c16, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


            }
        }
}
namespace AutoCSer.TestCase.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class MemberServer : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public MemberServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("MemberServer", typeof(AutoCSer.TestCase.TcpStaticServer.Member), true)), verify, onCustomData, log, false)
            {
                setCommandData(5);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setCommand(4);
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
                            {
                                (_s0/**/.Pop() ?? new _s0()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                                (_s2/**/.Pop() ?? new _s2()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                            {
                                (_s3/**/.Pop() ?? new _s3()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s4/**/.Pop() ?? new _s4()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticServer.ServerCall<_s0>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.TestCase.TcpStaticServer.Member/**/.TcpStaticServer._M12();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c12, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s1 : AutoCSer.Net.TcpStaticServer.ServerCall<_s1, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.TcpStaticServer.Member/**/.TcpStaticServer._M13(inputParameter.p0);


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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c13 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
            sealed class _s2 : AutoCSer.Net.TcpStaticServer.ServerCall<_s2>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.TestCase.TcpStaticServer.MemberProperty/**/.TcpStaticServer._M14();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c14, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c14 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s3 : AutoCSer.Net.TcpStaticServer.ServerCall<_s3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.TestCase.TcpStaticServer.MemberProperty/**/.TcpStaticServer._M15();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c15, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c15 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s4 : AutoCSer.Net.TcpStaticServer.ServerCall<_s4, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.TcpStaticServer.MemberProperty/**/.TcpStaticServer._M16(inputParameter.p0);


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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c16 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
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
            internal struct _p9
            {
                public int p0;
            }
            static MemberServer()
            {
                CompileSerialize(new System.Type[] { typeof(_p9), null }
                    , new System.Type[] { typeof(_p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class MemberServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 自定义数据包处理
                /// </summary>
                public Action<AutoCSer.SubArray<byte>> OnCustomData;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            static MemberServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("MemberServer", typeof(AutoCSer.TestCase.TcpStaticServer.Member));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 MemberServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticServer/**/.MemberServer/**/._p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        internal partial class GetSession
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static string _M1(AutoCSer.Net.TcpInternalSimpleServer.ServerSocket _socket_)
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.GetSession.myName(_socket_);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// TCP 服务客户端识别测试
            /// </summary>
            public partial class GetSession
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p1 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p1
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleSessionServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p1>(_c1, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        internal partial class Session
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M2(AutoCSer.Net.TcpInternalSimpleServer.ServerSocket _socket_, string user, string password)
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.Session.login(_socket_, user, password);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// TCP 服务客户端识别测试
            /// </summary>
            public partial class Session
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 2, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> login(string user, string password)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p2 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p2
                    {
                        
                        p0 = user,
                        
                        p1 = password,
                    };
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p3 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p3
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleSessionServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p2, AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p3>(_c2, ref _inputParameter_, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}
namespace AutoCSer.TestCase.TcpStaticSimpleServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class SimpleSessionServer : AutoCSer.Net.TcpInternalSimpleServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public SimpleSessionServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("SimpleSessionServer", typeof(AutoCSer.TestCase.TcpStaticSimpleServer.Session), true)), verify, log, false)
            {
                setCommandData(2);
                setCommand(0);
                setVerifyCommand(1);
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
                switch (index - 128)
                {
                    case 0:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p1 _outputParameter_ = new _p1();
                                
                                string Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.GetSession/**/.TcpStaticSimpleServer._M1(socket);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c1, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p2 inputParameter = new _p2();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p3 _outputParameter_ = new _p3();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.Session/**/.TcpStaticSimpleServer._M2(socket, inputParameter.p0, inputParameter.p1);
                                if (Return) socket.SetVerifyMethod();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c2, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    default: return false;
                }
            }
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public string Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public string Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (string)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p2
            {
                public string p0;
                public string p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
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
            static SimpleSessionServer()
            {
                CompileSerialize(new System.Type[] { typeof(_p2), null }
                    , new System.Type[] { typeof(_p1), typeof(_p3), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticSimpleClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class SimpleSessionServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticSimpleServer.Client TcpClient;
            static SimpleSessionServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("SimpleSessionServer", typeof(AutoCSer.TestCase.TcpStaticSimpleServer.Session));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 SimpleSessionServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticSimpleServer.Client(config.ServerAttribute, config.Log, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p2), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p1), typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleSessionServer/**/._p3), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        internal partial class Json
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M5(int a, int b)
                {

                    AutoCSer.TestCase.TcpStaticSimpleServer.Json.Add(a, b);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M3()
                {

                    AutoCSer.TestCase.TcpStaticSimpleServer.Json.Inc();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M4(int a)
                {

                    AutoCSer.TestCase.TcpStaticSimpleServer.Json.Set(a);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M8(int a, int b)
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.Json.add(a, b);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M6()
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.Json.inc();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M7(int a)
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.Json.inc(a);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
            /// </summary>
            public partial class Json
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c5 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5
                    {
                        
                        a = a,
                        
                        b = b,
                    };
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Call(_c5, ref _inputParameter_) };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Call(_c3) };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4
                    {
                        
                        a = a,
                    };
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Call(_c4, ref _inputParameter_) };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c8 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 5 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5
                    {
                        
                        a = a,
                        
                        b = b,
                    };
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5, AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6>(_c8, ref _inputParameter_, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c6 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6>(_c6, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c7 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4
                    {
                        
                        a = a,
                    };
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4, AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6>(_c7, ref _inputParameter_, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        internal partial class JsonOut
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M11(int a, int b, out int c)
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut.add(a, b, out c);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M9(out int a)
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut.inc(out a);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M10(int a, out int b)
                {

                    
                    return AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut.inc(a, out b);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
            /// </summary>
            public partial class JsonOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c11 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 8 + 128, InputParameterIndex = 9 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p9
                    {
                        
                        a = a,
                        
                        b = b,
                    };
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p10 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p10
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p9, AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p10>(_c11, ref _inputParameter_, ref _outputParameter_);
                    
                    c = _outputParameter_.c;
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c9 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 6 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4
                    {
                    };
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p7 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p7
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4, AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p7>(_c9, ref _inputParameter_, ref _outputParameter_);
                    
                    a = _outputParameter_.a;
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c10 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 7 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5
                    {
                        
                        a = a,
                    };
                    
                    AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p8 _outputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p8
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleJsonServer/**/.TcpClient.Get<AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5, AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p8>(_c10, ref _inputParameter_, ref _outputParameter_);
                    
                    b = _outputParameter_.b;
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}
namespace AutoCSer.TestCase.TcpStaticSimpleServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class SimpleJsonServer : AutoCSer.Net.TcpInternalSimpleServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[9];
                names[0].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.Json()Inc", 0);
                names[1].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.Json(int)Set", 1);
                names[2].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.Json(int,int)Add", 2);
                names[3].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.Json()inc", 3);
                names[4].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.Json(int)inc", 4);
                names[5].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.Json(int,int)add", 5);
                names[6].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut(out int)inc", 6);
                names[7].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut(int,out int)inc", 7);
                names[8].Set(@"AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut(int,int,out int)add", 8);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public SimpleJsonServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("SimpleJsonServer", typeof(AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut), true)), verify, log, false)
            {
                setCommandData(9);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setCommand(4);
                setCommand(5);
                setCommand(6);
                setCommand(7);
                setCommand(8);
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
                switch (index - 128)
                {
                    case 0:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                
                                 AutoCSer.TestCase.TcpStaticSimpleServer.Json/**/.TcpStaticSimpleServer._M3();
                                return socket.Send();
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.Send(returnType);
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (socket.DeSerialize(ref data, ref inputParameter))
                            {
                                
                                 AutoCSer.TestCase.TcpStaticSimpleServer.Json/**/.TcpStaticSimpleServer._M4(inputParameter.a);
                                return socket.Send();
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.Send(returnType);
                    case 2:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p5 inputParameter = new _p5();
                            if (socket.DeSerialize(ref data, ref inputParameter))
                            {
                                
                                 AutoCSer.TestCase.TcpStaticSimpleServer.Json/**/.TcpStaticSimpleServer._M5(inputParameter.a, inputParameter.b);
                                return socket.Send();
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.Send(returnType);
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p6 _outputParameter_ = new _p6();
                                
                                int Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.Json/**/.TcpStaticSimpleServer._M6();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c6, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 4:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (socket.DeSerialize(ref data, ref inputParameter))
                            {
                                _p6 _outputParameter_ = new _p6();
                                
                                int Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.Json/**/.TcpStaticSimpleServer._M7(inputParameter.a);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c7, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 5:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p5 inputParameter = new _p5();
                            if (socket.DeSerialize(ref data, ref inputParameter))
                            {
                                _p6 _outputParameter_ = new _p6();
                                
                                int Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.Json/**/.TcpStaticSimpleServer._M8(inputParameter.a, inputParameter.b);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c8, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 6:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (socket.DeSerialize(ref data, ref inputParameter))
                            {
                                _p7 _outputParameter_ = new _p7();
                                
                                int Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut/**/.TcpStaticSimpleServer._M9(out _outputParameter_.a);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c9, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 7:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p5 inputParameter = new _p5();
                            if (socket.DeSerialize(ref data, ref inputParameter))
                            {
                                _p8 _outputParameter_ = new _p8();
                                
                                int Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut/**/.TcpStaticSimpleServer._M10(inputParameter.a, out _outputParameter_.b);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c10, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 8:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (socket.DeSerialize(ref data, ref inputParameter))
                            {
                                _p10 _outputParameter_ = new _p10();
                                
                                int Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut/**/.TcpStaticSimpleServer._M11(inputParameter.a, inputParameter.b, out _outputParameter_.c);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c11, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    default: return false;
                }
            }
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c5 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c6 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 6 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c7 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 6 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c8 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 6 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c9 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 7 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c10 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 8 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c11 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 10 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
            {
                public int a;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
            {
                public int a;
                public int b;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
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
            internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int a;
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
            internal struct _p8
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int b;
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
            internal struct _p9
            {
                public int a;
                public int b;
                public int c;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p10
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int c;
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
            static SimpleJsonServer()
            {
                CompileSerialize(new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(_p4), typeof(_p5), typeof(_p9), null }
                    , new System.Type[] { typeof(_p6), typeof(_p7), typeof(_p8), typeof(_p10), null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticSimpleClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class SimpleJsonServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticSimpleServer.Client TcpClient;
            static SimpleJsonServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("SimpleJsonServer", typeof(AutoCSer.TestCase.TcpStaticSimpleServer.JsonOut));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 SimpleJsonServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticSimpleServer.Client(config.ServerAttribute, config.Log, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p4), typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p5), typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p6), typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p7), typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p8), typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleJsonServer/**/._p10), null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        public partial class Member
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M12()
                {
                    return AutoCSer.TestCase.TcpStaticSimpleServer.Member/**/.field;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M13(int value)
                {
                    AutoCSer.TestCase.TcpStaticSimpleServer.Member/**/.field = value;

                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// TCP服务字段与属性支持测试
            /// </summary>
            public partial class Member
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c12 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试字段
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        
                        AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p8 _outputParameter_ = default(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p8);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleMemberServer/**/.TcpClient.Get(_c12, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    set
                    {
                        
                        AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p9
                        {
                            
                            p0 = value,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleMemberServer/**/.TcpClient.Call(_c13, ref _inputParameter_);
                        if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                        throw new Exception(_returnType_.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c13 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 9, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        public partial class MemberProperty
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M14()
                {
                    return AutoCSer.TestCase.TcpStaticSimpleServer.MemberProperty/**/.getProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M15()
                {
                    return AutoCSer.TestCase.TcpStaticSimpleServer.MemberProperty/**/.property;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M16(int value)
                {
                    AutoCSer.TestCase.TcpStaticSimpleServer.MemberProperty/**/.property = value;

                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// TCP服务字段与属性支持测试
            /// </summary>
            public partial class MemberProperty
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c14 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性[不支持不可读属性]
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        
                        AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p8 _outputParameter_ = default(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p8);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleMemberServer/**/.TcpClient.Get(_c14, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c15 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试属性
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        
                        AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p8 _outputParameter_ = default(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p8);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleMemberServer/**/.TcpClient.Get(_c15, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    set
                    {
                        
                        AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p9
                        {
                            
                            p0 = value,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.TestCase.TcpStaticSimpleClient/**/.SimpleMemberServer/**/.TcpClient.Call(_c16, ref _inputParameter_);
                        if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                        throw new Exception(_returnType_.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c16 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 9, IsSimpleSerializeInputParamter = true };


            }
        }
}
namespace AutoCSer.TestCase.TcpStaticSimpleServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class SimpleMemberServer : AutoCSer.Net.TcpInternalSimpleServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public SimpleMemberServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("SimpleMemberServer", typeof(AutoCSer.TestCase.TcpStaticSimpleServer.Member), true)), verify, log, false)
            {
                setCommandData(5);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setCommand(4);
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
                switch (index - 128)
                {
                    case 0:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p8 _outputParameter_ = new _p8();
                                
                                int Return;
                                Return = AutoCSer.TestCase.TcpStaticSimpleServer.Member/**/.TcpStaticSimpleServer._M12();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c12, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                
                                AutoCSer.TestCase.TcpStaticSimpleServer.Member/**/.TcpStaticSimpleServer._M13(inputParameter.p0);
                                return socket.Send();
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.Send(returnType);
                    case 2:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p8 _outputParameter_ = new _p8();
                                
                                int Return;
                                Return = AutoCSer.TestCase.TcpStaticSimpleServer.MemberProperty/**/.TcpStaticSimpleServer._M14();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c14, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p8 _outputParameter_ = new _p8();
                                
                                int Return;
                                Return = AutoCSer.TestCase.TcpStaticSimpleServer.MemberProperty/**/.TcpStaticSimpleServer._M15();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c15, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 4:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                
                                AutoCSer.TestCase.TcpStaticSimpleServer.MemberProperty/**/.TcpStaticSimpleServer._M16(inputParameter.p0);
                                return socket.Send();
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.Send(returnType);
                    default: return false;
                }
            }
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c12 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c13 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c14 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c15 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c16 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
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
            internal struct _p9
            {
                public int p0;
            }
            static SimpleMemberServer()
            {
                CompileSerialize(new System.Type[] { typeof(_p9), null }
                    , new System.Type[] { typeof(_p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticSimpleClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class SimpleMemberServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticSimpleServer.Client TcpClient;
            static SimpleMemberServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("SimpleMemberServer", typeof(AutoCSer.TestCase.TcpStaticSimpleServer.Member));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 SimpleMemberServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticSimpleServer.Client(config.ServerAttribute, config.Log, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticSimpleServer/**/.SimpleMemberServer/**/._p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        internal partial class GetSession
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static string _M1(AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender _sender_)
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.GetSession.myName(_sender_);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// TCP 服务客户端识别测试
            /// </summary>
            public partial class GetSession
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<string> myName()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p1>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamSessionServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p1 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p1
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p1>(_c1, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p1>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端读客户端标识测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<string> myNameAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<string> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<string>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamSessionServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<string>>(_a1, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        internal partial class Session
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M2(AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender _sender_, string user, string password)
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.Session.login(_sender_, user, password);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// TCP 服务客户端识别测试
            /// </summary>
            public partial class Session
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsVerifyMethod = true, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p2 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p2
                            {
                                
                                p0 = user,
                                
                                p1 = password,
                            };
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p3 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p2, AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p3>(_c2, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 服务器端写客户端标识测试+服务器端验证函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> loginAwaiter(AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _sender_, string user, string password)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _sender_;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p2 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p2
                        {
                            
                            p0 = user,
                            
                            p1 = password,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p2, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a2, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}
namespace AutoCSer.TestCase.TcpStaticStreamServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class StreamSessionServer : AutoCSer.Net.TcpInternalStreamServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public StreamSessionServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamSessionServer", typeof(AutoCSer.TestCase.TcpStaticStreamServer.Session), true)), verify, log)
            {
                setCommandData(2);
                setCommand(0);
                setVerifyCommand(1);
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
                            {
                                (_s0/**/.Pop() ?? new _s0()).Set(sender);
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
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p2 inputParameter = new _p2();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p3 _outputParameter_ = new _p3();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.TcpStaticStreamServer.Session/**/.TcpStaticStreamServer._M2(sender, inputParameter.p0, inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                _outputParameter_.Return = Return;
                                sender.Push(_c2, ref _outputParameter_);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s0>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                {
                    try
                    {
                        
                        string Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.GetSession/**/.TcpStaticStreamServer._M1(Sender);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<string>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public string Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public string Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (string)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p2
            {
                public string p0;
                public string p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
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
            static StreamSessionServer()
            {
                CompileSerialize(new System.Type[] { typeof(_p2), null }
                    , new System.Type[] { typeof(_p1), typeof(_p3), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticStreamClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class StreamSessionServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalStreamServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticStreamServer.Client TcpClient;
            static StreamSessionServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamSessionServer", typeof(AutoCSer.TestCase.TcpStaticStreamServer.Session));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 StreamSessionServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticStreamServer.Client(config.ServerAttribute, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p2), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p1), typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamSessionServer/**/._p3), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        internal partial class Json
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M5(int a, int b)
                {

                    AutoCSer.TestCase.TcpStaticStreamServer.Json.Add(a, b);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M3()
                {

                    AutoCSer.TestCase.TcpStaticStreamServer.Json.Inc();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M4(int a)
                {

                    AutoCSer.TestCase.TcpStaticStreamServer.Json.Set(a);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M8(int a, int b)
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.Json.add(a, b);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M6()
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.Json.inc();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M7(int a)
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.Json.inc(a);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
            /// </summary>
            public partial class Json
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c5, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 多参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.Awaiter AddAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a5, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
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
                /// <summary>
                /// 无参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.Awaiter IncAwaiter()
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a3, _awaiter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue Set(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4
                            {
                                
                                a = a,
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
                /// <summary>
                /// 单参数无返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.Awaiter SetAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a4, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5, AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 多参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> addAwaiter(int a, int b)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5
                        {
                            
                            a = a,
                            
                            b = b,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a8, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>(_c6, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a6, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4
                            {
                                
                                a = a,
                            };
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4, AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 单参数有返回值调用测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> incAwaiter(int a)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4
                        {
                            
                            a = a,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a7, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        internal partial class JsonOut
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M11(int a, int b, out int c)
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.JsonOut.add(a, b, out c);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M9(out int a)
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.JsonOut.inc(out a);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M10(int a, out int b)
                {

                    
                    return AutoCSer.TestCase.TcpStaticStreamServer.JsonOut.inc(a, out b);
                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
            /// </summary>
            public partial class JsonOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 9 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> add(int a, int b, out int c)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p10> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p10>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p9
                            {
                                
                                a = a,
                                
                                b = b,
                            };
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p10 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p10
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p9, AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p10>(_c11, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            c = _outputParameter_.c;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p10>.PushNotNull(_wait_);
                    }
                    c = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(out int a)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4
                            {
                            };
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p7 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4, AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p7>(_c9, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            a = _outputParameter_.a;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p7>.PushNotNull(_wait_);
                    }
                    a = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 5 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 混合输出参数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> inc(int a, out int b)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p8>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamJsonServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5
                            {
                                
                                a = a,
                            };
                            
                            AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p8 _outputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p8
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5, AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p8>(_c10, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            b = _outputParameter_.b;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p8>.PushNotNull(_wait_);
                    }
                    b = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}
namespace AutoCSer.TestCase.TcpStaticStreamServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class StreamJsonServer : AutoCSer.Net.TcpInternalStreamServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[9];
                names[0].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.Json()Inc", 0);
                names[1].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.Json(int)Set", 1);
                names[2].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.Json(int,int)Add", 2);
                names[3].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.Json()inc", 3);
                names[4].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.Json(int)inc", 4);
                names[5].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.Json(int,int)add", 5);
                names[6].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.JsonOut(out int)inc", 6);
                names[7].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.JsonOut(int,out int)inc", 7);
                names[8].Set(@"AutoCSer.TestCase.TcpStaticStreamServer.JsonOut(int,int,out int)add", 8);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public StreamJsonServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamJsonServer", typeof(AutoCSer.TestCase.TcpStaticStreamServer.JsonOut), true)), verify, log)
            {
                setCommandData(9);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setCommand(4);
                setCommand(5);
                setCommand(6);
                setCommand(7);
                setCommand(8);
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
                            {
                                (_s0/**/.Pop() ?? new _s0()).Set(sender);
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
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s2/**/.Pop() ?? new _s2()).Set(sender, ref inputParameter);
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
                            {
                                (_s3/**/.Pop() ?? new _s3()).Set(sender);
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
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s4/**/.Pop() ?? new _s4()).Set(sender, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s5/**/.Pop() ?? new _s5()).Set(sender, ref inputParameter);
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
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s6/**/.Pop() ?? new _s6()).Set(sender, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s7/**/.Pop() ?? new _s7()).Set(sender, ref inputParameter);
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
                    case 8:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s8/**/.Pop() ?? new _s8()).Set(sender, ref inputParameter);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s0>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.TestCase.TcpStaticStreamServer.Json/**/.TcpStaticStreamServer._M3();

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(value.Type);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
            sealed class _s1 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s1, _p4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.TestCase.TcpStaticStreamServer.Json/**/.TcpStaticStreamServer._M4(inputParameter.a);

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(value.Type);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
            sealed class _s2 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s2, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.TestCase.TcpStaticStreamServer.Json/**/.TcpStaticStreamServer._M5(inputParameter.a, inputParameter.b);

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(value.Type);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsKeepCallback = 1 };
            sealed class _s3 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.Json/**/.TcpStaticStreamServer._M6();

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c6, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsKeepCallback = 1 };
            sealed class _s4 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s4, _p4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.Json/**/.TcpStaticStreamServer._M7(inputParameter.a);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c7, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsKeepCallback = 1 };
            sealed class _s5 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s5, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.Json/**/.TcpStaticStreamServer._M8(inputParameter.a, inputParameter.b);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c8, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsKeepCallback = 1 };
            sealed class _s6 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s6, _p4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.JsonOut/**/.TcpStaticStreamServer._M9(out value.Value.a);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p7> value = new AutoCSer.Net.TcpServer.ReturnValue<_p7>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c9, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsKeepCallback = 1 };
            sealed class _s7 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s7, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.JsonOut/**/.TcpStaticStreamServer._M10(inputParameter.a, out value.Value.b);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c10, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsKeepCallback = 1 };
            sealed class _s8 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s8, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p10> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.JsonOut/**/.TcpStaticStreamServer._M11(inputParameter.a, inputParameter.b, out value.Value.c);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p10> value = new AutoCSer.Net.TcpServer.ReturnValue<_p10>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c11, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 10, IsKeepCallback = 1 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
            {
                public int a;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
            {
                public int a;
                public int b;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
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
            internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int a;
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
            internal struct _p8
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int b;
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
            internal struct _p9
            {
                public int a;
                public int b;
                public int c;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p10
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                public int c;
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
            static StreamJsonServer()
            {
                CompileSerialize(new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(_p4), typeof(_p5), typeof(_p9), null }
                    , new System.Type[] { typeof(_p6), typeof(_p7), typeof(_p8), typeof(_p10), null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticStreamClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class StreamJsonServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalStreamServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticStreamServer.Client TcpClient;
            static StreamJsonServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamJsonServer", typeof(AutoCSer.TestCase.TcpStaticStreamServer.JsonOut));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 StreamJsonServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticStreamServer.Client(config.ServerAttribute, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p4), typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p5), typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p6), typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p7), typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p8), typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamJsonServer/**/._p10), null });
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        public partial class Member
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M12()
                {
                    return AutoCSer.TestCase.TcpStaticStreamServer.Member/**/.field;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M13(int value)
                {
                    AutoCSer.TestCase.TcpStaticStreamServer.Member/**/.field = value;

                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// TCP服务字段与属性支持测试
            /// </summary>
            public partial class Member
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试字段
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> field
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamMemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8> _outputParameter_ = _socket_.WaitGet(_c12, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamMemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p9
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c13, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        public partial class MemberProperty
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M14()
                {
                    return AutoCSer.TestCase.TcpStaticStreamServer.MemberProperty/**/.getProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M15()
                {
                    return AutoCSer.TestCase.TcpStaticStreamServer.MemberProperty/**/.property;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M16(int value)
                {
                    AutoCSer.TestCase.TcpStaticStreamServer.MemberProperty/**/.property = value;

                }
            }
        }
}namespace AutoCSer.TestCase.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// TCP服务字段与属性支持测试
            /// </summary>
            public partial class MemberProperty
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性[不支持不可读属性]
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> getProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamMemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8> _outputParameter_ = _socket_.WaitGet(_c14, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 测试属性
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> property
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamMemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8> _outputParameter_ = _socket_.WaitGet(_c15, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.TestCase.TcpStaticStreamClient/**/.StreamMemberServer/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p9 _inputParameter_ = new AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p9
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c16, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 9, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


            }
        }
}
namespace AutoCSer.TestCase.TcpStaticStreamServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class StreamMemberServer : AutoCSer.Net.TcpInternalStreamServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public StreamMemberServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamMemberServer", typeof(AutoCSer.TestCase.TcpStaticStreamServer.Member), true)), verify, log)
            {
                setCommandData(5);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setCommand(4);
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
                            {
                                (_s0/**/.Pop() ?? new _s0()).Set(sender);
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
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, ref inputParameter);
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
                                (_s2/**/.Pop() ?? new _s2()).Set(sender);
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
                            {
                                (_s3/**/.Pop() ?? new _s3()).Set(sender);
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
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s4/**/.Pop() ?? new _s4()).Set(sender, ref inputParameter);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s0>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.Member/**/.TcpStaticStreamServer._M12();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c12, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true };
            sealed class _s1 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s1, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.TcpStaticStreamServer.Member/**/.TcpStaticStreamServer._M13(inputParameter.p0);


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(value.Type);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c13 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
            sealed class _s2 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s2>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.MemberProperty/**/.TcpStaticStreamServer._M14();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c14, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c14 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true };
            sealed class _s3 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.TestCase.TcpStaticStreamServer.MemberProperty/**/.TcpStaticStreamServer._M15();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c15, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c15 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsSimpleSerializeOutputParamter = true };
            sealed class _s4 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s4, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.TcpStaticStreamServer.MemberProperty/**/.TcpStaticStreamServer._M16(inputParameter.p0);


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(value.Type);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c16 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
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
            internal struct _p9
            {
                public int p0;
            }
            static StreamMemberServer()
            {
                CompileSerialize(new System.Type[] { typeof(_p9), null }
                    , new System.Type[] { typeof(_p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.TestCase.TcpStaticStreamClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class StreamMemberServer
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalStreamServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticStreamServer.Client TcpClient;
            static StreamMemberServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamMemberServer", typeof(AutoCSer.TestCase.TcpStaticStreamServer.Member));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 StreamMemberServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticStreamServer.Client(config.ServerAttribute, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.TcpStaticStreamServer/**/.StreamMemberServer/**/._p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
#endif