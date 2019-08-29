//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CacheServer
{
        public partial class MasterServer
#if !NOJIT
             : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalServer.Server, AutoCSer.Net.TcpInternalServer.ServerAttribute>
#endif
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[32];
                names[0].Set(@"(System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.CacheReturnParameter>,bool>)GetCache", 0);
                names[1].Set(@"(AutoCSer.Net.TcpInternalServer.ServerSocketSender,string,ulong,byte[],ref long)verify", 1);
                names[2].Set(@"()GetFileVersion", 2);
                names[3].Set(@"(AutoCSer.CacheServer.OperationParameter.ClientDataStructure)GetOrCreate", 3);
                names[4].Set(@"()NewFileStream", 4);
                names[5].Set(@"(AutoCSer.CacheServer.OperationParameter.OperationNode)Operation", 5);
                names[6].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode)Operation", 6);
                names[7].Set(@"(AutoCSer.CacheServer.OperationParameter.OperationNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)OperationAsynchronous", 7);
                names[8].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)OperationAsynchronous", 8);
                names[9].Set(@"(AutoCSer.CacheServer.OperationParameter.OperationNode)OperationAsynchronousOnly", 9);
                names[10].Set(@"(AutoCSer.CacheServer.OperationParameter.OperationNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)OperationAsynchronousStream", 10);
                names[11].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)OperationAsynchronousStream", 11);
                names[12].Set(@"(AutoCSer.CacheServer.OperationParameter.OperationNode)OperationOnly", 12);
                names[13].Set(@"(AutoCSer.CacheServer.OperationParameter.OperationNode)OperationStream", 13);
                names[14].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode)OperationStream", 14);
                names[15].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode)Query", 15);
                names[16].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode)Query", 16);
                names[17].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronous", 17);
                names[18].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronous", 18);
                names[19].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronousStream", 19);
                names[20].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronousStream", 20);
                names[21].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IdentityReturnParameter>,bool>)QueryKeepCallback", 21);
                names[22].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryKeepCallback", 22);
                names[23].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IdentityReturnParameter>,bool>)QueryKeepCallbackStream", 23);
                names[24].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryKeepCallbackStream", 24);
                names[25].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode)QueryOnly", 25);
                names[26].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode)QueryStream", 26);
                names[27].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode)QueryStream", 27);
                names[28].Set(@"(ulong,long,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReadFileParameter>,bool>)ReadFile", 28);
                names[29].Set(@"(AutoCSer.CacheServer.OperationParameter.RemoveDataStructure)Remove", 29);
                names[30].Set(@"(bool)SetCanWrite", 30);
                names[31].Set(@"()WriteFile", 31);
                return names;
            }
            /// <summary>
            /// MasterCache TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.CacheServer.MasterServer Value;
                /// <summary>
                /// MasterCache TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.CacheServer.MasterServer value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("MasterCache", typeof(AutoCSer.CacheServer.MasterServer))), verify, onCustomData, log, true)
                {
                    Value = value ?? new AutoCSer.CacheServer.MasterServer();
                    setCommandData(32);
                    setCommand(0);
                    setVerifyCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
                    setCommand(9);
                    setCommand(10);
                    setCommand(11);
                    setCommand(12);
                    setCommand(13);
                    setCommand(14);
                    setCommand(15);
                    setCommand(16);
                    setCommand(17);
                    setCommand(18);
                    setCommand(19);
                    setCommand(20);
                    setCommand(21);
                    setCommand(22);
                    setCommand(23);
                    setCommand(24);
                    setCommand(25);
                    setCommand(26);
                    setCommand(27);
                    setCommand(28);
                    setCommand(29);
                    setCommand(30);
                    setCommand(31);
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
                                {
                                    _p1 outputParameter = new _p1();
                                    
                                    Value.GetCache(sender.GetCallback<_p1, AutoCSer.CacheServer.CacheReturnParameter>(_c0, ref outputParameter));
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
                                    _p3 _outputParameter_ = new _p3();
                                    
                                    bool Return;
                                    
                                    Return = Value.verify(sender, inputParameter.p2, inputParameter.p3, inputParameter.p0, ref inputParameter.p1);
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
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p4 _outputParameter_ = new _p4();
                                    
                                    ulong Return;
                                    
                                    Return = Value.GetFileVersion();
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c2, ref _outputParameter_);
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
                                _p5 inputParameter = new _p5();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
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
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue);
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
                        case 5:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s5/**/.Pop() ?? new _s5()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
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
                                _p10 inputParameter = new _p10();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s6/**/.Pop() ?? new _s6()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
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
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.OperationAsynchronous(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c7, ref outputParameter));
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
                                _p10 inputParameter = new _p10();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.OperationAsynchronous(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c8, ref outputParameter));
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
                        case 9:
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s9/**/.Pop() ?? new _s9()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                sender.AddLog(error);
                            }
                            return;
                        case 10:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.OperationAsynchronousStream(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c10, ref outputParameter));
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
                        case 11:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p10 inputParameter = new _p10();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.OperationAsynchronousStream(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c11, ref outputParameter));
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
                        case 12:
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s12/**/.Pop() ?? new _s12()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                sender.AddLog(error);
                            }
                            return;
                        case 13:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s13/**/.Pop() ?? new _s13()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
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
                        case 14:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p10 inputParameter = new _p10();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s14/**/.Pop() ?? new _s14()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
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
                        case 15:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 _outputParameter_ = new _p9();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.Query(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c15, ref _outputParameter_);
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
                        case 16:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p12 inputParameter = new _p12();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 _outputParameter_ = new _p9();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.Query(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c16, ref _outputParameter_);
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
                        case 17:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.QueryAsynchronous(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c17, ref outputParameter));
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
                        case 18:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p12 inputParameter = new _p12();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.QueryAsynchronous(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c18, ref outputParameter));
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
                        case 19:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.QueryAsynchronousStream(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c19, ref outputParameter));
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
                        case 20:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p12 inputParameter = new _p12();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.QueryAsynchronousStream(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c20, ref outputParameter));
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
                        case 21:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p13 outputParameter = new _p13();
                                    
                                    Value.QueryKeepCallback(inputParameter.p0, sender.GetCallback<_p13, AutoCSer.CacheServer.IdentityReturnParameter>(_c21, ref outputParameter));
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
                        case 22:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.QueryKeepCallback(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c22, ref outputParameter));
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
                        case 23:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p13 outputParameter = new _p13();
                                    
                                    Value.QueryKeepCallbackStream(inputParameter.p0, sender.GetCallback<_p13, AutoCSer.CacheServer.IdentityReturnParameter>(_c23, ref outputParameter));
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
                        case 24:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 outputParameter = new _p9();
                                    
                                    Value.QueryKeepCallbackStream(inputParameter.p0, sender.GetCallback<_p9, AutoCSer.CacheServer.ReturnParameter>(_c24, ref outputParameter));
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
                        case 25:
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.QueryOnly(inputParameter.p0);
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                sender.AddLog(error);
                            }
                            return;
                        case 26:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p11 inputParameter = new _p11();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 _outputParameter_ = new _p9();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.QueryStream(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c26, ref _outputParameter_);
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
                        case 27:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p12 inputParameter = new _p12();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p9 _outputParameter_ = new _p9();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.QueryStream(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c27, ref _outputParameter_);
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
                        case 28:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p14 inputParameter = new _p14();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p15 outputParameter = new _p15();
                                    
                                    Value.ReadFile(inputParameter.p1, inputParameter.p0, sender.GetCallback<_p15, AutoCSer.CacheServer.ReadFileParameter>(_c28, ref outputParameter));
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
                        case 29:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p16 inputParameter = new _p16();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    (_s29/**/.Pop() ?? new _s29()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
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
                        case 30:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p17 inputParameter = new _p17();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s30/**/.Pop() ?? new _s30()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Queue, ref inputParameter);
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
                        case 31:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    (_s31/**/.Pop() ?? new _s31()).Set(sender, Value, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                sealed class _s3 : AutoCSer.Net.TcpInternalServer.ServerCall<_s3, AutoCSer.CacheServer.MasterServer, _p5>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                    {
                        try
                        {
                            
                            AutoCSer.CacheServer.IndexIdentity Return;

                            
                            Return = serverValue.GetOrCreate(inputParameter.p0);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
                sealed class _s4 : AutoCSer.Net.TcpInternalServer.ServerCall<_s4, AutoCSer.CacheServer.MasterServer>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                    {
                        try
                        {
                            
                            AutoCSer.CacheServer.ReturnType Return;

                            
                            Return = serverValue.NewFileStream();

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
                            Sender.Push(CommandIndex, _c4, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                sealed class _s5 : AutoCSer.Net.TcpInternalServer.ServerCall<_s5, AutoCSer.CacheServer.MasterServer, _p8>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p9> value)
                    {
                        try
                        {
                            
                            AutoCSer.CacheServer.ReturnParameter Return;

                            
                            Return = serverValue.Operation(inputParameter.p0);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p9> value = new AutoCSer.Net.TcpServer.ReturnValue<_p9>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c5, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                sealed class _s6 : AutoCSer.Net.TcpInternalServer.ServerCall<_s6, AutoCSer.CacheServer.MasterServer, _p10>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p9> value)
                    {
                        try
                        {
                            
                            AutoCSer.CacheServer.ReturnParameter Return;

                            
                            Return = serverValue.Operation(inputParameter.p0);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p9> value = new AutoCSer.Net.TcpServer.ReturnValue<_p9>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c6, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                sealed class _s9 : AutoCSer.Net.TcpInternalServer.ServerCall<_s9, AutoCSer.CacheServer.MasterServer, _p8>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.OperationAsynchronousOnly(inputParameter.p0);

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
                        if (Sender.IsSocket) get(ref value);
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                sealed class _s12 : AutoCSer.Net.TcpInternalServer.ServerCall<_s12, AutoCSer.CacheServer.MasterServer, _p8>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.OperationOnly(inputParameter.p0);

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
                        if (Sender.IsSocket) get(ref value);
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1, IsBuildOutputThread = true };
                sealed class _s13 : AutoCSer.Net.TcpInternalServer.ServerCall<_s13, AutoCSer.CacheServer.MasterServer, _p8>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p9> value)
                    {
                        try
                        {
                            
                            AutoCSer.CacheServer.ReturnParameter Return;

                            
                            Return = serverValue.OperationStream(inputParameter.p0);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p9> value = new AutoCSer.Net.TcpServer.ReturnValue<_p9>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c13, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c13 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                sealed class _s14 : AutoCSer.Net.TcpInternalServer.ServerCall<_s14, AutoCSer.CacheServer.MasterServer, _p10>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p9> value)
                    {
                        try
                        {
                            
                            AutoCSer.CacheServer.ReturnParameter Return;

                            
                            Return = serverValue.OperationStream(inputParameter.p0);

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
                        AutoCSer.Net.TcpServer.ReturnValue<_p9> value = new AutoCSer.Net.TcpServer.ReturnValue<_p9>();
                        if (Sender.IsSocket)
                        {
                            get(ref value);
                            Sender.Push(CommandIndex, _c14, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c14 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c15 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c16 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c17 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c18 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c19 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c20 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c21 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 13, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c22 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c23 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 13, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c24 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c25 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c26 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c27 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c28 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 15, IsKeepCallback = 1, IsBuildOutputThread = true };
                sealed class _s29 : AutoCSer.Net.TcpInternalServer.ServerCall<_s29, AutoCSer.CacheServer.MasterServer, _p16>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                    {
                        try
                        {
                            
                            AutoCSer.CacheServer.IndexIdentity Return;

                            
                            Return = serverValue.Remove(inputParameter.p0);

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
                            Sender.Push(CommandIndex, _c29, ref value);
                        }
                        push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c29 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
                sealed class _s30 : AutoCSer.Net.TcpInternalServer.ServerCall<_s30, AutoCSer.CacheServer.MasterServer, _p17>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.SetCanWrite(inputParameter.p0);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c30 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                sealed class _s31 : AutoCSer.Net.TcpInternalServer.ServerCall<_s31, AutoCSer.CacheServer.MasterServer>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.WriteFile();

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c31 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsBuildOutputThread = true };
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p14), typeof(_p17), null }
                        , new System.Type[] { typeof(_p3), typeof(_p4), typeof(_p7), null }
                        , new System.Type[] { typeof(_p2), typeof(_p5), typeof(_p8), typeof(_p10), typeof(_p11), typeof(_p12), typeof(_p16), null }
                        , new System.Type[] { typeof(_p1), typeof(_p6), typeof(_p9), typeof(_p13), typeof(_p15), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.CacheReturnParameter>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.CacheReturnParameter Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.CacheReturnParameter Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.CacheReturnParameter)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public byte[] p0;
                    public long p1;
                    public string p2;
                    public ulong p3;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
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
                internal struct _p4
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
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p5
                {
                    public AutoCSer.CacheServer.OperationParameter.ClientDataStructure p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.IndexIdentity>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.IndexIdentity Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.IndexIdentity Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.IndexIdentity)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.ReturnType>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.ReturnType Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.ReturnType Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.ReturnType)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p8
                {
                    public AutoCSer.CacheServer.OperationParameter.OperationNode p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p9
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.ReturnParameter>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.ReturnParameter Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.ReturnParameter Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.ReturnParameter)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p10
                {
                    public AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p11
                {
                    public AutoCSer.CacheServer.OperationParameter.QueryNode p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p12
                {
                    public AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p13
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.IdentityReturnParameter>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.IdentityReturnParameter Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.IdentityReturnParameter Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.IdentityReturnParameter)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p14
                {
                    public long p0;
                    public ulong p1;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p15
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.ReadFileParameter>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.ReadFileParameter Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.ReadFileParameter Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.ReadFileParameter)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p16
                {
                    public AutoCSer.CacheServer.OperationParameter.RemoveDataStructure p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p17
                {
                    public bool p0;
                }

            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalClient : AutoCSer.Net.TcpInternalServer.MethodClient<TcpInternalClient>
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
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("MasterCache", typeof(AutoCSer.CacheServer.MasterServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute, verifyMethod ?? (Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool>)_timerVerify_);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsKeepCallback = 1 };
                /// <summary>
                /// 获取缓存数据
                /// </summary>
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback GetCache(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.CacheReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.CacheReturnParameter, TcpInternalServer._p1>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<TcpInternalServer._p1>(_ac0, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p1> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                internal 
                AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p2 _inputParameter_ = new TcpInternalServer._p2
                            {
                                
                                p2 = userID,
                                
                                p3 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            TcpInternalServer._p3 _outputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p2, TcpInternalServer._p3>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 获取物理文件版本
                /// </summary>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<ulong> GetFileVersion()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p4 _outputParameter_ = new TcpInternalServer._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p4>(_c2, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<ulong> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<ulong> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 5, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 添加数据结构定义
                /// </summary>
                /// <param name="parameter">数据结构操作参数</param>
                /// <returns>数据结构索引标识</returns>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> GetOrCreate(AutoCSer.CacheServer.OperationParameter.ClientDataStructure parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p5 _inputParameter_ = new TcpInternalServer._p5
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p6 _outputParameter_ = new TcpInternalServer._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p5, TcpInternalServer._p6>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 重建文件流
                /// </summary>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnType> NewFileStream()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p7 _outputParameter_ = new TcpInternalServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p7>(_c4, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnType> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnType> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 重建文件流
                /// </summary>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnType> NewFileStreamAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnType> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnType>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnType> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnType>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnType>>(_a4, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };
                /// <summary>
                /// 重建文件流
                /// </summary>
                internal 
                void NewFileStream(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnType>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnType, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            _socket_.Get<TcpInternalServer._p7>(_ac4, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> Operation(AutoCSer.CacheServer.OperationParameter.OperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p8, TcpInternalServer._p9>(_c5, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> OperationAwaiter(AutoCSer.CacheServer.OperationParameter.OperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p8, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a5, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void Operation(AutoCSer.CacheServer.OperationParameter.OperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p9>(_ac5, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> Operation(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p10, TcpInternalServer._p9>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> OperationAwaiter(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p10, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void Operation(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p10, TcpInternalServer._p9>(_ac6, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> OperationAsynchronous(AutoCSer.CacheServer.OperationParameter.OperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p8, TcpInternalServer._p9>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> OperationAsynchronousAwaiter(AutoCSer.CacheServer.OperationParameter.OperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p8, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a7, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                internal 
                void OperationAsynchronous(AutoCSer.CacheServer.OperationParameter.OperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p9>(_ac7, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">短路径操作参数</param>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> OperationAsynchronous(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p10, TcpInternalServer._p9>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">短路径操作参数</param>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> OperationAsynchronousAwaiter(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p10, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a8, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">短路径操作参数</param>
                internal 
                void OperationAsynchronous(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p10, TcpInternalServer._p9>(_ac8, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 9 + 128, InputParameterIndex = 8, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 模拟异步操作数据
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                internal void OperationAsynchronousOnly(AutoCSer.CacheServer.OperationParameter.OperationNode parameter)
                {
                    TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                    {
                        
                        p0 = parameter,
                    };
                    _TcpClient_.Sender.CallOnly(_c9, ref _inputParameter_);
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 10 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                internal 
                void OperationAsynchronousStream(AutoCSer.CacheServer.OperationParameter.OperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p9>(_ac10, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 11 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 异步操作数据
                /// </summary>
                /// <param name="parameter">短路径操作参数</param>
                internal 
                void OperationAsynchronousStream(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p10, TcpInternalServer._p9>(_ac11, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 8, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 操作数据
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                internal void OperationOnly(AutoCSer.CacheServer.OperationParameter.OperationNode parameter)
                {
                    TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                    {
                        
                        p0 = parameter,
                    };
                    _TcpClient_.Sender.CallOnly(_c12, ref _inputParameter_);
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 13 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">数据结构定义节点操作参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void OperationStream(AutoCSer.CacheServer.OperationParameter.OperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p9>(_ac13, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 14 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 操作数据并返回参数
                /// </summary>
                /// <param name="parameter">短路径操作参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void OperationStream(AutoCSer.CacheServer.OperationParameter.ShortPathOperationNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p10 _inputParameter_ = new TcpInternalServer._p10
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p10, TcpInternalServer._p9>(_ac14, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 15 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 15 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> Query(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p11, TcpInternalServer._p9>(_c15, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAwaiter(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p11, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a15, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 15 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void Query(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p11, TcpInternalServer._p9>(_ac15, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 16 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 16 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> Query(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p12, TcpInternalServer._p9>(_c16, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                /// <returns>返回参数</returns>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAwaiter(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p12, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a16, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 16 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void Query(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p12, TcpInternalServer._p9>(_ac16, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c17 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 17 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a17 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 17 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p11, TcpInternalServer._p9>(_c17, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronousAwaiter(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p11, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a17, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac17 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 17 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                internal 
                void QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p11, TcpInternalServer._p9>(_ac17, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c18 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 18 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a18 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 18 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p9 _outputParameter_ = new TcpInternalServer._p9
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p12, TcpInternalServer._p9>(_c18, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronousAwaiter(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p12, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a18, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac18 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 18 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                internal 
                void QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p12, TcpInternalServer._p9>(_ac18, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac19 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 19 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                internal 
                void QueryAsynchronousStream(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p11, TcpInternalServer._p9>(_ac19, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac20 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 20 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                internal 
                void QueryAsynchronousStream(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p12, TcpInternalServer._p9>(_ac20, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac21 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 21 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1 };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallback(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IdentityReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p13>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.IdentityReturnParameter, TcpInternalServer._p13>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p11, TcpInternalServer._p13>(_ac21, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p13> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p13> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac22 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 22 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1 };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallback(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p11, TcpInternalServer._p9>(_ac22, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac23 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 23 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsKeepCallback = 1 };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallbackStream(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IdentityReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p13>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.IdentityReturnParameter, TcpInternalServer._p13>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p11, TcpInternalServer._p13>(_ac23, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p13> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p13> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac24 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 24 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsKeepCallback = 1 };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallbackStream(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p11, TcpInternalServer._p9>(_ac24, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c25 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 25 + 128, InputParameterIndex = 11, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                internal void QueryOnly(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                    {
                        
                        p0 = parameter,
                    };
                    _TcpClient_.Sender.CallOnly(_c25, ref _inputParameter_);
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac26 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 26 + 128, InputParameterIndex = 11, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">数据结构定义节点查询参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void QueryStream(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p11 _inputParameter_ = new TcpInternalServer._p11
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p11, TcpInternalServer._p9>(_ac26, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac27 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 27 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                /// <summary>
                /// 表达式节点查询
                /// </summary>
                /// <param name="parameter">短路径查询参数</param>
                /// <param name="_onReturn_">返回参数</param>
                internal 
                void QueryStream(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p12 _inputParameter_ = new TcpInternalServer._p12
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p12, TcpInternalServer._p9>(_ac27, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac28 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 28 + 128, InputParameterIndex = 14, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 读取物理文件
                /// </summary>
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback ReadFile(ulong version, long index, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReadFileParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p15>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReadFileParameter, TcpInternalServer._p15>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p14 _inputParameter_ = new TcpInternalServer._p14
                            {
                                
                                p1 = version,
                                
                                p0 = index,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p14, TcpInternalServer._p15>(_ac28, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p15> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p15> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c29 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 29 + 128, InputParameterIndex = 16, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a29 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 29 + 128, InputParameterIndex = 16, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 删除数据结构定义
                /// </summary>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> Remove(AutoCSer.CacheServer.OperationParameter.RemoveDataStructure parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p16 _inputParameter_ = new TcpInternalServer._p16
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p6 _outputParameter_ = new TcpInternalServer._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p16, TcpInternalServer._p6>(_c29, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 删除数据结构定义
                /// </summary>
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.IndexIdentity> RemoveAwaiter(AutoCSer.CacheServer.OperationParameter.RemoveDataStructure parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.IndexIdentity> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.IndexIdentity>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p16 _inputParameter_ = new TcpInternalServer._p16
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.IndexIdentity> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.IndexIdentity>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p16, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.IndexIdentity>>(_a29, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac29 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 29 + 128, InputParameterIndex = 16, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 删除数据结构定义
                /// </summary>
                internal 
                void Remove(AutoCSer.CacheServer.OperationParameter.RemoveDataStructure parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p6>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.IndexIdentity, TcpInternalServer._p6>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p16 _inputParameter_ = new TcpInternalServer._p16
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p16, TcpInternalServer._p6>(_ac29, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p6> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p6> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c30 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 30 + 128, InputParameterIndex = 17, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a30 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 30 + 128, InputParameterIndex = 17, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 设置是否允许写操作
                /// </summary>
                /// <param name="canWrite">是否允许写操作</param>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue SetCanWrite(bool canWrite)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p17 _inputParameter_ = new TcpInternalServer._p17
                            {
                                
                                p0 = canWrite,
                            };
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c30, ref _wait_, ref _inputParameter_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 设置是否允许写操作
                /// </summary>
                /// <param name="canWrite">是否允许写操作</param>
                internal 
                AutoCSer.Net.TcpServer.Awaiter SetCanWriteAwaiter(bool canWrite)
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p17 _inputParameter_ = new TcpInternalServer._p17
                        {
                            
                            p0 = canWrite,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a30, _awaiter_, ref _inputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac30 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 30 + 128, InputParameterIndex = 17, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };
                /// <summary>
                /// 设置是否允许写操作
                /// </summary>
                /// <param name="canWrite">是否允许写操作</param>
                internal 
                void SetCanWrite(bool canWrite, Action<AutoCSer.Net.TcpServer.ReturnValue> _onReturn_)
                {
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p17 _inputParameter_ = new TcpInternalServer._p17
                            {
                                
                                p0 = canWrite,
                            };
                            _socket_.Call(_ac30, _onReturn_, ref _inputParameter_);
                            _onReturn_ = null;
                        }
                    }
                    finally
                    {
                        if (_onReturn_ != null) _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException });
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c31 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 31 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a31 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 31 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 数据立即写入文件
                /// </summary>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue WriteFile()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return new AutoCSer.Net.TcpServer.ReturnValue { Type = _socket_.WaitCall(_c31, ref _wait_) };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 数据立即写入文件
                /// </summary>
                internal 
                AutoCSer.Net.TcpServer.Awaiter WriteFileAwaiter()
                {
                    AutoCSer.Net.TcpServer.Awaiter _awaiter_ = new AutoCSer.Net.TcpServer.Awaiter();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        _returnType_ = _socket_.GetAwaiter(_a31, _awaiter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac31 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 31 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                /// <summary>
                /// 数据立即写入文件
                /// </summary>
                internal 
                void WriteFile(Action<AutoCSer.Net.TcpServer.ReturnValue> _onReturn_)
                {
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            _socket_.Call(_ac31, _onReturn_);
                            _onReturn_ = null;
                        }
                    }
                    finally
                    {
                        if (_onReturn_ != null) _onReturn_(new AutoCSer.Net.TcpServer.ReturnValue { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException });
                    }
                }

                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalServer._p14), typeof(TcpInternalServer._p17), null }
                        , new System.Type[] { typeof(TcpInternalServer._p3), typeof(TcpInternalServer._p4), typeof(TcpInternalServer._p7), null }
                        , new System.Type[] { typeof(TcpInternalServer._p2), typeof(TcpInternalServer._p5), typeof(TcpInternalServer._p8), typeof(TcpInternalServer._p10), typeof(TcpInternalServer._p11), typeof(TcpInternalServer._p12), typeof(TcpInternalServer._p16), null }
                        , new System.Type[] { typeof(TcpInternalServer._p1), typeof(TcpInternalServer._p6), typeof(TcpInternalServer._p9), typeof(TcpInternalServer._p13), typeof(TcpInternalServer._p15), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.CacheServer
{
        public partial class SlaveServer
#if !NOJIT
             : AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalServer.Server, AutoCSer.Net.TcpInternalServer.ServerAttribute>
#endif
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[14];
                names[0].Set(@"(AutoCSer.CacheServer.OperationParameter.ClientDataStructure)Get", 0);
                names[1].Set(@"(AutoCSer.Net.TcpInternalServer.ServerSocketSender,string,ulong,byte[],ref long)verify", 1);
                names[2].Set(@"(System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.CacheReturnParameter>,bool>)GetCache", 2);
                names[3].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode)Query", 3);
                names[4].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode)Query", 4);
                names[5].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronous", 5);
                names[6].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronous", 6);
                names[7].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronousStream", 7);
                names[8].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryAsynchronousStream", 8);
                names[9].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryKeepCallback", 9);
                names[10].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode,System.Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>,bool>)QueryKeepCallbackStream", 10);
                names[11].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode)QueryOnly", 11);
                names[12].Set(@"(AutoCSer.CacheServer.OperationParameter.QueryNode)QueryStream", 12);
                names[13].Set(@"(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode)QueryStream", 13);
                return names;
            }
            /// <summary>
            /// SlaveCache TCP服务
            /// </summary>
            public sealed class TcpInternalServer : AutoCSer.Net.TcpInternalServer.Server
            {
                public readonly AutoCSer.CacheServer.SlaveServer Value;
                /// <summary>
                /// SlaveCache TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="value">TCP 服务目标对象</param>
                /// <param name="log">日志接口</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                public TcpInternalServer(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.CacheServer.SlaveServer value = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("SlaveCache", typeof(AutoCSer.CacheServer.SlaveServer))), verify, onCustomData, log, false)
                {
                    Value = value ?? new AutoCSer.CacheServer.SlaveServer();
                    setCommandData(14);
                    setCommand(0);
                    setVerifyCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    setCommand(5);
                    setCommand(6);
                    setCommand(7);
                    setCommand(8);
                    setCommand(9);
                    setCommand(10);
                    setCommand(11);
                    setCommand(12);
                    setCommand(13);
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
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    AutoCSer.CacheServer.IndexIdentity Return;
                                    
                                    Return = Value.Get(inputParameter.p0);
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p4 _outputParameter_ = new _p4();
                                    
                                    bool Return;
                                    
                                    Return = Value.verify(sender, inputParameter.p2, inputParameter.p3, inputParameter.p0, ref inputParameter.p1);
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
                                sender.AddLog(error);
                            }
                            sender.Push(returnType);
                            return;
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p5 outputParameter = new _p5();
                                    
                                    Value.GetCache(sender.GetCallback<_p5, AutoCSer.CacheServer.CacheReturnParameter>(_c2, ref outputParameter));
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
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 _outputParameter_ = new _p7();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.Query(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c3, ref _outputParameter_);
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
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 _outputParameter_ = new _p7();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.Query(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c4, ref _outputParameter_);
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
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 outputParameter = new _p7();
                                    
                                    Value.QueryAsynchronous(inputParameter.p0, sender.GetCallback<_p7, AutoCSer.CacheServer.ReturnParameter>(_c5, ref outputParameter));
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
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 outputParameter = new _p7();
                                    
                                    Value.QueryAsynchronous(inputParameter.p0, sender.GetCallback<_p7, AutoCSer.CacheServer.ReturnParameter>(_c6, ref outputParameter));
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
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 outputParameter = new _p7();
                                    
                                    Value.QueryAsynchronousStream(inputParameter.p0, sender.GetCallback<_p7, AutoCSer.CacheServer.ReturnParameter>(_c7, ref outputParameter));
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
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 outputParameter = new _p7();
                                    
                                    Value.QueryAsynchronousStream(inputParameter.p0, sender.GetCallback<_p7, AutoCSer.CacheServer.ReturnParameter>(_c8, ref outputParameter));
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
                        case 9:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 outputParameter = new _p7();
                                    
                                    Value.QueryKeepCallback(inputParameter.p0, sender.GetCallback<_p7, AutoCSer.CacheServer.ReturnParameter>(_c9, ref outputParameter));
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
                        case 10:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 outputParameter = new _p7();
                                    
                                    Value.QueryKeepCallbackStream(inputParameter.p0, sender.GetCallback<_p7, AutoCSer.CacheServer.ReturnParameter>(_c10, ref outputParameter));
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
                        case 11:
                            try
                            {
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.QueryOnly(inputParameter.p0);
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                sender.AddLog(error);
                            }
                            return;
                        case 12:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p6 inputParameter = new _p6();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 _outputParameter_ = new _p7();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.QueryStream(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c12, ref _outputParameter_);
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
                        case 13:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p8 inputParameter = new _p8();
                                if (sender.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p7 _outputParameter_ = new _p7();
                                    
                                    AutoCSer.CacheServer.ReturnParameter Return;
                                    
                                    Return = Value.QueryStream(inputParameter.p0);
                                    _outputParameter_.Return = Return;
                                    sender.Push(_c13, ref _outputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsKeepCallback = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c13 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
                static TcpInternalServer()
                {
                    CompileSerialize(new System.Type[] { null }
                        , new System.Type[] { typeof(_p4), null }
                        , new System.Type[] { typeof(_p1), typeof(_p3), typeof(_p6), typeof(_p8), null }
                        , new System.Type[] { typeof(_p2), typeof(_p5), typeof(_p7), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public AutoCSer.CacheServer.OperationParameter.ClientDataStructure p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.IndexIdentity>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.IndexIdentity Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.IndexIdentity Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.IndexIdentity)value; }
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
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.CacheReturnParameter>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.CacheReturnParameter Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.CacheReturnParameter Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.CacheReturnParameter)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p6
                {
                    public AutoCSer.CacheServer.OperationParameter.QueryNode p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.CacheServer.ReturnParameter>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.CacheServer.ReturnParameter Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.CacheServer.ReturnParameter Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.CacheServer.ReturnParameter)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p8
                {
                    public AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode p0;
                }

            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalClient : AutoCSer.Net.TcpInternalServer.MethodClient<TcpInternalClient>
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
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="onCustomData">自定义数据包处理</param>
                /// <param name="log">日志接口</param>
                public TcpInternalClient(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalServer.ServerAttribute.GetConfig("SlaveCache", typeof(AutoCSer.CacheServer.SlaveServer));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalServer.Client<TcpInternalClient>(this, attribute, onCustomData, log, clientRoute, verifyMethod ?? (Func<TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool>)_timerVerify_);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 获取数据结构索引标识
                /// </summary>
                /// <param name="parameter">数据结构操作参数</param>
                /// <returns>数据结构索引标识</returns>
                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> Get(AutoCSer.CacheServer.OperationParameter.ClientDataStructure parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p1 _inputParameter_ = new TcpInternalServer._p1
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p2 _outputParameter_ = new TcpInternalServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p1, TcpInternalServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                internal 
                AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p3 _inputParameter_ = new TcpInternalServer._p3
                            {
                                
                                p2 = userID,
                                
                                p3 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            TcpInternalServer._p4 _outputParameter_ = new TcpInternalServer._p4
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p3, TcpInternalServer._p4>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsKeepCallback = 1 };
                /// <summary>
                /// 获取缓存数据
                /// </summary>
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback GetCache(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.CacheReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p5>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.CacheReturnParameter, TcpInternalServer._p5>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<TcpInternalServer._p5>(_ac2, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p5> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p5> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> Query(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p7 _outputParameter_ = new TcpInternalServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p6, TcpInternalServer._p7>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAwaiter(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p6, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                internal 
                void Query(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p6, TcpInternalServer._p7>(_ac3, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> Query(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p7 _outputParameter_ = new TcpInternalServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p8, TcpInternalServer._p7>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAwaiter(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p8, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a4, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                internal 
                void Query(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p7>(_ac4, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p7 _outputParameter_ = new TcpInternalServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p6, TcpInternalServer._p7>(_c5, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronousAwaiter(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p6, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a5, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                internal 
                void QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p6, TcpInternalServer._p7>(_ac5, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                internal 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            TcpInternalServer._p7 _outputParameter_ = new TcpInternalServer._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalServer._p8, TcpInternalServer._p7>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalServer._p7>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                internal 
                AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> QueryAsynchronousAwaiter(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.CacheServer.ReturnParameter>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                        {
                            
                            p0 = parameter,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalServer._p8, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.CacheServer.ReturnParameter>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };
                internal 
                void QueryAsynchronous(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p7>(_ac6, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                internal 
                void QueryAsynchronousStream(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p6, TcpInternalServer._p7>(_ac7, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                internal 
                void QueryAsynchronousStream(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p7>(_ac8, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 9 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallback(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p6, TcpInternalServer._p7>(_ac9, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 10 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                internal 
                AutoCSer.Net.TcpServer.KeepCallback QueryKeepCallbackStream(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            return _socket_.GetKeep<TcpInternalServer._p6, TcpInternalServer._p7>(_ac10, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 11 + 128, InputParameterIndex = 6, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                internal void QueryOnly(AutoCSer.CacheServer.OperationParameter.QueryNode parameter)
                {
                    TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                    {
                        
                        p0 = parameter,
                    };
                    _TcpClient_.Sender.CallOnly(_c11, ref _inputParameter_);
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                internal 
                void QueryStream(AutoCSer.CacheServer.OperationParameter.QueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p6 _inputParameter_ = new TcpInternalServer._p6
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p6, TcpInternalServer._p7>(_ac12, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 13 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                internal 
                void QueryStream(AutoCSer.CacheServer.OperationParameter.ShortPathQueryNode parameter, Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7>> _onOutput_ = _TcpClient_.GetCallback<AutoCSer.CacheServer.ReturnParameter, TcpInternalServer._p7>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalServer._p8 _inputParameter_ = new TcpInternalServer._p8
                            {
                                
                                p0 = parameter,
                            };
                            _socket_.Get<TcpInternalServer._p8, TcpInternalServer._p7>(_ac13, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalServer._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                static TcpInternalClient()
                {
                    _compileSerialize_(new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalServer._p4), null }
                        , new System.Type[] { typeof(TcpInternalServer._p1), typeof(TcpInternalServer._p3), typeof(TcpInternalServer._p6), typeof(TcpInternalServer._p8), null }
                        , new System.Type[] { typeof(TcpInternalServer._p2), typeof(TcpInternalServer._p5), typeof(TcpInternalServer._p7), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}
#endif