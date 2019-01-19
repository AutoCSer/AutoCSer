using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net.Http.ServerNameIndication
{
    /// <summary>
    /// SSL 客户端 Hello 流，用于获取请求的 domain
    /// </summary>
    internal sealed partial class HelloStream
    {
        /// <summary>
        /// 刷新流中的数据作为异步操作
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return Socket.NetworkStream.FlushAsync(cancellationToken);
        }
    }
}
