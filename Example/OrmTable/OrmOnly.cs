using System;
using AutoCSer.Extension;
using System.Threading;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 简单 ORM 映射
    /// </summary>
    [AutoCSer.Sql.Table(ConnectionName = AutoCSer.Example.OrmConfig.Pub.ConnectionName, IsOnlyQueue = false)]
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
                sqlTable.InsertQueue(new OrmOnly { Value = 1 });
                foreach (OrmOnly value in sqlTable.SelectQueue())
                {
                    Console.WriteLine(value.toJson());
                    sqlTable.UpdateQueue(new OrmOnly { Id = value.Id, Value = value.Value + 1 }, updateMemberMap);
                }
                //foreach (OrmOnly value in sqlTable.Select())
                //{
                //    Console.WriteLine(value.toJson());
                //    sqlTable.Delete(value.Id);
                //}
                using (deleteWait = new AutoResetEvent(false))
                {
                    deleteCount = 1;
                    foreach (OrmOnly value in sqlTable.SelectQueue())
                    {
                        Console.WriteLine(value.toJson());
                        Interlocked.Increment(ref deleteCount);
                        sqlTable.DeleteQueue(value.Id, onDeleted);
                    }
                    onDeleted();
                    deleteWait.WaitOne();
                }
            }
        }
        /// <summary>
        /// 异步测试结束等待事件
        /// </summary>
        private static AutoResetEvent deleteWait;
        /// <summary>
        /// 异步删除计数
        /// </summary>
        private static int deleteCount;
        /// <summary>
        /// 异步删除完成处理
        /// </summary>
        private static void onDeleted()
        {
            if (System.Threading.Interlocked.Decrement(ref deleteCount) == 0) deleteWait.Set();
        }
        /// <summary>
        /// 异步删除完成处理
        /// </summary>
        /// <param name="value"></param>
        private static void onDeleted(OrmOnly value)
        {
            if (value == null) Console.WriteLine("Delete Error!");
            onDeleted();
        }
    }
}
