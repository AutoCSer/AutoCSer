using System;
using System.Reflection;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 关键字远程成员映射 代码生成
    /// </summary>
    internal abstract partial class SqlRemotePrimaryKey
    {
        /// <summary>
        /// 关键字远程成员映射 代码生成
        /// </summary>
        [Generator(Name = "关键字远程成员映射", DependType = typeof(CSharper), IsAuto = true, IsDotNet2 = false, IsMono = false)]
        internal partial class Generator : SqlTable.Generator<AutoCSer.Sql.RemotePrimaryKeyAttribute>
        {
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                LeftArray<RemoteMethod> remoteMethods = new LeftArray<RemoteMethod>();
                foreach (MethodIndex method in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.MethodAttribute>(Type, AutoCSer.Metadata.MemberFilters.Static, false, true, false))
                {
                    if (!method.Method.IsGenericMethodDefinition)
                    {
                        AutoCSer.Sql.RemoteMemberAttribute remoteAttribute = method.GetSetupAttribute<AutoCSer.Sql.RemoteMemberAttribute>(false);
                        if (remoteAttribute != null)
                        {
                            MethodParameter[] parameters = method.Parameters;
                            if (parameters.Length >= 1 && parameters[0].ParameterType == Type.Type) remoteMethods.Add(new RemoteMethod { Type = Type, ProjectParameter = AutoParameter, Method = method, MethodParameters = parameters, Attribute = remoteAttribute });
                        }
                    }
                }
                RemoteMethods = remoteMethods.ToArray();

                setRemoteMemberCache();

                if ((RemoteMethods.Length | RemoteMembers.Length | RemoteCaches.Length) != 0) create(true);
            }
        }
    }
}
