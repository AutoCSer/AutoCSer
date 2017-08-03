using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 生成当前时间精度
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class NowTime
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 生成当前时间精度
        /// </summary>
        [AutoCSer.Sql.Member(IsNowTime = true)]
        public DateTime AppendTime;
    }
}
