using System;

namespace AutoCSer.Example.Json
{
    /// <summary>
    /// 匿名类型序列化 示例
    /// </summary>
    class AnonymousType
    {
        /// <summary>
        /// 字段成员
        /// </summary>
        public int Value;

        /// <summary>
        /// 匿名类型序列化 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            string json = AutoCSer.Json.Serializer.Serialize(new { Value = 1 });
            AnonymousType newValue = AutoCSer.Json.Parser.Parse<AnonymousType>(json);

            return newValue != null && newValue.Value == 1;
        }
    }
}
