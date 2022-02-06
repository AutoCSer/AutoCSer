using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ServerUploadFile
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 上传文件
        /// </summary>
        [FieldOffset(0)]
        public FileSynchronous.ServerUploadFile Value;
    }
}
