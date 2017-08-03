//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.SqlTableCacheServer
{
        public partial class Class
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Class _M1(int id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.Get(id);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int[] _M2()
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.getIds();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int[] _M3(int id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.GetStudentIds(id);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 班级表格定义
            /// </summary>
            public partial class Class
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取班级信息
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>班级</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> Get(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                                {
                                    
                                    p0 = id,
                                };
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2
                                {
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2>(_c1, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 获取班级标识集合
                /// </summary>
                /// <returns>班级标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> getIds()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                                {
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>(_c2, _wait_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 1, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生标识集合
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> GetStudentIds(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                                {
                                    
                                    p0 = id,
                                };
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                                {
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>(_c3, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        internal partial class DataReaderTcpVerify
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M4(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify.verify(_sender_, randomPrefix, md5Data, ref ticks);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP 调用验证
            /// </summary>
            public partial class DataReaderTcpVerify
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                                {
                                    
                                    p2 = randomPrefix,
                                    
                                    p0 = md5Data,
                                    
                                    p1 = ticks,
                                };
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5
                                {
                                    
                                    p0 = ticks,
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5>(_c4, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                
                                ticks = _returnOutputParameter_.Value.p0;
                                return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        public partial class Student
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Student _M5(int id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Student.Get(id);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 学生表格定义
            /// </summary>
            public partial class Student
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 1, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生信息
                /// </summary>
                /// <param name="id">学生标识</param>
                /// <returns>学生</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> Get(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                                {
                                    
                                    p0 = id,
                                };
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6
                                {
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>(_c5, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}
namespace AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class DataReader : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public DataReader(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("DataReader", typeof(AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify), true)), verify, onCustomData, log)
            {
                setCommandData(5);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setVerifyCommand(3);
                setCommand(4);
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
                                _s0/**/.Call(sender, AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask, ref inputParameter);
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
                            {
                                _s1/**/.Call(sender, AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask);
                                return;
                            }
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
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _s2/**/.Call(sender, AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask, ref inputParameter);
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
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                _p5 _outputParameter_ = new _p5();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify/**/.TcpStaticServer._M4(sender, inputParameter.p2, inputParameter.p0, ref inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                
                                _outputParameter_.p0 = inputParameter.p1;
                                _outputParameter_.Return = Return;
                                sender.Push(_c4, ref _outputParameter_);
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
                    case 4:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _s4/**/.Call(sender, AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask, ref inputParameter);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticServer.ServerCall<_s0, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.Class Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M1(inputParameter.p0);

                        value.Value.Return = Return;
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                    }
                    catch (Exception error)
                    {
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                        Sender.Log(error);
                    }
                }
                public override void Call()
                {
                    AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c1, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2 };
            sealed class _s1 : AutoCSer.Net.TcpStaticServer.ServerCall<_s1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                {
                    try
                    {
                        
                        int[] Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M2();

                        value.Value.Return = Return;
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                    }
                    catch (Exception error)
                    {
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                        Sender.Log(error);
                    }
                }
                public override void Call()
                {
                    AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c2, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3 };
            sealed class _s2 : AutoCSer.Net.TcpStaticServer.ServerCall<_s2, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p3> value)
                {
                    try
                    {
                        
                        int[] Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M3(inputParameter.p0);

                        value.Value.Return = Return;
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                    }
                    catch (Exception error)
                    {
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                        Sender.Log(error);
                    }
                }
                public override void Call()
                {
                    AutoCSer.Net.TcpServer.ReturnValue<_p3> value = new AutoCSer.Net.TcpServer.ReturnValue<_p3>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c3, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3 };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            sealed class _s4 : AutoCSer.Net.TcpStaticServer.ServerCall<_s4, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.Student Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M5(inputParameter.p0);

                        value.Value.Return = Return;
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                    }
                    catch (Exception error)
                    {
                        value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                        Sender.Log(error);
                    }
                }
                public override void Call()
                {
                    AutoCSer.Net.TcpServer.ReturnValue<_p6> value = new AutoCSer.Net.TcpServer.ReturnValue<_p6>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c5, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.SqlTableCacheServer.Class>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.TestCase.SqlTableCacheServer.Class Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.TestCase.SqlTableCacheServer.Class Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.TestCase.SqlTableCacheServer.Class)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int[]>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public int[] Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public int[] Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (int[])value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
            {
                public byte[] p0;
                public long p1;
                public ulong p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.SqlTableCacheServer.Student>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.TestCase.SqlTableCacheServer.Student Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.TestCase.SqlTableCacheServer.Student Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.TestCase.SqlTableCacheServer.Student)value; }
                }
#endif
            }
        }
}
namespace AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class DataReader
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
                return AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verify(AutoCSer.TestCase.SqlTableCacheServer.TcpCall.DataReaderTcpVerify/**/.verify, sender, TcpClient);
            }
            static DataReader()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("DataReader", typeof(AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 DataReader 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.VerifyMethod);
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        public partial class Class
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M6(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>, bool> _onReturn_)
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class.onSqlLog(_onReturn_);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Class _M7(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.getSqlCache(Id);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 班级表格定义
            /// </summary>
            public partial class Class
            {

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                public static AutoCSer.Net.TcpServer.KeepCallback onSqlLog(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p7>> _onOutput_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetCallback<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p7>(_onReturn_);
                    if (_onReturn_ == null || _onOutput_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p7>(_ac6, _onOutput_);
                                _isWait_ = 1;
                                return _keepCallback_;
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p7> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p7> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                                if (_onOutput_ != null) _onOutput_.Call(ref _outputParameter_);
                            }
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 8, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> getSqlCache(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p8 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p8
                                {
                                    
                                    p0 = Id,
                                };
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9
                                {
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p8, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9>(_c7, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        internal partial class DataLogTcpVerify
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M8(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify.verify(_sender_, randomPrefix, md5Data, ref ticks);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// TCP 调用验证
            /// </summary>
            public partial class DataLogTcpVerify
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 10, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10
                                {
                                    
                                    p2 = randomPrefix,
                                    
                                    p0 = md5Data,
                                    
                                    p1 = ticks,
                                };
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11
                                {
                                    
                                    p0 = ticks,
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11>(_c8, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                
                                ticks = _returnOutputParameter_.Value.p0;
                                return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        public partial class Student
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M9(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>, bool> _onReturn_)
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student.onSqlLog(_onReturn_);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Student _M10(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Student.getSqlCache(Id);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 学生表格定义
            /// </summary>
            public partial class Student
            {

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                public static AutoCSer.Net.TcpServer.KeepCallback onSqlLog(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12>> _onOutput_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetCallback<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12>(_onReturn_);
                    if (_onReturn_ == null || _onOutput_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.KeepCallback _keepCallback_ = _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12>(_ac9, _onOutput_);
                                _isWait_ = 1;
                                return _keepCallback_;
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                                if (_onOutput_ != null) _onOutput_.Call(ref _outputParameter_);
                            }
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 8, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> getSqlCache(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13> _wait_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetAutoWait<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13>();
                    if (_wait_ != null)
                    {
                        int _isWait_ = 0;
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p8 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p8
                                {
                                    
                                    p0 = Id,
                                };
                                AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13
                                {
                                };
                                _socket_.Get<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p8, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13>(_c10, _wait_, ref _inputParameter_, ref _outputParameter_);
                                _isWait_ = 1;
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13> _returnOutputParameter_;
                                _wait_.Get(out _returnOutputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_isWait_ == 0) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13>.PushNotNull(_wait_);
                        }
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}
namespace AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class DataLog : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public DataLog(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("DataLog", typeof(AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify), true)), verify, onCustomData, log)
            {
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
                                _p7 outputParameter = new _p7();
                                Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>, bool> callbackReturn = sender.GetCallback<_p7, AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>(_c6, ref outputParameter);
                                if (callbackReturn != null)
                                {
                                    AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M6(callbackReturn);
                                }
                                return;
                            }
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
                            _p8 inputParameter = new _p8();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p9 _outputParameter_ = new _p9();
                                
                                AutoCSer.TestCase.SqlTableCacheServer.Class Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M7(inputParameter.p0);
                                _outputParameter_.Return = Return;
                                sender.Push(_c7, ref _outputParameter_);
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
                            _p10 inputParameter = new _p10();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                _p11 _outputParameter_ = new _p11();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify/**/.TcpStaticServer._M8(sender, inputParameter.p2, inputParameter.p0, ref inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                
                                _outputParameter_.p0 = inputParameter.p1;
                                _outputParameter_.Return = Return;
                                sender.Push(_c8, ref _outputParameter_);
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
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p12 outputParameter = new _p12();
                                Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>, bool> callbackReturn = sender.GetCallback<_p12, AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>(_c9, ref outputParameter);
                                if (callbackReturn != null)
                                {
                                    AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M9(callbackReturn);
                                }
                                return;
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            sender.Log(error);
                        }
                        sender.Push(returnType);
                        return;
                    case 4:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p8 inputParameter = new _p8();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p13 _outputParameter_ = new _p13();
                                
                                AutoCSer.TestCase.SqlTableCacheServer.Student Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M10(inputParameter.p0);
                                _outputParameter_.Return = Return;
                                sender.Push(_c10, ref _outputParameter_);
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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsKeepCallback = 1 };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9 };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 11, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 12, IsKeepCallback = 1 };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 13 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p7
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.SqlTableCacheServer.Class>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.TestCase.SqlTableCacheServer.Class Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.TestCase.SqlTableCacheServer.Class Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.TestCase.SqlTableCacheServer.Class)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p10
            {
                public byte[] p0;
                public long p1;
                public ulong p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p11
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
            internal struct _p12
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p13
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.TestCase.SqlTableCacheServer.Student>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.TestCase.SqlTableCacheServer.Student Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.TestCase.SqlTableCacheServer.Student Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.TestCase.SqlTableCacheServer.Student)value; }
                }
#endif
            }
        }
}
namespace AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class DataLog
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
                return AutoCSer.Net.TcpInternalServer.TimeVerifyClient.Verify(AutoCSer.TestCase.SqlTableCacheServer.TcpCall.DataLogTcpVerify/**/.verify, sender, TcpClient);
            }
            static DataLog()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("DataLog", typeof(AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 DataLog 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.VerifyMethod);
            }
        }
}
#endif