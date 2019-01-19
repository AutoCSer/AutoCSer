using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 反射调用函数
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <typeparam name="returnType"></typeparam>
    internal sealed class InvokeMethodReturn<valueType, returnType>
    {
        /// <summary>
        /// 函数委托
        /// </summary>
        private Func<valueType, returnType> method;
        /// <summary>
        /// 反射调用函数
        /// </summary>
        /// <param name="method">函数委托</param>
        private InvokeMethodReturn(Func<valueType, returnType> method)
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
            return method((valueType)value);
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<object, object> getObjectReturn(MethodInfo method)
        {
            return new InvokeMethodReturn<valueType, returnType>((Func<valueType, returnType>)Delegate.CreateDelegate(typeof(Func<valueType, returnType>), method)).objectReturn;
        }
    }
    /// <summary>
    /// 反射调用函数
    /// </summary>
    /// <typeparam name="valueType1"></typeparam>
    /// <typeparam name="valueType2"></typeparam>
    /// <typeparam name="returnType"></typeparam>
    internal sealed class InvokeMethodReturn<valueType1, valueType2, returnType>
    {
        /// <summary>
        /// 函数委托
        /// </summary>
        private Func<valueType1, valueType2, returnType> method;
        /// <summary>
        /// 反射调用函数
        /// </summary>
        /// <param name="method">函数委托</param>
        private InvokeMethodReturn(Func<valueType1, valueType2, returnType> method)
        {
            this.method = method;
        }
        /// <summary>
        /// 调用函数委托
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        private returnType typeObjectReturnType(valueType1 value1, object value2)
        {
            return method(value1, (valueType2)value2);
        }
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Func<valueType1, object, returnType> getTypeObjectReturnType(MethodInfo method)
        {
            return new InvokeMethodReturn<valueType1, valueType2, returnType>((Func<valueType1, valueType2, returnType>)Delegate.CreateDelegate(typeof(Func<valueType1, valueType2, returnType>), method)).typeObjectReturnType;
        }
    }
}
