using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员索引
    /// </summary>
    internal abstract partial class MemberIndexInfo
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        protected MemberIndexInfo(MemberIndexInfo member)
        {
            Member = member.Member;
            MemberSystemType = member.MemberSystemType;
            MemberIndex = member.MemberIndex;
            MemberFilters = member.MemberFilters;
            IsField = member.IsField;
            CanSet = member.CanSet;
            CanGet = member.CanGet;
            isIgnore = member.isIgnore;
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <typeparam name="memberType">成员索引类型</typeparam>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="members">待匹配的成员信息集合</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static memberType[] find<memberType, attributeType>(memberType[] members, bool isAttribute, bool isBaseType)
            where memberType : MemberIndexInfo
            where attributeType : AutoCSer.Metadata.IgnoreMemberAttribute
        {
            return members.getFindArray(value => isAttribute ? value.IsAttribute<attributeType>(isBaseType) : !value.IsIgnoreAttribute<attributeType>(isBaseType));
        }
    }
}
