using AutoCSer.Metadata;
using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 序列化类型配置
    /// </summary>
    public class SerializeAttribute : MemberFilterAttribute.PublicInstance
    {
        /// <summary>
        /// 匿名类型序列化配置
        /// </summary>
        internal static readonly SerializeAttribute AnonymousTypeMember = new SerializeAttribute { IsBaseType = false, Filter = MemberFilters.InstanceField };
        /// <summary>
        /// 是否作用与派生类型，默认为 true
        /// </summary>
        public bool IsBaseType = true;
    }
}
