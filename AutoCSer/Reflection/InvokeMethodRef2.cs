using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 反射调用函数
    /// </summary>
    /// <typeparam name="valueType1"></typeparam>
    /// <typeparam name="valueType2"></typeparam>
    internal sealed class InvokeMethodRef2<valueType1, valueType2>
    {
        /// <summary>
        /// 委托定义
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        private delegate void ActionRef(valueType1 value1, ref valueType2 value2);
        /// <summary>
        /// 函数委托
        /// </summary>
        private ActionRef method;
        /// <summary>
        /// 反射调用函数
        /// </summary>
        /// <param name="method">函数委托</param>
        private InvokeMethodRef2(ActionRef method)
        {
            this.method = method;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        private object typeObjectReturn(valueType1 value1, object value2)
        {
            valueType2 value = (valueType2)value2;
            method(value1, ref value);
            return value;
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<valueType1, object, object> getTypeObjectReturn(MethodInfo method)
        {
            return new InvokeMethodRef2<valueType1, valueType2>((ActionRef)Delegate.CreateDelegate(typeof(ActionRef), method)).typeObjectReturn;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        private object typeObjectReturnDefault(valueType1 value1, object value2)
        {
            valueType2 value = default(valueType2);
            method(value1, ref value);
            return value;
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<valueType1, object, object> getTypeObjectReturnDefault(MethodInfo method)
        {
            return new InvokeMethodRef2<valueType1, valueType2>((ActionRef)Delegate.CreateDelegate(typeof(ActionRef), method)).typeObjectReturnDefault;
        }
    }
}
