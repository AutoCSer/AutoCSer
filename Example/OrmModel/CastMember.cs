using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 基于强制类型转换的自定义数据类型
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class CastMember
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 基于强制类型转换的自定义数据类型
        /// </summary>
        public Member.Ipv4 Ipv4;
        /// <summary>
        /// 整形日期映射到 int
        /// </summary>
        public AutoCSer.Sql.Member.IntDate Date;
    }
}
