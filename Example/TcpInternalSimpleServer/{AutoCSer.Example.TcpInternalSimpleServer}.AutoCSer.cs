//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpInternalSimpleServer
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
                public sealed class StaticFieldRemoteExpression : AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1.RemoteExpression
                {
                    public StaticFieldRemoteExpression() { }
                    internal StaticFieldRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInternalSimpleServer.Expression/**/.StaticField;
                    }
                }
                /// <summary>
                /// 远程表达式静态字段测试 远程表达式
                /// </summary>
                public static readonly StaticFieldRemoteExpression StaticField = new StaticFieldRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public sealed class StaticPropertyRemoteExpression : AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1.RemoteExpression
                {
                    public StaticPropertyRemoteExpression() { }
                    internal StaticPropertyRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInternalSimpleServer.Expression/**/.StaticProperty;
                    }
                }
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public static readonly StaticPropertyRemoteExpression StaticProperty = new StaticPropertyRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态方法测试 远程表达式
                /// </summary>
                public sealed class StaticMethodRemoteExpression : AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1.RemoteExpression
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
                    protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInternalSimpleServer.Expression/**/.StaticMethod(value);
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
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
    internal static partial class Expression
    {
        internal partial class Node1
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1>)base.Parent).GetValue();
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
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                public sealed class NextNodeRemoteExpression : AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2.RemoteExpression
                {
                    public NextNodeRemoteExpression() { }
                    internal NextNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.NextNode;
                        }
                        return default(AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public NextNodeRemoteExpression NextNode { get { return new NextNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetNextNodeRemoteExpression : AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2.RemoteExpression
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
                    protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetNextNode(value);
                        }
                        return default(AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2);
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
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
    internal static partial class Expression
    {
        internal partial class Node2
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2>)base.Parent).GetValue();
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
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                public sealed class LastNodeRemoteExpression : AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1.RemoteExpression
                {
                    public LastNodeRemoteExpression() { }
                    internal LastNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.LastNode;
                        }
                        return default(AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public LastNodeRemoteExpression LastNode { get { return new LastNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetLastNodeRemoteExpression : AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1.RemoteExpression
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
                    protected override AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpInternalSimpleServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetLastNode(value);
                        }
                        return default(AutoCSer.Example.TcpInternalSimpleServer.Expression.Node1);
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
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
        internal partial class Asynchronous
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,int,System.Func<AutoCSer.Net.TcpServer.ReturnValue<int>,bool>)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpInternalSimpleServer.Asynchronous TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalSimpleServer.Asynchronous Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalSimpleServer.Asynchronous TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Asynchronous", typeof(AutoCSer.Example.TcpInternalSimpleServer.Asynchronous))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpInternalSimpleServer.Asynchronous();
                    setCommandData(1);
                    setCommand(0);
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
                                    
                                    Value.Add(inputParameter.p0, inputParameter.p1, socket.GetCallback<_p2, int>(_c0, ref outputParameter));
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
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Asynchronous", typeof(AutoCSer.Example.TcpInternalSimpleServer.Asynchronous));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 异步回调测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        TcpInternalSimpleServer._p2 _outputParameter_ = new TcpInternalSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1, TcpInternalSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
        internal partial class Field
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[3];
                names[0].Set(@"()get_GetField", 0);
                names[1].Set(@"()get_SetField", 1);
                names[2].Set(@"(int)set_SetField", 2);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpInternalSimpleServer.Field TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalSimpleServer.Field Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalSimpleServer.Field TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Field", typeof(AutoCSer.Example.TcpInternalSimpleServer.Field))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpInternalSimpleServer.Field();
                    setCommandData(3);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
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
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.GetField;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
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
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.SetField;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c1, ref _outputParameter_);
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
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    
                                    Value.SetField = inputParameter.p0;
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
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
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
                internal struct _p2
                {
                    public int p0;
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Field", typeof(AutoCSer.Example.TcpInternalSimpleServer.Field));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读字段支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> GetField
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1>(_c0, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写字段支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> SetField
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1>(_c1, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                            {
                                
                                p0 = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c2, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 2, IsSimpleSerializeInputParamter = true };


                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p2), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
        internal partial class NoAttribute
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,int)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpInternalSimpleServer.NoAttribute TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalSimpleServer.NoAttribute Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalSimpleServer.NoAttribute TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.NoAttribute", typeof(AutoCSer.Example.TcpInternalSimpleServer.NoAttribute))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpInternalSimpleServer.NoAttribute();
                    setCommandData(1);
                    setCommand(0);
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
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    int Return;
                                    
                                    Return = Value.Add(inputParameter.p0, inputParameter.p1);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
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
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.NoAttribute", typeof(AutoCSer.Example.TcpInternalSimpleServer.NoAttribute));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        TcpInternalSimpleServer._p2 _outputParameter_ = new TcpInternalSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1, TcpInternalSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
        internal partial class Property
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[5];
                names[0].Set(@"()get_GetProperty", 0);
                names[1].Set(@"(int)get_Item", 1);
                names[2].Set(@"(int,int)set_Item", 2);
                names[3].Set(@"()get_SetProperty", 3);
                names[4].Set(@"(int)set_SetProperty", 4);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpInternalSimpleServer.Property TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalSimpleServer.Property Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalSimpleServer.Property TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Property", typeof(AutoCSer.Example.TcpInternalSimpleServer.Property))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpInternalSimpleServer.Property();
                    setCommandData(5);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
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
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.GetProperty;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
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
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value[inputParameter.p0];
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c1, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
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
                                _p3 inputParameter = new _p3();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    
                                    Value[inputParameter.p0] = inputParameter.p1;
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
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.SetProperty;
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
                        case 4:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    
                                    Value.SetProperty = inputParameter.p0;
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
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p2), typeof(_p3), null }
                        , new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
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
                internal struct _p2
                {
                    public int p0;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int p0;
                    public int p1;
                }
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Property", typeof(AutoCSer.Example.TcpInternalSimpleServer.Property));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> GetProperty
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1>(_c0, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c1 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 1 + 128, InputParameterIndex = 2, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                            {
                                
                                p0 = index,
                            };
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p2, TcpInternalSimpleServer._p1>(_c1, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p3 _inputParameter_ = new TcpInternalSimpleServer._p3
                            {
                                
                                p0 = index,
                                
                                p1 = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c2, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c2 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 2 + 128, InputParameterIndex = 3, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c3 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 3 + 128, InputParameterIndex = 0, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写属性支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> SetProperty
                {
                    get
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p1 _outputParameter_ = default(TcpInternalSimpleServer._p1);
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1>(_c3, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        if (_isDisposed_ == 0)
                        {
                            TcpInternalSimpleServer._p2 _inputParameter_ = new TcpInternalSimpleServer._p2
                            {
                                
                                p0 = value,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Call(_c4, ref _inputParameter_);
                            if (_returnType_ == AutoCSer.Net.TcpServer.ReturnType.Success) return;
                            throw new Exception(_returnType_.ToString());
                        }
                        throw new Exception(AutoCSer.Net.TcpServer.ReturnType.ClientException.ToString());
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c4 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 4 + 128, InputParameterIndex = 2, IsSimpleSerializeInputParamter = true };


                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p2), typeof(TcpInternalSimpleServer._p3), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
        internal partial class RefOut
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,ref int,out int)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpInternalSimpleServer.RefOut TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalSimpleServer.RefOut Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalSimpleServer.RefOut TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.RefOut", typeof(AutoCSer.Example.TcpInternalSimpleServer.RefOut))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpInternalSimpleServer.RefOut();
                    setCommandData(1);
                    setCommand(0);
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
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    AutoCSer.Net.TcpServer.ReturnValue<int> Return;
                                    
                                    Return = Value.Add(inputParameter.p0, ref inputParameter.p1, out _outputParameter_.p1);
                                    
                                    _outputParameter_.p0 = inputParameter.p1;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
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
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.RefOut", typeof(AutoCSer.Example.TcpInternalSimpleServer.RefOut));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// ref / out 参数测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                /// <param name="product">乘积</param>
                /// <returns>和</returns>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> Add(int left, ref int right, out int product)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        TcpInternalSimpleServer._p2 _outputParameter_ = new TcpInternalSimpleServer._p2
                        {
                            
                            p0 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1, TcpInternalSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        
                        right = _outputParameter_.p0;
                        
                        product = _outputParameter_.p1;
                        return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    product = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalSimpleServer
{
        internal partial class Static
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,int)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpInternalSimpleServer.Static TCP服务
            /// </summary>
            public sealed class TcpInternalSimpleServer : AutoCSer.Net.TcpInternalSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalSimpleServer.Static Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalSimpleServer.Static TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleServer(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Static", typeof(AutoCSer.Example.TcpInternalSimpleServer.Static))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpInternalSimpleServer.Static();
                    setCommandData(1);
                    setCommand(0);
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
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    int Return;
                                    
                                    Return = Value.Add(inputParameter.p0, inputParameter.p1);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
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
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                static TcpInternalSimpleServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { typeof(_p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }

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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalSimpleClient : AutoCSer.Net.TcpInternalSimpleServer.MethodClient<TcpInternalSimpleClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpInternalSimpleClient(AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalSimpleServer.Static", typeof(AutoCSer.Example.TcpInternalSimpleServer.Static));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalSimpleServer.Client<TcpInternalSimpleClient>(this, attribute, log);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c0 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 0 + 128, InputParameterIndex = 1, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 支持公共函数
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    if (_isDisposed_ == 0)
                    {
                        TcpInternalSimpleServer._p1 _inputParameter_ = new TcpInternalSimpleServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        TcpInternalSimpleServer._p2 _outputParameter_ = new TcpInternalSimpleServer._p2
                        {
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_ = _TcpClient_.Get<TcpInternalSimpleServer._p1, TcpInternalSimpleServer._p2>(_c0, ref _inputParameter_, ref _outputParameter_);
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalSimpleClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalSimpleServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalSimpleServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}
#endif