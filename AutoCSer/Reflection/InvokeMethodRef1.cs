using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 委托定义
    /// </summary>
    /// <typeparam name="valueType1"></typeparam>
    /// <typeparam name="valueType2"></typeparam>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    internal delegate void ActionRef1<valueType1, valueType2>(ref valueType1 value1, valueType2 value2);
    /// <summary>
    /// 反射调用函数
    /// </summary>
    /// <typeparam name="valueType1"></typeparam>
    /// <typeparam name="valueType2"></typeparam>
    internal sealed class InvokeMethodRef1<valueType1, valueType2>
    {
        /// <summary>
        /// 函数委托
        /// </summary>
        private ActionRef1<valueType1, valueType2> method;
        /// <summary>
        /// 反射调用函数
        /// </summary>
        /// <param name="method">函数委托</param>
        private InvokeMethodRef1(ActionRef1<valueType1, valueType2> method)
        {
            this.method = method;
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static ActionRef1<valueType1, valueType2> get(MethodInfo method)
        {
            return (ActionRef1<valueType1, valueType2>)Delegate.CreateDelegate(typeof(ActionRef1<valueType1, valueType2>), method);
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        private object objectTypeReturn(object value1, valueType2 value2)
        {
            valueType1 value = (valueType1)value1;
            method(ref value, value2);
            return value;
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<object, valueType2, object> getObjectTypeReturn(MethodInfo method)
        {
            return new InvokeMethodRef1<valueType1, valueType2>((ActionRef1<valueType1, valueType2>)Delegate.CreateDelegate(typeof(ActionRef1<valueType1, valueType2>), method)).objectTypeReturn;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        private object objectTypeReturnDefault(object value1, valueType2 value2)
        {
            valueType1 value = default(valueType1);
            method(ref value, value2);
            return value;
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<object, valueType2, object> getObjectTypeReturnDefault(MethodInfo method)
        {
            return new InvokeMethodRef1<valueType1, valueType2>((ActionRef1<valueType1, valueType2>)Delegate.CreateDelegate(typeof(ActionRef1<valueType1, valueType2>), method)).objectTypeReturnDefault;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        private void objectType(object value1, valueType2 value2)
        {
            valueType1 value = (valueType1)value1;
            method(ref value, value2);
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Action<object, valueType2> getObjectType(MethodInfo method)
        {
            return new InvokeMethodRef1<valueType1, valueType2>((ActionRef1<valueType1, valueType2>)Delegate.CreateDelegate(typeof(ActionRef1<valueType1, valueType2>), method)).objectType;
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
            method(ref value, (valueType2)value2);
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
            return new InvokeMethodRef1<valueType1, valueType2>((ActionRef1<valueType1, valueType2>)Delegate.CreateDelegate(typeof(ActionRef1<valueType1, valueType2>), method)).objectObjectReturn;
        }
    }
}
