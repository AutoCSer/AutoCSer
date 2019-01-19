using System;
using AutoCSer.Metadata;

namespace AutoCSer.Data
{
    /// <summary>
    /// 数据成员信息
    /// </summary>
    public class MemberAttribute : IgnoreMemberAttribute
    {
        /// <summary>
        /// 主键索引,0标识非主键
        /// </summary>
        public int PrimaryKeyIndex;
        /// <summary>
        /// 字符串最大长度验证
        /// </summary>
        public int MaxStringLength;
        /// <summary>
        /// 分组标识
        /// </summary>
        public int Group;
        /// <summary>
        /// 是否固定长度
        /// </summary>
        public bool IsFixedLength;
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity;
        /// <summary>
        /// 是否允许空值
        /// </summary>
        public bool IsNull;
    }
}
