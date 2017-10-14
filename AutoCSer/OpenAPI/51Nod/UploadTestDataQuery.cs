using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// Zip 文件模式修改或者添加测试数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct UploadTestDataQuery
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string token;
        /// <summary>
        /// 题目ID
        /// </summary>
        public int problemId;
    }
}
