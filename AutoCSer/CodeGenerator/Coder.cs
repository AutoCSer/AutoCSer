using System;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 模板代码生成器
    /// </summary>
    internal sealed class Coder : TreeTemplate<TreeBuilder.Node>
    {
        /// <summary>
        /// 模板文件路径
        /// </summary>
        private static readonly string templatePath = (@"Template\").pathSeparator();
        /// <summary>
        /// 模板命令类型
        /// </summary>
        internal enum Command
        {
            /// <summary>
            /// 输出绑定的数据
            /// </summary>
            AT,
            /// <summary>
            /// 子代码段
            /// </summary>
            PUSH,
            /// <summary>
            /// 循环
            /// </summary>
            LOOP,
            /// <summary>
            /// 循环=LOOP
            /// </summary>
            FOR,
            /// <summary>
            /// 屏蔽代码段输出
            /// </summary>
            NOTE,
            /// <summary>
            /// 绑定的数据为true非0非null时输出代码
            /// </summary>
            IF,
            /// <summary>
            /// 绑定的数据为false或者0或者null时输出代码
            /// </summary>
            NOT,
            /// <summary>
            /// 用于标识一个子段模板，可以被别的模板引用
            /// </summary>
            NAME,
            /// <summary>
            /// 引用NAME标识一个子段模板
            /// </summary>
            FROMNAME,
            /// <summary>
            /// 用于标识一个子段程序代码，用于代码的分类输出
            /// </summary>
            PART,
        }
        /// <summary>
        /// 安装参数
        /// </summary>
        private ProjectParameter parameter;
        /// <summary>
        /// 模板文件扩展名
        /// </summary>
        private string extensionName;
        /// <summary>
        /// CSharp代码生成器
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <param name="type">模板数据视图</param>
        /// <param name="language">语言</param>
        public Coder(ProjectParameter parameter, Type type, CodeLanguage language)
            : base(type, Messages.Add, Messages.Message)
        {
            this.parameter = parameter;
            extensionName = "." + EnumAttribute<CodeLanguage, CodeLanguageAttribute>.Array((int)(byte)language).ExtensionName;
            creators[Command.NOTE.ToString()] = note;
            creators[Command.LOOP.ToString()] = creators[Command.FOR.ToString()] = loop;
            creators[Command.AT.ToString()] = at;
            creators[Command.PUSH.ToString()] = push;
            creators[Command.IF.ToString()] = ifThen;
            creators[Command.NOT.ToString()] = not;
            creators[Command.NAME.ToString()] = name;
            creators[Command.FROMNAME.ToString()] = fromName;
            creators[Command.PART.ToString()] = part;
        }
        /// <summary>
        /// 根据类型名称获取子段模板
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="name">子段模板名称</param>
        /// <returns>子段模板</returns>
        protected override TreeBuilder.Node fromNameNode(string typeName, ref SubString name)
        {
            TreeBuilder.Node node = GetNode(typeName);
            if (node != null)
            {
                node = node.GetFirstNodeByTag(Command.NAME, ref name);
                if (node == null) Messages.Add("模板文件 " + getTemplateFileName(typeName) + " 未找到NAME " + name.ToString());
            }
            return node;
        }
        /// <summary>
        /// 添加代码树节点
        /// </summary>
        /// <param name="node">代码树节点</param>
        internal void SkinEnd(TreeBuilder.Node node)
        {
            skin(node);
            pushCode();
        }
        /// <summary>
        /// 根据类型名称获取CSharp代码树节点
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>CSharp代码树节点</returns>
        internal TreeBuilder.Node GetNode(string fileName)
        {
            TreeBuilder.Node node;
            if (!nodeCache.TryGetValue(fileName + extensionName, out node))
            {
                fileName = getTemplateFileName(fileName);
                if (File.Exists(fileName))
                {
                    nodeCache.Add(fileName, node = new TreeBuilder().Create(File.ReadAllText(fileName)));
                }
                else Messages.Add("未找到模板文件 " + fileName);
            }
            return node;
        }
        /// <summary>
        /// 根据类型名称获取模板文件名
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>模板文件名</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private string getTemplateFileName(string typeName)
        {
            return new DirectoryInfo(parameter.ProjectPath).fullName() + templatePath + typeName + extensionName;
        }


        /// <summary>
        /// 声明与警告+文件头
        /// </summary>
        public const string WarningCode = @"//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
";
        /// <summary>
        /// 文件结束
        /// </summary>
        public const string FileEndCode = @"
#endif";
        /// <summary>
        /// 已经生成代码的类型
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct CodeType : IEquatable<CodeType>
        {
            /// <summary>
            /// 模板类型
            /// </summary>
            public Type TemplateType;
            /// <summary>
            /// 当前生成代码的应用类型
            /// </summary>
            public Type Type;
            /// <summary>
            /// 判断生成代码类型是否相等
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(CodeType other)
            {
                return TemplateType == other.TemplateType && Type == other.Type;
            }
            /// <summary>
            /// 判断生成代码类型是否相等
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return Equals((CodeType)obj);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return TemplateType.GetHashCode() ^ Type.GetHashCode();
            }
        }
        /// <summary>
        /// 已经生成的代码
        /// </summary>
        private static readonly StringArray[] codes;
        ///// <summary>
        ///// 没有依赖的记忆代码
        ///// </summary>
        //private static StringArray rememberCodes = new StringArray();
        /// <summary>
        /// 已经生成代码的类型
        /// </summary>
        private static HashSet<CodeType> codeTypes = HashSetCreator<CodeType>.Create();
        /// <summary>
        /// CSharp代码树节点缓存
        /// </summary>
        private static Dictionary<string, TreeBuilder.Node> nodeCache = DictionaryCreator.CreateOnly<string, TreeBuilder.Node>();
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="cSharperType">模板类型</param>
        /// <param name="type">实例类型</param>
        /// <returns>锁定是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool Add(Type cSharperType, Type type)
        {
            if (codeTypes.Contains(new CodeType { TemplateType = cSharperType, Type = type })) return false;
            codeTypes.Add(new CodeType { TemplateType = cSharperType, Type = type });
            return true;
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="language">代码生成语言</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Add(string code, CodeLanguage language = CodeLanguage.CSharp)
        {
            codes[(int)(byte)language].Add(code);
        }
        /// <summary>
        /// 判断是否已经存在代码
        /// </summary>
        /// <param name="cSharperType">模板类型</param>
        /// <param name="type">实例类型</param>
        /// <returns>是否已经存在代码</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool CheckCodeType(Type cSharperType, Type type)
        {
            return codeTypes.Contains(new CodeType { TemplateType = cSharperType, Type = type });
        }
        ///// <summary>
        ///// 添加没有依赖的记忆代码
        ///// </summary>
        ///// <param name="code">没有依赖的记忆代码</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public static void AddRemember(string code)
        //{
        //    rememberCodes.Add(code);
        //}
        /// <summary>
        /// 输出代码
        /// </summary>
        public static void Output(ProjectParameter parameter)
        {
            StringArray[] builders = new StringArray[codes.Length];
            for (int index = codes.Length; index != 0; )
            {
                StringArray builder = codes[--index];
                if (builder.Length != 0)
                {
                    builders[index] = builder;
                    codes[index] = new StringArray();
                }
                CodeLanguage language = (CodeLanguage)(byte)index;
                switch (language)
                {
                    case CodeLanguage.JavaScript:
                    case CodeLanguage.TypeScript:
                        if (builders[index] != null) Messages.Add("生成了未知的 " + language + " 代码。");
                        break;
                }
            }
            //StringArray rememberCodeBuilder = null;
            //if (rememberCodes.Length != 0)
            //{
            //    rememberCodeBuilder = rememberCodes;
            //    rememberCodes = new StringArray();
            //}
            codeTypes.Clear();
            if (Messages.IsError) return;
            //string message = string.Empty;
            for (int index = builders.Length; index != 0; )
            {
                StringArray builder = builders[--index];
                if (builder != null)
                {
                    switch (index)
                    {
                        case (int)CodeLanguage.CSharp:
                            string code = builder.ToString(), AutoCSerFileName = null;
#if !DotNetStandard
                            bool isAutoCSer = false;
#endif
                            if (!string.IsNullOrEmpty(code))
                            {
                                string fileName = parameter.ProjectPath + (AutoCSerFileName = "{" + parameter.DefaultNamespace + "}.AutoCSer.cs");
                                if (WriteFile(fileName, WarningCode + code + FileEndCode))
                                {
#if !DotNetStandard
                                    isAutoCSer = true;
#endif
                                    Messages.Message(fileName + " 被修改");
                                    //message = fileName + " 被修改";
                                }
                            }
#if !DotNetStandard
                            if (parameter.IsProjectFile && isAutoCSer)
                            {
                                string projectFile = parameter.AssemblyPath + parameter.ProjectName + ".csproj";
                                if (File.Exists(projectFile))
                                {
                                    string projectXml = File.ReadAllText(projectFile, System.Text.Encoding.UTF8);
                                    if (isAutoCSer) AutoCSerFileName = @"<Compile Include=""" + AutoCSerFileName + @""" />";
                                    int fileIndex;
                                    if (isAutoCSer && (fileIndex = projectXml.IndexOf(AutoCSerFileName)) != -1) break;
                                    string csFileName = @".cs"" />
";
                                    if ((fileIndex = projectXml.IndexOf(csFileName)) != -1)
                                    {
                                        if (isAutoCSer)
                                        {
                                            AutoCSerFileName += @"
    ";
                                        }
                                        projectXml = projectXml.Insert(fileIndex + csFileName.Length, AutoCSerFileName);
                                        MoveFile(projectFile, projectXml);
                                    }
                                }
                            }
#endif
                            break;
                    }
                }
            }
            //if (message.Length != 0) AutoCSer.Log.Pub.Log.waitThrow(AutoCSer.Log.LogType.All, message);
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>是否写入新内容</returns>
        public static bool WriteFile(string fileName, string content)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    if (File.ReadAllText(fileName) != content) return MoveFile(fileName, content);
                }
                else
                {
                    File.WriteAllText(fileName, content);
                    return true;
                }
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.WaitThrow(AutoCSer.Log.LogType.All, error, "文件创建失败 : " + fileName);
            }
            return false;
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>是否写入新内容</returns>
        public static bool MoveFile(string fileName, string content)
        {
            try
            {
                AutoCSer.IO.File.MoveBak(fileName);
                File.WriteAllText(fileName, content);
                return true;
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.WaitThrow(AutoCSer.Log.LogType.All, error, "文件创建失败 : " + fileName);
            }
            return false;
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>是否写入新内容</returns>
        public static bool WriteFileSuffix(string fileName, string content)
        {
            try
            {
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    if (File.ReadAllText(fileName = file.FullName) != content)
                    {
                        string bakName = file.Directory.fullName() + AutoCSer.IO.File.BakPrefix + Date.NowTime.Set().ToString("yyyyMMdd-HHmmss") + "_" + file.Name + "." + ((uint)Random.Default.Next()).toString();
                        if (File.Exists(bakName)) File.Delete(bakName);
                        File.Move(fileName, bakName);
                        File.WriteAllText(fileName, content);
                        return true;
                    }
                }
                else
                {
                    DirectoryInfo directory = file.Directory;
                    if (!directory.Exists) directory.Create();
                    File.WriteAllText(fileName, content);
                    return true;
                }
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.WaitThrow(AutoCSer.Log.LogType.All, error, "文件创建失败 : " + fileName);
            }
            return false;
        }
        static Coder()
        {
            codes = new StringArray[EnumAttribute<CodeLanguage>.GetMaxValue(-1) + 1];
            for (int index = codes.Length; index != 0; codes[--index] = new StringArray()) ;
        }
    }
}
