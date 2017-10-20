using System;

namespace AutoCSer.CodeGenerator.Template
{
    class RemoteExpression : Pub
    {
        #region NOTE
        private const int NodeMemberIndex = 0;
        private const int StaticMemberStartIndex = 0;
        private static FullName ParameterName = null;
        private static FullName ParameterJoinName = null;
        #endregion NOTE
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            /// <summary>
            /// @Type.XmlDocument 远程表达式
            /// </summary>
            public class @RemoteExpressionTypeName : AutoCSer.Net.RemoteExpression.Node/*NOT:Type.IsStatic*/<@Type.FullName>/*NOT:Type.IsStatic*/
            {
                internal @RemoteExpressionTypeName() : base(/*NOT:Type.IsStatic*/_clientNodeId_.Id/*NOT:Type.IsStatic*/) { }
                protected @RemoteExpressionTypeName(int clientNodeId) : base(clientNodeId) { }
                #region NOT Type.IsStatic
                private static class _clientNodeId_
                {
                    internal static readonly int Id = registerClient(_createReturnValue_);
                    private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<@Type.FullName>(); }
                }
                private @Type.FullName _getValue_()
                {
                    object _value_ = base.getParentValue();
                    return _value_ != null ? (@Type.FullName)_value_ : default(@Type.FullName);
                }
                protected override object get()
                {
                    return _getValue_();
                }
                protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                {
                    @Type.FullName value = _getValue_();
                    return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<@Type.FullName> { Value = value } : null;
                }
                #endregion NOT Type.IsStatic
                #region LOOP Members
                #region NAME MEMBERNODETYPE
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                public sealed class @MemberNodeTypeName/*AT:Method.GenericParameterName*/ : @MemberRemoteExpressionTypeName
                {
                    #region IF MemberIsReturn
                    private static class _clientNodeId_
                    {
                        internal static readonly int Id = registerClient(_createReturnValue_);
                        private static AutoCSer.Net.RemoteExpression.ReturnValue _createReturnValue_() { return new AutoCSer.Net.RemoteExpression.ReturnValue<@MethodReturnType.FullName>(); }
                    }
                    #endregion IF MemberIsReturn
                    #region LOOP IntputParameters
                    private @ParameterType.FullName @ParameterName;
                    #endregion LOOP IntputParameters
                    public @MemberNodeTypeName() { }
                    internal @MemberNodeTypeName(@RemoteExpressionTypeName _parent_/*LOOP:IntputParameters*/, @RemoteExpressionParameterType.FullName @ParameterName/*LOOP:IntputParameters*/) : base(/*IF:MemberIsReturn*/_clientNodeId_.Id/*IF:MemberIsReturn*/)
                    {
                        this.Parent = _parent_;
                        #region LOOP IntputParameters
                        #region IF IsClientNodeParameter
                        setParameter(ref this.@ParameterName, @ParameterName);
                        #endregion IF IsClientNodeParameter
                        #region NOT IsClientNodeParameter
                        this.@ParameterName = @ParameterName;
                        #endregion NOT IsClientNodeParameter
                        #endregion LOOP IntputParameters
                    }
                    protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializer.Stream.Write(checker.Get(typeof(@MemberNodeTypeName/*AT:Method.GenericParameterName*/)));
                        #region LOOP IntputParameters
                        #region IF ParameterType.IsNull
                        serializeParameter(serializer, @ParameterName);
                        #endregion IF ParameterType.IsNull
                        #region NOT ParameterType.IsNull
                        serializeParameterStruct(serializer, ref @ParameterName);
                        #endregion NOT ParameterType.IsNull
                        #endregion LOOP IntputParameters
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        #region LOOP IntputParameters
                        #region IF ParameterType.IsNull
                        deSerializeParameter(deSerializer, ref @ParameterName);
                        #endregion IF ParameterType.IsNull
                        #region NOT ParameterType.IsNull
                        deSerializeParameterStruct(deSerializer, ref @ParameterName);
                        #endregion NOT ParameterType.IsNull
                        #endregion LOOP IntputParameters
                        deSerializeParent(deSerializer);
                    }
                    protected override void serialize(AutoCSer.Json.Serializer serializer, AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker)
                    {
                        serializeStart(serializer, checker);
                        #region LOOP IntputParameters
                        #region IF ParameterType.IsNull
                        serializeParameter(serializer, @ParameterName);
                        #endregion IF ParameterType.IsNull
                        #region NOT ParameterType.IsNull
                        serializeParameterStruct(serializer, ref @ParameterName);
                        #endregion NOT ParameterType.IsNull
                        #endregion LOOP IntputParameters
                        serializeParent(serializer, checker);
                    }
                    protected override void deSerialize(AutoCSer.Json.Parser parser)
                    {
                        #region LOOP IntputParameters
                        deSerializeParameter(parser, ref @ParameterName);
                        #endregion LOOP IntputParameters
                        deSerializeParent(parser);
                    }
                    #region IF IsClientNodeParameter
                    protected override void checkParameterServerNodeId(AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker, ref LeftArray<System.Type> checkTypes)
                    {
                        #region LOOP IntputParameters
                        #region IF IsClientNodeParameter
                        checkServerNodeId(checker, ref checkTypes, ref this.@ParameterName);
                        #endregion IF IsClientNodeParameter
                        #endregion LOOP IntputParameters
                    }
                    #endregion IF IsClientNodeParameter
                    private @MethodReturnType.FullName getValue()
                    {
                        #region IF IsStatic
                        #region PUSH Method
                        return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/@Type.FullName/**/.@StaticMethodName(/*LOOP:IntputParameters*/@ParameterJoinName/*LOOP:IntputParameters*/);
                        #endregion PUSH Method
                        #region NOT Method
                        return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/@Type.FullName/**/.@StaticPropertyName;
                        #endregion NOT Method
                        #endregion IF IsStatic
                        #region NOT IsStatic
                        object _value_ = getParentValue();
                        if (_value_ != null)
                        {
                            #region PUSH Method
                            /*IF:MemberIsReturn*/
                            return /*IF:MemberIsReturn*//*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/((@Type.FullName)_value_).@MethodName(/*LOOP:IntputParameters*/@ParameterJoinName/*LOOP:IntputParameters*/);
                            #endregion PUSH Method
                            #region NOT Method
                            #region IF IntputParameters.Length
                            return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/((@Type.FullName)_value_)[/*LOOP:IntputParameters*/@ParameterJoinName/*LOOP:IntputParameters*/];
                            #endregion IF IntputParameters.Length
                            #region NOT IntputParameters.Length
                            return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/((@Type.FullName)_value_)./*PUSH:Member*/@MemberName/*PUSH:Member*/;
                            #endregion NOT IntputParameters.Length
                            #endregion NOT Method
                        }
                        return default(@MethodReturnType.FullName);
                        #endregion NOT IsStatic
                    }
                    protected override object get()
                    {
                        /*IF:MemberIsReturn*/
                        return /*IF:MemberIsReturn*/getValue();
                        #region NOT MemberIsReturn
                        return null;
                        #endregion NOT MemberIsReturn
                    }
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        #region IF MemberIsReturn
                        @MethodReturnType.FullName value = getValue();
                        return value != null ? new AutoCSer.Net.RemoteExpression.ReturnValue<@MethodReturnType.FullName> { Value = value } : null;
                        #endregion IF MemberIsReturn
                        #region NOT MemberIsReturn
                        getValue();
                        return null;
                        #endregion NOT MemberIsReturn
                    }
                }
                #endregion NAME MEMBERNODETYPE
                #region IF IntputParameters.Length
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                #region LOOP IntputParameters
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion LOOP IntputParameters
                /// <returns>@XmlDocument 远程表达式</returns>
                public @MemberNodeTypeName this[/*LOOP:IntputParameters*/@RemoteExpressionParameterType.FullName @ParameterJoinName/*LOOP:IntputParameters*/] { get { return new @MemberNodeTypeName(this/*LOOP:IntputParameters*/, @ParameterName/*LOOP:IntputParameters*/); } }
                #endregion IF IntputParameters.Length
                #region NOT IntputParameters.Length
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                public @MemberNodeTypeName /*PUSH:Member*/@MemberName/*PUSH:Member*/ { get { return new @MemberNodeTypeName(this/*NOTE*/, null/*NOTE*/); } }
                #endregion NOT IntputParameters.Length
                #endregion LOOP Members
                #region LOOP Methods
                #region FROMNAME MEMBERNODETYPE
                #endregion FROMNAME MEMBERNODETYPE
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                #region LOOP IntputParameters
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion LOOP IntputParameters
                /// <returns>@XmlDocument 远程表达式</returns>
                public @MemberNodeTypeName/*AT:Method.GenericParameterName*/ /*PUSH:Method*/@GenericMethodName/*PUSH:Method*/(/*LOOP:IntputParameters*/@RemoteExpressionParameterType.FullName @ParameterJoinName/*LOOP:IntputParameters*/) { return new @MemberNodeTypeName/*AT:Method.GenericParameterName*/(this/*LOOP:IntputParameters*/, @ParameterName/*LOOP:IntputParameters*/); }
                #endregion LOOP Methods
                #region IF StaticMemberCount
                private static readonly @RemoteExpressionTypeName _static_ = new @RemoteExpressionTypeName();
                #endregion IF StaticMemberCount
                #region LOOP StaticMembers
                #region FROMNAME MEMBERNODETYPE
                #endregion FROMNAME MEMBERNODETYPE
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                public static readonly @MemberNodeTypeName /*PUSH:Member*/@StaticPropertyName/*PUSH:Member*/ = new @MemberNodeTypeName(_static_/*NOTE*/, null/*NOTE*/);
                #endregion LOOP StaticMembers
                #region LOOP StaticMethods
                #region FROMNAME MEMBERNODETYPE
                #endregion FROMNAME MEMBERNODETYPE
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                #region LOOP IntputParameters
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion LOOP IntputParameters
                /// <returns>@XmlDocument 远程表达式</returns>
                public static @MemberNodeTypeName/*AT:Method.GenericParameterName*/ /*PUSH:Method*/@GenericStaticMethodName/*PUSH:Method*/(/*LOOP:IntputParameters*/@RemoteExpressionParameterType.FullName @ParameterJoinName/*LOOP:IntputParameters*/) { return new @MemberNodeTypeName/*AT:Method.GenericParameterName*/(_static_/*LOOP:IntputParameters*/, @ParameterName/*LOOP:IntputParameters*/); }
                #endregion LOOP StaticMethods
            }
        }
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        /// <summary>
        /// 远程表达式参数类型
        /// </summary>
        public class RemoteExpressionParameterType : Pub { }
        /// <summary>
        /// 远程表达式节点类型
        /// </summary>
        public class MemberRemoteExpressionTypeName : AutoCSer.Net.RemoteExpression.Node
        {
            protected MemberRemoteExpressionTypeName() : base() { }
            protected MemberRemoteExpressionTypeName(int clientNodeId) : base(clientNodeId) { }
        }
        /// <summary>
        /// 
        /// </summary>
        public partial class FullName
        {
            /// <summary>
            /// 远程表达式节点类型
            /// </summary>
            public class RemoteExpression { }
            /// <summary>
            /// 远程表达式参数
            /// </summary>
            public AutoCSer.Net.RemoteExpression.Node Node;
            /// <summary>
            /// 客户端检测服务端映射标识
            /// </summary>
            public AutoCSer.Net.RemoteExpression.ServerNodeIdChecker Checker;
        }
    }
    #endregion NOTE
}
