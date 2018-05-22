using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 抽象类型与接口类型支持 示例
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsBaseType = true)]
    class RealType : AbstractType, InterfaceType
    {
        /// <summary>
        /// 抽象类型与接口类型支持 配置参数
        /// </summary>
        private static readonly AutoCSer.BinarySerialize.SerializeConfig realTypeConfig = new AutoCSer.BinarySerialize.SerializeConfig { IsRealType = true };
        /// <summary>
        /// 抽象类型与接口类型支持 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            AbstractType value = new RealType { Value = 1 };
            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(value, realTypeConfig);
            AbstractType newValue = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<AbstractType>(data);
            if (newValue == null || newValue.Value != 1)
            {
                return false;
            }

            InterfaceType value2 = new RealType { Value = 2 };
            data = AutoCSer.BinarySerialize.Serializer.Serialize(value2, realTypeConfig);
            AbstractType newValue2 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<InterfaceType>(data) as AbstractType;
            return newValue2 != null && newValue2.Value == 2;
        }
    }
    /// <summary>
    /// 抽象类型支持 示例
    /// </summary>
    abstract class AbstractType
    {
        /// <summary>
        /// 数据
        /// </summary>
        public int Value;
    }
    /// <summary>
    /// 接口类型支持 示例
    /// </summary>
    interface InterfaceType
    {
    }
}
