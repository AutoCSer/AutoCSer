using System;
#pragma warning disable 162

namespace AutoCSer.CodeGenerator.Template
{
    class SqlRemotePrimaryKey : Pub
    {
        #region PART CLASS
        [AutoCSer.Json.Serialize(Filter = AutoCSer.Metadata.MemberFilters.PublicInstanceField)]
        [AutoCSer.Json.Parse(Filter = AutoCSer.Metadata.MemberFilters.PublicInstanceField)]
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
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
                    return /*IF:IsMethodReturn*/ /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value/*LOOP:NextParameters*/, @ParameterName/*LOOP:NextParameters*/);
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
                        return /*NOTE*/(AutoCSer.Net.TcpServer.AwaiterBox<MemberType.FullName>)/*NOTE*/TcpCall.@TypeName/*PUSH:Method*/.@StaticMethodName/*PUSH:Method*/(Value);
                    }
                }
                #endregion NOT IsMethod
                #endregion PUSH Method
                #endregion LOOP RemoteMethods
            }
            /// <summary>
            /// 远程对象扩展
            /// </summary>
            [AutoCSer.BinarySerialize.IgnoreMember]
            [AutoCSer.Json.IgnoreMember]
            public RemoteExtension Remote
            {
                get { return new RemoteExtension { Value = /*NOTE*/(Type.FullName)(object)/*NOTE*/this }; }
            }
            #endregion IF RemoteMethods.Length

            #region LOOP RemoteMembers
            #region PUSH Member
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="value">@Type.XmlDocument</param>
            /// <returns>@XmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Sql.RemoteMethod(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Sql.TcpMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MemberType.FullName @GetMemberName(@Type.FullName value)
            {
                return /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@MemberName;
            }
            #endregion PUSH Member
            #region PUSH Method
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="value">@Type.XmlDocument</param>
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Sql.RemoteMethod(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Sql.RemoteMember(MemberName = @"@MemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Sql.TcpMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MethodReturnType.FullName @RemoteMethodName(@Type.FullName value/*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
                /*IF:IsMethodReturn*/
                return /*IF:IsMethodReturn*//*NOTE*/(@MethodReturnType.FullName)/*NOTE*/value.@MethodName(/*LOOP:Parameters*/@ParameterJoinName/*LOOP:Parameters*/);
            }
            #endregion PUSH Method
            #endregion LOOP RemoteMembers

            #region LOOP RemoteCaches
            #region PUSH Member
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="value">@Type.XmlDocument</param>
            #region LOOP PropertyParameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP PropertyParameters
            /// <returns>@XmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Sql.RemoteMethod(MemberName = @"@CacheMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Sql.RemoteMember(MemberName = @"@CacheMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Sql.TcpMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MemberType.FullName @GetCacheMemberName(@Type.FullName value/*LOOP:PropertyParameters*/, @ParameterTypeRefName @ParameterName/*LOOP:PropertyParameters*/)
            {
                #region LOOP Members
                #region PUSH Member
                @MemberType.FullName @IndexName = /*NOTE*/(@MemberType.FullName)/*NOTE*/@ParentIndexName/**/.@MemberName;
                #endregion PUSH Member
                #region IF IsNull
                if (@IndexName != null)
                {
                    #endregion IF IsNull
                    #endregion LOOP Members
                    return /*NOTE*/(@MemberType.FullName)/*NOTE*/@IndexName/*NOT:PropertyParameters.Length*/.@MemberName/*NOT:PropertyParameters.Length*//*IF:PropertyParameters.Length*/[/*LOOP:PropertyParameters*/@ParameterJoinName/*LOOP:PropertyParameters*/]/*IF:PropertyParameters.Length*/;
                    #region LOOP Members
                    #region IF IsNull
                }
                #endregion IF IsNull
                #endregion LOOP Members
                #region IF IsAnyNull
                return default(@MemberType.FullName);
                #endregion IF IsAnyNull
            }
            #endregion PUSH Member
            #region PUSH Method
            /// <summary>
            /// @XmlDocument
            /// </summary>
            /// <param name="value">@Type.XmlDocument</param>
            #region LOOP Parameters
            #region IF XmlDocument
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion IF XmlDocument
            #endregion LOOP Parameters
            /// <returns>@ReturnXmlDocument</returns>
            #region NOT Attribute.IsCancel
            #region IF AttributeIsMethod
            [AutoCSer.Sql.RemoteMethod(MemberName = @"@CacheMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion IF AttributeIsMethod
            #region NOT AttributeIsMethod
            [AutoCSer.Sql.RemoteMember(MemberName = @"@CacheMemberName"/*NOT:Attribute.IsAwait*/, IsAwait = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT AttributeIsMethod
            [AutoCSer.Sql.TcpMethod(/*NOT:Attribute.IsAwait*/IsClientAwaiter = false/*NOT:Attribute.IsAwait*/)]
            #endregion NOT Attribute.IsCancel
            private static @MethodReturnType.FullName @RemoteCacheMethodName(@Type.FullName value/*LOOP:Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Parameters*/)
            {
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
            }
            #endregion PUSH Method
            #endregion LOOP RemoteCaches
        }
        #endregion PART CLASS
    }
}
