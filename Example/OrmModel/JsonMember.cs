using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// JSON 映射
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class JsonMember
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// JSON 映射
        /// </summary>
        [AutoCSer.Sql.Member(IsAscii = true, MaxStringLength = 11 * 10)]
        public int[] CatalogIds;
    }
}
