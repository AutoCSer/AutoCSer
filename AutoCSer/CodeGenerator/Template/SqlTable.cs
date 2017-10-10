using System;
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class SqlTable : Pub
    {
        #region NOTE
        private static FullName ParameterName = null;
        private static Type.FullName GetRemoteMethodName(MemberType.FullName MemberName) { return null; }
        #endregion NOTE
        #region PART CLASS
        #region PART CLIENT
        #region IF RemoteMethods.Length
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition/*NOTE*/ : SqlModel.TypeNameDefinition.SqlModel<TypeNameDefinition, MemberCacheCounterType>/*NOTE*/
        {
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                #region IF IsRemoteMember
                /// <summary>
                /// @Type.XmlDocument
                /// </summary>
                internal @Type.FullName Value;
                #endregion IF IsRemoteMember
                #region LOOP RemoteMethods
                #region PUSH Member
                /// <summary>
                /// @XmlDocument
                /// </summary>
                public @MemberType.FullName @MemberName
                {
                    get
                    {
                        if (Value.@IsSqlLogProxyLoadedName) return /*NOTE*/(MemberType.FullName)/*NOTE*/Value.@LogProxyMemberName/**/.@MemberName;
                        return /*NOTE*/(MemberType.FullName)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*//*AT:Awaiter*/(Value./*PUSH:RemoteLinkType.RemoteKeyMember*/@MemberName/*PUSH:RemoteLinkType.RemoteKeyMember*/);
                    }
                }
                #endregion PUSH Member
                #endregion LOOP RemoteMethods
            }
            #region IF IsRemoteMember
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            [AutoCSer.BinarySerialize.IgnoreMember]
            [AutoCSer.Json.IgnoreMember]
            public RemoteExtension Remote
            {
                get { return new RemoteExtension { Value = this }; }
            }
            #endregion IF IsRemoteMember
        }
        #endregion IF RemoteMethods.Length
        #endregion PART CLIENT

        #region PART CLIENTCALL
        #region IF RemoteMethods.Length
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            #region NOTE
            public class TypeName : Pub { }
            #endregion NOTE
            #region IF Type.XmlDocument
            /// <summary>
            /// @Type.XmlDocument
            /// </summary>
            #endregion IF Type.XmlDocument
            public /*NOTE*/partial class /*NOTE*/@NoAccessTypeNameDefinition
            {
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                public partial struct RemoteExtension
                {
                    #region IF IsRemoteMember
                    #region PUSH RemoteLinkType.RemoteKeyMember
                    #region IF XmlDocument
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    #endregion IF XmlDocument
                    public @MemberType.FullName @MemberName;
                    #endregion PUSH RemoteLinkType.RemoteKeyMember
                    #endregion IF IsRemoteMember
                    #region LOOP RemoteMethods
                    #region PUSH Method
                    #region IF IsMethod
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    #region LOOP NextParameters
                    #region IF XmlDocument
                    /// <param name="@ParameterName">@XmlDocument</param>
                    #endregion IF XmlDocument
                    #endregion LOOP NextParameters
                    public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeMethodName(/*LOOP:NextParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:NextParameters*/)
                    {
                        /*IF:IsMethodReturn*/
                        return /*IF:IsMethodReturn*/ /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(@MemberName/*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                    }
                    #endregion IF IsMethod
                    #region NOT IsMethod
                    /// <summary>
                    /// @XmlDocument
                    /// </summary>
                    public /*IF:Attribute.IsAwait*/AutoCSer.Net.TcpServer.AwaiterBox</*IF:Attribute.IsAwait*/@MethodReturnType.FullName/*IF:Attribute.IsAwait*/>/*IF:Attribute.IsAwait*/ @AttributeMemberName
                    {
                        get
                        {
                            return /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(@MemberName);
                        }
                    }
                    #endregion NOT IsMethod
                    #endregion PUSH Method
                    #endregion LOOP RemoteMethods
                }
                #region IF IsRemoteMember
                #region PUSH RemoteLinkType.RemoteKeyMember
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                #region IF XmlDocument
                /// <param name="@MemberName">@XmlDocument</param>
                #endregion IF XmlDocument
                /// <returns>远程对象扩展</returns>
                public static RemoteExtension Remote(@MemberType.FullName @MemberName)
                {
                    return new RemoteExtension { @MemberName = @MemberName };
                }
                #endregion PUSH RemoteLinkType.RemoteKeyMember
                #endregion IF IsRemoteMember
            }
        }
        #endregion IF RemoteMethods.Length
        #endregion PART CLIENTCALL

        #region PART SERVER
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            #region LOOP LogMembers
            #region PUSH Member
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH RemoteLinkType.RemoteKeyMember
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion PUSH RemoteLinkType.RemoteKeyMember
            /// <returns></returns>
            #region IF Attribute.IsRemoteMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(MemberName = @"@MemberName", IsClientRemoteMember = false/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF Attribute.IsRemoteMethod
            #region NOT Attribute.IsRemoteMethod
            [AutoCSer.Net.TcpStaticServer.RemoteMember(MemberName = @"@MemberName", IsClientRemoteMember = false/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsRemoteMethod
            [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            internal/*NOTE*/ new/*NOTE*/ static @MemberType.FullName @GetMethodName(/*PUSH:RemoteLinkType.RemoteKeyMember*/@MemberType.FullName @MemberName/*PUSH:RemoteLinkType.RemoteKeyMember*/)
            {
                @Type.FullName value = /*PUSH:RemoteLinkType*/@GetRemoteMethodName(/*PUSH:RemoteKeyMember*/@MemberName/*PUSH:RemoteKeyMember*/);/*PUSH:RemoteLinkType*/
                if (!value.@IsSqlLogProxyLoadedName) sqlStream.WaitMember(@MemberIndex);
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@LogProxyMemberName/**/.@MemberName;
            }
            #endregion PUSH Member
            #endregion LOOP LogMembers
        }
        #endregion PART SERVER
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        /// <summary>
        /// 成员计数缓存类型
        /// </summary>
        public abstract class MemberCacheCounterType : AutoCSer.Sql.Cache.Whole.MemberCacheCounter<SqlTable.TypeNameDefinition, MemberCacheCounterType> { }
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : Pub
        {
            /// <summary>
            /// 日志字段代理是否加载完毕
            /// </summary>
            public bool IsSqlLogProxyLoadedName;
            /// <summary>
            /// 日志字段代理成员名称
            /// </summary>
            public FullName LogProxyMemberName;
        }
        /// <summary>
        /// 成员名称
        /// </summary>
        public Pub NextMemberName = null;
    }
    #endregion NOTE
}
