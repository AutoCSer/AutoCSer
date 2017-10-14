using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 提交测试
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct JudgeQuery
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string token;
        /// <summary>
        /// 提交测试
        /// </summary>
        public Judge judge;
    }
}
