using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 代码生成接口
    /// </summary>
    internal interface IGenerator
    {
        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否生成成功</returns>
        bool Run(ProjectParameter parameter);
    }
}
