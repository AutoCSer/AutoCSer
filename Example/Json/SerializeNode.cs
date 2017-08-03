using System;

namespace AutoCSer.Example.Json
{
    /// <summary>
    /// JSON 节点解析 示例
    /// </summary>
    class SerializeNode
    {
        /// <summary>
        /// JSON 节点解析 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            var value = new { Number = 1.1, Bool = true, DateTime = new DateTime(2000, 1, 2, 3, 4, 5), String = @"大
小" };
            string json = AutoCSer.Json.Serializer.Serialize(value);
            AutoCSer.Json.Node node = AutoCSer.Json.Parser.Parse<AutoCSer.Json.Node>(json);
            return node.Type == AutoCSer.Json.NodeType.Dictionary && node["Number"].Number == value.Number && node["Bool"].Bool == value.Bool && node["DateTime"].DateTime == value.DateTime && node["String"].String == value.String;
        }
    }
}
