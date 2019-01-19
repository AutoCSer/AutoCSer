using System;
using AutoCSer.Metadata;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析成员配置
    /// </summary>
    public class ParseMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 是否默认解析成员
        /// </summary>
        public bool IsDefault;
        ///// <summary>
        ///// JSON 解析成员配置
        ///// </summary>
        //public ParseMemberAttribute() { }
        ///// <summary>
        ///// JSON 解析成员配置
        ///// </summary>
        ///// <param name="isIgnoreCurrent">是否禁止当前安装</param>
        //internal ParseMemberAttribute(bool isIgnoreCurrent)
        //{
        //    IsIgnoreCurrent = isIgnoreCurrent;
        //}
    }
}
