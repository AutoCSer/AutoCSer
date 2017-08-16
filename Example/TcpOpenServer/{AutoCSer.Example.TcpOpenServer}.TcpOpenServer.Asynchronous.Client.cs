//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpOpenServer.TcpClient
{
        internal partial class Asynchronous
        {
            /// <summary>
            /// AutoCSer.Example.TcpOpenServer.Asynchronous TCP服务参数
            /// </summary>
            public sealed class TcpOpenServer
            {

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
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
            public class TcpOpenClient : AutoCSer.Net.TcpOpenServer.MethodClient<TcpOpenClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpOpenClient(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenServer.ServerAttribute>("AutoCSer.Example.TcpOpenServer.Asynchronous") ?? AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenServer.ServerAttribute>(@"{""CheckSeconds"":59,""ClientOutputSleep"":1,""ClientSegmentationCopyPath"":null,""ClientSendBufferMaxSize"":0x100000,""ClientTryCreateSleep"":1000,""CommandPoolBitSize"":0x03,""GenericType"":null,""Host"":""127.0.0.1"",""IsAttribute"":true,""IsAutoClient"":false,""IsAutoServer"":true,""IsBaseTypeAttribute"":false,""IsJsonSerialize"":true,""IsMarkData"":false,""IsSegmentation"":true,""IsServerBuildOutputThread"":false,""IsSimpleSerialize"":true,""MaxCustomDataSize"":0x3FF4,""MaxInputSize"":0x3FF4,""MaxVerifyDataSize"":1024,""MemberFilters"":""Instance"",""MinCompressSize"":0,""Name"":null,""Port"":0x32CF,""ReceiveBufferSize"":""Kilobyte8"",""ReceiveVerifyCommandSeconds"":9,""SendBufferSize"":""Kilobyte8"",""ServerOutputSleep"":1,""ServerSendBufferMaxSize"":0,""VerifyString"":null,""TypeId"":{}}");
                        if (attribute.Name == null) attribute.Name = "AutoCSer.Example.TcpOpenServer.Asynchronous";
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                public AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
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
                public AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
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
                        _returnType_ = _socket_.GetAwaiter<TcpOpenServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_c0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };
                public void Add(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2>> _onOutput_ = _TcpClient_.GetCallback<int, TcpOpenServer._p2>(_onReturn_);
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
                            _socket_.Get<TcpOpenServer._p1, TcpOpenServer._p2>(_ac0, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpOpenServer._p2> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

            }
        }
}
#endif