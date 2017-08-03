using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 循环引用支持 示例
    /// </summary>
    class Reference
    {
        /// <summary>
        /// 循环引用成员
        /// </summary>
        Reference This;

        /// <summary>
        /// 循环引用支持 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            Reference value = new Reference();
            value.This = value;//构造循环引用

            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(value);
            Reference newValue = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Reference>(data);

            return newValue != null && newValue.This == newValue;
        }
    }
}
