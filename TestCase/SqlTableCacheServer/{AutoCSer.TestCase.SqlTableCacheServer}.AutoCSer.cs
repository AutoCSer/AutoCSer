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
            /// 当前学生数量
            /// </summary>
            /// <param name="Id">班级标识（默认自增）</param>
            /// <returns></returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"StudentCount", IsClientRemoteMember = false, IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            internal static int GetStudentCount(int Id)
            {
                AutoCSer.TestCase.SqlTableCacheServer.Class value = getSqlCache(Id);
                if (!value._IsSqlLogProxyLoaded_) sqlStream.WaitMember(4);
                return value.SqlLogProxyMember/**/.StudentCount;
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
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
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        public partial class Class
        {
            /// <summary>
            /// 获取学生标识集合（远程实例成员，推荐模式）
            /// </summary>
            /// <param name="Id">班级标识（默认自增）</param>
            /// <returns>获取学生标识集合（远程实例成员，推荐模式）</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"StudentIds", IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            private static int[] getStudentIds(int Id)
            {
                return getSqlCache(Id).StudentIds;
            }
            /// <summary>
            /// 获取学生标识（远程实例成员，推荐模式）
            /// </summary>
            /// <param name="Id">班级标识（默认自增）</param>
            /// <param name="index">学生索引</param>
            /// <returns>学生标识集合</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"GetStudentId", IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            private static int remote_GetStudentId(int Id, int index)
            {
                
                return getSqlCache(Id).GetStudentId(index);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Id">班级标识（默认自增）</param>
            /// <returns></returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"ExtensionItem", IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            private static int get_Extension_Item(int Id, int index)
            {
                AutoCSer.TestCase.SqlTableCacheServer.Class _value_ = getSqlCache(Id);
                AutoCSer.TestCase.SqlTableCacheServer.Class.MemberCache _value0_ = _value_/**/.Extension;
                    return _value0_[index];
            }
            /// <summary>
            /// 获取学生标识集合（远程缓存成员）
            /// </summary>
            /// <param name="Id">班级标识（默认自增）</param>
            /// <returns>获取学生标识集合（远程缓存成员）</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"ExtensionStudentIds", IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            private static int[] get_Extension_StudentIds(int Id)
            {
                AutoCSer.TestCase.SqlTableCacheServer.Class _value_ = getSqlCache(Id);
                AutoCSer.TestCase.SqlTableCacheServer.Class.MemberCache _value0_ = _value_/**/.Extension;
                    return _value0_.StudentIds;
            }
            /// <summary>
            /// 获取学生标识（远程缓存成员）
            /// </summary>
            /// <param name="Id">班级标识（默认自增）</param>
            /// <param name="index">学生索引</param>
            /// <returns>学生标识</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"ExtensionGetStudentId", IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            private static int remote_Extension_GetStudentId(int Id, int index)
            {
                AutoCSer.TestCase.SqlTableCacheServer.Class _value_ = getSqlCache(Id);
                AutoCSer.TestCase.SqlTableCacheServer.Class.MemberCache _value0_ = _value_/**/.Extension;
                    
                    return _value0_/**/.GetStudentId(index);
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        public partial class Class
        {
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                /// <summary>
                /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
                /// </summary>
                /// <param name="index">学生索引</param>
                public int GetStaticStudentId(int index)
                {
                    
                    return  TcpCall.Class.GetStaticStudentId(Value.Id, index);
                }
                /// <summary>
                /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
                /// </summary>
                public int[] GetStaticStudentIds()
                {
                    
                    return  TcpCall.Class.GetStaticStudentIds(Value.Id);
                }
                /// <summary>
                /// 获取学生标识集合（远程实例成员，推荐模式）
                /// </summary>
                public int[] StudentIds
                {
                    get
                    {
                        return TcpCall.Class.getStudentIds(Value.Id);
                    }
                }
                /// <summary>
                /// 
                /// </summary>
                public int ExtensionItem(int index)
                {
                    
                    return  TcpCall.Class.get_Extension_Item(Value.Id, index);
                }
                /// <summary>
                /// 获取学生标识集合（远程缓存成员）
                /// </summary>
                public int[] ExtensionStudentIds
                {
                    get
                    {
                        return TcpCall.Class.get_Extension_StudentIds(Value.Id);
                    }
                }
                /// <summary>
                /// 获取学生标识（远程缓存成员）
                /// </summary>
                /// <param name="index">学生索引</param>
                public int ExtensionGetStudentId(int index)
                {
                    
                    return  TcpCall.Class.remote_Extension_GetStudentId(Value.Id, index);
                }
                /// <summary>
                /// 获取学生标识（远程实例成员，推荐模式）
                /// </summary>
                /// <param name="index">学生索引</param>
                public int GetStudentId(int index)
                {
                    
                    return  TcpCall.Class.remote_GetStudentId(Value.Id, index);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType
{
        public partial struct ClassStudent
        {
            /// <summary>
            /// 获取学生信息
            /// </summary>
            /// <param name="value">远程调用连类型映射测试</param>
            /// <returns>获取学生信息</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"Student", IsAwait = false)]
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(IsClientAwaiter = false)]
            private static AutoCSer.TestCase.SqlTableCacheServer.Student getStudent(AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType.ClassStudent value)
            {
                return value.Student;
            }

        }
}namespace AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType
{
        public partial struct ClassStudent
        {
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                /// <summary>
                /// 远程调用连类型映射测试
                /// </summary>
                internal AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType.ClassStudent Value;
                /// <summary>
                /// 获取学生信息
                /// </summary>
                public AutoCSer.TestCase.SqlTableCacheServer.Student Student
                {
                    get
                    {
                        return TcpCall.ClassStudent.getStudent(Value);
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
                public static int _M2(int id, int index)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.GetStaticStudentId(id, index);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int[] _M3(int id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.GetStaticStudentIds(id);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M4(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.GetStudentCount(Id);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int[] _M5()
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.getIds();
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int[] _M6(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.getStudentIds(Id);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M7(int Id, int index)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.get_Extension_Item(Id, index);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int[] _M8(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.get_Extension_StudentIds(Id);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M9(int Id, int index)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.remote_Extension_GetStudentId(Id, index);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M10(int Id, int index)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.remote_GetStudentId(Id, index);
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

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <param name="index">学生索引</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetStaticStudentId(int id, int index)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                            {
                                
                                p0 = id,
                                
                                p1 = index,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>(_c2, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <param name="index">学生索引</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.AwaiterBox<int> GetStaticStudentIdAwaiter(int id, int index)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                        {
                            
                            p0 = id,
                            
                            p1 = index,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int>>(_a2, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 5, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> GetStaticStudentIds(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5
                            {
                                
                                p0 = id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
                /// </summary>
                /// <param name="id">班级标识</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.AwaiterBox<int[]> GetStaticStudentIdsAwaiter(int id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<int[]> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<int[]>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5
                        {
                            
                            p0 = id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int[]> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int[]>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<int[]>>(_a3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 5, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 当前学生数量
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetStudentCount(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout };

                /// <summary>
                /// 获取班级标识集合
                /// </summary>
                /// <returns>班级标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> getIds()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7>(_c5, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7>.PushNotNull(_wait_);
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
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int[]>>(_a5, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 5, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生标识集合（远程实例成员，推荐模式）
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                /// <returns>获取学生标识集合（远程实例成员，推荐模式）</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> getStudentIds(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <param name="Id">班级标识（默认自增）</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> get_Extension_Item(int Id, int index)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                            {
                                
                                p0 = Id,
                                
                                p1 = index,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>(_c7, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 5, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生标识集合（远程缓存成员）
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                /// <returns>获取学生标识集合（远程缓存成员）</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int[]> get_Extension_StudentIds(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 获取学生标识（远程缓存成员）
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                /// <param name="index">学生索引</param>
                /// <returns>学生标识</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> remote_Extension_GetStudentId(int Id, int index)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                            {
                                
                                p0 = Id,
                                
                                p1 = index,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>(_c9, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 9 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 获取学生标识（远程实例成员，推荐模式）
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                /// <param name="index">学生索引</param>
                /// <returns>学生标识集合</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> remote_GetStudentId(int Id, int index)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3
                            {
                                
                                p0 = Id,
                                
                                p1 = index,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>(_c10, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
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
                public static bool _M11(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify.verify(_sender_, userID, randomPrefix, md5Data, ref ticks);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 10 + 128, InputParameterIndex = 8, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p9> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p9>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8
                            {
                                
                                p2 = userID,
                                
                                p3 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p9 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p9
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p9>(_c11, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p9>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType
{
        public partial struct ClassStudent
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Student _M12(AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType.ClassStudent value)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType.ClassStudent.getStudent(value);
                }
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 远程调用连类型映射测试
            /// </summary>
            public partial struct ClassStudent
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 11 + 128, InputParameterIndex = 10, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous };

                /// <summary>
                /// 获取学生信息
                /// </summary>
                /// <param name="value">远程调用连类型映射测试</param>
                /// <returns>获取学生信息</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> getStudent(AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType.ClassStudent value)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p11> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p11>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p10 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p10
                            {
                                
                                p0 = value,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p11 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p11
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p10, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p11>(_c12, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p11>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
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
                public static AutoCSer.TestCase.SqlTableCacheServer.Student _M13(int id)
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 获取学生信息
                /// </summary>
                /// <param name="id">学生标识</param>
                /// <returns>学生</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> Get(int id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p12> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p12>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataReader/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1
                            {
                                
                                p0 = id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p12 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p12
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p12>(_c13, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p12>.PushNotNull(_wait_);
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
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<AutoCSer.TestCase.SqlTableCacheServer.Student>>(_a13, _awaiter_, ref _inputParameter_, ref _outputParameter_);
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
                setCommandData(13);
                setCommand(0);
                setCommand(1);
                setCommand(2);
                setCommand(3);
                setCommand(4);
                setCommand(5);
                setCommand(6);
                setCommand(7);
                setCommand(8);
                setCommand(9);
                setVerifyCommand(10);
                setCommand(11);
                setCommand(12);
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
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
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
                            {
                                (_s4/**/.Pop() ?? new _s4()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                            _p5 inputParameter = new _p5();
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
                    case 6:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s6/**/.Pop() ?? new _s6()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s7/**/.Pop() ?? new _s7()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s8/**/.Pop() ?? new _s8()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s9/**/.Pop() ?? new _s9()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                            _p8 inputParameter = new _p8();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                _p9 _outputParameter_ = new _p9();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.DataReaderTcpVerify/**/.TcpStaticServer._M11(sender, inputParameter.p2, inputParameter.p3, inputParameter.p0, ref inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                
                                _outputParameter_.p0 = inputParameter.p1;
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
                    case 11:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p10 inputParameter = new _p10();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                (_s11/**/.Pop() ?? new _s11()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s12/**/.Pop() ?? new _s12()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
            sealed class _s1 : AutoCSer.Net.TcpStaticServer.ServerCall<_s1, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M2(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c2, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s2 : AutoCSer.Net.TcpStaticServer.ServerCall<_s2, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
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
            sealed class _s3 : AutoCSer.Net.TcpStaticServer.ServerCall<_s3, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c4, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s4 : AutoCSer.Net.TcpStaticServer.ServerCall<_s4>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p7> value)
                {
                    try
                    {
                        
                        int[] Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M5();

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
                        Sender.Push(CommandIndex, _c5, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 7, IsBuildOutputThread = true };
            sealed class _s5 : AutoCSer.Net.TcpStaticServer.ServerCall<_s5, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int[] Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M6(inputParameter.p0);

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
                        Sender.Push(CommandIndex, _c6, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
            sealed class _s6 : AutoCSer.Net.TcpStaticServer.ServerCall<_s6, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M7(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c7, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s7 : AutoCSer.Net.TcpStaticServer.ServerCall<_s7, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        int[] Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M8(inputParameter.p0);

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
                        Sender.Push(CommandIndex, _c8, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6, IsBuildOutputThread = true };
            sealed class _s8 : AutoCSer.Net.TcpStaticServer.ServerCall<_s8, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M9(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c9, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s9 : AutoCSer.Net.TcpStaticServer.ServerCall<_s9, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M10(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c10, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 9, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            sealed class _s11 : AutoCSer.Net.TcpStaticServer.ServerCall<_s11, _p10>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p11> value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.Student Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType.ClassStudent/**/.TcpStaticServer._M12(inputParameter.p0);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p11> value = new AutoCSer.Net.TcpServer.ReturnValue<_p11>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c12, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 11, IsBuildOutputThread = true };
            sealed class _s12 : AutoCSer.Net.TcpStaticServer.ServerCall<_s12, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p12> value)
                {
                    try
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.Student Return;

                        
                        Return = AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M13(inputParameter.p0);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p12> value = new AutoCSer.Net.TcpServer.ReturnValue<_p12>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c13, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c13 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 12, IsBuildOutputThread = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
            {
                public int p0;
                public int p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p7
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
            {
                public byte[] p0;
                public long p1;
                public string p2;
                public ulong p3;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
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
            internal struct _p10
            {
                public AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType.ClassStudent p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p11
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p12
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
            static DataReader()
            {
                CompileSerialize(new System.Type[] { typeof(_p1), typeof(_p3), typeof(_p5), null }
                    , new System.Type[] { typeof(_p4), typeof(_p9), null }
                    , new System.Type[] { typeof(_p8), typeof(_p10), null }
                    , new System.Type[] { typeof(_p2), typeof(_p6), typeof(_p7), typeof(_p11), typeof(_p12), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
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
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p1), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p3), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p5), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p4), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p8), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p10), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p2), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p6), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p7), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p11), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataReader/**/._p12), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.TestCase.SqlTableCacheServer
{
        public partial class Class
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.TestCase.SqlTableCacheServer.Class _M14(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Class.getSqlCache(Id);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M15(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>, bool> _onReturn_)
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Class.onSqlLog(_onReturn_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 13, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 13, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> getSqlCache(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14>(_c14, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Class> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Class> getSqlCacheAwaiter(int Id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Class> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Class>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13
                        {
                            
                            p0 = Id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Class> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Class>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Class>>(_a14, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                public static AutoCSer.Net.TcpServer.KeepCallback onSqlLog(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15>> _onOutput_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetCallback<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15>(_ac15, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
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
                public static bool _M16(AutoCSer.Net.TcpInternalServer.ServerSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify.verify(_sender_, userID, randomPrefix, md5Data, ref ticks);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 16, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsVerifyMethod = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender _sender_, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p17> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p17>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = _sender_;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p16 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p16
                            {
                                
                                p2 = userID,
                                
                                p3 = randomPrefix,
                                
                                p0 = md5Data,
                                
                                p1 = ticks,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p17 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p17
                            {
                                
                                p0 = ticks,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p16, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p17>(_c16, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            ticks = _outputParameter_.p0;
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p17>.PushNotNull(_wait_);
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
                public static AutoCSer.TestCase.SqlTableCacheServer.Student _M17(int Id)
                {

                    
                    return AutoCSer.TestCase.SqlTableCacheServer.Student.getSqlCache(Id);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M18(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>, bool> _onReturn_)
                {
                    AutoCSer.TestCase.SqlTableCacheServer.Student.onSqlLog(_onReturn_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c17 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 13, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a17 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 13, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> getSqlCache(int Id)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p18> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p18>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13
                            {
                                
                                p0 = Id,
                            };
                            
                            AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p18 _outputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p18
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p18>(_c17, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p18>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.Student> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Student> getSqlCacheAwaiter(int Id)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Student> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBox<AutoCSer.TestCase.SqlTableCacheServer.Student>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13 _inputParameter_ = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13
                        {
                            
                            p0 = Id,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Student> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Student>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<AutoCSer.TestCase.SqlTableCacheServer.Student>>(_a17, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac18 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1 };
                /// <returns>保持异步回调</returns>
                public static AutoCSer.Net.TcpServer.KeepCallback onSqlLog(Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p19>> _onOutput_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.GetCallback<AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data, AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p19>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.TestCase.SqlTableCacheServer.TcpStaticClient/**/.DataLog/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            return _socket_.GetKeep<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p19>(_ac18, ref _onOutput_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p19> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p19> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
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
                            _p13 inputParameter = new _p13();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p14 _outputParameter_ = new _p14();
                                
                                AutoCSer.TestCase.SqlTableCacheServer.Class Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M14(inputParameter.p0);
                                _outputParameter_.Return = Return;
                                sender.Push(_c14, ref _outputParameter_);
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
                                _p15 outputParameter = new _p15();
                                AutoCSer.TestCase.SqlTableCacheServer.Class/**/.TcpStaticServer._M15(sender.GetCallback<_p15, AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Class,AutoCSer.TestCase.SqlModel.Class>.Data>(_c15, ref outputParameter));
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
                            _p16 inputParameter = new _p16();
                            if (sender.DeSerialize(ref data, ref inputParameter))
                            {
                                _p17 _outputParameter_ = new _p17();
                                
                                bool Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.DataLogTcpVerify/**/.TcpStaticServer._M16(sender, inputParameter.p2, inputParameter.p3, inputParameter.p0, ref inputParameter.p1);
                                if (Return) sender.SetVerifyMethod();
                                
                                _outputParameter_.p0 = inputParameter.p1;
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
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p13 inputParameter = new _p13();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p18 _outputParameter_ = new _p18();
                                
                                AutoCSer.TestCase.SqlTableCacheServer.Student Return;
                                
                                Return =  AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M17(inputParameter.p0);
                                _outputParameter_.Return = Return;
                                sender.Push(_c17, ref _outputParameter_);
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
                                _p19 outputParameter = new _p19();
                                AutoCSer.TestCase.SqlTableCacheServer.Student/**/.TcpStaticServer._M18(sender.GetCallback<_p19, AutoCSer.Sql.LogStream.Log<AutoCSer.TestCase.SqlTableCacheServer.Student,AutoCSer.TestCase.SqlModel.Student>.Data>(_c18, ref outputParameter));
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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c14 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 14, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c15 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 15, IsKeepCallback = 1, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c16 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 17, IsSimpleSerializeOutputParamter = true, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c17 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 18, IsBuildOutputThread = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c18 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 19, IsKeepCallback = 1, IsBuildOutputThread = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p13
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p14
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p15
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p16
            {
                public byte[] p0;
                public long p1;
                public string p2;
                public ulong p3;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p17
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
            internal struct _p18
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p19
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
            static DataLog()
            {
                CompileSerialize(new System.Type[] { typeof(_p13), null }
                    , new System.Type[] { typeof(_p17), null }
                    , new System.Type[] { typeof(_p16), null }
                    , new System.Type[] { typeof(_p14), typeof(_p15), typeof(_p18), typeof(_p19), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
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
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p13), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p17), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p16), null }
                    , new System.Type[] { typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p14), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p15), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p18), typeof(AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer/**/.DataLog/**/._p19), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
#endif