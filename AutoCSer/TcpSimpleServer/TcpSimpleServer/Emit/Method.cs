using System;
using System.Reflection;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
    /// <typeparam name="serverSocketType">TCP 服务套接字</typeparam>
    internal sealed partial class Method<attributeType, methodAttributeType, serverSocketType> : TcpServer.Emit.Method<Method<attributeType, methodAttributeType, serverSocketType>, attributeType, methodAttributeType, serverSocketType>
        where attributeType : TcpServer.ServerBaseAttribute
        where methodAttributeType : TcpServer.MethodBaseAttribute
        where serverSocketType : TcpSimpleServer.ServerSocket
    {
        /// <summary>
        /// 验证方法是否支持异步
        /// </summary>
        internal override bool IsVerifyMethodAsynchronous { get { return true; } }
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
            if (!isSet && property.CanWrite) PropertySetMethod = new Method<attributeType, methodAttributeType, serverSocketType>(this, serverMethodAttribute);
        }
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="propertyGetMethod">属性获取 TCP 函数信息</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        private Method(Method<attributeType, methodAttributeType, serverSocketType> propertyGetMethod, methodAttributeType serverMethodAttribute)
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
                errorString = "TCP 应答服务不支持异步方法 " + MethodInfo.Name;
                return false;
            }
            return true;
        }
    }
}
