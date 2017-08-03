using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using AutoCSer.CodeGenerator.Metadata;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 异步信息
    /// </summary>
    internal abstract class AsynchronousMethod
    {
        /// <summary>
        /// 方法信息
        /// </summary>
        public Metadata.MethodIndex Method;
        /// <summary>
        /// 属性或者字段信息
        /// </summary>
        public AutoCSer.Metadata.MemberIndexInfo MemberIndex;
        /// <summary>
        /// 成员名称
        /// </summary>
        internal string MemberFullName
        {
            get
            {
                if (MemberIndex == null) return Method.Method.fullName();
                return MemberIndex.Member.DeclaringType.fullName() + "." + Method.MethodName;
            }
        }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ExtensionType methodReturnType { get; private set; }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ExtensionType MethodReturnType
        {
            get
            {
                checkAsynchronousReturn();
                return methodReturnType;
            }
        }
        /// <summary>
        /// 方法参数
        /// </summary>
        internal MethodParameter[] methodParameters;
        /// <summary>
        /// 方法参数
        /// </summary>
        public MethodParameter[] MethodParameters
        {
            get
            {
                checkAsynchronousReturn();
                return methodParameters;
            }
        }
        /// <summary>
        /// 第一个方法参数
        /// </summary>
        public MethodParameter FristParameter
        {
            get { return MethodParameters[0]; }
        }
        /// <summary>
        /// 输入参数
        /// </summary>
        public MethodParameter[] NotOutParameters
        {
            get { return MethodParameter.GetInput(MethodParameters); }
        }
        /// <summary>
        /// 输入参数匹配集合
        /// </summary>
        public MethodParameterPair[] InputParameters;
        /// <summary>
        /// 输出参数匹配集合
        /// </summary>
        public MethodParameterPair[] OutputParameters;
        /// <summary>
        /// 第一个方法参数是否值类型
        /// </summary>
        public bool IsFristParameterValueType
        {
            get
            {
                Type type = FristParameter.ParameterType;
                return type.IsValueType || type.IsEnum;
            }
        }
        /// <summary>
        /// 是否异步回调方法
        /// </summary>
        protected bool isAsynchronousCallback;
        /// <summary>
        /// 是否异步回调方法
        /// </summary>
        public bool IsAsynchronousCallback
        {
            get
            {
                checkAsynchronousReturn();
                return isAsynchronousCallback;
            }
        }
        /// <summary>
        /// 异步回调是否检测成功状态
        /// </summary>
        protected virtual bool isAsynchronousFunc { get { return true; } }
        /// <summary>
        /// 检测异步回调方法
        /// </summary>
        protected virtual void checkAsynchronousReturn()
        {
            if (methodParameters == null)
            {
                methodParameters = Method.Parameters;
                methodReturnType = Method.ReturnType;
                if (Method.ReturnType.Type == typeof(void) && methodParameters.Length != 0)
                {
                    Type type = methodParameters[methodParameters.Length - 1].ParameterType.Type;
                    if (type.IsGenericType)
                    {
                        Type parameterType = null;
                        if (isAsynchronousFunc && type.GetGenericTypeDefinition() == typeof(Func<,>))
                        {
                            Type[] types = type.GetGenericArguments();
                            if (types[1] == typeof(bool)) parameterType = types[0];
                        }
                        else if (type.GetGenericTypeDefinition() == typeof(Action<>)) parameterType = type.GetGenericArguments()[0];
                        if (parameterType != null)
                        {
                            if (parameterType == typeof(AutoCSer.Net.TcpServer.ReturnValue))
                            {
                                isAsynchronousCallback = true;
                            }
                            else if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(AutoCSer.Net.TcpServer.ReturnValue<>))
                            {
                                methodReturnType = parameterType.GetGenericArguments()[0];
                                isAsynchronousCallback = true;
                            }
                            if (isAsynchronousCallback)
                            {
                                methodParameters = MethodParameter.Get(methodParameters.getSub(0, methodParameters.Length - 1));
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 是否有返回值
        /// </summary>
        public bool MethodIsReturn
        {
            get
            {
                checkAsynchronousReturn();
                return methodReturnType.Type != typeof(void);
            }
        }
        /// <summary>
        /// 返回值参数名称
        /// </summary>
        public string ReturnName
        {
            get { return AutoCSer.Net.TcpServer.ReturnValue.ReturnParameterName; }
        }
        /// <summary>
        /// 属性或者字段名称
        /// </summary>
        public string PropertyName
        {
            get { return MemberIndex.Member.Name; }
        }
        /// <summary>
        /// 属性或者字段名称
        /// </summary>
        public string StaticPropertyName
        {
            get { return PropertyName; }
        }
    }
}
