﻿using System;

namespace AutoCSer.Example.Xml
{
    /// <summary>
    /// 忽略成员 示例
    /// </summary>
    class IgnoreMember
    {
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Value;
        /// <summary>
        /// 公共字段成员
        /// </summary>
        [AutoCSer.Xml.IgnoreMember]
        public int Ignore;

        /// <summary>
        /// 忽略成员 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            IgnoreMember value = new IgnoreMember { Value = 1, Ignore = 2 };
            string xml = AutoCSer.XmlSerializer.Serialize(value);
            NoIgnoreMember newValue = AutoCSer.XmlDeSerializer.DeSerialize<NoIgnoreMember>(xml);
            if (newValue == null || newValue.Value != 1 || newValue.Ignore != 0)
            {
                return false;
            }

            newValue = new NoIgnoreMember { Value = 1, Ignore = 2 };
            xml = AutoCSer.XmlSerializer.Serialize(newValue);
            value = AutoCSer.XmlDeSerializer.DeSerialize<IgnoreMember>(xml);
            return value != null && value.Value == 1 && value.Ignore == 0;
        }
    }
}
