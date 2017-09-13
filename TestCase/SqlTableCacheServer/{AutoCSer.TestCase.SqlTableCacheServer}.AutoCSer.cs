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
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                /// <summary>
                /// 班级表格定义
                /// </summary>
                internal AutoCSer.TestCase.SqlTableCacheServer.Class Value;
                /// <summary>
                /// 当前学生数量
                /// </summary>
                public int StudentCount
                {
                    get
                    {
                        if (Value._IsSqlLogProxyLoaded_) return Value.SqlLogProxyMember/**/.StudentCount;
                        return TcpCall.Class.GetStudentCount(Value.Id);
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
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="Id">班级标识（默认自增）</param>
            /// <returns></returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"StudentCount")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            internal static int GetStudentCount(int Id)
            {
                AutoCSer.TestCase.SqlTableCacheServer.Class value = sqlCache[Id];
                if (!value._IsSqlLogProxyLoaded_) sqlStream.WaitMember(5);
                return value.SqlLogProxyMember/**/.StudentCount;

            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
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
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M4(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.GetStudentCount(Id);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取班级信息
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>班级</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> Get(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2>.Pop();
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
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 获取班级信息
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>班级</returns>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Class> GetAwaiter(int id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Class> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Class>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                        {
                            
                            p0 = id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Class> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Class>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Class>>(_a1, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 获取班级标识集合
                /// </summary>
                /// <returns>班级标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> getIds()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>(_c2, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 获取班级标识集合
                /// </summary>
                /// <returns>班级标识集合</returns>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int[]> getIdsAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int[]> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int[]>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int[]> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int[]>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int[]>>(_a2, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生标识集合
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> GetStudentIds(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>.Pop();
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
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 获取学生标识集合
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int[]> GetStudentIdsAwaiter(int id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int[]> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int[]>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                        {
                            
                            p0 = id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int[]> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int[]>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int[]>>(_a3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 4, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 获取数据
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetStudentCount(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 获取数据
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                public static AutoCSer.Net.TcpServer.AwaiterBox<int> GetStudentCountAwaiter(int Id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                        {
                            
                            p0 = Id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int>>(_a4, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
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
                public static bool _M5(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 6, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6
                            {
                                
                                p2 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7>(_c5, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7>.PushNotNull(_wait_);
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
                public static AutoCSer.TestCase.SqlTableCacheServer.Student _M6(int id)
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生信息
                /// </summary>
                /// <param name="id">学生标识</param>
                /// <returns>学生</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> Get(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                            {
                                
                                p0 = id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 获取学生信息
                /// </summary>
                /// <param name="id">学生标识</param>
                /// <returns>学生</returns>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Student> GetAwaiter(int id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Student> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Student>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                        {
                            
                            p0 = id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Student> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Student>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Student>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
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
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("DataReader", typeof(AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify), true)), verify, onCustomData, log, false)
            {
                setCommandData(6);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setVerifyCommand(4);
                setCommand(5);
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
                                (_s0/**/.Pop() ?? new _s0()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
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
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p4 inputParameter = new _p4();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s3/**/.Pop() ?? new _s3()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p6 inputParameter = new _p6();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                _p7 _outputParameter_ = new _p7();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify/**/.TcpStaticServer._M5(sender, inputParameter.p2, inputParameter.p0, ref inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                
                                _outputParameter_.p0 = inputParameter.p1;
                                _outputParameter_.Return = Return;
                                sender.Push(_c5, ref _outputParameter_);
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
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s5/**/.Pop() ?? new _s5()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                        Sender.AddLog(error);
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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsBuildOutputThread = true };
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
                        Sender.AddLog(error);
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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
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
                        Sender.AddLog(error);
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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 3, IsBuildOutputThread = true };
            sealed class _s3 : AutoCSer.Net.TcpStaticServer.ServerCall<_s3, _p4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p5> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M4(inputParameter.p0);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p5> value = new AutoCSer.Net.TcpServer.ReturnValue<_p5>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c4, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s5 : AutoCSer.Net.TcpStaticServer.ServerCall<_s5, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.Student Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M6(inputParameter.p0);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c6, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8, IsBuildOutputThread = true };

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
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
            {
                public byte[] p0;
                public long p1;
                public ulong p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p7
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
            internal struct _p8
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
                public static void _M7(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>, bool> _onReturn_)
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class.onSqlLog(_onReturn_);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Class _M8(int Id)
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

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                public static AutoCSer.Net.TcpServer.KeepCallback onSqlLog(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9>> _onOutput_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetCallback<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9>(_ac7, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p9> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> getSqlCache(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p11>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Class> getSqlCacheAwaiter(int Id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Class> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Class>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10
                        {
                            
                            p0 = Id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Class> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Class>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Class>>(_a8, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
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
                public static bool _M9(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 12, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12
                            {
                                
                                p2 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p12, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13>(_c9, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13>.PushNotNull(_wait_);
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
                public static void _M10(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>, bool> _onReturn_)
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student.onSqlLog(_onReturn_);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Student _M11(int Id)
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

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                public static AutoCSer.Net.TcpServer.KeepCallback onSqlLog(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14>> _onOutput_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetCallback<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14>(_ac10, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> getSqlCache(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15>(_c11, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Student> getSqlCacheAwaiter(int Id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Student> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Student>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10
                        {
                            
                            p0 = Id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Student> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Student>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p10, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Student>>(_a11, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
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
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("DataLog", typeof(AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify), true)), verify, onCustomData, log, false)
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
                                _p9 outputParameter = new _p9();
                                AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M7(sender.GetCallback<_p9, AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>(_c7, ref outputParameter));
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
                            _p10 inputParameter = new _p10();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p11 _outputParameter_ = new _p11();
                                
                                AutoCSer.TestCase.SqlTableCacheServer.Class Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M8(inputParameter.p0);
                                _outputParameter_.Return = Return;
                                sender.Push(_c8, ref _outputParameter_);
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
                            _p12 inputParameter = new _p12();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                _p13 _outputParameter_ = new _p13();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify/**/.TcpStaticServer._M9(sender, inputParameter.p2, inputParameter.p0, ref inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                
                                _outputParameter_.p0 = inputParameter.p1;
                                _outputParameter_.Return = Return;
                                sender.Push(_c9, ref _outputParameter_);
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
                                _p14 outputParameter = new _p14();
                                AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M10(sender.GetCallback<_p14, AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>(_c10, ref outputParameter));
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
                            _p10 inputParameter = new _p10();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p15 _outputParameter_ = new _p15();
                                
                                AutoCSer.TestCase.SqlTableCacheServer.Student Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M11(inputParameter.p0);
                                _outputParameter_.Return = Return;
                                sender.Push(_c11, ref _outputParameter_);
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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsKeepCallback = 1, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 11, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 13, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 14, IsKeepCallback = 1, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 15, IsBuildOutputThread = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
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
            internal struct _p10
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p11
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
            internal struct _p12
            {
                public byte[] p0;
                public long p1;
                public ulong p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p13
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
            internal struct _p14
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
            internal struct _p15
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