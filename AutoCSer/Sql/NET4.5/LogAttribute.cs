using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 日志同步成员信息
    /// </summary>
    public partial class LogAttribute
    {
        /// <summary>
        /// 默认为 false 表示不生成 await 客户端函数
        /// </summary>
        public bool IsAwait;
    }
}
