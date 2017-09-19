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
            #region NOTE
            private static FullName ParameterName = null;
            private static FullName ParameterJoinName = null;
            private static FullName ParentIndexName = null;
            #endregion NOTE
            #region IF RemoteMethods.Length
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            public partial struct RemoteExtension
            {
                /// <summary>
                /// @Type.XmlDocument
                /// </summary>
                internal @Type.FullName Value;
                #region LOOP RemoteMethods
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
                        /*IF:IsMethodReturn*/
                        return /*IF:IsMethodReturn*//*NOTE*/(MemberType.FullName)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value./*PUSH:Identity*/@MemberName/*PUSH:Identity*//*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                        #endregion IF Identity
                        #region NOT Identity
                        #region IF IsSinglePrimaryKey
                        /*IF:IsMethodReturn*/
                        return /*IF:IsMethodReturn*/ /*NOTE*/(MemberType.FullName)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value./*LOOP:PrimaryKeys*/@MemberName/*LOOP:PrimaryKeys*//*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                        #endregion IF IsSinglePrimaryKey
                        #region NOT IsSinglePrimaryKey
                        /*IF:IsMethodReturn*/
                        return /*IF:IsMethodReturn*/ /*NOTE*/(MemberType.FullName)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(new DataPrimaryKey { /*PUSH:PrimaryKey0*/@MemberName = Value.@MemberName/*PUSH:PrimaryKey0*//*LOOP:NextPrimaryKeys*/, @NextMemberName = Value.@MemberName/*LOOP:NextPrimaryKeys*/ }/*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
                        #endregion NOT IsSinglePrimaryKey
                        #endregion NOT Identity
                        #endregion NAME GETMEMEBER
                    }
                }
                #endregion PUSH Member
                #endregion IF Member
                #region NOT Member
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
                public @MethodReturnType.FullName @AttributeMethodName(/*LOOP:NextParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:NextParameters*/)
                {
                    #region FROMNAME GETMEMEBER
                    #endregion FROMNAME GETMEMEBER
                    #region NOTE
                    return null;
                    #endregion NOTE
                }
                #endregion IF IsMethod
                #region NOT IsMethod
                /// <summary>
                /// @XmlDocument
                /// </summary>
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
                #endregion NOT IsMethod
                #endregion PUSH Method
                #endregion NOT Member
                #endregion LOOP RemoteMethods
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
            #endregion IF RemoteMethods.Length

            #region LOOP LogMembers
            #region PUSH Member
            #region IF Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH Identity
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion PUSH Identity
            /// <returns></returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            internal static @MemberType.FullName @GetMethodName(int /*PUSH:Identity*/@MemberName/*PUSH:Identity*/)
            {
                @Type.FullName value = @IdentityArrayCacheName[/*PUSH:Identity*/@MemberName/*PUSH:Identity*/];
                if (!value.@IsSqlLogProxyLoadedName) sqlStream.WaitMember(@MemberIndex);
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@LogProxyMemberName/**/.@MemberName;

            }
            #endregion IF Identity
            #region NOT Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns></returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            internal static @MemberType.FullName @GetMethodName(@PrimaryKeyType key)
            {
                @Type.FullName value = @PrimaryKeyCacheName[key];
                if (!value.@IsSqlLogProxyLoadedName) sqlStream.WaitMember(@MemberIndex);
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@LogProxyMemberName/**/.@MemberName;
            }
            #endregion NOT Identity
            #endregion PUSH Member
            #endregion LOOP LogMembers

            #region LOOP RemoteMembers
            #region PUSH Member
            #region IF Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH Identity
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion PUSH Identity
            /// <returns>@XmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MemberType.FullName @GetMemberName(int /*PUSH:Identity*/@MemberName/*PUSH:Identity*/)
            {
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/@IdentityArrayCacheName[/*PUSH:Identity*/@MemberName/*PUSH:Identity*/].@MemberName;
            }
            #endregion IF Identity
            #region NOT Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region IF IsSinglePrimaryKey
            #region IF PrimaryKey0.XmlDocument
            /// <param name="@PrimaryKeyName">@PrimaryKey0.XmlDocument</param>
            #endregion IF PrimaryKey0.XmlDocument
            #endregion IF IsSinglePrimaryKey
            #region NOT IsSinglePrimaryKey
            /// <param name="key">关键字</param>
            #endregion NOT IsSinglePrimaryKey
            /// <returns>@XmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MemberType.FullName @GetMemberName(@PrimaryKeyType @PrimaryKeyName)
            {
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/@PrimaryKeyCacheName[@PrimaryKeyName].@MemberName;
            }
            #endregion NOT Identity
            #endregion PUSH Member
            #region PUSH Method
            #region IF Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH Identity
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion PUSH Identity
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MethodReturnType.FullName @RemoteMethodName(int /*PUSH:Identity*/@MemberName/*PUSH:Identity*//*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                /*IF:IsMethodReturn*/
                return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/@IdentityArrayCacheName[/*PUSH:Identity*/@MemberName/*PUSH:Identity*/].@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
            }
            #endregion IF Identity
            #region NOT Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region IF IsSinglePrimaryKey
            #region IF PrimaryKey0.XmlDocument
            /// <param name="@PrimaryKeyName">@PrimaryKey0.XmlDocument</param>
            #endregion IF PrimaryKey0.XmlDocument
            #endregion IF IsSinglePrimaryKey
            #region NOT IsSinglePrimaryKey
            /// <param name="key">关键字</param>
            #endregion NOT IsSinglePrimaryKey
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MethodReturnType.FullName @RemoteMethodName(@PrimaryKeyType @PrimaryKeyName/*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                /*IF:IsMethodReturn*/
                return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/@PrimaryKeyCacheName[@PrimaryKeyName].@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
            }
            #endregion NOT Identity
            #endregion PUSH Method
            #endregion LOOP RemoteMembers

            #region LOOP RemoteCaches
            #region PUSH Member
            #region IF Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH Identity
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion PUSH Identity
            /// <returns>@XmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@CacheMemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MemberType.FullName @GetCacheMemberName(int /*PUSH:Identity*/@MemberName/*PUSH:Identity*/)
            {
                @Type.FullName value = @IdentityArrayCacheName[/*PUSH:Identity*/@MemberName/*PUSH:Identity*/];
                #region NAME CACHEMEMBER
                #region LOOP Members
                #region PUSH Member
                @MemberType.FullName @IndexName = /*NOTE*/(@MemberType.FullName)/*NOTE*/@ParentIndexName/**/.@MemberName;
                #endregion PUSH Member
                #region IF IsNull
                if (@IndexName != null)
                {
                    #endregion IF IsNull
                    #endregion LOOP Members
                    return /*NOTE*/(@MemberType.FullName)/*NOTE*/@IndexName/**/.@MemberName;
                    #region LOOP Members
                    #region IF IsNull
                }
                #endregion IF IsNull
                #endregion LOOP Members
                #endregion NAME CACHEMEMBER
                #region IF IsAnyNull
                return default(@MemberType.FullName);
                #endregion IF IsAnyNull
            }
            #endregion IF Identity
            #region NOT Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region IF IsSinglePrimaryKey
            #region IF PrimaryKey0.XmlDocument
            /// <param name="@PrimaryKeyName">@PrimaryKey0.XmlDocument</param>
            #endregion IF PrimaryKey0.XmlDocument
            #endregion IF IsSinglePrimaryKey
            #region NOT IsSinglePrimaryKey
            /// <param name="key">关键字</param>
            #endregion NOT IsSinglePrimaryKey
            /// <returns>@XmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@CacheMemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MemberType.FullName @GetCacheMemberName(@PrimaryKeyType @PrimaryKeyName)
            {
                @Type.FullName value = @PrimaryKeyCacheName[@PrimaryKeyName];
                #region FROMNAME CACHEMEMBER
                #endregion FROMNAME CACHEMEMBER
                #region IF IsAnyNull
                return default(@MemberType.FullName);
                #endregion IF IsAnyNull
            }
            #endregion NOT Identity
            #endregion PUSH Member
            #region PUSH Method
            #region IF Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region PUSH Identity
            /// <param name="@MemberName">@XmlDocument</param>
            #endregion PUSH Identity
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@CacheMemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MethodReturnType.FullName @RemoteCacheMethodName(int /*PUSH:Identity*/@MemberName/*PUSH:Identity*//*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                @Type.FullName value = @IdentityArrayCacheName[/*PUSH:Identity*/@MemberName/*PUSH:Identity*/];
                #region NAME CACHEMETHOD
                #region LOOP Members
                #region PUSH Member
                @MemberType.FullName @IndexName = /*NOTE*/(@MemberType.FullName)/*NOTE*/@ParentIndexName/**/.@MemberName;
                #endregion PUSH Member
                #region IF IsNull
                if (@IndexName != null)
                {
                    #endregion IF IsNull
                    #endregion LOOP Members
                    /*IF:IsMethodReturn*/
                    return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/@IndexName/**/.@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
                    #region LOOP Members
                    #region IF IsNull
                }
                #endregion IF IsNull
                #endregion LOOP Members
                #region IF IsAnyNull
                #region LOOP Parameters
                #region IF IsOut
                @ParameterName = /*NOTE*/(ParameterTypeRefName)(object)/*NOTE*/default(@ParameterType.FullName);
                #endregion IF IsOut
                #endregion LOOP Parameters
                #region IF IsMethodReturn
                return default(@MemberType.FullName);
                #endregion IF IsMethodReturn
                #endregion IF IsAnyNull
                #endregion NAME CACHEMETHOD
            }
            #endregion IF Identity
            #region NOT Identity
            /// <summary>
            /// @XmlDocument
            /// </summary>
            #region IF IsSinglePrimaryKey
            #region IF PrimaryKey0.XmlDocument
            /// <param name="@PrimaryKeyName">@PrimaryKey0.XmlDocument</param>
            #endregion IF PrimaryKey0.XmlDocument
            #endregion IF IsSinglePrimaryKey
            #region NOT IsSinglePrimaryKey
            /// <param name="key">关键字</param>
            #endregion NOT IsSinglePrimaryKey
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            [AutoCSer.Sql.RemoteMember(MemberName = @"@CacheMemberName")]
            [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.OutputSerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
            private static @MethodReturnType.FullName @RemoteCacheMethodName(@PrimaryKeyType @PrimaryKeyName/*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                @Type.FullName value = @PrimaryKeyCacheName[@PrimaryKeyName];
                #region FROMNAME CACHEMETHOD
                #endregion FROMNAME CACHEMETHOD
                #region NOTE
                return null;
                #endregion NOTE
            }
            #endregion NOT Identity
            #endregion PUSH Method
            #endregion LOOP RemoteCaches
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
