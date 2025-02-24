//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient
{
        internal partial class RefOut
        {
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.RefOut TCP服务参数
            /// </summary>
            public sealed class TcpOpenSimpleServer
            {

                [AutoCSer.BinarySerialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int left;
                    public int product;
                    public int right;
                }
                [AutoCSer.BinarySerialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    public int product;
                    public int right;
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
                public TcpOpenSimpleClient(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, AutoCSer.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = (AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute), "AutoCSer.Example.TcpOpenSimpleServer.RefOut") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "AutoCSer.Example.TcpOpenSimpleServer.RefOut";
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenSimpleServer.Client<TcpOpenSimpleClient>(this, attribute, log);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.JsonDeSerializer.DeSerialize<AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute>(@"{""BinaryDeSerializeMaxArraySize"":1024,""CheckSeconds"":60,""ClientLazyLogSeconds"":60,""ClientRouteType"":null,""ClientSegmentationCopyPath"":null,""ClientSendBufferMaxSize"":1048576,""GenericType"":null,""Host"":""127.0.0.1"",""IsAttribute"":true,""IsAutoClient"":false,""IsAutoServer"":true,""IsBaseTypeAttribute"":false,""IsCompileSerialize"":true,""IsJsonSerialize"":true,""IsMarkData"":false,""IsRememberCommand"":true,""IsRemoteExpression"":true,""IsSegmentation"":true,""IsSimpleSerialize"":true,""MaxInputSize"":16372,""MaxVerifyDataSize"":256,""MemberFilters"":""Instance"",""MinCompressSize"":0,""Name"":null,""Port"":13304,""ReceiveVerifyCommandSeconds"":9,""SendBufferSize"":""Kilobyte8"",""ServerSendBufferMaxSize"":0,""VerifyCount"":4,""VerifyString"":null,""TypeId"":{}}"); }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1, CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize };

                /// <summary>
                /// ref / out 参数测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                /// <param name="product">乘积</param>
                /// <returns>和</returns>
                public AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, ref int right, out int product)
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
                            
                            right = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpOpenSimpleServer._p1, TcpOpenSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        
                        right = _outputParameter_.right;
                        
                        product = _outputParameter_.product;
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    product = default(int);
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