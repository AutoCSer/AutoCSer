using System;

namespace AutoCSer.Example.Xml
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
        [AutoCSer.Xml.Custom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Serialize(AutoCSer.XmlSerializer serializer)
        {
            serializer.TypeSerialize(Value == null ? 1 : 2);
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        [AutoCSer.Xml.Custom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.XmlDeSerializer xmlDeSerializer)
        {
            switch (xmlDeSerializer.TypeDeSerialize<int>())
            {
                case 1: Value = null; return;
                case 2: Value = string.Empty; return;
                default: xmlDeSerializer.MoveRead(-1); return;
            }
        }

        /// <summary>
        /// 值类型自定义序列化函数 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            string xml = AutoCSer.XmlSerializer.Serialize(new CustomStruct { Value = null });
            if (AutoCSer.XmlDeSerializer.DeSerialize<CustomStruct>(xml).Value != null)
            {
                return false;
            }

            xml = AutoCSer.XmlSerializer.Serialize(new CustomStruct { Value = string.Empty });
            return AutoCSer.XmlDeSerializer.DeSerialize<CustomStruct>(xml).Value == string.Empty;
        }
    }
}
