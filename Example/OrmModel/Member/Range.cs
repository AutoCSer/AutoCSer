using System;

namespace AutoCSer.Example.OrmModel.Member
{
    /// <summary>
    /// 自定义组合列
    /// </summary>
    [AutoCSer.Sql.Column]
    public struct Range : AutoCSer.Sql.IVerify
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        public int Start;
        /// <summary>
        /// 结束位置
        /// </summary>
        public int End;
        /// <summary>
        /// 是否通过SQL数据验证
        /// </summary>
        /// <returns></returns>
        public bool IsSqlVeify()
        {
            return End >= Start && Start >= 0;
        }
    }
}
