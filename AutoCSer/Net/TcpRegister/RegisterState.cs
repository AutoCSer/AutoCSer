using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 注册状态
    /// </summary>
    public enum RegisterState : byte
    {
        /// <summary>
        /// 客户端不可用
        /// </summary>
        NoClient,
        /// <summary>
        /// 客户端标识错误
        /// </summary>
        ClientError,
        ///// <summary>
        ///// 单例服务冲突
        ///// </summary>
        //SingleError,
        ///// <summary>
        ///// TCP 服务信息检测被更新,需要重试
        ///// </summary>
        //ServiceChange,
        /// <summary>
        /// 注册成功
        /// </summary>
        Success,
    }
}
