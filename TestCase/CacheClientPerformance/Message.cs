using System;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 测试消息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    internal sealed class Message
    {
        /// <summary>
        /// 消息数据
        /// </summary>
        internal int Value;
    }
}
