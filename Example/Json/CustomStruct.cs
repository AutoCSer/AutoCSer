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
        [AutoCSer.JsonSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Serialize(AutoCSer.JsonSerializer serializer)
        {
            serializer.CallSerialize(Value == null ? 1 : 2);
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="parser"></param>
        [AutoCSer.JsonDeSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.JsonDeSerializer parser)
        {
            switch (parser.TypeDeSerialize<int>())
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
            string json = AutoCSer.JsonSerializer.Serialize(new CustomStruct { Value = null });
            if (AutoCSer.JsonDeSerializer.DeSerialize<CustomStruct>(json).Value != null)
            {
                return false;
            }

            json = AutoCSer.JsonSerializer.Serialize(new CustomStruct { Value = string.Empty });
            return AutoCSer.JsonDeSerializer.DeSerialize<CustomStruct>(json).Value == string.Empty;
        }
    }
}
