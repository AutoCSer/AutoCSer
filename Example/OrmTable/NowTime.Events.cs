using System;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 写操作事件
    /// </summary>
    public sealed partial class NowTime
    {
        /// <summary>
        /// 添加数据前验证数据可取消添加数据操作
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="cancel">取消操作数据</param>
        private static void onInsert(NowTime value, ref AutoCSer.Sql.AnyCancel cancel)
        {
            if (!cancel.IsCancel)
            {
                //添加数据前验证数据可取消添加数据操作
            }
        }
        /// <summary>
        /// 更新数据前验证数据可取消更新数据操作
        /// </summary>
        /// <param name="value">待更新数据</param>
        /// <param name="memberMap">更新数据成员位图</param>
        /// <param name="cancel">取消操作数据</param>
        private static void onUpdate(NowTime value, AutoCSer.Metadata.MemberMap<OrmModel.NowTime> memberMap, ref AutoCSer.Sql.AnyCancel cancel)
        {
            if (!cancel.IsCancel)
            {
                //更新数据前验证数据可取消更新数据操作
            }
        }
        /// <summary>
        /// 删除数据前验证数据可取消删除数据操作
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="cancel">取消操作数据</param>
        private static void onDelete(NowTime value, ref AutoCSer.Sql.AnyCancel cancel)
        {
            if (!cancel.IsCancel)
            {
                //取消删除数据操作
                //cancel.Cancel();
            }
        }

        /// <summary>
        /// 添加数据操作后续处理
        /// </summary>
        /// <param name="value">已添加的数据</param>
        private static void onInserted(NowTime value)
        {
        }
        /// <summary>
        /// 更新数据操作后续处理
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新数据成员位图</param>
        private static void onUpdated(NowTime value, NowTime oldValue, AutoCSer.Metadata.MemberMap<OrmModel.NowTime> memberMap)
        {
        }
        /// <summary>
        /// 删除数据操作后续处理
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private static void onDeleted(NowTime value)
        {
        }

        static NowTime()
        {
            if (sqlTable != null)
            {
                sqlTable.OnInsert += onInsert;
                sqlTable.OnUpdate += onUpdate;
                sqlTable.OnDelete += onDelete;

                sqlTable.OnInserted += onInserted;
                sqlTable.OnUpdated += onUpdated;
                sqlTable.OnDeleted += onDeleted;
            }
        }
    }
}
