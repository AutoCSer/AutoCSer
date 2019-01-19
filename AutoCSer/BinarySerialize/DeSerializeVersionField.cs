using System;
using AutoCSer.Metadata;
using System.Runtime.InteropServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化版本字段信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DeSerializeVersionField
    {
        /// <summary>
        /// 字段索引
        /// </summary>
        internal FieldIndex Field;
        /// <summary>
        /// 二进制数据序列化成员配置
        /// </summary>
        internal SerializeMemberAttribute Attribute;
        /// <summary>
        /// 是否删除字段
        /// </summary>
        internal bool IsRemove;
        /// <summary>
        /// 全局版本编号
        /// </summary>
        internal uint GlobalVersion
        {
            get
            {
                return IsRemove ? Attribute.RemoveGlobalVersion : (Attribute != null ? Attribute.GlobalVersion : 0);
            }
        }
        /// <summary>
        /// 静态成员转换为实例成员
        /// </summary>
        private int SortMemberFilters
        {
            get
            {
                MemberFilters memberFilters = Field.MemberFilters;
                if (Attribute != null && Attribute.IsRemove)
                {
                    switch (memberFilters)
                    {
                        case MemberFilters.PublicStaticField: memberFilters = MemberFilters.PublicInstanceField; break;
                        case MemberFilters.NonPublicStaticField: memberFilters = MemberFilters.NonPublicInstanceField; break;
                    }
                    switch (memberFilters)
                    {
                        case MemberFilters.PublicInstanceField:
                            if (!Attribute.IsRemovePublic) memberFilters = MemberFilters.NonPublicInstanceField;
                            break;
                        case MemberFilters.NonPublicInstanceField:
                            if (Attribute.IsRemovePublic) memberFilters = MemberFilters.PublicInstanceField;
                            break;
                    }
                }
                return (int)memberFilters;
            }
        }
        /// <summary>
        /// 复制字段索引
        /// </summary>
        /// <param name="memberIndex"></param>
        /// <returns></returns>
        internal DeSerializeVersionField Copy(int memberIndex)
        {
            return new DeSerializeVersionField { Field = new FieldIndex(Field.Member, Field.MemberFilters, memberIndex), Attribute = Attribute, IsRemove = IsRemove };
        }

        /// <summary>
        /// 全局版本编号排序比较
        /// </summary>
        internal static readonly Func<DeSerializeVersionField, DeSerializeVersionField, int> GlobalVersionSort = globalVersionSort;
        /// <summary>
        /// 全局版本编号排序比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int globalVersionSort(DeSerializeVersionField left, DeSerializeVersionField right)
        {
            return left.GlobalVersion.CompareTo(right.GlobalVersion);
        }
        /// <summary>
        /// 成员名称排序比较
        /// </summary>
        internal static readonly Func<DeSerializeVersionField, DeSerializeVersionField, int> MemberNameSort = memberNameSort;
        /// <summary>
        /// 成员名称排序比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int memberNameSort(DeSerializeVersionField left, DeSerializeVersionField right)
        {
            int value = left.SortMemberFilters - right.SortMemberFilters;
            return value == 0 ? string.CompareOrdinal(left.Field.Member.Name, right.Field.Member.Name) : value;
        }
    }
}
