using System;
using System.Reflection;

namespace AutoCSer.Net.TcpStreamServer.Emit
{
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
    /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送</typeparam>
    internal sealed partial class Method<attributeType, methodAttributeType, serverSocketSenderType> : TcpServer.Emit.Method<Method<attributeType, methodAttributeType, serverSocketSenderType>, attributeType, methodAttributeType, serverSocketSenderType>
        where attributeType : ServerAttribute
        where methodAttributeType : TcpStreamServer.MethodAttribute
        where serverSocketSenderType : TcpStreamServer.ServerSocketSender
    {
        /// <summary>
        /// 服务端需要应答客户端请求
        /// </summary>
        internal readonly bool IsClientSendOnly;
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <param name="method">方法信息</param>
        /// <param name="attribute">TCP 服务器端配置</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        /// <param name="isClient">是否客户端</param>
        internal Method(Type type, MethodInfo method, attributeType attribute, methodAttributeType serverMethodAttribute, bool isClient)
            : base(type, method, attribute, serverMethodAttribute, isClient)
        {
            IsClientSendOnly = Attribute.GetIsClientSendOnly && !IsAsynchronousCallback && ReturnType == typeof(void) && ClientParameter == null;
        }
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <param name="property">属性信息</param>
        /// <param name="attribute">TCP 服务器端配置</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        /// <param name="isSet">是否设置</param>
        internal Method(Type type, PropertyInfo property, attributeType attribute, methodAttributeType serverMethodAttribute, bool isSet = false)
            : base(type, property, attribute, serverMethodAttribute, isSet)
        {
            if (!isSet && property.CanWrite) PropertySetMethod = new Method<attributeType, methodAttributeType, serverSocketSenderType>(this, serverMethodAttribute);
            IsClientSendOnly = Attribute.GetIsClientSendOnly && ReturnType == typeof(void) && ClientParameter == null;
        }
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="propertyGetMethod">属性获取 TCP 函数信息</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        private Method(Method<attributeType, methodAttributeType, serverSocketSenderType> propertyGetMethod, methodAttributeType serverMethodAttribute)
            : this(propertyGetMethod.Type, propertyGetMethod.PropertyInfo, propertyGetMethod.ServerAttribute, serverMethodAttribute, true)
        {
            PropertyGetMethod = propertyGetMethod;
        }
        /// <summary>
        /// 引用参数检测
        /// </summary>
        /// <param name="errorString"></param>
        /// <returns></returns>
        internal override bool CheckRef(ref string errorString)
        {
            if (IsAsynchronousCallback)
            {
                errorString = "TCP 应答流服务不支持异步方法 " + MethodInfo.Name;
                return false;
            }
            if (IsClientSendOnly)
            {
                foreach (ParameterInfo parameter in Parameters)
                {
                    if (parameter.ParameterType.IsByRef)
                    {
                        errorString = "异步/仅应答 方法 " + MethodInfo.Name + " 不支持 ref / out 参数 " + parameter.Name;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
