using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 远程成员缓存配置
    /// </summary>
    public sealed partial class RemoteMemberCacheAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 是否需要可空检查
        /// </summary>
        public bool IsNull = true;
    }
}
