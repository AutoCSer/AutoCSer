using System;

namespace AutoCSer.Example.Xml
{
    /// <summary>
    /// 自定义构造函数 示例
    /// </summary>
    abstract class NoConstructor
    {
        /// <summary>
        /// 数据字段
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 自定义构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object checkConstructor(Type type)
        {
            return type == typeof(NoConstructor) ? new NoConstructorSon() : null;
        }
        /// <summary>
        /// 自定义构造函数 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            NoConstructorSon value = new NoConstructorSon { Value = 1 };
            string xml = AutoCSer.Xml.Serializer.Serialize(value);

            AutoCSer.Xml.ParseConfig parseConfig = new AutoCSer.Xml.ParseConfig { Constructor = checkConstructor };
            NoConstructor newValue = AutoCSer.Xml.Parser.Parse<NoConstructor>(xml, parseConfig);
            return newValue != null && newValue.Value == 1;
        }
    }
    /// <summary>
    /// 序列化数据类型
    /// </summary>
    class NoConstructorSon : NoConstructor { }
}
