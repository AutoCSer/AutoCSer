using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 忽略成员
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class IgnoreMember
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 被忽略成员
        /// </summary>
        [AutoCSer.Sql.IgnoreMember]
        public int Ignore;
    }
}
