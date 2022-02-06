using AutoCSer.Metadata;
using System;

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化类型配置
    /// </summary>
    public class JsonSerializeAttribute : MemberFilterAttribute.PublicInstance
    {
        /// <summary>
        /// 匿名类型序列化配置
        /// </summary>
        internal static readonly JsonSerializeAttribute AnonymousTypeMember = new JsonSerializeAttribute { IsBaseType = false, Filter = MemberFilters.InstanceField };
        /// <summary>
        /// 是否作用与派生类型，默认为 true
        /// </summary>
        public bool IsBaseType = true;
    }
}
