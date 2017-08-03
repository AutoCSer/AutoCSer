using System;

namespace AutoCSer.TestCase.SqlModel.Member
{
    /// <summary>
    /// 时间范围（自定义组合字段，映射到数据库表格的多个字段）
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [AutoCSer.Sql.Column]
    public struct DateRange : AutoCSer.Sql.IVerify
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public AutoCSer.Sql.Member.IntDate Start;
        /// <summary>
        /// 结束日志
        /// </summary>
        public AutoCSer.Sql.Member.IntDate End;
        /// <summary>
        /// 是否通过SQL数据验证
        /// </summary>
        /// <returns></returns>
        public bool IsSqlVeify()
        {
            return End.Value >= Start.Value;
        }
    }
}
