//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpInternalStreamServer
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
                public sealed class StaticFieldRemoteExpression : AutoCSer.Example.TcpInternalStreamServer.Expression.Node1.RemoteExpression
                {
                    public StaticFieldRemoteExpression() { }
                    internal StaticFieldRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInternalStreamServer.Expression/**/.StaticField;
                    }
                }
                /// <summary>
                /// 远程表达式静态字段测试 远程表达式
                /// </summary>
                public static readonly StaticFieldRemoteExpression StaticField = new StaticFieldRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public sealed class StaticPropertyRemoteExpression : AutoCSer.Example.TcpInternalStreamServer.Expression.Node1.RemoteExpression
                {
                    public StaticPropertyRemoteExpression() { }
                    internal StaticPropertyRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInternalStreamServer.Expression/**/.StaticProperty;
                    }
                }
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public static readonly StaticPropertyRemoteExpression StaticProperty = new StaticPropertyRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态方法测试 远程表达式
                /// </summary>
                public sealed class StaticMethodRemoteExpression : AutoCSer.Example.TcpInternalStreamServer.Expression.Node1.RemoteExpression
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
                    protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInternalStreamServer.Expression/**/.StaticMethod(value);
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
}namespace AutoCSer.Example.TcpInternalStreamServer
{
    internal static partial class Expression
    {
        internal partial class Node1
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalStreamServer.Expression.Node1>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalStreamServer.Expression.Node1>)base.Parent).GetValue();
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
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                public sealed class NextNodeRemoteExpression : AutoCSer.Example.TcpInternalStreamServer.Expression.Node2.RemoteExpression
                {
                    public NextNodeRemoteExpression() { }
                    internal NextNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.NextNode;
                        }
                        return default(AutoCSer.Example.TcpInternalStreamServer.Expression.Node2);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public NextNodeRemoteExpression NextNode { get { return new NextNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetNextNodeRemoteExpression : AutoCSer.Example.TcpInternalStreamServer.Expression.Node2.RemoteExpression
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
                    protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node2 getValue()
                    {
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetNextNode(value);
                        }
                        return default(AutoCSer.Example.TcpInternalStreamServer.Expression.Node2);
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
}namespace AutoCSer.Example.TcpInternalStreamServer
{
    internal static partial class Expression
    {
        internal partial class Node2
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalStreamServer.Expression.Node2>
            {
                internal RemoteExpression() : base(ReturnClientNodeId.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node2 getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInternalStreamServer.Expression.Node2>)base.Parent).GetValue();
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
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
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
                public sealed class LastNodeRemoteExpression : AutoCSer.Example.TcpInternalStreamServer.Expression.Node1.RemoteExpression
                {
                    public LastNodeRemoteExpression() { }
                    internal LastNodeRemoteExpression(RemoteExpression _parent_) : base(ReturnClientNodeId.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            return _value_.LastNode;
                        }
                        return default(AutoCSer.Example.TcpInternalStreamServer.Expression.Node1);
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public LastNodeRemoteExpression LastNode { get { return new LastNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetLastNodeRemoteExpression : AutoCSer.Example.TcpInternalStreamServer.Expression.Node1.RemoteExpression
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
                    protected override AutoCSer.Example.TcpInternalStreamServer.Expression.Node1 getValue()
                    {
                        AutoCSer.Example.TcpInternalStreamServer.Expression.Node2 _value_ = ((RemoteExpression)base.Parent).getValue();
                        if (_value_ != null)
                        {
                            
                            return _value_.GetLastNode(value);
                        }
                        return default(AutoCSer.Example.TcpInternalStreamServer.Expression.Node1);
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
}namespace AutoCSer.Example.TcpInternalStreamServer
{
        internal partial class ClientAsynchronous
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
            /// AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous", typeof(AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous();
                    setCommandData(1);
                    setCommand(0);
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
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.Add(inputParameter.p0, inputParameter.p1);

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
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                static TcpInternalStreamServer()
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous", typeof(AutoCSer.Example.TcpInternalStreamServer.ClientAsynchronous));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 同步函数客户端异步测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 同步函数客户端异步测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _ac0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                /// <summary>
                /// 同步函数客户端异步测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                void Add(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> _onReturn_)
                {
                    AutoCSer.Net.Callback<AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p2>> _onOutput_ = _TcpClient_.GetCallback<int, TcpInternalStreamServer._p2>(_onReturn_);
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            _socket_.Get<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_ac0, ref _onOutput_, ref _inputParameter_);
                        }
                    }
                    finally
                    {
                        if (_onOutput_ != null)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p2> _outputParameter_ = new AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p2> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                            _onOutput_.Call(ref _outputParameter_);
                        }
                    }
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalStreamServer
{
        internal partial class ClientTaskAsync
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
            /// AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync", typeof(AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync();
                    setCommandData(1);
                    setCommand(0);
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
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.Add(inputParameter.p0, inputParameter.p1);

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
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                static TcpInternalStreamServer()
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync", typeof(AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 同步函数客户端 async / await 测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 同步函数客户端 async / await 测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
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
                public 
                async System.Threading.Tasks.Task<AutoCSer.Net.TcpServer.ReturnValue<int>> AddAsync(int left, int right)
                {
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2> _wait_ = new AutoCSer.Net.TcpServer.TaskAsyncReturnValue<TcpInternalStreamServer._p2>();
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                        {
                        };
                        if ((_returnType_ = _socket_.GetAsync<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_a0, _wait_, ref _inputParameter_, ref _outputParameter_)) == AutoCSer.Net.TcpServer.ReturnType.Success)
                        {
                            AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p2> _returnOutputParameter_ = await _wait_;
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnOutputParameter_.Type, Value = _returnOutputParameter_.Value.Return };
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_ };
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
#endif

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalStreamServer
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
            /// AutoCSer.Example.TcpInternalStreamServer.Field TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.Field Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.Field TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.Field", typeof(AutoCSer.Example.TcpInternalStreamServer.Field))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.Field();
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
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value);
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
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value);
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.Field>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.GetField;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s1, AutoCSer.Example.TcpInternalStreamServer.Field>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.SetField;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s2 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s2, AutoCSer.Example.TcpInternalStreamServer.Field, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.SetField = inputParameter.p0;


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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
                static TcpInternalStreamServer()
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.Field", typeof(AutoCSer.Example.TcpInternalStreamServer.Field));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读字段支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> GetField
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p1>(_c0, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写字段支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> SetField
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p1>(_c1, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c2, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };


                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalStreamServer
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
            /// AutoCSer.Example.TcpInternalStreamServer.NoAttribute TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.NoAttribute Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.NoAttribute TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.NoAttribute", typeof(AutoCSer.Example.TcpInternalStreamServer.NoAttribute))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.NoAttribute();
                    setCommandData(1);
                    setCommand(0);
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
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.NoAttribute, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.Add(inputParameter.p0, inputParameter.p1);

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
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                static TcpInternalStreamServer()
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.NoAttribute", typeof(AutoCSer.Example.TcpInternalStreamServer.NoAttribute));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 无需 TCP 远程函数申明配置测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalStreamServer
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
            /// AutoCSer.Example.TcpInternalStreamServer.Property TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.Property Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.Property TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.Property", typeof(AutoCSer.Example.TcpInternalStreamServer.Property))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.Property();
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
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value);
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
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s1/**/.Pop() ?? new _s1()).Set(sender, Value, ref inputParameter);
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
                                _p3 inputParameter = new _p3();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s2/**/.Pop() ?? new _s2()).Set(sender, Value, ref inputParameter);
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
                                    (_s3/**/.Pop() ?? new _s3()).Set(sender, Value);
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
                                _p2 inputParameter = new _p2();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s4/**/.Pop() ?? new _s4()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.Property>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.GetProperty;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s1 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s1, AutoCSer.Example.TcpInternalStreamServer.Property, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue[inputParameter.p0];


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c1 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s2 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s2, AutoCSer.Example.TcpInternalStreamServer.Property, _p3>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue[inputParameter.p0] = inputParameter.p1;


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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c2 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
                sealed class _s3 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s3, AutoCSer.Example.TcpInternalStreamServer.Property>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p1> value)
                    {
                        try
                        {
                            
                            int Return;
                            Return = serverValue.SetProperty;


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
                        AutoCSer.Net.TcpServer.ReturnValue<_p1> value = new AutoCSer.Net.TcpServer.ReturnValue<_p1>();
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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c3 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 1, IsSimpleSerializeOutputParamter = true };
                sealed class _s4 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s4, AutoCSer.Example.TcpInternalStreamServer.Property, _p2>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            
                            serverValue.SetProperty = inputParameter.p0;


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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c4 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0 };
                static TcpInternalStreamServer()
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.Property", typeof(AutoCSer.Example.TcpInternalStreamServer.Property));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 只读属性支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> GetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p1>(_c0, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };


                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> this[int index]
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                                {
                                    
                                    p0 = index,
                                };
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p2, TcpInternalStreamServer._p1>(_c1, ref _wait_, ref _inputParameter_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p3 _inputParameter_ = new TcpInternalStreamServer._p3
                                {
                                    
                                    p0 = index,
                                    
                                    p1 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c2, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 2 + 128, InputParameterIndex = 3, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a3 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 3 + 128, InputParameterIndex = 0, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeOutputParamter = true };


                /// <summary>
                /// 可写属性支持
                /// </summary>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> SetProperty
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                AutoCSer.Net.TcpServer.ReturnValue<TcpInternalStreamServer._p1> _outputParameter_ = _socket_.WaitGet<TcpInternalStreamServer._p1>(_c3, ref _wait_);
                                return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _outputParameter_.Type, Value = _outputParameter_.Value.Return };
                            }
                        }
                        finally
                        {
                            if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p1>.PushNotNull(_wait_);
                        }
                        return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                    }
                    set
                    {
                        AutoCSer.Net.TcpServer.AutoWaitReturnValue _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue.Pop();
                        try
                        {
                            AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                            if (_socket_ != null)
                            {
                                TcpInternalStreamServer._p2 _inputParameter_ = new TcpInternalStreamServer._p2
                                {
                                    
                                    p0 = value,
                                };
                                AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitCall(_c4, ref _wait_, ref _inputParameter_);
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
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a4 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 4 + 128, InputParameterIndex = 2, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true };


                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p2), typeof(TcpInternalStreamServer._p3), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalStreamServer
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
            /// AutoCSer.Example.TcpInternalStreamServer.RefOut TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.RefOut Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.RefOut TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.RefOut", typeof(AutoCSer.Example.TcpInternalStreamServer.RefOut))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.RefOut();
                    setCommandData(1);
                    setCommand(0);
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
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.RefOut, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            AutoCSer.Net.TcpServer.ReturnValue<int> Return;

                            
                            Return = serverValue.Add(inputParameter.p0, ref inputParameter.p1, out value.Value.p1);

                            
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
                        AutoCSer.Net.TcpServer.ReturnValue<_p2> value = new AutoCSer.Net.TcpServer.ReturnValue<_p2>();
                        AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender sender = Sender;
                        if (sender.IsSocket)
                        {
                            get(ref value);
                            push(this);
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2 };
                static TcpInternalStreamServer()
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.RefOut", typeof(AutoCSer.Example.TcpInternalStreamServer.RefOut));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

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
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                                
                                p0 = right,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            right = _outputParameter_.p0;
                            
                            product = _outputParameter_.p1;
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    product = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalStreamServer
{
        internal partial class SendOnly
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,int)SetSum", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpInternalStreamServer.SendOnly TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.SendOnly Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.SendOnly TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.SendOnly", typeof(AutoCSer.Example.TcpInternalStreamServer.SendOnly))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.SendOnly();
                    setCommandData(1);
                    setCommand(0);
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
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (sender.DeSerialize(ref data, ref inputParameter, true))
                                {
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.SendOnly, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue value)
                    {
                        try
                        {
                            

                            serverValue.SetSum(inputParameter.p0, inputParameter.p1);

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
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 0, IsClientSendOnly = 1 };
                static TcpInternalStreamServer()
                {
                    CompileSerialize(new System.Type[] { typeof(_p1), null }
                        , new System.Type[] { null }
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
            }
            /// <summary>
            /// TCP客户端
            /// </summary>
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.SendOnly", typeof(AutoCSer.Example.TcpInternalStreamServer.SendOnly));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 仅发送请求测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public 
                void SetSum(int left, int right)
                {
                    TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    _TcpClient_.Sender.CallOnly(_c0, ref _inputParameter_);
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}namespace AutoCSer.Example.TcpInternalStreamServer
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
            /// AutoCSer.Example.TcpInternalStreamServer.Static TCP服务
            /// </summary>
            public sealed class TcpInternalStreamServer : AutoCSer.Net.TcpInternalStreamServer.Server
            {
                public readonly AutoCSer.Example.TcpInternalStreamServer.Static Value;
                /// <summary>
                /// AutoCSer.Example.TcpInternalStreamServer.Static TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="verify">套接字验证委托</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamServer(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.Static", typeof(AutoCSer.Example.TcpInternalStreamServer.Static))), verify, log)
                {
                    Value =new AutoCSer.Example.TcpInternalStreamServer.Static();
                    setCommandData(1);
                    setCommand(0);
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
                                    (_s0/**/.Pop() ?? new _s0()).Set(sender, Value, ref inputParameter);
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
                sealed class _s0 : AutoCSer.Net.TcpInternalStreamServer.ServerCall<_s0, AutoCSer.Example.TcpInternalStreamServer.Static, _p1>
                {
                    private void get(ref AutoCSer.Net.TcpServer.ReturnValue<_p2> value)
                    {
                        try
                        {
                            
                            int Return;

                            
                            Return = serverValue.Add(inputParameter.p0, inputParameter.p1);

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
                            sender.Push(_c0, ref value);
                        }
                        else push(this);
                    }
                }
                private static readonly AutoCSer.Net.TcpServer.OutputInfo _c0 = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = 2, IsSimpleSerializeOutputParamter = true };
                static TcpInternalStreamServer()
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
            public partial class TcpInternalStreamClient : AutoCSer.Net.TcpInternalStreamServer.MethodClient<TcpInternalStreamClient>
            {
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                /// <param name="attribute">TCP 调用服务器端配置信息</param>
                /// <param name="clientRoute">TCP 客户端路由</param>
                /// <param name="log">日志接口</param>
                public TcpInternalStreamClient(AutoCSer.Net.TcpInternalStreamServer.ServerAttribute attribute = null, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender> clientRoute = null, AutoCSer.Log.ILog log = null)
                {
                    if (attribute == null)
                    {
                        attribute = AutoCSer.Net.TcpInternalStreamServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpInternalStreamServer.Static", typeof(AutoCSer.Example.TcpInternalStreamServer.Static));
                    }
                    _TcpClient_ = new AutoCSer.Net.TcpInternalStreamServer.Client<TcpInternalStreamClient>(this, attribute, log, clientRoute);
                    if (attribute.IsAuto) _TcpClient_.TryCreateSocket();
                }

                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _a0 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Timeout, IsSimpleSerializeInputParamter = true, IsSimpleSerializeOutputParamter = true };

                /// <summary>
                /// 支持公共函数
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                        if (_socket_ != null)
                        {
                            TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            TcpInternalStreamServer._p2 _outputParameter_ = new TcpInternalStreamServer._p2
                            {
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<TcpInternalStreamServer._p1, TcpInternalStreamServer._p2>(_c0, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<TcpInternalStreamServer._p2>.PushNotNull(_wait_);
                    }
                    return new AutoCSer.Net.TcpServer.ReturnValue<int> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }
                /// <summary>
                /// 支持公共函数
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                public 
                AutoCSer.Net.TcpServer.AwaiterBoxReference<int> AddAwaiter(int left, int right)
                {
                    AutoCSer.Net.TcpServer.AwaiterBoxReference<int> _awaiter_ = new AutoCSer.Net.TcpServer.AwaiterBoxReference<int>();
                    AutoCSer.Net.TcpInternalStreamServer.ClientSocketSender _socket_ = _TcpClient_.Sender;
                    if (_socket_ != null)
                    {
                        TcpInternalStreamServer._p1 _inputParameter_ = new TcpInternalStreamServer._p1
                        {
                            
                            p0 = left,
                            
                            p1 = right,
                        };
                        AutoCSer.Net.TcpServer.ReturnType _returnType_;
                        AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int> _outputParameter_ = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>);
                        _returnType_ = _socket_.GetAwaiter<TcpInternalStreamServer._p1, AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<int>>(_a0, _awaiter_, ref _inputParameter_, ref _outputParameter_);
                        if (_returnType_ != AutoCSer.Net.TcpServer.ReturnType.Success) _awaiter_.Call(_returnType_);
                    }
                    else _awaiter_.Call(AutoCSer.Net.TcpServer.ReturnType.ClientException);
                    return _awaiter_;
                }

                static TcpInternalStreamClient()
                {
                    _compileSerialize_(new System.Type[] { typeof(TcpInternalStreamServer._p1), null }
                        , new System.Type[] { typeof(TcpInternalStreamServer._p2), null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null }
                        , new System.Type[] { null });
                }
            }
        }
}
#endif