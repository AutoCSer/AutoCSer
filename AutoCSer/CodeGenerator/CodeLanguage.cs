using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 代码生成语言
    /// </summary>
    internal enum CodeLanguage : byte
    {
        /// <summary>
        /// C#
        /// </summary>
        [CodeLanguage(ExtensionName = "cs")]
        CSharp,
        /// <summary>
        /// JavaScript
        /// </summary>
        [CodeLanguage(ExtensionName = "js")]
        JavaScript,
        /// <summary>
        /// TypeScript
        /// </summary>
        [CodeLanguage(ExtensionName = "ts.txt")]
        TypeScript,
    }
    /// <summary>
    /// 代码生成语言扩展
    /// </summary>
    internal sealed class CodeLanguageAttribute : Attribute
    {
        /// <summary>
        /// 模板文件扩展名
        /// </summary>
        public string ExtensionName;
    }
}
