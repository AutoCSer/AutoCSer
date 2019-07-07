using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 服务端返回上传文件信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct UploadFileIdentity
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
        /// 服务端返回文件信息
        /// </summary>
        /// <param name="identity"></param>
        internal UploadFileIdentity(long identity)
        {
            Tick = AutoCSer.Pub.StartTime.Ticks;
            Identity = identity;
        }
        /// <summary>
        /// 服务端返回文件信息
        /// </summary>
        /// <param name="state"></param>
        internal UploadFileIdentity(SynchronousState state)
        {
            Tick = 0;
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
