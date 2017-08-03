using System;

namespace AutoCSer.Example.Xml
{
    /// <summary>
    /// 成员位图选择 示例
    /// </summary>
    class MemberMap
    {
        /// <summary>
        /// 字段成员
        /// </summary>
        public int Value1;
        /// <summary>
        /// 字段成员
        /// </summary>
        public int Value2;
        /// <summary>
        /// 字段成员
        /// </summary>
        public int Value3;

        /// <summary>
        /// 成员位图选择 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            MemberMap value = new MemberMap { Value1 = 1, Value2 = 2, Value3 = 3 };

            //成员位图初始化代价比较大，应该根据需求重用该对象
            AutoCSer.Metadata.MemberMap<MemberMap> serializeMemberMap = AutoCSer.Metadata.MemberMap<MemberMap>.NewEmpty();
#if DOTNET2
            serializeMemberMap.SetMember("Value1");//添加成员 Value1
            serializeMemberMap.SetMember("Value2");//添加成员 Value2
#else
            serializeMemberMap.SetMember(member => member.Value1);//添加成员 Value1
            serializeMemberMap.SetMember(member => member.Value2);//添加成员 Value2
#endif
            AutoCSer.Xml.SerializeConfig serializeMemberMapConfig = new AutoCSer.Xml.SerializeConfig { MemberMap = serializeMemberMap };

            string xml = AutoCSer.Xml.Serializer.Serialize(value, serializeMemberMapConfig);
            MemberMap newValue = AutoCSer.Xml.Parser.Parse<MemberMap>(xml);

            return newValue != null && newValue.Value1 == 1 && newValue.Value2 == 2 && newValue.Value3 == 0;
        }
    }
}
