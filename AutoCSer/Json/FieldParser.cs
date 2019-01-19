using System;
using System.Reflection;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    internal unsafe static partial class TypeParser<valueType>
    {
        /// <summary>
        /// 字段解析（反射模式）
        /// </summary>
        private sealed class FieldParser : AutoCSer.Reflection.CustomFieldParser<Parser, valueType>
        {
            /// <summary>
            /// 字段解析
            /// </summary>
            /// <param name="field"></param>
            public FieldParser(FieldInfo field) : base(field) { }
            /// <summary>
            /// 获取函数信息
            /// </summary>
            /// <param name="fieldType"></param>
            /// <param name="isCustom"></param>
            /// <returns></returns>
            protected override MethodInfo getMethod(Type fieldType, ref bool isCustom)
            {
                return ParseMethodCache.GetMemberMethodInfo(fieldType, ref isCustom);
            }
            /// <summary>
            /// 获取解析委托
            /// </summary>
            /// <returns></returns>
            public TryParse Parser()
            {
                return typeof(valueType).IsValueType ? (method != null ? (TryParse)parseValue : (TryParse)parseValueCustom) : (method != null ? (TryParse)parse : (TryParse)parseCustom);
            }
        }
    }
}
