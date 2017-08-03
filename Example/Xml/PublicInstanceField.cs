using System;

namespace AutoCSer.Example.Xml
{
    /// <summary>
    /// 仅选择公共字段成员 示例
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = AutoCSer.Metadata.MemberFilters.PublicInstanceField)]
    class PublicInstanceField
    {
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Public;
        /// <summary>
        /// 公共属性成员，被忽略
        /// </summary>
        public int Property { get; set; }
        /// <summary>
        /// 非公共字段成员，被忽略
        /// </summary>
        private int Private;
        /// <summary>
        /// 非公共字段成员，被忽略
        /// </summary>
        protected int Protected;
        /// <summary>
        /// 非公共字段成员，被忽略
        /// </summary>
        internal int Internal;

        /// <summary>
        /// 仅选择公共字段成员 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            PublicInstanceField value = new PublicInstanceField { Public = 1, Private = 2, Protected = 3, Internal = 4, Property = 5 };

            string xml = AutoCSer.Xml.Serializer.Serialize(value);
            PublicInstanceField newValue = AutoCSer.Xml.Parser.Parse<PublicInstanceField>(xml);

            return newValue != null && newValue.Public == 1 && newValue.Private == 0 && newValue.Protected == 0 && newValue.Internal == 0 && newValue.Property == 0;
        }
    }
}
