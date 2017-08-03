using System;

namespace AutoCSer.CodeGenerator.CustomApply
{
    /// <summary>
    /// 自定义代码生成 示例数据
    /// </summary>
    [AutoCSer.CodeGenerator.Custom.Attribute.ClearFields]
    internal partial class Data 
    {
        /// <summary>
        /// 测试数据
        /// </summary>
        private int PrivateField; 
        /// <summary>
        /// 测试数据
        /// </summary>
        public int PublicField;
    }
}
