using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 反射调用函数
    /// </summary>
    /// <typeparam name="valueType1"></typeparam>
    /// <typeparam name="valueType2"></typeparam>
    internal sealed class InvokeMethod<valueType1, valueType2>
    {
        /// <summary>
        /// 函数委托
        /// </summary>
        private Action<valueType1, valueType2> method;
        /// <summary>
        /// 反射调用函数
        /// </summary>
        /// <param name="method">函数委托</param>
        private InvokeMethod(Action<valueType1, valueType2> method)
        {
            this.method = method;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        private void typeObject(valueType1 value1, object value2)
        {
            method(value1, (valueType2)value2);
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Action<valueType1, object> getTypeObject(MethodInfo method)
        {
            return new InvokeMethod<valueType1, valueType2>((Action<valueType1, valueType2>)Delegate.CreateDelegate(typeof(Action<valueType1, valueType2>), method)).typeObject;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        private object objectObjectReturn(object value1, object value2)
        {
            valueType1 value = (valueType1)value1;
            method(value, (valueType2)value2);
            return value;
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<object, object, object> getObjectObjectReturn(MethodInfo method)
        {
            return new InvokeMethod<valueType1, valueType2>((Action<valueType1, valueType2>)Delegate.CreateDelegate(typeof(Action<valueType1, valueType2>), method)).objectObjectReturn;
        }
    }
}
