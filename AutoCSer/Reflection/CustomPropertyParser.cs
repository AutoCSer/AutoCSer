using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 属性解析（反射模式）
    /// </summary>
    /// <typeparam name="parserType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    internal abstract class CustomPropertyParser<parserType, valueType> : PropertyParser<parserType, valueType>
        where parserType : class
    {
        /// <summary>
        /// 自定义解析函数信息
        /// </summary>
        protected Func<object, parserType, object> customMethod;
        /// <summary>
        /// 属性解析
        /// </summary>
        /// <param name="property"></param>
        protected CustomPropertyParser(PropertyInfo property) : base(property, null)
        {
            bool isCustom = false;
            MethodInfo method = getMethodInfo(property.PropertyType, ref isCustom);
            if (isCustom) customMethod = (Func<object, parserType, object>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(property.PropertyType, typeof(parserType)).GetMethod(getMethod == null ? "getObjectTypeReturnDefault" : "getObjectTypeReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
            else createMethod(property, method);
        }
        /// <summary>
        /// 获取函数信息
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="isCustom"></param>
        /// <returns></returns>
        protected abstract MethodInfo getMethodInfo(Type propertyType, ref bool isCustom);
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseCustom(parserType parser, ref valueType value)
        {
            setMethod(value, customMethod(getMethod(value), parser));
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseValueCustom(parserType parser, ref valueType value)
        {
            object objectValue = value;
            value = (valueType)setMethod(objectValue, customMethod(getMethod(objectValue), parser));
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseCustomDefault(parserType parser, ref valueType value)
        {
            setMethod(value, customMethod(null, parser));
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseValueCustomDefault(parserType parser, ref valueType value)
        {
            object objectValue = value;
            value = (valueType)setMethod(objectValue, customMethod(null, parser));
        }
    }
}
