using System;

namespace AutoCSer.Example.BinarySerialize
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
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            serializer.TypeSerialize(Value == null ? 1 : 2);
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            switch (deSerializer.TypeDeSerialize<int>())
            {
                case 1: Value = null; return;
                case 2: Value = string.Empty; return;
                default: deSerializer.MoveRead(-1); return;
            }
        }

        /// <summary>
        /// 值类型自定义序列化函数 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(new CustomStruct { Value = null });
            if (AutoCSer.BinarySerialize.DeSerializer.DeSerialize<CustomStruct>(data).Value != null)
            {
                return false;
            }

            data = AutoCSer.BinarySerialize.Serializer.Serialize(new CustomStruct { Value = string.Empty });
            return AutoCSer.BinarySerialize.DeSerializer.DeSerialize<CustomStruct>(data).Value == string.Empty;
        }
    }
}
