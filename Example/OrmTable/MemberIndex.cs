using System;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 生成成员索引
    /// </summary>
    [AutoCSer.Sql.Table(ConnectionName = AutoCSer.Example.OrmConfig.Pub.ConnectionName)]
    public sealed partial class MemberIndex : AutoCSer.Example.OrmModel.MemberIndex.SqlModel<MemberIndex>
    {
        /// <summary>
        /// 更新数据事件
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private static void onUpdated(MemberIndex value, MemberIndex oldValue, Metadata.MemberMap<OrmModel.MemberIndex> memberMap)
        {
            if (MemberIndexs.UpdateIndex.IsMember(memberMap))
            {
                Console.WriteLine("Updated Index " + oldValue.UpdateIndex.ToString() + " to " + value.UpdateIndex.ToString());
            }
        }
        static MemberIndex()
        {
            if (sqlTable != null)
            {
                sqlTable.OnUpdated += onUpdated;
            }
        }
    }
}
