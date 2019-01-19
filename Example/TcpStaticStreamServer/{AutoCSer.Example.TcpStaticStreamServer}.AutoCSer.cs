//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class Expression
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node
            {
                internal RemoteExpression() : base() { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                private static readonly RemoteExpression _static_ = new RemoteExpression();
                /// <summary>
                /// 远程表达式静态字段测试 远程表达式
                /// </summary>
                public sealed class StaticFieldRemoteExpression : AutoCSer.Example.TcpStaticStreamServer.Expression.Node1.RemoteExpression
                {
                    public StaticFieldRemoteExpression() { }
                    internal StaticFieldRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpStaticStreamServer.Expression/**/.StaticField;
                    }
                }
                /// <summary>
                /// 远程表达式静态字段测试 远程表达式
                /// </summary>
                public static readonly StaticFieldRemoteExpression StaticField = new StaticFieldRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public sealed class StaticPropertyRemoteExpression : AutoCSer.Example.TcpStaticStreamServer.Expression.Node1.RemoteExpression
                {
                    public StaticPropertyRemoteExpression() { }
                    internal StaticPropertyRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpStaticStreamServer.Expression/**/.StaticProperty;
                    }
                }
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public static readonly StaticPropertyRemoteExpression StaticProperty = new StaticPropertyRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态方法测试 远程表达式
                /// </summary>
                public sealed class StaticMethodRemoteExpression : AutoCSer.Example.TcpStaticStreamServer.Expression.Node1.RemoteExpression
                {
                    private int value;
                    public StaticMethodRemoteExpression() { }
                    internal StaticMethodRemoteExpression(RemoteExpression _parent_, int value) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serializeParameter(AutoCSer.BinarySerialize.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        base.deSerializeParameterStruct(deSerializer, ref value);
                    }
                    protected override void serializeParameter(AutoCSer.Json.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.Json.Parser parser)
                    {
                        base.deSerializeParameter(parser, ref value);
                    }
                    protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpStaticStreamServer.Expression/**/.StaticMethod(value);
                    }
                }
                /// <summary>
                /// 远程表达式静态方法测试 远程表达式
                /// </summary>
                /// <param name="value">远程表达式静态方法测试</param>
                /// <returns>远程表达式静态方法测试 远程表达式</returns>
                public static StaticMethodRemoteExpression StaticMethod(int value) { return new StaticMethodRemoteExpression(_static_, value); }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
    internal static partial class Expression
    {
        internal partial class Node1
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticStreamServer.Expression.Node1>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticStreamServer.Expression.Node1>)base.Parent).GetValue();
                }
                /// <summary>
                /// 远程表达式实例字段测试 远程表达式
                /// </summary>
                public sealed class ValueRemoteExpression : AutoCSer.Net.RemoteExpression.Node<int>
                {
                    public ValueRemoteExpression() { }
                    internal ValueRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override int getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.Value;
                        }
                        return default(int);
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        return new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = getValue() };
                    }
                }
                /// <summary>
                /// 远程表达式实例字段测试 远程表达式
                /// </summary>
                public ValueRemoteExpression Value { get { return new ValueRemoteExpression(this); } }
                /// <summary>
                ///  远程表达式
                /// </summary>
                public sealed class ItemRemoteExpression : AutoCSer.Net.RemoteExpression.Node<int>
                {
                    private int value;
                    public ItemRemoteExpression() { }
                    internal ItemRemoteExpression(RemoteExpression _parent_, int value) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serializeParameter(AutoCSer.BinarySerialize.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        base.deSerializeParameterStruct(deSerializer, ref value);
                    }
                    protected override void serializeParameter(AutoCSer.Json.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.Json.Parser parser)
                    {
                        base.deSerializeParameter(parser, ref value);
                    }
                    protected override int getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_[value];
                        }
                        return default(int);
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        return new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = getValue() };
                    }
                }
                /// <summary>
                ///  远程表达式
                /// </summary>
                /// <param name="value"></param>
                /// <returns> 远程表达式</returns>
                public ItemRemoteExpression this[int value] { get { return new ItemRemoteExpression(this, value); } }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public sealed class NextNodeRemoteExpression : AutoCSer.Example.TcpStaticStreamServer.Expression.Node2.RemoteExpression
                {
                    public NextNodeRemoteExpression() { }
                    internal NextNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.NextNode;
                        }
                        return default(AutoCSer.Example.TcpStaticStreamServer.Expression.Node2);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public NextNodeRemoteExpression NextNode { get { return new NextNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetNextNodeRemoteExpression : AutoCSer.Example.TcpStaticStreamServer.Expression.Node2.RemoteExpression
                {
                    private int value;
                    public GetNextNodeRemoteExpression() { }
                    internal GetNextNodeRemoteExpression(RemoteExpression _parent_, int value) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serializeParameter(AutoCSer.BinarySerialize.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        base.deSerializeParameterStruct(deSerializer, ref value);
                    }
                    protected override void serializeParameter(AutoCSer.Json.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.Json.Parser parser)
                    {
                        base.deSerializeParameter(parser, ref value);
                    }
                    protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetNextNode(value);
                        }
                        return default(AutoCSer.Example.TcpStaticStreamServer.Expression.Node2);
                    }
                }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                /// <param name="value">远程表达式实例方法测试</param>
                /// <returns>远程表达式实例方法测试 远程表达式</returns>
                public GetNextNodeRemoteExpression GetNextNode(int value) { return new GetNextNodeRemoteExpression(this, value); }
            }
        }
    }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
    internal static partial class Expression
    {
        internal partial class Node2
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticStreamServer.Expression.Node2>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node2 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticStreamServer.Expression.Node2>)base.Parent).GetValue();
                }
                /// <summary>
                /// 远程表达式实例字段测试 远程表达式
                /// </summary>
                public sealed class ValueRemoteExpression : AutoCSer.Net.RemoteExpression.Node<int>
                {
                    public ValueRemoteExpression() { }
                    internal ValueRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override int getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.Value;
                        }
                        return default(int);
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        return new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = getValue() };
                    }
                }
                /// <summary>
                /// 远程表达式实例字段测试 远程表达式
                /// </summary>
                public ValueRemoteExpression Value { get { return new ValueRemoteExpression(this); } }
                /// <summary>
                ///  远程表达式
                /// </summary>
                public sealed class ItemRemoteExpression : AutoCSer.Net.RemoteExpression.Node<int>
                {
                    private int value;
                    public ItemRemoteExpression() { }
                    internal ItemRemoteExpression(RemoteExpression _parent_, int value) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serializeParameter(AutoCSer.BinarySerialize.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        base.deSerializeParameterStruct(deSerializer, ref value);
                    }
                    protected override void serializeParameter(AutoCSer.Json.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.Json.Parser parser)
                    {
                        base.deSerializeParameter(parser, ref value);
                    }
                    protected override int getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_[value];
                        }
                        return default(int);
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        return new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = getValue() };
                    }
                }
                /// <summary>
                ///  远程表达式
                /// </summary>
                /// <param name="value"></param>
                /// <returns> 远程表达式</returns>
                public ItemRemoteExpression this[int value] { get { return new ItemRemoteExpression(this, value); } }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public sealed class LastNodeRemoteExpression : AutoCSer.Example.TcpStaticStreamServer.Expression.Node1.RemoteExpression
                {
                    public LastNodeRemoteExpression() { }
                    internal LastNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.LastNode;
                        }
                        return default(AutoCSer.Example.TcpStaticStreamServer.Expression.Node1);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public LastNodeRemoteExpression LastNode { get { return new LastNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetLastNodeRemoteExpression : AutoCSer.Example.TcpStaticStreamServer.Expression.Node1.RemoteExpression
                {
                    private int value;
                    public GetLastNodeRemoteExpression() { }
                    internal GetLastNodeRemoteExpression(RemoteExpression _parent_, int value) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serializeParameter(AutoCSer.BinarySerialize.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        base.deSerializeParameterStruct(deSerializer, ref value);
                    }
                    protected override void serializeParameter(AutoCSer.Json.Serializer serializer)
                    {
                        base.serializeParameterStruct(serializer, ref value);
                    }
                    protected override void deSerializeParameter(AutoCSer.Json.Parser parser)
                    {
                        base.deSerializeParameter(parser, ref value);
                    }
                    protected override AutoCSer.Example.TcpStaticStreamServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpStaticStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetLastNode(value);
                        }
                        return default(AutoCSer.Example.TcpStaticStreamServer.Expression.Node1);
                    }
                }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                /// <param name="value">远程表达式实例方法测试</param>
                /// <returns>远程表达式实例方法测试 远程表达式</returns>
                public GetLastNodeRemoteExpression GetLastNode(int value) { return new GetLastNodeRemoteExpression(this, value); }
            }
        }
    }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class ClientAsynchronous
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M1(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.ClientAsynchronous.Add(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// 同步函数客户端异步测试 示例
            /// </summary>
            public partial class ClientAsynchronous
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 同步函数客户端异步测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 同步函数客户端异步测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a1, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                /// <summary>
                /// 同步函数客户端异步测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static void Add(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>> _onOutput_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.GetCallback<int, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>(_ac1, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class ClientTaskAsync
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M2(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.ClientTaskAsync.Add(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// 同步函数客户端 async / await 测试 示例
            /// </summary>
            public partial class ClientTaskAsync
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 同步函数客户端 async / await 测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>(_c2, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 同步函数客户端 async / await 测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a2, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
#if !DOTNET2 && !DOTNET4
                /// <summary>
                /// 同步函数客户端 async / await 测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue<int>> AddAsync(int left, int right)
                {
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>();
                        
                        AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        
                        AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>(_a2, _wait_, ref _inputParameter_, ref _outputParameter_)) == Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif

            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class Field
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M3()
                {
                    return AutoCSer.Example.TcpStaticStreamServer.Field/**/.GetField;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M4()
                {
                    return AutoCSer.Example.TcpStaticStreamServer.Field/**/.SetField;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M5(int value)
                {
                    AutoCSer.Example.TcpStaticStreamServer.Field/**/.SetField = value;

                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// 字段支持 示例
            /// </summary>
            public partial class Field
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读字段支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetField
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _outputParameter_ = _socket_.WaitGet(_c3, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写字段支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> SetField
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _outputParameter_ = _socket_.WaitGet(_c4, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p3
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c5, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a5 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class NoAttribute
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M6(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.NoAttribute.Add(left, right);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M7()
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.NoAttribute.TestCase();
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// 无需 TCP 远程函数申明配置 示例
            /// </summary>
            public partial class NoAttribute
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a6 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 5 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>(_c6, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a6, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a7 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 6 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> TestCase()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4>(_c7, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> TestCaseAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a7, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class Property
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M8()
                {
                    return AutoCSer.Example.TcpStaticStreamServer.Property/**/.GetProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M9()
                {
                    return AutoCSer.Example.TcpStaticStreamServer.Property/**/.SetProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M10(int value)
                {
                    AutoCSer.Example.TcpStaticStreamServer.Property/**/.SetProperty = value;

                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// 可读属性支持 示例
            /// </summary>
            public partial class Property
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a8 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 7 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _outputParameter_ = _socket_.WaitGet(_c8, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a9 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 8 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写属性支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> SetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _outputParameter_ = _socket_.WaitGet(_c9, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                            if (_socket_ != null)
                            {
                                
                                AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p3
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c10, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 9 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a10 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 9 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class RefOut
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.Net.TcpServer.ReturnValue<int> _M11(int left, ref int right, out int product)
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.RefOut.Add1(left, ref right, out product);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class SendOnly
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M12(int left, int right)
                {

                    AutoCSer.Example.TcpStaticStreamServer.SendOnly.SetSum1(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class Static
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M13(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.Static.Add(left, right);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M14()
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.Static.TestCase();
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// 支持公共函数 示例
            /// </summary>
            public partial class Static
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a13 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 12 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 支持公共函数
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>(_c13, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 支持公共函数
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        
                        AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a13, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 13 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a14 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 13 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 支持公共函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> TestCase()
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4>(_c14, ref _wait_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample1/**/._p4>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 支持公共函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> TestCaseAwaiter()
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<bool> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<bool>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample1/**/.TcpClient.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>);
                        _returnType_ = _socket_.GetAwaiter<AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<bool>>(_a14, _awaiter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class StreamExample1 : AutoCSer.Net.TcpInternalStreamServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[14];
                names[0].Set(@"AutoCSer.Example.TcpStaticStreamServer.ClientAsynchronous(int,int)Add", 0);
                names[1].Set(@"AutoCSer.Example.TcpStaticStreamServer.ClientTaskAsync(int,int)Add", 1);
                names[2].Set(@"AutoCSer.Example.TcpStaticStreamServer.Field()get_GetField", 2);
                names[3].Set(@"AutoCSer.Example.TcpStaticStreamServer.Field()get_SetField", 3);
                names[4].Set(@"AutoCSer.Example.TcpStaticStreamServer.Field(int)set_SetField", 4);
                names[5].Set(@"AutoCSer.Example.TcpStaticStreamServer.NoAttribute(int,int)Add", 5);
                names[6].Set(@"AutoCSer.Example.TcpStaticStreamServer.NoAttribute()TestCase", 6);
                names[7].Set(@"AutoCSer.Example.TcpStaticStreamServer.Property()get_GetProperty", 7);
                names[8].Set(@"AutoCSer.Example.TcpStaticStreamServer.Property()get_SetProperty", 8);
                names[9].Set(@"AutoCSer.Example.TcpStaticStreamServer.Property(int)set_SetProperty", 9);
                names[10].Set(@"AutoCSer.Example.TcpStaticStreamServer.RefOut(int,ref int,out int)Add1", 10);
                names[11].Set(@"AutoCSer.Example.TcpStaticStreamServer.SendOnly(int,int)SetSum1", 11);
                names[12].Set(@"AutoCSer.Example.TcpStaticStreamServer.Static(int,int)Add", 12);
                names[13].Set(@"AutoCSer.Example.TcpStaticStreamServer.Static()TestCase", 13);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public StreamExample1(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamExample1", typeof(AutoCSer.Example.TcpStaticStreamServer.RefOut), true)), verify, log)
            {
                setCommandData(14);
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
                if (attribute.IsAutoServer) Start();
            }
            /// <summary>
            /// 命令处理
            /// </summary>
            /// <param name="index">命令序号</param>
            /// <param name="sender">TCP 内部服务套接字数据发送</param>
            /// <param name="data">命令数据</param>
            public override void DoCommand(int index, AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
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
                                (_s0/**/.Pop() ?? new _s0()).Set(sender, ref inputParameter);
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
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, ref inputParameter);
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
                                (_s2/**/.Pop() ?? new _s2()).Set(sender);
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
                            {
                                (_s3/**/.Pop() ?? new _s3()).Set(sender);
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
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s4/**/.Pop() ?? new _s4()).Set(sender, ref inputParameter);
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
                                (_s5/**/.Pop() ?? new _s5()).Set(sender, ref inputParameter);
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
                            {
                                (_s6/**/.Pop() ?? new _s6()).Set(sender);
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
                    case 7:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s7/**/.Pop() ?? new _s7()).Set(sender);
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
                    case 8:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                (_s8/**/.Pop() ?? new _s8()).Set(sender);
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
                    case 9:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s9/**/.Pop() ?? new _s9()).Set(sender, ref inputParameter);
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
                            _p5 inputParameter = new _p5();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s10/**/.Pop() ?? new _s10()).Set(sender, ref inputParameter);
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
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s11/**/.Pop() ?? new _s11()).Set(sender, ref inputParameter);
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
                            _p1 inputParameter = new _p1();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s12/**/.Pop() ?? new _s12()).Set(sender, ref inputParameter);
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
                            {
                                (_s13/**/.Pop() ?? new _s13()).Set(sender);
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
            sealed class _s0 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s0, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.ClientAsynchronous/**/.TcpStaticStreamServer._M1(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c1, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s1 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s1, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.ClientTaskAsync/**/.TcpStaticStreamServer._M2(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c2, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s2 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s2>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticStreamServer.Field/**/.TcpStaticStreamServer._M3();


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c3, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s3 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticStreamServer.Field/**/.TcpStaticStreamServer._M4();


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c4, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s4 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s4, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.Example.TcpStaticStreamServer.Field/**/.TcpStaticStreamServer._M5(inputParameter.p0);


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(value.Type);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c5 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
            sealed class _s5 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s5, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.NoAttribute/**/.TcpStaticStreamServer._M6(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c6, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c6 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s6 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s6>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        bool Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.NoAttribute/**/.TcpStaticStreamServer._M7();

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c7, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c7 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };
            sealed class _s7 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s7>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticStreamServer.Property/**/.TcpStaticStreamServer._M8();


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c8, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c8 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s8 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s8>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;
                        Return = AutoCSer.Example.TcpStaticStreamServer.Property/**/.TcpStaticStreamServer._M9();


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c9, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c9 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s9 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s9, _p3>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        
                        AutoCSer.Example.TcpStaticStreamServer.Property/**/.TcpStaticStreamServer._M10(inputParameter.p0);


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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(value.Type);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c10 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
            sealed class _s10 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s10, _p5>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p6> value)
                {
                    try
                    {
                        
                        AutoCSer.Net.TcpServer.ReturnValue<int> Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.RefOut/**/.TcpStaticStreamServer._M11(inputParameter.p0, ref inputParameter.p1, out value.Value.p1);

                        
                        value.Value.p0 = inputParameter.p1;
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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c11, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c11 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 6 };
            sealed class _s11 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s11, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.Example.TcpStaticStreamServer.SendOnly/**/.TcpStaticStreamServer._M12(inputParameter.p0, inputParameter.p1);

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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c12 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1 };
            sealed class _s12 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s12, _p1>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                {
                    try
                    {
                        
                        int Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.Static/**/.TcpStaticStreamServer._M13(inputParameter.p0, inputParameter.p1);

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c13, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c13 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            sealed class _s13 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s13>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p4> value)
                {
                    try
                    {
                        
                        bool Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.Static/**/.TcpStaticStreamServer._M14();

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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c14, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c14 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 4, IsSimpleSerializeOutputParamter = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
            {
                public int p0;
                public int p1;
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
            {
                public int p0;
                public int p1;
                public int p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
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
            static StreamExample1()
            {
                CompileSerialize(new System.Type[] { typeof(_p1), typeof(_p3), typeof(_p5), null }
                    , new System.Type[] { typeof(_p2), typeof(_p4), null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(_p6), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class RefOut
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.Net.TcpServer.ReturnValue<int> _M15(int left, ref int right, out int product)
                {

                    
                    return AutoCSer.Example.TcpStaticStreamServer.RefOut.Add2(left, ref right, out product);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// ref / out 参数测试 示例
            /// </summary>
            public partial class RefOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c15 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 7, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// ref / out 参数测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                /// <param name="product">乘积</param>
                /// <returns>和</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> Add2(int left, ref int right, out int product)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p8> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p8>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample2/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p7 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p7
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p8 _outputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p8
                            {
                                
                                p0 = right,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p7, AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p8>(_c15, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            right = _outputParameter_.p0;
                            
                            product = _outputParameter_.p1;
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p8>.PushNotNull(_wait_);
                    }
                    product = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        internal partial class SendOnly
        {
            internal static partial class TcpStaticStreamServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M16(int left, int right)
                {

                    AutoCSer.Example.TcpStaticStreamServer.SendOnly.SetSum2(left, right);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticStreamServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallStream
        {
            /// <summary>
            /// 仅发送请求测试 示例
            /// </summary>
            public partial class SendOnly
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c16 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 9, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 仅发送请求测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void SetSum2(int left, int right)
                {
                    AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p9 _inputParameter_ = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p9
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient/**/.StreamExample2/**/.TcpClient.Sender.CallOnly(_c16, ref _inputParameter_);
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class StreamExample2 : AutoCSer.Net.TcpInternalStreamServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[2];
                names[0].Set(@"AutoCSer.Example.TcpStaticStreamServer.RefOut(int,ref int,out int)Add2", 0);
                names[1].Set(@"AutoCSer.Example.TcpStaticStreamServer.SendOnly(int,int)SetSum2", 1);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public StreamExample2(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamExample2", typeof(AutoCSer.Example.TcpStaticStreamServer.SendOnly), true)), verify, log)
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
            public override void DoCommand(int index, AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender, ref SubArray<byte> data)
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
                                (_s0/**/.Pop() ?? new _s0()).Set(sender, ref inputParameter);
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
                        try
                        {
                            _p9 inputParameter = new _p9();
                            if (sender.DeSerialize(ref data, ref inputParameter, true))
                            {
                                (_s1/**/.Pop() ?? new _s1()).Set(sender, ref inputParameter);
                                return;
                            }
                        }
                        catch (Exception error)
                        {
                            sender.AddLog(error);
                        }
                        return;
                    default: return;
                }
            }
            sealed class _s0 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s0, _p7>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p8> value)
                {
                    try
                    {
                        
                        AutoCSer.Net.TcpServer.ReturnValue<int> Return;

                        
                        Return = AutoCSer.Example.TcpStaticStreamServer.RefOut/**/.TcpStaticStreamServer._M15(inputParameter.p0, ref inputParameter.p1, out value.Value.p1);

                        
                        value.Value.p0 = inputParameter.p1;
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
                    AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                    if (sender.IsSocket)
                    {
                        get(ref value);
                        push(this);
                        sender.Push(_c15, ref value);
                    }
                    else push(this);
                }
            }
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c15 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 8 };
            sealed class _s1 : AutoCSer.Net.TcpStaticStreamServer.ServerCall<_s1, _p9>
            {
                private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                {
                    try
                    {
                        

                        AutoCSer.Example.TcpStaticStreamServer.SendOnly/**/.TcpStaticStreamServer._M16(inputParameter.p0, inputParameter.p1);

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
            private static readonly AutoCSer.Net.TcpServer.OutputInfo _c16 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1 };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
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
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
            {
                public int p0;
                public int p1;
            }
            static StreamExample2()
            {
                CompileSerialize(new System.Type[] { typeof(_p7), typeof(_p9), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(_p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class StreamExample2
        {
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalStreamServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticStreamServer.Client TcpClient;
            static StreamExample2()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticStreamServer.ServerAttribute.GetConfig("StreamExample2", typeof(AutoCSer.Example.TcpStaticStreamServer.SendOnly));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 StreamExample2 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticStreamServer.Client(config.ServerAttribute, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p7), typeof(AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p9), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer/**/.StreamExample2/**/._p8), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
#endif