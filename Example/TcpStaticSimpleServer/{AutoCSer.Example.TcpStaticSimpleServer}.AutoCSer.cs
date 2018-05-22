//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpStaticSimpleServer
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
                public sealed class StaticFieldRemoteExpression : AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1.RemoteExpression
                {
                    public StaticFieldRemoteExpression() { }
                    internal StaticFieldRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpStaticSimpleServer.Expression/**/.StaticField;
                    }
                }
                /// <summary>
                /// 远程表达式静态字段测试 远程表达式
                /// </summary>
                public static readonly StaticFieldRemoteExpression StaticField = new StaticFieldRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public sealed class StaticPropertyRemoteExpression : AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1.RemoteExpression
                {
                    public StaticPropertyRemoteExpression() { }
                    internal StaticPropertyRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpStaticSimpleServer.Expression/**/.StaticProperty;
                    }
                }
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public static readonly StaticPropertyRemoteExpression StaticProperty = new StaticPropertyRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态方法测试 远程表达式
                /// </summary>
                public sealed class StaticMethodRemoteExpression : AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1.RemoteExpression
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
                    protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpStaticSimpleServer.Expression/**/.StaticMethod(value);
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
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
    internal static partial class Expression
    {
        internal partial class Node1
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1>)base.Parent).GetValue();
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
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                public sealed class NextNodeRemoteExpression : AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2.RemoteExpression
                {
                    public NextNodeRemoteExpression() { }
                    internal NextNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.NextNode;
                        }
                        return default(AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public NextNodeRemoteExpression NextNode { get { return new NextNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetNextNodeRemoteExpression : AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2.RemoteExpression
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
                    protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetNextNode(value);
                        }
                        return default(AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2);
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
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
    internal static partial class Expression
    {
        internal partial class Node2
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2>)base.Parent).GetValue();
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
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                public sealed class LastNodeRemoteExpression : AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1.RemoteExpression
                {
                    public LastNodeRemoteExpression() { }
                    internal LastNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.LastNode;
                        }
                        return default(AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public LastNodeRemoteExpression LastNode { get { return new LastNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetLastNodeRemoteExpression : AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1.RemoteExpression
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
                    protected override AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpStaticSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetLastNode(value);
                        }
                        return default(AutoCSer.Example.TcpStaticSimpleServer.Expression.Node1);
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
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        internal partial class Asynchronous
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M1(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> _onReturn_)
                {
                    AutoCSer.Example.TcpStaticSimpleServer.Asynchronous.Add(left, right, _onReturn_);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// 异步回调测试 示例
            /// </summary>
            public partial class Asynchronous
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 异步回调测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p1
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p2
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example2/**/.TcpClient.Get<AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p1, AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p2>(_c1, ref _inputParameter_, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        internal partial class RefOut
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.Net.TcpServer.ReturnValue<int> _M2(int left, ref int right, out int product)
                {

                    
                    return AutoCSer.Example.TcpStaticSimpleServer.RefOut.Add2(left, ref right, out product);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// ref / out 参数测试 示例
            /// </summary>
            public partial class RefOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 3, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// ref / out 参数测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                /// <param name="product">乘积</param>
                /// <returns>和</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> Add2(int left, ref int right, out int product)
                {
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p3
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p4 _outputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p4
                    {
                        
                        p0 = right,
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example2/**/.TcpClient.Get<AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p3, AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p4>(_c2, ref _inputParameter_, ref _outputParameter_);
                    
                    right = _outputParameter_.p0;
                    
                    product = _outputParameter_.p1;
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class Example2 : AutoCSer.Net.TcpInternalSimpleServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[2];
                names[0].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Asynchronous(int,int,System.Func<AutoCSer.Net.TcpServer.ReturnValue<int>,bool>)Add", 0);
                names[1].Set(@"AutoCSer.Example.TcpStaticSimpleServer.RefOut(int,ref int,out int)Add2", 1);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public Example2(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("Example2", typeof(AutoCSer.Example.TcpStaticSimpleServer.Asynchronous), true)), verify, log, false)
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
            /// <param name="socket">TCP 内部服务套接字数据发送</param>
            /// <param name="data">命令数据</param>
            /// <returns>是否成功</returns>
            public override bool DoCommand(int index, AutoCSer.Net.TcpInternalSimpleServer.ServerSocket socket, ref SubArray<byte> data)
            {
                AutoCSer.Net.TcpServer.ReturnType returnType;
                switch (index - 128)
                {
                    case 0:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p1 inputParameter = new _p1();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p2 outputParameter = new _p2();
                                AutoCSer.Example.TcpStaticSimpleServer.Asynchronous/**/.TcpStaticSimpleServer._M1(inputParameter.p0, inputParameter.p1, socket.GetCallback<_p2, int>(_c1, ref outputParameter));
                                return true;
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p3 inputParameter = new _p3();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p4 _outputParameter_ = new _p4();
                                
                                AutoCSer.Net.TcpServer.ReturnValue<int> Return;
                                
                                Return =  AutoCSer.Example.TcpStaticSimpleServer.RefOut/**/.TcpStaticSimpleServer._M2(inputParameter.p0, ref inputParameter.p1, out _outputParameter_.p1);
                                
                                _outputParameter_.p0 = inputParameter.p1;
                                _outputParameter_.Return = Return;
                                return socket.Send(_c2, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    default: return false;
                }
            }
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 4 };

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
                public int p1;
                public int p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
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
            static Example2()
            {
                CompileSerialize(new System.Type[] { typeof(_p1), typeof(_p3), null }
                    , new System.Type[] { typeof(_p2), null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(_p4), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
namespace AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient
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
                public AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticSimpleServer.Client TcpClient;
            static Example2()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("Example2", typeof(AutoCSer.Example.TcpStaticSimpleServer.Asynchronous));
                }
                if (config.ServerAttribute.IsServer) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Warn | AutoCSer.Log.LogType.Debug, null, "请确认 Example2 服务器端是否本地调用", AutoCSer.Log.CacheType.None);
                TcpClient = new AutoCSer.Net.TcpStaticSimpleServer.Client(config.ServerAttribute, config.Log, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p1), typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p3), null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p2), null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example2/**/._p4), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        internal partial class Field
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M3()
                {
                    return AutoCSer.Example.TcpStaticSimpleServer.Field/**/.GetField;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M4()
                {
                    return AutoCSer.Example.TcpStaticSimpleServer.Field/**/.SetField;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M5(int value)
                {
                    AutoCSer.Example.TcpStaticSimpleServer.Field/**/.SetField = value;

                }
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// 字段支持 示例
            /// </summary>
            public partial class Field
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读字段支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetField
                {
                    get
                    {
                        
                        AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5 _outputParameter_ = default(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get(_c3, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写字段支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> SetField
                {
                    get
                    {
                        
                        AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5 _outputParameter_ = default(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get(_c4, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    set
                    {
                        
                        AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p6 _inputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p6
                        {
                            
                            p0 = value,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Call(_c5, ref _inputParameter_);
                        if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                        throw new Exception(_returnType_.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c5 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 6, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        internal partial class NoAttribute
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M6(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticSimpleServer.NoAttribute.Add(left, right);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M12()
                {

                    
                    return AutoCSer.Example.TcpStaticSimpleServer.NoAttribute.TestCase();
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// 无需 TCP 远程函数申明配置 示例
            /// </summary>
            public partial class NoAttribute
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c6 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 7, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p7 _inputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p7
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5 _outputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get<AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p7, AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5>(_c6, ref _inputParameter_, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c12 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 9 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> TestCase()
                {
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p10 _outputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p10
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get<AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p10>(_c12, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        internal partial class Property
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M7()
                {
                    return AutoCSer.Example.TcpStaticSimpleServer.Property/**/.GetProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M8()
                {
                    return AutoCSer.Example.TcpStaticSimpleServer.Property/**/.SetProperty;

                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void _M9(int value)
                {
                    AutoCSer.Example.TcpStaticSimpleServer.Property/**/.SetProperty = value;

                }
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// 可读属性支持 示例
            /// </summary>
            public partial class Property
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c7 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> GetProperty
                {
                    get
                    {
                        
                        AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5 _outputParameter_ = default(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get(_c7, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c8 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 5 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写属性支持
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> SetProperty
                {
                    get
                    {
                        
                        AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5 _outputParameter_ = default(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5);
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get(_c8, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    set
                    {
                        
                        AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p6 _inputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p6
                        {
                            
                            p0 = value,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Call(_c9, ref _inputParameter_);
                        if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                        throw new Exception(_returnType_.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c9 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 6 + 128, InputParameterIndex = 6, IsSimpleSerializeInputParamter = true };


            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        internal partial class RefOut
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static AutoCSer.Net.TcpServer.ReturnValue<int> _M10(int left, ref int right, out int product)
                {

                    
                    return AutoCSer.Example.TcpStaticSimpleServer.RefOut.Add1(left, ref right, out product);
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        internal partial class Static
        {
            internal static partial class TcpStaticSimpleServer
            {
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static int _M11(int left, int right)
                {

                    
                    return AutoCSer.Example.TcpStaticSimpleServer.Static.Add(left, right);
                }
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static bool _M13()
                {

                    
                    return AutoCSer.Example.TcpStaticSimpleServer.Static.TestCase();
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// 支持公共函数 示例
            /// </summary>
            public partial class Static
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c11 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 8 + 128, InputParameterIndex = 7, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 支持公共函数
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public static AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p7 _inputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p7
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5 _outputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get<AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p7, AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p5>(_c11, ref _inputParameter_, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c13 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 10 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 支持公共函数测试
                /// </summary>
                public static AutoCSer.Net.TcpServer.ReturnValue<bool> TestCase()
                {
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p10 _outputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p10
                    {
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get<AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer/**/.Example1/**/._p10>(_c13, ref _outputParameter_);
                    return new AutoCSer.Net.TcpServer.ReturnValue<bool> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleServer
{

        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public partial class Example1 : AutoCSer.Net.TcpInternalSimpleServer.Server
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[11];
                names[0].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Field()get_GetField", 0);
                names[1].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Field()get_SetField", 1);
                names[2].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Field(int)set_SetField", 2);
                names[3].Set(@"AutoCSer.Example.TcpStaticSimpleServer.NoAttribute(int,int)Add", 3);
                names[4].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Property()get_GetProperty", 4);
                names[5].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Property()get_SetProperty", 5);
                names[6].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Property(int)set_SetProperty", 6);
                names[7].Set(@"AutoCSer.Example.TcpStaticSimpleServer.RefOut(int,ref int,out int)Add1", 7);
                names[8].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Static(int,int)Add", 8);
                names[9].Set(@"AutoCSer.Example.TcpStaticSimpleServer.NoAttribute()TestCase", 9);
                names[10].Set(@"AutoCSer.Example.TcpStaticSimpleServer.Static()TestCase", 10);
                return names;
            }
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="log">日志接口</param>
            public Example1(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                : base(attribute ?? (attribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("Example1", typeof(AutoCSer.Example.TcpStaticSimpleServer.RefOut), true)), verify, log, false)
            {
                setCommandData(11);
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
                if (attribute.IsAutoServer) Start();
            }
            /// <summary>
            /// 命令处理
            /// </summary>
            /// <param name="index">命令序号</param>
            /// <param name="socket">TCP 内部服务套接字数据发送</param>
            /// <param name="data">命令数据</param>
            /// <returns>是否成功</returns>
            public override bool DoCommand(int index, AutoCSer.Net.TcpInternalSimpleServer.ServerSocket socket, ref SubArray<byte> data)
            {
                AutoCSer.Net.TcpServer.ReturnType returnType;
                switch (index - 128)
                {
                    case 0:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p5 _outputParameter_ = new _p5();
                                
                                int Return;
                                Return = AutoCSer.Example.TcpStaticSimpleServer.Field/**/.TcpStaticSimpleServer._M3();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c3, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 1:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p5 _outputParameter_ = new _p5();
                                
                                int Return;
                                Return = AutoCSer.Example.TcpStaticSimpleServer.Field/**/.TcpStaticSimpleServer._M4();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c4, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 2:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p6 inputParameter = new _p6();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                
                                AutoCSer.Example.TcpStaticSimpleServer.Field/**/.TcpStaticSimpleServer._M5(inputParameter.p0);
                                return socket.Send();
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.Send(returnType);
                    case 3:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p7 inputParameter = new _p7();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p5 _outputParameter_ = new _p5();
                                
                                int Return;
                                
                                Return =  AutoCSer.Example.TcpStaticSimpleServer.NoAttribute/**/.TcpStaticSimpleServer._M6(inputParameter.p0, inputParameter.p1);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c6, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 4:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p5 _outputParameter_ = new _p5();
                                
                                int Return;
                                Return = AutoCSer.Example.TcpStaticSimpleServer.Property/**/.TcpStaticSimpleServer._M7();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c7, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 5:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p5 _outputParameter_ = new _p5();
                                
                                int Return;
                                Return = AutoCSer.Example.TcpStaticSimpleServer.Property/**/.TcpStaticSimpleServer._M8();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c8, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 6:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p6 inputParameter = new _p6();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                
                                AutoCSer.Example.TcpStaticSimpleServer.Property/**/.TcpStaticSimpleServer._M9(inputParameter.p0);
                                return socket.Send();
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.Send(returnType);
                    case 7:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p8 inputParameter = new _p8();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p9 _outputParameter_ = new _p9();
                                
                                AutoCSer.Net.TcpServer.ReturnValue<int> Return;
                                
                                Return =  AutoCSer.Example.TcpStaticSimpleServer.RefOut/**/.TcpStaticSimpleServer._M10(inputParameter.p0, ref inputParameter.p1, out _outputParameter_.p1);
                                
                                _outputParameter_.p0 = inputParameter.p1;
                                _outputParameter_.Return = Return;
                                return socket.Send(_c10, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 8:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            _p7 inputParameter = new _p7();
                            if (socket.DeSerialize(ref data, ref inputParameter, true))
                            {
                                _p5 _outputParameter_ = new _p5();
                                
                                int Return;
                                
                                Return =  AutoCSer.Example.TcpStaticSimpleServer.Static/**/.TcpStaticSimpleServer._M11(inputParameter.p0, inputParameter.p1);
                                _outputParameter_.Return = Return;
                                return socket.Send(_c11, ref _outputParameter_);
                            }
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 9:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p10 _outputParameter_ = new _p10();
                                
                                bool Return;
                                
                                Return =  AutoCSer.Example.TcpStaticSimpleServer.NoAttribute/**/.TcpStaticSimpleServer._M12();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c12, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    case 10:
                        returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                        try
                        {
                            {
                                _p10 _outputParameter_ = new _p10();
                                
                                bool Return;
                                
                                Return =  AutoCSer.Example.TcpStaticSimpleServer.Static/**/.TcpStaticSimpleServer._M13();
                                _outputParameter_.Return = Return;
                                return socket.Send(_c13, ref _outputParameter_);
                            }
                        }
                        catch (Exception error)
                        {
                            returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                            socket.Log(error);
                        }
                        return socket.SendOutput(returnType);
                    default: return false;
                }
            }
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c5 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c6 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c7 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c8 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c9 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c10 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 9 };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c11 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 5, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c12 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 10, IsSimpleSerializeOutputParamter = true };
            private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c13 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 10, IsSimpleSerializeOutputParamter = true };

            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
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
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p7
            {
                public int p0;
                public int p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
            {
                public int p0;
                public int p1;
                public int p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
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
            internal struct _p10
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
            static Example1()
            {
                CompileSerialize(new System.Type[] { typeof(_p6), typeof(_p7), typeof(_p8), null }
                    , new System.Type[] { typeof(_p5), typeof(_p10), null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(_p9), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
#endif