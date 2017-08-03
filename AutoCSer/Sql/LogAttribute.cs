using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 日志同步成员信息
    /// </summary>
    public class LogAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 计数完成类型
        /// </summary>
        public Type CountType;
        /// <summary>
        /// 计数完成类型表格编号
        /// </summary>
        public int CountTypeNumber;
        /// <summary>
        /// SQL 数据库成员
        /// </summary>
        public bool IsMember;
    }
}
