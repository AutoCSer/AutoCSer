//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpInterfaceServer
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
                public sealed class StaticFieldRemoteExpression : AutoCSer.Example.TcpInterfaceServer.Expression.Node1.RemoteExpression
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1>(); }
                    }
                    public StaticFieldRemoteExpression() { }
                    internal StaticFieldRemoteExpression(RemoteExpression _parent_) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(StaticFieldRemoteExpression)));
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParent(parser);
                    }
                    private AutoCSer.Example.TcpInterfaceServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInterfaceServer.Expression/**/.StaticField;
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        AutoCSer.Example.TcpInterfaceServer.Expression.Node1 value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1> { Value = value } : null;
                    }
                }
                /// <summary>
                /// 远程表达式静态字段测试 远程表达式
                /// </summary>
                public static readonly StaticFieldRemoteExpression StaticField = new StaticFieldRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public sealed class StaticPropertyRemoteExpression : AutoCSer.Example.TcpInterfaceServer.Expression.Node1.RemoteExpression
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1>(); }
                    }
                    public StaticPropertyRemoteExpression() { }
                    internal StaticPropertyRemoteExpression(RemoteExpression _parent_) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(StaticPropertyRemoteExpression)));
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParent(parser);
                    }
                    private AutoCSer.Example.TcpInterfaceServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInterfaceServer.Expression/**/.StaticProperty;
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        AutoCSer.Example.TcpInterfaceServer.Expression.Node1 value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1> { Value = value } : null;
                    }
                }
                /// <summary>
                /// 远程表达式静态属性测试 远程表达式
                /// </summary>
                public static readonly StaticPropertyRemoteExpression StaticProperty = new StaticPropertyRemoteExpression(_static_);
                /// <summary>
                /// 远程表达式静态方法测试 远程表达式
                /// </summary>
                public sealed class StaticMethodRemoteExpression : AutoCSer.Example.TcpInterfaceServer.Expression.Node1.RemoteExpression
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1>(); }
                    }
                    private int value;
                    public StaticMethodRemoteExpression() { }
                    internal StaticMethodRemoteExpression(RemoteExpression _parent_, int value) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(StaticMethodRemoteExpression)));
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParameterStruct(deSerializer, ref value);
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParameter(parser, ref value);
                        deSerializeParent(parser);
                    }
                    private AutoCSer.Example.TcpInterfaceServer.Expression.Node1 getValue()
                    {
                        return AutoCSer.Example.TcpInterfaceServer.Expression/**/.StaticMethod(value);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        AutoCSer.Example.TcpInterfaceServer.Expression.Node1 value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1> { Value = value } : null;
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
}namespace AutoCSer.Example.TcpInterfaceServer
{
    internal static partial class Expression
    {
        internal partial class Node1
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInterfaceServer.Expression.Node1>
            {
                internal RemoteExpression() : base(_clientNodeId_.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                private static class _clientNodeId_
                {
                    internal static readonly int Id = registerClient(_createReturnValue_);
                    private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1>(); }
                }
                private AutoCSer.Example.TcpInterfaceServer.Expression.Node1 _getValue_()
                {
                    object _value_ = base.getParentValue();
                    return _value_ != null ? (AutoCSer.Example.TcpInterfaceServer.Expression.Node1)_value_ : default(AutoCSer.Example.TcpInterfaceServer.Expression.Node1);
                }
                protected override object get()
                {
                    return _getValue_();
                }
                protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                {
                    AutoCSer.Example.TcpInterfaceServer.Expression.Node1 value = _getValue_();
                    return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1> { Value = value } : null;
                }
                /// <summary>
                /// 远程表达式实例字段测试 远程表达式
                /// </summary>
                public sealed class ValueRemoteExpression : AutoCSer.Net.RemoteExpression.Node<int>
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<int>(); }
                    }
                    public ValueRemoteExpression() { }
                    internal ValueRemoteExpression(RemoteExpression _parent_) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(ValueRemoteExpression)));
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParent(parser);
                    }
                    private int getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node1)_value_).Value;
                        }
                        return default(int);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        int value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = value } : null;
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
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<int>(); }
                    }
                    private int value;
                    public ItemRemoteExpression() { }
                    internal ItemRemoteExpression(RemoteExpression _parent_, int value) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(ItemRemoteExpression)));
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParameterStruct(deSerializer, ref value);
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParameter(parser, ref value);
                        deSerializeParent(parser);
                    }
                    private int getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node1)_value_)[value];
                        }
                        return default(int);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        int value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = value } : null;
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
                public sealed class NextNodeRemoteExpression : AutoCSer.Example.TcpInterfaceServer.Expression.Node2.RemoteExpression
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node2>(); }
                    }
                    public NextNodeRemoteExpression() { }
                    internal NextNodeRemoteExpression(RemoteExpression _parent_) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(NextNodeRemoteExpression)));
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParent(parser);
                    }
                    private AutoCSer.Example.TcpInterfaceServer.Expression.Node2 getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node1)_value_).NextNode;
                        }
                        return default(AutoCSer.Example.TcpInterfaceServer.Expression.Node2);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        AutoCSer.Example.TcpInterfaceServer.Expression.Node2 value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node2> { Value = value } : null;
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public NextNodeRemoteExpression NextNode { get { return new NextNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetNextNodeRemoteExpression : AutoCSer.Example.TcpInterfaceServer.Expression.Node2.RemoteExpression
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node2>(); }
                    }
                    private int value;
                    public GetNextNodeRemoteExpression() { }
                    internal GetNextNodeRemoteExpression(RemoteExpression _parent_, int value) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(GetNextNodeRemoteExpression)));
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParameterStruct(deSerializer, ref value);
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParameter(parser, ref value);
                        deSerializeParent(parser);
                    }
                    private AutoCSer.Example.TcpInterfaceServer.Expression.Node2 getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node1)_value_).GetNextNode(value);
                        }
                        return default(AutoCSer.Example.TcpInterfaceServer.Expression.Node2);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        AutoCSer.Example.TcpInterfaceServer.Expression.Node2 value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node2> { Value = value } : null;
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
}namespace AutoCSer.Example.TcpInterfaceServer
{
    internal static partial class Expression
    {
        internal partial class Node2
        {
            /// <summary>
            /// 远程表达式测试 远程表达式
            /// </summary>
            public class RemoteExpression : AutoCSer.Net.RemoteExpression.Node<AutoCSer.Example.TcpInterfaceServer.Expression.Node2>
            {
                internal RemoteExpression() : base(_clientNodeId_.Id) { }
                protected RemoteExpression(int clientNodeId) : base(clientNodeId) { }
                private static class _clientNodeId_
                {
                    internal static readonly int Id = registerClient(_createReturnValue_);
                    private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node2>(); }
                }
                private AutoCSer.Example.TcpInterfaceServer.Expression.Node2 _getValue_()
                {
                    object _value_ = base.getParentValue();
                    return _value_ != null ? (AutoCSer.Example.TcpInterfaceServer.Expression.Node2)_value_ : default(AutoCSer.Example.TcpInterfaceServer.Expression.Node2);
                }
                protected override object get()
                {
                    return _getValue_();
                }
                protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                {
                    AutoCSer.Example.TcpInterfaceServer.Expression.Node2 value = _getValue_();
                    return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node2> { Value = value } : null;
                }
                /// <summary>
                /// 远程表达式实例字段测试 远程表达式
                /// </summary>
                public sealed class ValueRemoteExpression : AutoCSer.Net.RemoteExpression.Node<int>
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<int>(); }
                    }
                    public ValueRemoteExpression() { }
                    internal ValueRemoteExpression(RemoteExpression _parent_) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(ValueRemoteExpression)));
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParent(parser);
                    }
                    private int getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node2)_value_).Value;
                        }
                        return default(int);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        int value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = value } : null;
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
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<int>(); }
                    }
                    private int value;
                    public ItemRemoteExpression() { }
                    internal ItemRemoteExpression(RemoteExpression _parent_, int value) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(ItemRemoteExpression)));
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParameterStruct(deSerializer, ref value);
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParameter(parser, ref value);
                        deSerializeParent(parser);
                    }
                    private int getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node2)_value_)[value];
                        }
                        return default(int);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        int value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<int> { Value = value } : null;
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
                public sealed class LastNodeRemoteExpression : AutoCSer.Example.TcpInterfaceServer.Expression.Node1.RemoteExpression
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1>(); }
                    }
                    public LastNodeRemoteExpression() { }
                    internal LastNodeRemoteExpression(RemoteExpression _parent_) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(LastNodeRemoteExpression)));
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParent(parser);
                    }
                    private AutoCSer.Example.TcpInterfaceServer.Expression.Node1 getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node2)_value_).LastNode;
                        }
                        return default(AutoCSer.Example.TcpInterfaceServer.Expression.Node1);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        AutoCSer.Example.TcpInterfaceServer.Expression.Node1 value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1> { Value = value } : null;
                    }
                }
                /// <summary>
                /// 远程表达式实例属性测试 远程表达式
                /// </summary>
                public LastNodeRemoteExpression LastNode { get { return new LastNodeRemoteExpression(this); } }
                /// <summary>
                /// 远程表达式实例方法测试 远程表达式
                /// </summary>
                public sealed class GetLastNodeRemoteExpression : AutoCSer.Example.TcpInterfaceServer.Expression.Node1.RemoteExpression
                {
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1>(); }
                    }
                    private int value;
                    public GetLastNodeRemoteExpression() { }
                    internal GetLastNodeRemoteExpression(RemoteExpression _parent_, int value) : base(_clientNodeId_.Id)
                    {
                        this.Parent = _parent_;
                        this.value = value;
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(GetLastNodeRemoteExpression)));
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        deSerializeParameterStruct(deSerializer, ref value);
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        serializeParameterStruct(serializer, ref value);
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        deSerializeParameter(parser, ref value);
                        deSerializeParent(parser);
                    }
                    private AutoCSer.Example.TcpInterfaceServer.Expression.Node1 getValue()
                    {
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            
                            return ((AutoCSer.Example.TcpInterfaceServer.Expression.Node2)_value_).GetLastNode(value);
                        }
                        return default(AutoCSer.Example.TcpInterfaceServer.Expression.Node1);
                    }
                    protected override object get()
                    {
                        
                        return getValue();
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        AutoCSer.Example.TcpInterfaceServer.Expression.Node1 value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<AutoCSer.Example.TcpInterfaceServer.Expression.Node1> { Value = value } : null;
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
}
#endif