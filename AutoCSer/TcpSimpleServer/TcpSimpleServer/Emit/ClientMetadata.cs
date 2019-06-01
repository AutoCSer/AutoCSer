using System;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// TCP 客户端元数据
    /// </summary>
    internal abstract class ClientMetadata
    {
        /// <summary>
        /// TCP 客户端基类类型
        /// </summary>
        internal readonly Type MethodClientType;
        /// <summary>
        /// TCP 客户端类型名称
        /// </summary>
        internal readonly string ClientTypeName;
        /// <summary>
        /// TCP 客户端基类获取 TCP 客户端属性函数信息
        /// </summary>
        internal readonly MethodInfo MethodClientGetTcpClientMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientCallInputMethod;
        /// <summary>
        /// TCP 服务客户端调用函数信息
        /// </summary>
        internal readonly MethodInfo ClientCallMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientGetInputMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientGetMethod;
        /// <summary>
        /// 输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, ParameterGenericType> GetParameterGenericType;
        /// <summary>
        /// 输入+输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, Type, ParameterGenericType2> GetParameterGenericType2;
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        /// <param name="clientType">TCP 客户端类型</param>
        /// <param name="methodClientType">TCP 客户端基类类型</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="getParameterGenericType2">输入+输出参数泛型类型元数据</param>
        /// <param name="clientCallMethod">TCP 服务客户端调用函数信息</param>
        internal ClientMetadata(Type clientType, Type methodClientType
            , Func<Type, ParameterGenericType> getParameterGenericType, Func<Type, Type, ParameterGenericType2> getParameterGenericType2
            , MethodInfo clientCallMethod)
        {
            MethodClientType = methodClientType;
            ClientTypeName = clientType.fullName();
            MethodClientGetTcpClientMethod = methodClientType.GetProperty(TcpServer.Emit.ClientMetadataBase.TcpClientName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod();
            GetParameterGenericType = getParameterGenericType;
            GetParameterGenericType2 = getParameterGenericType2;
            ClientCallMethod = clientCallMethod;
            //foreach (MethodInfo method in clientType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            //{
            //    switch (method.Name)
            //    {
            //        case "Call":
            //            if (method.ReturnType == typeof(TcpServer.ReturnType))
            //            {
            //                if (method.IsGenericMethodDefinition) ClientCallInputMethod = method;
            //                else ClientCallMethod = method;
            //            }
            //            break;
            //        case "Get":
            //            if (method.ReturnType == typeof(TcpServer.ReturnType) && method.IsGenericMethodDefinition)
            //            {
            //                if (method.GetParameters().Length == 2) ClientGetMethod = method; 
            //                else ClientGetInputMethod = method;
            //            }
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// TCP 客户端基类 是否已经释放资源字段信息
        /// </summary>
        internal static readonly FieldInfo MethodClientIsDisposedField = typeof(TcpServer.Emit.MethodClient).GetField("_isDisposed_", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }
}
