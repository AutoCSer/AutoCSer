using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块处理接口
    /// </summary>
    public interface IBlock
    {
        /// <summary>
        /// 磁盘块编号
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="size">字节数量</param>
        /// <param name="onRead">获取数据回调委托</param>
        void Read(long index, int size, Func<AutoCSer.Net.TcpServer.ReturnValue<ClientBuffer>, bool> onRead);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="onWrite">添加数据回调委托</param>
        void Append(ref AppendBuffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>, bool> onWrite);
    }
}
