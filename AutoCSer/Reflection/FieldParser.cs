using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 字段解析
    /// </summary>
    internal abstract class FieldParser<parserType, valueType>
        where parserType : class
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        protected FieldInfo field;
        /// <summary>
        /// 解析函数信息
        /// </summary>
        protected Func<parserType, object, object> method;
        /// <summary>
        /// 字段解析
        /// </summary>
        /// <param name="field"></param>
        /// <param name="methodInfo"></param>
        protected FieldParser(FieldInfo field, MethodInfo methodInfo)
        {
            this.field = field;
            if (methodInfo != null) createMethod(methodInfo);
        }
        /// <summary>
        /// 创建解析函数信息
        /// </summary>
        /// <param name="methodInfo"></param>
        protected void createMethod(MethodInfo methodInfo)
        {
            method = (Func<parserType, object, object>)typeof(AutoCSer.Reflection.InvokeMethodRef2<,>).MakeGenericType(typeof(parserType), field.FieldType).GetMethod("getTypeObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { methodInfo });
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parse(parserType parser, ref valueType value)
        {
            object fieldValue = field.GetValue(value), newValue = method(parser, fieldValue);
            if (!object.ReferenceEquals(newValue, fieldValue)) field.SetValue(value, newValue);
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseValue(parserType parser, ref valueType value)
        {
            object objectValue = value, fieldValue = field.GetValue(objectValue), newValue = method(parser, fieldValue);
            if (!object.ReferenceEquals(newValue, fieldValue)) field.SetValue(objectValue, newValue);
            value = (valueType)objectValue;
        }
    }
}
