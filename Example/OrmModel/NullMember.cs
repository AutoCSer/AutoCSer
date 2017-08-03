using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 允许空值 null
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class NullMember
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 允许空值 null
        /// </summary>
        [AutoCSer.Sql.Member(IsNull = true, MaxStringLength = 32)]
        public string CanNull;
        /// <summary>
        /// 不允许空值 null，所以需要使用非 null 值初始化
        /// </summary>
        [AutoCSer.Sql.Member(MaxStringLength = 32)]
        public string CanNotNull = string.Empty;
    }
}
