using System;
using System.IO;
using AutoCSer.Extensions;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 自定义简单组合模板参数
    /// </summary>
    //internal sealed class CombinationTemplateConfig : AutoCSer.CodeGenerator.CombinationTemplateConfig
    {
        /// <summary>
        /// JSON 自定义简单组合模板参数
        /// </summary>
        public CombinationTemplateConfig()
        {
            TemplatePath = (@"\Json\CombinationTemplate\").pathSeparator();
            CodeFileName = typeof(CombinationTemplateConfig).Namespace;
        }
    }
}
