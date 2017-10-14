using System;
using AutoCSer.Metadata;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 签名计算
    /// </summary>
    internal sealed class SignAttribute : AutoCSer.Metadata.MemberFilterAttribute.InstanceField
    {
        /// <summary>
        /// 默认签名计算类型配置
        /// </summary>
        internal static readonly SignAttribute AllMember = new SignAttribute { IsAllMember = true };
        /// <summary>
        /// 是否序列化所有成员
        /// </summary>
        internal bool IsAllMember;

        /// <summary>
        /// 字符串比较大小
        /// </summary>
        public static Func<KeyValue<FieldIndex, PropertyIndex>, KeyValue<FieldIndex, PropertyIndex>, int> NameCompare = compare;
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(KeyValue<FieldIndex, PropertyIndex> left, KeyValue<FieldIndex, PropertyIndex> right)
        {
            return string.CompareOrdinal(left.Key == null ? left.Value.Member.Name : left.Key.Member.Name, right.Key == null ? right.Value.Member.Name : right.Key.Member.Name);
        }
    }
}
