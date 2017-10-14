using System;
using System.Reflection;
using AutoCSer.Metadata;

namespace AutoCSer.OpenAPI.Weixin.Emit
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Pub
    {
        /// <summary>
        /// 获取属性成员集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="memberAttribute"></typeparam>
        /// <param name="memberFilter"></param>
        /// <param name="isAllMember"></param>
        /// <param name="isGet">是否必须可读</param>
        /// <param name="isSet">是否必须可写</param>
        /// <returns>属性成员集合</returns>
        internal static LeftArray<PropertyIndex> GetProperties<valueType, memberAttribute>(MemberFilters memberFilter, bool isAllMember, bool isGet, bool isSet)
            where memberAttribute : IgnoreMemberAttribute
        {
            PropertyIndex[] propertyIndexs = AutoCSer.Metadata.MemberIndexGroup<valueType>.GetProperties(memberFilter);
            LeftArray<PropertyIndex> properties = new LeftArray<PropertyIndex>(propertyIndexs.Length);
            foreach (PropertyIndex property in propertyIndexs)
            {
                if ((!isGet || property.CanGet) && (!isSet || property.CanSet))
                {
                    Type type = property.Member.PropertyType;
                    if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !property.IsIgnore && !typeof(Delegate).IsAssignableFrom(type))
                    {
                        memberAttribute attribute = property.GetAttribute<memberAttribute>(true);
                        if (isAllMember ? (attribute == null || attribute.IsSetup) : (attribute != null && attribute.IsSetup))
                        {
                            properties.Add(property);
                        }
                    }
                }
            }
            return properties;
        }
    }
}
