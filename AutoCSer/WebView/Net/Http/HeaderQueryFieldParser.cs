using System;
using System.Reflection;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 查询类型解析器
    /// </summary>
    internal unsafe static partial class HeaderQueryTypeParser<valueType>
    {
        /// <summary>
        /// 字段解析（反射模式）
        /// </summary>
        private sealed class HeaderQueryFieldParser : AutoCSer.Reflection.FieldParser<HeaderQueryParser, valueType>
        {
            /// <summary>
            /// 字段信息
            /// </summary>
            /// <param name="field">字段信息</param>
            public HeaderQueryFieldParser(FieldInfo field) : base(field, HeaderQueryParser.GetParseMemberMethod(field.FieldType)) { }
            /// <summary>
            /// 获取解析委托
            /// </summary>
            /// <returns></returns>
            public TryParse Parser()
            {
                return typeof(valueType).IsValueType ? (TryParse)parseValue : parse;
            }
        }
    }
}
