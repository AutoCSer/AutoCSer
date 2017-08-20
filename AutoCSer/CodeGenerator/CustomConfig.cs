using System;
using System.IO;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 自定义配置
    /// </summary>
    internal sealed class CustomConfig
    {
        /// <summary>
        /// 是否调用 AutoCSer 的代码生成组件
        /// </summary>
        internal bool IsAutoCSer = true;

        /// <summary>
        /// 自定义配置
        /// </summary>
        internal static readonly CustomConfig Default;
        /// <summary>
        /// 自定义代码生成程序集名称
        /// </summary>
        internal const string CustomAssemblyName = "AutoCSer.CodeGenerator.Custom";
        /// <summary>
        /// 自定义配置文件
        /// </summary>
        private const string configFileName = "AutoCSer.CodeGenerator.CustomConfig";
        /// <summary>
        /// 程序集
        /// </summary>
        internal static readonly Assembly Assembly;

        static CustomConfig()
        {
            FileInfo jsonFile = new FileInfo(configFileName + ".json");
            if (jsonFile.Exists) Default = AutoCSer.Json.Parser.Parse<CustomConfig>(File.ReadAllText(jsonFile.FullName));
            else if (File.Exists(configFileName + ".xml")) Default = AutoCSer.Xml.Parser.Parse<CustomConfig>(File.ReadAllText(configFileName + ".xml"));
            else if (jsonFile.Directory.Name == "Release" && jsonFile.Directory.Parent.Name == "bin")
            {
#if DotNetStandard
                string fileName = jsonFile.Directory.Parent.Parent.Parent.fullName() + configFileName;
#else
                string fileName = jsonFile.Directory.Parent.Parent.Parent.fullName() + configFileName;
#endif
                if (File.Exists(fileName + ".json")) Default = AutoCSer.Json.Parser.Parse<CustomConfig>(File.ReadAllText(fileName + ".json"));
                else if (File.Exists(fileName + ".xml")) Default = AutoCSer.Xml.Parser.Parse<CustomConfig>(File.ReadAllText(fileName + ".xml"));
            }
            if (Default == null) Default = new CustomConfig();
            FileInfo assemblyFile = new FileInfo(new FileInfo(ProjectParameter.CurrentAssembly.Location).Directory.fullName() + CustomAssemblyName + ".dll");
            if (assemblyFile.Exists) Assembly = Assembly.LoadFrom(assemblyFile.FullName);
        }
    }
}
