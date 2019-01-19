using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTTP 调用函数配置
    /// </summary>
    public sealed partial class CallAttribute : Attribute
    {
        /// <summary>
        /// 默认为 false 表示代码生成选择所有函数，否则仅选择申明了 AutoCSer.WebView.CallAttribute 的函数，有效域为当前 class。
        /// </summary>
        public bool IsAttribute;
        /// <summary>
        /// 指定是否搜索该成员的继承链以查找这些特性，参考System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)，有效域为当前 class。
        /// </summary>
        public bool IsBaseTypeAttribute;
    }
}
