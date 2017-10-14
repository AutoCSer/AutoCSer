using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客服聊天记录查询
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct RecordQuery
    {
        /// <summary>
        /// 查询开始时间，UNIX时间戳
        /// </summary>
        public long starttime;
        /// <summary>
        /// 查询结束时间，UNIX时间戳，每次查询不能跨日查询
        /// </summary>
        public long endtime;
        /// <summary>
        /// 查询第几页，从1开始
        /// </summary>
        public int pageindex;
        /// <summary>
        /// 每页大小，每页最多拉取50条
        /// </summary>
        public byte pagesize;
    }
}
