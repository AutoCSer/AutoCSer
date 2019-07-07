using System;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 下载数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct DownloadData
    {
        /// <summary>
        /// 文件同步状态
        /// </summary>
        internal SynchronousState State;
        /// <summary>
        /// 文件数据
        /// </summary>
        internal SubArray<byte> Data;

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="state">文件同步状态</param>
        internal DownloadData(SynchronousState state)
        {
            State = state;
            Data = default(SubArray<byte>);
        }
        /// <summary>
        /// 设置下载数据
        /// </summary>
        /// <param name="data">下载数据</param>
        /// <param name="size"></param>
        internal void Set(byte[] data, int size)
        {
            State = SynchronousState.Success;
            Data.Set(data, 0, size);
        }
    }
}
