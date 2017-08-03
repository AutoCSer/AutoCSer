using System;

namespace AutoCSer.Example.Json
{
    /// <summary>
    /// 值类型自定义序列化函数 示例
    /// </summary>
    struct CustomStruct
    {
        /// <summary>
        /// 字段数据
        /// </summary>
        public string Value;

        /// <summary>
        /// 自定义序列化函数
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.Json.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Serialize(AutoCSer.Json.Serializer serializer)
        {
            serializer.TypeSerialize(Value == null ? 1 : 2);
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="parser"></param>
        [AutoCSer.Json.ParseCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.Json.Parser parser)
        {
            switch (parser.TypeParse<int>())
            {
                case 1: Value = null; return;
                case 2: Value = string.Empty; return;
                default: parser.MoveRead(-1); return;
            }
        }

        /// <summary>
        /// 值类型自定义序列化函数 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            string json = AutoCSer.Json.Serializer.Serialize(new CustomStruct { Value = null });
            if (AutoCSer.Json.Parser.Parse<CustomStruct>(json).Value != null)
            {
                return false;
            }

            json = AutoCSer.Json.Serializer.Serialize(new CustomStruct { Value = string.Empty });
            return AutoCSer.Json.Parser.Parse<CustomStruct>(json).Value == string.Empty;
        }
    }
}
