using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 禁止安装属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
