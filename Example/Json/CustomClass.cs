﻿using System;

namespace AutoCSer.Example.Json
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
        [AutoCSer.JsonSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void Serialize(AutoCSer.JsonSerializer serializer, CustomClass value)
        {
            serializer.CharStream.Write(value.Value == null ? '1' : '2');
        }
        /// <summary>
        /// 自定义反序列化函数
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value">目标数据，可能为 null</param>
        [AutoCSer.JsonDeSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static unsafe void deSerialize(AutoCSer.JsonDeSerializer parser, ref CustomClass value)
        {
            char* read = parser.CustomRead;
            if (parser.VerifyRead(1))
            {
                switch (*(char*)read)
                {
                    case '1': value = new CustomClass { Value = null }; return;
                    case '2': value = new CustomClass { Value = string.Empty }; return;
                    default: parser.MoveRead(-1); return;
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
            string json = AutoCSer.JsonSerializer.Serialize(new CustomClass { Value = null });
            CustomClass newValue = AutoCSer.JsonDeSerializer.DeSerialize<CustomClass>(json);
            if (newValue == null || newValue.Value != null)
            {
                return false;
            }

            json = AutoCSer.JsonSerializer.Serialize(new CustomClass { Value = string.Empty });
            newValue = AutoCSer.JsonDeSerializer.DeSerialize<CustomClass>(json);
            return newValue != null && newValue.Value == string.Empty;
        }
    }
}
