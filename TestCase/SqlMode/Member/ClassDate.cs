using System;

namespace AutoCSer.TestCase.SqlModel.Member
{
    /// <summary>
    /// 班级+日期
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct ClassDate
    {
        /// <summary>
        /// 班级标识
        /// </summary>
        public int ClassId;
        /// <summary>
        /// 日期
        /// </summary>
        public AutoCSer.Sql.Member.IntDate Date;
    }
}
