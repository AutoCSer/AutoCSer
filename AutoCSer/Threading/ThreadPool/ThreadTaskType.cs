using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 调用类型
    /// </summary>
    internal enum ThreadTaskType : byte
    {
        /// <summary>
        /// 没有线程调用
        /// </summary>
        None,
        /// <summary>
        /// 委托
        /// </summary>
        Action,
        /// <summary>
        /// 二维秒级定时任务 触发定时操作
        /// </summary>
        SecondTimerTaskNodeOnTimer,
        /// <summary>
        /// 二维秒级定时任务 执行之后添加新的定时任务
        /// </summary>
        SecondTimerTaskNodeAfter,
        /// <summary>
        /// 配置缓存加载失败异常处理
        /// </summary>
        ConfigLoadException,
        /// <summary>
        /// 公共全局配置格式化异常处理
        /// </summary>
        ConfigFormatException,
        /// <summary>
        /// 二进制序列化预编译
        /// </summary>
        CompileBinarySerialize,
        /// <summary>
        /// 二进制反序列化预编译
        /// </summary>
        CompileBinaryDeSerialize,
        /// <summary>
        /// JSON 序列化预编译
        /// </summary>
        CompileJsonSerialize,
        /// <summary>
        /// JSON 反序列化预编译
        /// </summary>
        CompileJsonDeSerialize,
        /// <summary>
        /// 简单序列化预编译
        /// </summary>
        CompileSimpleSerialize,
        /// <summary>
        /// 简单反序列化预编译
        /// </summary>
        CompileSimpleDeSerialize,
        /// <summary>
        /// 删除应用程序卸载处理
        /// </summary>
        DomainUnloadRemoveLast,
        /// <summary>
        /// 删除应用程序卸载处理
        /// </summary>
        DomainUnloadRemoveLastRun,
        ///// <summary>
        ///// 关闭文件流写入器
        ///// </summary>
        //FileStreamWriterDispose,
        ///// <summary>
        ///// 文件流写入器写入文件数据
        ///// </summary>
        //FileStreamWriteFile,
        /// <summary>
        /// 运行超时切换任务线程
        /// </summary>
        TaskSwitchThreadRun,
        /// <summary>
        /// 创建 TCP 服务客户端套接字
        /// </summary>
        TcpClientSocketBaseCreate,
        /// <summary>
        /// TCP 客户端命令池计数超时事件
        /// </summary>
        TcpClientCommandPoolTimeout,
        /// <summary>
        /// TCP 内部服务客户端套接字创建数据发送
        /// </summary>
        TcpInternalClientSocketSenderBuildOutput,
        /// <summary>
        /// TCP 内部服务获取客户端请求
        /// </summary>
        TcpInternalServerGetSocket,
        /// <summary>
        /// TCP 内部服务套接字创建数据发送
        /// </summary>
        TcpInternalServerSocketSenderBuildOutput,
        /// <summary>
        /// TCP 服务客户端套接字创建数据发送
        /// </summary>
        TcpOpenClientSocketSenderBuildOutput,
        /// <summary>
        /// TCP 服务获取客户端请求
        /// </summary>
        TcpOpenServerGetSocket,
        /// <summary>
        /// TCP 服务套接字处理
        /// </summary>
        TcpOpenServerOnSocket,
        /// <summary>
        /// TCP 服务套接字创建数据发送
        /// </summary>
        TcpOpenServerSocketSenderBuildOutput,
    }
}
