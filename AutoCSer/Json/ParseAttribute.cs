using System;
using AutoCSer.Metadata;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析类型配置
    /// </summary>
    public class ParseAttribute : MemberFilterAttribute.Instance
    {
        /// <summary>
        /// 是否作用与派生类型，默认为 true
        /// </summary>
        public bool IsBaseType = true;
    }
}
