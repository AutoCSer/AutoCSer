using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 申明自增标识
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class IdentityMember
    {
        /// <summary>
        /// 申明自增标识
        /// </summary>
        [AutoCSer.Sql.Member(IsIdentity = true)]
        public int Key;
        /// <summary>
        /// 因为已经存在申明自增标识 Key，所以该字段的默认自增标识无效，仅为普通数据列
        /// </summary>
        public int Id;
    }
}
