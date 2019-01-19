using System;
using AutoCSer.Extension;

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
            //value.AppendTime = NowTimes.AppendTime.Next;
            sqlTable.InsertQueue(value);
        }


        /// <summary>
        /// 当前时间精度测试
        /// </summary>
        internal static void Test()
        {
            Append(new NowTime());
            foreach (NowTime value in sqlTable.SelectQueue())
            {
                Console.WriteLine(value.toJson());
                sqlTable.DeleteQueue(value.Id);
            }
        }
    }
}
