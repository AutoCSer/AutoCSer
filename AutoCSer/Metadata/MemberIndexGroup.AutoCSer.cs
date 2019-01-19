using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员索引分组
    /// </summary>
    internal sealed partial class MemberIndexGroup
    {
        /// <summary>
        /// 获取成员索引集合
        /// </summary>
        /// <param name="isValue">成员匹配委托</param>
        /// <returns>成员索引集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private MemberIndexInfo[] get(Func<MemberIndexInfo, bool> isValue)
        {
            return AutoCSer.Extension.ArrayExtension.concat(PublicFields.getFindArray(isValue), NonPublicFields.getFindArray(isValue), PublicProperties.getFindArray(isValue), NonPublicProperties.getFindArray(isValue));
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="filter">选择类型</param>
        /// <param name="isFilter">是否完全匹配选择类型</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal MemberIndexInfo[] Find(MemberFilters filter, bool isFilter = false)
        {
            return get(value => isFilter ? (value.MemberFilters & filter) == filter : ((value.MemberFilters & filter) != 0));
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="filter">成员选择</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal MemberIndexInfo[] Find<attributeType>(MemberFilterAttribute filter) where attributeType : IgnoreMemberAttribute
        {
            return Find(filter.MemberFilters).getFindArray(value => filter.IsAttribute ? value.IsAttribute<attributeType>(filter.IsBaseTypeAttribute) : !value.IsIgnoreAttribute<attributeType>(filter.IsBaseTypeAttribute));
        }
        /// <summary>
        /// 获取匹配成员集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="isFilter">是否完全匹配选择类型</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">指定是否搜索该成员的继承链以查找这些特性，参考System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)。</param>
        /// <returns>匹配成员集合</returns>
        internal static MemberIndexInfo[] Get<attributeType>(Type type, MemberFilters filter, bool isFilter, bool isAttribute, bool isBaseType)
             where attributeType : IgnoreMemberAttribute
        {
            return Get(type).Find(filter, isFilter).getFindArray(value => isAttribute ? value.IsAttribute<attributeType>(isBaseType) : !value.IsIgnoreAttribute<attributeType>(isBaseType));
        }
    }
}
