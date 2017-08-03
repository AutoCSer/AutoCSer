using System;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 类定义生成
    /// </summary>
    internal abstract class TypeDefinition
    {
        /// <summary>
        /// 类定义开始
        /// </summary>
        protected StringArray start = new StringArray();
        /// <summary>
        /// 类定义开始
        /// </summary>
        public string Start
        {
            get
            {
                return start.ToString();
            }
        }
        /// <summary>
        /// 类定义结束
        /// </summary>
        protected StringArray end = new StringArray();
        /// <summary>
        /// 类定义结束
        /// </summary>
        public string End
        {
            get
            {
                return end.ToString();
            }
        }
    }
}
