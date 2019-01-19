using System;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 自定义序列化成员位图
    /// </summary>
    public struct SerializeMemberMap
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap MemberMap;
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap CurrentMemberMap;
        /// <summary>
        /// JSON序列化成员位图
        /// </summary>
        internal MemberMap JsonMemberMap;
    }
}
