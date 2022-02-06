﻿using System;

namespace AutoCSer.Example.Xml
{
    /// <summary>
    /// 成员位图对象绑定
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    struct MemberMapValue<valueType>
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        internal AutoCSer.Metadata.MemberMap<valueType> MemberMap;
        /// <summary>
        /// 目标数据
        /// </summary>
        internal valueType Value;
        /// <summary>
        /// 自定义序列化函数
        /// </summary>
        /// <param name="serializer">XML 序列化</param>
        [AutoCSer.Xml.Custom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void ToXml(AutoCSer.XmlSerializer serializer)
        {
            if (MemberMap == null || MemberMap.IsDefault) serializer.TypeSerialize(Value);
            else
            {
                AutoCSer.Metadata.MemberMap memberMap = serializer.SetCustomMemberMap(MemberMap);
                try
                {
                    serializer.TypeSerialize(Value);
                }
                finally { serializer.SetCustomMemberMap(memberMap); }
            }
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="xmlDeSerializer">XML 解析</param>
        [AutoCSer.Xml.Custom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void ParseXml(AutoCSer.XmlDeSerializer xmlDeSerializer)
        {
            if (MemberMap == null) MemberMap = AutoCSer.Metadata.MemberMap<valueType>.NewEmpty();
            xmlDeSerializer.MemberMap = MemberMap;
            xmlDeSerializer.TypeDeSerialize(ref Value);
        }
    }
    /// <summary>
    /// 成员位图数据对象 示例
    /// </summary>
    class MemberMapValue
    {
        /// <summary>
        /// 成员位图对象绑定
        /// </summary>
        public MemberMapValue<MemberMap> Value;

        /// <summary>
        /// 成员位图数据对象 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            AutoCSer.Metadata.MemberMap<MemberMap> serializeMemberMap = AutoCSer.Metadata.MemberMap<MemberMap>.NewEmpty();
#if DOTNET2
            serializeMemberMap.SetMember("Value1");//添加成员 Value1
            serializeMemberMap.SetMember("Value2");//添加成员 Value2
#else
            serializeMemberMap.SetMember(member => member.Value1);//添加成员 Value1
            serializeMemberMap.SetMember(member => member.Value2);//添加成员 Value2
#endif
            MemberMapValue value = new MemberMapValue { Value = new MemberMapValue<MemberMap> { MemberMap = serializeMemberMap, Value = new MemberMap { Value1 = 1, Value2 = 2, Value3 = 3 } } };

            string xml = AutoCSer.XmlSerializer.Serialize(value);
            MemberMapValue newValue = AutoCSer.XmlDeSerializer.DeSerialize<MemberMapValue>(xml);

            return newValue != null && newValue.Value.Value != null && newValue.Value.Value.Value1 == 1 && newValue.Value.Value.Value2 == 2 && newValue.Value.Value.Value3 == 0;
        }
    }
}
