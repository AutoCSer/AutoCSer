using System;

namespace AutoCSer.Config
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class MemberAttribute : Attribute
    {
        /// <summary>
        /// 配置名称，默认为空字符串表示默认名称，null 表示使用定义名称
        /// </summary>
        public string Name = string.Empty;
    }
}
