using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 禁用成员位图 示例
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
    class DisabledMemberMap
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
        /// 禁用成员位图 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            DisabledMemberMap value = new DisabledMemberMap { Value1 = 1, Value2 = 2, Value3 = 3 };

            AutoCSer.Metadata.MemberMap<DisabledMemberMap> serializeMemberMap = AutoCSer.Metadata.MemberMap<DisabledMemberMap>.NewEmpty();
#if DOTNET2
            serializeMemberMap.SetMember("Value1");//添加成员 Value1
            serializeMemberMap.SetMember("Value2");//添加成员 Value2
#else
            serializeMemberMap.SetMember(member => member.Value1);//添加成员 Value1
            serializeMemberMap.SetMember(member => member.Value2);//添加成员 Value2
#endif
            AutoCSer.BinarySerialize.SerializeConfig serializeMemberMapConfig = new AutoCSer.BinarySerialize.SerializeConfig { MemberMap = serializeMemberMap };

            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(value, serializeMemberMapConfig);
            DisabledMemberMap newValue = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<DisabledMemberMap>(data);

            return newValue != null && newValue.Value1 == 1 && newValue.Value2 == 2 && newValue.Value3 == 3;
        }
    }
}
