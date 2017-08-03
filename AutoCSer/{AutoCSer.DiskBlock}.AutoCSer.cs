//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.DiskBlock
{
        public partial class Server
#if NOJIT
#else
             : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalServer.Server, AutoCSer.Net.TcpInternalServer.ServerAttribute>
#endif
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[3];
                names[0].Set(@"(AutoCSer.DiskBlock.ClientBuffer,ulong,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.DiskBlock.ClientBuffer>,bool>)read", 0);
                names[1].Set(@"(AutoCSer.Net.TcpInternalServer.ServerSocketSender,ulong,byte[],ref long)verify", 1);
                names[2].Set(@"(AutoCSer.DiskBlock.AppendBuffer,System.Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>,bool>)append", 2);
                return names;
            }
            /// <summary>
            /// DiskBlock TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.DiskBlock.Server Value;
                /// <summary>
                /// DiskBlock TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.DiskBlock.Server value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("DiskBlock", typeof(AutoCSer.DiskBlock.Server))), verify, onCustomData, log)
                {
                    Value = value ?? new AutoCSer.DiskBlock.Server();
                    setCommandData(3);
                    setCommand(0);
                    setVerifyCommand(1);
                    setCommand(2);
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
                                    _p2 outputParameter = new _p2();
                                    Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.DiskBlock.ClientBuffer>, bool> callbackReturn = sender.GetCallback<_p2, AutoCSer.DiskBlock.ClientBuffer>(_c0, ref outputParameter);
                                    if (callbackReturn != null)
                                    {
                                        
                                        Value.read(inputParameter.p0, inputParameter.p1, callbackReturn);
                                    }
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.Log(error);
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
                                    _p4 _outputParameter_ = new _p4();
                                    
                                    bool Return;
                                    
                                    Return = Value.verify(sender, inputParameter.p2, inputParameter.p0, ref inputParameter.p1);
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
                                sender.Log(error);
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
                                    _p6 outputParameter = new _p6();
                                    Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>, bool> callbackReturn = sender.GetCallback<_p6, ulong>(_c2, ref outputParameter);
                                    if (callbackReturn != null)
                                    {
                                        
                                        Value.append(inputParameter.p0, callbackReturn);
                                    }
                                    return;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                sender.Log(error);
                            }
                            sender.Push(returnType);
                            return;
                        default: return;
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2 };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsSimpleSerializeOutputParamter = true };

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public AutoCSer.DiskBlock.ClientBuffer p0;
                    public ulong p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.DiskBlock.ClientBuffer>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.DiskBlock.ClientBuffer Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.DiskBlock.ClientBuffer Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.DiskBlock.ClientBuffer)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public byte[] p0;
                    public long p1;
                    public ulong p2;
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
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
                {
                    public AutoCSer.DiskBlock.AppendBuffer p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<ulong>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public ulong Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public ulong Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (ulong)value; }
                    }
#endif
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public class TcpInternalClient : AutoCSer.Net.TcpInternalServer.MethodClient<TcpInternalClient>
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
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("DiskBlock", typeof(AutoCSer.DiskBlock.Server));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, verifyMethod ?? (Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool>)_timerVerify_);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 获取数据
                /// </summary>
                /// <param name="buffer">缓冲区，Start 指定字节数量</param>
                /// <param name="index">索引位置</param>
                public AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.DiskBlock.ClientBuffer> read(AutoCSer.DiskBlock.ClientBuffer buffer, ulong index)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2> _wait_ = _TcpClient_.GetAutoWait<TcpInternalServer._p2>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                                {
                                    
                                    p0 = buffer,
                                    
                                    p1 = index,
                                };
                                TcpInternalServer._p2 _outputParameter_ = new TcpInternalServer._p2
                                {
                                    Ret = buffer
                                };
                                _socket_.Get<TcpInternalServer._p1, TcpInternalServer._p2>(_c0, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p2> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.DiskBlock.ClientBuffer> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.DiskBlock.ClientBuffer> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4> _wait_ = _TcpClient_.GetAutoWait<TcpInternalServer._p4>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                            if (_socket_ != null)
                            {
                                TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                                {
                                    
                                    p2 = randomPrefix,
                                    
                                    p0 = md5Data,
                                    
                                    p1 = ticks,
                                };
                                TcpInternalServer._p4 _outputParameter_ = new TcpInternalServer._p4
                                {
                                    
                                    p0 = ticks,
                                };
                                _socket_.Get<TcpInternalServer._p3, TcpInternalServer._p4>(_c1, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p4> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                
                                ticks = _returnOutputParameter_.Value.p0;
                                return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 添加数据
                /// </summary>
                /// <param name="buffer">数据</param>
                public AutoCSer.Net.TcpServer.ReturnValue<ulong> append(AutoCSer.DiskBlock.AppendBuffer buffer)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6> _wait_ = _TcpClient_.GetAutoWait<TcpInternalServer._p6>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalServer._p5 _inputParameter_ = new TcpInternalServer._p5
                                {
                                    
                                    p0 = buffer,
                                };
                                TcpInternalServer._p6 _outputParameter_ = new TcpInternalServer._p6
                                {
                                };
                                _socket_.Get<TcpInternalServer._p5, TcpInternalServer._p6>(_c2, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p6> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<ulong> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<ulong> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}
#endif