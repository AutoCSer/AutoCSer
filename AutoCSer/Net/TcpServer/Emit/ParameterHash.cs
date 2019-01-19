using System;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 参数集合
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ParameterHash : IEquatable<ParameterHash>
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        private ParameterInfo[] parameters;
        /// <summary>
        /// 类型
        /// </summary>
        private Type returnType;
        /// <summary>
        /// TCP 参数类型配置
        /// </summary>
        private ParameterFlag flag;
        /// <summary>
        /// 
        /// </summary>
        private int hashCode;
        /// <summary>
        /// TCP 参数类型
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <param name="returnType">类型</param>
        /// <param name="flag">TCP 参数类型配置</param>
        internal ParameterHash(ParameterInfo[] parameters, Type returnType, ParameterFlag flag)
        {
            this.parameters = parameters;
            this.returnType = returnType;
            this.flag = flag;
            hashCode = (int)flag;
            if (returnType != typeof(void)) returnType.GetHashCode();
            int index = 0;
            foreach (ParameterInfo parameter in parameters) hashCode ^= (parameter.elementType().GetHashCode() ^ parameter.Name.GetHashCode()) + index++;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ParameterHash other)
        {
            if (flag == other.flag && returnType == other.returnType)
            {
                if (parameters.Length == other.parameters.Length)
                {
                    int index = 0;
                    foreach (ParameterInfo otherParameter in other.parameters)
                    {
                        ParameterInfo parameter = parameters[index++];
                        if (parameter.ParameterType != otherParameter.ParameterType || parameter.Name != otherParameter.Name) return false;
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((ParameterType)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
