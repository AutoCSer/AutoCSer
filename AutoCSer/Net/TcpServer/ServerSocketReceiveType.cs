using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务端套接字接收数据回调类型
    /// </summary>
    internal enum ServerSocketReceiveType : byte
    {
        None,
        ///// <summary>
        /////  Hello 应答
        ///// </summary>
        //Hello,
        /// <summary>
        /// 获取验证命令
        /// </summary>
        VerifyCommand,
        /// <summary>
        /// 获取验证数据
        /// </summary>
        VerifyData,
        ///// <summary>
        ///// 获取验证临时数据
        ///// </summary>
        //VerifyBigData,
        /// <summary>
        /// 获取命令
        /// </summary>
        Command,
        /// <summary>
        /// 获取数据
        /// </summary>
        Data,
        /// <summary>
        /// 获取临时数据
        /// </summary>
        BigData,
        /// <summary>
        /// 获取压缩数据
        /// </summary>
        CompressionData,
        /// <summary>
        /// 获取临时压缩数据
        /// </summary>
        CompressionBigData,
    }
}
