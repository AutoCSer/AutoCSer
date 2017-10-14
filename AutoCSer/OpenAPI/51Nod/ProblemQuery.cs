using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 题目参数
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct ProblemQuery
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string token;
        /// <summary>
        /// 题目
        /// </summary>
        public Problem problem;
    }
}
