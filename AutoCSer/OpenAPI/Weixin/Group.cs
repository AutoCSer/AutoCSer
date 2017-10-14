using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 分组
    /// </summary>
    public sealed class Group
    {
        /// <summary>
        /// 分组名字，UTF8编码
        /// </summary>
        public string name;
        /// <summary>
        /// 分组id，由微信分配
        /// </summary>
        public int id;
    }
}
