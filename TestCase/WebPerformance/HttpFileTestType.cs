using System;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 测试类型
    /// </summary>
    internal enum TestType : byte
    {
        /// <summary>
        /// JSON 测试
        /// </summary>
        Json,
        /// <summary>
        /// 文件测试
        /// </summary>
        HelloFile,
        LoopEnd
    }
}
