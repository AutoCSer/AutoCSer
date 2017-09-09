using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 匿名类型序列化 示例
    /// </summary>
    class AnonymousType
    {
        /// <summary>
        /// 匿名类型序列化 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(new { Value = 1 });
            var newValue = new { Value = 0 };
            AutoCSer.BinarySerialize.DeSerializer.DeSerialize(data, ref newValue);
            return newValue.Value == 1;
        }
    }
}
