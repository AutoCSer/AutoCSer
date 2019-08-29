//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Web.SearchServer
{
        public partial struct SearchItem
        {
            /// <summary>
            /// 图片地址
            /// </summary>
            /// <param name="DataKey">搜索数据关键字</param>
            /// <returns>图片地址</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"ImageUrl", IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            private static string getImageUrl(AutoCSer.Web.SearchServer.DataKey DataKey)
            {
                return get(DataKey).ImageUrl;
            }

        }
}namespace AutoCSer.Web.SearchServer
{
        public partial struct SearchItem
        {
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                /// <summary>
                /// 搜索结果项
                /// </summary>
                internal AutoCSer.Web.SearchServer.SearchItem Value;
                /// <summary>
                /// 图片地址
                /// </summary>
                public string ImageUrl
                {
                    get
                    {
                        return TcpCall.SearchItem.getImageUrl(Value.DataKey);
                    }
                }
            }
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            [AutoCSer.BinarySerialize.IgnoreMember]
            [AutoCSer.Json.IgnoreMember]
            public RemoteExtension Remote
            {
                get { return new RemoteExtension { Value = this }; }
            }
        }
}namespace AutoCSer.Web.SearchServer
{
        public partial struct SearchItem
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static string _M3(AutoCSer.Web.SearchServer.DataKey DataKey)
                {

                    
                    return AutoCSer.Web.SearchServer.SearchItem.getImageUrl(DataKey);
                }
            }
        }
}namespace AutoCSer.Web.SearchServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 搜索结果项
            /// </summary>
            public partial struct SearchItem
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 图片地址
                /// </summary>
                /// <param name="DataKey">搜索数据关键字</param>
                /// <returns>图片地址</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<string> getImageUrl(AutoCSer.Web.SearchServer.DataKey DataKey)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Web.SearchServer.TcpStaticClient/**/.SearchServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p5 _inputParameter_ = new AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p5
                            {
                                
                                p0 = DataKey,
                            };
                            
                            AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p6 _outputParameter_ = new AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p5, AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p6>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<string> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.Web.SearchServer
{
        public partial class Server
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M1(string key, Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Web.SearchServer.SearchItem[]>, bool> _onReturn_)
                {
                    AutoCSer.Web.SearchServer.Server.Search(key, _onReturn_);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M2(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {

                    
                    return AutoCSer.Web.SearchServer.Server.verify(_sender_, userID, randomPrefix, md5Data, ref ticks);
                }
            }
        }
}namespace AutoCSer.Web.SearchServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 搜索服务
            /// </summary>
            public partial class Server
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 关键字搜索
                /// </summary>
                /// <param name="key">字搜索</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Web.SearchServer.SearchItem[]> Search(string key)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Web.SearchServer.TcpStaticClient/**/.SearchServer/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p1 _inputParameter_ = new AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p1
                            {
                                
                                p0 = key,
                            };
                            
                            AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p2 _outputParameter_ = new AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p1, AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p2>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Web.SearchServer.SearchItem[]> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Web.SearchServer.SearchItem[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 关键字搜索
                /// </summary>
                /// <param name="key">字搜索</param>
                public static AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.Web.SearchServer.SearchItem[]> SearchAwaiter(string key)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.Web.SearchServer.SearchItem[]> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.Web.SearchServer.SearchItem[]>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Web.SearchServer.TcpStaticClient/**/.SearchServer/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p1 _inputParameter_ = new AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p1
                        {
                            
                            p0 = key,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.Web.SearchServer.SearchItem[]> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.Web.SearchServer.SearchItem[]>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.Web.SearchServer.SearchItem[]>>(_a1, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p3 _inputParameter_ = new AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p3
                            {
                                
                                p2 = userID,
                                
                                p3 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            
                            AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p4 _outputParameter_ = new AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p4
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p3, AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p4>(_c2, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}
namespace AutoCSer.Web.SearchServer.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class SearchServer : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[3];
                names[0].Set(@"AutoCSer.Web.SearchServer.Server(string,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Web.SearchServer.SearchItem[]>,bool>)Search", 0);
                names[1].Set(@"AutoCSer.Net.TcpStaticServer.TimeVerify<AutoCSer.Web.SearchServer.Server>(AutoCSer.Net.TcpInternalServer.ServerSocketSender,string,ulong,byte[],ref long)verify", 1);
                names[2].Set(@"AutoCSer.Web.SearchServer.SearchItem(AutoCSer.Web.SearchServer.DataKey)getImageUrl", 2);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public SearchServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("SearchServer", typeof(AutoCSer.Web.SearchServer.Server), true)), verify, onCustomData, log, false)
            {
                setCommandData(3);
                setCommand(0);
                setVerifyCommand(1);
                setCommand(2);
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
                                _p2 outputParameter = new _p2();
                                AutoCSer.Web.SearchServer.Server/**/.TcpStaticServer._M1(inputParameter.p0, sender.GetCallback<_p2, AutoCSer.Web.SearchServer.SearchItem[]>(_c1, ref outputParameter));
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
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                _p4 _outputParameter_ = new _p4();
                                
                                bool Return;
                                
                                Return =  AutoCSer.Web.SearchServer.Server/**/.TcpStaticServer._M2(sender, inputParameter.p2, inputParameter.p3, inputParameter.p0, ref inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                
                                _outputParameter_.p0 = inputParameter.p1;
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
                    default: return;
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s2 : AutoCSer.Net.TcpStaticServer.ServerCall<_s2, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        string Return;

                        
                        Return = AutoCSer.Web.SearchServer.SearchItem/**/.TcpStaticServer._M3(inputParameter.p0);

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
                        Sender.Push(CommandIndex, _c3, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
            {
                public string p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Web.SearchServer.SearchItem[]>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.Web.SearchServer.SearchItem[] Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.Web.SearchServer.SearchItem[] Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.Web.SearchServer.SearchItem[])value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
            {
                public byte[] p0;
                public long p1;
                public string p2;
                public ulong p3;
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
            {
                public AutoCSer.Web.SearchServer.DataKey p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
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
            static SearchServer()
            {
                CompileSerialize(new System.Type[] { typeof(_p1), null }
                    , new System.Type[] { typeof(_p4), typeof(_p6), null }
                    , new System.Type[] { typeof(_p3), typeof(_p5), null }
                    , new System.Type[] { typeof(_p2), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.Web.SearchServer.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class SearchServer
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
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod = verify;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            private static bool verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender sender)
            {
                return AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verify(AutoCSer.Web.SearchServer.TcpCall.Server/**/.verify, sender, TcpClient);
            }
            static SearchServer()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("SearchServer", typeof(AutoCSer.Web.SearchServer.Server));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 SearchServer 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p1), null }
                    , new System.Type[] { typeof(AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p4), typeof(AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p6), null }
                    , new System.Type[] { typeof(AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p3), typeof(AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p5), null }
                    , new System.Type[] { typeof(AutoCSer.Web.SearchServer.TcpStaticServer/**/.SearchServer/**/._p2), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
#endif