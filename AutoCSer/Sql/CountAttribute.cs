using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 计数成员信息
    /// </summary>
    public class CountAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 计数读取服务名称
        /// </summary>
        public string ReadServerName;
        /// <summary>
        /// 计数更新服务名称
        /// </summary>
        public string WriteServerName;
        /// <summary>
        /// 计数成员超时秒数，默认为 0 表示非计数成员
        /// </summary>
        public int Timeout;
    }
}
