using System;
using AutoCSer.Extension;
using System.IO;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 自定义简单组合模板参数
    /// </summary>
    internal class CombinationTemplateConfig
    {
        /// <summary>
        /// 自定义模板相对项目路径
        /// </summary>
        internal string TemplatePath = Path.DirectorySeparatorChar + "CombinationTemplate" + Path.DirectorySeparatorChar;
        /// <summary>
        /// 目标代码文件名称（默认为项目命名空间）
        /// </summary>
        internal string CodeFileName;
    }
}
