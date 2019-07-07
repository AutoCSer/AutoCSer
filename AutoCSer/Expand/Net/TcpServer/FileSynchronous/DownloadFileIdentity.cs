using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 服务端返回下载文件信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class DownloadFileIdentity
    {
        /// <summary>
        /// 时钟周期标识
        /// </summary>
        internal long Tick;
        /// <summary>
        /// 文件编号
        /// </summary>
        internal long Identity;
        /// <summary>
        /// 列表文件数据
        /// </summary>
        internal ListFileItem ListFileItem;
        /// <summary>
        /// 文件读取位置
        /// </summary>
        internal long Index;
        /// <summary>
        /// 文件数据
        /// </summary>
        internal byte[] Data;
        /// <summary>
        /// 服务端返回下载文件信息
        /// </summary>
        public DownloadFileIdentity() { }
        /// <summary>
        /// 服务端返回下载文件信息
        /// </summary>
        /// <param name="identity">文件编号</param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="data">文件信息</param>
        /// <param name="index">文件读取位置</param>
        /// <param name="fileLength">文件长度</param>
        internal DownloadFileIdentity(long identity, FileInfo fileInfo, byte[] data, long index, long fileLength)
        {
            Tick = AutoCSer.Pub.StartTime.Ticks;
            Identity = identity;
            ListFileItem.Set(null, fileInfo.LastWriteTimeUtc, fileLength);
            Index = index;
            Data = data;
        }
        /// <summary>
        /// 服务端返回下载文件信息
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="data">文件信息</param>
        /// <param name="index">文件读取位置</param>
        /// <param name="fileLength">文件长度</param>
        internal DownloadFileIdentity(FileInfo fileInfo, byte[] data, long index, long fileLength)
        {
            Identity = (byte)SynchronousState.Success;
            ListFileItem.Set(null, fileInfo.LastWriteTimeUtc, fileLength);
            Index = index;
            Data = data;
        }
        /// <summary>
        /// 服务端返回下载文件信息
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="data">文件信息</param>
        internal DownloadFileIdentity(FileInfo fileInfo, byte[] data)
        {
            Identity = (byte)SynchronousState.Success;
            ListFileItem.Set(null, fileInfo.LastWriteTimeUtc, fileInfo.Length);
            Data = data;
        }
        /// <summary>
        /// 服务端返回下载文件信息
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        internal DownloadFileIdentity(FileInfo fileInfo)
        {
            Identity = (byte)SynchronousState.Success;
            ListFileItem.Set(null, fileInfo.LastWriteTimeUtc, Index = fileInfo.Length);
            //Data = NullValue<byte>.Array;
        }
        /// <summary>
        /// 服务端返回下载文件信息
        /// </summary>
        /// <param name="state"></param>
        internal DownloadFileIdentity(SynchronousState state)
        {
            Identity = (byte)state;
        }
        /// <summary>
        /// 获取错误文件同步状态
        /// </summary>
        /// <returns>错误文件同步状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SynchronousState GetErrorState()
        {
            return (SynchronousState)(byte)Identity;
        }
    }
}
