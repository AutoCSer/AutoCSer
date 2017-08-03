using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 忽略成员 示例
    /// </summary>
    class IgnoreMember
    {
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Value;
        /// <summary>
        /// 公共字段成员
        /// </summary>
        [AutoCSer.BinarySerialize.IgnoreMember]
        public int Ignore;

        /// <summary>
        /// 忽略成员 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            IgnoreMember value = new IgnoreMember { Value = 1, Ignore = 2 };

            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(value);
            IgnoreMember newValue = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<IgnoreMember>(data);

            return newValue != null && newValue.Value == 1 && newValue.Ignore == 0;
        }
    }
}
