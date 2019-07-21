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
        /// 动态成员分组
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct group
        {
            /// <summary>
            /// 公有动态字段
            /// </summary>
            public FieldInfo[] PublicFields;
            /// <summary>
            /// 非公有动态字段
            /// </summary>
            public FieldInfo[] NonPublicFields;
            /// <summary>
            /// 公有动态属性
            /// </summary>
            public PropertyInfo[] PublicProperties;
            /// <summary>
            /// 非公有动态属性
            /// </summary>
            public PropertyInfo[] NonPublicProperties;
            /// <summary>
            /// 匿名字段
            /// </summary>
            public FieldInfo[] AnonymousFields;
            /// <summary>
            /// 动态成员分组
            /// </summary>
            /// <param name="type">目标类型</param>
            /// <param name="isStatic">是否静态成员</param>
            public group(Type type, bool isStatic)
            {
                Dictionary<HashString, typeDepth> members = AutoCSer.DictionaryCreator.CreateHashString<typeDepth>();
                BindingFlags staticFlags = isStatic ? (BindingFlags.Static | BindingFlags.FlattenHierarchy) : BindingFlags.Instance;
                typeDepth oldMember;
                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | staticFlags))
                {
                    typeDepth member = new typeDepth(type, field, true);
                    HashString nameKey = field.Name;
                    if (!members.TryGetValue(nameKey, out oldMember) || member.Depth < oldMember.Depth) members[nameKey] = member;
                }
                int anonymousCount = 0;
                bool isAnonymous = !isStatic && type.Name[0] == '<', isAnonymousCount = !isStatic && !isAnonymous;
                foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | staticFlags))
                {
                    if (field.Name[0] == '<')
                    {
                        if (isAnonymous)
                        {
                            int index = field.Name.IndexOf('>');
                            if (index != -1)
                            {
                                typeDepth member = new typeDepth(type, field, false);
                                HashString nameKey = new SubString { String = field.Name, Start = 1, Length = index - 1 };
                                if (!members.TryGetValue(nameKey, out oldMember)) members[nameKey] = member;
                            }
                        }
                        else if (isAnonymousCount) ++anonymousCount;
                    }
                    else
                    {
                        typeDepth member = new typeDepth(type, field, false);
                        HashString nameKey = field.Name;
                        if (!members.TryGetValue(nameKey, out oldMember) || member.Depth < oldMember.Depth) members[nameKey] = member;
                    }
                }
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | staticFlags))
                {
                    if (!members.ContainsKey(property.Name))
                    {
                        typeDepth member = new typeDepth(type, property, true);
                        string name = property.Name + ".";
                        ParameterInfo[] parameters = property.GetIndexParameters();
                        if (parameters.Length != 0) name += parameters.joinString(',', value => value.ParameterType.fullName());
                        HashString nameKey = name;
                        if (!members.TryGetValue(nameKey, out oldMember) || member.Depth < oldMember.Depth) members[nameKey] = member;
                    }
                }
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.NonPublic | staticFlags))
                {
                    if (!members.ContainsKey(property.Name))
                    {
                        typeDepth member = new typeDepth(type, property, false);
                        string name = property.Name + ".";
                        ParameterInfo[] parameters = property.GetIndexParameters();
                        if (parameters.Length != 0) name += parameters.joinString(',', value => value.ParameterType.fullName());
                        HashString nameKey = name;
                        if (!members.TryGetValue(nameKey, out oldMember) || member.Depth < oldMember.Depth) members[nameKey] = member;
                    }
                }
                PublicFields = members.Values.getFindArray(value => value.PublicField, value => value != null);
                NonPublicFields = members.Values.getFindArray(value => value.NonPublicField, value => value != null);
                PublicProperties = members.Values.getFindArray(value => value.PublicProperty, value => value != null);
                NonPublicProperties = members.Values.getFindArray(value => value.NonPublicProperty, value => value != null);
                if (anonymousCount != 0)
                {
                    LeftArray<FieldInfo> anonymousFields = new LeftArray<FieldInfo>();
                    foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | staticFlags))
                    {
                        if (field.Name[0] == '<')
                        {
                            HashString nameKey = field.Name;
                            if (!members.ContainsKey(nameKey))
                            {
                                members[nameKey] = new typeDepth();
                                anonymousFields.Add(field);
                            }
                        }
                    }
                    AnonymousFields = anonymousFields.ToArray();
                }
                else AnonymousFields = NullValue<FieldInfo>.Array;
            }
        }
        /// <summary>
        /// 类型深度
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct typeDepth
        {
            /// <summary>
            /// 成员信息
            /// </summary>
            private System.Reflection.MemberInfo member;
            /// <summary>
            /// 类型深度
            /// </summary>
            public int Depth;
            /// <summary>
            /// 是否字段
            /// </summary>
            private bool isField;
            /// <summary>
            /// 是否共有成员
            /// </summary>
            private bool isPublic;
            /// <summary>
            /// 共有字段成员
            /// </summary>
            public FieldInfo PublicField
            {
                get
                {
                    return isPublic && isField ? (FieldInfo)member : null;
                }
            }
            /// <summary>
            /// 非共有字段成员
            /// </summary>
            public FieldInfo NonPublicField
            {
                get
                {
                    return !isPublic && isField ? (FieldInfo)member : null;
                }
            }
            /// <summary>
            /// 共有属性成员
            /// </summary>
            public PropertyInfo PublicProperty
            {
                get
                {
                    return isPublic && !isField ? (PropertyInfo)member : null;
                }
            }
            /// <summary>
            /// 非共有属性成员
            /// </summary>
            public PropertyInfo NonPublicProperty
            {
                get
                {
                    return !isPublic && !isField ? (PropertyInfo)member : null;
                }
            }
            /// <summary>
            /// 类型深度
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="field">成员字段</param>
            /// <param name="isPublic">是否共有成员</param>
            public typeDepth(Type type, FieldInfo field, bool isPublic)
            {
                Type memberType = field.DeclaringType;
                member = field;
                isField = true;
                this.isPublic = isPublic;
                for (Depth = 0; type != memberType; ++Depth) type = type.BaseType;
            }
            /// <summary>
            /// 类型深度
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="property">成员属性</param>
            /// <param name="isPublic">是否共有成员</param>
            public typeDepth(Type type, PropertyInfo property, bool isPublic)
            {
                Type memberType = property.DeclaringType;
                member = property;
                isField = false;
                this.isPublic = isPublic;
                for (Depth = 0; type != memberType; ++Depth) type = type.BaseType;
            }
        }
        /// <summary>
        /// 成员索引分组集合
        /// </summary>
        private static Dictionary<Type, MemberIndexGroup> cache = AutoCSer.DictionaryCreator.CreateOnly<Type, MemberIndexGroup>();
        /// <summary>
        /// 成员索引分组集合访问锁
        /// </summary>
        private static readonly object cacheLock = new object();
        /// <summary>
        /// 公有字段
        /// </summary>
        internal readonly FieldIndex[] PublicFields;
        /// <summary>
        /// 非公有字段
        /// </summary>
        internal readonly FieldIndex[] NonPublicFields;
        /// <summary>
        /// 公有属性
        /// </summary>
        internal readonly PropertyIndex[] PublicProperties;
        /// <summary>
        /// 非公有属性
        /// </summary>
        internal readonly PropertyIndex[] NonPublicProperties;
        /// <summary>
        /// 匿名字段
        /// </summary>
        internal readonly FieldIndex[] AnonymousFields;
        /// <summary>
        /// 所有成员数量
        /// </summary>
        public readonly int MemberCount;
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        private static readonly Func<FieldInfo, FieldInfo, int> fieldCompare = Compare;
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int Compare(FieldInfo left, FieldInfo right)
        {
            return string.CompareOrdinal(left.Name, right.Name);
        }
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        private static readonly Func<PropertyInfo, PropertyInfo, int> propertyCompare = compare;
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(PropertyInfo left, PropertyInfo right)
        {
            return string.CompareOrdinal(left.Name, right.Name);
        }
        /// <summary>
        /// 成员索引分组
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="isStatic">是否静态成员</param>
        internal MemberIndexGroup(Type type, bool isStatic)
        {
            int index = 0;
            if (type.IsEnum)
            {
                PublicFields = type.GetFields(BindingFlags.Public | BindingFlags.Static).getArray(member => new FieldIndex(member, MemberFilters.PublicStaticField, index++));
                NonPublicFields = AnonymousFields = NullValue<FieldIndex>.Array;
                PublicProperties = NonPublicProperties = NullValue<PropertyIndex>.Array;
            }
            else if (type.IsPrimitive || type == typeof(decimal) || type == typeof(object))// || type.IsArray || type == typeof(string)
            {
                PublicFields = NonPublicFields = AnonymousFields = NullValue<FieldIndex>.Array;
                PublicProperties = NonPublicProperties = NullValue<PropertyIndex>.Array;
            }
            else if (isStatic)
            {
                group group = new group(type, true);
                PublicFields = group.PublicFields.sort(fieldCompare).getArray(value => new FieldIndex(value, MemberFilters.PublicStaticField, index++));
                NonPublicFields = group.NonPublicFields.sort(fieldCompare).getArray(value => new FieldIndex(value, MemberFilters.NonPublicStaticField, index++));
                PublicProperties = group.PublicProperties.sort(propertyCompare).getArray(value => new PropertyIndex(value, MemberFilters.PublicStaticProperty, index++));
                NonPublicProperties = group.NonPublicProperties.sort(propertyCompare).getArray(value => new PropertyIndex(value, MemberFilters.NonPublicStaticProperty, index++));
                AnonymousFields = NullValue<FieldIndex>.Array;
            }
            else
            {
                group group = new group(type, false);
                PublicFields = group.PublicFields.sort(fieldCompare).getArray(value => new FieldIndex(value, MemberFilters.PublicInstanceField, index++));
                NonPublicFields = group.NonPublicFields.sort(fieldCompare).getArray(value => new FieldIndex(value, MemberFilters.NonPublicInstanceField, index++));
                PublicProperties = group.PublicProperties.sort(propertyCompare).getArray(value => new PropertyIndex(value, MemberFilters.PublicInstanceProperty, index++));
                NonPublicProperties = group.NonPublicProperties.sort(propertyCompare).getArray(value => new PropertyIndex(value, MemberFilters.NonPublicInstanceProperty, index++));
                AnonymousFields = group.AnonymousFields.sort(fieldCompare).getArray(value => new FieldIndex(value, MemberFilters.NonPublicInstanceField, index++));
            }
            MemberCount = index;
        }
        /// <summary>
        /// 根据类型获取成员索引分组
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>成员索引分组</returns>
        public static MemberIndexGroup Get(Type type)
        {
            MemberIndexGroup value;
            Monitor.Enter(cacheLock);
            try
            {
                if (!cache.TryGetValue(type, out value)) cache.Add(type, value = new MemberIndexGroup(type, false));
            }
            finally { Monitor.Exit(cacheLock); }
            return value;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClearCache()
        {
            Monitor.Enter(cacheLock);
            try
            {
                if (cache.Count != 0) cache = AutoCSer.DictionaryCreator.CreateOnly<Type, MemberIndexGroup>();
            }
            finally { Monitor.Exit(cacheLock); }
        }
    }
    /// <summary>
    /// 成员索引分组
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    internal static class MemberIndexGroup<valueType>
    {
        /// <summary>
        /// 成员索引分组
        /// </summary>
        public static readonly MemberIndexGroup Group = MemberIndexGroup.Get(typeof(valueType));
        /// <summary>
        /// 所有成员数量
        /// </summary>
        public static readonly int MemberCount = Group.MemberCount;
        /// <summary>
        /// 字段成员数量
        /// </summary>
        public static readonly int FieldCount = Group.PublicFields.Length + Group.NonPublicFields.Length;
        /// <summary>
        /// 成员集合
        /// </summary>
        public static MemberIndexInfo[] GetAllMembers()
        {
            LeftArray<MemberIndexInfo> members = new LeftArray<MemberIndexInfo>(MemberCount);
            members.Add(Group.PublicFields.toGeneric<MemberIndexInfo>());
            members.Add(Group.NonPublicFields.toGeneric<MemberIndexInfo>());
            members.Add(Group.PublicProperties.toGeneric<MemberIndexInfo>());
            members.Add(Group.NonPublicProperties.toGeneric<MemberIndexInfo>());
            return members.ToArray();
        }
        /// <summary>
        /// 获取字段集合
        /// </summary>
        /// <param name="memberFilter">成员选择类型</param>
        /// <returns></returns>
        public static FieldIndex[] GetFields(MemberFilters memberFilter = MemberFilters.InstanceField)
        {
            if ((memberFilter & MemberFilters.PublicInstanceField) == 0)
            {
                if ((memberFilter & MemberFilters.NonPublicInstanceField) == 0) return NullValue<FieldIndex>.Array;
                return Group.NonPublicFields;
            }
            else if ((memberFilter & MemberFilters.NonPublicInstanceField) == 0) return Group.PublicFields;
            return Group.PublicFields.concat(Group.NonPublicFields);
        }
        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="memberFilter">成员选择类型</param>
        /// <returns></returns>
        public static PropertyIndex[] GetProperties(MemberFilters memberFilter = MemberFilters.InstanceField)
        {
            if ((memberFilter & MemberFilters.PublicInstanceProperty) == 0)
            {
                if ((memberFilter & MemberFilters.NonPublicInstanceProperty) == 0) return NullValue<PropertyIndex>.Array;
                return Group.NonPublicProperties;
            }
            else if ((memberFilter & MemberFilters.NonPublicInstanceProperty) == 0) return Group.PublicProperties;
            return Group.PublicProperties.concat(Group.NonPublicProperties);
        }
        /// <summary>
        /// 获取字段集合
        /// </summary>
        /// <param name="memberFilter">成员选择类型</param>
        /// <returns></returns>
        public static FieldIndex[] GetAnonymousFields(MemberFilters memberFilter = MemberFilters.InstanceField)
        {
            if (Group.AnonymousFields.Length == 0) return GetFields(memberFilter);
            if ((memberFilter & MemberFilters.PublicInstanceField) == 0)
            {
                if ((memberFilter & MemberFilters.NonPublicInstanceField) == 0) return Group.AnonymousFields;
                return Group.NonPublicFields.concat(Group.AnonymousFields);
            }
            else if ((memberFilter & MemberFilters.NonPublicInstanceField) == 0) return Group.PublicFields.concat(Group.AnonymousFields);
            return AutoCSer.Extension.ArrayExtension.concat(Group.PublicFields, Group.NonPublicFields, Group.AnonymousFields);
        }
    }
}
