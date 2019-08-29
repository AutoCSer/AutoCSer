//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpOpenServer.TcpClient
{
        internal partial class SendOnly
        {
            /// <summary>
            /// AutoCSer.Example.TcpOpenServer.SendOnly TCP服务参数
            /// </summary>
            public sealed class TcpOpenServer
            {

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int left;
                    public int right;
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
                        attribute = AutoCSer.Config.Loader.Get<AutoCSer.Net.TcpOpenServer.ServerAttribute>("AutoCSer.Example.TcpOpenServer.SendOnly") ?? _DefaultServerAttribute_;
                        if (attribute.Name == null) attribute.Name = "AutoCSer.Example.TcpOpenServer.SendOnly";
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpOpenServer.Client<TcpOpenClient>(this, attribute, onCustomData, log, clientRoute);
                    if (attribute.IsAutoClient) _TcpClient_.TryCreateSocket();
                }
                /// <summary>
                /// 默认 TCP 调用服务器端配置信息
                /// </summary>
                public static AutoCSer.Net.TcpOpenServer.ServerAttribute _DefaultServerAttribute_
                {
                    get { return AutoCSer.Json.Parser.Parse<AutoCSer.Net.TcpOpenServer.ServerAttribute>(@"{""BinaryDeSerializeMaxArraySize"":1024,""CheckSeconds"":59,""ClientFirstTryCreateSleep"":1000,""ClientOutputSleep"":-1,""ClientRouteType"":null,""ClientSegmentationCopyPath"":null,""ClientSendBufferMaxSize"":1048576,""ClientTryCreateSleep"":1000,""CommandPoolBitSize"":3,""GenericType"":null,""Host"":""127.0.0.1"",""IsAttribute"":true,""IsAutoClient"":false,""IsAutoServer"":true,""IsBaseTypeAttribute"":false,""IsClientAwaiter"":false,""IsCompileSerialize"":true,""IsJsonSerialize"":true,""IsMarkData"":false,""IsRemoteExpression"":false,""IsSegmentation"":true,""IsServerBuildOutputThread"":false,""IsSimpleSerialize"":true,""MaxCustomDataSize"":16372,""MaxInputSize"":16372,""MaxVerifyDataSize"":256,""MemberFilters"":""Instance"",""MinCompressSize"":0,""Name"":null,""Port"":13006,""ReceiveBufferSize"":""Kilobyte8"",""ReceiveVerifyCommandSeconds"":9,""RemoteExpressionServerTask"":""Timeout"",""SendBufferSize"":""Kilobyte8"",""ServerOutputSleep"":-1,""ServerSendBufferMaxSize"":0,""VerifyString"":null,""TypeId"":{}}"); }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1 , CommandFlags = AutoCSer.Net.TcpServer.CommandFlags.JsonSerialize, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 仅发送请求测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void SetSum(int left, int right)
                {
                    TcpOpenServer._p1 _inputParameter_ = new TcpOpenServer._p1
                    {
                        
                        left = left,
                        
                        right = right,
                    };
                    _TcpClient_.Sender.CallOnly(_c0, ref _inputParameter_);
                }

                static TcpOpenClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpOpenServer._p1), null }
                        , new System.Type[] { null });
                }
            }
        }
}
#endif