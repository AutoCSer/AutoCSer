using System;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 生成当前时间精度
    /// </summary>
    [AutoCSer.Sql.Table(ConnectionName = AutoCSer.Example.OrmConfig.Pub.ConnectionName)]
    public sealed partial class NowTime : AutoCSer.Example.OrmModel.NowTime.SqlModel<NowTime>
    {
        /// <summary>
        /// 添加数据库记录
        /// </summary>
        /// <param name="value"></param>
        internal static void Append(NowTime value)
        {
            value.AppendTime = NowTimes.AppendTime.Next;
            sqlTable.Insert(value);
        }
    }
}
