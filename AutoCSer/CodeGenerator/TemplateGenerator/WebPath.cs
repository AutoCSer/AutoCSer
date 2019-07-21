using System;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// WEB Path 代码生成
    /// </summary>
    internal abstract partial class WebPath
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public const string AutoCSerPath = "AutoCSerPath";
        /// <summary>
        /// WEB Path 代码生成
        /// </summary>
        internal abstract class Generator<cSharpType> : MemberGenerator<AutoCSer.WebView.PathAttribute>
            where cSharpType : Generator<cSharpType>, new()
        {
            /// <summary>
            /// WEB Path 代码生成
            /// </summary>
            internal static readonly cSharpType Default = new cSharpType();
            /// <summary>
            /// Path 成员
            /// </summary>
            internal struct PathMember
            {
                /// <summary>
                /// 成员信息
                /// </summary>
                public AutoCSer.CodeGenerator.Metadata.MemberIndex Member;
                /// <summary>
                /// Path
                /// </summary>
                public string Path;
                /// <summary>
                /// 其他查询前缀
                /// </summary>
                public string OtherQuery;
                /// <summary>
                /// 查询名称
                /// </summary>
                public string QueryName;
                /// <summary>
                /// 是否 Id 传参
                /// </summary>
                public bool IsIdentity;
                /// <summary>
                /// 是否 #! 查询
                /// </summary>
                public bool IsHash;
            }
            /// <summary>
            /// 输出文件名称
            /// </summary>
            private string outputFileName;
            /// <summary>
            /// 代码
            /// </summary>
            protected StringArray code = new StringArray();
            /// <summary>
            /// Path 成员
            /// </summary>
            internal LeftArray<PathMember> PathMembers;
            /// <summary>
            /// 输出文件扩展名称
            /// </summary>
            protected abstract string outputFileExtensionName { get; }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                PathMembers.Length = 0;
                object pathValue = typeof(AutoCSer.Emit.Constructor<>).MakeGenericType(Type).GetMethod("Default", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, NullValue<Type>.Array, null).Invoke(null, null);
                string queryName = (Attribute.QueryName ?? (Type.Type.Name + "Id")), query;
                FieldInfo idField = Type.Type.GetField("Id", BindingFlags.Instance | BindingFlags.Public);
                if (idField == null || idField.FieldType != typeof(int)) query = queryName + "=0";
                else
                {
                    idField.SetValue(pathValue, 1);
                    query = queryName + "=1";
                }
                foreach (AutoCSer.CodeGenerator.Metadata.MemberIndex member in AutoCSer.CodeGenerator.Metadata.MethodIndex.GetMembers<AutoCSer.WebView.PathMemberAttribute>(Type, Attribute.MemberFilters, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute))
                {
                    if (member.MemberSystemType == typeof(string))
                    {
                        string url = (string)(member.Member as PropertyInfo).GetValue(pathValue, null);
                        if (url.EndsWith(query, StringComparison.Ordinal))
                        {
                            PathMembers.Add(new PathMember { Member = member, Path = url.Substring(0, url.Length - query.Length), QueryName = queryName, IsHash = false, IsIdentity = true });
                        }
                        else PathMembers.Add(new PathMember { Member = member, Path = url });
                    }
                    else if (member.MemberSystemType == typeof(AutoCSer.WebView.HashUrl))
                    {
                        AutoCSer.WebView.HashUrl url = (AutoCSer.WebView.HashUrl)(member.Member as PropertyInfo).GetValue(pathValue, null);
                        if (url.Query == query) PathMembers.Add(new PathMember { Member = member, Path = url.Path, QueryName = queryName, IsHash = true, IsIdentity = true });
                        else if (url.Query.EndsWith(query, StringComparison.Ordinal))
                        {
                            PathMembers.Add(new PathMember { Member = member, Path = url.Path, OtherQuery = url.Query.Substring(0, url.Query.Length - query.Length), QueryName = queryName, IsHash = true, IsIdentity = true });
                        }
                    }
                }
                if (PathMembers.Length != 0)
                {
                    _code_.Length = 0;
                    create(false);
                    code.Add(_code_);
                }
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
                string webPathCode = @"//本文件由程序自动生成,请不要自行修改
" + onCreatedCode;
                code.Length = 0;
                if (Coder.WriteFileSuffix(outputFileName, webPathCode)) Messages.Message(outputFileName + " 被修改");
            }
            /// <summary>
            /// 安装完成处理代码
            /// </summary>
            protected abstract string onCreatedCode { get; }
            /// <summary>
            /// 安装入口
            /// </summary>
            /// <param name="exportPathType">导出引导类型</param>
            /// <param name="outputFileName"></param>
            /// <returns>是否安装成功</returns>
            public bool Run(Type exportPathType, string outputFileName)
            {
                AutoCSer.WebView.PathAttribute exportWebPath = exportPathType.customAttribute<AutoCSer.WebView.PathAttribute>();
                if (exportWebPath == null) Messages.Message("没有找到 path 导出信息 " + exportPathType.fullName());
                else if (exportWebPath.Flag == 0) Messages.Message("缺少导出二进制位标识 " + exportPathType.fullName());
                else
                {
                    LeftArray<KeyValue<Type, AutoCSer.WebView.PathAttribute>> types = new LeftArray<KeyValue<Type, AutoCSer.WebView.PathAttribute>>();
                    foreach (Type type in exportPathType.Assembly.GetTypes())
                    {
                        AutoCSer.WebView.PathAttribute webPath = type.customAttribute<AutoCSer.WebView.PathAttribute>();
                        if (webPath != null && (webPath.Flag & exportWebPath.Flag) == exportWebPath.Flag) types.Add(new KeyValue<Type, AutoCSer.WebView.PathAttribute>(type, webPath));
                    }
                    foreach (KeyValue<Type, AutoCSer.WebView.PathAttribute> type in types.Sort((left, right) => string.CompareOrdinal(left.Key.FullName, right.Key.FullName)))
                    {
                        this.Type = type.Key;
                        Attribute = type.Value;
                        nextCreate();
                    }
                    this.outputFileName = outputFileName + outputFileExtensionName;
                    onCreated();
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// WEB Path JavaScript 代码生成
        /// </summary>
        [Generator(Name = "WEB Path JavaScript", DependType = typeof(Html), Language = CodeLanguage.JavaScript)]
        internal sealed partial class JavaScript : Generator<JavaScript>
        {
            /// <summary>
            /// 输出文件扩展名称
            /// </summary>
            protected override string outputFileExtensionName { get { return ".js"; } }
            /// <summary>
            /// 命名空间
            /// </summary>
            public string Namespace
            {
                get { return AutoCSerPath; }
            }
            /// <summary>
            /// 安装完成处理代码
            /// </summary>
            protected override string onCreatedCode
            {
                get
                {
                    return "var " + AutoCSerPath + @"={
" + code.ToString() + @"_:0};";
                }
            }
        }
        /// <summary>
        /// WEB Path TypeScript 代码生成
        /// </summary>
        [Generator(Name = "WEB Path TypeScript", DependType = typeof(Html), Language = CodeLanguage.TypeScript)]
        internal sealed partial class TypeScript : Generator<TypeScript>
        {
            /// <summary>
            /// 输出文件扩展名称
            /// </summary>
            protected override string outputFileExtensionName { get { return ".ts"; } }
            /// <summary>
            /// 安装完成处理代码
            /// </summary>
            protected override string onCreatedCode
            {
                get
                {
                    return @"namespace AutoCSerPath {
" + code.ToString() + @"}";
                }
            }
        }
    }
}
