using System;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 缓存扩展支持
    /// </summary>
    [AutoCSer.Sql.Table(ConnectionName = AutoCSer.Example.OrmConfig.Pub.ConnectionName, IsLoadIdentity = false)]
    public sealed partial class MemberCache : AutoCSer.Example.OrmModel.MemberCache.SqlModel<MemberCache, MemberCache.MemberCacheExtension>
    {
        /// <summary>
        /// 缓存扩展类型
        /// </summary>
        public sealed class MemberCacheExtension : AutoCSer.Sql.Cache.Whole.MemberCache<MemberCache>
        {
            /// <summary>
            /// 缓存扩展数据
            /// </summary>
            public int Extension1;
        }
        /// <summary>
        /// 扩展缓存数据
        /// </summary>
        [AutoCSer.Sql.MemberCache]
        internal MemberCacheExtension Extension;

        static MemberCache()
        {
            if (sqlTable != null) sqlLoaded();
        }
    }
}
