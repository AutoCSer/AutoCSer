using System;
using AutoCSer.Extension;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 简单 ORM 映射
    /// </summary>
    [AutoCSer.Sql.Table(ConnectionName = AutoCSer.Example.OrmConfig.Pub.ConnectionName)]
    public sealed class OrmOnly
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 测试数据
        /// </summary>
        public int Value;

        /// <summary>
        /// 简单 ORM 映射
        /// </summary>
        internal static void Test()
        {
            using (AutoCSer.Sql.ModelTable<OrmOnly> sqlTable = AutoCSer.Sql.ModelTable<OrmOnly>.Get())
            {
                AutoCSer.Metadata.MemberMap<OrmOnly> updateMemberMap = sqlTable.CreateMemberMap().Set(value => value.Value);
                sqlTable.Insert(new OrmOnly { Value = 1 });
                foreach (OrmOnly value in sqlTable.Select())
                {
                    Console.WriteLine(value.Value.ToString());
                    sqlTable.Update(new OrmOnly { Id = value.Id, Value = value.Value + 1 }, updateMemberMap);
                }
                foreach (OrmOnly value in sqlTable.Select())
                {
                    Console.WriteLine(value.Value.ToString());
                    sqlTable.Delete(value.Id);
                }
            }
        }
    }
}
