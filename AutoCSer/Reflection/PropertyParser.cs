using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 属性解析（反射模式）
    /// </summary>
    /// <typeparam name="parserType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    internal abstract class PropertyParser<parserType, valueType>
        where parserType : class
    {
        /// <summary>
        /// 获取函数信息
        /// </summary>
        protected Func<object, object> getMethod;
        /// <summary>
        /// 设置函数信息
        /// </summary>
        protected Func<object, object, object> setMethod;
        /// <summary>
        /// 解析函数信息
        /// </summary>
        protected Func<parserType, object, object> method;
        /// <summary>
        /// 属性解析
        /// </summary>
        /// <param name="property"></param>
        /// <param name="methodInfo"></param>
        protected PropertyParser(PropertyInfo property, MethodInfo methodInfo)
        {
            if (property.CanRead)
            {
                if (property.DeclaringType.IsValueType) getMethod = (Func<object, object>)typeof(AutoCSer.Reflection.InvokeMethodRefReturn<,>).MakeGenericType(property.DeclaringType, property.PropertyType).GetMethod("getObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { property.GetGetMethod(true) });
                else getMethod = (Func<object, object>)typeof(AutoCSer.Reflection.InvokeMethodReturn<,>).MakeGenericType(property.DeclaringType, property.PropertyType).GetMethod("getObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { property.GetGetMethod(true) });
            }
            if (property.DeclaringType.IsValueType) setMethod = (Func<object, object, object>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(property.DeclaringType, property.PropertyType).GetMethod("getObjectObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { property.GetSetMethod(true) });
            else setMethod = (Func<object, object, object>)typeof(AutoCSer.Reflection.InvokeMethod<,>).MakeGenericType(property.DeclaringType, property.PropertyType).GetMethod("getObjectObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { property.GetSetMethod(true) });
            if (methodInfo != null) createMethod(property, methodInfo);
        }
        /// <summary>
        /// 创建解析函数信息
        /// </summary>
        /// <param name="property"></param>
        /// <param name="methodInfo"></param>
        protected void createMethod(PropertyInfo property, MethodInfo methodInfo)
        {
            method = (Func<parserType, object, object>)typeof(AutoCSer.Reflection.InvokeMethodRef2<,>).MakeGenericType(typeof(parserType), property.PropertyType).GetMethod(getMethod == null ? "getTypeObjectReturnDefault" : "getTypeObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { methodInfo });
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parse(parserType parser, ref valueType value)
        {
            object fieldValue = getMethod(value), newValue = method(parser, fieldValue);
            if (!object.ReferenceEquals(newValue, fieldValue)) setMethod(value, newValue);
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseValue(parserType parser, ref valueType value)
        {
            object objectValue = value, fieldValue = getMethod(objectValue), newValue = method(parser, fieldValue);
            if (!object.ReferenceEquals(newValue, fieldValue)) objectValue = setMethod(objectValue, newValue);
            value = (valueType)objectValue;
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseDefault(parserType parser, ref valueType value)
        {
            setMethod(value, method(parser, null));
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        protected void parseValueDefault(parserType parser, ref valueType value)
        {
            object objectValue = value;
            value = (valueType)setMethod(objectValue, method(parser, null));
        }
    }
}
