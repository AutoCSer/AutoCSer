using System;
using AutoCSer.Threading;
using System.Reflection;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpInternalSimpleServer.Emit
{
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ParameterGenericType : AutoCSer.Net.TcpSimpleServer.Emit.ParameterGenericType
    {
        /// <summary>
        /// TCP 内部服务端套接字 错误日志处理函数
        /// </summary>
        internal static MethodInfo ServerSocketLogMethod { get { return ((Action<ServerSocket, Exception>)ServerSocket.Log).Method; } }
        /// <summary>
        /// TCP 内部服务端套接字 发送数据函数
        /// </summary>
        internal static MethodInfo ServerSocketSendOutputMethod { get { return ((Func<ServerSocket, TcpServer.ReturnType, bool>)ServerSocket.SendOutput).Method; } }
        /// <summary>
        /// TCP 内部服务端套接字 发送数据函数
        /// </summary>
        internal static MethodInfo ServerSocketSendReturnTypeMethod { get { return ((Func<ServerSocket, TcpServer.ReturnType, bool>)ServerSocket.Send).Method; } }
        /// <summary>
        /// TCP 内部服务端套接字 发送数据函数
        /// </summary>
        internal static MethodInfo ServerSocketSendMethod { get { return ((Func<ServerSocket, bool>)ServerSocket.Send).Method; } }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, ParameterGenericType> cache = new LockLastDictionary<HashType, ParameterGenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static ParameterGenericType create<parameterType>()
        where parameterType : struct
        {
            return new ParameterGenericType<parameterType>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(ParameterGenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="outputParameterType"></param>
        /// <returns></returns>
        public static ParameterGenericType Get(Type outputParameterType)
        {
            ParameterGenericType value;
            if (!cache.TryGetValue(outputParameterType, out value))
            {
                try
                {
                    value = new UnionType.ParameterGenericType { Object = createMethod.MakeGenericMethod(outputParameterType).Invoke(null, null) }.Value;
                    cache.Set(outputParameterType, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="parameterType">泛型类型</typeparam>
    internal sealed partial class ParameterGenericType<parameterType> : ParameterGenericType
        where parameterType : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(parameterType); } }

        /// <summary>
        /// TCP调用
        /// </summary>
        internal delegate TcpServer.ReturnType Call(TcpSimpleServer.Client client, TcpServer.CommandInfoBase commandInfo, ref parameterType inputParameter);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientCallMethod
        {
            get { return ((Call)TcpSimpleServer.Client.Call<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal override MethodInfo ClientGetMethod
        {
            get { return ((Call)TcpSimpleServer.Client.Get<parameterType>).Method; }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal delegate bool Send(ServerSocket socket, TcpSimpleServer.OutputInfo outputInfo, ref parameterType outputParameter);
        /// <summary>
        /// 发送数据
        /// </summary>
        internal override MethodInfo ServerSocketSendParameterMethod
        {
            get { return ((Send)ServerSocket.Send<parameterType>).Method; }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="outputInfo"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal delegate bool SendAsync(TcpSimpleServer.OutputInfo outputInfo, ref TcpServer.ReturnValue<parameterType> outputParameter);
        /// <summary>
        /// 反序列化
        /// </summary>
        internal override MethodInfo ServerSocketDeSerializeMethod { get { return ((DeSerialize)TcpSimpleServer.ServerSocket.DeSerialize<parameterType>).Method; } }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns></returns>
        internal delegate bool DeSerialize(TcpSimpleServer.ServerSocket socket, ref SubArray<byte> data, ref parameterType value, bool isSimpleSerialize);
    }
}
