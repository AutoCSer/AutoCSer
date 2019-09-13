using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 时间序列化输出类型
    /// </summary>
    public enum DateTimeType : byte
    {
        /// <summary>
        /// 默认格式 yyyy/MM/dd HH:mm:ss.fff
        /// </summary>
        Default,
        /// <summary>
        /// yyyy-MM-ddTHH:mm:ss...
        /// </summary>
        ISO,
        /// <summary>
        /// 第三方格式 /Date(xxx)/
        /// </summary>
        ThirdParty,
        /// <summary>
        /// JS格式 new Date(xxx)
        /// </summary>
        Javascript,
        /// <summary>
        /// 自定义 ToString("xxx") 格式
        /// </summary>
        CustomFormat,
    }
}