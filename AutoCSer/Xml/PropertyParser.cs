using System;
using System.Reflection;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    internal unsafe static partial class TypeParser<valueType>
    {
        /// <summary>
        /// 属性解析（反射模式）
        /// </summary>
        private sealed class PropertyParser : AutoCSer.Reflection.CustomPropertyParser<Parser, valueType>
        {
            /// <summary>
            /// 属性解析
            /// </summary>
            /// <param name="property"></param>
            public PropertyParser(PropertyInfo property) : base(property) { }
            /// <summary>
            /// 获取函数信息
            /// </summary>
            /// <param name="propertyType"></param>
            /// <param name="isCustom"></param>
            /// <returns></returns>
            protected override MethodInfo getMethodInfo(Type propertyType, ref bool isCustom)
            {
                return ParseMethodCache.GetMemberMethodInfo(propertyType, ref isCustom);
            }
            /// <summary>
            /// 获取解析委托
            /// </summary>
            /// <returns></returns>
            public TryParse Parser()
            {
                if (getMethod == null) return typeof(valueType).IsValueType ? (method != null ? (TryParse)parseValueDefault : (TryParse)parseValueCustomDefault) : (method != null ? (TryParse)parseDefault : (TryParse)parseCustomDefault);
                return typeof(valueType).IsValueType ? (method != null ? (TryParse)parseValue : (TryParse)parseValueCustom) : (method != null ? (TryParse)parse : (TryParse)parseCustom);
            }
        }
    }
}

