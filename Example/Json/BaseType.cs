using System;

namespace AutoCSer.Example.Json
{
    /// <summary>
    /// 入侵派生类型 示例
    /// </summary>
    [AutoCSer.Json.Serialize(IsBaseType = true)]
    [AutoCSer.Json.Parse(IsBaseType = true)]
    class BaseType
    {
        /// <summary>
        /// 基类数据字段
        /// </summary>
        public int Value;

        /// <summary>
        /// 入侵派生类型 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            SonType value = new SonType { Value = 1, SonValue = 2 };
            string json = AutoCSer.Json.Serializer.Serialize(value);

            SonType newValue = AutoCSer.Json.Parser.Parse<SonType>(json);
            if (newValue == null || newValue.Value != 1 || newValue.SonValue != 0)
            {
                return false;
            }

            SonType2 newValue2 = AutoCSer.Json.Parser.Parse<SonType2>(json);
            return newValue2 != null && newValue2.Value == 1 && newValue2.SonValue == 0;
        }
    }
    /// <summary>
    /// 派生类型
    /// </summary>
    class SonType : BaseType
    {
        /// <summary>
        /// 派生类型数据字段
        /// </summary>
        public int SonValue;
    }
    /// <summary>
    /// 派生类型
    /// </summary>
    class SonType2 : BaseType
    {
        /// <summary>
        /// 派生类型数据字段
        /// </summary>
        public int SonValue;
    }
}
