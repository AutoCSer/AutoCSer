using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 代码生成器配置
    /// </summary>
    internal sealed class GeneratorAttribute : Attribute
    {
        /// <summary>
        /// 安装名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 自动安装依赖,指定当前安装必须后于依赖安装
        /// </summary>
        public Type DependType;
        /// <summary>
        /// 模板文件名称，不包括扩展名
        /// </summary>
        public string Template;
        /// <summary>
        /// 代码生成语言
        /// </summary>
        public CodeLanguage Language = CodeLanguage.CSharp;
        /// <summary>
        /// 是否自动生成
        /// </summary>
        public bool IsAuto;
        /// <summary>
        /// 是否支持 .NET 2.0
        /// </summary>
        public bool IsDotNet2 = true;
        /// <summary>
        /// 是否支持 MONO
        /// </summary>
        public bool IsMono = true;
        /// <summary>
        /// 是否生成模板代码
        /// </summary>
        public bool IsTemplate = true;
        /// <summary>
        /// 模板文件是否使用嵌套类型名称
        /// </summary>
        public bool IsDeclaringTypeName = true;
        /// <summary>
        /// 获取模板文件名，不包括扩展名
        /// </summary>
        /// <param name="type">模板数据视图</param>
        /// <returns>模板文件名</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetFileName(Type type)
        {
            return Template ?? (IsDeclaringTypeName ? type.DeclaringType.Name : type.Name);
        }
    }
}
