using System;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 文件同步状态
    /// </summary>
    public enum SynchronousState : byte
    {
        /// <summary>
        /// 未知异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 异步执行中
        /// </summary>
        Asynchronous,
        /// <summary>
        /// 网络调用失败
        /// </summary>
        TcpError,
        /// <summary>
        /// 服务端异常
        /// </summary>
        ServerException,
        /// <summary>
        /// 文件被占用，正在上传中
        /// </summary>
        Uploading,
        /// <summary>
        /// 文件被占用，正在下载中
        /// </summary>
        Downloading,
        /// <summary>
        /// 文件不存在
        /// </summary>
        NotExists,
        /// <summary>
        /// 文件读取错误
        /// </summary>
        ReadError,
        /// <summary>
        /// 文件编号错误或者过期
        /// </summary>
        IdentityError,
    }
}
