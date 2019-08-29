//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient
{
        internal partial class Property
        {
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.Property TCP服务参数
            /// </summary>
            public sealed class TcpOpenSimpleServer
            {

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
                    public int index;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int index;
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int value;
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
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute>("AutoCSer.Example.TcpOpenSimpleServer.Property") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "AutoCSer.Example.TcpOpenSimpleServer.Property";
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenSimpleServer.Client<TcpOpenSimpleClient>(this, attribute, log);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute>(@"{""BinaryDeSerializeMaxArraySize"":1024,""CheckSeconds"":59,""ClientRouteType"":null,""ClientSegmentationCopyPath"":null,""ClientSendBufferMaxSize"":1048576,""GenericType"":null,""Host"":""127.0.0.1"",""IsAttribute"":true,""IsAutoClient"":false,""IsAutoServer"":true,""IsBaseTypeAttribute"":false,""IsCompileSerialize"":true,""IsJsonSerialize"":true,""IsMarkData"":false,""IsRemoteExpression"":false,""IsSegmentation"":true,""IsSimpleSerialize"":true,""MaxInputSize"":16372,""MaxVerifyDataSize"":256,""MemberFilters"":""Instance"",""MinCompressSize"":0,""Name"":null,""Port"":13303,""ReceiveVerifyCommandSeconds"":9,""SendBufferSize"":""Kilobyte8"",""ServerSendBufferMaxSize"":0,""VerifyString"":null,""TypeId"":{}}"); }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                /// <summary>
                /// 只读属性支持
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> GetProperty
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
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 2 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                public AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p2 _inputParameter_ = new TcpOpenSimpleServer._p2
                            {
                                
                                index = index,
                            };
                            TcpOpenSimpleServer._p1 _outputParameter_ = default(TcpOpenSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p2, TcpOpenSimpleServer._p1>(_c1, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p3 _inputParameter_ = new TcpOpenSimpleServer._p3
                            {
                                
                                index = index,
                                
                                value = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c2, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 3 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 0 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                /// <summary>
                /// 可写属性支持
                /// </summary>
                public AutoCSer.Net.TcpServer.ReturnValue<int> SetProperty
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpOpenSimpleServer._p1 _outputParameter_ = default(TcpOpenSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1>(_c3, ref _outputParameter_);
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
                                
                                value = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c4, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 4 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };


                static TcpOpenSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p2), typeof(TcpOpenSimpleServer._p3), typeof(TcpOpenSimpleServer._p4), null }
                        , new System.Type[] { typeof(TcpOpenSimpleServer._p1), null });
                }
            }
        }
}
#endif