using System;
using AutoCSer.Extension;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 写操作 Insert / Update / Delete
    /// </summary>
    public sealed partial class NowTime
    {
        /// <summary>
        /// 同步添加数据
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <returns>添加是否成功</returns>
        internal static bool Insert(NowTime value)
        {
            //value.AppendTime = NowTimes.AppendTime.Next;
            return sqlTable.InsertQueue(value);
        }
        /// <summary>
        /// 异步添加数据
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="onInserted">添加数据回调</param>
        internal static void Insert(NowTime value, Action<NowTime> onInserted)
        {
            //value.AppendTime = NowTimes.AppendTime.Next;
            sqlTable.InsertQueue(value, onInserted);
        }

        /// <summary>
        /// 修改时间字段成员位图，由于手动构造成员位图代价比较大，应该定义静态变量公用
        /// </summary>
        private static readonly AutoCSer.Metadata.MemberMap<OrmModel.NowTime> updateAppendTimeMemberMap = sqlTable.CreateMemberMap().Set(value => value.AppendTime);
        /// <summary>
        /// 同步更新数据
        /// </summary>
        /// <param name="id">自增 Id</param>
        /// <returns>更新是否成功</returns>
        internal static bool UpdateAppendTime(int id)
        {
            //return sqlTable.Update(new NowTime { Id = id, AppendTime = NowTimes.AppendTime.Next }, updateAppendTimeMemberMap);
            return sqlTable.UpdateQueue(new NowTime { Id = id }, updateAppendTimeMemberMap);
        }
        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="id">自增 Id</param>
        /// <param name="onUpdated">更新数据回调</param>
        internal static void UpdateAppendTime(int id, Action<NowTime> onUpdated)
        {
            sqlTable.UpdateQueue(new NowTime { Id = id }, updateAppendTimeMemberMap, onUpdated);
            //sqlTable.Update(new NowTime { Id = id, AppendTime = NowTimes.AppendTime.Next }, updateAppendTimeMemberMap, onUpdated);
        }

        /// <summary>
        /// 同步删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <returns>是否删除成功</returns>
        internal static bool Delete(NowTime value)
        {
            return sqlTable.DeleteQueue(value);
        }
        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="onDeleted">删除数据回调</param>
        internal static void Delete(NowTime value, Action<NowTime> onDeleted)
        {
            sqlTable.DeleteQueue(value, onDeleted);
        }
        /// <summary>
        /// 同步删除数据
        /// </summary>
        /// <param name="id">待删除数据自增 Id</param>
        /// <returns>是否删除成功</returns>
        internal static bool Delete(int id)
        {
            return sqlTable.DeleteQueue(id);
        }
        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="id">待删除数据自增 Id</param>
        /// <param name="onDeleted">删除数据回调</param>
        internal static void Delete(int id, Action<NowTime> onDeleted)
        {
            sqlTable.DeleteQueue(id, onDeleted);
        }
    }
}
