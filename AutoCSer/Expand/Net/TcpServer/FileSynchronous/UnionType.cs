using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 上传文件
        /// </summary>
        [FieldOffset(0)]
        public ServerUploadFile ServerUploadFile;
        /// <summary>
        /// 上传下载
        /// </summary>
        [FieldOffset(0)]
        public ServerDownloadFile ServerDownloadFile;
    }
}
