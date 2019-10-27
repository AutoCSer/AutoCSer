using System;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 异步回调类型
    /// </summary>
    internal enum CallbackType : byte
    {
        /// <summary>
        /// 同步模式
        /// </summary>
        None,
        /// <summary>
        /// 回调抽象类
        /// </summary>
        ServerCallback,
        /// <summary>
        /// 回调委托
        /// </summary>
        Func,
        /// <summary>
        /// 无返回值回调委托
        /// </summary>
        Action,
    }
}
