using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 委托定义
    /// </summary>
    /// <typeparam name="valueType1"></typeparam>
    /// <typeparam name="valueType2"></typeparam>
    /// <typeparam name="valueType3"></typeparam>
    /// <typeparam name="returnType"></typeparam>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <param name="value3"></param>
    internal delegate returnType ActionRef13<valueType1, valueType2, valueType3, returnType>(ref valueType1 value1, valueType2 value2, ref valueType3 value3);
    /// <summary>
    /// 反射调用函数
    /// </summary>
    /// <typeparam name="valueType1"></typeparam>
    /// <typeparam name="valueType2"></typeparam>
    /// <typeparam name="valueType3"></typeparam>
    /// <typeparam name="returnType"></typeparam>
    internal sealed class InvokeMethodRef13<valueType1, valueType2, valueType3, returnType>
    {
        ///// <summary>
        ///// 函数委托
        ///// </summary>
        //private ActionRef13<valueType1, valueType2, valueType3> method;
        ///// <summary>
        ///// 反射调用函数
        ///// </summary>
        ///// <param name="method">函数委托</param>
        //private InvokeMethodRef13(ActionRef13<valueType1, valueType2, valueType3> method)
        //{
        //    this.method = method;
        //}
        /// <summary>
        /// 获取反射调用函数
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>反射调用函数</returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static ActionRef13<valueType1, valueType2, valueType3, returnType> get(MethodInfo method)
        {
            return (ActionRef13<valueType1, valueType2, valueType3, returnType>)Delegate.CreateDelegate(typeof(ActionRef13<valueType1, valueType2, valueType3, returnType>), method);
        }
    }
}
