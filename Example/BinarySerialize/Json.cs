using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// JSON 序列化扩展 示例。即使没有 JSON 序列化字段，也应该预留 JSON 序列化标记。
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsJson = true)]
    class Json
    {
        /// <summary>
        /// 二进制序列化字段
        /// </summary>
        public int Value;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerialize.JsonMember]
        public string Json1;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerialize.JsonMember]
        public string Json2;

        /// <summary>
        /// JSON 序列化扩展 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            Json value = new Json { Value = 1, Json1 = "1", Json2 = "2" };

            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(value);
            JsonDeSerialize newValue = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<JsonDeSerialize>(data);

            return newValue != null && newValue.Value == 1 && newValue.Json1 == 1 && newValue.Json2 == 2 && newValue.Json3 == 0;
        }
    }
    /// <summary>
    /// JSON 序列化扩展 示例 反序列化定义
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsJson = true)]
    class JsonDeSerialize
    {
        /// <summary>
        /// 二进制序列化字段
        /// </summary>
        public int Value;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerialize.JsonMember]
        public int Json1;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerialize.JsonMember]
        public int Json2;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerialize.JsonMember]
        public int Json3;
    }
}
