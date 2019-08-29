using System;

namespace AutoCSer.Config
{
    /// <summary>
    /// 配置容器类型
    /// </summary>
    public sealed class TypeAttribute : Attribute
    {
        /// <summary>
        /// 是否实例模式，默认为 false 表示静态模式
        /// </summary>
        public bool IsInstance;
    }
}
