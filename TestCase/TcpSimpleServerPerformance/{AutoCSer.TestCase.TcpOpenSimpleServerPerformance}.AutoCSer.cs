//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.TcpOpenSimpleServerPerformance
{
        public partial class OpenSimpleServer
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[6];
                names[0].Set(@"(int,int,System.Func<AutoCSer.Net.TcpServer.ReturnValue<int>,bool>)addAsynchronous", 0);
                names[1].Set(@"(int,int)addQueue", 1);
                names[2].Set(@"(int,int)addSynchronous", 2);
                names[3].Set(@"(int,int)addTcpTask", 3);
                names[4].Set(@"(int,int)addThreadPool", 4);
                names[5].Set(@"(int,int)addTimeoutTask", 5);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer Value;
                /// <summary>
                /// AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="value">TCP服务目标对象</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer value = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer", typeof(AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer))), verify, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer();
                    setCommandData(6);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
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
                                    _p2 outputParameter = new _p2();
                                    
                                    Value.addAsynchronous(inputParameter.left, inputParameter.right,  socket.GetCallback<_p2, int>(_c0, ref outputParameter));
                                    return true;
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
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(socket, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, ref inputParameter);
                                    return true;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    int Return;
                                    
                                    Return = Value.addSynchronous(inputParameter.left, inputParameter.right);
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
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s3/**/.Pop() ?? new _s3()).Set(socket, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpTask, ref inputParameter);
                                    return true;
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
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(socket, Value, AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ref inputParameter);
                                    return true;
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
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(socket, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
                                    return true;
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
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                sealed class _s1 : AutoCSer.Net.TcpOpenSimpleServer.ServerCall<_s1, AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.addQueue(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                        }
                        catch (Exception error)
                        {
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            Socket.Log(error);
                        }
                    }
                    public override void Call()
                    {
                        AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket = Socket;
                        AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                        get(ref value);
                        push(this);
                        socket.SendAsync(_c1, ref value);
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                sealed class _s3 : AutoCSer.Net.TcpOpenSimpleServer.ServerCall<_s3, AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.addTcpTask(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                        }
                        catch (Exception error)
                        {
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            Socket.Log(error);
                        }
                    }
                    public override void Call()
                    {
                        AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket = Socket;
                        AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                        get(ref value);
                        push(this);
                        socket.SendAsync(_c3, ref value);
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                sealed class _s4 : AutoCSer.Net.TcpOpenSimpleServer.ServerCall<_s4, AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.addThreadPool(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                        }
                        catch (Exception error)
                        {
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            Socket.Log(error);
                        }
                    }
                    public override void Call()
                    {
                        AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket = Socket;
                        AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                        get(ref value);
                        push(this);
                        socket.SendAsync(_c4, ref value);
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                sealed class _s5 : AutoCSer.Net.TcpOpenSimpleServer.ServerCall<_s5, AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.addTimeoutTask(inputParameter.left, inputParameter.right);

                            value.Value.Return = Return;
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                        }
                        catch (Exception error)
                        {
                            value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            Socket.Log(error);
                        }
                    }
                    public override void Call()
                    {
                        AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket = Socket;
                        AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                        get(ref value);
                        push(this);
                        socket.SendAsync(_c5, ref value);
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c5 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                static TcpOpenSimpleServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), null });
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
                        attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer", typeof(AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenSimpleServer.Client<TcpOpenSimpleClient>(this, attribute, log);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 异步计算测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> addAsynchronous(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        TcpOpenSimpleServer._p2 _outputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 计算队列测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> addQueue(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        TcpOpenSimpleServer._p2 _outputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c1, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 简单计算测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> addSynchronous(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        TcpOpenSimpleServer._p2 _outputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c2, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 计算任务测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> addTcpTask(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        TcpOpenSimpleServer._p2 _outputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c3, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 计算任务测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> addThreadPool(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        TcpOpenSimpleServer._p2 _outputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c4, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c5 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 5 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// 计算任务测试
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> addTimeoutTask(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpOpenSimpleServer._p1 _inputParameter_ = new TcpOpenSimpleServer._p1
                        {
                            
                            left = left,
                            
                            right = right,
                        };
                        TcpOpenSimpleServer._p2 _outputParameter_ = new TcpOpenSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c5, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpOpenSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p1), null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p2), null });
                }
            }
        }
}
#endif