using System;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 自定义属性字段模板生成基类
    /// </summary>
    /// <typeparam name="attributeType">自定义属性类型</typeparam>
    internal abstract class MemberGenerator<attributeType> : Generator<attributeType>
        where attributeType : AutoCSer.Metadata.MemberFilterAttribute
    {
        /// <summary>
        /// 安装完成处理
        /// </summary>
        protected override void onCreated() { }
    }
}
