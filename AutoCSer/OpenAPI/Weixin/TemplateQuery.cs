using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 获得模板ID
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct TemplateQuery
    {
        /// <summary>
        /// 模板库中模板的编号，有“TM**”和“OPENTMTM**”等形式
        /// </summary>
        public string template_id_short;
    }
}
