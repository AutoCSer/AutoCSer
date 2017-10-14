using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 删除测试数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DeleteTestDataQuery
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string token;
        /// <summary>
        /// 题目ID
        /// </summary>
        public int problemId;
        /// <summary>
        /// 测试数据ID
        /// </summary>
        public byte testId;
    }
}
