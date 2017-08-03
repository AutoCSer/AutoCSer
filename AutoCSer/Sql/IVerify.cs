using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL数据验证接口
    /// </summary>
    public interface IVerify
    {
        /// <summary>
        /// 是否通过SQL数据验证
        /// </summary>
        /// <returns></returns>
        bool IsSqlVeify();
    }
}
