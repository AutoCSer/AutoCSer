using System;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 属性索引
    /// </summary>
    internal sealed class PropertyIndex : MemberIndexInfo<PropertyInfo>
    {
        /// <summary>
        /// 属性信息
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        public PropertyIndex(PropertyInfo property, MemberFilters filter, int index)
            : base(property, filter, index)
        {
            CanSet = property.CanWrite;
            CanGet = property.CanRead;
            MemberSystemType = property.PropertyType;
        }
    }
}
