using System;

namespace AutoCSer.Net.TcpOpenStreamServer.Emit
{
    /// <summary>
    /// TCP 客户端元数据
    /// </summary>
    internal sealed class ClientMetadata : AutoCSer.Net.TcpServer.Emit.ClientMetadataBase
    {
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        private ClientMetadata()
            : base(typeof(TcpOpenStreamServer.Client), typeof(ClientSocketSender), typeof(MethodClient)
                , ((Func<ClientSocketSender>)ParameterGenericType.Client.GetSender).Method
                , ParameterGenericType.Get, ParameterGenericType2.Get
                , ((AutoCSer.Net.TcpServer.Emit.ParameterGenericType.WaitCall)ParameterGenericType.ClientSocketSender.WaitCall).Method
                , ((Action<AutoCSer.Net.TcpServer.CommandInfo>)ParameterGenericType.ClientSocketSender.CallOnly).Method)
        {
        }
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        internal static readonly ClientMetadata Default = new ClientMetadata();
    }
}
