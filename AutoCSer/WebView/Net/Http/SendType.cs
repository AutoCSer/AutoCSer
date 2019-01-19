using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 发送数据下一步操作类型
    /// </summary>
    internal enum SendType : byte
    {
        /// <summary>
        /// 处理下一个请求
        /// </summary>
        Next,
        /// <summary>
        /// 输出内容
        /// </summary>
        Body,
        /// <summary>
        /// 输出文件
        /// </summary>
        File,
        /// <summary>
        /// 获取请求表单数据
        /// </summary>
        GetForm,
        /// <summary>
        /// 关闭套接字
        /// </summary>
        Close,
    }
}
