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
                #region NOTE
                public abstract class GenericParentMemberNodeTypeName : AutoCSer.Net.RemoteExpression.Node<Type.FullName>
                {
                    public class GenericMemberNodeTypeName : Pub { }
                }
                #endregion NOTE
                internal @RemoteExpressionTypeName() : base(/*NOT:Type.IsStatic*/ReturnClientNodeId.Id/*NOT:Type.IsStatic*/) { }
                protected @RemoteExpressionTypeName(int clientNodeId) : base(clientNodeId) { }
                #region NOT Type.IsStatic
                protected override @Type.FullName getValue()
                {
                    return ((AutoCSer.Net.RemoteExpression.Node<@Type.FullName>)base.Parent).GetValue();
                }
                #endregion NOT Type.IsStatic
                #region LOOP MemberGroup.Members
                #region NAME MEMBERNODETYPE
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                public sealed class @MemberNodeTypeName/*AT:Method.GenericParameterName*/ : @MemberRemoteExpressionTypeName
                {
                    #region LOOP IntputParameters
                    private @ParameterType.FullName @ParameterName;
                    #endregion LOOP IntputParameters
                    public @MemberNodeTypeName() { }
                    internal @MemberNodeTypeName(@RemoteExpressionTypeName _parent_/*LOOP:IntputParameters*/, @RemoteExpressionParameterType.FullName @ParameterName/*LOOP:IntputParameters*/) : base(/*IF:MemberIsReturn*/ReturnClientNodeId.Id/*IF:MemberIsReturn*/)
                    {
                        this.Parent = _parent_;
                        #region LOOP IntputParameters
                        #region IF IsClientNodeParameter
                        base.setParameter(ref this.@ParameterName, @ParameterName);
                        #endregion IF IsClientNodeParameter
                        #region NOT IsClientNodeParameter
                        this.@ParameterName = @ParameterName;
                        #endregion NOT IsClientNodeParameter
                        #endregion LOOP IntputParameters
                    }
                    #region IF IntputParameters.Length
                    protected override void serializeParameter(AutoCSer.BinarySerialize.Serializer serializer)
                    {
                        #region LOOP IntputParameters
                        #region IF ParameterType.IsNull
                        base.serializeParameter(serializer, @ParameterName);
                        #endregion IF ParameterType.IsNull
                        #region NOT ParameterType.IsNull
                        base.serializeParameterStruct(serializer, ref @ParameterName);
                        #endregion NOT ParameterType.IsNull
                        #endregion LOOP IntputParameters
                    }
                    protected override void deSerializeParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                    {
                        #region LOOP IntputParameters
                        #region IF ParameterType.IsNull
                        base.deSerializeParameter(deSerializer, ref @ParameterName);
                        #endregion IF ParameterType.IsNull
                        #region NOT ParameterType.IsNull
                        base.deSerializeParameterStruct(deSerializer, ref @ParameterName);
                        #endregion NOT ParameterType.IsNull
                        #endregion LOOP IntputParameters
                    }
                    protected override void serializeParameter(AutoCSer.Json.Serializer serializer)
                    {
                        #region LOOP IntputParameters
                        #region IF ParameterType.IsNull
                        base.serializeParameter(serializer, @ParameterName);
                        #endregion IF ParameterType.IsNull
                        #region NOT ParameterType.IsNull
                        base.serializeParameterStruct(serializer, ref @ParameterName);
                        #endregion NOT ParameterType.IsNull
                        #endregion LOOP IntputParameters
                    }
                    protected override void deSerializeParameter(AutoCSer.Json.Parser parser)
                    {
                        #region LOOP IntputParameters
                        base.deSerializeParameter(parser, ref @ParameterName);
                        #endregion LOOP IntputParameters
                    }
                    #region IF IsClientNodeParameter
                    protected override void checkParameterServerNodeId(AutoCSer.Net.RemoteExpression.ServerNodeIdChecker checker, ref LeftArray<System.Type> checkTypes)
                    {
                        #region LOOP IntputParameters
                        #region IF IsClientNodeParameter
                        base.checkServerNodeId(checker, ref checkTypes, ref @ParameterName);
                        #endregion IF IsClientNodeParameter
                        #endregion LOOP IntputParameters
                    }
                    #endregion IF IsClientNodeParameter
                    #endregion IF IntputParameters.Length
                    protected override @MethodReturnType.FullName getValue()
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
                        @Type.FullName _value_ = ((@RemoteExpressionTypeName)base.Parent).getValue();
                        #region IF Type.IsNull
                        if (_value_ != null)
                        #endregion IF Type.IsNull
                        {
                            #region PUSH Method
                            /*IF:MemberIsReturn*/
                            return /*IF:MemberIsReturn*//*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/_value_.@MethodName(/*LOOP:IntputParameters*/@ParameterJoinName/*LOOP:IntputParameters*/);
                            #endregion PUSH Method
                            #region NOT Method
                            #region IF IntputParameters.Length
                            return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/_value_[/*LOOP:IntputParameters*/@ParameterJoinName/*LOOP:IntputParameters*/];
                            #endregion IF IntputParameters.Length
                            #region NOT IntputParameters.Length
                            return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/_value_./*PUSH:Member*/@MemberName/*PUSH:Member*/;
                            #endregion NOT IntputParameters.Length
                            #endregion NOT Method
                        }
                        #region IF Type.IsNull
                        return default(@MethodReturnType.FullName);
                        #endregion IF Type.IsNull
                        #endregion NOT IsStatic
                    }
                    #region NOT ReturnTypeIsNull
                    protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                    {
                        #region IF MemberIsReturn
                        return new AutoCSer.Net.RemoteExpression.ReturnValue<@MethodReturnType.FullName> { Value = getValue() };
                        #endregion IF MemberIsReturn
                        #region NOT MemberIsReturn
                        getValue();
                        return null;
                        #endregion NOT MemberIsReturn
                    }
                    #endregion NOT ReturnTypeIsNull
                    #region IF GenericMemberGroup
                    #region LOOP GenericMemberGroup.Members
                    /// <summary>
                    /// @XmlDocument 远程表达式
                    /// </summary>
                    public sealed class @GenericMemberNodeTypeName/*AT:Method.GenericParameterName*/ : @MemberRemoteExpressionTypeName
                    {
                        #region LOOP IntputParameters
                        private @ParameterType.FullName @ParameterName;
                        #endregion LOOP IntputParameters
                        public @GenericMemberNodeTypeName() { }
                        internal @GenericMemberNodeTypeName(@RemoteExpressionTypeName/**/.@GenericParentMemberNodeTypeName _parent_/*LOOP:IntputParameters*/, @RemoteExpressionParameterType.FullName @ParameterName/*LOOP:IntputParameters*/) : base(/*IF:MemberIsReturn*/ReturnClientNodeId.Id/*IF:MemberIsReturn*/)
                        {
                            Parent = _parent_;
                            #region LOOP IntputParameters
                            #region IF IsClientNodeParameter
                            setParameter(ref this.@ParameterName, @ParameterName);
                            #endregion IF IsClientNodeParameter
                            #region NOT IsClientNodeParameter
                            this.@ParameterName = @ParameterName;
                            #endregion NOT IsClientNodeParameter
                            #endregion LOOP IntputParameters
                        }
                        #region IF IntputParameters.Length
                        protected override void serializeParameter(AutoCSer.BinarySerialize.Serializer serializer)
                        {
                            #region LOOP IntputParameters
                            #region IF ParameterType.IsNull
                            serializeParameter(serializer, @ParameterName);
                            #endregion IF ParameterType.IsNull
                            #region NOT ParameterType.IsNull
                            serializeParameterStruct(serializer, ref @ParameterName);
                            #endregion NOT ParameterType.IsNull
                            #endregion LOOP IntputParameters
                        }
                        protected override void deSerializeParameter(AutoCSer.BinarySerialize.DeSerializer deSerializer)
                        {
                            #region LOOP IntputParameters
                            #region IF ParameterType.IsNull
                            deSerializeParameter(deSerializer, ref @ParameterName);
                            #endregion IF ParameterType.IsNull
                            #region NOT ParameterType.IsNull
                            deSerializeParameterStruct(deSerializer, ref @ParameterName);
                            #endregion NOT ParameterType.IsNull
                            #endregion LOOP IntputParameters
                        }
                        protected override void serializeParameter(AutoCSer.Json.Serializer serializer)
                        {
                            #region LOOP IntputParameters
                            #region IF ParameterType.IsNull
                            serializeParameter(serializer, @ParameterName);
                            #endregion IF ParameterType.IsNull
                            #region NOT ParameterType.IsNull
                            serializeParameterStruct(serializer, ref @ParameterName);
                            #endregion NOT ParameterType.IsNull
                            #endregion LOOP IntputParameters
                        }
                        protected override void deSerializeParameter(AutoCSer.Json.Parser parser)
                        {
                            #region LOOP IntputParameters
                            deSerializeParameter(parser, ref @ParameterName);
                            #endregion LOOP IntputParameters
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
                        #endregion IF IntputParameters.Length
                        protected override @MethodReturnType.FullName getValue()
                        {
                            @Type.FullName _value_ = (/*NOTE*/(GenericMemberNodeTypeName)(object)/*NOTE*/(@RemoteExpressionTypeName/**/.@GenericParentMemberNodeTypeName)Parent).getValue();
                            #region IF Type.IsNull
                            if (_value_ != null)
                            #endregion IF Type.IsNull
                            {
                                #region PUSH Method
                                /*IF:MemberIsReturn*/
                                return /*IF:MemberIsReturn*//*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/_value_.@MethodName(/*LOOP:IntputParameters*/@ParameterJoinName/*LOOP:IntputParameters*/);
                                #endregion PUSH Method
                                #region NOT Method
                                #region IF IntputParameters.Length
                                return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/_value_[/*LOOP:IntputParameters*/@ParameterJoinName/*LOOP:IntputParameters*/];
                                #endregion IF IntputParameters.Length
                                #region NOT IntputParameters.Length
                                return /*NOTE*/(MethodReturnType.FullName)(object)/*NOTE*/_value_./*PUSH:Member*/@MemberName/*PUSH:Member*/;
                                #endregion NOT IntputParameters.Length
                                #endregion NOT Method
                            }
                            #region IF Type.IsNull
                            return default(@MethodReturnType.FullName);
                            #endregion IF Type.IsNull
                        }
                        #region NOT ReturnTypeIsNull
                        protected override AutoCSer.Net.RemoteExpression.ReturnValue getReturn()
                        {
                            #region IF MemberIsReturn
                            return new AutoCSer.Net.RemoteExpression.ReturnValue<@MethodReturnType.FullName> { Value = getValue() };
                            #endregion IF MemberIsReturn
                            #region NOT MemberIsReturn
                            getValue();
                            return null;
                            #endregion NOT MemberIsReturn
                        }
                        #endregion NOT ReturnTypeIsNull
                    }
                    #region IF Method
                    /// <summary>
                    /// @XmlDocument 远程表达式
                    /// </summary>
                    #region LOOP IntputParameters
                    /// <param name="@ParameterName">@XmlDocument</param>
                    #endregion LOOP IntputParameters
                    /// <returns>@XmlDocument 远程表达式</returns>
                    public @GenericMemberNodeTypeName/*AT:Method.GenericParameterName*/ /*PUSH:Method*/@GenericMethodName/*PUSH:Method*/(/*LOOP:IntputParameters*/@RemoteExpressionParameterType.FullName @ParameterJoinName/*LOOP:IntputParameters*/) { return new @GenericMemberNodeTypeName/*AT:Method.GenericParameterName*/(/*NOTE*/(RemoteExpressionTypeName.GenericParentMemberNodeTypeName)(object)/*NOTE*/this/*LOOP:IntputParameters*/, @ParameterName/*LOOP:IntputParameters*/); }
                    #endregion IF Method
                    #region NOT Method
                    #region IF IntputParameters.Length
                    /// <summary>
                    /// @XmlDocument 远程表达式
                    /// </summary>
                    #region LOOP IntputParameters
                    /// <param name="@ParameterName">@XmlDocument</param>
                    #endregion LOOP IntputParameters
                    /// <returns>@XmlDocument 远程表达式</returns>
                    public @GenericMemberNodeTypeName this[/*LOOP:IntputParameters*/@RemoteExpressionParameterType.FullName @ParameterJoinName/*LOOP:IntputParameters*/] { get { return new @GenericMemberNodeTypeName(/*NOTE*/(RemoteExpressionTypeName.GenericParentMemberNodeTypeName)(object)/*NOTE*/this/*LOOP:IntputParameters*/, @ParameterName/*LOOP:IntputParameters*/); } }
                    #endregion IF IntputParameters.Length
                    #region NOT IntputParameters.Length
                    /// <summary>
                    /// @XmlDocument 远程表达式
                    /// </summary>
                    public @GenericMemberNodeTypeName /*PUSH:Member*/@MemberName/*PUSH:Member*/ { get { return new @GenericMemberNodeTypeName(/*NOTE*/(RemoteExpressionTypeName.GenericParentMemberNodeTypeName)(object)/*NOTE*/this/*NOTE*/, null/*NOTE*/); } }
                    #endregion NOT IntputParameters.Length
                    #endregion NOT Method
                    #endregion LOOP GenericMemberGroup.Members
                    #endregion IF GenericMemberGroup
                }
                #endregion NAME MEMBERNODETYPE
                #region IF Method
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                #region LOOP IntputParameters
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion LOOP IntputParameters
                /// <returns>@XmlDocument 远程表达式</returns>
                public @MemberNodeTypeName/*AT:Method.GenericParameterName*/ /*PUSH:Method*/@GenericMethodName/*PUSH:Method*/(/*LOOP:IntputParameters*/@RemoteExpressionParameterType.FullName @ParameterJoinName/*LOOP:IntputParameters*/) { return new @MemberNodeTypeName/*AT:Method.GenericParameterName*/(this/*LOOP:IntputParameters*/, @ParameterName/*LOOP:IntputParameters*/); }
                #endregion IF Method
                #region NOT Method
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
                #endregion NOT Method
                #endregion LOOP MemberGroup.Members
                #region IF MemberGroup.StaticMembers.Length
                private static readonly @RemoteExpressionTypeName _static_ = new @RemoteExpressionTypeName();
                #endregion IF MemberGroup.StaticMembers.Length
                #region LOOP MemberGroup.StaticMembers
                #region FROMNAME MEMBERNODETYPE
                #endregion FROMNAME MEMBERNODETYPE
                #region IF Method
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                #region LOOP IntputParameters
                /// <param name="@ParameterName">@XmlDocument</param>
                #endregion LOOP IntputParameters
                /// <returns>@XmlDocument 远程表达式</returns>
                public static @MemberNodeTypeName/*AT:Method.GenericParameterName*/ /*PUSH:Method*/@GenericStaticMethodName/*PUSH:Method*/(/*LOOP:IntputParameters*/@RemoteExpressionParameterType.FullName @ParameterJoinName/*LOOP:IntputParameters*/) { return new @MemberNodeTypeName/*AT:Method.GenericParameterName*/(_static_/*LOOP:IntputParameters*/, @ParameterName/*LOOP:IntputParameters*/); }
                #endregion IF Method
                #region NOT Method
                /// <summary>
                /// @XmlDocument 远程表达式
                /// </summary>
                public static readonly @MemberNodeTypeName /*PUSH:Member*/@StaticPropertyName/*PUSH:Member*/ = new @MemberNodeTypeName(_static_/*NOTE*/, null/*NOTE*/);
                #endregion NOT Method
                #endregion LOOP MemberGroup.StaticMembers
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
        public abstract class MemberRemoteExpressionTypeName : AutoCSer.Net.RemoteExpression.Node<MethodReturnType.FullName>
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
