using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 设置行业
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct IndustryQuery
    {
        /// <summary>
        /// 公众号模板消息所属行业编号
        /// </summary>
        public string industry_id1;
        /// <summary>
        /// 公众号模板消息所属行业编号
        /// </summary>
        public string industry_id2;
    }
}
