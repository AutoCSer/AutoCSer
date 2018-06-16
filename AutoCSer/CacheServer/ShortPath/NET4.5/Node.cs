using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 短路径节点
    /// </summary>
    public abstract partial class Node<nodeType>
    {
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        internal async Task<ReturnValue<nodeType>> CreateTask()
        {
            if (node != null)
            {
                do
                {
                    socketIdentity = Client.SocketIdentity;
                    AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await Client.QueryAwaiter(Parameter);
                    if (socketIdentity == Client.SocketIdentity)
                    {
                        if (isCreate(ref value)) return (nodeType)this;
                        return new ReturnValue<nodeType> { Type = Identity.Type, TcpReturnType = Identity.TcpReturnType };
                    }
                }
                while (true);
            }
            return new ReturnValue<nodeType> { Type = ReturnType.CanNotCreateShortPath };
        }
    }
}
