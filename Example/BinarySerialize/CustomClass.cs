using System;

namespace AutoCSer.Example.BinarySerialize
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
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void Serialize(AutoCSer.BinarySerializer serializer, CustomClass value)
        {
            serializer.Stream.Write(value.Value == null ? 1 : 2);
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="value">目标数据，可能为 null</param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static unsafe void deSerialize(AutoCSer.BinaryDeSerializer deSerializer, ref CustomClass value)
        {
            byte* read = deSerializer.CustomRead;
            if (deSerializer.MoveReadAny(sizeof(int)))
            {
                switch (*(int*)read)
                {
                    case 1: value = new CustomClass { Value = null }; return;
                    case 2: value = new CustomClass { Value = string.Empty }; return;
                    default: deSerializer.MoveRead(-1); return;
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
            byte[] data = AutoCSer.BinarySerializer.Serialize(new CustomClass { Value = null });
            CustomClass newValue = AutoCSer.BinaryDeSerializer.DeSerialize<CustomClass>(data);
            if (newValue == null || newValue.Value != null)
            {
                return false;
            }

            data = AutoCSer.BinarySerializer.Serialize(new CustomClass { Value = string.Empty });
            newValue = AutoCSer.BinaryDeSerializer.DeSerialize<CustomClass>(data);
            return newValue != null && newValue.Value == string.Empty;
        }
    }
}
