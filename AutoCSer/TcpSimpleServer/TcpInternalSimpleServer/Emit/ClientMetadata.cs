using System;

namespace AutoCSer.Net.TcpInternalSimpleServer.Emit
{
    /// <summary>
    /// TCP 客户端元数据
    /// </summary>
    internal sealed class ClientMetadata : AutoCSer.Net.TcpSimpleServer.Emit.ClientMetadata
    {
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        private ClientMetadata()
            : base(typeof(TcpInternalSimpleServer.Client), typeof(MethodClient)
            , ParameterGenericType.Get, ParameterGenericType2.Get
            , ((Func<TcpServer.CommandInfoBase, TcpServer.ReturnType>)ParameterGenericType.Client.Call).Method)
        {
        }
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        internal static readonly ClientMetadata Default = new ClientMetadata();
    }
}
