using System;

namespace AutoCSer.TestCase.SqlModel
{
    /// <summary>
    /// 班级数据定义
    /// </summary>
    [AutoCSer.WebView.ClientType(PrefixName = Pub.WebViewClientTypePrefixName)]
    [AutoCSer.Sql.Model(CacheType = AutoCSer.Sql.Cache.Whole.Event.Type.IdentityArray, IsMemberCache = true, LogServerName = Pub.LogServerName, IsLogSerializeReferenceMember = false)]
    public partial class Class
    {
        /// <summary>
        /// 班级标识（默认自增）
        /// </summary>
        [AutoCSer.WebView.OutputAjax]
        [AutoCSer.Net.TcpStaticServer.RemoteKey]
        public int Id;
        /// <summary>
        /// 班级名称
        /// </summary>
        [AutoCSer.Sql.Member(MaxStringLength = 32)]
        public string Name;
        /// <summary>
        /// 开始日期+结束日期（自定义组合字段，映射到数据库表格的多个字段）
        /// </summary>
        public Member.DateRange DateRange;
        /// <summary>
        /// 专业（自定义 enum 字段）
        /// </summary>
        public Member.Discipline Discipline;

        #region 计算列（不映射到数据库表格，由缓存数据实时计算结果）
        /// <summary>
        /// 当前学生数量
        /// </summary>
        [AutoCSer.Sql.Log(CountType = typeof(Student))]
        internal int StudentCount;
        #endregion
    }
}
