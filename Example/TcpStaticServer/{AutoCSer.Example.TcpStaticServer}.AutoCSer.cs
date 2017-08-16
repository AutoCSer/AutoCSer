//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class Asynchronous
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M4(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> _onReturn_)
                {
                    AutoCSer.Example.TcpStaticServer.Asynchronous.Add(left, right, _onReturn_);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class Asynchronous
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 3, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_c4, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_c4, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                public static void Add(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>> _onOutput_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.GetCallback<int, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_ac4, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class ClientAsynchronous
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M3(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticServer.ClientAsynchronous.Add(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class ClientAsynchronous
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_c3, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_c3, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                public static void Add(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>> _onOutput_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.GetCallback<int, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_ac3, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class ClientTaskAsync
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M16(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticServer.ClientTaskAsync.Add(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class ClientTaskAsync
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 15 + 128, InputParameterIndex = 3, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_c16, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_c16, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
#if DOTNET2
#else
#if DOTNET4
#else
                public static async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue<int>> AddAsync(int left, int right)
                {
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>();
                        
                        AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        
                        AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_c16, _wait_, ref _inputParameter_, ref _outputParameter_)) == Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif
#endif

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class Field
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M10()
                {
                    return AutoCSer.Example.TcpStaticServer.Field/**/.GetField;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M11()
                {
                    return AutoCSer.Example.TcpStaticServer.Field/**/.SetField;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M12(int value)
                {
                    AutoCSer.Example.TcpStaticServer.Field/**/.SetField = value;

                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class Field
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 9 + 128, InputParameterIndex = 0, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetField
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _outputParameter_ = _socket_.WaitGet(_c10, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c11 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 10 + 128, InputParameterIndex = 0, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                public static AutoCSer.Net.TcpServer.ReturnValue<int> SetField
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _outputParameter_ = _socket_.WaitGet(_c11, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p6 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p6
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c12, ref _wait_, ref _inputParameter_);
                                if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                                throw new Exception(_returnType_.ToString());
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c12 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 11 + 128, InputParameterIndex = 6, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class KeepCallback
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M5(int left, int right, int count, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> _onReturn_)
                {
                    AutoCSer.Example.TcpStaticServer.KeepCallback.Add(left, right, count, _onReturn_);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class KeepCallback
            {

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsKeepCallback = 1, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                /// <returns>保持异步回调</returns>
                public static AutoCSer.Net.TcpServer.KeepCallback Add(int left, int right, int count, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>> _onOutput_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.GetCallback<int, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                                
                                p2 = count,
                            };
                            return _socket_.GetKeep<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p1, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_ac5, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                    return null;
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class NoAttribute
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M6(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticServer.NoAttribute.Add(left, right);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M7()
                {

                    
                    return AutoCSer.Example.TcpStaticServer.NoAttribute.TestCase();
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class NoAttribute
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 3, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_c6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> TestCase()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5>(_c7, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> TestCaseAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_c7, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class Property
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M13()
                {
                    return AutoCSer.Example.TcpStaticServer.Property/**/.GetProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M14()
                {
                    return AutoCSer.Example.TcpStaticServer.Property/**/.SetProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M15(int value)
                {
                    AutoCSer.Example.TcpStaticServer.Property/**/.SetProperty = value;

                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class Property
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 0, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _outputParameter_ = _socket_.WaitGet(_c13, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 13 + 128, InputParameterIndex = 0, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                public static AutoCSer.Net.TcpServer.ReturnValue<int> SetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _outputParameter_ = _socket_.WaitGet(_c14, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p6 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p6
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c15, ref _wait_, ref _inputParameter_);
                                if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                                throw new Exception(_returnType_.ToString());
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue.PushNotNull(_wait_);
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 14 + 128, InputParameterIndex = 6, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class RefOut
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.Net.TcpServer.ReturnValue<int> _M1(int left, ref int right, out int product)
                {

                    
                    return AutoCSer.Example.TcpStaticServer.RefOut.Add1(left, ref right, out product);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class RefOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> Add1(int left, ref int right, out int product)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p2
                            {
                                
                                p0 = right,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p1, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p2>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            right = _outputParameter_.p0;
                            
                            product = _outputParameter_.p1;
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p2>.PushNotNull(_wait_);
                    }
                    product = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class SendOnly
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M2(int left, int right)
                {

                    AutoCSer.Example.TcpStaticServer.SendOnly.SetSum1(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class SendOnly
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void SetSum1(int left, int right)
                {
                    AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender.CallOnly(_c2, ref _inputParameter_);
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class Static
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M8(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticServer.Static.Add(left, right);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M9()
                {

                    
                    return AutoCSer.Example.TcpStaticServer.Static.TestCase();
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class Static
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 3, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>(_c8, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p3, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_c8, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 0, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<bool> TestCase()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5>(_c9, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example1/**/._p5>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> TestCaseAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_c9, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticServer.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class Example1 : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[16];
                names[0].Set(@"AutoCSer.Example.TcpStaticServer.RefOut(int,ref int,out int)Add1", 0);
                names[1].Set(@"AutoCSer.Example.TcpStaticServer.SendOnly(int,int)SetSum1", 1);
                names[2].Set(@"AutoCSer.Example.TcpStaticServer.ClientAsynchronous(int,int)Add", 2);
                names[3].Set(@"AutoCSer.Example.TcpStaticServer.Asynchronous(int,int,System.Func<AutoCSer.Net.TcpServer.ReturnValue<int>,bool>)Add", 3);
                names[4].Set(@"AutoCSer.Example.TcpStaticServer.KeepCallback(int,int,int,System.Func<AutoCSer.Net.TcpServer.ReturnValue<int>,bool>)Add", 4);
                names[5].Set(@"AutoCSer.Example.TcpStaticServer.NoAttribute(int,int)Add", 5);
                names[6].Set(@"AutoCSer.Example.TcpStaticServer.NoAttribute()TestCase", 6);
                names[7].Set(@"AutoCSer.Example.TcpStaticServer.Static(int,int)Add", 7);
                names[8].Set(@"AutoCSer.Example.TcpStaticServer.Static()TestCase", 8);
                names[9].Set(@"AutoCSer.Example.TcpStaticServer.Field()get_GetField", 9);
                names[10].Set(@"AutoCSer.Example.TcpStaticServer.Field()get_SetField", 10);
                names[11].Set(@"AutoCSer.Example.TcpStaticServer.Field(int)set_SetField", 11);
                names[12].Set(@"AutoCSer.Example.TcpStaticServer.Property()get_GetProperty", 12);
                names[13].Set(@"AutoCSer.Example.TcpStaticServer.Property()get_SetProperty", 13);
                names[14].Set(@"AutoCSer.Example.TcpStaticServer.Property(int)set_SetProperty", 14);
                names[15].Set(@"AutoCSer.Example.TcpStaticServer.ClientTaskAsync(int,int)Add", 15);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public Example1(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("Example1", typeof(AutoCSer.Example.TcpStaticServer.RefOut), true)), verify, onCustomData, log, false)
            {
                setCommandData(16);
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
                setCommand(10);
                setCommand(11);
                setCommand(12);
                setCommand(13);
                setCommand(14);
                setCommand(15);
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
                            sender.Log(error);
                        }
                        sender.Push(returnType);
                        return;
                    case 1:
                        try
                        {
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
                                return;
                            }
                        }
                        catch (Exception error)
                        {
                            sender.Log(error);
                        }
                        return;
                    case 2:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
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
                            sender.Log(error);
                        }
                        sender.Push(returnType);
                        return;
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p4 outputParameter = new _p4();
                                AutoCSer.Example.TcpStaticServer.Asynchronous/**/.TcpStaticServer._M4(inputParameter.p0, inputParameter.p1, sender.GetCallback<_p4, int>(_c4, ref outputParameter));
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
                                _p4 outputParameter = new _p4();
                                AutoCSer.Example.TcpStaticServer.KeepCallback/**/.TcpStaticServer._M5(inputParameter.p0, inputParameter.p1, inputParameter.p2, sender.GetCallback<_p4, int>(_c5, ref outputParameter));
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
                    case 5:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
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
                            sender.Log(error);
                        }
                        sender.Push(returnType);
                        return;
                    case 6:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s6/**/.Pop() ?? new _s6()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 7:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
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
                            sender.Log(error);
                        }
                        sender.Push(returnType);
                        return;
                    case 8:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s8/**/.Pop() ?? new _s8()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 9:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s9/**/.Pop() ?? new _s9()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 10:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s10/**/.Pop() ?? new _s10()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 11:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p6 inputParameter = new _p6();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s11/**/.Pop() ?? new _s11()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                    case 12:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s12/**/.Pop() ?? new _s12()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 13:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s13/**/.Pop() ?? new _s13()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout);
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
                    case 14:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p6 inputParameter = new _p6();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s14/**/.Pop() ?? new _s14()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                    case 15:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s15/**/.Pop() ?? new _s15()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
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
                        
                        AutoCSer.Net.TcpServer.ReturnValue<int> Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.RefOut/**/.TcpStaticServer._M1(inputParameter.p0, ref inputParameter.p1, out value.Value.p1);

                        
                        value.Value.p0 = inputParameter.p1;
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
            sealed class _s1 : AutoCSer.Net.TcpStaticServer.ServerCall<_s1, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.Example.TcpStaticServer.SendOnly/**/.TcpStaticServer._M2(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue value = new AutoCSer.Net.TcpServer.ReturnValue();
                    if (Sender.IsSocket) get(ref value);
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1 };
            sealed class _s2 : AutoCSer.Net.TcpStaticServer.ServerCall<_s2, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.ClientAsynchronous/**/.TcpStaticServer._M3(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c3, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsKeepCallback = 1, IsSimpleSerializeOutputParamter = true };
            sealed class _s5 : AutoCSer.Net.TcpStaticServer.ServerCall<_s5, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.NoAttribute/**/.TcpStaticServer._M6(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c6, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            sealed class _s6 : AutoCSer.Net.TcpStaticServer.ServerCall<_s6>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p5> value)
                {
                    try
                    {
                        
                        bool Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.NoAttribute/**/.TcpStaticServer._M7();

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p5> value = new AutoCSer.Net.TcpServer.ReturnValue<_p5>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c7, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            sealed class _s7 : AutoCSer.Net.TcpStaticServer.ServerCall<_s7, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.Static/**/.TcpStaticServer._M8(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c8, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            sealed class _s8 : AutoCSer.Net.TcpStaticServer.ServerCall<_s8>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p5> value)
                {
                    try
                    {
                        
                        bool Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.Static/**/.TcpStaticServer._M9();

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p5> value = new AutoCSer.Net.TcpServer.ReturnValue<_p5>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c9, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            sealed class _s9 : AutoCSer.Net.TcpStaticServer.ServerCall<_s9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticServer.Field/**/.TcpStaticServer._M10();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c10, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            sealed class _s10 : AutoCSer.Net.TcpStaticServer.ServerCall<_s10>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticServer.Field/**/.TcpStaticServer._M11();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c11, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            sealed class _s11 : AutoCSer.Net.TcpStaticServer.ServerCall<_s11, _p6>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.Example.TcpStaticServer.Field/**/.TcpStaticServer._M12(inputParameter.p0);


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
                    AutoCSer.Net.TcpServer.ReturnValue value = new AutoCSer.Net.TcpServer.ReturnValue();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
            sealed class _s12 : AutoCSer.Net.TcpStaticServer.ServerCall<_s12>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticServer.Property/**/.TcpStaticServer._M13();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c13, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c13 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            sealed class _s13 : AutoCSer.Net.TcpStaticServer.ServerCall<_s13>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticServer.Property/**/.TcpStaticServer._M14();


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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c14, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c14 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            sealed class _s14 : AutoCSer.Net.TcpStaticServer.ServerCall<_s14, _p6>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.Example.TcpStaticServer.Property/**/.TcpStaticServer._M15(inputParameter.p0);


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
                    AutoCSer.Net.TcpServer.ReturnValue value = new AutoCSer.Net.TcpServer.ReturnValue();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c15 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
            sealed class _s15 : AutoCSer.Net.TcpStaticServer.ServerCall<_s15, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.ClientTaskAsync/**/.TcpStaticServer._M16(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue<_p4> value = new AutoCSer.Net.TcpServer.ReturnValue<_p4>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c16, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c16 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
            {
                public int p0;
                public int p1;
                public int p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Net.TcpServer.ReturnValue<int>>
#endif
            {
                public int p0;
                public int p1;
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.Net.TcpServer.ReturnValue<int> Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.Net.TcpServer.ReturnValue<int> Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.Net.TcpServer.ReturnValue<int>)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
            {
                public int p0;
                public int p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
            {
                public int p0;
            }
        }
}
namespace AutoCSer.Example.TcpStaticServer.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class Example1
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
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            static Example1()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("Example1", typeof(AutoCSer.Example.TcpStaticServer.RefOut));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 Example1 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.VerifyMethod);
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class RefOut
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.Net.TcpServer.ReturnValue<int> _M17(int left, ref int right, out int product)
                {

                    
                    return AutoCSer.Example.TcpStaticServer.RefOut.Add2(left, ref right, out product);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class RefOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c17 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 7, IsSendOnly = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> Add2(int left, ref int right, out int product)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p8>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example2/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p7 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p7
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p8 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p8
                            {
                                
                                p0 = right,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p7, AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p8>(_c17, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            right = _outputParameter_.p0;
                            
                            product = _outputParameter_.p1;
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p8>.PushNotNull(_wait_);
                    }
                    product = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        internal partial class SendOnly
        {
            internal static partial class TcpStaticServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M18(int left, int right)
                {

                    AutoCSer.Example.TcpStaticServer.SendOnly.SetSum2(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            public partial class SendOnly
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c18 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 9, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void SetSum2(int left, int right)
                {
                    AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p9 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticServer/**/.Example2/**/._p9
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example2/**/.TcpClient.Sender.CallOnly(_c18, ref _inputParameter_);
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticServer.TcpStaticServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class Example2 : AutoCSer.Net.TcpInternalServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[2];
                names[0].Set(@"AutoCSer.Example.TcpStaticServer.RefOut(int,ref int,out int)Add2", 0);
                names[1].Set(@"AutoCSer.Example.TcpStaticServer.SendOnly(int,int)SetSum2", 1);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            /// <param name="onCustomData">自定义数据包处理</param>
            public Example2(AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, Action<SubArray<byte>> onCustomData = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("Example2", typeof(AutoCSer.Example.TcpStaticServer.SendOnly), true)), verify, onCustomData, log, false)
            {
                setCommandData(2);
                setCommand(0);
                setCommand(1);
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
                            _p7 inputParameter = new _p7();
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
                            sender.Log(error);
                        }
                        sender.Push(returnType);
                        return;
                    case 1:
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ref inputParameter);
                                return;
                            }
                        }
                        catch (Exception error)
                        {
                            sender.Log(error);
                        }
                        return;
                    default: return;
                }
            }
            sealed class _s0 : AutoCSer.Net.TcpStaticServer.ServerCall<_s0, _p7>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        AutoCSer.Net.TcpServer.ReturnValue<int> Return;

                        
                        Return = AutoCSer.Example.TcpStaticServer.RefOut/**/.TcpStaticServer._M17(inputParameter.p0, ref inputParameter.p1, out value.Value.p1);

                        
                        value.Value.p0 = inputParameter.p1;
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
                    AutoCSer.Net.TcpServer.ReturnValue<_p8> value = new AutoCSer.Net.TcpServer.ReturnValue<_p8>();
                    if (Sender.IsSocket)
                    {
                        get(ref value);
                        Sender.Push(CommandIndex, _c17, ref value);
                    }
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c17 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8 };
            sealed class _s1 : AutoCSer.Net.TcpStaticServer.ServerCall<_s1, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.Example.TcpStaticServer.SendOnly/**/.TcpStaticServer._M18(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpServer.ReturnValue value = new AutoCSer.Net.TcpServer.ReturnValue();
                    if (Sender.IsSocket) get(ref value);
                    push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c18 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p7
            {
                public int p0;
                public int p1;
                public int p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Net.TcpServer.ReturnValue<int>>
#endif
            {
                public int p0;
                public int p1;
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.Net.TcpServer.ReturnValue<int> Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.Net.TcpServer.ReturnValue<int> Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.Net.TcpServer.ReturnValue<int>)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
            {
                public int p0;
                public int p1;
            }
        }
}
namespace AutoCSer.Example.TcpStaticServer.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class Example2
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
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            static Example2()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("Example2", typeof(AutoCSer.Example.TcpStaticServer.SendOnly));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 Example2 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.VerifyMethod);
            }
        }
}
#endif