using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 缓存扩展支持
    /// </summary>
    [AutoCSer.Sql.Model(CacheType = AutoCSer.Sql.Cache.Whole.Event.Type.IdentityArray, IsMemberCache = true)]
    public partial class MemberCache
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
    }
}
