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
        /// 属性解析（反射模式）
        /// </summary>
        private sealed class HeaderQueryPropertyParser : AutoCSer.Reflection.PropertyParser<HeaderQueryParser, valueType>
        {
            /// <summary>
            /// 属性信息
            /// </summary>
            /// <param name="property">字段信息</param>
            public HeaderQueryPropertyParser(PropertyInfo property) : base(property, HeaderQueryParser.GetParseMemberMethod(property.PropertyType)) { }
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
