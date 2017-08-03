using System;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 成员缓存
    /// </summary>
    /// <typeparam name="valueType">缓存数据类型</typeparam>
    public abstract class MemberCache<valueType>
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public valueType Value;
    }
}
