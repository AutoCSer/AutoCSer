using System;

namespace AutoCSer.CodeGenerator.Custom
{
    /// <summary>
    /// 清除字段数据 代码生成
    /// </summary>
    [Generator(Name = "清除字段数据", DependType = typeof(CSharper), IsAuto = true, IsDeclaringTypeName = false)]
    internal partial class ClearFields : AutoCSer.CodeGenerator.TemplateGenerator.Generator<AutoCSer.CodeGenerator.Custom.Attribute.ClearFieldsAttribute>
    {
        /// <summary>
        /// 成员集合
        /// </summary>
        AutoCSer.CodeGenerator.Metadata.MemberIndex[] Members;
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override void nextCreate()
        {
            Members = AutoCSer.CodeGenerator.Metadata.MemberIndex.GetMembers(Type, AutoCSer.Metadata.MemberFilters.Instance);
            create(true);
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        protected override void onCreated() { }
    }
}
