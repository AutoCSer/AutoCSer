using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 查询模式类别
    /// </summary>
    internal enum MethodType : byte
    {
        /// <summary>
        /// 未知查询模式类别
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 请求获取Request-URI所标识的资源
        /// </summary>
        GET,
        /// <summary>
        /// 在Request-URI所标识的资源后附加新的数据
        /// </summary>
        POST,
        /// <summary>
        /// 请求获取由Request-URI所标识的资源的响应消息报头
        /// </summary>
        HEAD,
        /// <summary>
        /// 请求服务器存储一个资源，并用Request-URI作为其标识
        /// </summary>
        PUT,
        /// <summary>
        /// 请求服务器删除Request-URI所标识的资源
        /// </summary>
        DELETE,
        /// <summary>
        /// 请求服务器回送收到的请求信息，主要用于测试或诊断
        /// </summary>
        TRACE,
        /// <summary>
        /// 保留将来使用
        /// </summary>
        CONNECT,
        /// <summary>
        /// 请求查询服务器的性能，或者查询与资源相关的选项和需求
        /// </summary>
        OPTIONS
    }
}
