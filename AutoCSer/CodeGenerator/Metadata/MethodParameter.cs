using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 函数参数信息
    /// </summary>
    internal sealed partial class MethodParameter
    {
        /// <summary>
        /// 定义方法
        /// </summary>
        private MethodInfo method;
        /// <summary>
        /// 参数信息
        /// </summary>
        public ParameterInfo Parameter { get; private set; }
        /// <summary>
        /// 参数索引位置
        /// </summary>
        public int ParameterIndex { get; private set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ExtensionType ParameterType { get; private set; }
        /// <summary>
        /// 函数泛型参数类型
        /// </summary>
        public ExtensionType GenericParameterType { get; private set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName;
        /// <summary>
        /// 值类型参数名称
        /// </summary>
        public string ValueTypeParameterName
        {
            get { return ParameterName; }
        }
        /// <summary>
        /// 参数连接逗号，最后一个参数为null
        /// </summary>
        public string ParameterJoin;
        /// <summary>
        /// 参数连接名称，最后一个参数不带逗号
        /// </summary>
        public string ParameterJoinName
        {
            get
            {
                return ParameterName + ParameterJoin;
            }
        }
        /// <summary>
        /// 带引用修饰的参数连接名称，最后一个参数不带逗号
        /// </summary>
        public string ParameterJoinRefName
        {
            get
            {
                return getRefName(ParameterJoinName);
            }
        }
        /// <summary>
        /// 带引用修饰的参数名称
        /// </summary>
        public string ParameterTypeRefName
        {
            get
            {
                return getRefName(ParameterType.FullName);
            }
        }
        /// <summary>
        /// 带引用修饰的参数名称
        /// </summary>
        public string ParameterRefName
        {
            get
            {
                return getRefName(ParameterName);
            }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        private string xmlDocument;
        /// <summary>
        /// XML文档注释
        /// </summary>
        public string XmlDocument
        {
            get
            {
                if (xmlDocument == null)
                {
                    xmlDocument = Parameter == null ? string.Empty : AutoCSer.CodeGenerator.XmlDocument.Get(method, Parameter);
                }
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ref
        {
            get { return "ref "; }
        }
        /// <summary>
        /// 是否引用参数
        /// </summary>
        public bool IsRef;
        /// <summary>
        /// 是否输出参数
        /// </summary>
        public bool IsOut { get; private set; }
        /// <summary>
        /// 是否输出参数
        /// </summary>
        public bool IsRefOrOut
        {
            get { return IsRef || IsOut; }
        }
        /// <summary>
        /// 参数引用前缀
        /// </summary>
        public string ParameterRef
        {
            get
            {
                return getRefName(null);
            }
        }
        /// <summary>
        /// 是否属性值参数
        /// </summary>
        public bool IsPropertyValue;
        /// <summary>
        /// 参数信息
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <param name="genericParameters">方法泛型参数集合</param>
        /// <param name="parameter">参数信息</param>
        /// <param name="index">参数索引位置</param>
        /// <param name="isLast">是否最后一个参数</param>
        private MethodParameter(MethodInfo method, Type[] genericParameters, ParameterInfo parameter, int index, bool isLast)
        {
            this.method = method;
            Parameter = parameter;
            ParameterIndex = index;
            Type parameterType = parameter.ParameterType;
            if (parameterType.IsByRef)
            {
                if (parameter.IsOut) IsOut = true;
                else IsRef = true;
                ParameterType = parameterType.GetElementType();
            }
            else ParameterType = parameterType;
            GenericParameterType = ParameterType.Type.IsGenericParameter && Array.IndexOf(genericParameters, ParameterType) != -1 ? (ExtensionType)typeof(object) : ParameterType;
            ParameterName = Parameter.Name;
            ParameterJoin = isLast ? null : ", ";
        }
        /// <summary>
        /// 参数信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterName"></param>
        internal MethodParameter(MethodParameter parameter, string parameterName = null)
        {
            method = parameter.method;
            Parameter = parameter.Parameter;
            ParameterIndex = parameter.ParameterIndex;
            ParameterType = parameter.ParameterType;
            GenericParameterType = parameter.GenericParameterType;
            IsOut = parameter.IsOut;
            IsRef = parameter.IsRef;
            ParameterName = parameterName ?? parameter.ParameterName;
            ParameterJoin = parameter.ParameterJoin;
            IsPropertyValue = parameter.IsPropertyValue;
        }
        /// <summary>
        /// 参数信息
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="type">参数类型</param>
        public MethodParameter(string name, Type type)
        {
            ParameterName = name;
            ParameterType = GenericParameterType = type;
        }
        /// <summary>
        /// 获取带引用修饰的名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>带引用修饰的名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private string getRefName(string name)
        {
            if (IsOut) return "out " + name;
            if (IsRef) return Ref + name;
            return name;
        }
        /// <summary>
        /// 获取方法参数信息集合
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="genericParameters">方法泛型参数集合</param>
        /// <returns>参数信息集合</returns>
        internal static MethodParameter[] Get(MethodInfo method, Type[] genericParameters)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.isEmpty()) return NullValue<MethodParameter>.Array;
            int index = 0;
            return parameters.getArray(value => new MethodParameter(method, genericParameters, value, index, ++index == parameters.Length));
        }
        /// <summary>
        /// 获取方法参数信息集合
        /// </summary>
        /// <param name="parameters">参数信息集合</param>
        /// <returns>参数信息集合</returns>
        internal static MethodParameter[] Get(MethodParameter[] parameters)
        {
            if (!parameters.isEmpty())
            {
                MethodParameter parameter = parameters[parameters.Length - 1];
                (parameters[parameters.Length - 1] = new MethodParameter(parameter)).ParameterJoin = null;
            }
            return parameters;
        }
        /// <summary>
        /// 获取方法输入参数信息集合
        /// </summary>
        /// <param name="parameters">参数信息集合</param>
        /// <returns>输入参数信息集合</returns>
        internal static MethodParameter[] GetInput(MethodParameter[] parameters)
        {
            if (parameters.any(value => value.IsOut))
            {
                parameters = parameters.getFindArray(value => !value.IsOut);
                if (parameters.Length != 0)
                {
                    MethodParameter parameter = parameters[parameters.Length - 1];
                    (parameters[parameters.Length - 1] = new MethodParameter(parameter)).ParameterJoin = null;
                }
            }
            return parameters;
        }
    }
}
