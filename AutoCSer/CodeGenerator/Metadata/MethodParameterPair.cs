using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 函数参数匹配信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MethodParameterPair
    {
        /// <summary>
        /// 函数参数
        /// </summary>
        public MethodParameter MethodParameter;
        /// <summary>
        /// 匹配参数
        /// </summary>
        public MethodParameter Parameter;
        /// <summary>
        /// 输出参数匹配输入参数
        /// </summary>
        public MethodParameter InputMethodParameter;
        /// <summary>
        /// 输出参数匹配输入参数
        /// </summary>
        public MethodParameter InputParameter;
        /// <summary>
        /// 参数引用前缀
        /// </summary>
        public string ParameterRef
        {
            get { return MethodParameter.ParameterRef; }
        }
        /// <summary>
        /// 设置函数参数匹配信息
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <param name="parameter"></param>
        private void set(MethodParameter methodParameter, MethodParameter parameter)
        {
            MethodParameter = methodParameter;
            Parameter = parameter;
        }
        /// <summary>
        /// 设置输出参数匹配输入参数
        /// </summary>
        /// <param name="inputParameters"></param>
        private void setInputParameter(MethodParameterPair[] inputParameters)
        {
            int parameterIndex = 0;
            foreach (MethodParameterPair parameter in inputParameters)
            {
                if (parameter.MethodParameter.ParameterName == MethodParameter.ParameterName)
                {
                    InputMethodParameter = parameter.MethodParameter;
                    InputParameter = parameter.Parameter;
                    inputParameters[parameterIndex].setInputParameter(MethodParameter, Parameter);
                    break;
                }
                ++parameterIndex;
            }
        }
        /// <summary>
        /// 设置输出参数匹配输入参数
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <param name="parameter"></param>
        private void setInputParameter(MethodParameter methodParameter, MethodParameter parameter)
        {
            InputMethodParameter = methodParameter;
            InputParameter = parameter;
        }

        /// <summary>
        /// 获取函数参数匹配信息
        /// </summary>
        /// <param name="methodParameters"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static MethodParameterPair[] get(MethodParameter[] methodParameters, MethodParameter[] parameters)
        {
            if (methodParameters.Length == 0) return NullValue<MethodParameterPair>.Array;
            MethodParameterPair[] methodParameterPairs = new MethodParameterPair[methodParameters.Length];
            int index = 0;
            foreach (MethodParameter methodParameter in methodParameters)
            {
                methodParameterPairs[index].set(methodParameter, parameters[index]);
                ++index;
            }
            return methodParameterPairs;
        }
        /// <summary>
        /// 获取函数参数匹配信息
        /// </summary>
        /// <param name="methodParameters"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static MethodParameterPair[] Get(MethodParameter[] methodParameters, MethodParameterTypes types)
        {
            MethodParameterPair[] parameters = get(methodParameters, types.Parameters);
            for (int index = 0, endIndex = methodParameters.Length - 1; index < endIndex; ++index)
            {
                MethodParameter methodParameter = methodParameters[index];
                for (int copyIndex = index; true; ++copyIndex)
                {
                    MethodParameter copyParameter = parameters[copyIndex].Parameter;
                    if (methodParameter.ParameterType == copyParameter.ParameterType)
                    {
                        if (index != copyIndex)
                        {
                            parameters[copyIndex].Parameter = parameters[index].Parameter;
                            parameters[index].Parameter = copyParameter;
                        }
                        break;
                    }
                }
            }
            return parameters;
        }
        /// <summary>
        /// 获取函数参数匹配信息
        /// </summary>
        /// <param name="methodParameters"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static MethodParameterPair[] Get(MethodParameter[] methodParameters, MethodParameterTypeNames types)
        {
            MethodParameterPair[] parameters = get(methodParameters, types.Parameters);
            for (int index = 0, endIndex = methodParameters.Length - 1; index < endIndex; ++index)
            {
                MethodParameter methodParameter = methodParameters[index];
                for (int copyIndex = index; true; ++copyIndex)
                {
                    MethodParameter copyParameter = parameters[copyIndex].Parameter;
                    if (methodParameter.ParameterType == copyParameter.ParameterType && methodParameter.ParameterName == copyParameter.ParameterName)
                    {
                        if (index != copyIndex)
                        {
                            parameters[copyIndex].Parameter = parameters[index].Parameter;
                            parameters[index].Parameter = copyParameter;
                        }
                        break;
                    }
                }
            }
            return parameters;
        }
        /// <summary>
        /// 设置输出参数匹配输入参数
        /// </summary>
        /// <param name="outputParameters"></param>
        /// <param name="inputParameters"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void SetInputParameter(MethodParameterPair[] outputParameters, MethodParameterPair[] inputParameters)
        {
            for (int index = outputParameters.Length; index != 0; outputParameters[--index].setInputParameter(inputParameters)) ;
        }
    }
}
