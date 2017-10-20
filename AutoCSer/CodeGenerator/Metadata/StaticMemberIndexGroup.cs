using System;
using AutoCSer.Metadata;
using System.Threading;
using System.Collections.Generic;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员索引分组
    /// </summary>
    internal static class StaticMemberIndexGroup
    {
        /// <summary>
        /// 成员索引分组集合
        /// </summary>
        private static readonly Dictionary<Type, MemberIndexGroup> cache = DictionaryCreator.CreateOnly<Type, MemberIndexGroup>();
        /// <summary>
        /// 成员索引分组集合访问锁
        /// </summary>
        private static readonly object cacheLock = new object();
        /// <summary>
        /// 根据类型获取成员索引分组
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>成员索引分组</returns>
        internal static MemberIndexGroup Get(Type type)
        {
            MemberIndexGroup value;
            Monitor.Enter(cacheLock);
            try
            {
                if (!cache.TryGetValue(type, out value)) cache.Add(type, value = new MemberIndexGroup(type, true));
            }
            finally { Monitor.Exit(cacheLock); }
            return value;
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
        public static MemberIndexInfo[] Get<attributeType>(Type type, MemberFilters filter, bool isFilter, bool isAttribute, bool isBaseType)
             where attributeType : AutoCSer.Metadata.IgnoreMemberAttribute
        {
            return Get(type).Find(filter, isFilter).getFindArray(value => isAttribute ? value.IsAttribute<attributeType>(isBaseType) : !value.IsIgnoreAttribute<attributeType>(isBaseType));
        }
    }
}
