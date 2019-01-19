using System;
using AutoCSer.Extension;
#if !NOJIT
using System.Reflection;
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Net.TcpServer
{
    ///// <summary>
    ///// 异步回调
    ///// </summary>
    ///// <typeparam name="outputParameterType">输出参数类型</typeparam>
    //internal sealed class CallbackReturnValue<outputParameterType> : Callback<ReturnValue<outputParameterType>>
    //{
    //    /// <summary>
    //    /// 回调委托
    //    /// </summary>
    //    private Action<ReturnValue> callback;
    //    /// <summary>
    //    /// 异步回调
    //    /// </summary>
    //    /// <param name="callback"></param>
    //    internal CallbackReturnValue(Action<ReturnValue> callback)
    //    {
    //        this.callback = callback;
    //    }
    //    /// <summary>
    //    /// 异步回调返回值
    //    /// </summary>
    //    /// <param name="outputParameter">输出参数</param>
    //    public override void Call(ref ReturnValue<outputParameterType> outputParameter)
    //    {
    //        callback(new ReturnValue { Type = outputParameter.Type });
    //    }
    //}
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal sealed class CallbackReturnValue<returnType, outputParameterType> : Callback<ReturnValue<outputParameterType>>
#if NOJIT
        where outputParameterType : IReturnParameter
#else
        where outputParameterType : IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 回调委托
        /// </summary>
        private Action<ReturnValue<returnType>> callback;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="callback"></param>
        internal CallbackReturnValue(Action<ReturnValue<returnType>> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        public override void Call(ref ReturnValue<outputParameterType> outputParameter)
        {
#if NOJIT
            callback(outputParameter.Type == ReturnType.Success ? new ReturnValue<returnType> { Type = ReturnType.Success, Value = (returnType)outputParameter.Value.ReturnObject } : new ReturnValue<returnType> { Type = outputParameter.Type });
#else
            //callback(outputParameter.Type == ReturnType.Success ? new ReturnValue<returnType> { Type = ReturnType.Success, Value = (returnType)outputParameter.Value.Return } : new ReturnValue<returnType> { Type = outputParameter.Type });
            callback(outputParameter.Type == ReturnType.Success ? new ReturnValue<returnType> { Type = ReturnType.Success, Value = getReturn(ref outputParameter.Value) } : new ReturnValue<returnType> { Type = outputParameter.Type });
#endif
        }
#if !NOJIT
        /// <summary>
        /// 获取返回值委托
        /// </summary>
        /// <param name="outputParamter">输出参数</param>
        /// <returns>返回值</returns>
        private delegate returnType getReturnValue(ref outputParameterType outputParamter);
        /// <summary>
        /// 获取返回值委托
        /// </summary>
        private static readonly getReturnValue getReturn;
        static CallbackReturnValue()
        {
            FieldInfo returnField = typeof(outputParameterType).GetField(ReturnValue.RetParameterName, BindingFlags.Instance | BindingFlags.Public);
            DynamicMethod dynamicMethod = new DynamicMethod("Get" + ReturnValue.RetParameterName, typeof(returnType), new Type[] { typeof(outputParameterType).MakeByRefType() }, typeof(outputParameterType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            if (!typeof(outputParameterType).IsValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldfld, returnField);
            generator.Emit(OpCodes.Ret);
            getReturn = (getReturnValue)dynamicMethod.CreateDelegate(typeof(getReturnValue));
        }
#endif
    }
}
