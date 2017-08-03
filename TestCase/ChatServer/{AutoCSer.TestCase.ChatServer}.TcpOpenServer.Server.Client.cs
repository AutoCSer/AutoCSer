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
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public string userName;
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
                internal struct _p3
                {
                    public string content;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
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
                internal struct _p5
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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public class TcpOpenClient : AutoCSer.Net.TcpOpenServer.MethodClient<TcpOpenClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="verifyMethod">TCP验证方法</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpOpenClient(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Func<TcpOpenClient, AutoCSer.Net.TcpOpenServer.ClientSocketSender, bool> verifyMethod = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenServer.ServerAttribute>("AutoCSer.TestCase.ChatServer.Server") ?? AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenServer.ServerAttribute>(@"{""CheckSeconds"":59,""ClientOutputSleep"":0,""ClientSegmentationCopyPath"":""..\\..\\..\\ChatClient\\"",""ClientSendBufferMaxSize"":0x100000,""ClientTryCreateSleep"":1000,""CommandPoolBitSize"":0x03,""GenericType"":null,""Host"":""127.0.0.1"",""IsAttribute"":true,""IsAutoClient"":true,""IsAutoServer"":true,""IsBaseTypeAttribute"":false,""IsCallQueue"":false,""IsJsonSerialize"":true,""IsMarkData"":false,""IsSegmentation"":true,""IsServerBuildOutputThread"":false,""IsSimpleSerialize"":true,""MaxCustomDataSize"":0x3FF4,""MaxInputSize"":0x3FF4,""MaxVerifyDataSize"":1024,""MemberFilters"":""Instance"",""MinCompressSize"":0,""Name"":null,""Port"":0x3070,""ReceiveBufferSize"":""Kilobyte"",""ReceiveVerifyCommandSeconds"":9,""SendBufferSize"":""Kilobyte"",""ServerOutputSleep"":1,""ServerSendBufferMaxSize"":0,""VerifyString"":null,""TypeId"":{}}");
                        if (attribute.Name == null) attribute.Name = "AutoCSer.TestCase.ChatServer.Server";
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log, verifyMethod);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true };

                /// <summary>
                /// 用户登录
                /// </summary>
                /// <param name="userName">用户名称</param>
                /// <returns>是否成功</returns>
                public AutoCSer.Net.TcpServer.ReturnValue<bool> login(AutoCSer.Net.TcpOpenServer.ClientSocketSender _sender_, string userName)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2> _wait_ = _TcpClient_.GetAutoWait<TcpOpenServer._p2>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _sender_;
                            if (_socket_ != null)
                            {
                                TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                                {
                                    
                                    userName = userName,
                                };
                                TcpOpenServer._p2 _outputParameter_ = new TcpOpenServer._p2
                                {
                                };
                                _socket_.Get<TcpOpenServer._p1, TcpOpenServer._p2>(_c0, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpOpenServer._p2>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 用户退出
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue logout()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = _TcpClient_.GetAutoWait();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                _socket_.Call(_c1, _wait_);
                                _isWait_ = 1;
                                return new AutoCSer.Net.TcpServer.ReturnValue { Type = _wait_.Wait() };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 发送消息
                /// </summary>
                /// <param name="content">消息内容</param>
                public AutoCSer.Net.TcpServer.ReturnValue send(string content)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = _TcpClient_.GetAutoWait();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpOpenServer._p3 _inputParameter_ = new TcpOpenServer._p3
                                {
                                    
                                    content = content,
                                };
                                _socket_.Call(_c2, _wait_, ref _inputParameter_);
                                _isWait_ = 1;
                                return new AutoCSer.Net.TcpServer.ReturnValue { Type = _wait_.Wait() };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsKeepCallback = 1 };
                /// <summary>
                /// 获取用户信息
                /// </summary>
                /// <returns>保持异步回调</returns>
                public AutoCSer.Net.TcpServer.KeepCallback getUser(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.ChatData.UserLogin>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.ChatData.UserLogin, TcpOpenServer._p4>(_onReturn_);
                    if (_onReturn_ == null || _onOutput_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.Get<TcpOpenServer._p4>(_ac3, _onOutput_);
                                _isWait_ = 1;
                                return _keepCallback_;
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                                if (_onOutput_ != null) _onOutput_.Call(ref _outputParameter_);
                            }
                        }
                    }
                    return null;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsKeepCallback = 1 };
                /// <summary>
                /// 获取消息
                /// </summary>
                /// <returns>保持异步回调</returns>
                public AutoCSer.Net.TcpServer.KeepCallback getMessage(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.ChatData.Message>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p5>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.TestCase.ChatData.Message, TcpOpenServer._p5>(_onReturn_);
                    if (_onReturn_ == null || _onOutput_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpOpenServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.Get<TcpOpenServer._p5>(_ac4, _onOutput_);
                                _isWait_ = 1;
                                return _keepCallback_;
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p5> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p5> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                                if (_onOutput_ != null) _onOutput_.Call(ref _outputParameter_);
                            }
                        }
                    }
                    return null;
                }

            }
        }
}
#endif