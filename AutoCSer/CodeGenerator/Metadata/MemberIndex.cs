using System;
using System.Reflection;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员信息
    /// </summary>
    internal class MemberIndex : MemberIndexInfo
    {
        /// <summary>
        /// 成员类型
        /// </summary>
        public ExtensionType MemberType { get; private set; }
        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName { get; protected set; }
        /// <summary>
        /// 成员名称
        /// </summary>
        public string NextMemberName { get { return MemberName; } }
        /// <summary>
        /// XML文档注释
        /// </summary>
        protected string xmlDocument;
        /// <summary>
        /// XML文档注释
        /// </summary>
        public virtual string XmlDocument
        {
            get
            {
                if (xmlDocument == null)
                {
                    xmlDocument = Member == null ? string.Empty : (IsField ? CodeGenerator.XmlDocument.Get((FieldInfo)Member) : CodeGenerator.XmlDocument.Get((PropertyInfo)Member));
                }
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="index">成员编号</param>
        internal MemberIndex(FieldInfo field, int index)
            : base(index)
        {
            Member = field;
            MemberType = MemberSystemType = field.FieldType;
            MemberName = field.Name;
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        private MemberIndex(MemberIndexInfo member)
            : base(member)
        {
            MemberType = member.MemberSystemType;
            MemberName = Member.Name;
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        protected MemberIndex(MethodInfo method, MemberFilters filter, int index)
            : base(method, filter, index)
        {
            Member = method;
            MemberName = method.Name;
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员名称</param>
        protected MemberIndex(ExtensionType type, string name)
            : base(0)
        {
            MemberType = type;
            MemberName = name;
        }

        /// <summary>
        /// 类型成员集合缓存
        /// </summary>
        private static readonly Dictionary<Type, MemberIndex[]> memberCache = DictionaryCreator.CreateOnly<Type, MemberIndex[]>();
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员信息集合</returns>
        private static MemberIndex[] getMembers(Type type)
        {
            MemberIndex[] members;
            if (!memberCache.TryGetValue(type, out members))
            {
                MemberIndexGroup group = MemberIndexGroup.Get(type);
                memberCache[type] = members =
                    ArrayExtension.concat(group.PublicFields.getArray(value => new MemberIndex(value)),
                        group.NonPublicFields.getArray(value => new MemberIndex(value)),
                        group.PublicProperties.getArray(value => new MemberIndex(value)),
                        group.NonPublicProperties.getArray(value => new MemberIndex(value)));
            }
            return members;
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberIndex[] GetMembers(Type type, MemberFilters filter)
        {
            return getMembers(type).getFindArray(value => (value.MemberFilters & filter) != 0);
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberIndex[] GetMembers<attributeType>(Type type, MemberFilters filter, bool isAttribute, bool isBaseType)
            where attributeType : AutoCSer.Metadata.IgnoreMemberAttribute
        {
            return find<MemberIndex, attributeType>(GetMembers(type, filter), isAttribute, isBaseType);
        }


        /// <summary>
        /// 类型成员集合缓存
        /// </summary>
        private static readonly Dictionary<Type, MemberIndex[]> staticMemberCache = DictionaryCreator.CreateOnly<Type, MemberIndex[]>();
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员信息集合</returns>
        private static MemberIndex[] getStaticMembers(Type type)
        {
            MemberIndex[] members;
            if (!staticMemberCache.TryGetValue(type, out members))
            {
                MemberIndexGroup group = StaticMemberIndexGroup.Get(type);
                staticMemberCache[type] = members =
                    ArrayExtension.concat(group.PublicFields.getArray(value => new MemberIndex(value)),
                        group.NonPublicFields.getArray(value => new MemberIndex(value)),
                        group.PublicProperties.getArray(value => new MemberIndex(value)),
                        group.NonPublicProperties.getArray(value => new MemberIndex(value)));
            }
            return members;
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberIndex[] GetStaticMembers(Type type, MemberFilters filter)
        {
            return getStaticMembers(type).getFindArray(value => (value.MemberFilters & filter) != 0);
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberIndex[] GetStaticMembers<attributeType>(Type type, MemberFilters filter, bool isAttribute, bool isBaseType)
            where attributeType : AutoCSer.Metadata.IgnoreMemberAttribute
        {
            return find<MemberIndex, attributeType>(GetStaticMembers(type, filter), isAttribute, isBaseType);
        }
    }
}
