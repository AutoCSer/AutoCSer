using System;
using System.Reflection;
using AutoCSer.Net.TcpStreamServer.Emit;

namespace AutoCSer.Net.TcpOpenStreamServer.Emit
{
    /// <summary>
    /// TCP 服务
    /// </summary>
    internal static class Server
    {
        /// <summary>
        /// TCP 服务端元数据
        /// </summary>
        internal static readonly ServerMetadata Metadata = new ServerMetadata(typeof(TcpOpenStreamServer.Server), typeof(ServerAttribute), typeof(ServerSocketSender), typeof(ServerCall)
            , ParameterGenericType.Get
            , ((Func<bool>)ParameterGenericType.ServerSocketSender.Push).Method
            , ((Func<AutoCSer.Net.TcpServer.ReturnType, bool>)ParameterGenericType.ServerSocketSender.Push).Method
            , ((Action<Exception>)ParameterGenericType.ServerSocketSender.AddLog).Method);
    }
    /// <summary>
    /// TCP 服务
    /// </summary>
    /// <typeparam name="interfaceType">接口类型</typeparam>
    [AutoCSer.IOS.Preserve(AllMembers = false)]
    public static class Server<interfaceType>
    {
        /// <summary>
        /// 默认 TCP 服务配置
        /// </summary>
        private static readonly ServerAttribute defaultServerAttribute;
        /// <summary>
        /// 错误字符串提示信息
        /// </summary>
        private static readonly string errorString;
        /// <summary>
        /// TCP 函数集合
        /// </summary>
        private static readonly Method<ServerAttribute, MethodAttribute, ServerSocketSender>[] methods;
        /// <summary>
        /// TCP 服务类型
        /// </summary>
        private static readonly Type serverType;
        /// <summary>
        /// TCP 服务构造函数
        /// </summary>
        private static readonly ConstructorInfo serverConstructorInfo;
        /// <summary>
        /// 服务端输出信息集合
        /// </summary>
        [AutoCSer.IOS.Preserve]
        internal static readonly TcpServer.OutputInfo[] Outputs;
        /// <summary>
        /// 创建 TCP 服务端对象
        /// </summary>
        /// <param name="value">接口对象</param>
        /// <param name="attribute">TCP 调用服务器端配置信息</param>
        /// <param name="verify">套接字验证委托</param>
        /// <param name="log">日志接口</param>
        /// <returns>TCP 服务</returns>
        public static TcpOpenStreamServer.Server Create(interfaceType value, TcpOpenStreamServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
        {
            if (serverConstructorInfo == null) throw new InvalidOperationException();
            if (errorString != null) throw new Exception(errorString);
            if (value == null) throw new ArgumentNullException();
            TcpOpenStreamServer.Server server = (TcpOpenStreamServer.Server)serverConstructorInfo.Invoke(new object[] { attribute = attribute ?? defaultServerAttribute, verify, value, log });
            server.setCommandData(methods.Length);
            foreach (Method<ServerAttribute, MethodAttribute, ServerSocketSender> method in methods)
            {
                if (method != null)
                {
                    if (method.Attribute.IsVerifyMethod) server.setVerifyCommand(method.Attribute.CommandIdentity);
                    else server.setCommand(method.Attribute.CommandIdentity);
                }
            }
            TcpServer.ISetTcpServer<TcpOpenStreamServer.Server, ServerAttribute> setTcpServer = value as TcpServer.ISetTcpServer<TcpOpenStreamServer.Server, ServerAttribute>;
            if (setTcpServer != null) setTcpServer.SetTcpServer(server);
            if (attribute.IsAutoServer) server.Start();
            return server;
        }

        static Server()
        {
            Type type = typeof(interfaceType);
            MethodGetter builder = new MethodGetter { IsClient = false };
            bool isMethod = builder.Get(type);
            errorString = builder.ErrorString;
            if (isMethod)
            {
                defaultServerAttribute = builder.DefaultServerAttribute;
                methods = builder.Methods;

                Type[] constructorParameterTypes = new Type[] { typeof(ServerAttribute), typeof(Func<System.Net.Sockets.Socket, bool>), type, typeof(AutoCSer.Log.ILog) };
                Method<ServerAttribute, MethodAttribute, ServerSocketSender>.ServerBuilder serverBuilder = new Method<ServerAttribute, MethodAttribute, ServerSocketSender>.ServerBuilder { Metadata = Server.Metadata };
                serverType = serverBuilder.Build(type, defaultServerAttribute, typeof(Server<interfaceType>), typeof(ServerCall<>), constructorParameterTypes, methods);
                Outputs = serverBuilder.Outputs;
                serverConstructorInfo = serverType.GetConstructor(constructorParameterTypes);

            }
        }
    }
}
