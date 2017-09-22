//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.ChatServer
{
        public partial class Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[5];
                names[0].Set(@"(AutoCSer.Net.TcpOpenServer.ServerSocketSender,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.ChatData.Message>,bool>)getMessage", 0);
                names[1].Set(@"(AutoCSer.Net.TcpOpenServer.ServerSocketSender,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.ChatData.UserLogin>,bool>)getUser", 1);
                names[2].Set(@"(AutoCSer.Net.TcpOpenServer.ServerSocketSender,string)login", 2);
                names[3].Set(@"(AutoCSer.Net.TcpOpenServer.ServerSocketSender)logout", 3);
                names[4].Set(@"(AutoCSer.Net.TcpOpenServer.ServerSocketSender,string)send", 4);
                return names;
            }
            /// <summary>
            /// AutoCSer.TestCase.ChatServer.Server TCP服务
            /// </summary>
            public sealed class TcpOpenServer : AutoCSer.Net.TcpOpenServer.Server
            {
                public readonly AutoCSer.TestCase.ChatServer.Server Value;
                /// <summary>
                /// AutoCSer.TestCase.ChatServer.Server TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="value">TCP服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpOpenServer(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.TestCase.ChatServer.Server value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.TestCase.ChatServer.Server", typeof(AutoCSer.TestCase.ChatServer.Server))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.TestCase.ChatServer.Server();
                    setCommandData(5);
                    setCommand(0);
                    setCommand(1);
                    setVerifyCommand(2);
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
                                    _p1 outputParameter = new _p1();
                                    
                                    Value.getMessage(sender,  sender.GetCallback<_p1, AutoCSer.TestCase.ChatData.Message>(_c0, ref outputParameter));
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
                                {
                                    _p2 outputParameter = new _p2();
                                    
                                    Value.getUser(sender,  sender.GetCallback<_p2, AutoCSer.TestCase.ChatData.UserLogin>(_c1, ref outputParameter));
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p4 _outputParameter_ = new _p4();
                                    
                                    bool Return;
                                    
                                    Return = Value.login(sender, inputParameter.userName);
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
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue);
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
                                _p5 inputParameter = new _p5();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, ref inputParameter);
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsBuildOutputThread = true };
                sealed class _s3 : AutoCSer.Net.TcpOpenServer.ServerCall<_s3, AutoCSer.TestCase.ChatServer.Server>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.logout(Sender);

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
                sealed class _s4 : AutoCSer.Net.TcpOpenServer.ServerCall<_s4, AutoCSer.TestCase.ChatServer.Server, _p5>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.send(Sender, inputParameter.content);

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
                static TcpOpenServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p3), typeof(_p5), null }
                        , new System.Type[] { typeof(_p1), typeof(_p2), typeof(_p4), null });
                }

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
        }
}
#endif