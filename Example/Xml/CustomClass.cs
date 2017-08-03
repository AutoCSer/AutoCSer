using System;

namespace AutoCSer.Example.Xml
{
    /// <summary>
    /// 引用类型自定义序列化函数 示例
    /// </summary>
    class CustomClass
    {
        /// <summary>
        /// 字段数据
        /// </summary>
        public string Value;

        /// <summary>
        /// 自定义序列化函数
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.Xml.Custom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void Serialize(AutoCSer.Xml.Serializer serializer, CustomClass value)
        {
            serializer.CharStream.Write(value.Value == null ? '1' : '2');
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value">目标数据，可能为 null</param>
        [AutoCSer.Xml.Custom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static unsafe void deSerialize(AutoCSer.Xml.Parser parser, ref CustomClass value)
        {
            char* read = parser.CustomRead;
            if (parser.VerifyRead(1))
            {
                switch (*(char*)read)
                {
                    case '1': value = new CustomClass { Value = null }; return;
                    case '2': value = new CustomClass { Value = string.Empty }; return;
                    default: parser.MoveRead(-1); return;
                }
            }
        }

        /// <summary>
        /// 引用类型自定义序列化函数 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            string xml = AutoCSer.Xml.Serializer.Serialize(new CustomClass { Value = null });
            CustomClass newValue = AutoCSer.Xml.Parser.Parse<CustomClass>(xml);
            if (newValue == null || newValue.Value != null)
            {
                return false;
            }

            xml = AutoCSer.Xml.Serializer.Serialize(new CustomClass { Value = string.Empty });
            newValue = AutoCSer.Xml.Parser.Parse<CustomClass>(xml);
            return newValue != null && newValue.Value == string.Empty;
        }
    }
}
