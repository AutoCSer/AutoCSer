using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 字段解析
    /// </summary>
    internal abstract class CustomFieldParser<parserType, valueType> : FieldParser<parserType, valueType>
        where parserType : class
    {
        /// <summary>
        /// 自定义解析函数信息
        /// </summary>
        protected Func<object, parserType, object> customMethod;
        /// <summary>
        /// 字段解析
        /// </summary>
        /// <param name="field"></param>
        protected CustomFieldParser(FieldInfo field) : base(field, null)
        {
            bool isCustom = false;
            MethodInfo method = getMethod(field.FieldType, ref isCustom);
            if (isCustom) customMethod = (Func<object, parserType, object>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(field.FieldType, typeof(parserType)).GetMethod("getObjectTypeReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
            else createMethod(method);
        }
        /// <summary>
        /// 获取函数信息
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="isCustom"></param>
        /// <returns></returns>
        protected abstract MethodInfo getMethod(Type fieldType, ref bool isCustom);
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseCustom(parserType parser, ref valueType value)
        {
            field.SetValue(value, customMethod(field.GetValue(value), parser));
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseValueCustom(parserType parser, ref valueType value)
        {
            object objectValue = value;
            field.SetValue(objectValue, customMethod(field.GetValue(objectValue), parser));
            value = (valueType)objectValue;
        }
    }
}
