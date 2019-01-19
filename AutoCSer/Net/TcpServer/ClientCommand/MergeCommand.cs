using System;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 合并处理命令
    /// </summary>
    internal sealed class MergeCommand : Command
    {
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        internal override void OnReceive(ref SubArray<byte> data)
        {
            Socket.Merge(ref data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns></returns>
        internal override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            throw new InvalidOperationException();
        }
    }
}
