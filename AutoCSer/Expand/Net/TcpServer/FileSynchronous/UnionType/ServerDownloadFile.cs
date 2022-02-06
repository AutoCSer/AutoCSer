using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ServerDownloadFile
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 上传下载
        /// </summary>
        [FieldOffset(0)]
        public FileSynchronous.ServerDownloadFile Value;
    }
}
