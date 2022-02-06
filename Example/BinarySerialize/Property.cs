using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 匿名字段绑定属性 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsAnonymousFields = true)]
    class Property
    {
        /// <summary>
        /// 匿名字段属性
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// 匿名字段绑定属性 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(new Property { Value = 1 });
            Property newValue = AutoCSer.BinaryDeSerializer.DeSerialize<Property>(data);
            return newValue != null && newValue.Value == 1;
        }
    }
}
