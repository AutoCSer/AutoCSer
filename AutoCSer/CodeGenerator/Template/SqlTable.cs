using System;
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class SqlTable : Pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition/*NOTE*/ : SqlModel.TypeNameDefinition.SqlModel<TypeNameDefinition, MemberCacheCounterType>/*NOTE*/
        {
            #region IF RemoteMembers.Length
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                /// <summary>
                /// @Type.XmlDocument
                /// </summary>
                internal @Type.FullName Value;
                #region LOOP RemoteMembers
                #region IF Member
                #region PUSH Member
                /// <summary>
                /// @XmlDocument
                /// </summary>
                public @MemberType.FullName @MemberName
                {
                    get
                    {
                        #region IF IsLogProxyMember
                        if (Value.@IsSqlLogProxyLoadedName) return /*NOTE*/(MemberType.FullName)/*NOTE*/Value.@LogProxyMemberName/**/.@MemberName;
                        #endregion IF IsLogProxyMember
                        #region NAME GETMEMEBER
                        #region IF Identity
                        return /*NOTE*/(MemberType.FullName)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value./*PUSH:Identity*/@MemberName/*PUSH:Identity*/);
                        #endregion IF Identity
                        #region NOT Identity
                        #region IF IsSinglePrimaryKey
                        return /*NOTE*/(MemberType.FullName)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value./*LOOP:PrimaryKeys*/@MemberName/*LOOP:PrimaryKeys*/);
                        #endregion IF IsSinglePrimaryKey
                        #region NOT IsSinglePrimaryKey
                        return /*NOTE*/(MemberType.FullName)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(new DataPrimaryKey { /*PUSH:PrimaryKey0*/@MemberName = Value.@MemberName/*PUSH:PrimaryKey0*//*LOOP:NextPrimaryKeys*/, @NextMemberName = Value.@MemberName/*LOOP:NextPrimaryKeys*/ });
                        #endregion NOT IsSinglePrimaryKey
                        #endregion NOT Identity
                        #endregion NAME GETMEMEBER
                    }
                }
                #endregion PUSH Member
                #endregion IF Member
                #region NOT Member
                #region PUSH Method
                public @MethodReturnType.FullName @AttributeMemberName
                {
                    get
                    {
                        #region FROMNAME GETMEMEBER
                        #endregion FROMNAME GETMEMEBER
                        #region NOTE
                        return null;
                        #endregion NOTE
                    }
                }
                #endregion PUSH Method
                #endregion NOT Member
                #endregion LOOP RemoteMembers
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
            #endregion IF RemoteMembers.Length
            #region IF LogMembers.Length
            #region LOOP LogMembers
            #region PUSH Member
            #region IF Identity
            /// <summary>
            /// 获取数据
            /// </summary>
            #region PUSH Identity
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion PUSH Identity
            /// <returns></returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            internal static @MemberType.FullName @GetMemberName(int /*PUSH:Identity*/@MemberName/*PUSH:Identity*/)
            {
                @Type.FullName value = @IdentityArrayCacheName[/*PUSH:Identity*/@MemberName/*PUSH:Identity*/];
                if (!value.@IsSqlLogProxyLoadedName) sqlStream.WaitMember(@MemberIndex);
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@LogProxyMemberName/**/.@MemberName;

            }
            #endregion IF Identity
            #region NOT Identity
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns></returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            internal static @MemberType.FullName @GetMemberName(@PrimaryKeyType key)
            {
                @Type.FullName value = @PrimaryKeyCacheName[key];
                if (!value.@IsSqlLogProxyLoadedName) sqlStream.WaitMember(@MemberIndex);
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@LogProxyMemberName/**/.@MemberName;
            }
            #endregion NOT Identity
            #endregion PUSH Member
            #endregion LOOP LogMembers
            #endregion IF LogMembers.Length
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
        /// 
        /// </summary>
        public class TcpCall
        {
            /// <summary>
            /// 当前类型名称
            /// </summary>
            public class TypeName : Pub
            {
            }
        }
        /// <summary>
        /// 成员名称
        /// </summary>
        public Pub NextMemberName = null;
    }
    #endregion NOTE
}
