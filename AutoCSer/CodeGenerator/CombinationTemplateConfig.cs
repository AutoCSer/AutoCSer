using System;
using AutoCSer.Extension;

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
        internal string TemplatePath = (@"\CombinationTemplate\").pathSeparator();
    }
}
