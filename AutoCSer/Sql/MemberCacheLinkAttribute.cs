using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 成员扩展缓存初始化依赖类型声明配置
    /// </summary>
    public sealed class MemberCacheLinkAttribute : Attribute
    {
        /// <summary>
        /// 初始化依赖类型
        /// </summary>
        public Type CacheType;
    }
}
