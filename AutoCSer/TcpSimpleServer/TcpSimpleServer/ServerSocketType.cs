using System;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 服务端套接字回调类型
    /// </summary>
    internal enum ServerSocketType : byte
    {
        None,
        /// <summary>
        /// 获取验证命令
        /// </summary>
        VerifyCommand,
        /// <summary>
        /// 获取验证数据
        /// </summary>
        VerifyData,
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
        /// 发送数据
        /// </summary>
        Send,
        /// <summary>
        /// 发送数据
        /// </summary>
        SendInt,
        /// <summary>
        /// 发送数据
        /// </summary>
        SendInt2,
        /// <summary>
        /// 发送验证数据
        /// </summary>
        SendVerify,
    }
}
