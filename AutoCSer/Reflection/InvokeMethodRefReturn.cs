using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 反射调用函数
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <typeparam name="returnType"></typeparam>
    internal sealed class InvokeMethodRefReturn<valueType, returnType>
    {
        /// <summary>
        /// 函数委托定义
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private delegate returnType refMethod(ref valueType value);
        /// <summary>
        /// 函数委托
        /// </summary>
        private refMethod method;
        /// <summary>
        /// 反射调用函数
        /// </summary>
        /// <param name="method">函数委托</param>
        private InvokeMethodRefReturn(refMethod method)
        {
            this.method = method;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object objectReturn(object value)
        {
            valueType parameter = (valueType)value;
            return method(ref parameter);
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<object, object> getObjectReturn(MethodInfo method)
        {
            return new InvokeMethodRefReturn<valueType, returnType>((refMethod)Delegate.CreateDelegate(typeof(refMethod), method)).objectReturn;
        }
    }
}
