using System;

namespace AutoCSer.Net.TcpInternalServer.Emit
{
    /// <summary>
    /// TCP 客户端元数据
    /// </summary>
    internal sealed class ClientMetadata : AutoCSer.Net.TcpServer.Emit.ClientMetadata
    {
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        private ClientMetadata()
            : base(typeof(TcpInternalServer.Client), typeof(ClientSocketSender), typeof(MethodClient)
                , ((Func<ClientSocketSender>)ParameterGenericType.Client.GetSender).Method, ReturnParameterGenericType.Get
                , ParameterGenericType.Get, ParameterGenericType2.Get
                , ((AutoCSer.Net.TcpServer.Emit.ParameterGenericType.WaitCall)ParameterGenericType.ClientSocketSender.WaitCall).Method
                , ((Action<AutoCSer.Net.TcpServer.CommandInfo>)ParameterGenericType.ClientSocketSender.CallOnly).Method
                , ((Action<AutoCSer.Net.TcpServer.CommandInfo, Action<AutoCSer.Net.TcpServer.ReturnValue>>)ParameterGenericType.ClientSocketSender.Call).Method
                , ((Func<AutoCSer.Net.TcpServer.CommandInfo, Action<AutoCSer.Net.TcpServer.ReturnValue>, AutoCSer.Net.TcpServer.KeepCallback>)ParameterGenericType.ClientSocketSender.CallKeep).Method
#if !DOTNET2 && !DOTNET4 && !UNITY3D
                , ((Func<AutoCSer.Net.TcpServer.CommandInfo, AutoCSer.Net.TcpServer.Awaiter, AutoCSer.Net.TcpServer.ReturnType>)ParameterGenericType.ClientSocketSender.GetAwaiter).Method
#endif
                  )
        {
        }
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        internal static readonly ClientMetadata Default = new ClientMetadata();
    }
}
