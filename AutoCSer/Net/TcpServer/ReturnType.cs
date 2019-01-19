using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 返回值类型
    /// </summary>
    public enum ReturnType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 版本过期
        /// </summary>
        VersionExpired = 1,
        /// <summary>
        /// 服务器端反序列化错误
        /// </summary>
        ServerDeSerializeError = 2,
        /// <summary>
        /// 服务器端异常
        /// </summary>
        ServerException = 3,
        /// <summary>
        /// 取消保持回调
        /// </summary>
        CancelKeep = 4,
        /// <summary>
        /// 远程表达式映射失败
        /// </summary>
        RemoteExpressionServerNodeError = 5,
        /// <summary>
        /// 成功，最大占用 3b，低端是服务端状态，用于 Identity 最高 3 位
        /// </summary>
        Success = 0x7,
        ///// <summary>
        ///// 终止保持回调
        ///// </summary>
        //CancelKeep,
        /// <summary>
        /// 客户端已关闭
        /// </summary>
        ClientDisposed,
        /// <summary>
        /// 客户端没有接收到数据
        /// </summary>
        ClientNullData,
        /// <summary>
        /// 客户端反序列化错误
        /// </summary>
        ClientDeSerializeError,
        /// <summary>
        /// 客户端异常
        /// </summary>
        ClientException,
        /// <summary>
        /// 创建输出错误取消命令调用
        /// </summary>
        ClientBuildError,
        /// <summary>
        /// 客户端发送数据错误
        /// </summary>
        ClientSendError,
        /// <summary>
        /// 客户端接收数据错误
        /// </summary>
        ClientReceiveError,
        /// <summary>
        /// 服务配置不支持远程表达式
        /// </summary>
        RemoteExpressionNotSupport,
        /// <summary>
        /// 远程表达式客户端检测服务端映射标识不匹配
        /// </summary>
        RemoteExpressionCheckerError,
        ///// <summary>
        ///// 日志流过期
        ///// </summary>
        //LogStreamExpired,
    }
}
