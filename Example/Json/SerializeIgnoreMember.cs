using System;

namespace AutoCSer.Example.Json
{
    /// <summary>
    /// 序列化忽略成员 示例
    /// </summary>
    class SerializeIgnoreMember
    {
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Value;
        /// <summary>
        /// 公共字段成员
        /// </summary>
        [AutoCSer.JsonSerializeMember(IsIgnoreCurrent = true)]
        public int Ignore;

        /// <summary>
        /// 序列化忽略成员 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            SerializeIgnoreMember value = new SerializeIgnoreMember { Value = 1, Ignore = 2 };
            string json = AutoCSer.JsonSerializer.Serialize(value);
            NoIgnoreMember newValue = AutoCSer.JsonDeSerializer.DeSerialize<NoIgnoreMember>(json);
            if (newValue == null || newValue.Value != 1 || newValue.Ignore != 0)
            {
                return false;
            }

            newValue = new NoIgnoreMember { Value = 1, Ignore = 2 };
            json = AutoCSer.JsonSerializer.Serialize(newValue);
            value = AutoCSer.JsonDeSerializer.DeSerialize<SerializeIgnoreMember>(json);
            return value != null && value.Value == 1 && value.Ignore == 2;
        }
    }
}
