using System;
using System.Reflection;
using System.Collections.Generic;
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
        public MemberInfo Member { get; protected set; }
        /// <summary>
        /// 成员类型
        /// </summary>
        public Type MemberSystemType { get; protected set; }
        /// <summary>
        /// AutoCSer.Net.TcpServer.Awaiter[,] 返回值类型
        /// </summary>
        public Type AwaiterReturnType { get; protected set; }
        /// <summary>
        /// 模板成员类型
        /// </summary>
        public Type TemplateMemberType
        {
            get { return AwaiterReturnType ?? MemberSystemType; }
        }
        /// <summary>
        /// 成员编号
        /// </summary>
        public int MemberIndex { get; protected set; }
        /// <summary>
        /// 选择类型
        /// </summary>
        public MemberFilters MemberFilters;
        /// <summary>
        /// 是否字段
        /// </summary>
        public bool IsField { get; protected set; }
        /// <summary>
        /// 是否可赋值
        /// </summary>
        public bool CanSet { get; protected set; }
        /// <summary>
        /// 是否可读取
        /// </summary>
        public bool CanGet { get; protected set; }
        /// <summary>
        /// 是否忽略该成员
        /// </summary>
        private NullableBool isIgnore;
        /// <summary>
        /// 是否忽略该成员
        /// </summary>
        public bool IsIgnore
        {
            get
            {
                if (isIgnore == NullableBool.Null) isIgnore = Member != null && GetAttribute<IgnoreAttribute>(true) != null ? NullableBool.True : NullableBool.False;
                return isIgnore == NullableBool.True;
            }
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        internal MemberIndexInfo(MemberInfo member, MemberFilters filter, int index)
        {
            Member = member;
            MemberIndex = index;
            MemberFilters = filter;
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="index">成员编号</param>
        internal MemberIndexInfo(int index)
        {
            MemberIndex = index;
            IsField = CanSet = CanSet = true;
            MemberFilters = MemberFilters.PublicInstance;
        }
        /// <summary>
        /// 自定义属性集合
        /// </summary>
        private object[] attributes;
        /// <summary>
        /// 自定义属性集合(包括基类成员属性)
        /// </summary>
        private object[] baseAttributes;
        /// <summary>
        /// 获取自定义属性集合
        /// </summary>
        /// <typeparam name="attributeType"></typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns></returns>
        internal IEnumerable<attributeType> Attributes<attributeType>(bool isBaseType) where attributeType : Attribute
        {
            if (Member != null)
            {
                object[] values;
                if (isBaseType)
                {
                    if (baseAttributes == null)
                    {
                        baseAttributes = Member.GetCustomAttributes(true);
                        if (baseAttributes.Length == 0) attributes = baseAttributes;
                    }
                    values = baseAttributes;
                }
                else
                {
                    if (attributes == null) attributes = Member.GetCustomAttributes(false);
                    values = attributes;
                }
                foreach (object value in values)
                {
                    attributeType attribute = value as attributeType;
                    if (attribute != null) yield return attribute;
                }
            }
        }
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>自定义属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public attributeType GetAttribute<attributeType>(bool isBaseType) where attributeType : Attribute
        {
            foreach (attributeType attribute in Attributes<attributeType>(isBaseType)) return attribute;
            return null;
        }
        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>自定义属性,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public attributeType GetSetupAttribute<attributeType>(bool isBaseType) where attributeType : IgnoreMemberAttribute
        {
            if (!IsIgnore)
            {
                attributeType value = GetAttribute<attributeType>(isBaseType);
                if (value != null && value.IsSetup) return value;
            }
            return null;
        }
        /// <summary>
        /// 判断是否存在自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>是否存在自定义属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsAttribute<attributeType>(bool isBaseType) where attributeType : IgnoreMemberAttribute
        {
            return GetSetupAttribute<attributeType>(isBaseType) != null;
        }
        /// <summary>
        /// 判断是否忽略自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>是否忽略自定义属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsIgnoreAttribute<attributeType>(bool isBaseType) where attributeType : IgnoreMemberAttribute
        {
            if (IsIgnore) return true;
            attributeType value = GetAttribute<attributeType>(isBaseType);
            return value != null && !value.IsSetup;
        }
    }
    /// <summary>
    /// 成员索引
    /// </summary>
    /// <typeparam name="memberType">成员类型</typeparam>
    internal abstract class MemberIndexInfo<memberType> : MemberIndexInfo where memberType : MemberInfo
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public new memberType Member;
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        protected MemberIndexInfo(memberType member, MemberFilters filter, int index)
            : base(member, filter, index)
        {
            Member = member;
        }
    }
}
