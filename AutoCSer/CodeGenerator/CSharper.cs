using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// CSharp代码生成数据视图生成
    /// </summary>
    [Generator(Name = "C#", IsAuto = true, Template = null, IsTemplate = false)]
    internal sealed class CSharper : IGenerator
    {
        /// <summary>
        /// 类定义生成
        /// </summary>
        private sealed class Definition
        {
            /// <summary>
            /// 类型
            /// </summary>
            public Type Type;
            /// <summary>
            /// 安装属性
            /// </summary>
            public GeneratorAttribute Auto;
            /// <summary>
            /// 安装参数
            /// </summary>
            public ProjectParameter Parameter;
            /// <summary>
            /// 类定义生成
            /// </summary>
            private TemplateGenerator.CSharpTypeDefinition typeDefinition;
            /// <summary>
            /// 模板代码生成器
            /// </summary>
            private Coder coder;
            /// <summary>
            /// 模板代码
            /// </summary>
            private StringArray codeBuilder;
            /// <summary>
            /// 生成类定义字符串
            /// </summary>
            /// <returns>类定义字符串</returns>
            public override string ToString()
            {
                typeDefinition = new TemplateGenerator.CSharpTypeDefinition(Type, true, true);
                (coder = new Coder(Parameter, Type, Auto.Language)).SkinEnd(coder.GetNode(Auto.GetFileName(Type)));
                codeBuilder = new StringArray();
                if (!Auto.IsDotNet2)
                {
                    codeBuilder.Append(@"
#if DOTNET2
#else");
                }
                if (!Auto.IsMono)
                {
                    codeBuilder.Append(@"
#if MONO
#else");
                }
                codeBuilder.Append(@"
", typeDefinition.Start, @"
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name=""isOut"">是否输出代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguage.", Auto.Language.ToString(), @", _isOut_))
            {
                ");
                switch (Auto.Language)
                {
                    case CodeLanguage.JavaScript:
                    case CodeLanguage.TypeScript:
                        return javaScript();
                    default: return cSharp();
                }
            }
            /// <summary>
            /// 模板结束
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private string end()
            {
                codeBuilder.Append(@"
                if (_isOut_) outEnd();
            }
        }", typeDefinition.End);
                if (!Auto.IsDotNet2)
                {
                    codeBuilder.Append(@"
#endif");
                }
                if (!Auto.IsMono)
                {
                    codeBuilder.Append(@"
#endif");
                }
                return codeBuilder.ToString();
            }
            /// <summary>
            /// 生成C#模板代码
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private string cSharp()
            {
                codeBuilder.Append(coder.PartCodes["CLASS"]);
                return end();
            }
            /// <summary>
            /// 生成JavaScript模板代码
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private string javaScript()
            {
                codeBuilder.Add(coder.Code);
                return end();
            }
        }
        /// <summary>
        /// 安装入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否安装成功</returns>
        public bool Run(ProjectParameter parameter)
        {
            if (parameter != null)
            {
                if (parameter.IsAutoCSerCodeGenerator || parameter.IsCustomCodeGenerator)
                {
                    LeftArray<Definition> definitions = (parameter.IsCustomCodeGenerator ? parameter.Assembly : ProjectParameter.CurrentAssembly).GetTypes().getArray(type => new Definition { Type = type, Auto = type.customAttribute<GeneratorAttribute>(), Parameter = parameter })
                        .getFind(type => type.Auto != null && type.Auto.IsTemplate)// && type.Auto.DependType == typeof(cSharper)
                        .Sort((left, right) => string.CompareOrdinal(left.Type.FullName, right.Type.FullName));
                    LeftArray<string> codes = new LeftArray<string>(definitions.Length);
                    foreach (Definition definition in definitions)
                    {
                        codes.Add(definition.ToString());
                        if (Messages.IsError) return false;
                    }
#if DotNetStandard
                    string path = new System.IO.FileInfo(parameter.AssemblyPath).Directory.fullName();
                    copyDotNetCoreJson(path, "AutoCSer.CodeGenerator.deps.json");
                    copyDotNetCoreJson(path, "AutoCSer.CodeGenerator.runtimeconfig.dev.json");
                    copyDotNetCoreJson(path, "AutoCSer.CodeGenerator.runtimeconfig.json");
#endif
                    string fileName = parameter.ProjectPath + @"{AutoCSer}.CSharper.cs";
                    if (Coder.WriteFile(fileName, Coder.WarningCode + string.Concat(codes.ToArray()) + Coder.FileEndCode))
                    {
                        Messages.Add(fileName + " 被修改");
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
#if DotNetStandard
        /// <summary>
        /// 复制 JSON 文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        private void copyDotNetCoreJson(string path, string fileName)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(AutoCSer.PubPath.ApplicationPath + fileName);
            if (file.Exists) file.CopyTo(path + fileName, true);
        }
#endif
    }
}
